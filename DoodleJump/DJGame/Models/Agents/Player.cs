using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using DJGame.Models.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DJGame.Models.Agents
{
    public class Player : AnimatedElement, IMonogameElement
    {
        // Champs de la classe...
        private float gravityEnv;
        private float jumpForce;
        private List<Projectile> shoots;
        private int score;

        // Propriétés de la classe...
        public List<Projectile> Shoots { get => shoots; }
        public int Score { get => score; }

        // Constructeur de la classe...
        public Player(Vector2 position, Vector2 velocity, int sizePourcent = 100, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, velocity, sizePourcent, flipped, rotation, showHitbox)
        {
            gravityEnv = 0.25f;
            jumpForce = velocity.Y;
            animationName = "idle";
            shoots = new List<Projectile>();
            score = 0;
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            // Init
            texture = content.Load<Texture2D>("Sprites/player");

            // Animations
            float intervalAnim = 350;
            animations["idle"] = new Animation(new List<Rectangle>
            {
                new Rectangle(16, 15, 46, 45)
            }, 0, 0, false);
            animations["jump"] = new Animation(new List<Rectangle>
            {
                new Rectangle(78, 19, 46, 41)
            }, intervalAnim, 0, false);

            animations["shoot"] = new Animation(new List<Rectangle>
            {
                new Rectangle(15, 62, 30, 58)
            }, 0, 0, false);
            animations["shoot_jump"] = new Animation(new List<Rectangle>
            {
                new Rectangle(77, 64, 30, 58)
            }, intervalAnim, 0, false);
        }

        public override void Update(GameTime gameTime)
        {
            // Contrôles
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Left) && Position.X > Game1.ScreenDimensions.X)
            {
                this.position.X -= this.velocity.X;
                flipped = true;
            }
            if (kstate.IsKeyDown(Keys.Right) && (Position.X + Hitbox().Width) < Game1.ScreenDimensions.Width)
            {
                this.position.X += this.velocity.X;
                flipped = false;
            }

            if (kstate.IsKeyDown(Keys.Up))
            {
                animationName = $"shoot{(animationName.Contains("jump") ? "_jump" : "")}";
                // Shoot();
            }

            // gravité
            velocity.Y += gravityEnv;
            const int maxFallSpeed = 15;
            velocity.Y = Math.Min(velocity.Y, maxFallSpeed);
            position.Y += velocity.Y;
            if (Position.Y > Game1.Camera.Position.Y)
                score += (int)Math.Min(velocity.Y, maxFallSpeed);

            // Gestion des animations
            CurrentAnimationObject.Update(gameTime);
            if (animationName.Contains("jump") && CurrentAnimationObject.Finished)
            {
                animationName = animationName.Contains("shoot") ? "shoot" : "idle";
            }
        }

        public void Jump()
        {
            animationName = "jump";
            velocity.Y = -jumpForce;
        }

        private void Shoot()
        {
            Vector2 positionNewProjectile = Position;
            shoots.Add(new Projectile(positionNewProjectile, false));
        }
    }
}
