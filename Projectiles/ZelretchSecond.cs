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
    public class ZelretchSecond : ModProjectile
    {
        bool If_begin = true;
        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {
            
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 20;
        
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 0f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 200;
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 18;



            Projectile.arrow = true;
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.aiStyle = 1; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)//	Electrosphere
        {
            Player player = Main.player[Projectile.owner];
            target.AddBuff(ModContent.BuffType<MagicBurn>(), 300); 

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
            if (Projectile.light >= -1f)
                Projectile.light -= 0.35f;
            if (Projectile.scale>=0.2f)
                Projectile.scale -= 0.2f;
            Projectile.alpha += 100;
            if (Projectile.alpha > 255)
                Projectile.alpha = 255;
            Projectile.Resize(140, 140);
            Projectile.type = ProjectileID.Daybreak;
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
          

            //失效的速度控制
            if (Projectile.scale < 1f && If_begin)
            {

                Projectile.scale += 0.2f;
                if (Projectile.scale == 1f)
                    If_begin = false;
            }

        
            //渐亮
            {
                if (Projectile.alpha > 0)
                {
                    Projectile.alpha -= 10;
                }
                if (Projectile.alpha < 100)
                {
                    Projectile.alpha = 100;
                }
            }
            //失效的淡入

            if(Projectile.frameCounter<= 4)
            {
                Projectile.scale += 0.02f;
            }
            else if(Projectile.frameCounter <= 8)
            {
                Projectile.scale += 0.03f;
            }
            else if (Projectile.frameCounter <= 12)
            {
                Projectile.scale += 0.05f;
            }
            else if (Projectile.frameCounter <= 16)
            {
                Projectile.scale += 0.06f;
            }
            /*
            if ((Projectile.frameCounter % 50 <= 8 && Projectile.frameCounter % 50 >= 1) ||
                (Projectile.frameCounter % 50 <= 44 && Projectile.frameCounter % 50 >= 49))
            {
                Projectile.scale += 0.07f;
            }
            if ((Projectile.frameCounter % 50 <= 20 && Projectile.frameCounter % 50 >= 30 ) ||
                (Projectile.frameCounter % 50 <= 33 && Projectile.frameCounter % 50 >= 17))
            {
                Projectile.scale -= 0.1f;
            }
            if (Projectile.scale > 2.3f)
            {
                Projectile.scale = 2.4f;
            }

            */
            Projectile.frameCounter++;

        }
        public override void AI()
        {
   ;
            Projectile.velocity = new Vector2(0f, 0f);
            Projectile.rotation = Projectile.velocity.ToRotation();
            //无翻转
            // Projectile.frameCounter++;
            Projectile.frame = (Projectile.frameCounter / 4) % Main.projFrames[Type];
           
           

                base.AI();
        }

 
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Rectangle rectangle = new Rectangle(
                0,
                texture.Height / Main.projFrames[Type] * Projectile.frame,
                texture.Width,
                texture.Height / Main.projFrames[Type]
                );
            Color MyColor = Color.White; MyColor.A = 0;
           
            Main.EntitySpriteDraw(  //entityspritedraw是弹幕，NPC等常用的绘制方法
               texture,//第一个参数是材质
               Projectile.Center - Main.screenPosition,//注意，绘制时的位置是以屏幕左上角为0点
                                                       //因此要用弹幕世界坐标减去屏幕左上角的坐标
               rectangle,//第三个参数就是帧图选框了
               lightColor,//第四个参数是颜色，这里我们用自带的lightcolor，可以受到自然光照影响
               Projectile.rotation,//第五个参数是贴图旋转方向
               new Vector2(texture.Width / 2, texture.Height / 8),
               //第六个参数是贴图参照原点的坐标，这里写为贴图单帧的中心坐标，这样旋转和缩放都是围绕中心
               new Vector2(Projectile.scale, Projectile.scale),//第七个参数是缩放，X是水平倍率，Y是竖直倍率
               Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None,
               //第八个参数是设置图片翻转效果，需要手动判定并设置spriteeffects
               0//第九个参数是绘制层级，但填0就行了，不太好使
               );



            return false;
        }
    
    }



        // Additional hooks/methods here.
    }
