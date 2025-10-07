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

namespace DJGame.Models.Game
{
    public class Projectile : AnimatedElement, IMonogameElement
    {
        // Constructeur de la classe...
        public Projectile(Vector2 position, bool showHitbox = false) : base(position, new Vector2(0, 750), 100, false, 0, showHitbox)
        {
            animationName = "normal";
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("Sprites/projectile");
            animations["normal"] = new Animation(new List<Rectangle>()
            {
                new Rectangle(0, 0, 13, 11)
            }, 0, 0, false);
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            position.Y -= velocity.Y * dt;
        }
    }
}
