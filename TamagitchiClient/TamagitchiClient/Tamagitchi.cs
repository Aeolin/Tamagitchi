using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReInject.Interfaces;
using Swan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TamagitchiClient.Database;
using TamagitchiClient.Engine;
using TamagitchiClient.Engine.Positioning;
using TamagitchiClient.Engine.Scaling;
using TamagitchiClient.Engine.Widgets;
using TamagitchiClient.TamagotchiLogic;
using TamagitchiClient.TamagotchiLogic.Models;

namespace TamagitchiClient
{
  public class Tamagitchi : Game
  {
    private GraphicsDeviceManager _graphics;
    private readonly TamagotchiCore _coreLogic;
    private Scene _activeScence;
    private Scene _logoScene;
    private Scene _idleScene;
    private Scene _updateScene;
    private Scene _udpateSceneHeadpat;
    private Scene _errorScene;

    private readonly List<IWidget> _widgets = new List<IWidget>();
    private GameState _state = GameState.Logo;
    private TimeSpan _sceneStartTime = TimeSpan.Zero;
    private DisplayUpdate _currentUpdate;
    private IDependencyContainer _container;
    private IConfiguration _config;

    public Tamagitchi(TamagotchiCore core, IConfiguration config, IDependencyContainer container)
    {
      _graphics = new GraphicsDeviceManager(this);
      _graphics.GraphicsProfile = GraphicsProfile.Reach;
      _graphics.PreferMultiSampling = false;
      var screenSettings = config.GetSection("Settings:Screen");
      _graphics.PreferredBackBufferWidth = screenSettings.GetValue<int>("Width");
      _graphics.PreferredBackBufferHeight = screenSettings.GetValue<int>("Height");
      _graphics.IsFullScreen=screenSettings.GetValue<bool>("Fullscreen");
      _graphics.ApplyChanges();
      Content.RootDirectory = "Content";
      IsMouseVisible = true;
      _coreLogic = core;
      _container = container;
      _config = config;
    }

    protected override void Initialize()
    {
      using(var context = _container.GetInstance<TamagitchiContext>())
      {
        var userId = _config.GetValue<long>("Settings:GitlabUserId");
        var pet = context.Pets.FirstOrDefault(x => x.Owner.GitlabId == userId);
        if(pet == null)
        {
          _state = GameState.Error;
          _activeScence = _errorScene;
        }

        _currentUpdate = new DisplayUpdate { Pet = pet };
      }
      base.Initialize();
    }

