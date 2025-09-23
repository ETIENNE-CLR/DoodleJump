using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Controllers;
using DJGame.Interfaces;
using DJGame.Models.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Windows
{
    public class GameScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private Player ply;

        // Constructeur de la classe...
        public GameScreen()
        {
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("bg");
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            DrawBackground(spriteBatch, gameTime);
        }
    }
}
