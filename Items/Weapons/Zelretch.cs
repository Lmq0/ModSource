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
using System.IO;
using System.Collections;
using test.Buffs;


using test.Projectiles;


namespace test.Items.Weapons
{

    public class Zelretch: ModItem
    {
        int Count=0;
  
        public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("TutorialSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("\n" +
                "A Sword which is made of the Magic,\n" +
                "Quite Magnificent and Powerful.\n" +
                "However,that Power is NOT FOR FREE.\n");
		}
		public override void SetDefaults()
		{
            Item.damage = 2120;//伤害
            Item.DamageType = DamageClass.Melee;//近战类型伤害
            Item.width = 20;//宽
            Item.height = 80;//高
            Item.useTime = 12;//攻速
            Item.useAnimation = 12;
            Item.useStyle = 1;//
            Item.knockBack = 10;//击退
            Item.value = 10000;//卖出价格
            Item.rare = -14;//稀有度
            Item.UseSound = SoundID.Item1;//音效
            Item.autoReuse = true;//自动挥舞


            Item.shoot = ModContent.ProjectileType<ZelretchFirst>();//ModContent.ProjectileType<ZelretchFirst>();
            Item.shootSpeed = 20f;
        }
        //EtherealLances


       

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
         
            player.AddBuff(ModContent.BuffType<MagicBurn>(), 150);
          
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY) + new Vector2(30f, 0); //鼠标位置
            float ceilingLimit = target.Y ;
            //Item.NewItem(source,(int)target.X, (int)target.Y, 88,88, ModContent.ItemType<Zelretch>());
            //ModContent.ProjectileType<ZelretchSecond>()
            if (Count % 1==0)
            {
                ceilingLimit = target.Y + 100f;
                Projectile.NewProjectile(source, target, target, ModContent.ProjectileType<ZelretchSecond>(), damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                ceilingLimit -= 100f;
            }
            int  c=0;

            while (c < 4)
            {
                Random ran = new Random();
                float a = ran.Next(360);//以水平向左为角的基准线 取25-150 210-330
                position = target;
                Vector2 heading;
                heading = new Vector2((float)(1500 * Math.Cos((a)*0.0175)), (float)(1500f * Math.Sin((a*0.0175))));
                
                //velocity = velocity.Normalize();
                heading.Normalize();
                heading *= velocity.Length();

                Projectile.NewProjectile(source, position, heading, type, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
       
                c++;
               
            }
            Count++;
            return false;
        }
        public override void AddRecipes()
		{
            ;
		}





        
    }
}