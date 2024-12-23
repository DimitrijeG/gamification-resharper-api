using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.PersistentMap;
using JetBrains.Application.Threading.Tasks;
using JetBrains.Diagnostics;
using JetBrains.Lifetimes;
using JetBrains.UsageStatistics;
using JetBrains.Util;
using JetBrains.Util.PersistentMap;
using JetBrains.Util.Special;

namespace ReSharperPlugin.ReSharperGamificationPlugin.Achievements.Storage;

public interface IAchievementDb
{
  public bool IsOperational { get; }
  public IKeyValueDb GetDb();
  public bool Contains(string group, string goal);
  public Guid Create(string group, string goal);
  public void Clear();
}

public class AchievementDb : IAchievementDb
{
  private readonly IKeyValueDb myDb;
  private readonly IPersistentSortedMap<Guid, string> myGoalsMap;
  private readonly IPersistentSortedMap<Guid, string> myGroupsMap;
  private readonly IPersistentSortedMap<int, JetHashSet<Guid>> myIdIndex;

  public AchievementDb(Lifetime lifetime, ILogger logger, ITaskHost taskHost, FileSystemPath path)
  {
    IsOperational = false;

    var seqLifetime = new SequentialLifetimes(lifetime);
    try
    {
      myDb = LevelDbDriver.Instance.OpenDb(
        seqLifetime.Next(),
        DbOpenParams.Create(path, taskHost).WithDbOpenError(DbErrorDuringCreationAction.Null)
      ).IfNotNull(result => result.Db);

      if (myDb == null) return;

      var stringMarshaller = UnsafeMarshallers.UnicodeStringMarshaller;
      var guidMarshaller = UnsafeMarshallers.GuidMarshaller;
      var guidSetMarshaller = UnsafeMarshallers.GetCollectionMarshaller(guidMarshaller, JetHashSet<Guid>.NewInstance);
      myGroupsMap = GetMap(StorageCollections.Groups, guidMarshaller, stringMarshaller);
      myGoalsMap = GetMap(StorageCollections.Goals, guidMarshaller, stringMarshaller);
      myIdIndex = GetMap(StorageCollections.IdxGroupAndGoalToId, UnsafeMarshallers.IntMarshaller, guidSetMarshaller);
    }
    catch (Exception e)
    {
      myDb = null;
      logger.LogExceptionSilently(e);
      logger.CatchSilent(path.Delete);
      seqLifetime.TerminateCurrent();
    }

    IsOperational = true;
  }

  public bool IsOperational { get; }

  public IKeyValueDb GetDb()
  {
    return myDb;
  }

  public bool Contains(string group, string goal)
  {
    return GetId(group, goal) != default;
  }

  public Guid Create(string group, string goal)
  {
    Assertion.Assert(IsOperational);
    group = FileUploadUtil.EnsureProperKey(group);
    goal = FileUploadUtil.EnsureProperKey(goal.ToString());
    var guid = Guid.NewGuid();

    myGroupsMap[guid] = group;
    myGoalsMap[guid] = goal;

    var hash = CalculateHash(group, goal);
    if (!myIdIndex.TryGetValue(hash, out var hashGuids))
      hashGuids = [];
    else
      Assertion.Assert(!hashGuids.Contains(guid));

    hashGuids.Add(guid);
    myIdIndex[hash] = hashGuids;

    return guid;
  }

  public void Clear()
  {
    Assertion.Assert(IsOperational);
    myGroupsMap.Clear();
    myGoalsMap.Clear();
    myIdIndex.Clear();
  }

  private Guid GetId(string group, string goal)
  {
    Assertion.Assert(IsOperational);
    group = FileUploadUtil.EnsureProperKey(group);
    goal = FileUploadUtil.EnsureProperKey(goal);

    if (!myIdIndex.TryGetValue(CalculateHash(group, goal), out var hashGuids))
      return default;

    return hashGuids.FirstOrDefault(guid =>
      group.Equals(GetGroup(guid)) && goal.Equals(GetGoal(guid)));
  }

  private static int CalculateHash(string group, string goal)
  {
    return (group + goal).GetPlatformIndependentHashCode();
  }

  private string GetGroup(Guid guid)
  {
    Assertion.Assert(IsOperational);
    return myGroupsMap.TryGetValue(guid, out var result) ? result : null;
  }

  private string GetGoal(Guid guid)
  {
    Assertion.Assert(IsOperational);
    return myGoalsMap.TryGetValue(guid, out var result) ? result : null;
  }

  private IPersistentSortedMap<TK, TV> GetMap<TK, TV>(
    string mapId, IUnsafeMarshaller<TK> keyMarshaller, IUnsafeMarshaller<TV> valMarshaller)
  {
    Assertion.AssertNotNull(myDb);
    return myDb.GetMap(mapId, keyMarshaller, valMarshaller);
  }

  private static class StorageCollections
  {
    public const string Groups = "AchievementGroups";
    public const string Goals = "AchievementGoals";
    public const string IdxGroupAndGoalToId = "IdxAchievementGroupAndGoalToId";
  }
}