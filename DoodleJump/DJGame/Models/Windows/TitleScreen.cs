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
    public class TitleScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private BtnPlay btnplay;
        private BtnOptions btnOptions;

        // Constructeur de la classe...
        public TitleScreen()
        {
            int btnX = (Game1.ScreenDimensions.Width * 1 / 4) - 50;
            int btnSize = 85;

            // Bouton principales
            btnplay = new BtnPlay(new Action(() => { }), new Vector2(btnX, (Game1.ScreenDimensions.Height * 1 / 4) - 35), Vector2.Zero, btnSize, false, 0, false);
            btnOptions = new BtnOptions(new Action(() => { }), new Vector2(btnX, (Game1.ScreenDimensions.Center.Y * 3 / 5) + 10), Vector2.Zero, btnSize, false, 0, false);
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/View/main_menu");
            btnplay.LoadContent(content);
            btnplay.actionToDo = () =>
            {
                GameScreen gs = new GameScreen();
                gs.LoadContent(content);
                SceneManager.activeScene = gs;
            };

            btnOptions.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            btnplay.Update(gameTime);
            btnOptions.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawBackground(spriteBatch, gameTime);
            btnplay.Draw(spriteBatch, gameTime);
            btnOptions.Draw(spriteBatch, gameTime);
        }
    }
}
