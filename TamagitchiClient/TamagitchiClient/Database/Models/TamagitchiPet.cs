using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient.Database.Models
{
  public class TamagitchiPet : EntityBase
  {
    [Required]
    public virtual TamagitchiUser Owner { get; set; }

    [MaxLength(5)]
    public virtual IList<TamagitchiCharacterTrait> TamagitchiCharacterTraits { get; set; } = new List<TamagitchiCharacterTrait>();

    [NotMapped]
    public HashSet<CharacterTrait> Personality => TamagitchiCharacterTraits.Select(t => t.Trait).ToHashSet();

    [NotMapped]
    public float HealthPercentage => (float)CurrentHealth / MaxHealth;

    public string Name { get; set; }
    public int MaxHealth { get; set; }
    public int CurrentHealth { get; set; }
    public bool Alive { get; set; }
    public DateTime? LastStarvationTick { get; set; }
    public DateTime LastFood { get; set; }

  }
}
