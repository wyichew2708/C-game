using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Tower_Defense
{
    class MonsterTimer1
    {
        public Point[,] MonsterPath;
        public Point[] MonsterCurrentPos;
        public int[] PosStages;

        public int TotalMonster = 0;
        public int level = 0;

        public int MonsterLimit = 12;

        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        public MonsterTimer1()
        {
            PosStages = new Int32[MonsterLimit];
            MonsterCurrentPos = new Point[MonsterLimit];
            MonsterPath = new Point[MonsterLimit, 14];

            for (int i = 0; i < MonsterLimit; i++)
            {
                AssignMonsterPath(i);
            }

            timer1.Interval = 10;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 1; i <= TotalMonster; i++)
            {
                UpdateMonsterPosition(i);
            }
        }
        /*
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (TotalMonster < 11)
                AddMonster();
        }
         */

        public void AssignMonsterPath(int number)
        {
            MonsterPath[number, 0] = new Point(0, 60);
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

        public void UpdateMonsterPosition(int Number)
        {
            int moveX = (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X) / 200;
            int moveY = (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y) / 200;

            if (moveX == 0 && (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X) > 0)
                moveX = 1;
            else if (moveX == 0 && (MonsterPath[Number, PosStages[Number] + 1].X - MonsterPath[Number, PosStages[Number]].X) < 0)
                moveX = -1;

            if (moveY == 0 && (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y) > 0)
                moveY = 1;
            else if (moveY == 0 && (MonsterPath[Number, PosStages[Number] + 1].Y - MonsterPath[Number, PosStages[Number]].Y) < 0)
                moveY = -1;

            if (Math.Abs(MonsterPath[Number, PosStages[Number] + 1].X - MonsterCurrentPos[Number].X) > 10)
                MonsterCurrentPos[Number].X = MonsterCurrentPos[Number].X + moveX;
            if (Math.Abs(MonsterPath[Number, PosStages[Number] + 1].Y - MonsterCurrentPos[Number].Y) > 10)
                MonsterCurrentPos[Number].Y = MonsterCurrentPos[Number].Y + moveY;

            if (Math.Abs(MonsterPath[Number, PosStages[Number] + 1].X - MonsterCurrentPos[Number].X) <= 10
                && Math.Abs(MonsterPath[Number, PosStages[Number] + 1].Y - MonsterCurrentPos[Number].Y) <= 10)
                PosStages[Number]++;

            if (PosStages[Number] == 13)
            {
                PosStages[Number] = 0;
                MonsterCurrentPos[Number] = MonsterPath[Number, 0];
            }

            //Monster[Number].Location = MonsterCurrentPos[Number];
        }
    }
}
