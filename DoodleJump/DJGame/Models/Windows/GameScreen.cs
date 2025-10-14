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
using Microsoft.VisualBasic.ApplicationServices;
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
                if (IsGameOver())
                {
                    gameOverScreen.UpdatePlayerPosition(player.Position);
                    Game1.activeScene = gameOverScreen;
                }
            }

            // Génération des nouvelles plateformes
            float gap = Game1.ScreenDimensions.Height / paddles.Count;
            float dernierePlateformeY = paddles.Max(p => p.Position.Y);
            if (player.Position.Y < dernierePlateformeY + 200)
            {
                Paddle paddle = new Paddle(PaddleType.SIMPLE, new Vector2(Game1.random.Next(0, Game1.ScreenDimensions.Width), dernierePlateformeY - gap));
                paddle.LoadContent(Game1.PublicContent);
            }

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
            const int maxAttemptsPerLayer = 5;

            // plateforme la plus haute
            float currentHighestY = paddles.Min(p => p.Position.Y);

            // Temps total (en minutes)
            float timeMinutes = (float)gameTime.TotalGameTime.TotalMinutes;

            // --- difficulté de base très lente ---
            float baseDifficulty = Math.Clamp((player.Score / 15000f) + (timeMinutes * 0.015f), 0f, 1f);

            // --- effet de vague ---
            // crée un cycle de variation : la difficulté monte et descend lentement avec le score
            // sin() varie entre -1 et +1 → on la remet entre 0 et 1
            float wave = (float)((Math.Sin(player.Score / 3000f) + 1f) / 2f);

            // combine les deux (base + vague)
            float difficulty = Math.Clamp(baseDifficulty * 0.7f + wave * 0.3f, 0f, 1f);

            // espacement entre plateformes (plus espacé = plus difficile)
            int baseSpacing = (int)MathHelper.Lerp(100, 250, difficulty);
            int variation = (int)MathHelper.Lerp(25, 80, difficulty);

            // chance de trou
            float removalChance = MathHelper.Lerp(0f, 0.20f, difficulty);

            // chance d’ajouter une plateforme cassable à proximité
            float breakableChance = MathHelper.Lerp(0.05f, 0.20f, difficulty);

            while (currentHighestY > Game1.Camera.Position.Y - marginTop)
            {
                bool placed = false;
                int attempts = 0;

                while (!placed && attempts < maxAttemptsPerLayer)
                {
                    attempts++;

                    int verticalSpacing = baseSpacing + Game1.random.Next(-variation, variation);
                    float newY = currentHighestY - verticalSpacing;
                    float x = Game1.random.Next(0, Game1.ScreenDimensions.Width - 80);

                    // sauter une chance de trou (pas de plateforme)
                    if (Game1.random.NextDouble() < removalChance)
                        continue;

                    var paddle = new Paddle(PaddleType.SIMPLE, new Vector2(x, newY));
                    paddle.LoadContent(Game1.PublicContent);

                    bool overlap = paddles.Any(p => p.Hitbox().Intersects(paddle.Hitbox()));
                    if (!overlap)
                    {
                        paddles.Add(paddle);
                        currentHighestY = newY;
                        placed = true;

                        // bonus : plateforme cassable (ne remplace pas)
                        if (Game1.random.NextDouble() < breakableChance)
                        {
                            float offsetX = Game1.random.Next(-100, 100);
                            var breakable = new Paddle(
                                PaddleType.BREAKABLE,
                                new Vector2(Math.Clamp(x + offsetX, 0, Game1.ScreenDimensions.Width - 80),
                                newY - Game1.random.Next(10, 30))
                            );
                            breakable.LoadContent(Game1.PublicContent);
                            paddles.Add(breakable);
                        }
                    }
                }

                if (!placed)
                    currentHighestY -= baseSpacing;
            }
        }

        public bool IsGameOver()
        {
            float lowestPlatformY = paddles.Max(p => p.Position.Y);
            return player.Position.Y > lowestPlatformY + Game1.ScreenDimensions.Height / 2;
        }
    }
}
