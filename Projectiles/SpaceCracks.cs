using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;
using test.Projectiles;
using test.Buffs;

namespace test.Projectiles
{
    public class SpaceCracks : ModProjectile
    {
        private bool If_begin=true;
        public override void SetStaticDefaults()
        {

            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;

            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 0f;
            Projectile.ignoreWater = true;
            Projectile.light = -1f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 200;
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 1000;
            Projectile.damage=150;


            Projectile.arrow = true;
            Projectile.width = 100;
            Projectile.height = 100;
            Projectile.aiStyle = -1; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)//	Electrosphere
        { 
            target.AddBuff(BuffID.Bleeding, 300);
            
        }
        public override bool PreAI()
        {
            return true;
        }
        public override void AI()
        {
            if(Projectile.frameCounter%75==25)
            {
                Random ran=new Random();
                float O_ANLGE = ran.Next(7);
                O_ANLGE%=6.28f;
                int qq = 0;
                while(qq<6)
                {
                    if (Main.dayTime)
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                            new Vector2(15f*(float)Math.Sin(O_ANLGE+qq*6.28f/6f), 15f*(float)Math.Cos(O_ANLGE+qq*6.28f/6f)),
                            ProjectileID.CultistBossFireBall, 25, 0f);
                    else
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                           new Vector2(15f*(float)Math.Sin(O_ANLGE+qq*6.28f/6f), 15f*(float)Math.Cos(O_ANLGE+qq*6.28f/6f)),
                           ProjectileID.CultistBossFireBallClone, 25, 0f);

                    qq++;
                }
            }
            Projectile.rotation+=0.2f;
            Projectile.rotation%=6.28f;
            Projectile.velocity*=0;
            if (Projectile.scale < 5f && If_begin)
            {

                Projectile.scale += 0.2f;
                if (Projectile.scale == 5f)
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
            Projectile.frameCounter++;  
        }
        public override bool PreKill(int timeLeft)
        {
            int qq = 0;
            while (qq<12)
            {
                if (Main.dayTime)
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                        new Vector2(20f*(float)Math.Sin(qq*6.28f/12f), 20f*(float)Math.Cos(qq*6.28f/12f)),
                        ProjectileID.CultistBossFireBall, 25, 0f);
                else
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center,
                       new Vector2(20f*(float)Math.Sin(qq*6.28f/12f), 20f*(float)Math.Cos(qq*6.28f/12f)),
                       ProjectileID.CultistBossFireBallClone, 25, 0f);
                qq++;
            }
            Projectile.type = ProjectileID.Daybreak;
            return true;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.frame=0;
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            if(!Main.dayTime)
            {
                texture=ModContent.Request<Texture2D>("test/Projectiles/SpaceCracks_Night").Value;
                lightColor = Color.White;
                lightColor.A=0;
            }
            Rectangle rectangle = new Rectangle(
                0,
                texture.Height / Main.projFrames[Type] * Projectile.frame,
                texture.Width,
                texture.Height / Main.projFrames[Type]
                );
            Main.EntitySpriteDraw(  //entityspritedraw是弹幕，NPC等常用的绘制方法
             texture,//第一个参数是材质
             Projectile.Center - Main.screenPosition,//注意，绘制时的位置是以屏幕左上角为0点
                                                     //因此要用弹幕世界坐标减去屏幕左上角的坐标
             rectangle,//第三个参数就是帧图选框了
             lightColor,//第四个参数是颜色，这里我们用自带的lightcolor，可以受到自然光照影响
             Projectile.rotation,//第五个参数是贴图旋转方向
             new Vector2(texture.Width / 2, texture.Height / 2),
             //第六个参数是贴图参照原点的坐标，这里写为贴图单帧的中心坐标，这样旋转和缩放都是围绕中心
             new Vector2(Projectile.scale, Projectile.scale),//第七个参数是缩放，X是水平倍率，Y是竖直倍率
             Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None,
             //第八个参数是设置图片翻转效果，需要手动判定并设置spriteeffects
             0//第九个参数是绘制层级，但填0就行了，不太好使
             );

            return false;
        }

    }
}
