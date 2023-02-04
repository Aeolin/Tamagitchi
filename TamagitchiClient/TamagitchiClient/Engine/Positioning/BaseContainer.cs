using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TamagitchiClient.Engine.Positioning
{
  public class BaseContainer : IContainer
  {
    private readonly GraphicsDeviceManager _graphics;
    public BaseContainer(GraphicsDeviceManager graphics)
    {
      _graphics=graphics;
    }
    public Vector2 Size => new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
  }
}
