using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using test.Buffs;
using test.Projectiles;
using test.Projectiles.HolyRay;
using Terraria.Audio;

namespace test.NPCs.GoddnessProvidence
{
    [AutoloadBossHead]
    public class ExProfanedGoddness : ModNPC
    {
        private int proj;
        private int lastlife;
        private bool if_kill = false;
        private float if_SecondState = 0;
        private bool if_protector1 = false;//δ���ɹ�����1
        private bool if_protector2 = false;
        private bool if_guardiance = false;
        private int action = -1;
        private int actionTime = 0;
        private bool if_birth = false;//�Ƿ������
        private float V = 0;//�����ٶ�ƽ��
        private float Distance = 0;//���Ծ���
        private int frameCounter = 0;//������
        private const int NPCframes = 9;//������(����ͼһ Ҳ��Ĭ��texture)
        private const int NPCcol = 2;//����֡��
        private const int LaserLength = 2000;
        //*********************************����Ϊ������
        const float FireballDistance = 100f;//���׶εĳ�ʼ��Ļ���ɾ��� ������ʥ����
        const int Time_For_Case0 = 400;//��̬����ʱ��
        const int Time_For_Case1 = 600;//��ʥ����Ч��ʱ��
        const int Time_For_Case2 = 1000;//����ʱ�� ������Զ����� ���ʱ��Զ����ʵ����Ҫ��ʱ��
        const int Time_For_Case3 = 300;//�ڶ������ʱ��
        const int Time_For_Case_Change = 500;//�л�������̬�Ļ���ʱ��
        float BeginAngle = 0;
        const int BirthTime = 200;//������ʱ200
        Vector2 TargetDifferenceVertical = new Vector2(0f, -50f);//����case0�׶ε�Ŀ���ʱ�� Ҫ�����ͷ��50f�ľ���ΪĿ��
        const float StepVelocity1 = 1100f;//case0�׶��ٶ�ƽ������1000f��ʼ���� ����ٶ������
        private Player Target => Main.player[Main.myPlayer];//ȷ��targetΪĿ��
        Vector2 lastposition = new Vector2();//��ҵ�oldposition
        Vector2 lastvelocity = new Vector2();
        //*********************************
        private Random ran = new Random();
        Random ranf = new Random(Guid.NewGuid().GetHashCode());
     
        //�����
        //**************************

        //���ڼ�¼���ⵯĻ��ȫ�ֱ��� 
        private int[] laser = new int[10];
        private float[] DesX = new float[10];
        private float[] DesY = new float[10];
        //�ڶ�������������ĳ�ʼλ��x��y
        //���漸���Ǻ�ɨ�����õ�tolaser
        private int laserTimes = 0;//��¼��������Ĵ���
        private float[] Laserinfo = new float[10];
        // parameter 0 ����ǻ����ƣ�����нǣ�
        // parameter 1 ���ɵ�������ĵ㴹ֱ���� Ĭ��1500
        // parameter 2 ����ķ�������x
        // parameter 3 ��������y
        // moving ������x��y 4 5
        // ��һ�ε�  ��ֵ  6
        // �Ƿ�ǿ���ú�/����  7 ÿ�γ�ʼ��Ϊ0
        // x�����������-25-25
        // y�������λ����-25-25

        internal float[] nowDistance = new float[10];//һ�ż�������� ��¼���˶��ٲ�

