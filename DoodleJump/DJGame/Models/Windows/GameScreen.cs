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
            ply = new Player(new Vector2(Game1.ScreenDimensions.Center.X, Game1.ScreenDimensions.Center.Y), new Vector2(7, 10), 100, false, 0, false);
            paddles = new List<Paddle>();

            int y = Game1.ScreenDimensions.Height * 3 / 4;
            for (int i = 0; i < (Game1.ScreenDimensions.Width / Paddle.NORME_SIZE) - 1; i++)
            {
                paddles.Add(new Paddle(PaddleType.SIMPLE, new Vector2(60 * i + 10, y), Paddle.NORME_SIZE, false, 0, false));
            }

            y = Game1.ScreenDimensions.Bottom + 100;
            for (int i = 0; i < 10; i++)
            {
                y -= 100;
                paddles.Add(new Paddle(PaddleType.SIMPLE, new Vector2(30, y), Paddle.NORME_SIZE));
            }
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            ply.LoadContent(content);
            foreach (Paddle p in paddles)
                p.LoadContent(content);
            foreach (Projectile s in ply.Shoots)
                s.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            int suppressionMarginPaddle = 0;
            ply.Update(gameTime);

            // Update des platformes
            for (int i = paddles.Count - 1; i > 0; i--)
            {
                Paddle p = paddles[i];
                p.Update(gameTime);

                // Gestion de la caméra
                if (ply.Position.Y < Game1.ScreenDimensions.Height * 1 / 3)
                {
                    p.Scroll(10);
                }

                // Collisions
                if (p.Hitbox().Intersects(ply.Hitbox()))
                    ply.Jump();

                // Suppression des paddles en dessous de l'écran
                if (p.Position.Y + suppressionMarginPaddle > Game1.ScreenDimensions.Bottom)
                    paddles.Remove(p);

                // Game Over
                if (ply.Position.Y > Game1.ScreenDimensions.Bottom)
                    p.Scroll(-10);
            }

            // Shoots
            foreach (Projectile s in ply.Shoots)
                s.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawBackground(spriteBatch, gameTime);
            foreach (Paddle p in paddles)
                p.Draw(spriteBatch, gameTime);
            foreach (Projectile s in ply.Shoots)
                s.Draw(spriteBatch, gameTime);
            ply.Draw(spriteBatch, gameTime);
        }
    }
}
