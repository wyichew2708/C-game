using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;

namespace WindowsApplication1
{
    public partial class MainForm : Form
    {
        private Bitmap bm;
        private SolidBrush sb;
        private Graphics g;

        private Random number;
        private List<DropObject> DropObjects;
        private Color[] clrList = new Color[] { Color.Red, Color.Orange, Color.Blue , Color.Purple, Color.Lime, Color.Gray};
        private DropObject player;
        private bool lastDirection = false;

        private int Score;
        private RegistryKey m_HighScoreSubKey;
        private DropObject Fire;
        private int Life = 1;
        private int Level = 1;
        private int genTimerInterval = 500;
        private bool UseUltiSkill = false;
        private int UltiSkillLeft = 2;

        //private DropObject AIPlayer;
        private List<AIObject> AIPlayer;
        //private DropObject AIFire;
        private bool KillAI = false;
        private int AICount = 10;   //countdown
        private int AINum = 1;

        private List<SpreadObject> SpreadFires;

        public MainForm()
        {
            InitializeComponent();

            SpreadFires = new List<SpreadObject>();
            number = new Random();
            DropObjects = new List<DropObject>();
            AIPlayer = new List<AIObject>();
            player = new DropObject(new Point(GamePanel.Width / 2, GamePanel.Height - 44),
                                        40, Color.Black, 10);

            CreateRegistryKey();

            timer1.Enabled = true;
            genTimer.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool bBreak = false;
            // update drop object
            bm = new Bitmap(GamePanel.Width, GamePanel.Height);
            //bm = new Bitmap(WindowsApplication1.Properties.Resources.backimage, GamePanel.Width, GamePanel.Height);
            GamePanel.BackgroundImage = bm;
            g = Graphics.FromImage(bm);

            Stack<int> nRecycle = new Stack<int>();

            //0328
            if (UseUltiSkill)
            {
                DropObjects.Clear();
                UseUltiSkill = false;
            }

            //if (KillAI && AIPlayer != null)
            //    AIPlayer = null;
            if (KillAI && AIPlayer.Count > 0)
                AIPlayer.Clear();

            foreach (DropObject dObj in DropObjects)
            {
                sb = new SolidBrush(dObj.color);
                g.FillRectangle(sb, dObj.loc.X, dObj.loc.Y, dObj.size, dObj.size);
                dObj.loc.Y += dObj.speed;

                if (dObj.loc.Y >= GamePanel.Height)
                    nRecycle.Push(DropObjects.IndexOf(dObj));

                // check hit
                //if (dObj.loc.Y + 

                //0238
                if (Fire != null)
                {
                    if (TargetShot(dObj.loc.X, dObj.loc.Y, dObj.size))
                    {
                        RemoveObject(dObj);
                        break;
                    }
                }

                //0228
                //if (AIFire != null)
                if (AIPlayer.Count > 0)
                {
                    foreach (AIObject ai in AIPlayer)
                    {
                        if (AITargetShot(dObj.loc.X, dObj.loc.Y, dObj.size, ai.AIFire))
                        {
                            RemoveObject(dObj);
                            bBreak = true;
                            break;
                        }
                    }
                    if (bBreak) break;
                    /*
                    if (AITargetShot(dObj.loc.X, dObj.loc.Y, dObj.size))
                    {
                        RemoveObject(dObj);
                        break;
                    }*/
                }

                //0328 special
                if (SpreadFires.Count > 0)
                {
                    foreach (SpreadObject fires in SpreadFires)
                    {
                        /*
                        if (SpreadTargetShot(dObj.loc.X, dObj.loc.Y, dObj.size, fires))
                        {
                            RemoveObject(dObj);
                            bBreak = true;
                            break;
                        }
                         */
                    }

                    if (bBreak) break;
                }

                //0328
                if (CheckGameOver(dObj.loc.X, dObj.loc.Y, dObj.size))
                {
                    Life--;
                    if (Life > 0)
                        break;

                    SetGameOver();

                    if (MessageBox.Show("Game Over.\n",
                        "Endless Survival", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        timer1.Enabled = true;
                        genTimer.Enabled = true;
                    }
                    return;
                }
            }

            // update player position
            sb = new SolidBrush(player.color);
            g.FillRectangle(sb, player.loc.X, player.loc.Y, player.size, player.size);

            //0228
            //if (AIPlayer != null)
            if (AIPlayer.Count > 0)
            {
                /*
                sb = new SolidBrush(AIPlayer.color);
                g.FillRectangle(sb, AIPlayer.loc.X, AIPlayer.loc.Y, AIPlayer.size, AIPlayer.size);

                if (AIFire != null)
                {
                    sb = new SolidBrush(AIFire.color);
                    g.FillRectangle(sb, AIFire.loc.X, AIFire.loc.Y, AIFire.size, GamePanel.Height);
                }
                */
                foreach (AIObject ai in AIPlayer)
                {
                    sb = new SolidBrush(ai.color);
                    g.FillRectangle(sb, ai.loc.X, ai.loc.Y, ai.size, ai.size);

                    sb = new SolidBrush(ai.AIFire.color);
                    g.FillRectangle(sb, ai.AIFire.loc.X, ai.AIFire.loc.Y, ai.AIFire.size, GamePanel.Height);
                }
            }

            //0238
            if (Fire != null)
            {
                sb = new SolidBrush(Fire.color);
                g.FillRectangle(sb, Fire.loc.X, Fire.loc.Y, Fire.size, Fire.size * 3);
                Fire.loc.Y -= Fire.speed;

                if (Fire.loc.Y <= 0)
                    Fire = null;
            }

            //0328 special
            if (SpreadFires.Count > 0)
            {
                foreach (SpreadObject fires in SpreadFires)
                {
                    sb = new SolidBrush(fires.color);
                    g.FillEllipse(sb, fires.loc.X, fires.loc.Y, fires.size, fires.size);
                    fires.loc.Y -= fires.speed;

                    fires.loc.X = fires.loc.Y / 4 * (fires.type + 1);

                    if (fires.loc.Y <= 0)
                    {
                        SpreadFires.Remove(fires);
                        break;
                    }
                    else if (fires.loc.X <= 0)
                    {
                        SpreadFires.Remove(fires);
                        break;
                    }
                    else if (fires.loc.X >= GamePanel.Width)
                    {
                        SpreadFires.Remove(fires);
                        break;
                    }
                }
            }

            // Recycle
            foreach (int ni in nRecycle)
                DropObjects.RemoveAt(ni);
        }

        private void genTimer_Tick(object sender, EventArgs e)
        {
            DropObjects.Add(new DropObject(new Point(number.Next(GamePanel.Width), 0),
                                                number.Next(5, 50), clrList[number.Next(0, 6)], number.Next(10,40)));

            //0328
            Score++;
            CurrentScore.Text = Score.ToString();
            if (Score > Convert.ToInt16(m_HighScoreSubKey.GetValue("HighScore")))
                HighScore.Text = CurrentScore.Text;

            if (Score > Level * 100)
            {
                Level++;
                float dividend = Level % 5;
                if (dividend == 0)
                {
                    Life++;
                    UltiSkillLeft++;
                    CurrentLife.Text = Life.ToString();
                }
                genTimer.Interval = genTimerInterval / Level;
                //0328
                AINum = Convert.ToInt32(Math.Log(Level, 2)) + 1;
            }
            //---
        }

        private class DropObject
        {
            public Point loc;
            public int size;
            public Color color;
            public int speed;

            public DropObject(Point p, int sz, Color c, int spd)
            {
                loc = p;
                size = sz;
                color = c;
                speed = spd;
            }
        }

        private class AIObject : DropObject
        {
            public DropObject AIFire;

            public AIObject(Point p, int sz, Color c, int spd)
                : base(p, sz, c, spd)
            {
                AIFire = new DropObject(new Point(loc.X + size / 2, 0),
                                        10, Color.Pink, 30);
            }
        }

        private class SpreadObject
        {
            public Point loc;
            public int size;
            public Color color;
            public int speed;
            public int type;

            public SpreadObject(Point p, int sz, Color c, int spd, int typ)
            {
                loc = p;
                size = sz;
                color = c;
                speed = spd;
                type = typ;
            }
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                player.loc.X -= player.speed;
                lastDirection = false;
            }
            else if (e.KeyCode == Keys.Right)
            {
                player.loc.X += player.speed;
                lastDirection = true;
            }
            else if (e.KeyCode == Keys.Z)
                player.loc.X += (lastDirection ? 1 : -1) * player.speed * 10;
            //0328
            else if (e.KeyCode == Keys.C)
                Fireinthehole();
            else if (e.KeyCode == Keys.V)
                KillAllBox();
            else if (e.KeyCode == Keys.X)
                CallAIAssictance();
            else if (e.KeyCode == Keys.Up)
                StartSpreadFires();
            /*
            else if (e.KeyCode == Keys.Up)
                player.loc.Y -= player.speed;
            else if (e.KeyCode == Keys.Down)
                player.loc.Y += player.speed;
            */

            //0328
            if (player.loc.X < 0)
                player.loc.X = 0;
            else if (player.loc.X + player.size > GamePanel.Width)
                player.loc.X = GamePanel.Width - player.size;
            //---
        }

