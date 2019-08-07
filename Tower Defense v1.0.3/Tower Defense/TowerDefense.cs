using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace Tower_Defense
{
    public partial class TowerDefense : Form
    {
        #region Declaration

        private GifImage SkillGif1;
        private GifImage SkillGif2;
        private GifImage SkillGif3;
        private GifImage SkillGif4;

        private Image TowerRangeGif;

        private PictureBox[] Monster;
        private PictureBox[] Skill1Location;
        private PictureBox[] Skill2Location;
        private PictureBox[] Skill3Location;
        private PictureBox[] Skill4Location;

        private int[] MonsterHP;

        private Pen RangePen = new Pen(Color.Red, 3);

        //private MonsterTimer1 m_timer1 = null;

        private Point[,] MonsterPath;
        private Point[] MonsterCurrentPos;
        private int[] PosStages;
        private int TotalMonster = 0;
        private int CurrentMonsterLimit = 11;
        private int MonsterLimit = 400;

        private string Monster1Path;
        private string PlayButtonPath;
        private string PauseButtonPath;

        private string SkillPath1;
        private string SkillPath2;
        private string SkillPath3;
        private string SkillPath4;
        private string SkillPath5;

        private string towerpath1;
        private string towerpath2;
        private string towerpath3;
        private string towerpath4;

        private string towerRangePath;

        private int level = 1;
        private int MoveDistance = 1;

        private bool RunningState = true;

        private DateTime Timer2Runtime;
        private int Timer2SpanTime;
        private DateTime Timer3Runtime;
        private int Timer3SpanTime;

        private int Timer2Interval = 10;
        private int Timer3Interval = 3000;

        private PictureBox towerselected;

        private int TotalTower = 56;
        private bool IsTowerSelected = false;

        private List<string> towerbuilt = new List<string>();

        private int[] TowerAtkRange;
        private int[] TowerTypes;

        private int CastleHP = 100;
        private int MonsterKilled = 0;

        private int TotalMoney = 100;
        private int GameSpeed = 1;

        private int TowerTechnology = 1;
        private int TowerRangeLevel = 1;

        private bool DeleteTower = false;

        #endregion

        public TowerDefense()
        {
            InitializeComponent();

            Init();

            Cursor.Current = Cursors.Hand;
        }

        private void StartAllTimer()
        {
            timer1.Enabled = true;
            //timer2.Enabled = true;
            timer3.Enabled = true;
            SkillTimer1.Enabled = true;
            SkillTimer2.Enabled = true;
            SkillTimer3.Enabled = true;
            SkillTimer4.Enabled = true;
            HpTimer.Enabled = true;
            RespawnTimer.Enabled = true;

            Timer2Runtime = DateTime.Now;
            Timer3Runtime = DateTime.Now;
        }

        private void Init()
        {
            StartAllTimer();

            Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

            GetFilePath();
            ResetAllSetting();

            DisableLocation();

            SetSkillGifImage();

            Monster = new PictureBox[MonsterLimit];
            PosStages = new Int32[MonsterLimit];
            MonsterCurrentPos = new Point[MonsterLimit];
            MonsterPath = new Point[MonsterLimit, 14];

            Skill1Location = new PictureBox[TotalTower];
            Skill2Location = new PictureBox[TotalTower];
            Skill3Location = new PictureBox[TotalTower];
            Skill4Location = new PictureBox[TotalTower];

            MonsterHP = new Int32[MonsterLimit];

            for (int i = 0; i < MonsterLimit; i++)
            {
                AssignMonsterPath(i);
            }

            CreateSkillBox();

            SetButtonLocation();

            SetTowerATKRange();
            SetTowerTypes();

            SetMonsterHP();
            //170324
            AddMonsterImage();

            AddMonster();

            timer2.Enabled = true;
        }

        private void ResetAllSetting()
        {
            //restart
            CurrentMonsterLimit = 11;
            GameSpeed = 1;
            level = 1;
            MoveDistance = 1 * GameSpeed;
            CastleHP = 100;
            MonsterKilled = 0;
            TotalMoney = 100;
            TotalMonster = 0;
            towerbuilt = new List<string>();
            MoveDistance = level;
            TowerTechnology = 1;
            TowerRangeLevel = 1;
            UpgradeTowerRangeButton.Text = "$150 Upgrade Tower ATK Range Level to 2";
            UpgradeTowerButton.Text = "$200 Upgrade Tower ATK Level to 2";
            MoneyLabel.Text = "Money: $100";
            SpeedLabel.Text = ">> X 1";
            //---
        }

        private void SetMonsterHP()
        {
            for (int i = 1; i < MonsterLimit; i++)
            {
                MonsterHP[i] = 0;
            }
        }

        private void AddMonsterImage()
        {
            GifImage gifImage2 = new GifImage(Monster1Path);
            gifImage2.ReverseAtEnd = false; //dont reverse at end

            for (int i = 1; i < MonsterLimit; i++)
            {
                Monster[i] = new PictureBox();
                Monster[i].SizeMode = PictureBoxSizeMode.Zoom;
                Monster[i].Parent = this;
                Monster[i].Image = gifImage2.GetFrame(0);
                Monster[i].Width = 80;
                Monster[i].Height = 70;
                Monster[i].Location = new Point(0, 60);
                Monster[i].BackColor = Color.Transparent;
                //Monster[i].BringToFront();
                Monster[i].Visible = false;
            }
        }

        private void CreateSkillBox()
        {
            for (int i = 1; i < TotalTower; i++)
            {
                Skill1Location[i] = new PictureBox();
                Skill1Location[i].Image = SkillGif1.GetNextFrame();
                Skill1Location[i].SizeMode = PictureBoxSizeMode.Zoom;
                Skill1Location[i].Parent = this;
                Skill1Location[i].Width = 80;
                Skill1Location[i].Height = 70;
                Skill1Location[i].BackColor = Color.Transparent;
                Skill1Location[i].Visible = false;

                Skill2Location[i] = new PictureBox();
                Skill2Location[i].Image = SkillGif2.GetNextFrame();
                Skill2Location[i].SizeMode = PictureBoxSizeMode.Zoom;
                Skill2Location[i].Parent = this;
                Skill2Location[i].Width = 80;
                Skill2Location[i].Height = 70;
                Skill2Location[i].BackColor = Color.Transparent;
                Skill2Location[i].Visible = false;

                Skill3Location[i] = new PictureBox();
                Skill3Location[i].Image = SkillGif3.GetNextFrame();
                Skill3Location[i].SizeMode = PictureBoxSizeMode.Zoom;
                Skill3Location[i].Parent = this;
                Skill3Location[i].Width = 80;
                Skill3Location[i].Height = 70;
                Skill3Location[i].BackColor = Color.Transparent;
                Skill3Location[i].Visible = false;

                Skill4Location[i] = new PictureBox();
                Skill4Location[i].Image = SkillGif4.GetNextFrame();
                Skill4Location[i].SizeMode = PictureBoxSizeMode.Zoom;
                Skill4Location[i].Parent = this;
                Skill4Location[i].Width = 80;
                Skill4Location[i].Height = 70;
                Skill4Location[i].BackColor = Color.Transparent;
                Skill4Location[i].Visible = false;
            }
        }

        private void SetTowerATKRange()
        {
            TowerAtkRange = new Int32[5];
            TowerAtkRange[1] = 120 + (TowerRangeLevel * 10);
            TowerAtkRange[2] = 120 + (TowerRangeLevel * 10);
            TowerAtkRange[3] = 120 + (TowerRangeLevel * 10);
            TowerAtkRange[4] = 120 + (TowerRangeLevel * 10);
        }

        private void SetTowerTypes()
        {
            TowerTypes = new Int32[TotalTower];
            for (int i = 1; i < TotalTower; i++)
            {
                TowerTypes[i] = 0;
            }
        }

        private void GetFilePath()
        {
            Monster1Path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\monster1.gif";
            PlayButtonPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\play.png";
            PauseButtonPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\pause.png";

            SkillPath1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\skill1.gif";
            SkillPath2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\skill2.gif";
            SkillPath3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\skill3.gif";
            SkillPath4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\skill4.gif";
            SkillPath5 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\skill5.gif";

            towerpath1 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\tower1.gif";
            towerpath2 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\tower2.gif";
            towerpath3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\tower3.gif";
            towerpath4 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\tower4.gif";

            towerRangePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName) +
                    @"\Resources\Range.png";
        }

        private void SetSkillGifImage()
        {
            SkillGif1 = new GifImage(SkillPath1);
            SkillGif2 = new GifImage(SkillPath3);
            SkillGif3 = new GifImage(SkillPath2);
            SkillGif4 = new GifImage(SkillPath5);

            TowerRangeGif = Image.FromFile(towerRangePath);
        }
        
        private void SetButtonLocation()
        {
            //Play Button
            Playbuttonbox.Image = Image.FromFile(PlayButtonPath);
            Playbuttonbox.SizeMode = PictureBoxSizeMode.Zoom;

            //Pause Button
            PauseButtonBox.Image = Image.FromFile(PauseButtonPath);
            PauseButtonBox.SizeMode = PictureBoxSizeMode.Zoom;

            //Tower1 Button
            Tower1box.Image = Image.FromFile(towerpath1);
            Tower1box.SizeMode = PictureBoxSizeMode.Zoom;

            //Tower2 Button
            Tower2box.Image = Image.FromFile(towerpath2);
            Tower2box.SizeMode = PictureBoxSizeMode.Zoom;

            //Tower3 Button
            Tower3box.Image = Image.FromFile(towerpath3);
            Tower3box.SizeMode = PictureBoxSizeMode.Zoom;

            //Tower1 Button
            Tower4box.Image = Image.FromFile(towerpath4);
            Tower4box.SizeMode = PictureBoxSizeMode.Zoom;

            //Fast Forward
            DecreaseSpeedButton.Enabled = true;
            DecreaseSpeedButton.BackColor = Color.LightGray;
            IncreaseSpeedButton.Enabled = true;
            IncreaseSpeedButton.BackColor = Color.Lime;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!RunningState)
            {
                int timerinterval2 = (Timer2Interval / GameSpeed) - Timer2SpanTime;
                int timerinterval3 = (Timer3Interval / GameSpeed) - Timer3SpanTime;

                if (Timer2SpanTime >= Timer2Interval || timerinterval2 <= 0)
                    timer2.Interval = 1;
                else
                    timer2.Interval = (Timer2Interval / GameSpeed) - Timer2SpanTime;

                if (Timer3SpanTime >= Timer3Interval || timerinterval3 <= 0)
                    timer3.Interval = 1;
                else
                    timer3.Interval = (Timer3Interval / GameSpeed) - Timer3SpanTime;
            }

            CastleHPLabel.Text = "Castle HP: " + CastleHP.ToString();
            KilledLabel.Text = "Monster Killed: " + MonsterKilled.ToString();
            LevelLabel.Text = "Level " + level.ToString();

            if (CastleHP < 1)
            {
                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = false;
                SkillTimer1.Enabled = false;
                SkillTimer2.Enabled = false;
                SkillTimer3.Enabled = false;
                SkillTimer4.Enabled = false;
                HpTimer.Enabled = false;
                RespawnTimer.Enabled = false;
                KillAllThings();
                ClearAllEffect();

                if (MessageBox.Show("You Lose! Click Ok to replay.\n", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.OK)
                {
                    Init();
                }
            }
        }

        private void UpdateMonsterPosition(int Number)
        {
            //int moveX = (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X) / 200;
            //int moveY = (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y) / 200;

            int moveX = 0;
            int moveY = 0;

            //if (moveX == 0 && (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X) > 0)
            if (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X > 0)
                moveX = MoveDistance;
            //else if (moveX == 0 && (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X) < 0)
            else if (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X < 0)
                moveX = MoveDistance  * - 1;

            //if (moveY == 0 && (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y) > 0)
            if (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y > 0)
                moveY = MoveDistance;
            //else if (moveY == 0 && (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y) < 0)
            else if (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y < 0)
                moveY = MoveDistance * -1;

            int distanceleftX = Math.Abs(MonsterPath[Number, PosStages[Number] + 1].X - MonsterCurrentPos[Number].X);
            int distanceleftY = Math.Abs(MonsterPath[Number, PosStages[Number] + 1].Y - MonsterCurrentPos[Number].Y);

            if (distanceleftX > 10)
                MonsterCurrentPos[Number].X = MonsterCurrentPos[Number].X + moveX;
            if (distanceleftY > 10)
                MonsterCurrentPos[Number].Y = MonsterCurrentPos[Number].Y + moveY;

            if (distanceleftX <= 10 && distanceleftY <= 10)
            {
                PosStages[Number]++;
                
                if (PosStages[Number] == 13)
                {
                    CastleHP -= 10;
                    PosStages[Number] = 0;
                    MonsterCurrentPos[Number] = MonsterPath[Number, 0];
                }
            }

            Monster[Number].Location = MonsterCurrentPos[Number];
        }

        private void AssignMonsterPath(int number)
        {
            MonsterPath[number,0] = new Point(0, 60);
            MonsterPath[number, 1] = new Point(278, 59);
            MonsterPath[number, 2] = new Point(276, 174);
            MonsterPath[number, 3] = new Point(472, 178);
            MonsterPath[number, 4] = new Point(477, 50);
            MonsterPath[number, 5] = new Point(693, 50);
            MonsterPath[number, 6] = new Point(698, 301);
            MonsterPath[number, 7] = new Point(480, 305);
            MonsterPath[number, 8] = new Point(480, 545);
            MonsterPath[number, 9] = new Point(705, 547);
            MonsterPath[number, 10] = new Point(707, 420);
            MonsterPath[number, 11] = new Point(907, 420);
            MonsterPath[number, 12] = new Point(912, 548);
            MonsterPath[number, 13] = new Point(1183, 550);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            MoneyLabel.Text = "Money: $" + TotalMoney.ToString();

            Timer2Runtime = DateTime.Now;
            Timer2SpanTime = 0;

            //for (int i = 1; i <= TotalMonster; i++)
            int i = 0;
            foreach (int hp in MonsterHP)
            {
                //if (MonsterHP[i] > 0)
                if (hp > 0)
                    UpdateMonsterPosition(i);

                i++;
            }

            timer2.Interval = Timer2Interval / GameSpeed;
            //170327
            MoveDistance = level * GameSpeed;
            if (level > 1)
                MoveDistance = level/2 * GameSpeed;
        }

        private void SetTimeIntervalSpeed()
        {
            SpeedLabel.Text = ">> X " + GameSpeed.ToString();

            SkillTimer1.Interval = 1000 / GameSpeed;
            HpTimer.Interval = 1000 / GameSpeed;
            SkillTimer2.Interval = 1300 / GameSpeed;
            SkillTimer3.Interval = 500 / GameSpeed;
            SkillTimer4.Interval = 2000 / GameSpeed;
            RespawnTimer.Interval = 6000 / GameSpeed;
        }

        private void AddMonster()
        {
            //GifImage gifImage2 = new GifImage(Monster1Path);
            //gifImage2.ReverseAtEnd = false; //dont reverse at end

            TotalMonster++;
            MonsterCurrentPos[TotalMonster] = MonsterPath[TotalMonster, 0];

            //Monster[TotalMonster] = new PictureBox();
            //Monster[TotalMonster].SizeMode = PictureBoxSizeMode.Zoom;
            //Monster[TotalMonster].Parent = this;
            //Monster[TotalMonster].Image = gifImage2.GetNextFrame();
            //Monster[TotalMonster].Image = gifImage2.GetFrame(0);
            //Monster[TotalMonster].Width = 80;
            //Monster[TotalMonster].Height = 70;
            Monster[TotalMonster].Location = new Point(0, 60);
            //Monster[TotalMonster].BackColor = Color.Transparent;
            Monster[TotalMonster].BringToFront();
            Monster[TotalMonster].Visible = true;

            PosStages[TotalMonster] = 0;
            MonsterHP[TotalMonster] = 150 + (level - 1) * (level - 1) * 200;
        }

        private void RespawnMonster(int monsternumber)
        {
            //GifImage gifImage2 = new GifImage(Monster1Path);
            //gifImage2.ReverseAtEnd = false; //dont reverse at end

            MonsterCurrentPos[monsternumber] = MonsterPath[TotalMonster, 0];

            //Monster[monsternumber].Image = gifImage2.GetFrame(0);
            Monster[monsternumber].Location = new Point(0, 60);
            Monster[monsternumber].BringToFront();
            Monster[monsternumber].Visible = true;

            PosStages[monsternumber] = 0;
            MonsterHP[monsternumber] = 150 + (level - 1) * (level - 1) * 100;
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Timer3Runtime = DateTime.Now;
            Timer3SpanTime = 0;

            if (TotalMonster < CurrentMonsterLimit)
                AddMonster();

            if (MonsterKilled > (10 + level * level * 10) && CurrentMonsterLimit < (10 + level * 10))
            {
                level++;
                CurrentMonsterLimit += 10;
                CastleHP += (level * 10);
            }

            timer3.Interval = Timer3Interval / GameSpeed;
        }

        private void KillAllThings()
        {
            for (int i = 1; i <= TotalMonster; i++)
            {
                Monster[i].Visible = false;
                Monster[i].Image = null;
            }

            for (int i = 1; i < TotalTower; i++)
            {
                PictureBox m_location = (PictureBox)this.Controls.Find("pictureBox" + i.ToString(), true)[0];
                m_location.Image = null;
            }
        }

        private void Playbuttonbox_Click(object sender, EventArgs e)
        {
            RunningState = true;

            timer2.Start();
            timer3.Start();
            SkillTimer1.Start();
            SkillTimer2.Start();
            SkillTimer3.Start();
            SkillTimer4.Start();
            RespawnTimer.Start();
            HpTimer.Start();

            Timer2Runtime = DateTime.Now;
            Timer3Runtime = DateTime.Now;

            PauseButtonBox.Enabled = true;
            Playbuttonbox.Enabled = false;
        }

        private void PauseButtonBox_Click(object sender, EventArgs e)
        {
            TimeSpan timer2span = DateTime.Now - Timer2Runtime;
            TimeSpan timer3span = DateTime.Now - Timer3Runtime;
            Timer2SpanTime += Convert.ToInt32(timer2span.TotalMilliseconds);
            Timer3SpanTime += Convert.ToInt32(timer3span.TotalMilliseconds);

            timer2.Stop();
            timer3.Stop();
            SkillTimer1.Stop();
            SkillTimer2.Stop();
            SkillTimer3.Stop();
            SkillTimer4.Stop();
            HpTimer.Stop();
            RespawnTimer.Stop();

            RunningState = false;
            PauseButtonBox.Enabled = false;
            Playbuttonbox.Enabled = true;
        }

        private void Towerbox_MouseClick(object sender, MouseEventArgs e)
        {
            DiselectAllTowerbox();

            if (!IsTowerSelected)
            {
                EnableLocation();

                PictureBox m_tower = (PictureBox)sender;
                m_tower.BackColor = Color.PaleGreen;
                towerselected = m_tower;
                IsTowerSelected = true;
            }
            else
            {
                DisableLocation();

                PictureBox m_tower = (PictureBox)sender;
                m_tower.BackColor = Color.Transparent;
                towerselected = null;
                IsTowerSelected = false;
            }
        }

        private void DiselectAllTowerbox()
        {
            for (int i = 1; i <= 4; i++)
            {
                PictureBox towerbox = (PictureBox)this.Controls.Find("Tower" + i.ToString() + "box", true)[0];
                towerbox.BackColor = Color.Transparent;
            }
        }

        private void EnableLocation()
        {
            int box = 1;
            while (box < TotalTower)
            {
                if (towerbuilt.Contains("pictureBox" + box.ToString()))
                {
                    box++;
                    continue;
                }

                PictureBox m_location = (PictureBox)this.Controls.Find("pictureBox" + box.ToString(), true)[0];
                m_location.BorderStyle = BorderStyle.FixedSingle;
                m_location.Enabled = true;
                box++;
            }
        }

        private void DisableLocation()
        {
            int box = 1;
            while (box < TotalTower)
            {
                PictureBox m_location = (PictureBox)this.Controls.Find("pictureBox" + box.ToString(), true)[0];
                m_location.BorderStyle = BorderStyle.None;
                //m_location.Enabled = false;
                box++;
            }
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox m_location = (PictureBox)sender;
            int m_TowerNumber = Convert.ToInt32(Regex.Match(m_location.Name, @"\d+").Value);

            if (DeleteTower)
            {
                ClearEffect(m_TowerNumber, TowerTypes[m_TowerNumber]);
                m_location.Image = null;
                towerbuilt.Remove(m_location.Name);
                RefundMoney(TowerTypes[m_TowerNumber]);
                TowerTypes[m_TowerNumber] = 0;
                DeleteTower = false;
                TowerATKRangePictureBox.Visible = false;
                DeleteButton.BackColor = Color.LightGray;
            }

            if (!towerbuilt.Contains(m_location.Name) && IsTowerSelected)
            {
                int m_TowerTypes = Convert.ToInt32(Regex.Match(towerselected.Name, @"\d+").Value);

                if (IsEnoughMoney(m_TowerTypes))
                {
                    m_location.Image = towerselected.Image;
                    towerbuilt.Add(m_location.Name);
                    TowerTypes[m_TowerNumber] = m_TowerTypes;
                }
                else
                    MessageBox.Show("No Enough $!.\n");

                towerselected.BackColor = Color.Transparent;
                m_location.BringToFront();
                IsTowerSelected = false;
                DisableLocation();
            }
        }

        private void RefundMoney(int towertypes)
        {
            switch (towertypes)
            {
                case 1:
                    TotalMoney += 10;
                    break;
                case 2:
                    TotalMoney += 13;
                    break;
                case 3:
                    TotalMoney += 5;
                    break;
                case 4:
                    TotalMoney += 20;
                    break;
            }
        }

        private bool IsEnoughMoney(int towertypes)
        {
            switch (towertypes)
            {
                case 1:
                    if (TotalMoney < 10)
                        return false;
                    TotalMoney -= 10;
                    break;
                case 2:
                    if (TotalMoney < 13)
                        return false;
                    TotalMoney -= 13;
                    break;
                case 3:
                    if (TotalMoney < 5)
                        return false;
                    TotalMoney -= 5;
                    break;
                case 4:
                    if (TotalMoney < 20)
                        return false;
                    TotalMoney -= 20;
                    break;
                case 5:
                    if (TotalMoney < (200 * TowerTechnology))
                        return false;
                    TotalMoney -= (200 * TowerTechnology);
                    break;
                case 6:
                    if (TotalMoney < (150 * TowerRangeLevel))
                        return false;
                    TotalMoney -= (150 * TowerRangeLevel);
                    break;
            }

            MoneyLabel.Text = "Money: $" + TotalMoney.ToString();

            return true;
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            if (IsTowerSelected)
                return;

            PictureBox m_location = (PictureBox)sender;

            if (towerbuilt.Contains(m_location.Name))
            {
                int TowerNumber = Convert.ToInt32(Regex.Match(m_location.Name, @"\d+").Value);
                //int Towertype = TowerTypes[TowerNumber];
                int CenterX = m_location.Location.X + (m_location.Size.Width / 2);
                int CenterY = m_location.Location.Y + (m_location.Size.Height / 2);

                int Radius = TowerAtkRange[TowerTypes[TowerNumber]];

                TowerATKRangePictureBox.Image = TowerRangeGif;
                TowerATKRangePictureBox.Size = new Size(Radius * 2, Radius * 2);
                TowerATKRangePictureBox.Parent = this;
                TowerATKRangePictureBox.Location = new Point(CenterX - Radius, CenterY - Radius);
                m_location.BringToFront();
                TowerATKRangePictureBox.Visible = true;

                //this.Invalidate();
            }                
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            if (IsTowerSelected)
                return;

            PictureBox m_location = (PictureBox)sender;

            if (towerbuilt.Contains(m_location.Name))
            {
                TowerATKRangePictureBox.Visible = false;
                //this.Invalidate();
            }
        }

        private bool IsMonsterInRange(int m_monster, int tower_number)
        {
            if (MonsterHP[m_monster] < 0)
            {
                ClearEffect(tower_number, TowerTypes[tower_number]);
                return false;
            }

            PictureBox towerbox = (PictureBox)this.Controls.Find("pictureBox" + tower_number.ToString(), true)[0];

            if (towerbox != null)
            {
                int monsterX = Monster[m_monster].Location.X + (Monster[m_monster].Size.Width / 2);
                int monsterY = Monster[m_monster].Location.Y + (Monster[m_monster].Size.Height / 2);
                int DistanceX = monsterX - (towerbox.Location.X + (towerbox.Size.Width / 2));
                int DistanceY = monsterY - (towerbox.Location.Y + (towerbox.Size.Height / 2));

                if (Math.Sqrt(DistanceX * DistanceX + DistanceY * DistanceY) < TowerAtkRange[TowerTypes[tower_number]])
                {
                    TowerAttackMonster(Monster[m_monster].Location.X, Monster[m_monster].Location.Y, TowerTypes[tower_number], m_monster, tower_number);
                    return true;
                }
                else
                {
                    ClearEffect(tower_number, TowerTypes[tower_number]);
                    return false;
                }
            }

            return false;
        }

        private void TowerAttackMonster(int Effect_X, int Effect_Y, int m_towertypes, int monster_number, int tower_number)
        {
            int damage = 0;

            switch (m_towertypes)
            {
                case 1:
                    damage = 10 + (TowerTechnology * 10/2);
                    break;
                case 2:
                    damage = 13 + (TowerTechnology * 13/2);
                    break;
                case 3:
                    damage = 5 + (TowerTechnology * 5/2);
                    break;
                case 4:
                    damage = 20 + (TowerTechnology * 20/2);
                    break;
            }

            MonsterHP[monster_number] -= damage;

            //effect on monster
            AddEffect(Effect_X, Effect_Y, tower_number, m_towertypes);
        }

        private void SkillTimer1_Tick(object sender, EventArgs e)
        {
            int count = -1;
            //for (int j = 1; j < TotalTower; j++)
            foreach (int j in TowerTypes)
            {
                count++;

                //if (TowerTypes[j] != 1)
                if (j != 1)
                    continue;

                for (int i = 1; i <= TotalMonster; i++)
                {
                    /*
                    if (MonsterHP[i] < 1)
                    {
                        ClearEffect(j, TowerTypes[j]);
                        continue;
                    }
                     */

                    //if (IsMonsterInRange(i, j))
                    if (IsMonsterInRange(i, count))
                        break;
                }
            }
        }

        private void AddEffect(int Effect_X, int Effect_Y, int TowerLocation, int m_towertypes)
        {
            //GifImage gifImage2;

            switch (m_towertypes)
            {
                case 1:
                    //gifImage2 = SkillGif1;

                    //Skill1Location[TowerLocation].Image = gifImage2.GetFrame(0);
                    //Skill1Location[TowerLocation].Image = SkillGif1.GetNextFrame();
                    Skill1Location[TowerLocation].Location = new Point(Effect_X, Effect_Y);
                    Skill1Location[TowerLocation].BringToFront();
                    Skill1Location[TowerLocation].Visible = true;
                    break;
                case 2:
                    //gifImage2 = SkillGif2;

                    //Skill2Location[TowerLocation].Image = gifImage2.GetFrame(0);
                    //Skill2Location[TowerLocation].Image = SkillGif2.GetNextFrame();
                    Skill2Location[TowerLocation].Location = new Point(Effect_X, Effect_Y);
                    Skill2Location[TowerLocation].BringToFront();
                    Skill2Location[TowerLocation].Visible = true;
                    break;
                case 3:
                    //gifImage2 = SkillGif3;

                    //Skill3Location[TowerLocation].Image = gifImage2.GetFrame(0);
                    //Skill3Location[TowerLocation].Image = SkillGif3.GetNextFrame();
                    Skill3Location[TowerLocation].Location = new Point(Effect_X, Effect_Y);
                    Skill3Location[TowerLocation].BringToFront();
                    Skill3Location[TowerLocation].Visible = true;
                    break;
                case 4:
                    //gifImage2 = SkillGif4;

                    //Skill4Location[TowerLocation].Image = gifImage2.GetFrame(0);
                    //Skill4Location[TowerLocation].Image = SkillGif4.GetNextFrame();
                    Skill4Location[TowerLocation].Location = new Point(Effect_X, Effect_Y);
                    Skill4Location[TowerLocation].BringToFront();
                    Skill4Location[TowerLocation].Visible = true;
                    break;
            }

            //gifImage2 = null;
        }

        private void ClearEffect(int TotalTower, int m_towertypes)
        {
            switch (m_towertypes)
            {
                case 1:
                    if (Skill1Location[TotalTower].Visible == true)
                    {
                        //Skill1Location[TotalTower].Image = null;
                        Skill1Location[TotalTower].Visible = false;
                    }
                    break;
                case 2:
                    if (Skill2Location[TotalTower].Visible == true)
                    {
                        //Skill2Location[TotalTower].Image = null;
                        Skill2Location[TotalTower].Visible = false;
                    }
                    break;
                case 3:
                    if (Skill3Location[TotalTower].Visible == true)
                    {
                        //Skill3Location[TotalTower].Image = null;
                        Skill3Location[TotalTower].Visible = false;
                    }
                    break;
                case 4:
                    if (Skill4Location[TotalTower].Visible == true)
                    {
                        //Skill4Location[TotalTower].Image = null;
                        Skill4Location[TotalTower].Visible = false;
                    }
                    break;
            }
        }

        private void ClearAllEffect()
        {
            for (int i = 1; i < TotalTower; i++)
            {
                if (Skill1Location[i].Visible == true)
                {
                    Skill1Location[i].Image = null;
                    Skill1Location[i].Visible = false;
                }
                if (Skill2Location[i].Visible == true)
                {
                    Skill2Location[i].Image = null;
                    Skill2Location[i].Visible = false;
                }
                if (Skill3Location[i].Visible == true)
                {
                    Skill3Location[i].Image = null;
                    Skill3Location[i].Visible = false;
                }
                if (Skill4Location[i].Visible == true)
                {
                    Skill4Location[i].Image = null;
                    Skill4Location[i].Visible = false;
                }
            }
        }

        private void HpTimer_Tick(object sender, EventArgs e)
        {
            //for (int i = 1; i <= TotalMonster; i++)
            int i = -1;
            foreach(int hp in MonsterHP)
            {
                i++;

                if (i < 1)
                    continue;

                if (hp < 0 && Monster[i].Visible == true)
                {
                    //Monster[i].Image = null;
                    Monster[i].Visible = false;

                    ///Skill1Location[i].Visible = false;
                    //Skill2Location[i].Visible = false;
                    //Skill3Location[i].Visible = false;
                    //Skill4Location[i].Visible = false;

                    MonsterKilled++;
                    TotalMoney += (2 + level);
                }
            }
        }

        private void SkillTimer2_Tick(object sender, EventArgs e)
        {
            int count = -1;

            //for (int j = 1; j < TotalTower; j++)
            foreach (int j in TowerTypes)
            {
                count++;

                //if (TowerTypes[j] != 2)
                if (j != 2)
                    continue;

                for (int i = 1; i <= TotalMonster; i++)
                {
                    /*
                    if (MonsterHP[i] < 1)
                    {
                        ClearEffect(j, TowerTypes[j]);
                        continue;
                    }
                     */

                    //if (IsMonsterInRange(i, j))
                    if (IsMonsterInRange(i, count))
                        break;
                }
            }
        }

        private void SkillTimer3_Tick(object sender, EventArgs e)
        {
            int count = -1;

            //for (int j = 1; j < TotalTower; j++)
            foreach (int j in TowerTypes)
            {
                count++;

                //if (TowerTypes[j] != 3)
                if (j != 3)
                    continue;

                for (int i = 1; i <= TotalMonster; i++)
                {
                    /*
                    if (MonsterHP[i] < 1)
                    {
                        ClearEffect(j, TowerTypes[j]);
                        continue;
                    }
                     */

                    if (IsMonsterInRange(i, count))
                        break;
                }
            }
        }

        private void SkillTimer4_Tick(object sender, EventArgs e)
        {
            int count = -1;

            //for (int j = 1; j < TotalTower; j++)
            foreach (int j in TowerTypes)
            {
                count++;

                //if (TowerTypes[j] != 4)
                if (j != 4)
                    continue;
                    
                for (int i = 1; i <= TotalMonster; i++)
                {
                    /*
                    if (MonsterHP[i] < 1)
                    {
                        ClearEffect(j, TowerTypes[j]);
                        continue;
                    }
                     */

                    if (IsMonsterInRange(i, count))
                        break;                    
                }
            }
        }

        private void RespawnTimer_Tick(object sender, EventArgs e)
        {
            int count = 0;

            //for (int i = 1; i <= TotalMonster; i++)
            foreach (int i in MonsterHP)
            {
                //if (MonsterHP[i] < 1)
                if (i < 0 && count > 0)
                {
                    RespawnMonster(count);
                    break;
                }

                count++;
            }

            //RespawnTimer.Interval = 5000 / (level + GameSpeed);
            RespawnTimer.Interval = 5000 / GameSpeed;
        }

        private void IncreaseSpeedButton_Click(object sender, EventArgs e)
        {
            if (GameSpeed == 4)
            {
                IncreaseSpeedButton.Enabled = false;
                IncreaseSpeedButton.BackColor = Color.LightGray;
                return;
            }

            GameSpeed++;

            //IncreaseSpeedButton.Enabled = false;
            if (GameSpeed > 1)
            {
                DecreaseSpeedButton.Enabled = true;
                DecreaseSpeedButton.BackColor = Color.Lime;

                if (GameSpeed == 4)
                {
                    IncreaseSpeedButton.Enabled = false;
                    IncreaseSpeedButton.BackColor = Color.LightGray;
                }
            }

            //IncreaseSpeedButton.BackColor = Color.LightGray;
            //DecreaseSpeedButton.BackColor = Color.Lime;

            SetTimeIntervalSpeed();
        }

        private void DecreaseSpeedButton_Click(object sender, EventArgs e)
        {
            if (GameSpeed == 1)
            {
                DecreaseSpeedButton.Enabled = false;
                DecreaseSpeedButton.BackColor = Color.LightGray;
                return;
            }

            GameSpeed--;

            //DecreaseSpeedButton.Enabled = false;
            if (GameSpeed < 4)
            {
                IncreaseSpeedButton.Enabled = true;
                IncreaseSpeedButton.BackColor = Color.Lime;

                if (GameSpeed == 1)
                {
                    DecreaseSpeedButton.Enabled = false;
                    DecreaseSpeedButton.BackColor = Color.LightGray;
                }
            }
            

            //DecreaseSpeedButton.BackColor = Color.LightGray;
            //IncreaseSpeedButton.BackColor = Color.Lime;

            SetTimeIntervalSpeed();
        }

        private void UpgradeTowerButton_Click(object sender, EventArgs e)
        {
            if (IsEnoughMoney(5))
            {
                TowerTechnology++;
                int moneyrequired = 200 * TowerTechnology * 2;
                int NextLevel = TowerTechnology + 1;
                UpgradeTowerButton.Text = "$" + moneyrequired.ToString() + "\n" + "Upgrade Tower ATK Level to " + NextLevel.ToString();
            }
            else
                MessageBox.Show("No Enough $!.\n");
        }

        private void UpgradeTowerRangeButton_Click(object sender, EventArgs e)
        {
            //$200 Upgrade Tower ATK Range Level to 2
            if (IsEnoughMoney(6))
            {
                TowerRangeLevel++;
                int moneyrequired = 150 * TowerRangeLevel * 2;
                int NextLevel = TowerRangeLevel + 1;
                UpgradeTowerRangeButton.Text = "$" + moneyrequired.ToString() + "\n" + "Upgrade Tower ATK Range Level to " + NextLevel.ToString();
                SetTowerATKRange();
            }
            else
                MessageBox.Show("No Enough $!.\n");
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DeleteTower = true;
            DeleteButton.BackColor = Color.Red;
        }
    }

    #region GifProcess
    public class GifImage
    {
        private Image gifImage;
        private FrameDimension dimension;
        private int frameCount;
        private int currentFrame = -1;
        private bool reverse;
        private int step = 1;

        public GifImage(string path)
        {
            gifImage = Image.FromFile(path);
            //initialize
            dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            //gets the GUID
            //total frames in the animation
            frameCount = gifImage.GetFrameCount(dimension);
        }

        public bool ReverseAtEnd
        {
            //whether the gif should play backwards when it reaches the end
            get { return reverse; }
            set { reverse = value; }
        }

        public Image GetNextFrame()
        {
            currentFrame += step;

            //if the animation reaches a boundary...
            if (currentFrame >= frameCount || currentFrame < 1)
            {
                if (reverse)
                {
                    step *= -1;
                    //...reverse the count
                    //apply it
                    currentFrame += step;
                }
                else
                {
                    currentFrame = 0;
                    //...or start over
                }
            }
            return GetFrame(currentFrame);
        }

        public Image GetFrame(int index)
        {
            gifImage.SelectActiveFrame(dimension, index);
            //find the frame
            return (Image)gifImage.Clone();
            //return a copy of it
        }
    }
    #endregion
}