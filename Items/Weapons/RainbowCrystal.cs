using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using test.Projectiles;

namespace test.Items.Weapons
{
	public class RainbowCrystal: ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("TutorialSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Thousands of Rainbow Crystal Drop as if it is Raining.");
		}

		public override void SetDefaults()
		{
            Item.damage = 212;//伤害
            Item.DamageType = DamageClass.Melee;//近战类型伤害
            Item.width = 30;//宽
            Item.height = 100;//高
            Item.useTime = 12;//攻速
            Item.useAnimation = 12;
            Item.useStyle = 1;//
            Item.knockBack = 10;//击退
            Item.value = 10000;//卖出价格
            Item.rare = 10;//稀有度
            Item.UseSound = SoundID.Item1;//音效
            Item.autoReuse = true;//自动挥舞


            Item.shoot = ModContent.ProjectileType<FireRain>();//FairyQueenRangedItemShot
            Item.shootSpeed = 25f;
        }
        //EtherealLances




        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
    
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);//鼠标位置
            float ceilingLimit = target.Y;//应该是触碰消失？的距离
            int i = 1;
            while (i <= 3)
            {
                switch (i)
                {
                    case 1:
                        position = player.Center - new Vector2(Main.rand.NextFloat(1301) * player.direction, 600f);//朝左挥舞 右往左坠星
                        break;
                    case 2:
                        position = player.Center + new Vector2(Main.rand.NextFloat(401) * player.direction, -600f);
                        break;
                    case 3:
                        position = player.Center + new Vector2(Main.rand.NextFloat(1001) * player.direction, -600f);
                        break;
                    default:
                        position = player.Center + new Vector2(Main.rand.NextFloat(401) * player.direction, -600f);
                        break;
                }

                Vector2 heading = target - position;
                {
                    if (heading.Y < 0f)
                    {
                        heading.Y *= -1f;
                    }

                    if (heading.Y < 15f)
                    {
                        heading.Y = 15f;
                    }
                    heading.Normalize();
                    heading *= velocity.Length();
                    heading.Y += Main.rand.Next(-20, 21) * 0.02f;
                }
                Projectile.NewProjectile(source, position, heading, type, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                i++;
            }
            return false;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();//意味着可以被制作
			recipe.AddIngredient(ItemID.EmpressBlade, 1);//合成路径
            recipe.AddIngredient(ItemID.PiercingStarlight, 1);
            recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
            recipe.AddIngredient(ItemID.LunarBar,7);
           
            recipe.AddTile(TileID.LunarCraftingStation);//制作需要的工作站
			recipe.Register();
		}
       
	}
}