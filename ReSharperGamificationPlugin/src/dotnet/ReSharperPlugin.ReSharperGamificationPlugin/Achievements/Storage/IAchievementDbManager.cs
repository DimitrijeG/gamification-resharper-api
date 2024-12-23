using System;
using JetBrains.Application;
using JetBrains.Application.BuildScript;
using JetBrains.Application.Environment.Components;
using JetBrains.Application.PerformanceTracking;
using JetBrains.Application.Threading;
using JetBrains.Application.Threading.Tasks;
using JetBrains.Lifetimes;
using JetBrains.Util;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Storage;

public interface IAchievementDbManager
{
  public void WithDb(Action<IAchievementDb> action, bool sync = false);
}

[ShellComponent]
public class AchievementDbManager : IAchievementDbManager
{
  private const string DatabaseDirBaseName = "Achievements";
  private readonly SingleThreadExecutor myExecutor;
  private readonly Lifetime myLifetime;
  private readonly ILogger myLogger;
  private readonly ProductSettingsLocation myProductSettingsLocation;
  private readonly IThreading myTaskHost;

  private IAchievementDb myDb;
  private bool myTriedToOpen;

  public AchievementDbManager(
    Lifetime lifetime,
    ILogger logger,
    IThreading taskHost,
    ProductSettingsLocation productSettingsLocation,
    EventRecordsProvider eventRecordsProvider)
  {
    myLifetime = lifetime;
    myLogger = logger;
    myTaskHost = taskHost;
    myProductSettingsLocation = productSettingsLocation;

    // EternalLifetime because all pending DB operations shall be completed
    var definition = Lifetime.Define("AchievementDbManager_SingleThreadExecutor");
    myExecutor = new SingleThreadExecutor(definition.Lifetime, myTaskHost.Tasks, "AchievementDbManager")
    {
      SyncContinuationAction = task =>
      {
        if (task.IsFaulted && task.Exception != null)
          myLogger.Error(task.Exception);
      }
    };
    lifetime.OnTermination(() => myExecutor.Queue(() => definition.Terminate()));

    eventRecordsProvider.AddMemoryEventRecordProvider(lifetime, () =>
    {
      var usageDb = myDb?.GetDb();
      return usageDb == null ? null : new LevelDbMemoryEventRecord("AchievementDb", usageDb.GetStatus());
    });
  }

  public void WithDb(Action<IAchievementDb> action, bool sync = false)
  {
    myExecutor.Queue(() =>
    {
      if (myDb == null && !myTriedToOpen)
        OpenDb();
      if (myDb is { IsOperational: true })
        action(myDb);
    }, sync: sync);
  }

  private void OpenDb()
  {
    var dbFolder = myProductSettingsLocation.GetUserSettingsNonRoamingDir(ApplicationHostDetails.PerHost);
    var path = dbFolder.Combine(DatabaseDirBaseName);

    myTriedToOpen = true;
    var db = new AchievementDb(myLifetime, myLogger, myTaskHost.Tasks, path);
    if (db.IsOperational)
    {
      myDb = db;
      return;
    }

    // Primary DB is not accessible. Generate name for secondary and open it
    var secondaryPath = CreateSecondaryPath(dbFolder);
    if (secondaryPath.IsNullOrEmpty())
      return;
    db = new AchievementDb(myLifetime, myLogger, myTaskHost.Tasks, secondaryPath);
    if (db.IsOperational)
      myDb = db;
  }

  private FileSystemPath CreateSecondaryPath(FileSystemPath basePath)
  {
    for (var i = 0; i < 100; i++)
    {
      var candidate = basePath.Combine(DatabaseDirBaseName + "." + i);
      if (candidate.Exists != FileSystemPath.Existence.Missing) continue;
      try
      {
        candidate.CreateDirectory();
        return candidate;
      }
      catch (Exception e)
      {
        myLogger.LogExceptionSilently(e);
      }
    }

    return null;
  }
}