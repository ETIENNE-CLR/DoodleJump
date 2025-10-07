using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TitleScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private BtnPlay btnPlay;
        private BtnOptions btnOptions;
        private Paddle platform;
        private Player agent;

        // Constructeur de la classe...
        public TitleScreen()
        {
            int btnX = (Game1.ScreenDimensions.Width * 1 / 4) - 50;
            int btnSize = 85;

            // Bouton principales
            btnPlay = new BtnPlay(new Action(() => { }), new Vector2(btnX, (Game1.ScreenDimensions.Height * 1 / 4) - 35), Vector2.Zero, btnSize, false, 0, false);
            btnOptions = new BtnOptions(new Action(() => { }), new Vector2(btnX, (Game1.ScreenDimensions.Center.Y * 3 / 5) + 10), Vector2.Zero, btnSize, false, 0, false);
            platform = new Paddle(PaddleType.SIMPLE, new Vector2((Game1.ScreenDimensions.Width * 1 / 4) - 60, Game1.ScreenDimensions.Height * 3 / 4), 55);
            agent = new Player(new Vector2(platform.Position.X + 11, platform.Position.Y - 50), new Vector2(0, 10));
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            // Base
            bgTexture = content.Load<Texture2D>("Backgrounds/View/main_menu");
            platform.LoadContent(content);
            agent.LoadContent(content);

            // Boutons
            btnOptions.LoadContent(content);
            btnPlay.LoadContent(content);
            btnPlay.actionToDo = () =>
            {
                GameScreen gs = new GameScreen();
                gs.LoadContent(content);
                Game1.activeScene = gs;
            };
        }

        public override void Update(GameTime gameTime)
        {
            // Main
            agent.Update(gameTime);
            btnPlay.Update(gameTime);
            btnOptions.Update(gameTime);

            // Collisions
            if (platform.Hitbox().Intersects(agent.Hitbox()) && agent.Velocity.Y > 0)
                agent.Jump();
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            platform.Draw(spriteBatch, gameTime);
            agent.Draw(spriteBatch, gameTime);
            btnPlay.Draw(spriteBatch, gameTime);
            btnOptions.Draw(spriteBatch, gameTime);
        }
    }
}
