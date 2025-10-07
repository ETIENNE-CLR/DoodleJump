using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Windows
{
    public class GameOverScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private Player player;

        // Constructeur de la classe...
        public GameOverScreen()
        {
            // player = new Player(new Vector2(platform.Position.X + 11, platform.Position.Y - 50), new Vector2(0, 10));
        }

        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            // throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            // throw new NotImplementedException();
        }
    }
}
