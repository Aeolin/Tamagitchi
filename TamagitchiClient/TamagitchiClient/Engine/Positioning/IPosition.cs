using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Positioning
{
  public interface IPosition
  {
    Vector2 GetPosition(IWidget widget);
  }
}
