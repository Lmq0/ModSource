using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using test.Projectiles;
using test.NPCs.GoddnessProvidence;

namespace test.Items.Items
{
    public class SolarCore: ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("TutorialSword"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
		
            Tooltip.SetDefault("No Consumption\n" +
                               "Wake up the Completed Profaned Goddness, Providence\n" +
                               "The time she rebirth, her power can defeat\n" +
                               "almost every god in the universe\n" +
                               "This time,she bet she herself will be the winer");
        }

		public override void SetDefaults()
		{
            
            Item.width = 20;//��
            Item.height = 20;//��
            Item.useTime = 12;//����
            Item.useAnimation = 12;
            Item.useStyle = 1;//
            Item.knockBack = 10;//����
            Item.value = 10000;//�����۸�
            Item.rare = 10;//ϡ�ж�
            Item.UseSound = SoundID.Item1;//��Ч
            Item.autoReuse = false;//�Զ�����

        }
        //EtherealLances
        public override bool CanUseItem(Player player)
        {
            return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !NPC.AnyNPCs(ModContent.NPCType<ExProfanedGoddness>());
        }
        public override bool? UseItem(Player player)
        {
            NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<ExProfanedGoddness>());
           
            return true;
        }
        public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();//��ζ�ſ��Ա�����
            recipe.AddIngredient(ItemID.FragmentSolar, 50);
            recipe.AddIngredient(ItemID.LunarBar,10);
           
            recipe.AddTile(TileID.LunarCraftingStation);//������Ҫ�Ĺ���վ
			recipe.Register();
		}
       
	}
}