using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine.Positioning
{
  public class CombinedPosition : IPosition
  {
    public bool Addititve { get; set; }
    public IPosition Left { get; init; }
    public IPosition Right { get; init; }

    public CombinedPosition(IPosition left, IPosition right, bool additive)
    {
      Left = left;
      Right = right;
      Addititve = additive;
    }


    public Vector2 GetPosition(IWidget widget)
    {
      if(Addititve)
        return Left.GetPosition(widget) + Right.GetPosition(widget);
      else
        return Left.GetPosition(widget) - Right.GetPosition(widget);
    }
  }
}
