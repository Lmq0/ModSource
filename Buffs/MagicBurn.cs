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

namespace test.Buffs
{ 
    
    public class MagicBurn : ModBuff
    {
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("MagicBurn");
            Description.SetDefault("Your Soul is Burning By the Magic Fire\n");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;

            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player,ref int buffIndex)
        {
            if (player.lifeRegen >= 0)
                player.lifeRegen = -120;
            else
                player.lifeRegen -= 120;
          
        ; 
            player.endurance -= 0.1f;
           
        }

    }




}