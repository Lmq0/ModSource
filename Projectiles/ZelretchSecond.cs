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
            Projectile.light = 2f;//����
            Projectile.tileCollide = false;//true����
            Projectile.penetrate = -1;//��͸����
            Projectile.alpha = 200;
            Projectile.friendly = false;//false�Ե�������
            Projectile.hostile = false;//true������˺�
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
          

            //ʧЧ���ٶȿ���
            if (Projectile.scale < 1f && If_begin)
            {

                Projectile.scale += 0.2f;
                if (Projectile.scale == 1f)
                    If_begin = false;
            }

        
            //����
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
            //ʧЧ�ĵ���

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
            //�޷�ת
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
           
            Main.EntitySpriteDraw(  //entityspritedraw�ǵ�Ļ��NPC�ȳ��õĻ��Ʒ���
               texture,//��һ�������ǲ���
               Projectile.Center - Main.screenPosition,//ע�⣬����ʱ��λ��������Ļ���Ͻ�Ϊ0��
                                                       //���Ҫ�õ�Ļ���������ȥ��Ļ���Ͻǵ�����
               rectangle,//��������������֡ͼѡ����
               lightColor,//���ĸ���������ɫ�������������Դ���lightcolor�������ܵ���Ȼ����Ӱ��
               Projectile.rotation,//�������������ͼ��ת����
               new Vector2(texture.Width / 2, texture.Height / 8),
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
