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
using test.Projectiles.HolyRay;

namespace test.Projectiles
{
    public class LavaLances : ModProjectile
    {
        private Vector2 OriginVelocity;
        private int RelaxTime = 40; 
        private bool if_explo = true;
        private bool if_paint = true;
        private Vector2 place;
        private int count=-100;
        Player player => Main.player[Projectile.owner];
        public override void SetStaticDefaults()
        {

            Main.projFrames[Type] = 1;
            ProjectileID.Sets.TrailingMode[Type] = 2;
            ProjectileID.Sets.TrailCacheLength[Type] = 5;

            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            Projectile.scale = 1f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 0;
            Projectile.friendly = false;//false对敌人无伤
            Projectile.hostile = true;//true有玩家伤害
            Projectile.timeLeft = 150;




            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.aiStyle = -1; // or 9
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override void OnHitPlayer(Player target, int damage, bool crit)		//	Electrosphere
        {


            target.AddBuff(BuffID.Daybreak, 300);
            target.AddBuff(ModContent.BuffType<ParadiseSanction>(), 500);

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
          


            ;

        }
        public override void AI()
        {
            if (Projectile.frameCounter==0)
            {
                if (!Main.dayTime)
                    RelaxTime/=2;
                OriginVelocity=Projectile.velocity;
                Projectile.rotation = Projectile.velocity.ToRotation();
                Projectile.velocity *=0f;
                count=-10000;
            }
            //记录当前的速度 方便恢复
       
  
            //无翻转
            Projectile.frameCounter++;

            if(Projectile.Center.X-player.Center.X<=10&&
                if_explo&&Projectile.velocity.X!=0&&
                Projectile.frameCounter>=60&&Main.dayTime)
            {
                if_explo=false;
                
                if (Projectile.timeLeft>40)
                {
                    if_paint=false;
                    Projectile.velocity*=0;
                    place=Projectile.Center;
                    count=Projectile.frameCounter;
                    int explosion = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, new Vector2(0, -1f), ProjectileID.DD2ExplosiveTrapT3Explosion,
                     100, 0);
                    Main.projectile[explosion].scale=1.5f;
                    Main.projectile[explosion].friendly=false;
                    Main.projectile[explosion].hostile=true;



                }
            }    
            if(Projectile.frameCounter==count+10&&!if_explo&&Main.dayTime)
            {
                int line=Projectile.NewProjectile(Projectile.GetSource_FromAI(), place, new Vector2(0, -1f), ModContent.ProjectileType<RayPreLine>(),
                  1500, 0);
                Main.projectile[line].width=15;
                Main.projectile[line].timeLeft=30;
            }
            if(Projectile.frameCounter == count + 40 && !if_explo && Main.dayTime)
            {
                int proj = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, new Vector2(0, -1),
               ModContent.ProjectileType<HolyRays>(), 1500, 0);
                Main.projectile[proj].timeLeft=40;
            }
            if(Projectile.frameCounter>RelaxTime&&Main.dayTime)
                Dust.NewDust(Projectile.Center, 5, 5, DustID.Firefly, Projectile.velocity.X*-0.0001f);

            if (Projectile.frameCounter==RelaxTime)
                Projectile.velocity=OriginVelocity;//恢复速度
            if (Projectile.frameCounter>RelaxTime&&Projectile.velocity.X<30f)
                Projectile.velocity*=1.05f;
            
      
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            lightColor= Color.LemonChiffon;
            if(!Main.dayTime)
            {
                texture=ModContent.Request<Texture2D>("test/Projectiles/NightFraction").Value;
                lightColor= Color.White;
                lightColor.A=0;
            }
            if (Math.Abs(Projectile.frameCounter-RelaxTime)<=15)
            {
               
                Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Main.EntitySpriteDraw(
                     texture,
                     Projectile.Center - Main.screenPosition,
                     rectangle,
                     lightColor*(Math.Abs(Projectile.frameCounter-RelaxTime)/30f),
                     Projectile.rotation,
                    new Vector2(texture.Width / 2, texture.Height / 2),
                    new Vector2(Projectile.scale, Projectile.scale),
                    SpriteEffects.None, 0);
            }
            else if (if_paint)
            {
                Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Main.EntitySpriteDraw(
                     texture,
                     Projectile.Center - Main.screenPosition,
                     rectangle,
                     lightColor,
                     Projectile.rotation,
                    new Vector2(texture.Width / 2, texture.Height / 2),
                    new Vector2(Projectile.scale, Projectile.scale),
                    SpriteEffects.None, 0);
            }

            return false;
        }

    }



    // Additional hooks/methods here.
}