    protected override void LoadContent()
    {
      _logoScene = SceneBuilder.New(Content, GraphicsDevice)
        .PushBaseContainer(_graphics)
        .WithSprite("Sprites/Logo")
        .WithRelativePosition(VerticalAlignment.Center, HorizontalAlignment.Center)
        .WithRelativeScaling(1F)
        .Build();

      _idleScene = SceneBuilder.New(Content, GraphicsDevice)
        .PushBaseContainer(_graphics)
        .WithSprite("Sprites/Background")
        .WithRelativePosition(VerticalAlignment.Center, HorizontalAlignment.Center)
        .WithRelativeScaling(1F)
        .WithAnimatedSprite("Sprites/Idle", 150)
        .WithPosition(
          new CombinedPosition(
            new RelativePosition(VerticalAlignment.Bottom, HorizontalAlignment.Center),
            new AbsolutePosition(new Vector2(0, 90)),
            false))
        .WithRelativeScaling(0.5F)
        .WithPercentageSprite("Gui/Healthbar", () => _currentUpdate?.Pet?.HealthPercentage ?? 0, 50, FrameSizeMode.Column, 48)
        .WithRelativePosition(0.01F, 0.1F, null, null)
        .WithRelativeScaling(0.3F)
        .Build();

      var updateBuilder = SceneBuilder.New(Content, GraphicsDevice)
        .PushBaseContainer(_graphics)
        .WithSprite("Sprites/Background")
        .WithRelativePosition(VerticalAlignment.Center, HorizontalAlignment.Center)
        .WithRelativeScaling(1F)
        .WithAnimatedSprite("Sprites/Idle", 150)
        .WithPosition(
          new CombinedPosition(
            new RelativePosition(VerticalAlignment.Bottom, HorizontalAlignment.Center),
            new AbsolutePosition(0, 90),
            false))
        .WithRelativeScaling(0.5F);
      var sprite = updateBuilder.CurrentWidget;
      updateBuilder.WithSprite("Gui/Speechbubble")
        .WithRelativeTo(sprite, Direction.East)
        .WithRelativeScaling(0.3F)
        .WithTextSprite("Font/Pixelated", () => _currentUpdate.Text, new Vector2(500, 300))
        .WithPosition(
          new CombinedPosition(
              new RelativeToWidgetPosition(sprite, Direction.East),
              new AbsolutePosition(10, 10),
              true
          ))
        .WithRelativeScaling(0.3F)
        .WithPercentageSprite("Gui/Healthbar", () => _currentUpdate?.Pet?.HealthPercentage ?? 0, 50, FrameSizeMode.Column, 48)
        .WithRelativePosition(0.01F, 0.1F, null, null)
        .WithRelativeScaling(0.3F);
      _updateScene = updateBuilder.Build();

      var headpatBuilder = SceneBuilder.New(Content, GraphicsDevice)
        .PushBaseContainer(_graphics)
        .WithSprite("Sprites/Background")
        .WithRelativePosition(VerticalAlignment.Center, HorizontalAlignment.Center)
        .WithRelativeScaling(1F)
        .WithAnimatedSprite("Sprites/Headpat", 150)
        .WithPosition(
          new CombinedPosition(
            new RelativePosition(VerticalAlignment.Bottom, HorizontalAlignment.Center),
            new AbsolutePosition(0, 90),
            false))
        .WithRelativeScaling(0.5F);
      sprite = headpatBuilder.CurrentWidget;
      headpatBuilder.WithSprite("Gui/Speechbubble")
        .WithRelativeTo(sprite, Direction.East)
        .WithRelativeScaling(0.3F)
        .WithTextSprite("Font/Pixelated", () => _currentUpdate.Text, new Vector2(500, 300))
        .WithPosition(
          new CombinedPosition(
              new RelativeToWidgetPosition(sprite, Direction.East),
              new AbsolutePosition(10, 10),
              true
          ))
        .WithRelativeScaling(0.3F)
        .WithPercentageSprite("Gui/Healthbar", () => _currentUpdate?.Pet?.HealthPercentage ?? 0, 50, FrameSizeMode.Column, 48)
        .WithRelativePosition(0.01F, 0.1F, null, null)
        .WithRelativeScaling(0.3F);
      _udpateSceneHeadpat = headpatBuilder.Build();

      _errorScene = SceneBuilder.New(Content, GraphicsDevice)
        .PushBaseContainer(_graphics)
        .WithTextSprite("Font/Pixelated", () => $"Error no Pet for User with GitlabId {_config.GetValue<long>("Settings:GitlabUserId")}", new Vector2(1000, 1000), Color.Red)
        .WithRelativePosition(VerticalAlignment.Center, HorizontalAlignment.Center)
        .WithRelativeScaling(0.5F)
        .Build();
    }

    protected override void Update(GameTime gameTime)
    {
      if (_sceneStartTime == TimeSpan.Zero && _state != GameState.Error)
      {
        _sceneStartTime = gameTime.TotalGameTime;
        _activeScence = _logoScene;
        _state = GameState.Logo;
      }

      switch (_state)
      {
        case GameState.Logo:
          if (gameTime.TotalGameTime - _sceneStartTime > TimeSpan.FromSeconds(5))
          {
            _state = GameState.Idle;
            _activeScence = _idleScene;
            _sceneStartTime = gameTime.TotalGameTime;
          }
          break;

        case GameState.Idle:
          if (gameTime.TotalGameTime - _sceneStartTime > TimeSpan.FromSeconds(15))
          { 
            if (_coreLogic?.TryGetNextUpdate(out var update) == true)
            {
              _currentUpdate = update;
              _state = GameState.DisplayUpdate;
              _activeScence = _currentUpdate.Animation != null ? _udpateSceneHeadpat : _updateScene;
              _sceneStartTime = gameTime.TotalGameTime;
            }
          }
          break;

        case GameState.DisplayUpdate:
          if (gameTime.TotalGameTime - _sceneStartTime > TimeSpan.FromMinutes(1))
          {
            _activeScence = _idleScene;
            _sceneStartTime = gameTime.TotalGameTime;
            _state = GameState.Idle;
          }
          break;

        case GameState.Error:
          {
            _activeScence = _errorScene;
          }
          break;
      }

      base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
      GraphicsDevice.Clear(Color.Black);
      _activeScence.Render(gameTime);
      base.Draw(gameTime);
    }
  }

  public enum GameState
  {
    Logo,
    Idle,
    DisplayUpdate,
    Error
  }
}