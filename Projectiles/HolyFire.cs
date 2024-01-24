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
    public class HolyFire : ModProjectile
    {
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
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.alpha = 200;
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物

            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 200;




            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.aiStyle = 102; // or 9
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
            Projectile.velocity *= 1f;
            Projectile.rotation = 0;


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
                if (Projectile.scale >= 1.3f)
                    Projectile.scale = 1.3f;
                if (Projectile.scale <= 1f)
                    Projectile.scale = 1f;
            }
            //无翻转

       
            Projectile.frameCounter++;


            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
            if (Projectile.localAI[0]==1)
                lightColor.A=0;
            Texture2D texture;
            if (Main.dayTime==true)
            {
                texture = TextureAssets.Projectile[Type].Value;
            }
            //夜晚特效
            else
            {
                texture= ModContent.Request<Texture2D>("test/Projectiles/HolyFireNight").Value;
                lightColor=Color.White;
                lightColor.A=0;
            }
          
            int counter = (Projectile.frameCounter / 5) % (4);

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
                       Projectile.rotation,
                      new Vector2(texture.Width / 2, texture.Height / 2),
                      new Vector2(Projectile.scale, Projectile.scale),
                      Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None, 0);
            return false;
        }

    }



    // Additional hooks/methods here.
}
