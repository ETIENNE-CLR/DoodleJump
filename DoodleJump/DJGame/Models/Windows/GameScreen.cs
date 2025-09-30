using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
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
    public class GameScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private Player ply;
        private List<Paddle> paddles;

        // Constructeur de la classe...
        public GameScreen()
        {
            ply = new Player(new Vector2(Game1.ScreenDimensions.Center.X, Game1.ScreenDimensions.Center.Y), new Vector2(6, 10), 100, false, 0, false);
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
            ply.Update(gameTime);
            Game1.Camera.Follow(ply);

            // Update des platformes
            for (int i = paddles.Count - 1; i > 0; i--)
            {
                Paddle p = paddles[i];
                p.Update(gameTime);

                // Collisions
                if (p.Hitbox().Intersects(ply.Hitbox()))
                    ply.Jump();

                // Suppression des paddles en dessous de l'écran
                if (p.Position.Y > Game1.Camera.Position.Y + Game1.ScreenDimensions.Bottom)
                    paddles.Remove(p);
            }

            // Shoots
            foreach (Projectile s in ply.Shoots)
                s.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Paddle p in paddles)
                p.Draw(spriteBatch, gameTime);
            foreach (Projectile s in ply.Shoots)
                s.Draw(spriteBatch, gameTime);
            ply.Draw(spriteBatch, gameTime);
        }
    }
}
