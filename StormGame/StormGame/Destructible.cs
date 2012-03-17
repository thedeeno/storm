using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    abstract class Destructible
    {
        private List<Texture2D> textures;
        private Texture2D CurrentTexture;
        private Vector2 Position;
        private int Health;
        private int CurrentHealth;
        private bool IsAlive;
        private Rectangle BoundingBox;
        private List<DamagePopup> DamagePopups;
        

        public void LoadContent(ContentManager Content, List<Texture2D> texArray, Vector2 pos, int health)
        {
            IsAlive = true;
            textures = new List<Texture2D>();
            DamagePopups = new List<DamagePopup>();

            for (int i = 0; i < 3; i++)
            {
                textures.Add(texArray[i]);
            }

            BoundingBox = new Rectangle((int)pos.X, (int)pos.Y, textures[0].Bounds.Width, textures[0].Bounds.Height);
            Health = CurrentHealth = health;
            Position = pos;
        }

        public bool CheckCollision(Rectangle r1)
        {
            bool isColliding = r1.Intersects(BoundingBox);
            if (IsAlive)
            {
                return isColliding;
            } 
            else
                return false;
        }

        public void DamageHealth(int Damage, Vector2 pos)
        {
            CurrentHealth -= Damage;
            DamagePopup dp = new DamagePopup(Damage, pos);
            DamagePopups.Add(dp);
        }

        public void Update(Level level, GameTime gameTime)
        {
            if (IsAlive)
            {
                if ((float)CurrentHealth / (float)Health > 0.4f)
                {
                    CurrentTexture = textures[0];
                }
                else if ((float)CurrentHealth / (float)Health > 0)
                {
                    CurrentTexture = textures[1];
                }
                else
                {
                    CurrentTexture = textures[2];
                }
                if (CurrentHealth <= 0)
                {
                    IsAlive = false;
                    level.SpawnDebris(Position);
                }
            }
            for (int i = DamagePopups.Count; i > 0; i--)
                {
                    DamagePopups[i - 1].Update(gameTime);
                    if (!DamagePopups[i - 1].isAlive)
                        DamagePopups.Remove(DamagePopups[i - 1]);
                }
        }

        

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentTexture, new Vector2((float)Position.X, (float)Position.Y), Color.White);

            foreach (DamagePopup dp in DamagePopups)
            {
                dp.Draw();
            }

        }
    }
}
