using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Game
{
    public class Projectile : UiElement, IMonogameElement
    {
        // Constructeur de la classe...
        public Projectile(Vector2 position, bool showHitbox = false) : base(position, new Vector2(10, 10), 100, false, 0, showHitbox)
        {
        }

        // Méthodes de la classe...
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw
            SpriteEffects effects = this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Texture, Position, Hitbox(), Color.White, Rotation, Vector2.Zero, this.Scale, effects, 0f);

            // Afficher la hitbox
            if (ShowHitbox)
            {
                int thickness = 2;
                var color = Color.Red;
                var rect = Hitbox();

                // Tracer les 4 côtés
                spriteBatch.Draw(Game1.WhitePixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color); // Haut
                spriteBatch.Draw(Game1.WhitePixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color); // Gauche
                spriteBatch.Draw(Game1.WhitePixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color); // Droite
                spriteBatch.Draw(Game1.WhitePixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color); // Bas
            }
        }

        public override Rectangle Hitbox()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/projectile");
        }

        public override void Update(GameTime gameTime)
        {
            this.position.Y -= velocity.Y;
        }
    }
}
