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
    public class RayPreLine_thin : ModProjectile
    {

        //height 传递长度参数
        //width 控制最大长度
        //timeleft 至少要是width的一倍 否则会出问题
        //damage 传伤害
        //localai[0]记录当前长度
        //localai[1]传递之后要生成的弹幕  伤害用这个的damage
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
            Projectile.timeLeft = 30;
            Projectile.width = 15;
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
            if (Projectile.localAI[1]>0)
            {
                int newone=Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, (int)Projectile.localAI[1], Projectile.damage, 0);
                Main.projectile[newone].timeLeft= 60;//想想怎么解决这个问题 加一个参数 也行?
            }
            return true;
        }
        public override void Kill(int timeleft)
        {
            ;
        }
        public override void OnSpawn(IEntitySource source)
        {


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
            if(Main.dayTime==false)
            {
                tex= ModContent.Request<Texture2D>("test/Projectiles/HolyRay/RayPreLine_thin_Night").Value;
                lightColor.A=0;
            }
            //透明处理 亮化

            Main.EntitySpriteDraw(tex, Projectile.Center- Main.screenPosition,
                new Rectangle(0, 0, Projectile.height, tex.Height),
                PaintColor*(Projectile.localAI[0]/Projectile.width),
                Projectile.velocity.ToRotation(),
                new Vector2(0, tex.Height / 2),
                new Vector2(1f, 2f),
                SpriteEffects.None, 0);
            //身体绘制

            return false;
        }

    }



    // Additional hooks/methods here.
}
