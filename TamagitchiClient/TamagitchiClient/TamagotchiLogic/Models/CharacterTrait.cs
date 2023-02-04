using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.TamagotchiLogic.Models
{
  public enum CharacterTrait
  {
    Happy,

    [ExclusiveTo(Grumpy, Depressed)]
    Cheerfull,

    [ExclusiveTo(Cheerfull)]
    Grumpy,

    [ExclusiveTo(Hatefull)]
    Loving,

    [ExclusiveTo(Loving, Friendly)]
    Hatefull,

    [ExclusiveTo(Diligent)]
    Lazy,

    [ExclusiveTo(Lazy)]
    Diligent,

    [ExclusiveTo(Unintelligent)]
    Intelligent,

    [ExclusiveTo(Intelligent)]
    Unintelligent,

    [ExclusiveTo(Weak)]
    Strong,

    [ExclusiveTo(Strong)]
    Weak,

    [ExclusiveTo(Shy)]
    Stubborn,

    [ExclusiveTo(Lucky)]
    Unlucky,

    [ExclusiveTo(Motivated)]
    Unmotivated,

    [ExclusiveTo(Unlucky)]
    Lucky,

    [ExclusiveTo(Unmotivated)]
    Motivated,

    [ExclusiveTo(Mean, Hatefull)]
    Friendly,

    [ExclusiveTo(Friendly)]
    Mean,

    [ExclusiveTo(Brave, Strong, Clingy)]
    Shy,

    [ExclusiveTo(Shy)]
    Brave,

    [ExclusiveTo(Hatefull, Depressed)]
    Caring,

    [ExclusiveTo(Loving)]
    Tsundere,

    Yandere, 
    Clumsy,

    [ExclusiveTo(Hatefull)]
    Clingy,
    
    [ExclusiveTo(Cheerfull, Diligent, Motivated, Brave, Energetic)]
    Depressed,

    [ExclusiveTo(Depressed)]
    Energetic,

    [ExclusiveTo(Energetic)]
    Calm
  }
}
