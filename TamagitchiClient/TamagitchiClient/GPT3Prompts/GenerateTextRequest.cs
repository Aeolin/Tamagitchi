using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TamagitchiClient.Database.Models;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient.GPT3Prompts
{
  public class GenerateTextRequest
  {
    public string PetName { get; init; }
    public string DeveloperName { get; init; }
    public HashSet<CharacterTrait> Personality { get; init; }
    public string Prompt { get; init; }
    public int FoodLevelBefore { get; init; }
    public int FoodLevelAfter { get; init; }
    
    public GenerateTextRequest(TamagitchiPet pet, int newHealth, string prompt)
    {
      PetName = pet.Name;
      DeveloperName = pet.Owner.Name;
      Personality = pet.Personality;
      Prompt = prompt;
      FoodLevelBefore = (int)((pet.CurrentHealth / (double)pet.MaxHealth) * 100);
      FoodLevelAfter = (int)((newHealth / (double)pet.MaxHealth) * 100);
    }
    
  }
}
