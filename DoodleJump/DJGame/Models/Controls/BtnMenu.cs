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
    internal class BtnMenu : Button
    {
        public BtnMenu(Action toDo, Vector2 position) : base(toDo, position)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            // Init
            base.LoadContent(content);
            texture = content.Load<Texture2D>("Controls/button");
            normalForm = new Rectangle(224, 82, 225, 82);
            clickedForm = new Rectangle(224, 0, 225, 82);
        }
    }
}
