using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace StormGame
{
    class Storm
    {
        private Rectangle BoundingBox;
        private Texture2D imageTexture;
        private List<Debris> Satallites;
        public Vector2 Position;

        private KeyboardState keyboardState;

        private float _Xspeed;
        private float _Yspeed;

        private const float OrbitPullRange = 100;

        public Storm(ContentManager Content)
        {
            Satallites = new List<Debris>();
            
            imageTexture = Content.Load<Texture2D>("Circle");
            BoundingBox = new Rectangle(100, 100, imageTexture.Width, imageTexture.Height);
            Position = new Vector2(100, 200);

        }

        public void Update(List<Destructible> ListOfDestructibles, GameTime gameTime)
        {
            GetInput();            
            MoveStorm();     
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(imageTexture, Position, new Color(255,255,255,50));
        }

        private void MoveStorm()
        {
            if ((BoundingBox.Left + _Xspeed) <= 0 || (BoundingBox.Right + _Xspeed) >= 800)
            { 
            }
            else
            {
                Position.X += (int)_Xspeed;
            }
            if ((BoundingBox.Top + _Yspeed) <= 0 || (BoundingBox.Bottom + _Yspeed) >= 480)
            {
            }
            else
            {
                Position.Y += (int)_Yspeed;
            }
            BoundingBox.X = (int)Position.X;
            BoundingBox.Y = (int)Position.Y;

        }

        private void GetInput()
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Right))
                _Xspeed += 0.1f;

            else if (keyboardState.IsKeyDown(Keys.Left))
                _Xspeed += -0.1f;

            else
            {
                _Xspeed += 0.0f;
            }

            if (keyboardState.IsKeyDown(Keys.Up))
                _Yspeed += -0.1f;

            else if (keyboardState.IsKeyDown(Keys.Down))
                _Yspeed += 0.1f;

            else
            {
                _Yspeed += 0.0f;
            }

        }

        public Vector2 GetCenter()
        {
            Vector2 center = new Vector2(BoundingBox.X + imageTexture.Width/2, BoundingBox.Y + imageTexture.Height/2);

            return center;
        }

        public void AddSatallite(Debris debris)
        {
            debris.StartOrbiting();
            Satallites.Add(debris);
        }

        public void AttachNearbyDebris(Debris debris)
        {
            Vector2 Distance = Vector2.Subtract(debris.Position, Position);
            if (Distance.Length() <= OrbitPullRange)
            {                
                AddSatallite(debris);
            }
        }


    }
}
