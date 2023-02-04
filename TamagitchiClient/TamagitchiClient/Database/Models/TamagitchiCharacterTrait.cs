using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient.Database.Models
{
  public class TamagitchiCharacterTrait : EntityBase
  {
    public TamagitchiCharacterTrait(CharacterTrait trait)
    {
      Trait = trait;
    }

    public CharacterTrait Trait { get; set; }
    [Required]
    public virtual TamagitchiPet Pet { get; set; }
  }
}
