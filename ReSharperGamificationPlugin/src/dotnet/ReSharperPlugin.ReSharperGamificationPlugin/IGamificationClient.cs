using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using JetBrains.Application;
using JetBrains.Application.Communication;
using Newtonsoft.Json;

namespace ReSharperPlugin.ReSharperGamificationPlugin;

public interface IGamificationClient
{
  public Task<TResponse> SendAndReadResponse<TRequest, TResponse>(
    HttpMethod method, string uri, TRequest request, CancellationToken cancellationToken, string authToken);
}

[ShellComponent]
public class GamificationClient(
  WebProxySettingsReader proxySettings,
  IGamificationServiceUrlProvider serviceUrlProvider) : IGamificationClient
{
  private const string JSON_MEDIA_TYPE = "application/json";

  private HttpClient Client { get; } = new(new HttpClientHandler { Proxy = proxySettings.GetWebProxy() })
  {
    Timeout = TimeSpan.FromSeconds(50),
    BaseAddress = new Uri(serviceUrlProvider.ServiceUrl)
  };

  public async Task<TResponse> SendAndReadResponse<TRequest, TResponse>(
    HttpMethod httpMethod, string uri, TRequest request, CancellationToken cancellationToken,
    string authToken)
  {
    var payload = JsonConvert.SerializeObject(request);
    var requestContent = new StringContent(payload, Encoding.UTF8, JSON_MEDIA_TYPE);
    requestContent.Headers.ContentLength = Encoding.UTF8.GetByteCount(payload);

    var httpRequestMessage = new HttpRequestMessage(httpMethod, uri);
    httpRequestMessage.Content = requestContent;
    httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

    try
    {
      using var httpResponseMessage = await Client
        .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseContentRead, cancellationToken);

      httpResponseMessage.EnsureSuccessStatusCode();

      using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
      using var streamReader = new StreamReader(responseStream);
      var content = await streamReader.ReadToEndAsync();
      return JsonConvert.DeserializeObject<TResponse>(content);
    }
    catch (Exception e)
    {
      if (IsTimeout(e))
        throw new GamificationClientConnectionException();

      throw;
    }
  }

  private static bool IsTimeout(Exception e)
  {
    return e is TimeoutException
           || e is TaskCanceledException
           || e is OperationCanceledException
           || e.InnerException is TimeoutException;
  }
}

public class GamificationClientConnectionException() : Exception("Request timed out. Please try again");