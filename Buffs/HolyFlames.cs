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
    
    public class HolyFlames: ModBuff
    {
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("HolyFlames");
            Description.SetDefault("Dissolving from holy light\n");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;

            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player,ref int buffIndex)
        {
            if (player.lifeRegen >= 0)
                player.lifeRegen = -40;
            else
                player.lifeRegen -= 40;
            player.endurance -= 0.2f;
        }

    }




}