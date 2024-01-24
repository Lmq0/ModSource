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
using IL.Terraria.GameContent.Bestiary;
using Microsoft.CodeAnalysis;
using test.Projectiles.HolyRay;

namespace test.Projectiles
{
    public class FlameCristal:ModProjectile
    {
        private bool If_begin = true;
        Player master => Main.player[Main.myPlayer];
        public override void SetStaticDefaults()
        {

            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;
        
    
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 0f;
            Projectile.ignoreWater = true;
            Projectile.light = -1f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 120;
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 700;




            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = -1; // or 9
            Projectile.DamageType = DamageClass.Ranged;
    
            base.SetDefaults();

        }
        public override bool PreAI()
        {
            return true;
        }
        public override void AI()
        {

            Entity.Center=master.Center+new Vector2(0, -400);
            if((Projectile.frameCounter%50==20&&!Main.dayTime)
                ||Projectile.frameCounter%100==20&&Main.dayTime)
            {
                int d = 0;
                int Num = 6;
                int type = ModContent.ProjectileType<HolyFireBall>();
                if (!Main.dayTime)
                {
                    Num=10;
                    type=ProjectileID.FrostShard;
                }
               
                while (d<Num)
                {
                 

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    new Vector2((d*5f)+2F, 0f),
                    type, 25, 0, Main.myPlayer);

                    Projectile.NewProjectile(Projectile.GetSource_FromThis(),
                   Projectile.Center,
                   new Vector2(-(d*5f)-2f, 0f),
                   type, 25, 0, Main.myPlayer);
                    d++;
                }
            }
            if (Projectile.scale < 2f && If_begin)
            {

                Projectile.scale += 0.2f;
                if (Projectile.scale == 2f)
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
            Projectile.velocity*=0f;
    
     
            Projectile.frameCounter++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            lightColor.A=(byte)(lightColor.A*1.5f);
            Projectile.frame=0;
            if(!Main.dayTime)
            {
                texture=ModContent.Request<Texture2D>("test/Projectiles/FlameCristalNight").Value;
                lightColor=Color.White;
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
             lightColor*0.5f,//第四个参数是颜色，这里我们用自带的lightcolor，可以受到自然光照影响
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
