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
    
    public class ProfanedFlames: ModBuff
    {
        private float MaxSpeed = 20f;
        public override void SetStaticDefaults() 
        {
            DisplayName.SetDefault("ProfanedFlames");
            Description.SetDefault("You are get increasingly weak\n");
            Main.debuff[Type] = true;
            Main.buffNoSave[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;

            BuffID.Sets.NurseCannotRemoveDebuff[Type] = true;
        }
        public override void Update(Player player,ref int buffIndex)
        {
            if (player.lifeRegen >= 0)
                player.lifeRegen = -60;
           else
                player.lifeRegen -=60;   
    
            player.GetDamage(DamageClass.Generic) /= 2f; 
            player.endurance -= 0.5f;
            Random ran = new Random();
            float RANDOM = ran.Next(7);//0.7-1.3间随机数
            RANDOM /= 10f;
            RANDOM += 0.7f;
        
            float V = player.velocity.Y * player.velocity.Y + player.velocity.X * player.velocity.X;
            if (V > MaxSpeed)
                player.velocity *= RANDOM;

        }

    }




}