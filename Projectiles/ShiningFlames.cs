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
    public class ShiningFlames : ModProjectile
    {
        int proj;
        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {

            Main.projFrames[Type] = 4;
            //ProjectileID.Sets.TrailingMode[Type] = 2;
            //ProjectileID.Sets.TrailCacheLength[Type] = 5;

            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.alpha = 240;
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物

            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 400;




            Projectile.width = 45;
            Projectile.height = 88;
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
            Projectile.velocity *= 0.98f;
            Projectile.rotation = 0;


            ;

        }
        public override void AI()
        {

            //无翻转
            
            if (Projectile.frameCounter%200==199)
            {
                Random ran = new Random();
                float velocity = ran.Next(15)+2f;
                Vector2 target = -Projectile.Center+player.Center;
                target.Normalize();
                target *= velocity;
                proj=Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, target,
                    ModContent.ProjectileType<HolyFire>(), 25, 0, 255);
                Main.projectile[proj].localAI[0]=Projectile.localAI[0];
            }    
            Projectile.frameCounter++;


            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0]==1)
                lightColor.A=30;
            Texture2D texture;

            if (Main.dayTime==true)
            {
                texture = TextureAssets.Projectile[Type].Value;
                Projectile.light=1f;
            }
            else
            {
        
                texture= ModContent.Request<Texture2D>("test/Projectiles/ShiningFlamesNight").Value;
                lightColor=Color.White;
                lightColor.A=0;
                Projectile.light=2f;
            }
            //夜晚特效
        
            int counter = (Projectile.frameCounter / 6) % (4);
           

            Rectangle rectangle = new Rectangle(
                   0,
                   texture.Height * counter / Main.projFrames[Type],
                   texture.Width,
                   texture.Height / Main.projFrames[Type]
                   );

            Main.EntitySpriteDraw(
                       texture,
                       Projectile.position - Main.screenPosition+ new Vector2(texture.Width / 2, texture.Height / 8),
                       rectangle,
                       lightColor * (Projectile.alpha / 255f),
                       Projectile.rotation,
                      new Vector2(texture.Width / 2, texture.Height /8),
                      new Vector2(Projectile.scale, Projectile.scale),
                      Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None, 0);
            return false;
        }

    }



    // Additional hooks/methods here.
}
