using System;
using System.Collections.Generic;
using DJGame.Enum;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;

namespace DJGame.Models.Game
{
    public class Paddle : AnimatedElement, IMonogameElement
    {
        // Champs de la classe...
        public const int NORME_SIZE = 50;
        private PaddleType type;
        int baseSize;

        // Propriétés de la classe...
        public PaddleType Type { get => type; }

        // Constructeur de la classe...
        public Paddle(PaddleType type, Vector2 position, int sizePourcent = NORME_SIZE, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, new Vector2(), sizePourcent, flipped, rotation, showHitbox)
        {
            this.type = type;
            animationName = "idle";
            baseSize = sizePourcent;
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            // Chargement de la texture
            texture = content.Load<Texture2D>("Sprites/default");

            // Animations
            switch (type)
            {
                case PaddleType.SIMPLE:
                    animations["idle"] = new Animation(Animation.GenerateAnimation(114, 29, 2, 3, 0, 0, 1), 0, 0, false);
                    break;

                case PaddleType.BREAKABLE:
                    animations["idle"] = new Animation(new List<Rectangle>()
                    {
                        new Rectangle(5, 146, 115, 29)
                    }, 0, 0, false);

                    animations["break"] = new Animation(new List<Rectangle>()
                    {
                        new Rectangle(5, 185, 115, 35),
                        new Rectangle(5, 235, 115, 50),
                        new Rectangle(5, 300, 115, 60),
                    }, 330, 0, false);
                    break;
            }
            if (animations.Count == 0)
                throw new Exception("Aucune animation n'a été initialisée !");
        }

        public override void Update(GameTime gameTime)
        {
            // co
        }

        public void Scroll(float intensity)
        {
            position.Y += intensity;
        }
    }
}