        Vector2 LinePosition;
        //***************************
        public override void SetStaticDefaults()
        {
         
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!Main.dayTime || !Main.hardMode)
                return 0f;
            else
                return base.SpawnChance(spawnInfo);
        }
        public override void OnSpawn(IEntitySource source)
        {
           
            base.OnSpawn(source);
        }
        public override void SetDefaults()
        {
            NPC.lifeMax = 400000;
            NPC.knockBackResist = 0;
            NPC.damage = 100;
            //NPC.light = 2f;
            NPC.friendly = false;
            NPC.defense = 100;
            NPC.noTileCollide = true;//true��ǽ
            NPC.noGravity = true;
            NPC.boss = true;
            NPC.height = 280;
            NPC.width = 240;
            NPC.aiStyle = -1;
            NPC.HitSound = SoundID.DD2_BetsyFlameBreath;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = Item.buyPrice(0, 0, 0, 20);
            Music = MusicID.Mushrooms;

        }
        private void painting(Rectangle rectangle, Color paintcolor, float scaleX, float scaleY,
            Texture2D paintTexture, int frames = NPCframes, int wide = NPCcol)
        {

            Main.EntitySpriteDraw(  //entityspritedraw�ǵ�Ļ��NPC�ȳ��õĻ��Ʒ���
                       paintTexture,
                       NPC.Center - Main.screenPosition,//ע�⣬����ʱ��λ��������Ļ���Ͻ�Ϊ0��
                                                        //���Ҫ�õ�Ļ���������ȥ��Ļ���Ͻǵ�����
                       rectangle,//��������������֡ͼѡ����
                       paintcolor,//���ĸ���������ɫ�������������Դ���lightcolor�������ܵ���Ȼ����Ӱ��
                       NPC.rotation,//�������������ͼ��ת����
                       new Vector2(paintTexture.Width / (2 * wide), paintTexture.Height / (2 * frames)),
                       //��������������ͼ����ԭ������꣬����дΪ��ͼ��֡���������꣬������ת�����Ŷ���Χ������
                       new Vector2(scaleX*2, scaleY*2),//���߸����������ţ�X��ˮƽ���ʣ�Y����ֱ����
                       NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.None,
                       //�ڰ˸�����������ͼƬ��תЧ������Ҫ�ֶ��ж�������spriteeffects
                       0
                       );
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {


            Texture2D texture = ModContent.Request<Texture2D>("test/NPCs/GoddnessProvidence/Providence1").Value;//ȱʡ״̬�µ�texture �ܹ���������case0;��һ�׶Σ�
            if(Main.dayTime==false)
            {
                texture = ModContent.Request<Texture2D>("test/NPCs/GoddnessProvidence/Providence2").Value;
            }

            if (action == 0 || action == 3 || action == 4 || 
                action == 2 && (actionTime < 220 || actionTime > 254) 
                || action == 9 || action == 10||action==11)
            {
                int counter = frameCounter / 10 % 8;
                if (counter >= 6)
                {
                    counter = 15 - 2 * counter;
                }

                Rectangle rectangle = new Rectangle(
                    0,
                    texture.Height * counter / NPCframes,
                    texture.Width / NPCcol,
                    texture.Height / NPCframes
                    );
                if (counter == 3 || counter == 4 || counter == 2 || counter == 5)
                {
                    if (action >= 4)
                    {
                        Color MyColor = drawColor;
                        MyColor.A = 0;
                        painting(rectangle, MyColor * 0.75f, NPC.scale + 0.09f * counter, NPC.scale + 0.09f * counter, texture);
                        painting(rectangle, MyColor * 0.5f, NPC.scale + 0.14f * counter, NPC.scale + 0.14f * counter, texture);
                        painting(rectangle, drawColor * 0.8f, NPC.scale + 0.04f * counter, NPC.scale + 0.04f * counter, texture);
                    }
                    else
                        painting(rectangle, drawColor, NPC.scale + 0.04f * counter, NPC.scale + 0.04f * counter, texture);
                }
                //����
                else
                {
                    if (action >= 4)
                    {
                        Color MyColor = drawColor;
                        MyColor.A = 0;
                        painting(rectangle, MyColor * 0.75f, NPC.scale + 0.05f * counter, NPC.scale + 0.05f * counter, texture);
                        painting(rectangle, MyColor * 0.5f, NPC.scale + 0.1f * counter, NPC.scale + 0.1f * counter, texture);
                        painting(rectangle, drawColor * 0.8f, NPC.scale, NPC.scale, texture);
                    }
                    else
                        painting(rectangle, drawColor, NPC.scale, NPC.scale, texture);
                }
            }
            //��̬���� ֡ͼʹ��0-5
            //0 1 2 3 4 5 3 1 /   
            //            6 7
            //************************************//
       
            else if (action == -1)
            {
                int counter = frameCounter / 3 % 3;
                Color MYcolor = drawColor;
                float ColorA = -0.0155f * frameCounter * frameCounter + 254f;
                MYcolor.A = (byte)ColorA;
                if (frameCounter <= 128)
                {
                    Rectangle rectangle = new Rectangle(
                   0,
                   texture.Height * (counter + 6) / NPCframes,
                   texture.Width / NPCcol,
                   texture.Height / NPCframes
                   );
                    painting(rectangle, MYcolor, NPC.scale + 0.02f * counter, NPC.scale + 0.02f * counter, texture);
                }
                else
                {


                    int counter2 = frameCounter / 8 % 9 - 1;
                    if (counter2 >= 6)
                    {
                        counter2 = 15 - 2 * counter2;
                    }
                    if (counter2 == -1)
                    {
                        counter2 = 8;
                    }
                    Rectangle rectangle = new Rectangle(
                        0,
                        texture.Height * counter2 / NPCframes,
                        texture.Width / NPCcol,
                        texture.Height / NPCframes
                        );

                    if (counter2 == 3 || counter2 == 4 || counter2 == 2 || counter2 == 5)
                    {

                        painting(rectangle, drawColor, NPC.scale + 0.02f * counter, NPC.scale + 0.02f * counter, texture);
                    }
                    //����
                    else
                    {
                        painting(rectangle, drawColor, NPC.scale, NPC.scale, texture);
                    }



                }



            }
            //��������  676767
           
            //************************************//
            else if (action == 1 || action == 8
                || action == 5 || action == 6 || 
                action == 2 && actionTime >= 220 && actionTime <= 254 || 
                action == 7 && actionTime < 40)
            {
                Rectangle rectangle = new Rectangle(
                    texture.Width / NPCcol,
                    texture.Height * 0 / NPCframes,
                    texture.Width / NPCcol,
                    texture.Height / NPCframes
                    );
                if (action >= 4)
                {
                    Color MyColor = drawColor;
                    MyColor.A = 0;
                    painting(rectangle, MyColor * 0.75f, NPC.scale + 0.05f, NPC.scale + 0.05f, texture);
                    painting(rectangle, MyColor * 0.5f, NPC.scale + 0.1f, NPC.scale + 0.1f, texture);
                    painting(rectangle, drawColor*0.8f, NPC.scale, NPC.scale, texture);
                }
                else
                painting(rectangle, drawColor, NPC.scale, NPC.scale, texture);
            }
            //������̬
            //************************************//
            else if (action == 7)
            {
                //40-60
                //90-110
                if (actionTime >= 40 && actionTime <= 60 || actionTime >= 90 && actionTime <= 110)
                {
                    int x;
                    if (actionTime >= 90)
                    {
                        x = 110 - actionTime;
                    }
                    else x = actionTime - 40;
                    int counter = frameCounter / 10 % 8;
                    if (counter >= 6)
                    {
                        counter = 15 - 2 * counter;
                    }

                    Rectangle rectangle = new Rectangle(
                        0,
                        texture.Height * counter / NPCframes,
                        texture.Width / NPCcol,
                        texture.Height / NPCframes
                        );
                    Color MyColor = drawColor;
                    MyColor.A = 0;

                    int q = -2;
                    while (q <= 2)
                    {
                        if (q != 0)
                        {
                            Main.EntitySpriteDraw(
                               texture,
                               NPC.Center - Main.screenPosition - new Vector2(x * 5f * q, 0),
                               rectangle,
                                MyColor * 0.75f * Math.Abs(1f / q) * (float)(Math.Pow(x, 2) / 400f - 0.1f * x + 1),
                               NPC.rotation,
                               new Vector2(texture.Width / 4f, texture.Height / (2 * 9)),

                               new Vector2(NPC.scale + 0.09f * counter + 0.15f * x, NPC.scale + 0.09f * counter),
                               NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.None,
                               0
                               );
                        }
                        q++;

                    }
                    if (x < 15)
                        painting(rectangle, MyColor * (float)(Math.Pow(x, 2) / -400f + 1), NPC.scale + 0.09f * counter + 0.15f * x,
                            NPC.scale + 0.09f * counter, texture);


                }

            }
            return false;
        }
        public override bool PreKill()
        {
            return true;
        }
        public override void OnKill()
        {
            ;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return false;//�޽Ӵ��˺�
        }
        public override bool CheckDead()
        {
            return if_kill;
        }
        public override bool CheckActive()
        {
            return if_kill;
        }
        private void ToSolarBreak(bool if_fireball=true,bool if_solarbreak=true)
        {
            if (frameCounter %60== 9)
            {
                LinePosition = NPC.Center + new Vector2(140f, 140f);//��¼��һ�λ����λ��
                if (if_fireball)
                {
                    proj=Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(30f, 30f), ModContent.ProjectileType<HolyBlasts6>()
                        , 50, 0, 255, 1f, 0f);//leftdown
                    Main.projectile[proj].localAI[0]=if_SecondState;
                    proj=Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(-30f, 30f), ModContent.ProjectileType<HolyBlasts6>()
                        , 50, 0, 255, 1f,0f);
                    Main.projectile[proj].localAI[0]=if_SecondState;
                    proj=Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(-30f, -30f), ModContent.ProjectileType<HolyBlasts6>()
                        , 50, 0, 255, 1f,0f);
                    Main.projectile[proj].localAI[0]=if_SecondState;
                    proj=Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(30f, -30f), ModContent.ProjectileType<HolyBlasts6>()
                        , 50, 0, 255, 1f,0f);
                    Main.projectile[proj].localAI[0]=if_SecondState;
                }

            }//����Ļ
            if (frameCounter % 60 == 25&&if_solarbreak)
            {
                int line1 = Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(1, 0), ModContent.ProjectileType<RayPreLine_thin>(),
                    2000, 0);
                int line2 = Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(-1, 0), ModContent.ProjectileType<RayPreLine_thin>(),
                   2000, 0);
                int line3 = Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(0, 1), ModContent.ProjectileType<RayPreLine_thin>(),
                   2000, 0);
                int line4 = Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(0, -1), ModContent.ProjectileType<RayPreLine_thin>(),
                   2000, 0);
                Main.projectile[line1].timeLeft=30;
                Main.projectile[line2].timeLeft=30;
                Main.projectile[line3].timeLeft=30;
                Main.projectile[line4].timeLeft=30;

                Main.projectile[line1].width=15;
                Main.projectile[line2].width=15;
                Main.projectile[line3].width=15;
                Main.projectile[line4].width=15;


            }
            if (frameCounter % 60 == 55&&if_solarbreak)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(80f, 0f), ModContent.ProjectileType<SolarBreak>(), 15, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(-80f, 0f), ModContent.ProjectileType<SolarBreak>(), 15, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(0f, -80f), ModContent.ProjectileType<SolarBreak>(), 15, 0f, Main.myPlayer, 0f, 0f);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), LinePosition, new Vector2(0f, 80f), ModContent.ProjectileType<SolarBreak>(), 15, 0f, Main.myPlayer, 0f, 0f);
            }

        }//�ƶ�ʱ�Ļ���ӹ�ì
        private void ToMove(Vector2 To_target)
        {

            if (Distance >= 1200f)//�������̫Զ
            {
                if (V > StepVelocity1)
                    NPC.velocity *= 0.5f;//���ټ���
                else
                    NPC.velocity -= (To_target + TargetDifferenceVertical) * 0.0005f;//�ٶ�̫�ͼ���

            }
            else if (Distance >= 600f)//�������һ��Զ
            {
                NPC.velocity -= (To_target + TargetDifferenceVertical) * 0.0003f;//����

                if (To_target.Y > 200f)
                {
                    NPC.velocity.Y -= 5f;//�����ұ�boss�� boss���ϼ���
                }
                if (V > StepVelocity1)
                {
                    NPC.velocity /= V / StepVelocity1;//���ٰ��ձ�������
                }
            }
            else
            {
                if (To_target.Y > 200f)
                {
                    NPC.velocity.Y -= 1f;//�����ұ�boss�� boss���ϼ���
                }
                if (NPC.velocity.Y > NPC.velocity.X)
                {
                    NPC.velocity.X *= 1.1f;//��ֹ��ֱ�ٶ�̫��
                }
                if (V > StepVelocity1)
                    NPC.velocity /= V / StepVelocity1 + 0.05f;//���ټ���

                else
                    NPC.velocity -= (To_target + TargetDifferenceVertical) * 0.0003f;//�������




            }
        }//�ƶ�
        private void ToMoveRewrite(Vector2 To_target)
        {
            if (To_target.X > 500f || To_target.X < -500f)
            {
                NPC.velocity.X -= To_target.X * 0.0003f;
            }
            //ʹ���Ҳ�಻�� Ƶ����������ҷ���
            //if(To_target.Y<400f&&NPC.velocity.Y<-16f)
            //{
            //    NPC.velocity.Y *= 0.7f;
            // }
            //ʹ��NPC��̫��
            if (V < 150f && Math.Abs(NPC.velocity.Y / NPC.velocity.X) < 0.4)
            {
                NPC.velocity.Normalize();
                NPC.velocity *= 24f;

            }
        }
        private void ToStar(int type, int damage = 100, float number = 5, float velocity = 30f, float rotatingSpeed = 1, float rotatingDirection = 1f)
        {

            if (V > 5f)
            {
                NPC.velocity /= 2.5f;

            }
            else
            {
                if (V > 0)
                {
                    NPC.velocity.X = 0f;
                    NPC.velocity.Y = 0f;

                }
                if (actionTime % 12 == 0)
                {
                    Vector2 Center = NPC.Center;

                    BeginAngle *= 3.14f / 180f;
                    int i = 0;//���������˶��ٸ���Ļ
                    while (i < number)
                    {

                        proj=Projectile.NewProjectile(NPC.GetSource_FromAI(),
                        Center +
                        new Vector2((float)(FireballDistance * Math.Cos(BeginAngle + 6.28f * i * rotatingDirection / number)),
                                   (float)(FireballDistance * Math.Sin(BeginAngle + 6.28f * i * rotatingDirection / number))),
                        new Vector2((float)(velocity * Math.Cos(BeginAngle + 6.28f * i / number)), (float)(velocity * Math.Sin(BeginAngle + 6.28f * i / number))),
                        type, damage, 0f, Main.myPlayer, if_SecondState, 0f);
                        Main.projectile[proj].localAI[0]=if_SecondState;
                        i++;
                    }
                    BeginAngle += 6.28f * rotatingSpeed / 36f;
                    BeginAngle /= 3.14f / 180f;
                    //����һȦ��Ļ

                }
            }
            BeginAngle %= 360;
        }//����
        private void ToLaser(int number1 = 1, int number2 = 2, float v = 1f, float time = 0, int type = 0)
        {
            //number1 2��Ϊ������  ����Ϊ����
            Vector2 Des_direction;

            if (actionTime == 0 - time)
            {
                if (type==0)
                {
                    int DifferenceValueX = ran.Next(200);
                    int DifferenceValueY = ran.Next(200);
                    DifferenceValueX -= 100;
                    DifferenceValueY -= 100;//�����Ŀ��������λ�õĲ�ֵ

                    Des_direction = -NPC.Center + Target.Center + new Vector2(DifferenceValueX, DifferenceValueY);
                    DesX[number1] = Des_direction.X;
                    DesY[number1] = Des_direction.Y;

                    Vector2 Ori_direction = Des_direction * -1f;
                    //�ϼ���
                    laser[number1] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Ori_direction, ModContent.ProjectileType<HolyRays>(), 2500, 0, 255);
                    //�¼���
                    laser[number2] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, Ori_direction, ModContent.ProjectileType<HolyRays>(), 2500, 0, 255);
                }
                else
                {
                    Vector2 velosity1 =Main.projectile[laser[1]].velocity;
                    Vector2 velosity2 =Main.projectile[laser[2]].velocity;
                    laser[number1]= Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velosity1, ModContent.ProjectileType<HolyRays>(), 2500, 0, 255);
                    laser[number2]= Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, velosity2, ModContent.ProjectileType<HolyRays>(), 2500, 0, 255);
                    //��ǰ���laser��Ϣ
                }
            }
            else if (actionTime <= 195)
            {
               
                float laser1Angle = Main.projectile[laser[number1]].velocity.ToRotation();
                float laser2Angle = Main.projectile[laser[number2]].velocity.ToRotation();
                if (type==0)
                {
                    laser1Angle += v * 0.0628f / 4f;
                    laser2Angle -= v * 0.0628f / 4f;
                }
                else
                {
                    laser1Angle -= v * 0.0628f / 4f;
                    laser2Angle += v * 0.0628f / 4f; 
                }
                Main.projectile[laser[number1]].velocity = new Vector2(10f * (float)Math.Cos(laser1Angle), 10f * (float)Math.Sin(laser1Angle));
                Main.projectile[laser[number1]].Center = NPC.Center;

                Main.projectile[laser[number2]].velocity = new Vector2(10f * (float)Math.Cos(laser2Angle), 10f * (float)Math.Sin(laser2Angle));
                Main.projectile[laser[number2]].Center = NPC.Center;
                //������ת

            }
            //�����ƶ�1
            else if (actionTime == 196)
            {
                NPC.velocity *= 0;
            }
            else if (actionTime == 215)
            {
                Main.projectile[laser[number1]].timeLeft = 26;
                Main.projectile[laser[number2]].timeLeft = 26;
                //�����ʼ����������
            }
            //��������  ����Ϊ��ɨ�ļ�������
            else if (actionTime == 249 && type == 0)
            {
                laser[number1] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(DesX[number1], DesY[number1]), ModContent.ProjectileType<HolyRays>(), 2500, 0, 255);
                laser[number2] = Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, new Vector2(DesX[number1], DesY[number1]), ModContent.ProjectileType<HolyRays>(), 2500, 0, 255);
            }
            //��������
            else if (actionTime > 270 && actionTime < 456 && type == 0)
            {
                float laser1Angle = Main.projectile[laser[number1]].velocity.ToRotation();
                float laser2Angle = Main.projectile[laser[number2]].velocity.ToRotation();
                laser1Angle += 0.0628f / 4.5f;
                laser2Angle -= 0.0628f / 4.5f;
                Main.projectile[laser[number1]].velocity = new Vector2(10f * (float)Math.Cos(laser1Angle), 10f * (float)Math.Sin(laser1Angle));
                Main.projectile[laser[number2]].velocity = new Vector2(10f * (float)Math.Cos(laser2Angle), 10f * (float)Math.Sin(laser2Angle));
            }
            //�����ƶ�2
            else if (actionTime == 460 && type == 0)
            {
                Main.projectile[laser[number1]].timeLeft = 26;
                Main.projectile[laser[number2]].timeLeft = 26;
            }
            //��������
            else if (actionTime >= 470)
            {
                actionTime = 1001;
            }
            //״̬����
        }//��ͨ����
        private void ToRain()
        {
            int number = ran.Next(5, 10);
            int i = 0;
            while (i < number)
            {
                float x = ran.Next(2000);
                x -= 1000;//
                x *= 2;
                float v_x = ran.Next(4, 8);
                float v_y = ran.Next(10, 20);
                v_x -= 4;
                float OripositionX = Target.position.X + x;

                Projectile.NewProjectile(NPC.GetSource_FromAI(),
                    new Vector2(OripositionX, Target.position.Y - 1500f), new Vector2(v_x, v_y),
                    ModContent.ProjectileType<FireRain>(), 15, 0f, Main.myPlayer, 0f, 0f);
                i++;
            }
        }//����
        private void ToBreak()
        {


            if (actionTime == 50)
            {
                lastposition = Target.Center;

                Main.NewText("I see you", 255, 255, 150);
                Projectile.NewProjectile(NPC.GetSource_FromAI(),
                   NPC.Center,
                   new Vector2(0f, 0f),
                   ModContent.ProjectileType<SpaceCracks>(), 150, 0f);
            }
            else if (actionTime == 90)
            {
                NPC.Center = lastposition;
                Main.NewText("You can't escape", 255, 255, 150);

            }
            else if (actionTime >= 110)
            {

                ChangeCase(-1000, 7);
            }

        }//˲��
        private void Tolaser_shape(Vector2 position, int OriginShape,
            float originangle = 0, int times = 0, float R = 1000, int lasting = 90)
        {


            Vector2[] NodeList = new Vector2[10];
            int i = 0;
            float the_angle = 6.28f / OriginShape;
            while (i < OriginShape)
            {
                NodeList[i] = position + new Vector2(R * (float)Math.Cos(i * the_angle + originangle),
                               -R * (float)Math.Sin(i * the_angle + originangle));
                i++;
            }
            i = 0;
            while (i < OriginShape)
            {
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(), NodeList[i], NodeList[(i + 1) % OriginShape] - NodeList[i],
                    ModContent.ProjectileType<HolyRays>(), 1000, 0);
                Main.projectile[proj].damage = (int)(NodeList[(i + 1) % OriginShape] - NodeList[i]).Length() - 100;//���ó���...��damage������Ҳ��û˭��
                Main.projectile[proj].timeLeft = lasting;
                Main.projectile[proj].width = 5;
                i++;
            }


        }
        //�˽Ǽ���  ����������ڻ�����εļ�������
        private void Tolaser_row(int p, Vector2 position, Vector2 target, Vector2 Move, float intervalDistance, int intervalTime, float MaxDistance, int number = 1,
            int width = 60, int length = LaserLength, int lasting = 80)
        {
            //������  Projectile Type     ��Ļ���   ��Ļ�ٶȣ���������Ϊ����  Move ǰ������������ˮƽλ�Ʒ���    internalDistance ������������ 
            //InternalTime ����������������ʱ���  MaxDistace��һ�������һ������ľ���  number �����¼ʱ��¼�ĵ�ַ  width ���  length ���ⳤ��   lastingÿ���������ʱ��

            Move.Normalize();
            target.Normalize();
            Move *= intervalDistance;//ÿһ����������λ����

            if (actionTime % intervalTime == 0 && nowDistance[number] * intervalDistance <= MaxDistance)
            {
                int proj = Projectile.NewProjectile(NPC.GetSource_FromAI(),
                    position + Move * nowDistance[number],
                    target,
                    p, 0, 0);
                Main.projectile[proj].width = width;
                Main.projectile[proj].height = length;
                Main.projectile[proj].timeLeft = lasting;
                nowDistance[number] += 1;
            }

        }
        //ƽ�м���Ļ��ƺ���
        private void NewRocks()
        {
            int i = 0;
            while (i < 6)
            {
                int rocks = NPC.NewNPC(NPC.GetSource_FromThis(),
                    (int)(NPC.Center.X + 500f * Math.Sin(Math.PI / 3f * i)),
                    (int)(NPC.Center.Y + 500f * Math.Cos(Math.PI / 3f * i)),
                    ModContent.NPCType<ProfanedRocks>(), 0, 0, 0, 0, 0, NPC.target
                    );
                //Main.npc[rocks[i]].target =NPC.target;
                Main.npc[rocks].localAI[0] = i;
                Main.npc[rocks].localAI[1] = Entity.whoAmI;
                i++;
            }
        }
        private void ToGuardiance()
        {
            int G1,G2,G3;
            float angle = frameCounter % 360f / 180f * (float)Math.PI;

            G2 = NPC.NewNPC(NPC.GetSource_FromThis(),
               (int)(NPC.Center.X + 550f * (float)Math.Sin(angle + Math.PI / 3f * 2f)),
               (int)(NPC.Center.Y + 550f * (float)Math.Cos(angle + Math.PI / 3f * 2f)),
               ModContent.NPCType<GuardianCommander>(),
               0, 0, Entity.whoAmI, 0, 0, NPC.target);
            Main.npc[G2].localAI[0] = 1;
            Main.npc[G2].localAI[1] = Entity.whoAmI;
        
            G3= NPC.NewNPC(NPC.GetSource_FromThis(),
               (int)(NPC.Center.X + 550f * (float)Math.Sin(angle + Math.PI / 3f * 4f)),
               (int)(NPC.Center.Y + 550f * (float)Math.Cos(angle + Math.PI / 3f * 4f)),
               ModContent.NPCType<GuardianDefender>(),
               0, 0, Entity.whoAmI, 0, 0, NPC.target);

            Main.npc[G3].localAI[0] = 2;
            Main.npc[G3].localAI[1] = Entity.whoAmI;

            G1 = NPC.NewNPC(NPC.GetSource_FromThis(),
             (int)(NPC.Center.X + 550f * (float)Math.Sin(angle)),
             (int)(NPC.Center.Y + 550f * (float)Math.Cos(angle)),
             ModContent.NPCType<GuardianHealer>(),
             0, 0, Entity.whoAmI, 0, 0, NPC.target);


            Main.npc[G1].localAI[0] = 0;//���
            Main.npc[G1].localAI[1] = Entity.whoAmI;
            Main.npc[G1].localAI[2]=G2;
            Main.npc[G1].localAI[3]=G3;



        }
        private void ChangeCase(int Maxtime, int OriginCase)
        {
            actionTime++;
            int RandomTime = ran.Next(320);
            RandomTime -= 160;
            if (actionTime > RandomTime + Maxtime)
            {
                switch (OriginCase)
                {
                    case 0:
                    case 4:
                        if (Distance <= 900 && actionTime > RandomTime + Maxtime)
                        {
                            actionTime = 0;
                            if (action == 0)
                                action = 1;
                            else
                            {
                                action = 6;
                                lastvelocity = NPC.velocity;
                            
                            }
                        }
                        else
                        {
                            Vector2 To_target = NPC.Center - Target.Center;
                            NPC.velocity -= To_target * 0.0002f;
                        }
                        break;
                    //�����ƶ��׶�
                    case 1:
                        if (actionTime > RandomTime + Maxtime)
                        {
                            actionTime = 0;
                            action = 3;
                            
                        }
                        break;
                    case 2:
                        if (actionTime >= Maxtime)
                        {
                            actionTime = 0;
                            action = 0;
                            
                            int i = 0;
                            while (i < 10)
                            {
                                laser[i] = 0;
                                DesY[i] = 0;
                                DesX[i] = 0;
                                i++;
                            }

                        }
                        break;
                    case 3:
                        {
                            if (Distance <= 900 && actionTime > RandomTime + Maxtime)
                            {
                                actionTime = 0;
                                action = 2;
                            }
                            else
                            {
                                Vector2 To_target = NPC.Center - Target.Center;
                                NPC.velocity -= To_target * 0.0002f;
                            }
                        }
                        break;
                    //1�׶�
                    case 5:
                        actionTime = 0;
                        action = 4;
                        Main.NewText("Let's begin", 255, 255, 150);
                        break;
                    case 6:
                        action = 7;
                        actionTime = 0;
                        break;
                    case 7:
                        if (actionTime > Maxtime)
                        {
                            action = 8;
                            actionTime = 0;
                            int iiii = 0;
                            while (iiii < 10)
                            {
                                Laserinfo[iiii] = 0;
                                iiii++;
                            }

                        }
                        break;
                    case 8:
                        if (actionTime > Maxtime)
                        {
                            action = 10;
                            actionTime = 0;
                            int ii = 0;
                            NPC.velocity = lastvelocity;
                            laserTimes = 0;
                            while (ii < 10)
                            {
                                nowDistance[ii] = 0;
                                ii++;
                            }
                        }
                        break;
                    case 9:
                        action = 4; actionTime = 0;
                        int iii = 0;
                        while (iii < 10)
                        {
                            laser[iii] = 0;
                            DesY[iii] = 0;
                            DesX[iii] = 0;
                            iii++;
                        }
                        break;
                    case 10:
                        if (actionTime >= 50 && Distance <= 800)
                        {
                            action = 9;
                            actionTime = 0;
                        }
                        else
                        {
                            Vector2 To_target = NPC.Center - Target.Center;
                            NPC.velocity -= To_target * 0.0002f;
                        }
                        break;
                    //2�׶�
                    default:
                        break;

                }

            }



        }
        private void Lifechange()
        {
            
            if (NPC.life <= 0.5 * NPC.lifeMax && action < 4)
            {
                if (action!=2)
                {
                    action = 5;//�ڶ��׶��л��׶εĳ���
                    actionTime = 0;
                    if_SecondState=1;
                }
                else NPC.life=(int)(NPC.lifeMax*0.5f);

            }
            if (NPC.life <= 0.8 * NPC.lifeMax && if_protector1 == false)
            {
                NewRocks();
                Main.NewText("Profaned Rocks has awoken!", Color.BlueViolet);
                if_protector1 = true;
            }
            if (NPC.life <= 0.4 * NPC.lifeMax && if_protector2 == false)
            {
                NewRocks();
                Main.NewText("Profaned Rocks has awoken!", Color.BlueViolet);
                if_protector2 = true;
            }
            if (NPC.life <= 0.3 * NPC.lifeMax && if_guardiance == false)
            {
                ToGuardiance();
                Main.NewText("Guardiance has awoken!", Color.BlueViolet);
                if_guardiance = true;
            }
            switch (action)
            {
                case 5:
                    if (NPC.life<NPC.lifeMax)
                        NPC.life=lastlife+=3000;
                    else
                        NPC.life=NPC.lifeMax;
                    break;
                case -1:
                    NPC.life=NPC.lifeMax;   
                    break;
                case 0:
                case 3:
                    break;
                case 1:
                case 2:
                    NPC.life += (int)(0.8 * (lastlife - NPC.life));//����80����
                    break;
                case 6:
                    NPC.life += (int)(0.3 * (lastlife - NPC.life));//30����
                    break;
                case 9:
                    NPC.life = lastlife;//����30����
                    //######��̬������Ҫ
                    break;
                case 4:
                case 10:
                case 7:
                case 8:
                    NPC.life += (int)(0.1 * (lastlife - NPC.life));//����10
                    break;
                default:
                    break;
            }
            //��������
            lastlife = NPC.life;

            if (lastlife<=1&&NPC.life<=1)
            {
                NPC.life=1;
                action=11;
                actionTime=0;

            }   
        }
       
        public override void AI()
        {

            Lighting.AddLight(NPC.Center,new Vector3(255,255,255));
            if (!if_birth&&actionTime == BirthTime)
            {
                
                if_birth = true;
                action = 0;
                actionTime = 0;
                NPC.scale = 1f;

            }
            //ִ�г�������
         
            Lifechange();
            Vector2 To_target = NPC.Center - Target.Center;//���������boss
            Distance = (float)Math.Sqrt(To_target.X * To_target.X + To_target.Y * To_target.Y);//���Ծ���
            V = NPC.velocity.X * NPC.velocity.X + NPC.velocity.Y * NPC.velocity.Y;//�����ٶ� ƽ��
            switch (action)
            {
                case -1:
                    {
                        if (actionTime==1)
                        {
                            NPC.aiStyle=120;
                            NPC.Center = Target.Center + new Vector2(0f, -600f);
                            NPC.velocity=new Vector2(0, 0.001f);
                        }
                        if (actionTime>=190)
                            NPC.aiStyle=-1;
                        if (frameCounter <= 128)
                        {
                            NPC.scale = 2 * frameCounter / 192f;
                        }
                        else
                        {
                            NPC.scale -= 1 / 144f;
                        }
                        if (NPC.alpha>0)
                            NPC.alpha-=30;
                        if (NPC.alpha<0)
                            NPC.alpha=0;
                        action = -1;
                        actionTime++;
                    }
                    break;//����
                case 0:
                    {
                        ToSolarBreak();
                        ToMove(To_target);
                        if (Target.statLife <= 0)
                        {
                            NPC.velocity.Y -= 25f;
                        }

                        ChangeCase(Time_For_Case0, 0);
                    }
                    break;//һ�׶γ�̬����
                case 1:
                    {
                     

                        ToStar(ModContent.ProjectileType<HolyBlasts>());
                        ChangeCase(Time_For_Case1, 1);
                    }
                    break;//�ƶ��ٶȱ�Ϊ0 Ȼ��ʼ���䵯Ļ
                case 2:
                    {
                    
                        if (actionTime <= 4 && V > 7f)
                        {
                            NPC.velocity *= 0.7f;
                        }//Ѹ�ټ���
                         //����ʣ�µĻ�Ҫ���̳���

                        ToLaser();
                        ChangeCase(Time_For_Case2, 2);
                    }
                    break;//����ȼ��� Ȼ�󷢼���
                case 3:
                    {
                        ToMove(To_target);
                        if (frameCounter / 6 % 8 == 3 && frameCounter % 6 == 3)
                        {
                            int i = 0;
                            float angle = 2f * 3.14f / 6f;
                            while (i < 6)
                            {
                                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position,
                                    new Vector2(30f * (float)Math.Cos(angle + i * 6.28f / 6f), 30f * (float)Math.Sin(angle + i * 6.28f / 6f)), ModContent.ProjectileType<ShiningFlames>(), 20, 0f, Main.myPlayer);
                                i++;
                            }
                        }
                        if (Target.statLife <= 0)
                        {
                            NPC.velocity.Y -= 25f;
                        }
                        ChangeCase(Time_For_Case3, 3);

                    }
                    break;//�ڶ�����״̬


                //����Ϊ��״̬�ĺ���

                case 4:
                    {
                        ToMove(To_target);
                        ToMoveRewrite(To_target);//�������move
                        //��Ҫ��ì�ĵ�Ļ 
                        if (Target.Center.Y<NPC.Center.Y)
                            ToSolarBreak(false);
                        else ToSolarBreak(true);

                        if (frameCounter / 6 % 8 == 3 && frameCounter % 6 == 3)
                        {
                            int i = 0;
                            float angle = 2f * 3.14f / 6f;
                            while (i < 6)
                            {
                                proj=Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position,
                                    new Vector2(30f * (float)Math.Cos(angle + i * 6.28f / 6f), 30f * (float)Math.Sin(angle + i * 6.28f / 6f)),
                                    ModContent.ProjectileType<ShiningFlames>(), 20, 0f, Main.myPlayer);
                                Main.projectile[proj].localAI[0]=1;
                                i++;
                            }
                        }

                        //��ϻ�
                        ChangeCase(Time_For_Case0, 4);
                        break;
                    }//�ƶ����׶�
                case 5:
                    {
                        if (actionTime == 1)
                            Main.NewText("You catch my interest", 255, 255, 150);
                        if (actionTime == 200)
                            Main.NewText("Next,I will show you the full power of sun", 255, 255, 150);
                        NPC.velocity *= 0f;
                        if (frameCounter % 10 == 0 && actionTime >= 10)
                            ToRain();
                        ChangeCase(Time_For_Case_Change, 5);
                        break;
                    }//�л��׶�state 1 -> state 2
                case 6:
                    {
                        ToStar(ModContent.ProjectileType<HolyBlasts>(), 150, 7, 25f, 1f);
                        ChangeCase(Time_For_Case1 + 200, 6);
                        break;
                    }//���� 7��
                case 7:
                    {
                        ToBreak();
                        ChangeCase(1000, 7);
                        break;
                    }//˲��
                case 8:

                    if (actionTime == 1)
                    {

                        Laserinfo[6] = Laserinfo[0];//��¼�ϴ�λ��ֵ ��0��Ϊ0
                        while (Laserinfo[1] <= 20)
                        {
                            //ÿһ����1/3���ʵõ������߻�������
                            //��������y++�� ��������x--
                            Laserinfo[2] = ranf.Next(20) + 0.1f;
                            Laserinfo[3] = -(ranf.Next(20) + 0.1f);
                            //�����ʼ��
                            Laserinfo[4] = 1;
                            Laserinfo[5] = -Laserinfo[2] / Laserinfo[3];
                            Laserinfo[0] = (float)Math.Atan(Laserinfo[2] / Math.Abs(Laserinfo[3]));
                            if (Math.Abs(Laserinfo[0] - Laserinfo[6]) > Math.PI / 4f)
                                break;
                            Laserinfo[1]++;
                        }
                        Laserinfo[9] = (int)Math.Pow(frameCounter % 27, 2) % 22;
                        if (Laserinfo[9] == 1)
                        {
                            Laserinfo[7] = 0;
                            Laserinfo[8] = 0;
                            Laserinfo[0] = 0;
                            Laserinfo[2] = 0; Laserinfo[3] = -1;
                            Laserinfo[4] = 1; Laserinfo[5] = 0;
                        }
                        else if (Laserinfo[9] == 0)
                        {
                            Laserinfo[7] = 0;
                            Laserinfo[8] = 0;
                            Laserinfo[0] = (float)Math.PI / 4;
                            Laserinfo[2] = 1; Laserinfo[3] = 0;
                            Laserinfo[4] = 0; Laserinfo[5] = -1;
                        }
                        else
                        {
                            Laserinfo[7] = ranf.Next(50);
                            Laserinfo[8] = ranf.Next(50);
                            Laserinfo[7] -= 25;
                            Laserinfo[8] -= 25;
                        }

                    }
                    if (actionTime > 1)
                    {
                        Tolaser_row(ModContent.ProjectileType<RayPreLine>(),
                            NPC.Center + new Vector2(-LaserLength/2 * (float)Math.Sin(Laserinfo[0]) + Laserinfo[7], LaserLength/2 * (float)Math.Cos(Laserinfo[0]) + Laserinfo[8]),
                            new Vector2(Laserinfo[2], Laserinfo[3]),
                            new Vector2(Laserinfo[4], Laserinfo[5]),
                            175f, 3, LaserLength / 2, 1, 15, LaserLength, 30);
                        Tolaser_row(ModContent.ProjectileType<RayPreLine>(),
                            NPC.Center + new Vector2(-LaserLength/2 * (float)Math.Sin(Laserinfo[0]) + Laserinfo[7], LaserLength/2 * (float)Math.Cos(Laserinfo[0]) + Laserinfo[8]),
                            new Vector2(Laserinfo[2], Laserinfo[3]),
                            new Vector2(-Laserinfo[4], -Laserinfo[5]),
                            175f, 3, LaserLength/2, 2, 15, LaserLength, 30);
                        //Tolaser_row(ModContent.ProjectileType<RayPreLine>(),
                        //  NPC.Center + new Vector2(0f, 1250f), new Vector2(0, -1f), new Vector2(-1f, 0), 200f, 1, 1500f, 3, 25, 2500, 55);
                    }
                    if (actionTime > 30)
                    {
                        Tolaser_row(ModContent.ProjectileType<HolyRays>(),
                            NPC.Center + new Vector2(-LaserLength/2 * (float)Math.Sin(Laserinfo[0]) + Laserinfo[7], LaserLength/2 * (float)Math.Cos(Laserinfo[0]) + Laserinfo[8]),
                            new Vector2(Laserinfo[2], Laserinfo[3]),
                            new Vector2(Laserinfo[4], Laserinfo[5]),
                            175f, 3, LaserLength/2, 4 + laserTimes % 2);
                        Tolaser_row(ModContent.ProjectileType<HolyRays>(),
                            NPC.Center + new Vector2(-LaserLength/2 * (float)Math.Sin(Laserinfo[0]) + Laserinfo[7], LaserLength/2 * (float)Math.Cos(Laserinfo[0]) + Laserinfo[8]),
                            new Vector2(Laserinfo[2], Laserinfo[3]),
                            new Vector2(-Laserinfo[4], -Laserinfo[5]),
                            175f, 3, LaserLength/2, 6 + laserTimes % 2);
                        //Tolaser_row(ModContent.ProjectileType<HolyRays>(),
                        // NPC.Center + new Vector2(0f, 1500f), new Vector2(0, -1f), new Vector2(1f, 0), 200f, 5, 1500f, 2);
                        // Tolaser_row(ModContent.ProjectileType<HolyRays>(),
                        //  NPC.Center + new Vector2(0f, 1500f), new Vector2(0, -1f), new Vector2(-1f, 0), 200f, 5, 1500f, 1);
                    }
                    if (actionTime >= 60 && laserTimes <= 8)
                    {
                        actionTime = 0;
                        laserTimes++;
                        int I = 0;
                        Laserinfo[1] = 0;
                        while (I < 10)
                        {
                            nowDistance[I] = 0;
                            I++;
                        }
                    }
                    ChangeCase(100, 8);
                    break;//����
                case 9:

                    if (actionTime <= 4 && V > 7f)
                    {
                        NPC.velocity *= 0.7f;
                    }//Ѹ�ټ���
                    ToLaser();
                    ToLaser(3, 4, 255f/175f, -40, 1);
                    ChangeCase(Time_For_Case2, 9);
                    break;//һ�׶εļ���plus 
                case 10:
                    if (actionTime == 10)
                    {
                        int pro=Projectile.NewProjectile(NPC.GetSource_FromAI(), Target.Center, new Vector2(10f, 10f), ModContent.ProjectileType<FlameCristal>(),
                            0, 0,NPC.target);
                 
                        
                        // Main.NewText("My Cristal", 255, 255, 150);
                    }
                    ToMove(To_target);
                    ToMoveRewrite(To_target);
                    ChangeCase(250, 10);
                    break;//ˮ�����`               
                case 11:
                    {
                        if(actionTime==0)
                        {
                            NPC.velocity*=0f;
                            int i = 0;
                            while(i<Main.npc.Length)
                            {
                                if (Main.npc[i].active&&
                                    (Main.npc[i].type==ModContent.NPCType<GuardianCommander>()||
                                     Main.npc[i].type==ModContent.NPCType<GuardianDefender>()||
                                     Main.npc[i].type==ModContent.NPCType<GuardianHealer>()) )
                                {
                                    Main.npc[i].life=0;
                                }

                                i++;
                            }
                            //�������
                        }
                        NPC.life=1;
                        if (frameCounter%3==0&&actionTime<1000)
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
                        if(actionTime>=1000&&actionTime<=1500)
                        {
                            ToRain();
                        }
                        if(actionTime>=1500)
                        {
                            if_kill=true;
                            NPC.life=1;
                        }




                        actionTime++;
                        break;
                    }
                    
                    //βɱ
                    
                  
                default:
                    break;
            }

            if (!(Target.statLife <= 0) && Distance > 2100)
            {

                bool if_hasbuff = Target.HasBuff(ModContent.BuffType<HolyInferno>()) && Target.statLife > 0;
                if (!if_hasbuff)
                    Main.NewText("The Holy Flames lit your wing", 255, 255, 150);
                Target.AddBuff(ModContent.BuffType<HolyInferno>(), 50);


            }
            frameCounter++;

        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.5f;
            return null;
        }
    }
}