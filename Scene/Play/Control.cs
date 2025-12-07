using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TheShadow;

public class Control : IScene
{
    private GraphicsDevice _graphics;
    private SceneManager _sceneManager;
    private ContentManager _content;

    private SpriteFont _pixelfont;

    private Texture2D _cubes;
    private Texture2D _mark;
    private Texture2D _x;

    public Control(GraphicsDevice _graphics, SceneManager _sceneManager, ContentManager _content)
    {
        this._graphics = _graphics;
        this._sceneManager = _sceneManager;
        this._content = _content;
    }

    public void LoadContent()
    {
        _pixelfont = _content.Load<SpriteFont>("pixelfont");

        _x = _content.Load<Texture2D>("x");
        _mark = _content.Load<Texture2D>("mark");
        
        _cubes = new Texture2D(_graphics, 1, 1);
        _cubes.SetData(new[] {Color.White});
    }

    public void Update(GameTime gameTime)
    {
        KeyboardState state = Keyboard.GetState();

        if(state.IsKeyDown(Keys.Escape) && !GameData.previous.IsKeyDown(Keys.Escape))
        {
            _sceneManager.ChangeScene("map");
        }

        GameData.previous = state;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _graphics.Clear(Color.Black);

        int Width = _graphics.Viewport.Width;
        int Height = _graphics.Viewport.Height;

        spriteBatch.Draw(_cubes, new Rectangle((int)(Width / 5 - 50), (int)(Height / 2) - 200, 160, 160), null, GameData.CubeColor[0]);
        spriteBatch.Draw(_cubes, new Rectangle((int)((Width / 5) * 2 - 50), (int)(Height / 2) - 200, 160, 160), null, GameData.CubeColor[1]);
        spriteBatch.Draw(_cubes, new Rectangle((int)((Width / 5) * 3 - 50), (int)(Height / 2) - 200, 160, 160), null, GameData.CubeColor[2]);
        spriteBatch.Draw(_cubes, new Rectangle((int)((Width / 5) * 4 - 50), (int)(Height / 2) - 200, 160, 160), null, GameData.CubeColor[3]);

        if(GameData.Cube1 == false)
        {
            spriteBatch.Draw(_x, new Vector2((int)(Width / 5) - 50, (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        } else {
            spriteBatch.Draw(_mark, new Vector2((int)(Width / 5) - 50, (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        }

        if(GameData.Cube2 == false)
        {
            spriteBatch.Draw(_x, new Vector2((int)((Width / 5) * 2 - 50), (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        } else {
            spriteBatch.Draw(_mark, new Vector2((int)((Width / 5) * 2 - 50), (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        }
        
        if(GameData.Cube3 == false)
        {
            spriteBatch.Draw(_x, new Vector2((int)((Width / 5) * 3 - 50), (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        } else {
            spriteBatch.Draw(_mark, new Vector2((int)((Width / 5) * 3 - 50), (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        }

        if(GameData.Cube4 == false)
        {
            spriteBatch.Draw(_x, new Vector2((int)((Width / 5) * 4 - 50), (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        } else {
            spriteBatch.Draw(_mark, new Vector2((int)((Width / 5) * 4 - 50), (Height / 2)), null, Color.White, 0f, Vector2.Zero, 10f, SpriteEffects.None, 0.1f);
        }

    }
}