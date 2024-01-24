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
   
    public class CrimePunishment:ModProjectile
    {
        int act = 0;
        int time = 0;      
        bool down = true;
        public override void SetDefaults()
        {
            Projectile.damage= 818;
            Projectile.scale = 0.5f;
            Projectile.ignoreWater = true;
            Projectile.light = 2f;//亮度
            Projectile.tileCollide = false;//true不穿
            Projectile.penetrate = -1;//穿透怪物
            Projectile.alpha = 0;
            Projectile.friendly = true;//false对敌人无伤
            Projectile.hostile = false;//true有玩家伤害
            Projectile.timeLeft = 1000;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.aiStyle = -1; //
            Projectile.DamageType = DamageClass.Ranged;


        }
        public override bool ShouldUpdatePosition()
        {
            return false;
        }
        
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            /*
            float point = 0;
            Vector2 start;
            Vector2 end;
            if(act==5)
            {
                start=Projectile.Center-new Vector2(Projectile.scale*Projectile.width,0);    
                end=Projectile.Center+new Vector2(Projectile.scale*Projectile.width,0);    
               
            }

       */
            Vector2 Center = projHitbox.TopLeft()+new Vector2(projHitbox.Width/2, projHitbox.Height/2);
            Vector2 newTopLeft = Center+Projectile.velocity.X*new Vector2(Projectile.scale*Projectile.width,Projectile.scale*Projectile.height);

            Rectangle NewBox;
            if (act!=4)
                NewBox=new Rectangle((int)newTopLeft.X, (int)newTopLeft.Y, (int)(Projectile.scale*Projectile.width), (int)(Projectile.scale*Projectile.width));
            else
                NewBox=new Rectangle((int)(Center.X-0.5f*Projectile.scale*Projectile.width),
                    (int)(Center.Y-0.5f*Projectile.scale*Projectile.width),
                    (int)(Projectile.scale*Projectile.width),
                    (int)(Projectile.scale*Projectile.width));



           bool if_hit= Collision.CheckAABBvAABBCollision(targetHitbox.TopLeft(), targetHitbox.Size(), NewBox.TopLeft(), NewBox.Size());
           return if_hit;
        }
    
        private void Changecase()
        {
           Projectile.rotation%=(float)(2*Math.PI);
           switch (act) 
            {
                case 0://提剑
                    if(Projectile.rotation*Projectile.velocity.X<=-2.0f)
                    {
                        act = 1;
                    }
                    if (Projectile.scale<1f)
                        Projectile.scale+=0.05f;
                      
                    break;
                case 1:
                    if(Projectile.rotation*Projectile.velocity.X>=Math.PI)
                    {
                        act=2;
                    }
                    break;//下挥动
                case 2:{
                        time++;
                        if(time==10)
                        {
                            time=0;
                            act=3;
                        }    
                        break;
                    }//停顿
                case 3://上挥动
                    {
                        if(Projectile.rotation*Projectile.velocity.X<=0f)
                        {
                            act=4;

                        }
                        break;
                    }
                case 4://转刀
                    {
                        time++;
                        if (time<=10)
                            Projectile.scale-=0.05f;
                        else if (time>40)
                            Projectile.scale+=0.05f;
                        if (time==50)
                        {
                            time=0;
                            act=6;
                            Projectile.rotation=0;  
                        }
                            break;
                    }
                case 5://
                    {
                        Projectile.scale+=0.04f;
                        if(Projectile.rotation*Projectile.velocity.X<-6)
                        {
                            Projectile.rotation=0;
                            act=7;
                        }

                        break;
                    }
                case 6:
                    {
                        Projectile.scale+=0.05f;
                        time++;
                        if(time==15)
                        {
                            time=0;
                            act=5;
                        }
                        break;
                    }
                case 7:
                    {
                        time++;
                        if (Projectile.scale>=0.5)
                            Projectile.scale-=0.2f;
                        if (Projectile.scale<0.5) Projectile.scale=0.5f;
                        if(time==20)
                        {
                            time=0;
                            act=0;
                            Projectile.rotation=0;  

                        }
                        break;
                    }
                default: break; 
            }
           
            
        }
        public override void AI()
        {
            if (Projectile.owner==Main.myPlayer)
            {
                if (Main.player[Main.myPlayer].channel==true)
                {
                    if (act!=4)
                        Projectile.Center=Main.player[Main.myPlayer].Center;
                    switch (act)
                    {
                        case 0:
                            {
                                Projectile.rotation-=Projectile.velocity.X*0.1f;
                                break;
                            }
                        case 1:
                            {
                             
                                Projectile.rotation += Projectile.velocity.X * 0.7f;//6桢
                                break;
                            }
                        case 3:
                            {
                                Projectile.rotation -= Projectile.velocity.X * 0.3f;
                                break;
                            }
                        case 2:
                        case 6:
                            break;
                        case 4:
                            {
                                Projectile.rotation+=Math.Min(time*0.02f, 0.6f);
                                Projectile.Center=Main.player[Main.myPlayer].Center+30f*new Vector2(Projectile.velocity.X, -1f);
                            }
                            break;
                        case 5:
                            {
                                Projectile.rotation-=Projectile.velocity.X*0.6f;//10
                                break;
                            }

                        default: break;
                    }
                    Changecase();
                    Projectile.timeLeft++;
                }
                else
                {
                    Projectile.timeLeft=0;
                }

            }
            else
            {
                Projectile.timeLeft=0;
                Main.NewText("Errow", Color.Red);
            }

        }
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Type].Value;
            if (act<=3)
            {
                Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Main.EntitySpriteDraw(
                     texture,
                     Projectile.Center - Main.screenPosition,
                     rectangle,
                     lightColor,
                     Projectile.rotation,
                    new Vector2(Projectile.velocity.X==1 ? 0 : texture.Width, texture.Height),
                    new Vector2(Projectile.scale, Projectile.scale),
                    Projectile.velocity.X==1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            }
            else if (act==4)
            {
                Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Main.EntitySpriteDraw(
                     texture,
                     Projectile.Center - Main.screenPosition,
                     rectangle,
                     lightColor,
                     Projectile.rotation,
                    new Vector2( texture.Width/2, texture.Height/2),
                    new Vector2(Projectile.scale, Projectile.scale),
                    Projectile.velocity.X==1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);


            }
            else
            {
                Rectangle rectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                Main.EntitySpriteDraw(
                     texture,
                     Projectile.Center - Main.screenPosition,
                     rectangle,
                     lightColor,
                     Projectile.rotation,
                    new Vector2(Projectile.velocity.X==1 ? -10 : texture.Width+10, texture.Height+10),
                    new Vector2(Projectile.scale, Projectile.scale),
                    Projectile.velocity.X==1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
                if (act==5)
                    return true;
            }
        return false;
        }






    }
}
