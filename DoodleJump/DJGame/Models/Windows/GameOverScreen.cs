using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using DJGame.Models.Controls;
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
        private Player player;
        private Song dieSoundEffect;
        private bool soundPlayed;
        
        private GameScreen gscreen;
        private BtnPlay btnPlay;

        private BtnMenu btnMenu;
        private TitleScreen ttlscreen;

        // Constructeur de la classe...
        public GameOverScreen()
        {
            // ttlscreen = new TitleScreen();
            // gscreen = new GameScreen();

            soundPlayed = false;
            player = new Player(new Vector2(0, 0));

            btnMenu = new BtnMenu(new Action(() =>
            {
                Game1.activeScene = ttlscreen;
            }), new Vector2(0, 0));
            btnPlay = new BtnPlay(new Action(() =>
            {
                Game1.activeScene = gscreen;
            }), new Vector2(0, 0));
        }

        public override void LoadContent(ContentManager content)
        {
            bgTexture = content.Load<Texture2D>("Backgrounds/Game/default");
            dieSoundEffect = content.Load<Song>("Sounds/fall");
            player.LoadContent(content);
            // ttlscreen.LoadContent(content);
            // gscreen.LoadContent(content);

            btnPlay.LoadContent(content);
            btnMenu.LoadContent(content);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            player.Draw(spriteBatch, gameTime);
            btnMenu.Draw(spriteBatch, gameTime);
            btnPlay.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);
            if (!soundPlayed)
            {
                soundPlayed = true;
                MediaPlayer.Play(dieSoundEffect);
            }
        }

        public void UpdatePlayerPosition(Vector2 position)
        {
            player.SetPosition(position);
        }
    }
}
