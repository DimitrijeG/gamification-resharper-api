using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Application;
using JetBrains.Application.License;
using JetBrains.Application.License2;
using JetBrains.Application.License2.JBAccount;
using JetBrains.Lifetimes;

namespace ReSharperPlugin.ReSharperGamificationPlugin;

public interface IJbaProvider
{
  public Task<string> GetAuthToken(Lifetime lifetime);
  public Task<string> GetAccessToken(Lifetime lifetime);
}

[ShellComponent]
public class JbaProvider(License2CheckComponent license2CheckComponent) : IJbaProvider
{
  public Task<string> GetAuthToken(Lifetime lifetime)
  {
    return GetToken(lifetime, model => model.AccountInfo.IdToken);
  }

  public Task<string> GetAccessToken(Lifetime lifetime)
  {
    return GetToken(lifetime, model =>
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(model.AccountInfo.UserId));
        return Convert.ToBase64String(hash).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    });
  }

  private Task<string> GetToken(Lifetime lifetime, Func<JBAcountLicenseViewSubmodel, string> extractor)
  {
    var tcs = lifetime.CreateTaskCompletionSource<string>();
    license2CheckComponent.WithLicenseViewModel(lifetime, model =>
    {
      var jbaModel = model.Submodels.OfType<JBAcountLicenseViewSubmodel>().SingleOrDefault();
      if (jbaModel != default)
      {
        tcs.SetResult(extractor(jbaModel));
      } else throw new LicenseCheckFailureException("Could not obtain account.");
    });
    return tcs.Task;
  }
}