using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.Engine.Positioning
{
  [Flags]
  public enum Direction
  {
    North = 1,
    East = 2,
    South = 4,
    West = 8,
    NorthEast = North | East,
    SouthEast = South | East,
    SouthWest = South | East,
    NorthWest = South | East,
  }
}
