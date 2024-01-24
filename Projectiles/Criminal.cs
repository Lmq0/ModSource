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
    public class Criminal:ModProjectile
    {
        const float a = 0.4f;
        bool state = true;//true去   false回
        Vector2 Distance = new Vector2(0, 0);
        Player master => Main.player[Main.myPlayer];
        public override void SetDefaults()
        {
            Projectile.damage= 8180;
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 0;
            Projectile.friendly = true;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 1000;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1; //
            Projectile.DamageType = DamageClass.Ranged;


        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Main.NewText("hit", Color.Wheat);
           damage=8180;
        }
        public override bool PreAI()
        {
            return true;
        }
        public override void AI()
        {
            if (Projectile.frameCounter==1)
            {
                Distance=Projectile.velocity*-1f;
                Distance.Normalize();
            }
            float v = (float)(Math.Pow((double)Projectile.velocity.X, 2.0)+Math.Pow((double)Projectile.velocity.Y, 2.0));
            if (state)
                Projectile.velocity*=(1-a/Projectile.velocity.Length());
            else
            {
                Distance=master.Center-Projectile.Center;
                if (Distance.Length()<7f)
                    Projectile.timeLeft=0;
                Distance.Normalize();
                Projectile.velocity=Distance*20f;
               
            }


            if(Projectile.frameCounter%20==0)
            {
                
            }
            if(v<4)
            {
                state=false;
               
            }


            Projectile.rotation+=0.5F;
            

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Main.EntitySpriteDraw(
                 texture,
                 Projectile.Center - Main.screenPosition,
                 rectangle,
                 lightColor,
                 Projectile.rotation,
                new Vector2(texture.Width / 2, texture.Height / 2),
                new Vector2(Projectile.scale, Projectile.scale),
                SpriteEffects.None, 0);
            return false;
        }




    }
}
