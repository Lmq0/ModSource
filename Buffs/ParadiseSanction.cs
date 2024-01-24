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
    
    public class ParadiseSanction: ModBuff
    {
        private float MaxSpeedX = 22f;
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("ParadiseSanction");
            Description.SetDefault("You are get increasingly weak\n");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;

            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player,ref int buffIndex)
        {
            if (player.lifeRegen >= 0)
                player.lifeRegen = 0;
           
    
            player.GetDamage(DamageClass.Generic) /= 2f; 
            player.endurance -= 0.25f;
            player.moveSpeed *= 0.7f;
            float V = player.velocity.Y * player.velocity.Y + player.velocity.X * player.velocity.X;
            if (V > MaxSpeedX)
                player.velocity.X *= 0.85f;
        }

    }




}