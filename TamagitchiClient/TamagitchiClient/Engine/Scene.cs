using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TamagitchiClient.Engine.Widgets;

namespace TamagitchiClient.Engine
{
  public class Scene : IEnumerable<IWidget>
  {
    private readonly List<IWidget> _widgets = new List<IWidget>();
    public IEnumerator<IWidget> GetEnumerator() => _widgets.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _widgets.GetEnumerator();

    private readonly GraphicsDevice _graphcis;
    private readonly SpriteBatch _spriteBatch;

    public Scene(GraphicsDevice graphics)
    {
      _graphcis = graphics;
      _spriteBatch = new SpriteBatch(graphics);
    }

    public void Add(IWidget widget)
    {
      _widgets.Add(widget);
    }

    public void Remove(IWidget widget)
    {
      _widgets.Remove(widget);
    }

    public void Render(GameTime time)
    {
      _spriteBatch.Begin();
      foreach (var widget in _widgets.Where(x => x.Visible))
        widget.Render(time, _spriteBatch);
      _spriteBatch.End();
    }

  }
}
