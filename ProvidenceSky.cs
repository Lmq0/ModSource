using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Effects;



namespace test.NPCs.GoddnessProvidence
{
    public class ProvidenceSky:CustomSky
    {
        private bool isActive;
        private float intensity = 0f;
        private int Providence = -1;
        public override bool IsVisible()
        {
            return true;
        }
     
        public override void Activate(Vector2 position, params object[] args)
        {
            isActive = true;
        }
        public override void Deactivate(params object[] args)
        {
            isActive = false;
        }
        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {
            Texture2D texture = ModContent.Request<Texture2D>("test/NPCs/GoddnessProvidence/ProvidenceSky.png").Value;
            if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
            {
                spriteBatch.Draw(texture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.13f, 0.13f, 0.13f) * intensity);
            }
            //front of bg
            if (maxDepth >= 0 && minDepth < 0)
            {
                spriteBatch.Draw(texture, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Color(0.3f, 0.3f, 0.3f) * 0.5f);
            }
        }
        public override float GetCloudAlpha() => 0f;
        private bool UpdateExProfanedGoddnessIndex()
        {
            int ProvidenceType =ModContent.NPCType<ExProfanedGoddness>();
            if (Providence >= 0 && Main.npc[Providence].active && Main.npc[Providence].type == ProvidenceType)
                return true;

            Providence = -1;
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[Providence].type == ProvidenceType)
                {
                    Providence = i;
                    break;
                }
            }
            //this.DoGIndex = DoGIndex;
            return Providence != -1;
        }
        public override bool IsActive()
        {
            return isActive||intensity>0;  
        }
        public override void Update(GameTime gameTime)
        {
         
            if (isActive && intensity < 1f)
            {
                intensity += 0.02f;
            }
            else if (!isActive && intensity > 0f)
            {
                intensity -= 0.02f;
            }
  
        }
        public override Color OnTileColor(Color inColor)
        {
            float amt = intensity * .02f;
            return inColor.MultiplyRGB(new Color(1f - amt, 1f - amt, 1f - amt));
        }
        public override void Reset()
        {
            isActive = false;   
        }

    }
}
