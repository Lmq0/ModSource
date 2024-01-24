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
using test.NPCs;

namespace test.Buffs
{ 
    
    public class HolyInferno : ModBuff
    {
        public int damageindex = -100; 
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("HolyInferno");
            Description.SetDefault("You get too far from the Profaned Goddness\n");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = true;

            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player,ref int buffIndex)
        {
            if (player.lifeRegen >= 0)
                player.lifeRegen = -100;
            else
                player.lifeRegen -= 100;



        }

    }




}