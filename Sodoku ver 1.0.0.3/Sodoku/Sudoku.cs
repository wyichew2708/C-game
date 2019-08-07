using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sodoku
{
    public partial class Sudoku : Form
    {
        private DataGridViewRow newRow;

        private int[,] m_nMap;
        private int[,] m_nUserMap;
        private int[,] m_nCheckMap;

        private List<string> ColLists = new List<string>();
        private List<string> RowLists = new List<string>();
        private int NumberSelected = 0;

        private int cellclickRow = 0;
        private int cellclickCol = 0;
        private DataGridView cellclickbox;

        private int UserMapCol = 0;
        private int UserMapRow = 0;

        public Sudoku()
        {
            InitializeComponent();

            DifficultyComboBox.SelectedIndex = 1;

            CreateNewGame();

            UpdateGameView();
        }

        private void UpdateGameView()
        {
            string gameviewname = "GameView";

            int row = 0;
            int col = 0;

            int box = 1;

            //Generate Game Interface
            Random number = new Random();
            int Difficulty = 2;
            Difficulty = (DifficultyComboBox.SelectedIndex + 1) * 2 + 1;
            if(Difficulty>8)
                Difficulty = 8;
            //---

            while (box < 10)
            {
                int[] RanX = new int[Difficulty + 1];
                int[] RanY = new int[Difficulty + 1];

                DataGridView FullGameViewName = (DataGridView)this.Controls.Find(gameviewname + box.ToString(), true)[0];

                FullGameViewName.Rows.Clear();
                FullGameViewName.Columns.Clear();
                FullGameViewName.ColumnCount = 3;

                //Assign Random Number
                for (int count = 0; count <= Difficulty; count++)
                {
                    RanX[count] = number.Next(0, 3);
                    RanY[count] = number.Next(0, 3);

                    for (int count2 = 0; count2 < count; count2++)
                    {
                        if (RanX[count] == RanX[count2] && RanY[count] == RanY[count2])
                            count--;
                    }
                }
                //---

                for (int i = 0; i < 3; i++)
                {
                    newRow = new DataGridViewRow();
                    newRow.Height = 40;
                    newRow.CreateCells(FullGameViewName, new object[] { " ", " ", " " });
                    FullGameViewName.Rows.Add(newRow);
                }

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        //Remove Random Number
                        for (int z = 0; z < Difficulty; z++)
                        {
                            if (x == RanX[z] && y == RanY[z])
                            {
                                m_nUserMap[col + x, row + y] = 0;
                                break;
                            }
                        }
                        //---

                        if (m_nUserMap[col + x, row + y] != 0)
                        {
                            FullGameViewName[x, y].Value = m_nMap[col + x, row + y].ToString();
                            FullGameViewName[x, y].Style.Font = new Font("Arial", 15, FontStyle.Bold);
                            FullGameViewName[x, y].Style.ForeColor = Color.Red;
                        }
                    }
                }

                box++;

                col += 3;
                if (col > 6)
                {
                    col = 0;
                    row += 3;
                }
            }
        }

        private void CreateNewGame()
        {
            SudokuGenerateAlgo3();
        }

        private void SudokuGenerateAlgo3()
        {
            n_Retry:

            m_nMap = new int[9, 9];
            m_nUserMap = new int[9, 9];

            int FillNumber = 1;
            int count = 0;

            bool DoneAssign = false;

            Random number = new Random();
            List<int> numberlists1 = new List<int>();
            List<int> numberlists2 = new List<int>();
            string[] m_Matrix = new string[9];

            int i = 0;
            int j = 0;

            int z = 0;
            int y = 0;

            numberlists1 = new List<int>();
            for (int k = z; k < z + 3; k++)
            {
                numberlists1.Add(k);
            }

            numberlists2 = new List<int>();
            for (int k = y; k < y + 3; k++)
            {
                numberlists2.Add(k);
            }

            while (FillNumber < 10)
            {
                //i = number.Next(z, z +3);
                //j = number.Next(y,y+3);

                //
                int test = 0;
                test = number.Next(0, 3);
                i = numberlists1[test];

                int test2 = 0;
                test2 = number.Next(0, 3);
                j = numberlists2[test2];
                //

                if (m_nMap[i, j] > 0)
                    continue;

                if (IsValidPosition(i, j, FillNumber,1))
                {
                    m_nMap[i, j] = FillNumber;
                    m_nUserMap[i, j] = FillNumber;
                    //GameView[j, i].Value = FillNumber.ToString();
                    DoneAssign = true;
                }

                if (DoneAssign)
                {
                    DoneAssign = false;
                    count = 0;
                }
                else
                {                        
                    count++;

                    if (count > 300)
                        goto n_Retry;

                    continue;
                }


                z += 3;
                if (z == 9)
                {
                    z = 0;
                    y += 3;
                }
                
                if (y == 9)
                {
                    z = 0;
                    y = 0;
                    FillNumber++;
                }

                numberlists1 = new List<int>();
                for (int k = z; k < z+3; k++)
                {
                    numberlists1.Add(k);
                }

                numberlists2 = new List<int>();
                for (int k = y; k < y + 3; k++)
                {
                    numberlists2.Add(k);
                }
            }
        }

        private bool IsValidPosition(int ii, int jj,int number, int option)
        {
            ColLists.Clear();
            RowLists.Clear();

            //Checking
            for (int i = 0; i < 9; i++)
            {
                if (option == 1)
                {
                    if (ColLists.Contains(number.ToString()))
                    {
                        return false;
                    }
                    else
                    {
                        ColLists.Add(m_nMap[ii, i].ToString());
                    }

                    if (RowLists.Contains(number.ToString()))
                    {
                        return false;
                    }
                    else
                    {
                        RowLists.Add(m_nMap[i, jj].ToString());
                    }
                }
                else
                {
                    if (ColLists.Contains(m_nCheckMap[ii, i].ToString()) || ColLists.Contains("0"))
                    {
                        return false;
                    }
                    else
                    {
                        ColLists.Add(m_nCheckMap[ii, i].ToString());
                    }

                    if (RowLists.Contains(m_nCheckMap[i, jj].ToString()) || RowLists.Contains("0"))
                    {
                        return false;
                    }
                    else
                    {
                        RowLists.Add(m_nCheckMap[i, jj].ToString());
                    }
                }
            }

            return true;
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            CreateNewGame();
            UpdateGameView();
            Cursor.Current = Cursors.Default;
        }

        private void GameView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridView FullGameViewName = (DataGridView)sender;
                if (FullGameViewName[e.ColumnIndex, e.RowIndex].Style.ForeColor != Color.Red)
                {
                    SudokuPanel.Enabled = false;

                    cellclickRow = e.RowIndex;
                    cellclickCol = e.ColumnIndex;
                    cellclickbox = FullGameViewName;

                    NumberPanel.Location = FullGameViewName.Location;
                    NumberPanel.BringToFront();

                    //170321
                    EnableNumberPad();
                    //ShowValidNumber(FullGameViewName);
                    NumberPanel.Visible = true;
                }

                //Highlight all same color
                HighLightSameColor(FullGameViewName[e.ColumnIndex, e.RowIndex].Value.ToString());
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button ButtonNumber = (Button)sender;
            NumberSelected = Convert.ToInt32(ButtonNumber.Text);

            if (NumberSelected > 0)
            {
                int row = 0;
                int col = 0;
                //Extract Number
                int cellparent = Convert.ToInt32(Regex.Match(cellclickbox.Name.ToString(), @"\d+").Value);
                for (int i = 1; i < cellparent; i++)
                {
                    col+=3;
                    if (col == 9)
                    {
                        col = 0;
                        row+=3;
                    }
                }

                UserMapCol = col + cellclickCol;
                UserMapRow = row + cellclickRow;

                m_nUserMap[col + cellclickCol, row + cellclickRow] = NumberSelected;
                cellclickbox[cellclickCol, cellclickRow].Value = NumberSelected.ToString();
            }

            CheckWinStatus();

            NumberPanel.Enabled = false;
            NumberPanel.Visible = false;

            SudokuPanel.Enabled = true;
        }

        private void ShowValidNumber(DataGridView FullGameViewName)
        {
            EnableNumberPad();

            int cellparent = Convert.ToInt32(Regex.Match(FullGameViewName.Name.ToString(), @"\d+").Value);
            int cellRow = Convert.ToInt32(FullGameViewName.CurrentCellAddress.Y);
            int cellCol = Convert.ToInt32(FullGameViewName.CurrentCellAddress.X);

            int row = 0;
            int col = 0;

            for (int i = 1; i<cellparent;i++)
            {
                col+=3;
                if (col == 9)
                {
                    col = 0;
                    row+=3;
                }
            }

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    try
                    {
                        DisableNumberPad(Convert.ToInt32(cellclickbox[x, y].Value));
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            for (int x = 0; x < 9; x++)
            {
                if (m_nUserMap[x, row + cellRow] > 0)
                    DisableNumberPad(m_nUserMap[x, row + cellRow]);
            }

            for (int y = 0; y < 9; y++)
            {
                 if (m_nUserMap[col + cellCol,y] > 0)
                     DisableNumberPad(m_nUserMap[col + cellCol, y]);
            }
        }

        private void EnableNumberPad()
        {
            NumberPanel.Enabled = true;
            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;
            button5.Enabled = true;
            button6.Enabled = true;
            button7.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
        }

        private void DisableNumberPad(int Number)
        {
            Button FullGameViewName = (Button)this.Controls.Find("button" + Number.ToString(), true)[0];
            FullGameViewName.Enabled = false;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NumberPanel.Visible = false;
            SudokuPanel.Enabled = true;
        }

        private void CleanButton_Click(object sender, EventArgs e)
        {
            m_nUserMap[UserMapCol, UserMapRow] = 0;
            cellclickbox[cellclickCol, cellclickRow].Value = "";
        }

        private bool CheckWinStatus()
        {
            /*
            bool Win = true;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (m_nMap[i, j] != m_nUserMap[i, j])
                    {
                        Win = false;
                        return;
                    }
                }
            }

            if (Win)
                MessageBox.Show("Congratz! You Win!\n");
             */

            m_nCheckMap = new int[9, 9];

            ColLists.Clear();
            RowLists.Clear();

            int box = 1;
            int col = 0;
            int row = 0;

            bool Win = true;

            while (box < 10)
            {
                ColLists.Clear();
                DataGridView FullGameViewName = (DataGridView)this.Controls.Find("GameView" + box.ToString(), true)[0];

                for (int x = 0; x < 3; x++)
                {
                    for (int y = 0; y < 3; y++)
                    {
                        try
                        {
                            if (ColLists.Contains(FullGameViewName[x, y].Value.ToString()))
                            {
                                Win = false;
                                return false;
                            }
                                
                            ColLists.Add(FullGameViewName[x, y].Value.ToString());

                            m_nCheckMap[x + col, y + row] = Convert.ToInt32(FullGameViewName[x, y].Value);
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }

                box++;

                col += 3;
                if (col > 6)
                {
                    col = 0;
                    row += 3;
                }
            }

            for(int i = 0 ;i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (!IsValidPosition(i, j, m_nCheckMap[i, j],2))
                        return false;
                }
            }

            if (Win)
                return true;
            else
                return false;
        }

        private void CheckAnswerButton_Click(object sender, EventArgs e)
        {
            if (CheckWinStatus())
                MessageBox.Show("Congratz! You Win!\n");
            else
                MessageBox.Show("Please try again.\n");
        }

        private void HighLightSameColor(string numberstring)
        {
            int number;

            try
            {
                number = Convert.ToInt32(numberstring);

                int box = 1;
                int row = 0;
                int col = 0;

                while (box < 10)
                {
                    DataGridView FullGameViewName = (DataGridView)this.Controls.Find("GameView" + box.ToString(), true)[0];
                    FullGameViewName.ClearSelection();

                    for (int x = 0; x < 3; x++)
                    {
                        for (int y = 0; y < 3; y++)
                        {
                            try
                            {
                                if (Convert.ToInt32(FullGameViewName[x, y].Value) == number)
                                {
                                    FullGameViewName[x, y].Style.BackColor = Color.PaleGreen;
                                }
                                else
                                    FullGameViewName[x, y].Style.BackColor = Color.White;
                            }
                            catch (Exception e)
                            {
                            }
                        }
                    }

                    box++;

                    col += 3;
                    if (col > 6)
                    {
                        col = 0;
                        row += 3;
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}