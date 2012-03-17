using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace StormGame
{
    class LargeDebris : Debris
    {
        public LargeDebris() : base()
        {
            
            Texture = Globals.Content.Load<Texture2D>("debris1");
            this.Initialize();
        }
    }
}
