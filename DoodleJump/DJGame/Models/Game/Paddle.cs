using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Cryptography.X509Certificates;
using DJGame.Enum;
using DJGame.Interfaces;
using DJGame.Models.Agents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
        private SoundEffect breakSoundEffect;
        private SoundEffectInstance breakSoundEffectInstance;

        // Propriétés de la classe...
        public PaddleType Type { get => type; }

        // Constructeur de la classe...
        public Paddle(PaddleType type, Vector2 position, int sizePourcent = NORME_SIZE, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, new Vector2(), sizePourcent, flipped, rotation, showHitbox)
        {
            this.type = type;
            animationName = "idle";
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            // Chargement de la texture
            texture = content.Load<Texture2D>("Sprites/default");
            breakSoundEffect = content.Load<SoundEffect>("Sounds/break");
            breakSoundEffectInstance = breakSoundEffect.CreateInstance();

            // Animations
            switch (type)
            {
                case PaddleType.SIMPLE:
                    animations["idle"] = new Animation(new List<Rectangle>()
                    {
                        new Rectangle(2, 3, 144, 29)
                    }, 0, 0, false);
                    break;

                case PaddleType.BREAKABLE:
                    animations["idle"] = new Animation(new List<Rectangle>()
                    {
                        new Rectangle(2, 146, 120, 30)
                    }, 0, 0, false);

                    animations["break"] = new Animation(new List<Rectangle>()
                    {
                        new Rectangle(2, 182, 121, 41),
                        new Rectangle(4, 232, 114, 55),
                        new Rectangle(3, 298, 116, 66),
                        new Rectangle(1210, 0, 116, 66),
                    }, 35, 0, false);
                    break;
            }
            if (animations.Count == 0)
                throw new Exception("Aucune animation n'a été initialisée !");
        }

        public override void Update(GameTime gameTime)
        {
            switch (Type)
            {
                case PaddleType.BREAKABLE:
                    if (animationName == "break") CurrentAnimationObject.Update(gameTime);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        public void Scroll(float intensity)
        {
            position.Y += intensity;
        }

        public void Break()
        {
            if (type != PaddleType.BREAKABLE) throw new Exception("La plateforme n'est pas cassable !");

            // Changement n'anim
            animationName = "break";
            if (breakSoundEffectInstance.State != SoundState.Playing)
            {
                breakSoundEffectInstance.Play();
            }
        }
    }
}
