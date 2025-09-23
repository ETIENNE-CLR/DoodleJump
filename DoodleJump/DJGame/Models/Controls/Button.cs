using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;

namespace DJGame.Models.Controls
{
    internal abstract class Button : UiElement, IMonogameElement
    {
        // Champs de la classe...
        public Action actionToDo;
        protected bool clicked;
        protected Rectangle normalForm;
        protected Rectangle clickedForm;

        // Constructeur de la classe...
        public Button(Action toDo, Vector2 position, Vector2 velocity, int sizePourcent = 100, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, velocity, sizePourcent, flipped, rotation, showHitbox)
        {
            this.actionToDo = toDo;
            clicked = false;
        }

        // Méthodes de la classe...
        public override Rectangle Hitbox()
        {
            Rectangle src = !clicked ? normalForm : clickedForm;
            return new Rectangle(
                (int)Math.Round(Position.X),
                (int)Math.Round(Position.Y),
                (int)Math.Round(src.Width * Scale),
                (int)Math.Round(src.Height * Scale)
            );
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // Draw
            Rectangle form = !clicked ? normalForm : clickedForm;
            SpriteEffects effects = this.Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteBatch.Draw(Texture, Position, form, Color.White, Rotation, Vector2.Zero, this.Scale, effects, 0f);

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

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Rectangle boutonRect = Hitbox();

            if (boutonRect.Contains(mouseState.Position))
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                    clicked = true;
                else if (mouseState.LeftButton == ButtonState.Released && clicked)
                {
                    actionToDo?.Invoke();
                    clicked = false;
                }
            }
            else
                clicked = false;
        }
    }
}
