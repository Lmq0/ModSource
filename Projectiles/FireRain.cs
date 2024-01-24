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
    public class FireRain: ModProjectile
    {
        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {

            Main.projFrames[Type] = 6;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;

            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 3f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.alpha = 230;
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物

            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 300;




            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 102; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)		//	Electrosphere
        {

            target.AddBuff(ModContent.BuffType<HolyFlames>(), 100);

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
            Random ran = new Random();
            int a = ran.Next(100);
            if(a==0&&Projectile.timeLeft<150)
            {
                Projectile.velocity *= 1.5f;
            }
            Projectile.velocity *= 1f;
 


            ;

        }
        public override void AI()
        {
            Random ran = new Random();
            if (Projectile.frameCounter > 15 && (Projectile.frameCounter % 4 == 0))
            {
                float change = ran.Next(4);
                change -= 2;
                change /= 10f;
                Projectile.scale += change;
                if (Projectile.scale >= 3.3f)
                    Projectile.scale = 3.3f;
                if (Projectile.scale <= 3f)
                    Projectile.scale = 3f;
            }
            //无翻转
            Projectile.frameCounter++;
            Projectile.rotation=Projectile.velocity.ToRotation();

            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture;
            int counter = (Projectile.frameCounter/10) % (6);
            lightColor.A=(byte)((lightColor.A)/1.5f);
            if(Main.dayTime==true)
            {
                 texture= TextureAssets.Projectile[Type].Value;
            }
            else
            {
                lightColor=Color.White;
                lightColor.A=0;
                texture= ModContent.Request<Texture2D>("test/Projectiles/FireRainNight").Value;
            }

            Rectangle rectangle = new Rectangle(
                   0,
                   texture.Height * counter / Main.projFrames[Type],
                   texture.Width,
                   texture.Height / Main.projFrames[Type]
                   );

            Main.EntitySpriteDraw(
                       texture,
                       Projectile.position - Main.screenPosition+new Vector2(15f,80f),
                       rectangle,
                       lightColor * (Projectile.alpha/255f),
                       Projectile.rotation+(float)Math.PI/2f,
                      new Vector2(texture.Width / 2, texture.Height / (2*Main.projFrames[Type])),
                      new Vector2(Projectile.scale, Projectile.scale),
                      0, 0);
            return false;
        }

    }



    // Additional hooks/methods here.
}
