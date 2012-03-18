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
        private Vector2 Accel;
        private bool wasHit;
        private Vector2 _windForce;
        private Vector2 _gravity;
                
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
            wasHit = false;
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
                CooldownReady = false;
                time = 0;
                wasHit = true;
            }            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, RotationAngle, Origin, 1.0f, SpriteEffects.None, 0f);

            Texture2D blank = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
            DrawLine(spriteBatch, blank, 1, Color.Red, Position, Position + _windForce);
            DrawLine(spriteBatch, blank, 1, Color.Green, Position, Position + _gravity);
        }

        void DrawLine(SpriteBatch batch, Texture2D blank, float width, Color color, Vector2 point1, Vector2 point2)
        {
            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            batch.Draw(blank, point1, null, color, angle, Vector2.Zero, new Vector2(length, width), SpriteEffects.None, 0);
        }

        private void UpdateDamage()
        {
            Damage = (int)Math.Ceiling((Speed*40) + (Weight / 4));
        }

        public void Move(Vector2 center, GameTime gameTime)
        {
            if (isOrbiting)
            {
                //var windAccel = new Vector2((float)(radius * Math.Cos(theta)), (float)(radius * Math.Sin(theta)));
                //Velocity += center;
                //Position = Velocity;
                //Position.Normalize();
                const float MAX_VELOCITY = 10f;
                const float MAX_ACCEL = 1f;

                var a0 = Accel;
                var v0 = Velocity;
                var x0 = Position;

                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                var distance = (center - Position);



                _gravity = distance * .5f;
                //Accel = _gravity;
                //var repulse = distance * -1 * .1f;

                //if (distance.Length() < radius)
                //{
                //    var temp = 1f - (distance.Length() / radius);
                //    Accel += -Accel * (float)temp;
                //}
                _windForce = CalcWindForce(distance, 100);
                //Accel += wind;
                //Accel 
                //Accel += repulse;

                //if (Accel.Length() > MAX_ACCEL)
                //    Accel = a0;
                //Velocity += _windForce;
                Velocity += Accel * deltaTime;
                Velocity = ApplyDrag(Velocity);
                if (wasHit)
                    Velocity *= 0.3f;
                if (Velocity.Length() > MAX_VELOCITY)
                    Velocity = v0;

                Position += Velocity;
                wasHit = false;
            }
        }

        public Vector2 ApplyDrag(Vector2 velocity)
        {
             var drag = new Vector2(0.95f);
             return velocity *= drag;
        }

        public Vector2 CalcWindForce(Vector2 distance, float windMagnitude)
        {
          //  Vector2 windForce;
            //var xlength = storm.X - debrie.X;
            //var ylength = storm.Y - debrie.Y;
            var a1 = (float)Math.Atan((float)distance.X /(float)distance.Y);
            var windAngle = a1;
            //a1 = MathHelper.ToDegrees(a1);
            //var step = Math.Pow(distance.Y, 2) / Math.Pow(distance.Length(), 2);
            //var a2 = 90f - a1;

            //windForce = new Vector2((float)Math.Sin(MathHelper.ToRadians(a2)) * windMagnitude, (float)Math.Cos(MathHelper.ToRadians(a2)) * windMagnitude);
            var windForce = new Vector2((float)Math.Sin(windAngle) * 10f, (float)Math.Cos(windAngle) * 10f);
            return windForce;
        }

        public void StartOrbiting()
        {
            Random rand = new Random();

            isOrbiting = true;
            radius = rand.Next(25) + 30;
        }
    }
}
