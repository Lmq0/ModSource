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
    public class HolyRays : ModProjectile
    {
        //height ���ݳ��Ȳ���
        //width ������󳤶�
        //timeleft ����Ҫ��width��һ�� ����������
        //damage �˺�
        //localai[0]��¼��ǰ����

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
            Projectile.light = 2f;//����
            Projectile.alpha = 150;
            Projectile.tileCollide = false;//true����ǽ
            Projectile.penetrate = -1;//��͸����
            Projectile.friendly = false;//false�Ե�������
            Projectile.hostile = true;//true������˺�
            Projectile.timeLeft = 350;
            Projectile.width = 25;
            Projectile.height = 2500;
            Projectile.aiStyle =-1; 
            Projectile.DamageType = DamageClass.Ranged;

            base.SetDefaults();

        }
        public override bool ShouldUpdatePosition()
        {
            return false;//λ�ò���Ϊvelocity�ı�
        }
        //ȡ���ٶȶ�λ�õĸı�
        public override void OnHitPlayer(Player target, int damage, bool crit)		//	Electrosphere
        {
            if (Projectile.damage==0)
                target.statLife=-1;//Ҫ����ɱ������˺���0  Ҫ������˺�дfriendly

        }
        public override Color? GetAlpha(Color lightColor)
        {
            return lightColor;
        }
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (Projectile.localAI[0] < Projectile.width) return false;
           
            float point = 0f;
            Vector2 startPoint = Projectile.Center;
            Vector2 endPoint = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.Zero) * Projectile.height;
            bool if_hit = Collision.CheckAABBvLineCollision
                (targetHitbox.TopLeft(), targetHitbox.Size(), startPoint, endPoint, Projectile.width/5f,ref point);
            if(if_hit)return true;
            return false;
        }
        //��ײ�串д
        public override bool PreKill(int timeLeft)
        {





            Projectile.type = ProjectileID.Daybreak;
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
            if (Projectile.localAI[0] < Projectile.width && Projectile.timeLeft > Projectile.width+1)
                Projectile.localAI[0]+=2;

            if (Projectile.timeLeft < Projectile.width + 1)
                Projectile.localAI[0]-=2;
            
            Projectile.frameCounter++;
            base.AI();
        }
        public override bool PreDraw(ref Color lightColor)
        {
          
            Texture2D head = ModContent.Request<Texture2D>("test/Projectiles/HolyRay/HolyRayHead").Value;
            Texture2D tail = ModContent.Request<Texture2D>("test/Projectiles/HolyRay/HolyRayTail").Value;
            Texture2D tex = TextureAssets.Projectile[Type].Value;
            Color PaintColor = Color.White;

            if (!Main.dayTime)
            {
                head = ModContent.Request<Texture2D>("test/Projectiles/HolyRay/HolyRayHead_Night").Value;
                tail = ModContent.Request<Texture2D>("test/Projectiles/HolyRay/HolyRayTail_Night").Value;
                tex = ModContent.Request<Texture2D>("test/Projectiles/HolyRay/HolyRays_Night").Value;
                PaintColor.A = 0;//͸������ ����
            }
            Main.EntitySpriteDraw(head, Projectile.Center - Main.screenPosition, null, PaintColor,
                Projectile.velocity.ToRotation(),
                new Vector2(0, head.Height / 2),
                new Vector2(7, Projectile.localAI[0] / 5f),
                SpriteEffects.None, 0);
            //ͷ������
            Main.EntitySpriteDraw(tex, Projectile.Center - Main.screenPosition+Projectile.velocity.SafeNormalize(Vector2.Zero)*head.Width,
                new Rectangle(0,0,Projectile.height,tex.Height),
                PaintColor,
                Projectile.velocity.ToRotation(),
                new Vector2(0, tex.Height / 2),
                new Vector2(1, Projectile.localAI[0] / 5f),
                SpriteEffects.None, 0);
            //�������
            Main.EntitySpriteDraw(tail, Projectile.Center - Main.screenPosition + Projectile.velocity.SafeNormalize(Vector2.Zero) * (head.Width+Projectile.height),
              null,
              PaintColor,
              Projectile.velocity.ToRotation(),
              new Vector2(0, tail.Height / 2),
              new Vector2(7, Projectile.localAI[0] / 5f),
              SpriteEffects.None, 0); ;
            //β������
            return false;
        }

    }



    // Additional hooks/methods here.
}
