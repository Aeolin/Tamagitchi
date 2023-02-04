using Microsoft.Extensions.DependencyInjection;
using OpenAI.Net;
using OpenAI.Net.Models;
using OpenAI.Net.Services.Interfaces;
using OpenAI.Net.Services;
using ReInject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using ReInject;
using System.Text.RegularExpressions;
using GitLabApiClient;
using TamagitchiClient.Database.Models;
using GitLabApiClient.Models.Webhooks.Requests;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;

namespace TamagitchiClient
{
  public static class Extensions
  {
    public static void AddOpenAIServices(this IDependencyContainer container, Action<OpenAIServiceRegistrationOptions> options)
    {
      OpenAIServiceRegistrationOptions openAIServiceRegistrationOptions = new OpenAIServiceRegistrationOptions();
      options(openAIServiceRegistrationOptions);
      OpenAIDefaults.ApiUrl = openAIServiceRegistrationOptions.ApiUrl;
      OpenAIDefaults.TextCompletionModel = openAIServiceRegistrationOptions.Defaults.TextCompletionModel;
      OpenAIDefaults.TextEditModel = openAIServiceRegistrationOptions.Defaults.TextEditModel;
      OpenAIDefaults.EmbeddingsModel = openAIServiceRegistrationOptions.Defaults.EmbeddingsModel;
      container.AddOpenAIServices(openAIServiceRegistrationOptions.ApiKey, openAIServiceRegistrationOptions.OrganizationId, openAIServiceRegistrationOptions.ApiUrl);
    }

    public static void AddOpenAIServices(this IDependencyContainer container, string apiKey, string? organization = null, string apiUrl = "https://api.openai.com/")
    {
      string apiUrl2 = apiUrl;
      string apiKey2 = apiKey;
      string organization2 = organization;
      Action<HttpClient> configureClient = delegate (HttpClient c)
      {
        c.BaseAddress = new Uri(apiUrl2);
        c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey2);
        if (!string.IsNullOrEmpty(organization2))
        {
          c.DefaultRequestHeaders.Add("OpenAI-Organization", organization2 ?? "");
        }
      };

      var client = new HttpClient();
      configureClient(client);
      container.Register<HttpClient>(DependencyStrategy.AtomicInstance, true, client);
      container.Register<IModelsService, ModelsService>(DependencyStrategy.SingleInstance);
      container.Register<ITextCompletionService, TextCompletionService>(DependencyStrategy.SingleInstance);
      container.Register<ITextEditService, TextEditService>(DependencyStrategy.SingleInstance);
      container.Register<IImageService, ImageService>(DependencyStrategy.SingleInstance);
      container.Register<IFilesService, FilesService>(DependencyStrategy.SingleInstance);
      container.Register<IFineTuneService, FineTuneService>(DependencyStrategy.SingleInstance);
      container.Register<IModerationService, ModerationService>(DependencyStrategy.SingleInstance);
      container.Register<IEmbeddingsService, EmbeddingsService>(DependencyStrategy.SingleInstance);
      container.Register<IOpenAIService, OpenAIService>(DependencyStrategy.SingleInstance);
    }

    private static void ConfigureHttpClientBuilder(IHttpClientBuilder clientBuilder, Action<IHttpClientBuilder> action)
    {
      action?.Invoke(clientBuilder);
    }

    private static readonly Regex UppercaseMatcher = new Regex(@"[^_][A-Z]", RegexOptions.Compiled);
    /// <summary>
    /// Converts a PascalCase string to KebabCase
    /// </summary>
    /// <param name="string">The string to be converted</param>
    /// <returns>A PascalCase string in KebabCase</returns>
    /// For example MyFancyString gets converted to my-fancy-string
    public static string ToKebabCase(this string @string)
    {
      return (@string[0] + UppercaseMatcher.Replace(@string.Substring(1), (match) =>
      {
        return $"{match.Value[0]}-{match.Value.ToLower()[1]}";
      })).ToLower();
    }

    private static readonly Regex KebabMatcher = new Regex(@"_[a-z]", RegexOptions.Compiled);

    /// <summary>
    /// Converts a kebab case string to camel case
    /// </summary>
    /// <param name="string"></param>
    /// <returns></returns>
    public static string ToCamelCase(this string @string)
    {
      return KebabMatcher.Replace(@string, (match) =>
      {
        return match.Value.ToUpper()[1..];
      }).Capitalize();
    }

    public static string Capitalize(this string @string)
    {
      return @string[0].ToString().ToUpper() + @string[1..];
    }

    public static async Task CreatePushWebhookAsync(this GitLabClient client, IConfiguration config, TamagitchiProject project)
    {
      var request = new CreateWebhookRequest($"http://{config.GetExternalUrl()}/api/callback/gitlab/push/{project.Id}")
      {
        PushEvents = true,
        
      };

      var hook = await client.Webhooks.CreateAsync((int)project.GitlabId, request);
      project.WebhookId= hook.Id;
    }

    public static string GetHostUrl(this IConfiguration config) => $"http://{config.GetValue<string>("CallbackService:Host")}:{config.GetValue<int>("CallbackService:Port")}";
    public static string GetExternalUrl(this IConfiguration config) => $"http://{config.GetValue<string>("CallbackService:ExternalIp")}:{config.GetValue<int>("CallbackService:Port")}";
  }
}
