using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Positioning
{
  public class AbsolutePosition : IPosition
  {
    private Vector2 Position { get; set; }
    public AbsolutePosition(Vector2 pos)
    {
      Position = pos;
    }

    public AbsolutePosition(float x, float y) 
    {
      Position = new Vector2(x, y);
    }

    public static implicit operator AbsolutePosition(Vector2 vec) => new AbsolutePosition(vec);

    public Vector2 GetPosition(IWidget widget) => Position;
  }
}
