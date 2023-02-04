using OpenAI.Net;
using OpenAI.Net.Models.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TamagitchiClient.GPT3Prompts
{
  public class PromptGenerator : IPromptGenerator
  {
    private IOpenAIService _aiService;
    private static readonly Regex ReplacementMarkerPattern = new Regex(@"<(\S*)>", RegexOptions.Compiled);


    public PromptGenerator(IOpenAIService aiService)
    {
      _aiService = aiService;
    }

    public async Task<string> GenerateTextAsync(object model)
    {
      var text = Resources.BasePrompt;
      var type = model.GetType();
      var res = ReplacementMarkerPattern.Replace(text, x =>
      {
        var propertyName = x.Groups[1].Value.ToCamelCase();
        var member = type.GetMember(propertyName, BindingFlags.Public | BindingFlags.Instance)
          .Where(x => x is not MethodInfo)
          .FirstOrDefault();

        var value = member switch
        {
          PropertyInfo property => property.GetValue(model),
          FieldInfo field => field.GetValue(model),
          _ => null
        };

        if (value == null)
          return x.Value;

        if (value is not string && value is IEnumerable enumerable)
          return string.Join(", ", enumerable.Cast<object>().Select(x => x.ToString()));

        return value.ToString();
      });

      var request = new TextCompletionRequest(OpenAI.Net.ModelTypes.TextDavinci003, res)
      {
        MaxTokens = 60,
        TopP = 1,
        FrequencyPenalty = 0.5,
        Temperature = 0.7
      };
      
      var response = await _aiService.TextCompletion.Get(request);
      if (response.IsSuccess)
        return response.Result.Choices.FirstOrDefault().Text.Trim();

      return null;
    }
  }
}