        private bool CheckGameOver(int BlockX, int BlockY, int BlockSize)
        {
            if ((BlockX < player.loc.X && (BlockX + BlockSize) > player.loc.X)
                || (BlockX < (player.loc.X + player.size) && (BlockX + BlockSize) > (player.loc.X + player.size)))
            {
                if (BlockY + BlockSize > player.loc.Y)
                    return true;
            }
            else if ((BlockX > player.loc.X && BlockX < (player.loc.X + player.size))
                || ((BlockX + BlockSize) > player.loc.X && (BlockX + BlockSize) < (player.loc.X + player.size)))
            {
                if (BlockY + BlockSize > player.loc.Y)
                    return true;
            }

            return false;
        }

        private void SetGameOver()
        {
            AICount = 0;
            DropObjects.Clear();
            Fire = null;
            Life = 1;
            CurrentLife.Text = "1";

            timer1.Enabled = false;
            genTimer.Enabled = false;
            genTimer.Interval = genTimerInterval;

            m_HighScoreSubKey.SetValue("HighScore", Score);
            Score = 0;
            Level = 1;
            //0328
            AINum = 1;

            UltiSkillLeft = 2;
            UltiSkillLeftLabel.Text = UltiSkillLeft.ToString();
        }

        private void CreateRegistryKey()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
            m_HighScoreSubKey = key.CreateSubKey("Endless Falling");
            HighScore.Text = m_HighScoreSubKey.GetValue("HighScore",0).ToString();
        }

