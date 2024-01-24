using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using test.Projectiles;

namespace test.Projectiles
{
    public class EtherealLances: ModProjectile
    {
        int HitTimes = 0;
        public override void SetDefaults()
        {


            
            Projectile.ignoreWater = true;
            Projectile.light = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;
            Projectile.friendly = true;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 100;
     
            

            Projectile.arrow = true;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = 5; // or 1
            Projectile.DamageType = DamageClass.Ranged;
          
            AIType = ProjectileID.WoodenArrowFriendly;
           
        }
      
        public override void OnHitNPC(NPC target, int damage,float knockback,bool crit)//	Electrosphere
        {
            
            if (!target.townNPC && crit == true)
            {
                target.AddBuff(69, 300);
            }

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return Main.DiscoColor;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = -Projectile.velocity;
            Projectile.velocity *= 1.2f;
            HitTimes++;
            if(HitTimes==3)
            {
                return true;//碰撞五块超过三次就消失
            }

            return false;
        }








        // Additional hooks/methods here.
    }
    }