using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Controls
{
    public class DJText : UiElement, IMonogameElement
    {
        // Champs de la classe...
        private List<DJLetter> letters;
        private string text;

        // Propriétés de la classe...
        // Constructeur de la classe...
        public DJText(string text, Vector2 position, Vector2 velocity, float rotation = 0, bool showHitbox = false) : base(position, velocity, 100, false, rotation, showHitbox)
        {
            this.text = text;
            letters = new List<DJLetter>();
        }

        // Méthodes de la classe...
        public override Rectangle Hitbox()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            for (int i = 0; i < text.Length; i++)
            {
                //letters.Add(new DJLetter(text[i]));
            }
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

    internal class DJLetter : UiElement
    {
        public DJLetter(char c, Vector2 position, Vector2 velocity, int sizePourcent = 100, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, velocity, 100, flipped, rotation, showHitbox)
        {
        }

        // Champs de la classe...
        // Propriétés de la classe...
        // Constructeur de la classe...
        // Méthodes de la classe...
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override Rectangle Hitbox()
        {
            throw new NotImplementedException();
        }

        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
