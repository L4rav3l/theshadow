using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

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

        rnd = new Random();
    }

    public void Update(GameTime gameTime)
    {
        for(int i = 0; i < 4; i++)
        {
            int num = rnd.Next(1, 4);

            if(num == 1)
            {
                GameData.CubeColor[i] = Color.Red;
            }

            if(num == 2)
            {
                GameData.CubeColor[i] = Color.Yellow;
            }

            if(num == 3)
            {
                GameData.CubeColor[i] = Color.Green;
            }

            if(num == 4)
            {
                GameData.CubeColor[i] = Color.Blue;
            }
        }

        _sceneManager.ChangeScene("map");        
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphics.Clear(Color.Black);
    }
}