using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Positioning;
using TamagitchiClient.Engine.Scaling;

namespace TamagitchiClient.Engine.Widgets
{
  public interface IWidget
  {
    public IContainer Container { get; }
    public IPosition Position { get; set; }
    public IScaling Scale { get; set; }
    public void Render(GameTime time, SpriteBatch batch);
    public Vector2 Size { get; }
    public Vector2 OriginalSize { get; }
    public bool Visible { get; }
  }
}
