using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*Projected Summary: 
 * Debris is attached to the storm.
 * It gains momentum slowly.
 * It loses momentum when it hits something.
 * If the the speed is below a certain point... it vaporizes or gets no clipping or something.
 * Damage is determined by speed and weight.
 */


namespace StormGame
{
    abstract class Debris
    {        
        public bool CooldownReady;
        private float Speed;
        private float Weight;
        public Texture2D Texture;
        private float Cooldown;
        private float time;
        private double radius;
        public bool isOrbiting;
        private float RotationAngle;
        private Vector2 Origin;
        private Vector2 Velocity;
        private float theta;
        private const float WindAcceleration = 0.0003f;
                
        public Rectangle BoundingBox { get; set;  }
                
        public Vector2 Position { get; set; }
        public int Damage { get; set; }


        public Debris()
        {

        }

        public void Initialize()
        {
            BoundingBox = new Rectangle();
            isOrbiting = false;
            CooldownReady = true;
            Speed = 0.07f;
            Weight = 10;
            time = 0.0f;
            RotationAngle = 0f;
            Origin.X = Texture.Width / 2;
            Origin.Y = Texture.Height / 2;

            Velocity = new Vector2(0, 0);

            //temp value
            radius = 0;

            UpdateDamage();
        }

        public void Update(GameTime gameTime, Storm storm)
        {
            if (isOrbiting)
            {
                time += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Speed += WindAcceleration;
                RotationAngle = (RotationAngle + -0.02f) % (MathHelper.Pi * 2);
                UpdateDamage();

                if (!CooldownReady && time > Cooldown)
                {
                    CooldownReady = true;
                }
                theta = (theta + Speed) % 360;
                BoundingBox = new Rectangle((int)Position.X, (int)Position.Y, BoundingBox.Width, BoundingBox.Height);
            }
            else
            {
                storm.AttachNearbyDebris(this);                
            }

        }

        public void Collide()
        {
            if (CooldownReady)
            {                
                Cooldown = 1.5f;  
                Speed *= 0.3f;
                CooldownReady = false;
                time = 0;
            }            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, RotationAngle, Origin, 1.0f, SpriteEffects.None, 0f);
        }

        private void UpdateDamage()
        {
            Damage = (int)Math.Ceiling((Speed*40) + (Weight / 4));
        }

        public void Move(Vector2 center, GameTime gameTime)
        {
            if (isOrbiting)
            {
                //Velocity = new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta)));
                //Velocity += center;
                //Position = Velocity;
                //Position.Normalize();
                var x0 = Position;
                var v0 = Velocity;
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var a = new Vector2(50, 0);

                Position = new Vector2(.5f * a.X * (float)Math.Pow(deltaTime, 2) + v0.X * deltaTime + Position.X, Position.Y);

                Velocity = new Vector2(a.X * deltaTime + Velocity.X, Velocity.Y);


            }
        }

        public void StartOrbiting()
        {
            Random rand = new Random();

            isOrbiting = true;
            radius = rand.Next(25) + 30;
        }

    }
}
