using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace StormGame
{
    class MediumBuilding : Destructible
    {
        private Texture2D tex1;
        private Texture2D tex2;
        private Texture2D tex3;

        private List<Texture2D> textures;
        private int health;
        public MediumBuilding(ContentManager content, Vector2 pos) : base()
        {
            textures = new List<Texture2D>();

            tex1 = content.Load<Texture2D>("MedBldgGood");
            tex2 = content.Load<Texture2D>("MedBldgPoor");
            tex3 = content.Load<Texture2D>("MedBldgDead");
            textures.Add(tex1);
            textures.Add(tex2);
            textures.Add(tex3);
            health = 50;

            this.LoadContent(content, textures, pos, health);
        }
        
    }
}
