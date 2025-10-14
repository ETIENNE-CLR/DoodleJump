using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DJGame.Models.Windows
{
    public class GameOverScreen : MonogameWindow, IMonogameElement
    {
        // Champs de la classe...
        private SpriteFont font;
        private Player player;
        private Song dieSoundEffect;
        private bool soundPlayed;

        // Constructeur de la classe...
        public GameOverScreen()
        {
            player = new Player(new Vector2(0, 0));
            soundPlayed = false;
        }

        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            dieSoundEffect = content.Load<Song>("Sounds/fall");
            player.LoadContent(content);
            font = content.Load<SpriteFont>("monogameText");
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            player.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            if (!soundPlayed)
            {
                MediaPlayer.Play(dieSoundEffect);
                soundPlayed = true;
            }
        }

        public void UpdatePlayerPosition(Vector2 position)
        {
            player.SetPosition(position);
        }
    }
}
