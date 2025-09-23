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
        private BtnPlay btnJouer;

        // Constructeur de la classe...
        public TitleScreen()
        {
            this.btnJouer = new BtnPlay(new Action(() =>
            {
                // tmp
            }), new Vector2(Game1.ScreenDimensions.Center.X + 50, Game1.ScreenDimensions.Center.Y + 200), Vector2.Zero, 65, false, 0, true);
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("default");
            btnJouer.LoadContent(content);
            btnJouer.actionToDo = () =>
            {
                GameScreen gs = new GameScreen();
                gs.LoadContent(content);
                SceneManager.activeScene = gs;
            };
        }

        public override void Update(GameTime gameTime)
        {
            btnJouer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            base.DrawBackground(spriteBatch, gameTime);
            btnJouer.Draw(spriteBatch, gameTime);
        }
    }
}
