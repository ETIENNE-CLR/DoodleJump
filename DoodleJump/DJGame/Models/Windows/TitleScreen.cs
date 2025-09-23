using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Windows
{
    public class TitleScreen : MonogameWindow, IMonogameElement
    {
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("default");
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawBackground(spriteBatch, gameTime);
        }
    }
}
