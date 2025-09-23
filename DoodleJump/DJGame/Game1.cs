using System.Collections.Generic;
using DJGame.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DJGame
{
    public class Game1 : Game
    {
        // Champs de la classe...
        private static Rectangle screenDimensions;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Propriétés de la classe...
        public static Rectangle ScreenDimensions { get => screenDimensions; }

        // Constructeur de la classe...
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        // Méthodes de la classe...
        private void ChangeScreenDimensions(int baseWidth = 640)
        {
            _graphics.PreferredBackBufferWidth = baseWidth;
            _graphics.PreferredBackBufferHeight = ((baseWidth * 3) / 2);
            Game1.screenDimensions = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.Window.Title = "Doodle Jump";
            ChangeScreenDimensions(520);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            SceneManager.activeScene = null;
            SceneManager.activeScene.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            SceneManager.activeScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            SceneManager.activeScene.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
