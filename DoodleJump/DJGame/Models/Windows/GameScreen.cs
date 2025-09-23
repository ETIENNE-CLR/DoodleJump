using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using DJGame.Controllers;
using DJGame.Enum;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using DJGame.Models.Controls;
using DJGame.Models.Game.Paddle;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Windows
{
    internal class GameScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private Player ply;
        private List<Paddle> paddles;

        // Constructeur de la classe...
        public GameScreen()
        {
            paddles = new List<Paddle>();
            paddles.Add(new Paddle(PaddleType.SIMPLE, new Vector2(10, 10), 100, false, 0, true));
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("bg");
            foreach (Paddle p in paddles)
                p.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawBackground(spriteBatch, gameTime);
            foreach (Paddle p in paddles)
                p.Draw(spriteBatch, gameTime);
        }
    }
}
