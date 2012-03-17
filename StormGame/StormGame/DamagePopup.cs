using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace StormGame
{
    class DamagePopup
    {
        private string DamageString;
        private Vector2 Position;
        private const float FadeTime = 2.0f;
        public bool isAlive;
        private float time;

        public DamagePopup(int damage, Vector2 pos)
        {
            DamageString = damage.ToString();
            Position = pos;
            isAlive = true;
            time = 0.0f;
        }

        public void Update(GameTime gameTime)
        {
            Position.Y -= 0.4f;

            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > FadeTime)
            {
                isAlive = false;
            }
        }

        public void Draw()
        {
            Globals.SpriteBatch.DrawString(Globals.Font1, DamageString, Position, Color.Red);
        }

    }
}
