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
using DJGame.Models.Game;
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
            ply = new Player(new Vector2(Game1.ScreenDimensions.Center.X, Game1.ScreenDimensions.Center.Y), new Vector2(10, 10), 100, false, 0, false);
            paddles = new List<Paddle>();

            int y = Game1.ScreenDimensions.Height * 3 / 4;
            for (int i = 0; i < 5; i++)
            {
                paddles.Add(new Paddle(PaddleType.SIMPLE, new Vector2(60 * i + 10, y), Paddle.NORME_SIZE, false, 0, false));
            }
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            foreach (Paddle p in paddles)
                p.LoadContent(content);
            ply.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            ply.Update(gameTime);
            foreach (Paddle p in paddles)
            {
                p.Update(gameTime);
                if (p.Hitbox().Intersects(ply.Hitbox()))
                {
                    ply.Jump();
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawBackground(spriteBatch, gameTime);
            foreach (Paddle p in paddles)
                p.Draw(spriteBatch, gameTime);
            ply.Draw(spriteBatch, gameTime);
        }
    }
}
