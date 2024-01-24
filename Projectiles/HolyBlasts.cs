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
    public class HolyBlasts: ModProjectile
    {
        int proj;
        private bool If_begin = true;
        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
        
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 0f;
            Projectile.ignoreWater = true;
            Projectile.light = -1f;//亮度
            Projectile.alpha = 200;
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
     
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 75;



     
            Projectile.width = 150;
            Projectile.height = 150;
            Projectile.aiStyle = -1; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)		//	Electrosphere
        {

            target.AddBuff(ModContent.BuffType<HolyFlames>(), 500);
            
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            return true;
        }
        public override bool PreKill(int timeLeft)
        {
            Random ran = new Random();
            float BeginAngle = ran.Next(360);
            BeginAngle *= (3.14f / 180f);
            int i = 0;//计算生成了多少个弹幕
            while (i < 1)
            {
          
                proj=Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                Projectile.Center,
                Projectile.velocity*(-0.9f), ModContent.ProjectileType<HolyFire>(), 50, 0f, Main.myPlayer);
                Main.projectile[proj].localAI[0]=Projectile.localAI[0];

                i++;
            }


            if (Main.dayTime)
                Projectile.type = ProjectileID.Daybreak;
            else
                Projectile.type = ProjectileID.LunarFlare;
            return true;
        }
        public override void Kill(int timeleft)
        {
            ;
        }
        public override bool PreAI()
        {
            return true;
        }
    
        public override void AI()
        {
            Random ran = new Random();
            if (Projectile.scale < 1f && If_begin)
            {

                Projectile.scale += 0.2f;
                if (Projectile.scale == 1f)
                    If_begin = false;
            }
            if (Projectile.light < 2f)
            {
                Projectile.light += 0.35f;
            }
            else
            {
                Projectile.light = 2f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            //无翻转
            // Projectile.velocity *= 0f;
            Projectile.rotation = Projectile.velocity.ToRotation();
            Projectile.frameCounter++;


            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0]==1)
                lightColor.A=(byte)(lightColor.A/2F);
            Texture2D texture;
            if (Main.dayTime==true)
            {
                texture = TextureAssets.Projectile[Type].Value;
                Projectile.light=1f;
            }
            else
            {
                texture= ModContent.Request<Texture2D>("test/Projectiles/HolyBlastsNight").Value;
                lightColor=Color.White;
                lightColor.A=0;
                Projectile.light=2f;
            }
            //夜晚特效
            int counter = (Projectile.frameCounter / 7) % (4);
           
            Rectangle rectangle = new Rectangle(
                   0,
                   texture.Height * counter / Main.projFrames[Type],
                   texture.Width,
                   texture.Height / Main.projFrames[Type]
                   );
            //+ new Vector2(50f*((float)Math.Sin(Projectile.rotation)+1)- 1.5f*225f*(float)Math.Sin(Projectile.rotation), 1.5f*154f*(float)Math.Cos(Projectile.rotation))
            Main.EntitySpriteDraw(
                       texture,
                       Projectile.Center - Main.screenPosition + new Vector2((float)Math.Cos(Projectile.rotation)*texture.Width/4f, (float)Math.Sin(Projectile.rotation)*texture.Height / 16f),
                       rectangle,
                       lightColor*(Projectile.alpha/255f),
                       Projectile.rotation,
                      new Vector2(texture.Width / 2, texture.Height / 8),
                      new Vector2(Projectile.scale, Projectile.scale),
                      SpriteEffects.None, 0);
           
            return false;
        }
    
    }



        // Additional hooks/methods here.
    }
