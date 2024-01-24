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

                }
                */
            }
            return true ;
        }
            
        
    }
}
