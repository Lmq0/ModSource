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
    public class SolarBreak: ModProjectile
    {
        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
        
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 1.5f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 0;
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 150;



     
            Projectile.width = 100;
            Projectile.height = 30;
            Projectile.aiStyle = -1; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)		//	Electrosphere
        {


            target.AddBuff(BuffID.Daybreak, 300);
            target.AddBuff(ModContent.BuffType<ParadiseSanction>(), 500);
            
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
        public override void OnSpawn(IEntitySource source)
        {
           
            
        }
        public override void PostAI()
        {
            Projectile.velocity *= 1.01f;
            Projectile.rotation = Projectile.velocity.ToRotation();


            ;

        }
        public override void AI()
        {
            Dust.NewDust(Projectile.Center, 10, 10, DustID.SolarFlare,Projectile.velocity.X*-0.0001f);
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            if(!Main.dayTime)
            {
                texture=ModContent.Request<Texture2D>("test/Projectiles/NightFraction").Value;
                lightColor=Color.White;
                lightColor.A=0;
            }
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



        // Additional hooks/methods here.
    }
