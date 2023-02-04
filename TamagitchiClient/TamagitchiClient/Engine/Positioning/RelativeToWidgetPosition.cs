using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Scaling;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Positioning
{
  public class RelativeToWidgetPosition : IPosition
  {

    public Direction Direction { get; set; }
    public IWidget Other { get; init; }

    public RelativeToWidgetPosition(IWidget other, Direction direction)
    {
      this.Other = other;
      this.Direction = direction;
    }

    public Vector2 GetPosition(IWidget widget)
    {
      var basePos = Other.Position.GetPosition(Other);
      float x = basePos.X + (Direction.HasFlag(Direction.West) ? -widget.Size.X : (Direction.HasFlag(Direction.East) ? Other.Size.X : 0));
      float y = basePos.Y + (Direction.HasFlag(Direction.North) ? -widget.Size.Y : (Direction.HasFlag(Direction.South) ? Other.Size.Y : 0));
      return new Vector2(x, y);
    }
  }
}
