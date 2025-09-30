using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace DJGame.Models.Controls
{
    public class BtnOptions : Button
    {
        public BtnOptions(Action toDo, Vector2 position, Vector2 velocity, int sizePourcent = 100, bool flipped = false, float rotation = 0, bool showHitbox = false) : base(toDo, position, velocity, sizePourcent, flipped, rotation, showHitbox)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            // Init
            texture = content.Load<Texture2D>("Controls/button");
            normalForm = new Rectangle(0, 326, 223, 80);
            clickedForm = new Rectangle(224, 246, 222, 80);
        }
    }
}
