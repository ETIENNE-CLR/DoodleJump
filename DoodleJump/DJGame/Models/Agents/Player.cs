using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DJGame.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace DJGame.Models.Agents
{
    internal class Player : AnimatedElement, IMonogameElement
    {
        // Champs de la classe...
        private bool isJumping;
        private float gravityEnv;
        private float jumpForce;
        private bool isFalling;

        // Constructeur de la classe...
        public Player(Vector2 position, Vector2 velocity, int sizePourcent = 100, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(position, velocity, sizePourcent, flipped, rotation, showHitbox)
        {
            isJumping = false;
            gravityEnv = 0.5f;
            jumpForce = 10;
            isFalling = false;
        }

        // Méthodes de la classe...
        public override void LoadContent(ContentManager content)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
