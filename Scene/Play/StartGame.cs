using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Linq;

namespace TheShadow;

public class StartGame : IScene
{
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private ContentManager _content;

    private Random rnd;

    public StartGame(GraphicsDevice _graphics, SceneManager _sceneManager, ContentManager _content)
    {
        this._graphics = _graphics;
        this._sceneManager = _sceneManager;
        this._content = _content;
    }

    public void LoadContent()
    {
        _sceneManager.AddScene(new Map(_graphics, _sceneManager, _content), "map");
        _sceneManager.AddScene(new Control(_graphics, _sceneManager, _content), "control");

        Random rnd = new Random();

        Color[] colors = new Color[]
        {
            Color.Red,
            Color.Yellow,
            Color.Green,
            Color.Blue
        };

        colors = colors.OrderBy(x => rnd.Next()).ToArray();

        for(int i = 0; i < 4; i++)
        {
            GameData.CubeColor[i] = colors[i];
        }
    }

    public void Update(GameTime gameTime)
    {
        _sceneManager.ChangeScene("map");        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphics.Clear(Color.Black);
    }
}