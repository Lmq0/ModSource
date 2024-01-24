using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework;
using test.Projectiles;

namespace test.Items.Weapons
{
    public class CrimeAndPunishment:ModItem
    {
        bool If_Left_Click = false;
        int state = 0;
        int cd = 0;
        public override void SetDefaults()
        {
            Item.damage = 818;//伤害
            Item.DamageType = DamageClass.Melee;//近战类型伤害
            Item.width = 50;//宽
            Item.height = 50;//高
            Item.useTime = 30;//攻速
            Item.useAnimation = 12;
            Item.useStyle = 1;//
            Item.knockBack = 10;//击退
            Item.value = 10000;//卖出价格
            Item.rare = -14;//稀有度
            Item.UseSound = SoundID.Item1;//音效
            Item.autoReuse = true;//自动挥舞
            Item.shoot = ProjectileID.StarWrath;
            Item.shootSpeed = 17f;
           
        }
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse==2)
            {
                Item.shootSpeed=15f;
                Item.useTime=15;
                Item.autoReuse=true;
               
            }
            else
            {
                Item.autoReuse=false;
                Item.channel=true;
                Item.useTime=state*5;
                Item.useAnimation = 20;

            }
            return true;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse==2)
            {
                state=0;
                Vector2 Des = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                //约定a为0.4
                float a = 0.4f;
                float distance = Math.Abs((Des-player.Center).Length());
                float T = (float)Math.Sqrt((double)(distance*2/a));
                Vector2 v = Des-player.Center;
                v.Normalize();
                v*=(T*a);
                Projectile.NewProjectile(source, player.Center, v, ModContent.ProjectileType<Criminal>(), 8180, 10, player.whoAmI);
                return false;
            }
            else
            {
                int towards = (Main.mouseX+Main.screenPosition.X-   player.Center.X>0) ? 1 : -1;
                Projectile.NewProjectile(source, player.Center, new Vector2(towards, 0), ModContent.ProjectileType<CrimePunishment>(), 100, 10, Main.myPlayer);
              
            }
            return false;
        }
        public override bool? UseItem(Player player)
        {
            


            return true;
        }
        public override bool? CanBurnInLava()
        {
            return false;
        }



    }
}
