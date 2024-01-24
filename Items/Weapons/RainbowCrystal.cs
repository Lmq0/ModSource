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
            Item.damage = 212;//�˺�
            Item.DamageType = DamageClass.Melee;//��ս�����˺�
            Item.width = 30;//��
            Item.height = 100;//��
            Item.useTime = 12;//����
            Item.useAnimation = 12;
            Item.useStyle = 1;//
            Item.knockBack = 10;//����
            Item.value = 10000;//�����۸�
            Item.rare = 10;//ϡ�ж�
            Item.UseSound = SoundID.Item1;//��Ч
            Item.autoReuse = true;//�Զ�����


            Item.shoot = ModContent.ProjectileType<FireRain>();//FairyQueenRangedItemShot
            Item.shootSpeed = 25f;
        }
        //EtherealLances




        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
    
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);//���λ��
            float ceilingLimit = target.Y;//Ӧ���Ǵ�����ʧ���ľ���
            int i = 1;
            while (i <= 3)
            {
                switch (i)
                {
                    case 1:
                        position = player.Center - new Vector2(Main.rand.NextFloat(1301) * player.direction, 600f);//������� ������׹��
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
			Recipe recipe = CreateRecipe();//��ζ�ſ��Ա�����
			recipe.AddIngredient(ItemID.EmpressBlade, 1);//�ϳ�·��
            recipe.AddIngredient(ItemID.PiercingStarlight, 1);
            recipe.AddIngredient(ItemID.DaedalusStormbow, 1);
            recipe.AddIngredient(ItemID.LunarBar,7);
           
            recipe.AddTile(TileID.LunarCraftingStation);//������Ҫ�Ĺ���վ
			recipe.Register();
		}
       
	}
}