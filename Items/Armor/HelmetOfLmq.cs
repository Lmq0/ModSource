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

namespace test.Items.Armor
{

    // The AutoloadEquip attribute automatically attaches an equip texture to this item.
    // Providing the EquipType.Head value here will result in TML expecting a X_Head.png file to be placed next to the item's main texture.
    [AutoloadEquip(EquipType.Head)]
    public class HelmetOfLmq : ModItem
    {
        public override void SetStaticDefaults()
        {
            // If your head equipment should draw hair while drawn, use one of the following:
            // ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false; // Don't draw the head at all. Used by Space Creature Mask
            // ArmorIDs.Head.Sets.DrawHatHair[Item.headSlot] = true; // Draw hair as if a hat was covering the top. Used by Wizards Hat
            // ArmorIDs.Head.Sets.DrawFullHair[Item.headSlot] = true; // Draw all hair as normal. Used by Mime Mask, Sunglasses
            // ArmorIDs.Head.Sets.DrawsBackHairWithoutHeadgear[Item.headSlot] = true; 
        }

        public override void SetDefaults()
        {
            Item.width = 18; // Width of the item
            Item.height = 18; // Height of the item
            Item.value = Item.sellPrice(gold: 1); // How many coins the item is worth
            Item.rare = -13; // The rarity of the item
            Item.defense = 999; // The amount of defense the item will give when equipped
        }

        // IsArmorSet determines what armor pieces are needed for the setbonus to take effect
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {

            return body.type == ModContent.ItemType<BreastPlateOfLmq>() && legs.type == ModContent.ItemType<LeggingsOfLmq>();
        }

        // UpdateArmorSet allows you to give set bonuses to the armor.
        public override void UpdateArmorSet(Player player)
        {
            int MoveSpeedBonus = 5;
            int MaxManaIncrease = 20 * 5;
            int MaxMinionIncrease = 1 * 10;
            player.setBonus = "Increases Your Speed\n" + "Increases Dealt Damage by 30%\n"+"10 Extra Minions\n"+"200 Extra MaxMana\n"+ "Only the Minority of Debuff Can Hurt You\n"+
                "You are Immune to MagicBurn!\n";
            // This is the setbonus tooltip
            
            player.buffImmune[BuffID.OnFire] = true;// Make the player immune to Fire
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Horrified] = true;
            player.buffImmune[BuffID.TheTongue] = true;
            player.buffImmune[BuffID.CursedInferno] = true;
            player.buffImmune[BuffID.Frostburn] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;
            player.buffImmune[BuffID.Ichor] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.buffImmune[BuffID.Blackout] = true;
            player.buffImmune[BuffID.Electrified] = true;
            player.buffImmune[BuffID.MoonLeech] = true;
            player.buffImmune[BuffID.Webbed] = true;
            player.buffImmune[BuffID.Obstructed] = true;
            player.buffImmune[BuffID.WitheredArmor] = true;
            player.buffImmune[BuffID.WitheredWeapon] = true;
            player.buffImmune[BuffID.OgreSpit] = true;
            player.buffImmune[BuffID.NoBuilding] = true;
            player.buffImmune[BuffID.ShadowFlame] = true;
            player.buffImmune[ModContent.BuffType<MagicBurn>()] = true;

            player.moveSpeed += MoveSpeedBonus;
            player.statManaMax2 += MaxManaIncrease; // Increase how many mana points the player can have by 20
            player.maxMinions += MaxMinionIncrease; // Increase how many minions the player can have by one
            player.GetDamage(DamageClass.Generic) += 0.1f; // Increase dealt damage for all weapon classes by 20%
        }
            
        
        // Please see Content/ExampleRecipes.cs for a detailed explanation of recipe creation.
        public override void AddRecipes()
        {
            ;
        }
    }
}