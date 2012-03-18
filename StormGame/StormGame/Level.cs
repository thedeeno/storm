using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    class Level
    {
        Storm storm;
        List<Destructible> destructibles;
        List<Debris> debris;
        private Debris StartingDebris;


        public Level()
        {

        }

        public void LoadContent(ContentManager Content)
        {
            storm = new Storm(Content);
            destructibles = new List<Destructible>();
            debris = new List<Debris>();

            MediumBuilding medbdg1 = new MediumBuilding(Content, new Vector2(300, 300));
            MediumBuilding medbdg2 = new MediumBuilding(Content, new Vector2(450, 300));
            MediumBuilding medbdg3 = new MediumBuilding(Content, new Vector2(370, 300));
            MediumBuilding medbdg5 = new MediumBuilding(Content, new Vector2(400, 100));
            MediumBuilding medbdg4 = new MediumBuilding(Content, new Vector2(220, 100));
            MediumBuilding medbdg6 = new MediumBuilding(Content, new Vector2(300, 100));
            destructibles.Add(medbdg1);
            destructibles.Add(medbdg2);
            destructibles.Add(medbdg3);
            destructibles.Add(medbdg4);
            destructibles.Add(medbdg5);
            destructibles.Add(medbdg6);

            StartingDebris = new LargeDebris();
            StartingDebris.Position = new Vector2(250, 100);
            debris.Add(StartingDebris);
            StartingDebris.StartOrbiting();

            Debris d1 = new LargeDebris();
            d1.Position = new Vector2(200, 400);
            debris.Add(d1);

            //Debris d2 = new LargeDebris();
            //d2.Position = new Vector2(300, 300);
            ////debris.Add(d2);

            
        }

        public void Update(GameTime gameTime)
        {
            storm.Update(destructibles, gameTime);
            foreach (Destructible d in destructibles)
            {
                d.Update(this, gameTime);
            }
            UpdateDebris(destructibles, gameTime);
        }


        public void Draw()
        {
            foreach (Destructible d in destructibles)
            {
                d.Draw(Globals.SpriteBatch);
            }
            foreach (Debris d in debris)
            {
                d.Draw(Globals.SpriteBatch);
            }
            storm.Draw(Globals.SpriteBatch);
        }


        public void AddDebris(Vector2 pos)
        {
            Debris deb = new LargeDebris();
            deb.Position = pos;
            debris.Add(deb);
        }

        private void UpdateDebris(List<Destructible> listofDestros, GameTime gameTime)
        {
            foreach (Debris deb in debris)
            {
                deb.Update(gameTime, storm);

                deb.Move(storm.GetCenter(), gameTime);
                foreach (Destructible destructible in listofDestros)
                {
                    if (destructible.CheckCollision(deb.BoundingBox) && deb.CooldownReady)
                    {
                        destructible.DamageHealth(deb.Damage, new Vector2(deb.BoundingBox.X, deb.BoundingBox.Y));
                        deb.Collide();
                    }
                }
            }
        }

        public void SpawnDebris(Vector2 origin)
        {
            Random rand = new Random();
            Vector2 position = new Vector2(rand.Next(400)-200, rand.Next(400)-200);

            position += origin;

            Debris newdebris = new LargeDebris();
            newdebris.Position = position;

            debris.Add(newdebris);
        }
    }
}
