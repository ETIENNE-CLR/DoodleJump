using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Enum;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Agents
{
    public abstract class AnimatedElement : UiElement, IMonogameElement
    {
        // Champs de la classe...
        protected Dictionary<string, Animation> animations;
        protected string animationName;
        protected bool isMoving;

        // Propriétés de la classe...
        protected Dictionary<string, Animation> Animations { get => animations; }
        public Animation CurrentAnimationObject { get => animations[animationName]; }
        public bool IsMoving { get => isMoving; }

        // Constructeur la classe...
        protected AnimatedElement(Vector2 position, Vector2 velocity, int sizePourcent = 100, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, velocity, sizePourcent, flipped, rotation, showHitbox)
        {
            this.animations = new Dictionary<string, Animation>();
            animationName = "";
            isMoving = false;
        }

        // Méthodes de la classe...
        public override Rectangle Hitbox()
        {
            Rectangle src = animations[animationName].CurrentFrame;
            return new Rectangle(
                (int)Math.Round(Position.X),
                (int)Math.Round(Position.Y),
                (int)Math.Round(src.Width * Scale),
                (int)Math.Round(src.Height * Scale)
            );
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!animations.ContainsKey(animationName)) return;

            // Draw
            Animation anim = animations[animationName];
            SpriteEffects effects = this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Texture, Position, anim.CurrentFrame, Color.White, Rotation, Vector2.Zero, Scale, effects, 0f);

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
    }
}
