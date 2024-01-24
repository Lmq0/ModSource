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
using test.NPCs.GoddnessProvidence;

using test.Buffs;
using test.Projectiles;

namespace test.Projectiles
{
    public class HealCircle:ModProjectile
    {
        public Player player;//玩家
        public NPC Master;//boss本体
        public NPC Partner1;
        public NPC Partner2;
        public NPC Partner3; 

        public override void SetDefaults()
        {
            Projectile.scale = 1.5f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 150;
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 1000;
            Projectile.damage=0;


            Projectile.width = 450;
            Projectile.height = 450;
            Projectile.aiStyle = -1; // or 9
            Projectile.DamageType = DamageClass.Default;

            base.SetDefaults();

        }
        public override bool ShouldUpdatePosition()
        {
            return false;//位置不因为velocity改变
        }
        public void initialize()
        {

            player= Main.player[Projectile.owner];
            int i = 0;
            while (i<Main.npc.Length)
            {
                if (Main.npc[i].active)
                {
                    if (Main.npc[i].type==ModContent.NPCType<ExProfanedGoddness>())
                        Master=Main.npc[i];
                    else if(Main.npc[i].type==ModContent.NPCType<GuardianCommander>())
                        Partner1= Main.npc[i];  
                    else if(Main.npc[i].type==ModContent.NPCType<GuardianHealer>())
                        Partner2 = Main.npc[i]; 
                    else if(Main.npc[i].type==ModContent.NPCType<GuardianDefender>())
                        Partner3= Main.npc[i];

                }
               
                i++;
            }



        }
        public override void AI()
        {
          
            if(Projectile.frameCounter%10==9)
            {
                if (Master.active)
                {
                    Master.life+=1000;
                    Master.HealEffect(1000);
                }
                if (Partner1.active)
                {
                    Partner1.life+=1000;
                    Partner1.HealEffect(1000);
                }
                if (Partner2.active)
                {
                    Partner2.life+=1000;
                    Partner2.HealEffect(1000);
                }
                else Projectile.Kill();
                if(Partner3.active)
                {
                    Partner3.life+=1000;
                    Partner3.HealEffect(1000);
                }
            }
            Projectile.owner=Main.myPlayer;
            Projectile.velocity*=0f;
            Projectile.Center=Master.Center;
            Projectile.rotation-=0.03f;
            Projectile.rotation+=(2f*(float)Math.PI);
            Projectile.rotation%=(2f*(float)Math.PI);//旋转
            base.AI();
            Projectile.frameCounter++;
            
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }
        public override void OnSpawn(IEntitySource source)
        {
            initialize();
 
            base.OnSpawn(source);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            damage=0;

            //  if(Projectile.frameCounter%100==0&&(target.type==ModContent.NPCType<ExProfanedGoddness>()
            //     ||target.type==ModContent.NPCType<ProfanedRocks>()
            //     ||target.type==ModContent.NPCType<GuardianCommander>()
            //     ||target.type==ModContent.NPCType<GuardianDefender>()
            //     ||target.type==ModContent.NPCType<GuardianHealer>()))
            //  {
            //if (target.life<target.lifeMax)
            // {
            //    target.HealEffect(250, true);
            //    target.life+=250;
           //  }
           // if (target.life>target.lifeMax)
           //     target.life=target.lifeMax; 
             
            //能治疗所有敌怪
          //  }
           
           
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            damage=2;
         
            base.OnHitPlayer(target, damage, crit);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            if (!Main.dayTime)
            {
                texture=ModContent.Request<Texture2D>("test/Projectiles/HealCircle_Night").Value;
                lightColor = Color.White;
                lightColor.A=0;
            }
            Rectangle rectangle = new Rectangle(
                0,
                texture.Height / Main.projFrames[Type] * Projectile.frame,
                texture.Width,
                texture.Height / Main.projFrames[Type]
                );
            Main.EntitySpriteDraw(  
              texture,
              Projectile.Center - Main.screenPosition,
              rectangle,
              lightColor,
              Projectile.rotation,
              new Vector2(texture.Width / 2, texture.Height / 2),
              new Vector2(Projectile.scale, Projectile.scale),
              Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None,
              0
              );

            return false;
        }

    }
}
