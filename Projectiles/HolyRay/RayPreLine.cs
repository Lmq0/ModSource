using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Enums;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.DataStructures;
using Terraria.Localization;
using test.Buffs;
using test.Projectiles;
using System.ComponentModel.DataAnnotations;

namespace test.Projectiles.HolyRay
{
    public class RayPreLine : ModProjectile
    {


        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {

            Main.projFrames[Type] = 1;
            //ProjectileID.Sets.TrailingMode[Type] = 2;
            //ProjectileID.Sets.TrailCacheLength[Type] = 5;

            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 1f;
            Projectile.damage = 0;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.alpha = 150;
            Projectile.tileCollide = false;//true不穿墙
            Projectile.penetrate = -1;//穿透怪物
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 350;
            Projectile.width = 25;
            Projectile.height = 2500;
            Projectile.aiStyle =-1;
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override bool ShouldUpdatePosition()
        {
            return false;//位置不因为velocity改变
        }
        //取消速度对位置的改变

        public override bool PreKill(int timeLeft)
        {


            return true;
        }
        public override bool CanHitPlayer(Player target)
        {
            return false;//预判线无伤
        }
        public override void AI()
        {
            if (Projectile.localAI[0] < Projectile.width && Projectile.timeLeft> (Projectile.width+1))
                Projectile.localAI[0]+=1;
            if (Projectile.localAI[0]>Projectile.width)
                Projectile.localAI[0]=Projectile.width; 
            if (Projectile.timeLeft< (Projectile.width + 1))
                Projectile.localAI[0]-=1;

            Projectile.frameCounter++;
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {

        
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Color PaintColor = Color.White;
            if (!Main.dayTime)
            {
                tex= ModContent.Request<Texture2D>("test/Projectiles/HolyRay/RayPreLine_Night").Value;
                PaintColor.A=0;
            }
         //透明处理 亮化

            Main.EntitySpriteDraw(tex, Projectile.Center- Main.screenPosition,
                new Rectangle(0, 0, Projectile.height, tex.Height),
                PaintColor*(Projectile.localAI[0]/Projectile.width),
                Projectile.velocity.ToRotation(),
                new Vector2(0, tex.Height / 2),
                new Vector2(1f, 12F),
                SpriteEffects.None, 0);
            //身体绘制
            
            return false;
        }

    }



    // Additional hooks/methods here.
}
