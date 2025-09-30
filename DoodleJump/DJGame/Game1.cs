using System;
using System.Collections.Generic;
using DJGame.Models.Game;
using DJGame.Models.Windows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;

namespace DJGame
{
    public class Game1 : Game
    {
        // Champs de la classe...
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private static Rectangle screenDimensions;
        private static Texture2D whitePixel;
        public static MonogameWindow activeScene;

        // Propriétés de la classe...
        public static Camera2D Camera { get; private set; }
        public static Rectangle ScreenDimensions { get => screenDimensions; }
        public static Texture2D WhitePixel { get => whitePixel; }
        public static Random random { get; set; }
        public static ContentManager PublicContent { get; private set; }

        // Constructeur de la classe...
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            random = new Random();
            IsMouseVisible = true;
        }

        // Méthodes de la classe...
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.Window.Title = "Doodle Jump";
            ChangeScreenDimensions(520);
            activeScene = new TitleScreen();
            Camera = new Camera2D(Game1.screenDimensions.Height);

            base.Initialize();
        }

        private void ChangeScreenDimensions(int baseWidth = 640)
        {
            _graphics.PreferredBackBufferWidth = baseWidth;
            _graphics.PreferredBackBufferHeight = (int)((double)(baseWidth * 3.5) / 2);
            Game1.screenDimensions = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _graphics.ApplyChanges();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            whitePixel = new Texture2D(GraphicsDevice, 1, 1);
            whitePixel.SetData(new[] { Color.White });
            activeScene.LoadContent(Content);
            PublicContent = Content;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            activeScene.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            // Dessiner le backgroud
            _spriteBatch.Begin();
            activeScene.DrawBackground(_spriteBatch, gameTime);
            _spriteBatch.End();

            // Dessiner le jeu
            if (activeScene is not GameScreen)
                _spriteBatch.Begin();
            else
                _spriteBatch.Begin(transformMatrix: Camera.Transform);
            activeScene.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
