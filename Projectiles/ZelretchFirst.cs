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
            Projectile.light = -1f;//����
            Projectile.tileCollide = false;//true����
            Projectile.penetrate = 1;//��͸����
            Projectile.alpha = 200;
            Projectile.friendly = true;//false�Ե�������
            Projectile.hostile = false;//true������˺�
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


            //ʧЧ���ٶȿ���
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
            //����
           

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
            //�޷�ת
         

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
            
            Main.EntitySpriteDraw(  //entityspritedraw�ǵ�Ļ��NPC�ȳ��õĻ��Ʒ���
               texture,//��һ�������ǲ���
               Projectile.Center - Main.screenPosition,//ע�⣬����ʱ��λ��������Ļ���Ͻ�Ϊ0��
                                                       //���Ҫ�õ�Ļ���������ȥ��Ļ���Ͻǵ�����
               rectangle,//��������������֡ͼѡ����
               lightColor,//���ĸ���������ɫ�������������Դ���lightcolor�������ܵ���Ȼ����Ӱ��
               Projectile.rotation,//�������������ͼ��ת����
               new Vector2(texture.Width / 2, texture.Height / 2),
               //��������������ͼ����ԭ������꣬����дΪ��ͼ��֡���������꣬������ת�����Ŷ���Χ������
               new Vector2(Projectile.scale, Projectile.scale),//���߸����������ţ�X��ˮƽ���ʣ�Y����ֱ����
               Projectile.direction == -1 ? SpriteEffects.None : SpriteEffects.None,
               //�ڰ˸�����������ͼƬ��תЧ������Ҫ�ֶ��ж�������spriteeffects
               0//�ھŸ������ǻ��Ʋ㼶������0�����ˣ���̫��ʹ
               );



            return false;
        }
    
    }



        // Additional hooks/methods here.
    }
