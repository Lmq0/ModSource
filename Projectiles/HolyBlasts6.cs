using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.DataStructures;
using Terraria.Localization;

using test.Buffs;
using test.Projectiles;

namespace test.Projectiles
{
    public class HolyBlasts6 : HolyBlasts
    {
        int proj;
  
        public override bool PreKill(int timeLeft)
        {
            Random ran = new Random();
            float BeginAngle = ran.Next(360);
            BeginAngle *= (3.14f / 180f);
            int i = 0;//计算生成了多少个弹幕
            while (i < 12)
            {
                Vector2 Center = Projectile.position + new Vector2(75f, 75f);
                proj=Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                Center + new Vector2((float)(75f * Math.Cos(BeginAngle + 6.28f * i / 12f)), ((float)(75f * Math.Sin(BeginAngle + 6.28f * i / 12f)))),
               new Vector2((float)(20f * Math.Cos(BeginAngle + 6.28f * i / 12f)), ((float)(20f * Math.Sin(BeginAngle + 6.28f * i / 12f)))),
               ModContent.ProjectileType<HolyFire>(), 50, 0f, Main.myPlayer, Projectile.ai[0], 0f);
                Main.projectile[proj].localAI[0]=Projectile.localAI[0];

                i++;
            }


            if (Main.dayTime)
                Projectile.type = ProjectileID.Daybreak;
            else
                Projectile.type = ProjectileID.LunarFlare;
            Projectile.scale=2.5f;
            return true;
        }
        
    }

}



        // Additional hooks/methods here.
    
