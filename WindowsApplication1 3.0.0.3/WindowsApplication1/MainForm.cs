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
        private int GameSize = 0;
        private DataGridViewRow newRow;
        private int[,] m_nMap;
        private List<string> BoomMapLists = new List<string>();
        private int MaxI = 0;
        private int MaxJ = 0;

        private bool Status = false;

        DateTime m_tClockStart, m_tClockEnd;
        TimeSpan m_tsDiff;
        int m_nLapsedSecond;

        Color FocusColor = Color.LightSkyBlue;

        private RegistryKey m_HighScorebyTypeSubKey;

        public MainForm()
        {
            InitializeComponent();

            comboBox1.Items.Add("10X10");
            comboBox1.Items.Add("20X20");
            comboBox1.Items.Add("30X30");
            comboBox1.SelectedIndex = 0;

            Cursor.Current = Cursors.Hand;

            timer1.Enabled = true;

            m_tClockStart = DateTime.Now;

            UpdateGameView();
            UpdateBOOMPosition();

            CreateRegistryKey();
            UpdateHighScore();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameSize = comboBox1.SelectedIndex;
            textBox1.Text = "50";

            UpdateGameView();
            UpdateBOOMPosition();

            CreateRegistryKey();
            UpdateHighScore();

            GameView.Enabled = true;
            timer1.Enabled = true;
            m_tClockStart = DateTime.Now;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                switch (GameSize)
                {
                    case 0:
                        if (Convert.ToInt32(textBox1.Text) <= 0 || Convert.ToInt32(textBox1.Text) >= 100)
                            textBox1.Text = "50";
                        break;
                    case 1:
                        if (Convert.ToInt32(textBox1.Text) <= 0 || Convert.ToInt32(textBox1.Text) >= 400)
                            textBox1.Text = "200";
                        break;
                    case 2:
                        if (Convert.ToInt32(textBox1.Text) <= 0 || Convert.ToInt32(textBox1.Text) >= 900)
                            textBox1.Text = "450";
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("please Key In Correct Number.\n");
                textBox1.Text = "50";
            }

            GameView.Enabled = true;
            UpdateGameView();
            UpdateBOOMPosition();

            timer1.Enabled = true;
            m_tClockStart = DateTime.Now;

            CreateRegistryKey();
            UpdateHighScore();
        }

        private void UpdateGameView()
        {
            GameView.Rows.Clear();
            GameView.Columns.Clear();

            switch (GameSize)
            {
                case 0:
                    MaxI = 9;
                    MaxJ = 9;

                    GameView.Size = new Size(200,200);
                    GameView.ColumnCount = 10;

                    for (int i = 0; i < 10; i++)
                    {
                        newRow = new DataGridViewRow();
                        newRow.Height = 20;
                        newRow.CreateCells(GameView, new object[] {" "," "," "," "," "," "," "," "," "," "});
                        GameView.Rows.Add(newRow);
                    }

                    break;
                case 1:
                    MaxI = 19;
                    MaxJ = 19;

                    GameView.Size = new Size(400, 400);
                    GameView.ColumnCount = 20;

                    for (int i = 0; i < 20; i++)
                    {
                        newRow = new DataGridViewRow();
                        newRow.Height = 20;
                        newRow.CreateCells(GameView, new object[] { " "," "," "," "," "," "," "," "," "," ",
                        " "," "," "," "," "," "," "," "," "," "});
                        GameView.Rows.Add(newRow);
                    }
                    break;
                case 2:
                    MaxI = 29;
                    MaxJ = 29;

                    GameView.Size = new Size(600, 600);
                    GameView.ColumnCount = 30;

                    for (int i = 0; i < 30; i++)
                    {
                        newRow = new DataGridViewRow();
                        newRow.Height = 20;
                        newRow.CreateCells(GameView, new object[] { " "," "," "," "," "," "," "," "," "," ",
                        " "," "," "," "," "," "," "," "," "," ",
                        " "," "," "," "," "," "," "," "," "," "});
                        GameView.Rows.Add(newRow);
                    }
                    break;
            }
        }

        private void UpdateBOOMPosition()
        {
            BoomMapLists.Clear();
            m_nMap = new int[900, 900];

            int BoomNumber = Convert.ToInt32(textBox1.Text);
            int i;
            int j;
            Random number = new Random();

            switch (GameSize)
            {
                case 0:

                    for (int ii = 0; ii < BoomNumber; ii++)
                    {
                        i = number.Next(0,9);
                        j = number.Next(0,9);

                        if (BoomMapLists.Contains(i.ToString() + "_" + j.ToString()))
                        {
                            ii--;
                            continue;
                        }

                        BoomMapLists.Add(i.ToString()+"_"+j.ToString());
                        m_nMap[i, j] = 1;
                    }

                    break;
                case 1:

                    for (int ii = 0; ii < BoomNumber; ii++)
                    {
                        i = number.Next(0, 19);
                        j = number.Next(0, 19);

                        if (BoomMapLists.Contains(i.ToString() + "_" + j.ToString()))
                        {
                            ii--;
                            continue;
                        }

                        BoomMapLists.Add(i.ToString() + "_" + j.ToString());
                        m_nMap[i, j] = 1;
                    }

                    break;
                case 2:

                    for (int ii = 0; ii < BoomNumber; ii++)
                    {
                        i = number.Next(0, 29);
                        j = number.Next(0, 29);

                        if (BoomMapLists.Contains(i.ToString() + "_" + j.ToString()))
                        {
                            ii--;
                            continue;
                        }

                        BoomMapLists.Add(i.ToString() + "_" + j.ToString());
                        m_nMap[i, j] = 1;
                    }

                    break;
            }
        }

        private void GameView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == " ")
                    {
                        GameView[e.ColumnIndex, e.RowIndex].Value = "#";

                        GameView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Yellow;
                    }
                    else if (GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == "#")
                    {
                        GameView[e.ColumnIndex, e.RowIndex].Value = "?";

                        GameView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Gray;
                    }
                    else if (GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == "?")
                    {
                        GameView[e.ColumnIndex, e.RowIndex].Value = " ";

                        GameView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
                    }  
                }
                else
                {
                    if (//GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == " " ||
                               //GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == "0" ||
                               GameView[e.ColumnIndex, e.RowIndex].Value.ToString() != "#")
                               //GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == "?")
                    {
                        int j = e.RowIndex;
                        int i = e.ColumnIndex;
                        //170321
                        int result = CheckFlags(i, j);

                        if (m_nMap[i, j] == 1 || result == -1) //BOOM
                        {
                            GameView.ClearSelection();
                            GameView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;

                            ShowAllBOOM();
                            GameView.ClearSelection();

                            timer1.Enabled = false;
                            GameView.Enabled = false;

                            MessageBox.Show("Game Over!! You Lose!!");
                        }
                        // 170321
                        else if (result > 0)
                        {
                            GameView.CurrentCell = GameView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                            if (result == 1)
                            {
                                int SurroundingBOOM = CheckSurroundingBOOM(i, j);
                                //GameView[e.ColumnIndex, e.RowIndex].Value = SurroundingBOOM;
                                //GameView[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.PaleGreen;
                            }
                            OpenRemainingGreen();

                            if (Status)
                            {
                                timer1.Enabled = false;
                                Status = false;
                                GameView.Enabled = false;
                                MessageBox.Show("Horay!!You Win!!\n");

                                DisableSomeButton();
                            }
                        }
                    }
                    else if (GameView[e.ColumnIndex, e.RowIndex].Value.ToString() == "#")
                    {
                    }
                }
            }
        }

        private int CheckSurroundingBOOM(int i, int j)
        {
            int nloop = 0;
            while (true)
            {
                int SurroundingBOOM = 0;
                if (GameView[i, j].Value.ToString() == " " || GameView[i, j].Value.ToString() == "?")
                {
                    if (i == 0 && j == 0)
                    {
                        SurroundingBOOM += CountBoom(i + 1, j);
                        SurroundingBOOM += CountBoom(i, j + 1);
                        SurroundingBOOM += CountBoom(i + 1, j + 1);
                    }
                    else if (i == 0 && j == MaxJ)
                    {
                        SurroundingBOOM += CountBoom(i + 1, j);
                        SurroundingBOOM += CountBoom(i, j - 1);
                        SurroundingBOOM += CountBoom(i + 1, j - 1);
                    }
                    else if (i == MaxI && j == 0)
                    {
                        SurroundingBOOM += CountBoom(i - 1, j);
                        SurroundingBOOM += CountBoom(i, j + 1);
                        SurroundingBOOM += CountBoom(i - 1, j + 1);
                    }
                    else if (i == 0)
                    {
                        SurroundingBOOM += CountBoom(i + 1, j);
                        SurroundingBOOM += CountBoom(i, j + 1);
                        SurroundingBOOM += CountBoom(i + 1, j + 1);
                        SurroundingBOOM += CountBoom(i, j - 1);
                        SurroundingBOOM += CountBoom(i + 1, j - 1);
                    }
                    else if (j == 0)
                    {
                        SurroundingBOOM += CountBoom(i + 1, j);
                        SurroundingBOOM += CountBoom(i, j + 1);
                        SurroundingBOOM += CountBoom(i + 1, j + 1);
                        SurroundingBOOM += CountBoom(i - 1, j);
                        SurroundingBOOM += CountBoom(i - 1, j + 1);
                    }
                    else if (i == MaxI && j == MaxJ)
                    {
                        SurroundingBOOM += CountBoom(i - 1, j - 1);
                        SurroundingBOOM += CountBoom(i - 1, j);
                        SurroundingBOOM += CountBoom(i, j - 1);
                    }
                    else if (j == MaxJ)
                    {
                        SurroundingBOOM += CountBoom(i - 1, j - 1);
                        SurroundingBOOM += CountBoom(i - 1, j);
                        SurroundingBOOM += CountBoom(i, j - 1);
                        SurroundingBOOM += CountBoom(i + 1, j - 1);
                        SurroundingBOOM += CountBoom(i + 1, j);
                    }
                    else if (i == MaxI)
                    {
                        SurroundingBOOM += CountBoom(i - 1, j - 1);
                        SurroundingBOOM += CountBoom(i - 1, j);
                        SurroundingBOOM += CountBoom(i, j + 1);
                        //wychew 170321
                        //SurroundingBOOM += CountBoom(i, j);
                        SurroundingBOOM += CountBoom(i - 1, j + 1);
                        SurroundingBOOM += CountBoom(i, j - 1);
                    }
                    else
                    {
                        SurroundingBOOM += CountBoom(i - 1, j - 1);
                        SurroundingBOOM += CountBoom(i - 1, j);
                        SurroundingBOOM += CountBoom(i, j - 1);
                        SurroundingBOOM += CountBoom(i - 1, j + 1);
                        SurroundingBOOM += CountBoom(i, j + 1);
                        SurroundingBOOM += CountBoom(i + 1, j);
                        SurroundingBOOM += CountBoom(i + 1, j - 1);
                        SurroundingBOOM += CountBoom(i + 1, j + 1);
                    }

                    //170321
                    if (m_nMap[i, j] != 1)
                    {
                        GameView[i, j].Value = SurroundingBOOM;
                        //170321
                        if (SurroundingBOOM > 0)
                            GameView[i, j].Style.BackColor = Color.PaleGreen;
                        else
                            GameView[i, j].Style.BackColor = Color.Gray;
                    }

                    if (SurroundingBOOM == 0)
                    {
                        if (nloop != 0)
                            CheckSurroundingBOOM(i, j);
                    }
                    else if (nloop == 0)
                        return SurroundingBOOM;
                }

                do
                {
                    switch (nloop++)
                    {
                        case 0:
                            i -= 1; j -= 1;
                            break;
                        case 1:
                        case 2:
                            i += 1;
                            break;
                        case 3:
                            j += 1;
                            break;
                        case 4:
                            i -= 2;
                            break;
                        case 5:
                            j += 1;
                            break;
                        case 6:
                        case 7:
                            i += 1;
                            break;
                        case 8:
                            return SurroundingBOOM;
                            break;
                    }
                }
                while (i < 0 || i > MaxI || j < 0 || j > MaxJ);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            GameView.Enabled = true;
            UpdateGameView();
            UpdateBOOMPosition();

            timer1.Enabled = true;
            m_tClockStart = DateTime.Now;

            Cursor.Current = Cursors.Default;
        }

        private void ShowAllBOOM()
        {
            int i = 0;
            int j = 0;
            
            for (int ii = 0; ii < BoomMapLists.Count; ii++)
            {
                string[] BoomText = BoomMapLists[ii].Split('_');
                i = Convert.ToInt32(BoomText[0]);
                j = Convert.ToInt32(BoomText[1]);

                //GameView.SelectAll();
                GameView.CurrentCell = GameView.Rows[j].Cells[i];
                GameView[i, j].Style.BackColor = Color.Red;
            }             
        }

        private void GameView_MouseMove(object sender, MouseEventArgs e)
        {
            Cursor.Current = Cursors.Hand;
        }

        private void OpenEmpty(int i, int j)
        {
            GameView[i, j].Style.BackColor = Color.PaleGreen;

            //CheckSurroundingBOOM2(i,j);
        }

        private int CountBoom(int i, int j)
        {
            if (m_nMap[i, j] == 1)
                return 1;
            else
            {
                /*
                if (CheckSurroundingBOOM(i, j) == 0)
                {
                    OpenEmpty(i, j);
                    CheckSurroundingBOOM(i, j);
                }*/

                //GameView[i, j].Style.BackColor = Color.PaleGreen;

                return 0;
            }
        }

        private void OpenRemainingGreen()
        {
            int GoodCount = 0;
            int SurroundingBoom = 0;

            for(int i = 0; i < GameView.Columns.Count; i++)
            {
                for (int j = 0; j < GameView.Rows.Count; j++)
                {
                    if (GameView[i, j].Style.BackColor == Color.PaleGreen
                        || GameView[i, j].Style.BackColor == Color.Gray)
                    {
                        //if (GameView[i, j].Value.ToString() == "0")
                        //    SurroundingBoom = CheckSurroundingBOOM(i, j);

                        GoodCount++;
                    }
                }
            }

            switch(GameSize)
            {
                case 0:
                    if (100 - GoodCount == Convert.ToInt32(textBox1.Text))
                        Status = true;
                    break;
                case 1:
                    if (400 - GoodCount == Convert.ToInt32(textBox1.Text))
                        Status = true;
                    break;
                case 2:
                    if (900 - GoodCount == Convert.ToInt32(textBox1.Text))
                        Status = true;
                    break;
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_tClockEnd = DateTime.Now;
            m_tsDiff = m_tClockEnd.Subtract(m_tClockStart);
            m_nLapsedSecond = Convert.ToInt32(m_tsDiff.TotalSeconds);

            textBox2.Text = m_nLapsedSecond.ToString();
        }

        private void UpdateHighScore()
        {
            HighScoreView.Rows.Clear();
            HighScoreView.ColumnCount = 2;

            string[] HighScoreLists = m_HighScorebyTypeSubKey.GetValueNames();

            for (int i = 0; i < HighScoreLists.Length; i++)
            {
                newRow = new DataGridViewRow();
                newRow.Height = 25;
                newRow.CreateCells(HighScoreView, new object[] { HighScoreLists[i], m_HighScorebyTypeSubKey.GetValue(HighScoreLists[i]) });
                HighScoreView.Rows.Add(newRow);
            }

            HighScoreView.Sort(HighScoreView.Columns[1], ListSortDirection.Descending);
        }

        private void CreateRegistryKey()
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);

            m_HighScorebyTypeSubKey = key.CreateSubKey("Mine Sweeper\\HighScore\\" + comboBox1.SelectedIndex.ToString() + "\\" + textBox1.ToString());
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            m_HighScorebyTypeSubKey.SetValue(NameTextBox.Text, textBox2.Text);
            UpdateHighScore();
            EnableSomeButton();
        }

        private void DisableSomeButton()
        {
            NameKeyinPanel.BringToFront();
            NameKeyinPanel.Visible = true;
            //170321
            NameTextBox.Focus();

            comboBox1.Enabled = false;
            textBox1.Enabled = false;
            button1.Enabled = false;
            textBox2.Enabled = false;
        }

        private void EnableSomeButton()
        {
            NameTextBox.Text = "";
            NameKeyinPanel.Visible = false;

            comboBox1.Enabled = true;
            textBox1.Enabled = true;
            button1.Enabled = true;
            textBox2.Enabled = true;
        }

        // Auto Open Feature
        private int CountFlag(int i, int j)
        {
            if (GameView[i, j].Value.ToString() == "#")
                return 1;
            else
            {
                return 0;
            }
        }

        private int CheckFlags(int i, int j)
        {
            int flagCount = 0;
            try
            {
                if (Convert.ToInt32(GameView[i, j].Value.ToString()) == 0)
                    return 0;
            }
            catch
            {
                return 1;
            }

            if (i == 0 && j == 0)
            {
                flagCount += CountFlag(i + 1, j);
                flagCount += CountFlag(i, j + 1);
                flagCount += CountFlag(i + 1, j + 1);
            }
            else if (i == 0 && j == MaxJ)
            {
                flagCount += CountFlag(i + 1, j);
                flagCount += CountFlag(i, j - 1);
                flagCount += CountFlag(i + 1, j - 1);
            }
            else if (i == MaxI && j == 0)
            {
                flagCount += CountFlag(i - 1, j);
                flagCount += CountFlag(i, j + 1);
                flagCount += CountFlag(i - 1, j + 1);
            }
            else if (i == 0)
            {
                flagCount += CountFlag(i + 1, j);
                flagCount += CountFlag(i, j + 1);
                flagCount += CountFlag(i + 1, j + 1);
                flagCount += CountFlag(i, j - 1);
                flagCount += CountFlag(i + 1, j - 1);
            }
            else if (j == 0)
            {
                flagCount += CountFlag(i + 1, j);
                flagCount += CountFlag(i, j + 1);
                flagCount += CountFlag(i + 1, j + 1);
                flagCount += CountFlag(i - 1, j);
                flagCount += CountFlag(i - 1, j + 1);
            }
            else if (i == MaxI && j == MaxJ)
            {
                flagCount += CountFlag(i - 1, j - 1);
                flagCount += CountFlag(i - 1, j);
                flagCount += CountFlag(i, j - 1);
            }
            else if (j == MaxJ)
            {
                flagCount += CountFlag(i - 1, j - 1);
                flagCount += CountFlag(i - 1, j);
                flagCount += CountFlag(i, j - 1);
                flagCount += CountFlag(i + 1, j - 1);
                flagCount += CountFlag(i + 1, j);
            }
            else if (i == MaxI)
            {
                flagCount += CountFlag(i - 1, j - 1);
                flagCount += CountFlag(i - 1, j);
                flagCount += CountFlag(i, j + 1);
                //170321
                //flagCount += CountFlag(i, j);
                flagCount += CountFlag(i - 1, j + 1);
                flagCount += CountFlag(i, j - 1);
            }
            else
            {
                flagCount += CountFlag(i - 1, j - 1);
                flagCount += CountFlag(i - 1, j);
                flagCount += CountFlag(i, j - 1);
                flagCount += CountFlag(i - 1, j + 1);
                flagCount += CountFlag(i, j + 1);
                flagCount += CountFlag(i + 1, j);
                flagCount += CountFlag(i + 1, j - 1);
                flagCount += CountFlag(i + 1, j + 1);
            }

            // Do nothing
            if (flagCount != Convert.ToInt32(GameView[i, j].Value.ToString()))
                return 0;

            for (int n = 0; n < 8; n++)
            {
                switch (n)
                {
                    case 0:
                        i -= 1; j -= 1;
                        break;
                    case 1:
                    case 2:
                    case 6:
                    case 7:
                        i += 1;
                        break;
                    case 3:
                    case 5:
                        j += 1;
                        break;
                    case 4:
                        i -= 2;
                        break;
                }

                if (!(i < 0 || i > MaxI || j < 0 || j > MaxJ) && (GameView[i, j].Value.ToString() == " " || GameView[i, j].Value.ToString() == "?"))
                {
                    // Game Over
                    if (m_nMap[i, j] == 1)
                        return -1;

                    CheckSurroundingBOOM(i, j);
                }
            }

            // Check Win
            return 2;
        }

        private void HighScoreView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1)
            {
                if (double.Parse(e.CellValue1.ToString()) > double.Parse(e.CellValue2.ToString()))
                {
                    e.SortResult = 1;
                }
                else if (double.Parse(e.CellValue1.ToString()) < double.Parse(e.CellValue2.ToString()))
                {
                    e.SortResult = -1;
                }
                else
                {
                    e.SortResult = 0;
                }
                e.Handled = true;
            }
        }
    }
}