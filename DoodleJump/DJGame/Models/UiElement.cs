using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace DJGame.Models
{
    public abstract class UiElement : IMonogameElement
    {
        // Champs de la classe...
        protected Texture2D texture;
        protected Vector2 position;
        protected Vector2 velocity;
        private float scale;
        private bool flipped;
        private float rotation;
        private bool showHitbox;

        // Propriétés de la classe...
        protected Vector2 Position { get => position; }
        protected Texture2D Texture { get => texture; }
        protected Vector2 Velocity { get => velocity; }
        public Rectangle Hitbox => new Rectangle(
                (int)(Position.X - (Texture.Width * scale) / 2f),
                (int)(Position.Y - (Texture.Height * scale) / 2f),
                (int)(Texture.Width * scale),
                (int)(Texture.Height * scale)
            );

        // Constructeur de la classe...
        public UiElement(Vector2 position, Vector2 velocity, float scale, bool flipped = false, float rotation = 0, bool showHitbox = false)
        {
            this.position = position;
            this.velocity = velocity;
            this.scale = scale;
            this.flipped = flipped;
            this.rotation = rotation;
            this.showHitbox = showHitbox;
        }

        // Méthodes de la classe...
        public void Remove()
        {
            SetSize(0);
        }

        protected void SetSize(int pourcent)
        {
            scale = pourcent / 100f;
        }

        protected void SetRotation(int degrees)
        {
            rotation = MathHelper.ToRadians(degrees);
        }

        public abstract void LoadContent(ContentManager content);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}
