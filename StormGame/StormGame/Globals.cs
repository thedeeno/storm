using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    static class Globals
    {
        public static SpriteFont Font1 { get; set; }
        //public static GameTime gameTime { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }
        public static ContentManager Content { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
    }
}
