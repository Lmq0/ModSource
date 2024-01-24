using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using test.Projectiles;
using test.Projectiles.HolyRay;

namespace test.Items.Weapons
{
	public class theFirstFractal: ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("TutorialSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("The Origine of all the Sword.\n" +
                                 "It is also called Lost final Sword.");
		}

		public override void SetDefaults()
		{
			Item.damage = 1000000;//�˺�
			Item.DamageType = DamageClass.Melee;//��ս�����˺�
			Item.width = 50;//��
			Item.height = 50;//��
			Item.useTime = 12;//����
			Item.useAnimation = 12;
			Item.useStyle = 7;//
			Item.knockBack = 10;//����
			Item.value = 10000;//�����۸�
			Item.rare = -14;//ϡ�ж�
			Item.UseSound = SoundID.Item1;//��Ч
			Item.autoReuse = true;//�Զ�����
            Item.shoot = ProjectileID.FirstFractal;
			Item.shootSpeed = 17f;
		}

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);//���λ��
            Vector2 origin = player.position + new Vector2(0, -800f);
            float ceilingLimit = target.Y;
            if (ceilingLimit > player.Center.Y - 200f)
            {
                ceilingLimit = player.Center.Y - 200f;
            }
            //��͸����
            Vector2 v=target - origin;
            v.Normalize();
            v*=30f;
            Random random = new Random();
            int TYPE = random.Next(1000);
            int proj=Projectile.NewProjectile(source,origin, v, TYPE, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit); ;
            if (Main.projectile[proj].active)
            {
                Main.projectile[proj].friendly=true;
                Main.projectile[proj].hostile=false;
            }
            { /*
            int times = 1;//ÿһ�����ѽ�
            while (times <= 3)
            {
                float a;
                while (true)
                {
                    Random ran = new Random();
                    a = ran.Next(360);//��ˮƽ����Ϊ�ǵĻ�׼�� ȡ25-150 210-330
                    if (Math.Abs(Math.Sin(a * 0.0175)) < 0.5)
                    {
                        continue;
                    }
                    break;
                }
                //��������Ƕ�
                float diractions = a - 180;//������
                if (diractions > 0)
                {
                    diractions = 1f;
                }
                else
                {
                    diractions = -1f;
                }
                position = player.Center - diractions * new Vector2((float)(Math.Abs(Math.Cos(a * 0.0175)) * Main.rand.NextFloat(1500) * player.direction), 600f);//������� ������׹��
                Vector2 heading = target - position;

                if (heading.Y < 15f && heading.Y > 0)
                {
                    heading.Y = 15f;
                }
                if (heading.Y > -15f && heading.Y < 0)
                {
                    heading.Y = -15f;
                }
                if (heading.Y == 0)
                {
                    continue;
                }

                heading.Normalize();
                heading *= velocity.Length();
                heading.Y += Main.rand.Next(-20, 21) * 0.02f;
                Projectile.NewProjectile(source, position, heading, type, damage * 2, knockback, player.whoAmI, 0f, ceilingLimit);
                times++;
            }
            */
            }
            return false;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();//��ζ�ſ��Ա�����
			recipe.AddIngredient(ItemID.Meowmere, 1);//�ϳ�·��
            recipe.AddIngredient(ItemID.StarWrath, 1);
            recipe.AddIngredient(ItemID.TerraBlade, 1);
            recipe.AddIngredient(ItemID.TheHorsemansBlade, 1);
            recipe.AddIngredient(ItemID.Seedler, 1);
            recipe.AddIngredient(ItemID.InfluxWaver, 1);
		    recipe.AddIngredient(ItemID.PiercingStarlight, 1);
            recipe.AddIngredient(ItemID.DD2SquireBetsySword, 1);
            //
            recipe.AddIngredient(ItemID.Frostbrand, 1);
            recipe.AddIngredient(ItemID.BeamSword, 1);
            //
            recipe.AddIngredient(ItemID.Starfury, 1);
            recipe.AddIngredient(ItemID.BeeKeeper, 1);
            recipe.AddIngredient(ItemID.EnchantedSword, 1);
            recipe.AddIngredient(ItemID.CopperShortsword, 1);
			//
            recipe.AddTile(TileID.LunarCraftingStation);//������Ҫ�Ĺ���վ
			recipe.Register();
		}
	}
}