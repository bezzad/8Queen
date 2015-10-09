using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace _8Queen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Button[,] Queen;
        public static int[,] TFQueen;
        private void Form1_Load(object sender, EventArgs e)
        {
            TFQueen = new int[8, 8];
            // Set All Home to Number '0'
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    TFQueen[i, j] = 0;
            // Design Queen Button            
            Queen = new Button[8, 8];
            bool BlackHome = false; 
            for (int i = 0; i < 8; i++) 
                for (int j = 0; j < 8; j++)
                {
                    this.Queen[i, j] = new Button();
                    if (BlackHome == false)
                    {
                        this.Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.White;
                        if (j != 7)
                            BlackHome = true;   // because after 8 White come 1 White
                    }
                    else
                    {
                        this.Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.Black;
                        if (j != 7)
                            BlackHome = false; // because after 8 black come 1 black
                    }
                    this.Queen[i, j].BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
                    this.Queen[i, j].Location = new System.Drawing.Point(130 + (i * 65), 98 + (j * 65));
                    this.Queen[i, j].Size = new System.Drawing.Size(68, 68);
                    this.Queen[i, j].TabIndex = i + j;
                    this.Queen[i, j].UseVisualStyleBackColor = true;
                    this.Queen[i, j].Click += new System.EventHandler(this.button_Click);
                    this.Queen[i, j].Cursor = System.Windows.Forms.Cursors.Hand;
                    this.Controls.Add(this.Queen[i, j]);
                }
        }

        public static int LocX;
        public static int LocY;
        public void FindLoc()
        {
            LocX = 0;
            LocY = 0;
            int x = (this.ActiveControl.Location.X - 130);
            int y = (this.ActiveControl.Location.Y - 98);
            while (x != 0)
            {
                x -= 65;
                LocX++;
            }
            while (y != 0)
            {
                y -= 65;
                LocY++;
            }
        }
        public bool TFClick = false;
        public void button_Click(object sender, EventArgs e)
        {
            // Save Location
            FindLoc();
            // Find Button location and name for change BackgraundImage
            if (TFQueen[LocX, LocY] == 0 && TFClick == false)
            {
                TFQueen[LocX, LocY] = 1;
                if (LocX % 2 == 0 && LocY % 2 == 0) // if Both of X & Y Even or ODD Then there is White else black
                    this.ActiveControl.BackgroundImage = global::_8Queen.Properties.Resources.QUEEN;
                else if (LocX % 2 != 0 && LocY % 2 != 0)
                    this.ActiveControl.BackgroundImage = global::_8Queen.Properties.Resources.QUEEN;
                else
                    this.ActiveControl.BackgroundImage = global::_8Queen.Properties.Resources.QUEEN2;
                TFClick = true;
                btnGo.Enabled = true;
                btnClear.Enabled = true;
            }
            else if (TFQueen[LocX, LocY] == 1)
            {
                TFQueen[LocX, LocY] = 0;
                if (LocX % 2 == 0 && LocY % 2 == 0) // if Both of X & Y Even or ODD Then there is White else black
                    this.ActiveControl.BackgroundImage = global::_8Queen.Properties.Resources.White;
                else if (LocX % 2 != 0 && LocY % 2 != 0)
                    this.ActiveControl.BackgroundImage = global::_8Queen.Properties.Resources.White;
                else
                    this.ActiveControl.BackgroundImage = global::_8Queen.Properties.Resources.Black;
                TFClick = false;
                btnGo.Enabled = false;
                btnClear.Enabled = false;
                LocX = 8;
                LocY = 8;
            }
            else return;

        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            btnGo.Enabled = false;
            bool Find = false;
            // Find Queen Home
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                    if (TFQueen[i, j] == 1)
                    {
                        LocX = i;
                        LocY = j;
                        Find = true;
                        break;
                    }
                if (Find == true) break;
            }
            prgbrGo.Value++;
            Thread.Sleep(1);
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number '-1'
            for (int i = 0; i < 8; i++)
            {
                if (i != LocY) // Verticality |
                    TFQueen[LocX, i] = -1;
                if (i != LocX) // Horizontal --
                    TFQueen[i, LocY] = -1;
                prgbrGo.Value += 2;
                Thread.Sleep(1);
            }
            for (int i = LocX + 1, j = LocY - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                TFQueen[i, j] = -1;
            for (int i = LocX - 1, j = LocY - 1; i >= 0 && j >= 0; i--, j--)    // H + V = × (North-Wast)
                TFQueen[i, j] = -1;
            for (int i = LocX - 1, j = LocY + 1; i >= 0 && j < 8; i--, j++)    // H + V = × (South-Wast)
                TFQueen[i, j] = -1;
            for (int i = LocX + 1, j = LocY + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                TFQueen[i, j] = -1;
            prgbrGo.Value += 54;
            Thread.Sleep(1);
            CheckFill();
            // Print All Queen
            PrintQueen();
        }

        public int[] FindLevel;
        public void CheckFill()
        {
            FindLevel = new int[7];
            //*********************************************************************************************************
            // Phase One-1 --------------------------------------------------------------------------------------------
            for (int i = 0, l = 0; i < 8; i++) // Find Level
                if (i == LocX)
                    continue;
                else
                {
                    FindLevel[l] = i;
                    l++;
                }
            prgbrGo.Value += 3;
            Thread.Sleep(1);
            // END Phase One-1 ----------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Two-2 --------------------------------------------------------------------------------------------
            Level2:
            int Lev2X = FindLevel[0];
            int Lev2Y = 0;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev2X, i] == 0)
                {
                    TFQueen[Lev2X, i] = 2;
                    Lev2Y = i;
                    break;
                }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number 
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev2Y) // Verticality |
                    if (TFQueen[Lev2X, i] == 0) TFQueen[Lev2X, i] = -2;
                if (i != Lev2X) // Horizontal --
                    if (TFQueen[i, Lev2Y] == 0) TFQueen[i, Lev2Y] = -2;
            }
            for (int i = Lev2X + 1, j = Lev2Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -2;
            for (int i = Lev2X - 1, j = Lev2Y - 1; i >= 0 && j >= 0; i--, j--)    // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -2;
            for (int i = Lev2X - 1, j = Lev2Y + 1; i >= 0 && j < 8; i--, j++)    // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -2;
            for (int i = Lev2X + 1, j = Lev2Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -2;
            // END Phase Two-2 ----------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Three-3 ------------------------------------------------------------------------------------------
            Level3:
            int Lev3X = FindLevel[1];
            int Lev3Y = 0;
            bool findQueen = false;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev3X, i] == 0)
                {
                    TFQueen[Lev3X, i] = 3;
                    Lev3Y = i;
                    findQueen = true;
                    break;
                }
            if (findQueen == true)
                findQueen = false;
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (TFQueen[i, j] == 3 || TFQueen[i, j] == -3)
                            TFQueen[i, j] = 0;
                        else if (TFQueen[i, j] == 2) TFQueen[i, j] = -1;
                        // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
                        else if (TFQueen[i, j] == -2) TFQueen[i, j] = 0;
                goto Level2;
            }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev3Y) // Verticality |
                    if (TFQueen[Lev3X, i] == 0) TFQueen[Lev3X, i] = -3;
                if (i != Lev3X) // Horizontal --
                    if (TFQueen[i, Lev3Y] == 0) TFQueen[i, Lev3Y] = -3;
            }
            for (int i = Lev3X + 1, j = Lev3Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -3;
            for (int i = Lev3X - 1, j = Lev3Y - 1; i >= 0 && j >= 0; i--, j--)    // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -3;
            for (int i = Lev3X - 1, j = Lev3Y + 1; i >= 0 && j < 8; i--, j++)    // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -3;
            for (int i = Lev3X + 1, j = Lev3Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -3;
            // END Phase Three-3 --------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Four-4 -------------------------------------------------------------------------------------------
            Level4:
            int Lev4X = FindLevel[2];
            int Lev4Y = 0;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev4X, i] == 0)
                {
                    TFQueen[Lev4X, i] = 4;
                    Lev4Y = i;
                    findQueen = true;
                    break;
                }
            if (findQueen == true)
                findQueen = false;
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (TFQueen[i, j] == 4 || TFQueen[i, j] == -4)
                            TFQueen[i, j] = 0;
                        else if (TFQueen[i, j] == 3) TFQueen[i, j] = -2;
                        // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
                        else if (TFQueen[i, j] == -3) TFQueen[i, j] = 0;
                goto Level3;
            }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number 
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev4Y) // Verticality |
                    if (TFQueen[Lev4X, i] == 0) TFQueen[Lev4X, i] = -4;
                if (i != Lev4X) // Horizontal --
                    if (TFQueen[i, Lev4Y] == 0) TFQueen[i, Lev4Y] = -4;
            }
            for (int i = Lev4X + 1, j = Lev4Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -4;
            for (int i = Lev4X - 1, j = Lev4Y - 1; i >= 0 && j >= 0; i--, j--)  // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -4;
            for (int i = Lev4X - 1, j = Lev4Y + 1; i >= 0 && j < 8; i--, j++)   // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -4;
            for (int i = Lev4X + 1, j = Lev4Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -4;
            // END Phase Four-4 ---------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Five-5 -------------------------------------------------------------------------------------------
            Level5:
            int Lev5X = FindLevel[3];
            int Lev5Y = 0;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev5X, i] == 0)
                {
                    TFQueen[Lev5X, i] = 5;
                    Lev5Y = i;
                    findQueen = true;
                    break;
                }
            if (findQueen == true)
                findQueen = false;
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (TFQueen[i, j] == 5 || TFQueen[i, j] == -5)
                            TFQueen[i, j] = 0;
                        else if (TFQueen[i, j] == 4) TFQueen[i, j] = -3;
                        // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
                        else if (TFQueen[i, j] == -4) TFQueen[i, j] = 0;
                goto Level4;
            }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev5Y) // Verticality |
                    if (TFQueen[Lev5X, i] == 0) TFQueen[Lev5X, i] = -5;
                if (i != Lev5X) // Horizontal --
                    if (TFQueen[i, Lev5Y] == 0) TFQueen[i, Lev5Y] = -5;
            }
            for (int i = Lev5X + 1, j = Lev5Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -5;
            for (int i = Lev5X - 1, j = Lev5Y - 1; i >= 0 && j >= 0; i--, j--)  // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -5;
            for (int i = Lev5X - 1, j = Lev5Y + 1; i >= 0 && j < 8; i--, j++)   // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -5;
            for (int i = Lev5X + 1, j = Lev5Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -5;
            // END Phase Five-5 ---------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Six-6 --------------------------------------------------------------------------------------------
            Level6:
            int Lev6X = FindLevel[4];
            int Lev6Y = 0;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev6X, i] == 0)
                {
                    TFQueen[Lev6X, i] = 6;
                    Lev6Y = i;
                    findQueen = true;
                    break;
                }
            if (findQueen == true)
                findQueen = false;
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (TFQueen[i, j] == 6 || TFQueen[i, j] == -6)
                            TFQueen[i, j] = 0;
                        else if (TFQueen[i, j] == 5) TFQueen[i, j] = -4;
                        // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
                        else if (TFQueen[i, j] == -5) TFQueen[i, j] = 0;
                goto Level5;
            }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number 
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev6Y) // Verticality |
                    if (TFQueen[Lev6X, i] == 0) TFQueen[Lev6X, i] = -6;
                if (i != Lev6X) // Horizontal --
                    if (TFQueen[i, Lev6Y] == 0) TFQueen[i, Lev6Y] = -6;
            }
            for (int i = Lev6X + 1, j = Lev6Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -6;
            for (int i = Lev6X - 1, j = Lev6Y - 1; i >= 0 && j >= 0; i--, j--)  // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -6;
            for (int i = Lev6X - 1, j = Lev6Y + 1; i >= 0 && j < 8; i--, j++)   // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -6;
            for (int i = Lev6X + 1, j = Lev6Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -6;
            // END Phase Six-6 ----------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Seven-7 ------------------------------------------------------------------------------------------
            Level7:
            int Lev7X = FindLevel[5];
            int Lev7Y = 0;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev7X, i] == 0)
                {
                    TFQueen[Lev7X, i] = 7;
                    Lev7Y = i;
                    findQueen = true;
                    break;
                }
            if (findQueen == true)
                findQueen = false;
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (TFQueen[i, j] == 7 || TFQueen[i, j] == -7)
                            TFQueen[i, j] = 0;
                        else if (TFQueen[i, j] == 6) TFQueen[i, j] = -5;
                        // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
                        else if (TFQueen[i, j] == -6) TFQueen[i, j] = 0;
                goto Level6;
            }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number 
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev7Y) // Verticality |
                    if (TFQueen[Lev7X, i] == 0) TFQueen[Lev7X, i] = -7;
                if (i != Lev7X) // Horizontal --
                    if (TFQueen[i, Lev7Y] == 0) TFQueen[i, Lev7Y] = -7;
            }
            for (int i = Lev7X + 1, j = Lev7Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -7;
            for (int i = Lev7X - 1, j = Lev7Y - 1; i >= 0 && j >= 0; i--, j--)  // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -7;
            for (int i = Lev7X - 1, j = Lev7Y + 1; i >= 0 && j < 8; i--, j++)   // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -7;
            for (int i = Lev7X + 1, j = Lev7Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -7;
            // END Phase Seven-7 --------------------------------------------------------------------------------------
            //*********************************************************************************************************
            // Phase Eight-8 ------------------------------------------------------------------------------------------
                
            int Lev8X = FindLevel[6];
            int Lev8Y = 0;
            for (int i = 0; i < 8; i++)
                if (TFQueen[Lev8X, i] == 0)
                {
                    TFQueen[Lev8X, i] = 8;
                    Lev8Y = i;
                    findQueen = true;
                    break;
                }
            if (findQueen == true)
                findQueen = false;
            else
            {
                for (int i = 0; i < 8; i++)
                    for (int j = 0; j < 8; j++)
                        if (TFQueen[i, j] == 8 || TFQueen[i, j] == -8)
                            TFQueen[i, j] = 0;
                        else if (TFQueen[i, j] == 7) TFQueen[i, j] = -6;
                        // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number
                        else if (TFQueen[i, j] == -7) TFQueen[i, j] = 0;
                goto Level7;
            }
            // Delete All VERTICALITY & HORIZONTAL Home of Cours the Queen Home by number 
            for (int i = 0; i < 8; i++)
            {
                if (i != Lev8Y) // Verticality |
                    if (TFQueen[Lev8X, i] == 0) TFQueen[Lev8X, i] = -8;
                if (i != Lev8X) // Horizontal --
                    if (TFQueen[i, Lev7Y] == 0) TFQueen[i, Lev7Y] = -8;
            }
            for (int i = Lev8X + 1, j = Lev8Y - 1; i < 8 && j >= 0; i++, j--)   // H + V = × (North-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -8;
            for (int i = Lev8X - 1, j = Lev8Y - 1; i >= 0 && j >= 0; i--, j--)  // H + V = × (North-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -8;
            for (int i = Lev8X - 1, j = Lev8Y + 1; i >= 0 && j < 8; i--, j++)   // H + V = × (South-Wast)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -8;
            for (int i = Lev8X + 1, j = Lev8Y + 1; i < 8 && j < 8; i++, j++)    // H + V = × (South-East)
                if (TFQueen[i, j] == 0) TFQueen[i, j] = -8;
            // END Phase Eight-8 --------------------------------------------------------------------------------------
            //*********************************************************************************************************
        }

        public void PrintQueen()
        {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (TFQueen[i, j] == 1 || TFQueen[i, j] == 2 || TFQueen[i, j] == 3 || TFQueen[i, j] == 4 || TFQueen[i, j] == 5 ||
                        TFQueen[i, j] == 6 || TFQueen[i, j] == 7 || TFQueen[i, j] == 8)
                    {
                        prgbrGo.Value++;
                        Thread.Sleep(1); 
                        if (i % 2 == 0 && j % 2 == 0) // if Both of X & Y Even or ODD Then there is White else black
                            Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.QUEEN;
                        else if (i % 2 != 0 && j % 2 != 0)
                            Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.QUEEN;
                        else
                            Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.QUEEN2;
                    }
            prgbrGo.Value += 18;
            Thread.Sleep(1);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            btnGo.Enabled = false;
            btnClear.Enabled = false;
            prgbrGo.Value = 0;
            TFClick = false;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (TFQueen[i, j] == 1 || TFQueen[i, j] == 2 || TFQueen[i, j] == 3 || TFQueen[i, j] == 4 || TFQueen[i, j] == 5 ||
                        TFQueen[i, j] == 6 || TFQueen[i, j] == 7 || TFQueen[i, j] == 8)
                    {
                        if (i % 2 == 0 && j % 2 == 0) // if Both of X & Y Even or ODD Then there is White else black
                            Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.White;
                        else if (i % 2 != 0 && j % 2 != 0)
                            Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.White;
                        else
                            Queen[i, j].BackgroundImage = global::_8Queen.Properties.Resources.Black;
                        TFQueen[i, j] = 0;
                    }
                    else TFQueen[i, j] = 0;
            LocX = LocY = 8;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormAbout About = new FormAbout();
            About.ShowDialog();
        }
    }
}
