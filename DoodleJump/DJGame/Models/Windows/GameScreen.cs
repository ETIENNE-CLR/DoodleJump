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
using Microsoft.Xna.Framework.Input;

namespace DJGame.Models.Windows
{
    public class GameScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private Texture2D top;
        private DJNumberFont scoreTextEl;
        private Player player;
        private List<Paddle> paddles;
        private float highestPaddleY;
        GameOverScreen gameOverScreen;

        // Constructeur de la classe...
        public GameScreen()
        {
            int marginScore = 17;
            player = new Player(new Vector2(Game1.ScreenDimensions.Center.X, Game1.ScreenDimensions.Center.Y));
            paddles = new List<Paddle>();
            scoreTextEl = new DJNumberFont(player.Score, new Vector2(marginScore, marginScore), Vector2.Zero, 0, true);
            gameOverScreen = new GameOverScreen();
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            // première plateforme de départ
            player.LoadContent(content);
            float startY = Game1.Camera.Position.Y + Game1.ScreenDimensions.Height - 100;
            Paddle startPlatform = new Paddle(PaddleType.SIMPLE, new Vector2((Game1.ScreenDimensions.Width / 2) - (player.Hitbox().Width * 1 / 4), startY));
            paddles.Add(startPlatform);
            gameOverScreen.LoadContent(content);

            // Texture et UI
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            top = content.Load<Texture2D>("Tops/default");
            scoreTextEl.LoadContent(content);

            // foreach LoadContent
            foreach (Paddle p in paddles)
                p.LoadContent(content);
            foreach (Projectile s in player.Shoots)
                s.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            // Joueur
            player.Update(gameTime);

            // Mouvement du joueur
            KeyboardState kstate = Keyboard.GetState();
            player.Move(kstate);

            // Update des platformes
            Game1.Camera.Follow(player);
            for (int i = paddles.Count - 1; i >= 0; i--)
            {
                Paddle p = paddles[i];
                if (p.Type != PaddleType.SIMPLE) p.Update(gameTime);
                highestPaddleY = paddles.Min(p => p.Position.Y);

                // Il y a eu une collision
                if (p.Hitbox().Intersects(player.Hitbox()) && player.Velocity.Y > 0)
                {
                    // Quel type ?
                    switch (p.Type)
                    {
                        case PaddleType.SIMPLE:
                            player.Jump();
                            break;

                        case PaddleType.BREAKABLE:
                            p.Break();
                            break;

                        default:
                            throw new NotImplementedException();
                    }
                }

                // Suppression des paddles cassés
                if (p.CurrentAnimationObject.Finished)
                    paddles.RemoveAt(i);

                // Suppression des paddles en dessous de l'écran
                if (p.Position.Y > Game1.Camera.Position.Y + Game1.ScreenDimensions.Height)
                    paddles.RemoveAt(i);

                // Check Game Over
                bool gameOver = (player.Position.Y > Game1.Camera.lowerLimit + Game1.ScreenDimensions.Height);
                if (gameOver)
                {
                    gameOverScreen.UpdatePlayerPosition(player.Position);
                    Game1.activeScene = gameOverScreen;
                }
            }

            // Nouvelles plateformes
            PlatformsGeneration(gameTime);

            // Shoots
            for (int i = player.Shoots.Count - 1; i >= 0; i--)
            {
                Projectile s = player.Shoots[i];
                s.Update(gameTime);
                if (s.Position.Y < player.Position.Y - Game1.ScreenDimensions.Center.Y)
                    player.ShootOutScreen(i);
            }

            // Update score view
            scoreTextEl.UpdateNumberText(player.Score);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach (Paddle p in paddles)
                p.Draw(spriteBatch, gameTime);
            foreach (Projectile s in player.Shoots)
                s.Draw(spriteBatch, gameTime);
            player.Draw(spriteBatch, gameTime);

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
            float difficulty = Math.Clamp((player.Score / 2000f) + (timeFactor * 0.2f), 0f, 1f);

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

                var newPaddle = new Paddle(GetRandomPaddleType(), new Vector2(x, newY));
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

        private PaddleType GetRandomPaddleType()
        {
            int nbRand = Game1.random.Next(0, 2);
            return nbRand == 0 ? PaddleType.SIMPLE : PaddleType.BREAKABLE;
        }
    }
}
