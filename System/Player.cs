using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TheShadow;

public class Player
{
    public Vector2 Position;
    public Vector2 screenPos;
    public float Speed = 50f;

    public int Width = 16;
    public int Height = 16;

    public Rectangle Hitbox =>
        new Rectangle(int)((Position.X - Width / 2), (int)(Position.Y - Height / 2), Width, Height);

    public Player(Vector2 startPosition)
    {
        Position = startPosition;
    }

    public void Update(GameTime gameTime, List<Rectangle> solidTiles, Camera2D camera)
    {
        KeyboardState state = Keyboard.GetState();
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Vector2 oldPos = Poisiton;

        Vector2 movement = Vector2.Zero;

        if(state.IsKeyDown(Keys.W)) movement.Y -=1;
        if(state.IsKeyDown(Keys.S)) movement.Y +=1;
        if(state.IsKeyDown(Keys.A)) movement.X -=1;
        if(state.IsKeyDown(Keys.D)) movement.X +=1;

        if(movement != Vector2.Zero)
        {
            movement.Normalize();
        }

        foreach(var solid in solidTiles)
        {
            Rectangle hitbox = Hitbox;

            if(hitbox.Intersects(solid))
            {
                float dx = (hitbox.Center.X < solid.Center.X)
                    ? solid.Left - hitbox.Right
                    : solid.Right - hitbox.Left;

                float dy = (hitbox.Center.Y < solid.Center.Y)
                    ? solid.Top - hitbox.Bottom
                    : solid.Bottom - hitbox.Top;

                if(Math.Abs(dx) < Math.Abs(dy))
                {
                    Position.X += dx;
                } else {
                    Position.Y += dy;
                }
            }

            camera.Position = Position;
        }

        public void Draw(spriteBatch spriteBatch, Texture2D texture, Camera2D camera)
        {
            screenPos = camera.WorldToScreen(Position - new Vector2(Width / 2, Height / 2));
            spriteBatch.Draw(
                texture,
                screenPos,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                1f,
                SpriteEffects.None,
                0.2f
            );
        }
    }
}