        private void Fireinthehole()
        {
            if (Fire == null)
                Fire = new DropObject(new Point(player.loc.X + player.size / 2, player.loc.Y),
                                        10, Color.Red, 30);
        }

        private void RemoveObject(DropObject item)
        {
            Score += 10;
            Fire = null;
            DropObjects.Remove(item);
        }

        private bool TargetShot(int BlockX, int BlockY, int Size)
        {
            for (int i = Fire.loc.X; i <= Fire.loc.X + Fire.size; i++)
            {
                if (i >= BlockX && i <= BlockX + Size)
                {
                    for (int j = Fire.loc.Y; j <= Fire.loc.Y + Fire.size * 3; j++)
                    {
                        if (j >= BlockY && j <= BlockY + Size)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool AITargetShot(int BlockX, int BlockY, int Size, DropObject aiFire)
        {
            for (int i = aiFire.loc.X; i <= aiFire.loc.X + aiFire.size; i++)
            {
                if (i >= BlockX && i <= BlockX + Size)
                {
                    for (int j = aiFire.loc.Y; j <= aiFire.loc.Y + GamePanel.Height; j++)
                    {
                        if (j >= BlockY && j <= BlockY + Size)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool SpreadTargetShot(int BlockX, int BlockY, int Size, SpreadObject SpreadFires)
        {
            for (int i = SpreadFires.loc.X; i <= SpreadFires.loc.X + SpreadFires.size; i++)
            {
                if (i >= BlockX && i <= BlockX + Size)
                {
                    for (int j = SpreadFires.loc.Y; j <= SpreadFires.loc.Y + GamePanel.Height; j++)
                    {
                        if (j >= BlockY && j <= BlockY + Size)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void KillAllBox()
        {
            if (!UseUltiSkill && UltiSkillLeft > 0)
            {
                UseUltiSkill = true;

                UltiSkillLeft--;
                UltiSkillLeftLabel.Text = UltiSkillLeft.ToString();
            }
        }

        private void CallAIAssictance()
        {
            //if (AIPlayer == null)
            if (AIPlayer.Count == 0)
            {
                AICount = 10;
                KillAI = false;
                //AIPlayer = new DropObject(new Point(GamePanel.Width / 2, GamePanel.Height - 44),
                //                        40, Color.LightBlue, 10);
                for (int i = 0; i < AINum; i++)
                    AIPlayer.Add(new AIObject(new Point(GamePanel.Width / 2, GamePanel.Height - 44),
                                        40, Color.LightBlue, 10));

                AITimer.Enabled = true;
            }
        }

        private void AITimer_Tick(object sender, EventArgs e)
        {
            //AIPlayer.loc.X = number.Next(0, GamePanel.Width - AIPlayer.size);
            //AIFire = new DropObject(new Point(AIPlayer.loc.X + AIPlayer.size / 2, 0),
            //                            10, Color.Pink, 30);
            foreach (AIObject ai in AIPlayer)
            {
                ai.loc.X = number.Next(0, GamePanel.Width - ai.size);
                ai.AIFire = new DropObject(new Point(ai.loc.X + ai.size / 2, 0),
                                        10, Color.Pink, 30);
            }

            if (AICount < 0)
            {
                //AIFire = null;
                KillAI = true;
                AITimer.Enabled = false;
            }

            AICount--;
        }

        private void StartSpreadFires()
        {
            for (int i = 0; i < 10; i++)
                SpreadFires.Add(new SpreadObject(new Point(player.loc.X + player.size / 2, player.loc.Y),
                                    20, Color.Black, 20, i));

        }

    }
}