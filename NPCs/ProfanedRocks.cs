using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using test.Projectiles;
using test.Projectiles.HolyRay;
using Terraria.DataStructures;
using Mono.Cecil;
using System.Security.Policy;
using IL.Terraria.GameContent;
using test.Buffs;

namespace test.NPCs.GoddnessProvidence
{

    public class ProfanedRocks : ModNPC
    {
        Random ran = new Random();
        int LIFE;
        public Player Target;
        public NPC Master;
        
        public void initialize()
        {
         
            Master = Main.npc[(int)NPC.localAI[1]];
            Target = Main.player[NPC.target];
            LIFE=Master.life;
           
        }
        public void Clean()
        {

            if (Master.life<=1)
            {
                NPC.life=0;//50血以下召唤的会在Master死亡时被清
            }
            if(Master.life<=LIFE)
            {
                Master.life=LIFE;//锁血
            }
        }
        public void Rotating(float num, bool if_distance_change = true, float BasicDistance = 450f, int speed = 80)
        {
            float rotation = (float)NPC.frameCounter % speed;
            float distance = 0;
            if (if_distance_change)
                distance = 100f * (float)Math.Sin((float)NPC.frameCounter % speed * (2 * Math.PI) / speed);
            NPC.Center = Master.Center +
                new Vector2((float)((BasicDistance + distance) * Math.Sin(Math.PI / num * NPC.localAI[0] + 2 * Math.PI / speed * rotation)),
                            (float)((BasicDistance + distance) * Math.Cos(Math.PI / num * NPC.localAI[0] + 2 * Math.PI / speed * rotation)));
        }
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 1000;
            NPC.knockBackResist = 0;
            NPC.damage = 100;
            //NPC.light = 2f;
            NPC.friendly = false;
            NPC.defense = 200;
            NPC.noTileCollide = true;//true穿墙
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.height = 50;
            NPC.width = 50;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = Item.buyPrice(0, 0, 0, 0);
            NPC.alpha = 255;

        }
        public override void AI()
        {
            if (NPC.frameCounter <= 2)
                initialize();
            NPC.frameCounter++;
            Clean();
            NPC.rotation += 0.01f;
            NPC.rotation %= (float)Math.PI * 2f;
            NPC.velocity *= 0f;
            if (NPC.frameCounter % 100 == 0)
            {
                if (Main.dayTime&&NPC.frameCounter % 300 == 0)
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Target.Center - NPC.Center, ProjectileID.CultistBossFireBall, 50, 0);
                else if(!Main.dayTime)
                {
                    Vector2 v=Target.Center - NPC.Center;
                    v.Normalize();
                    v*=30f;
                   Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Target.Center - NPC.Center, ModContent.ProjectileType<RayPreLine_thin>(), 1500, 0);
                   Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, v, ModContent.ProjectileType<LavaLances>(), 100, 0);
                }


            }
            if (NPC.scale < 1.25f)
            {
                NPC.scale += 0.01f;
            }
            else if (NPC.scale > 0.85f)
            {
                NPC.scale -= 0.01f;
            }
            Rotating(3);



            base.AI();
        }
        public override void OnKill()
        {
            ;
        }
        public override void OnSpawn(IEntitySource source)
        {
            initialize();
           

        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public override bool PreKill()
        {
            int num = 0;
            Vector2 velocity = new Vector2();
            while (num < 4)
            {
                velocity.X = ran.Next(20) -10f;
                velocity.Y = ran.Next(20) -10f;
                if (Main.dayTime)
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, velocity, ModContent.ProjectileType<HolyFireBall>(), 100, 0);
                else
                    Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Center, Target.Center-NPC.Center, ProjectileID.PhantasmalBolt, 100, 0);
                ++num;
            }
            return true;
        }
   
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {

            Texture2D texture = ModContent.Request<Texture2D>("test/NPCs/GoddnessProvidence/ProfanedRocks1").Value;
            if (!Main.dayTime)
            {
                texture = ModContent.Request<Texture2D>("test/NPCs/GoddnessProvidence/ProfanedRocks2").Value;
                drawColor= Color.White;
                drawColor.A=0;
            }
            if (Master.life<0.5*Master.lifeMax)
            {
                drawColor= Color.White;
                drawColor.A = 0;
            }
            Rectangle rectangle = new Rectangle(
                0, (int)(texture.Height / 6f * NPC.localAI[0]), texture.Width, texture.Height / 6);
            Main.EntitySpriteDraw(texture, NPC.Center - Main.screenPosition, rectangle, drawColor * (NPC.alpha / 255f), NPC.rotation,
                new Vector2(texture.Width / 2, texture.Height / 12), NPC.scale, 0, 0);


            return false;
        }






    }
}
