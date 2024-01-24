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
    
    public class IcarusFolly : ModBuff
    {
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("IcarusFolly");
            Description.SetDefault("Your wing time is reduced by 33%, infinite flight is disabled\n");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;

            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player,ref int buffIndex)
        {
            player.GetDamage(DamageClass.Generic) /= 1.5f;
        }

    }




}