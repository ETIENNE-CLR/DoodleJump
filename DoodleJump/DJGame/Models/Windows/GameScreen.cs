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
        private DJNumberFont scoreTextEl;
        private Player ply;
        private List<Paddle> paddles;
        private float highestPaddleY;

        // Constructeur de la classe...
        public GameScreen()
        {
            int marginScore = 17;
            ply = new Player(new Vector2(Game1.ScreenDimensions.Center.X, Game1.ScreenDimensions.Center.Y), new Vector2(6, 12), 100, false, 0, false);
            paddles = new List<Paddle>();
            scoreTextEl = new DJNumberFont(ply.Score, new Vector2(marginScore, marginScore), Vector2.Zero, 0, true);
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            // première plateforme de départ
            ply.LoadContent(content);
            float startY = Game1.Camera.Position.Y + Game1.ScreenDimensions.Height - 100;
            Paddle startPlatform = new Paddle(PaddleType.SIMPLE, new Vector2((Game1.ScreenDimensions.Width / 2) - (ply.Hitbox().Width * 1 / 4), startY));
            paddles.Add(startPlatform);

            // Texture et UI
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            top = content.Load<Texture2D>("Tops/default");
            scoreTextEl.LoadContent(content);

            // foreach LoadContent
            foreach (Paddle p in paddles)
                p.LoadContent(content);
            foreach (Projectile s in ply.Shoots)
                s.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            // Joueur
            ply.Update(gameTime);
            Game1.Camera.Follow(ply);

            // Update des platformes
            for (int i = paddles.Count - 1; i >= 0; i--)
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

            // Nouvelles plateformes
            PlatformsGeneration(gameTime);

            // Shoots
            for (int i = ply.Shoots.Count - 1; i >= 0; i--)
            {
                Projectile s = ply.Shoots[i];
                s.Update(gameTime);
                if (s.Position.Y < ply.Position.Y - Game1.ScreenDimensions.Center.Y)
                    ply.ShootOutScreen(i);
            }

            // Update score view
            scoreTextEl.UpdateNumberText(ply.Score);
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
            spriteBatch.Draw(top, new Vector2(0, 0), new Rectangle(0, 0, 640, 92), Color.White);
            scoreTextEl.Draw(spriteBatch, gameTime);
        }

        /// <summary>
        /// Méthode qui permet de générer les plateformes pour le DoodleJump
        /// Générer avec ChatGPT
        /// </summary>
        private void PlatformsGeneration(GameTime gameTime)
        {
            const int marginTop = 300;
    
            // Trouve la plateforme la plus haute (donc la plus petite valeur Y)
            float currentHighestY = paddles.Min(p => p.Position.Y);

            // Difficulté : dépend du score et du temps
            float timeFactor = (float)(gameTime.TotalGameTime.TotalSeconds / 60f);
            float difficulty = Math.Clamp((ply.Score / 2000f) + (timeFactor * 0.2f), 0f, 1f);

            // Espacement moyen et variation
            int baseSpacing = (int)MathHelper.Lerp(80, 200, difficulty);
            int variation = (int)MathHelper.Lerp(20, 60, difficulty);
            float removalChance = MathHelper.Lerp(0f, 0.3f, difficulty);

            // Génération vers le haut jusqu’à la marge
            while (currentHighestY > Game1.Camera.Position.Y - marginTop)
            {
                int verticalSpacing = baseSpacing + Game1.random.Next(-variation, variation);
                float newY = currentHighestY - verticalSpacing;
                float x = Game1.random.Next(0, Game1.ScreenDimensions.Width - 80);

                var newPaddle = new Paddle(PaddleType.SIMPLE, new Vector2(x, newY));
                newPaddle.LoadContent(Game1.PublicContent);

                // Chance de rater une plateforme (pour créer des trous)
                if (Game1.random.NextDouble() < removalChance)
                    continue;

                // Vérifie qu’on ne chevauche pas une autre
                bool overlap = paddles.Any(p => p.Hitbox().Intersects(newPaddle.Hitbox()));
                if (!overlap)
                {
                    paddles.Add(newPaddle);
                    currentHighestY = newY; // on avance la génération naturellement
                }
                else
                {
                    // si ça chevauche, on essaie juste un peu plus haut
                    currentHighestY -= 10;
                }
            }
        }
    }
}
