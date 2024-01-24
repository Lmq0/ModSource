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


namespace test.Items.Armor
{
    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Body)]
    public class BreastPlateOfLmq : ModItem
    {
        public override void SetStaticDefaults()
        {
            ;
        }
        public static int MaxManaIncrease = 20*5;
        public static int MaxMinionIncrease =1*5;
      
        public override void SetDefaults()
        {
            Item.width = 18; // Width of the item
            Item.height = 18; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = -13; // The rarity of the item
            Item.defense = 999; // The amount of defense the item will give when equipped
        }

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
     

        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)//È«Ì×Ð§¹û
        {
        
            player.setBonus = "Increases dealt damage by 10%"; // This is the setbonus tooltip
            player.GetDamage(DamageClass.Generic) += 0.1f; // Increase dealt damage for all weapon classes by 20%
        }


        public override void UpdateEquip(Player player)
        {
            ;
        }


        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            ;
        }
    }
}