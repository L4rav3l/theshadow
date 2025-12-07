using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace TheShadow;

public class End : IScene
{
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private ContentManager _content;

    private SpriteFont _pixelfont;

    private bool _first = true;

    private TimeSpan _runtime;

    public End(GraphicsDevice _graphics, SceneManager _sceneManager, ContentManager _content)
    {
        this._graphics = _graphics;
        this._sceneManager = _sceneManager;
        this._content = _content;
    }

    public void LoadContent()
    {
        _pixelfont = _content.Load<SpriteFont>("pixelfont");
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();
        
        if(state.IsKeyDown(Keys.Enter) && !GameData.previous.IsKeyDown(Keys.Enter))
        {
            _sceneManager.ChangeScene("menu");
        }

        if(_first)
        {
            GameData.endDate = DateTime.Now;
            _runtime = GameData.endDate - GameData.startDate;
            _first = false;
        }

        GameData.previous = state;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphics.Clear(Color.Black);

        int Width = _graphics.Viewport.Width;
        int Height = _graphics.Viewport.Height;

        Vector2 YouAreM = _pixelfont.MeasureString("You completed this game");
        Vector2 YouAre = new Vector2((Width / 2) - (YouAreM.X / 2), (Height / 2) - (YouAreM.Y / 2) - 50);

        Vector2 DuringM = _pixelfont.MeasureString($"Under {_runtime.Minutes}:{_runtime.Seconds}");
        Vector2 During = new Vector2((Width / 2) - (DuringM.X / 2), (Height / 2) - (DuringM.Y / 2) + 50);

        Vector2 PressEnterM = _pixelfont.MeasureString("Press Enter");
        Vector2 PressEnter = new Vector2((Width / 2) - (PressEnterM.X / 2), (Height / 2) - (PressEnterM.Y / 2) + 100);

        spriteBatch.DrawString(_pixelfont, "You completed this game", YouAre, Color.White);
        spriteBatch.DrawString(_pixelfont, $"Under {_runtime.Minutes}:{_runtime.Seconds}", During, Color.White);
        spriteBatch.DrawString(_pixelfont, "Press Enter", PressEnter, Color.White);
    }
}