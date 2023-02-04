using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.GPT3Prompts
{
  public interface IPromptGenerator
  {
    public Task<string> GenerateTextAsync(object model);
  }
}
