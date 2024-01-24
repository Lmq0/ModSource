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
    public class ZelretchFirst : ModProjectile
    {
        bool If_begin = true;
        Player player => Main.player[Projectile.owner];
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
            Projectile.penetrate = 1;//穿透怪物
            Projectile.alpha = 200;
            Projectile.friendly = true;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 300;



            Projectile.arrow = true;
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = 9; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)//	Electrosphere
        {

            if (!target.townNPC && crit == true)
            {
                Player player = Main.player[Projectile.owner];
                player.Heal(5);
                target.AddBuff(BuffID.Poisoned, 300);
                target.AddBuff(BuffID.Frostburn, 300);
                target.AddBuff(BuffID.Burning, 300);
                target.AddBuff(BuffID.Ichor, 300);
                target.AddBuff(BuffID.OnFire, 300);
                target.AddBuff(BuffID.ShadowFlame, 300);
                target.AddBuff(BuffID.CursedInferno, 300);
                target.AddBuff(BuffID.Venom, 300);
                target.AddBuff(BuffID.Bleeding, 300);
                target.AddBuff(BuffID.Daybreak, 300);
                target.AddBuff(BuffID.Shine, 300);
                target.AddBuff(ModContent.BuffType<MagicBurn>(), 300);
            }
      
           

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
            Projectile.Resize(140, 140);
            Projectile.type = ProjectileID.Daybreak;
            Projectile.alpha += 100;
            if (Projectile.alpha > 255)
                Projectile.alpha = 255;
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
            if(Projectile.frameCounter < 35)
            {
              
                    Projectile.velocity /= 1.2f;
            }
            else 
                Projectile.velocity *= 1.5f;
            if (Projectile.frameCounter==38)
                Projectile.velocity /= 2f;


            //失效的速度控制
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
            //渐亮
           

            if ((Projectile.frameCounter % 50 <= 8 && Projectile.frameCounter % 50 >= 1) ||
                (Projectile.frameCounter % 50 <= 44 && Projectile.frameCounter % 50 >= 49))
            {
                Projectile.scale += 0.07f;
            }
            if ((Projectile.frameCounter % 50 <= 33 && Projectile.frameCounter % 50 >= 17) ||
                (Projectile.frameCounter % 50 <= 33 && Projectile.frameCounter % 50 >= 17))
            {
                Projectile.scale -= 0.1f;
            }
            if (Projectile.scale > 2.3f)
            {
                Projectile.scale = 2.4f;
            }
            Projectile.frameCounter++;

        }
        public override void AI()
        {
            {
                if (If_begin)
                    Projectile.velocity /= 1.15f;
                else
                    Projectile.velocity /= 1.2f;
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
            //无翻转
         

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

            if (Projectile.frameCounter >= 45)
            {
                for (int i = 0; i < ProjectileID.Sets.TrailCacheLength[Type]; i++)
                {
                    float factor = 1 - (float)i / 10;
                    if (factor < 0.05)
                        factor = 0.05f;
                    Vector2 oldcenter = Projectile.oldPos[i] - Main.screenPosition;
                    Main.EntitySpriteDraw(
                        texture,
                        oldcenter,
                        rectangle,
                        lightColor * factor,
                       Projectile.oldRot[i],
                       new Vector2(texture.Width / 2, texture.Height / 2),
                       new Vector2(Projectile.scale, Projectile.scale),
                       Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None, 0);

                }
            }
            else if (Projectile.frameCounter >= 10)
            {
                for (int i = 0; i < 5; i++)
                {
                    float factor = 0.1f;
                    Vector2 oldcenter = Projectile.oldPos[i] - Main.screenPosition;
                    Main.EntitySpriteDraw(
                        texture,
                        oldcenter,
                        rectangle,
                        lightColor * factor,
                       Projectile.oldRot[i],
                       new Vector2(texture.Width / 2, texture.Height / 2),
                       new Vector2(Projectile.scale, Projectile.scale),
                       Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None, 0);

                }
            }
            else
                ;
            
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



        // Additional hooks/methods here.
    }
