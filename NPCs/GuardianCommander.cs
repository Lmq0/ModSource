using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using test.Projectiles;
using System.Diagnostics.Metrics;
using test.Projectiles.HolyRay;

namespace test.NPCs.GoddnessProvidence
{
    public class GuardianCommander :ModNPC
    {
        Random ran = new Random();
        public Player Target;
        public NPC Master;
        public override void SetDefaults()
        {
            NPC.lifeMax = 10000;
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

        }
        public void Clean()
        {
          
            if (Master.life<=1)
                NPC.life=0;//50血以下召唤的会在Master死亡时被清

        }
        public override void AI()
        {
            if(NPC.frameCounter%100<=60&&NPC.frameCounter%100>=52&&NPC.frameCounter%4==0)
            {

                int horizon = ran.Next(1000);
                horizon-=500;
                int vertical = (int)Math.Sqrt((double)(1000*1000-horizon*horizon));
                
                Projectile.NewProjectile(NPC.GetSource_FromAI(), Target.Center+new Vector2(horizon, -vertical), new Vector2(-horizon, vertical),
                    ModContent.ProjectileType<RayPreLine_thin>(), 100, 0);
                Vector2 VI = new Vector2(-horizon, vertical);
                VI.Normalize(); 
                Projectile.NewProjectile(NPC.GetSource_FromAI(), Target.Center+new Vector2(horizon, -vertical), 40f*VI,
                    ModContent.ProjectileType<LavaLances>(), 50, 0);


            }
            
            if (NPC.frameCounter <= 2)
                initialize();
            NPC.velocity *= 0f;
            NPC.frameCounter++;
            Clean();
            Rotating(1.5f, false, 300f,140);

        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;
        }
        public void initialize()
        {
            Master = Main.npc[(int)NPC.localAI[1]];
            Target = Main.player[NPC.target];
        }
        public void Rotating(float num, bool if_distance_change = true, float BasicDistance = 600f, int speed = 80)
        {
            float rotation = (float)NPC.frameCounter % speed;
            float distance = 0;
            if (if_distance_change)
                distance = 100f * (float)Math.Sin((float)NPC.frameCounter % speed * (2 * Math.PI) / speed);
            NPC.Center = Master.Center +
                new Vector2((float)((BasicDistance + distance) * Math.Sin(Math.PI / num * NPC.localAI[0] + 2 * Math.PI / speed * rotation)),
                            (float)((BasicDistance + distance) * Math.Cos(Math.PI / num * NPC.localAI[0] + 2 * Math.PI / speed * rotation)));
        }
        public override void OnSpawn(IEntitySource source)
        {
            //Projectile.NewProjectile(source,);治疗光环
    
        }
        public override bool PreKill()
        {
            return true;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            return true;
        }
    }
}
