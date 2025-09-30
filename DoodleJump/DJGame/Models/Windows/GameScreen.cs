using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private Texture2D top;
        private SpriteFont font;
        private DJText scoreTextEl;
        private Player ply;
        private List<Paddle> paddles;
        private float highestPaddleY;

        // Constructeur de la classe...
        public GameScreen()
        {
            ply = new Player(new Vector2(Game1.ScreenDimensions.Center.X, Game1.ScreenDimensions.Center.Y), new Vector2(6, 10), 100, false, 0, false);
            paddles = new List<Paddle>();

            // Ajout de paddle de base pour test
            int y = Game1.ScreenDimensions.Height * 3 / 4;
            for (int i = 0; i < (Game1.ScreenDimensions.Width / Paddle.NORME_SIZE) - 1; i++)
            {
                paddles.Add(new Paddle(PaddleType.SIMPLE, new Vector2(60 * i + 10, y)));
            }

            y = Game1.ScreenDimensions.Bottom + 100;
            for (int i = 0; i < 10; i++)
            {
                y -= 100;
                paddles.Add(new Paddle(PaddleType.SIMPLE, new Vector2(30, y)));
            }
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            font = content.Load<SpriteFont>("monogameText");
            top = content.Load<Texture2D>("Tops/default");

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
                highestPaddleY = paddles.Min(p => p.Position.Y);

                // Collisions
                if (p.Hitbox().Intersects(ply.Hitbox()) && ply.Velocity.Y > 0)
                    ply.Jump();

                // Suppression des paddles en dessous de l'écran
                if (p.Position.Y > Game1.Camera.Position.Y + Game1.ScreenDimensions.Height)
                    paddles.RemoveAt(i);
            }

            // Générer des nouvelles plateformes - aidé par ChatGPT
            int marginTop = 200;
            while (highestPaddleY > Game1.Camera.Position.Y - marginTop)
            {
                // Espacement vertical aléatoire en fonction du score du joueur
                int minSpacing = Math.Min(120, 60 + ply.Score / 100);
                int maxSpacing = Math.Min(200, 100 + ply.Score / 50);
                int verticalSpacing = Game1.random.Next(minSpacing, maxSpacing);
                highestPaddleY -= verticalSpacing;

                // Position horizontale aléatoire
                float x = Game1.random.Next(0, Game1.ScreenDimensions.Width - paddles[0].Hitbox().Width);

                // Vérifie qu’on ne chevauche pas une plateforme existante
                Paddle newPaddle = new Paddle(PaddleType.SIMPLE, new Vector2(x, highestPaddleY));
                newPaddle.LoadContent(Game1.PublicContent);
                bool overlap = paddles.Any(p => p.Hitbox().Intersects(newPaddle.Hitbox()));

                if (!overlap)
                {
                    paddles.Add(newPaddle);
                }
                else
                {
                    // si ça chevauche, on refait un tour → la boucle while va redonner une nouvelle chance
                    // (on pourrait limiter le nombre d'essais si nécessaire)
                }
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

            // Element fixe
            spriteBatch.End();
            spriteBatch.Begin();
            // spriteBatch.DrawString(font, ply.Score.ToString(), new Vector2(0, 0), Color.Black);
            spriteBatch.Draw(top, new Vector2(0, 0), new Rectangle(0, 0, 640, 92), Color.White);
        }
    }
}
