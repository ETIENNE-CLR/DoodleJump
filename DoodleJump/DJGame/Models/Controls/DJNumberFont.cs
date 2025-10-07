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

namespace DJGame.Models.Controls
{
    public class DJNumberFont : UiElement, IMonogameElement
    {
        // Champs de la classe...
        private List<Rectangle> digitsToDisplay;
        private Dictionary<char, Rectangle> referenceDigits;
        private int numberText;

        // Propriétés de la classe...
        public int Text { get => numberText; }

        // Constructeur de la classe...
        public DJNumberFont(int numbText, Vector2 position, Vector2 velocity, float rotation = 0, bool showHitbox = false) : base(position, velocity, 95, false, rotation, showHitbox)
        {
            referenceDigits = new Dictionary<char, Rectangle>();
            digitsToDisplay = new List<Rectangle>();
            UpdateNumberText(numbText);
        }

        // Méthodes de la classe...
        public override Rectangle Hitbox()
        {
            int w = 0;
            int h = 0;
            foreach (Rectangle l in digitsToDisplay)
            {
                w += l.Width;
                h = Math.Max(h, l.Height);
            }
            return new Rectangle((int)Position.X, (int)Position.Y, w, h);
        }

        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Tops/default");
            referenceDigits.Add('1', Animation.GenerateAnimation(9, 33, 644, 1, 0, 0, 1)[0]);
            referenceDigits.Add('2', Animation.GenerateAnimation(29, 33, 655, 1, 0, 0, 1)[0]);
            referenceDigits.Add('3', Animation.GenerateAnimation(25, 33, 688, 1, 0, 0, 1)[0]);
            referenceDigits.Add('4', Animation.GenerateAnimation(22, 33, 714, 1, 0, 0, 1)[0]);
            referenceDigits.Add('5', Animation.GenerateAnimation(27, 33, 738, 1, 0, 0, 1)[0]);
            referenceDigits.Add('6', Animation.GenerateAnimation(26, 33, 769, 1, 0, 0, 1)[0]);
            referenceDigits.Add('7', Animation.GenerateAnimation(26, 33, 798, 1, 0, 0, 1)[0]);
            referenceDigits.Add('8', Animation.GenerateAnimation(24, 33, 825, 1, 0, 0, 1)[0]);
            referenceDigits.Add('9', Animation.GenerateAnimation(22, 33, 853, 1, 0, 0, 1)[0]);
            referenceDigits.Add('0', Animation.GenerateAnimation(25, 33, 878, 1, 0, 0, 1)[0]);
        }

        public void UpdateNumberText(int updatedNumberText)
        {
            numberText = Math.Max(0, updatedNumberText);
            digitsToDisplay.Clear();
            foreach (char c in numberText.ToString())
                if (referenceDigits.ContainsKey(c))
                    digitsToDisplay.Add(referenceDigits[c]);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Vector2 copy = new Vector2(Position.X, Position.Y);
            int letterSpacing = 2;

            // Draw chaque digitsToDisplay
            foreach (Rectangle l in digitsToDisplay)
            {
                spriteBatch.Draw(texture, copy, l, Color.White, Rotation, Vector2.Zero, Scale, SpriteEffects.None, 0f);
                copy.X += l.Width + letterSpacing;
            }
        }
    }
}
