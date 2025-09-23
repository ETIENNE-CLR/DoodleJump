using System;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Windows
{
    public abstract class MonogameWindow : IMonogameElement
    {
        // Champs de la classe...
        protected Texture2D bgTexture;

        // Méthodes de la classe...
        public abstract void LoadContent(ContentManager content);

        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);

        public void DrawBackground(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Init
            float scaleX = (float)Game1.ScreenDimensions.Width / bgTexture.Width;
            float scaleY = (float)Game1.ScreenDimensions.Height / bgTexture.Height;

            // Dessiner le background
            spriteBatch.Draw(bgTexture, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, new Vector2(scaleX, scaleY), SpriteEffects.None, 0f);
        }

        public abstract void Update(GameTime gameTime);
    }
}
