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
    public class BtnPlay : Button
    {
        public BtnPlay(Action toDo, Vector2 position) : base(toDo, position)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            // Init
            texture = content.Load<Texture2D>("Controls/button");
            normalForm = new Rectangle(448, 240, 222, 80);
            clickedForm = new Rectangle(448, 160, 222, 80);
        }
    }
}
