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
    public class HolyFireBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 12;
            DisplayName.SetDefault("HolyFireball");
        }
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.DD2BetsyFireball);
            AIType = ProjectileID.DD2BetsyFireball;
            Projectile.light = 2f;
        }

        public override bool PreKill(int timeLeft)
        {
            Projectile.type = ProjectileID.DD2BetsyFireball;
            return true;
        }
        public override Color? GetAlpha(Color lightColor)
        {
  
            return lightColor;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.type = ProjectileID.DD2BetsyFireball;
            return true;
            // Additional hooks/methods here.
        }
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(ModContent.BuffType<HolyFlames>(), 500);
            target.AddBuff(BuffID.Daybreak, 500);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            lightColor.R = 255;
            lightColor.G = 255;
            lightColor.B = 150;
       
            lightColor.A =50 ;

            Texture2D texture = TextureAssets.Projectile[Type].Value;
            Projectile.type = ProjectileID.DD2BetsyFireball;
            {
                /*
                {
                    Rectangle rectangle = new Rectangle(
                        0,
                        texture.Height / Main.projFrames[Type] * Projectile.frame,
                        texture.Width,
                        texture.Height / Main.projFrames[Type]
                        );



                    for (int i = 0; i < 10; i++)
                    {
                        float factor = 0.8f - (float)(i) / 8f;
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

                }
                */
            }
            return true ;
        }
            
        
    }
}
