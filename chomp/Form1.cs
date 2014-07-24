using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace chomp
{
    public partial class Form1 : Form
    {
        Button[,] btnArray = new Button[5, 11];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ClientSize = new Size(670, 310);
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 11; y++) {
                    btnArray[x,y] = new Button();
                    btnArray[x, y].Width = 50;
                    btnArray[x, y].Height = 50;
                    btnArray[x, y].FlatStyle = FlatStyle.Flat;
                    btnArray[x, y].BackColor = System.Drawing.Color.Red;
                    btnArray[x, y].FlatAppearance.BorderSize = 0;
                    btnArray[x, y].Location = new Point(10 + (60 * y), 10 + (60 * x));
                    btnArray[x, y].Click += new  EventHandler(btnClick);
                    Controls.Add(btnArray[x,y]);
                }
            }
            
        }

        private Point btnCoor(Button target)
        {
            Point p = new Point(((target.Location.Y - 10) / 60), ((target.Location.X - 10) / 60));
            return p;
        }

        private void btnClick(object sender, EventArgs e)
        {
            Random k_thing = new Random();
            var current = sender as Button;
            move(btnCoor(current));
            if (check() || calcCount(btnArray) == 0)
            { 
                MessageBox.Show("#u lose:(");
                DialogResult ans = MessageBox.Show("new-game?","chomp-game",MessageBoxButtons.YesNo);
                if (ans.ToString() == "Yes")
                {
                    newGame();
                }
                else
                {
                    Application.Exit();
                }
                return; 
            }
            this.Text = "thinking...";
            System.Threading.Thread.Sleep(1000*k_thing.Next(1,3));
            this.Text = "chomp-game";
            if (botMove() == false || calcCount(btnArray)==0) { 
                MessageBox.Show("#u win:)");
                DialogResult ans = MessageBox.Show("new-game?", "chomp-game", MessageBoxButtons.YesNo);
                if (ans.ToString() == "Yes")
                {
                    newGame();
                }
                else
                {
                    Application.Exit();
                }
                return; 
            }
        }

        private bool check()
        {
            for (int x = 0; x < 5; x++) {
                for (int y = 0; y < 11; y++) {
                    if (btnArray[x, y].Visible == true && btnArray[x, y].Enabled == true) { return false; }
                }
            }
            return true;
        }

        private void move(Point p)
        {
            for (int x = p.X; x < 5; x++)
            {
                for (int y = p.Y; y < 11; y++)
                {
                    btnArray[x, y].Visible = false;
                    btnArray[x, y].Enabled = false;
                }
            }
        }

        private bool virtualMove(Point t,Button[,] tArray)
        {
            bool[,] virArray = new bool[5, 11];
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    if (tArray[i, j].Visible == true && tArray[i, j].Enabled == true)
                    {
                        virArray[i, j] = true;
                    }
                    else
                    {
                        virArray[i, j] = false;
                    }
                }
            }
            for (int i = t.X; i < 5; i++)
            {
                for (int j = t.Y; j < 11; j++)
                {
                    virArray[i, j] = false;
                }
            }
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    if (virArray[x, y]==true)
                    {
                        Point p = new Point(x, y);
                        //
                        int cCount = 0;
                        for (int i = 0; i < 5; i++)
                        {
                            for (int j = 0; j < 11; j++)
                            {
                                if (virArray[i, j] == true)
                                {
                                    cCount++;
                                }
                            }
                        }
                        //
                        int cErase = 0;
                        for (int i = p.X; i < 5; i++)
                        {
                            for (int j = p.Y; j < 11; j++)
                            {
                                if (virArray[i,j] == true)
                                {
                                    cErase++;
                                }
                            }
                        }
                        //
                        if ((cCount - cErase) == 1)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;

        }

        private int calcCount(Button[,] tArray)
        {
            int count=0;
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y <11; y++)
                {
                    if (tArray[x, y].Visible == true && tArray[x, y].Enabled == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private int calcErase(Point p, Button[,] tArray)
        {
            int count = 0;
            for (int x = p.X; x < 5; x++)
            {
                for (int y = p.Y; y < 11; y++)
                {
                    if (tArray[x, y].Visible == true && tArray[x, y].Enabled == true)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private bool botMove()
        {
            bool find = false;
            int lastErase=0;
            Point lastP = new Point();
            for (int x = 0; x < 5 ; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    if (btnArray[x, y].Visible == true && btnArray[x, y].Enabled == true)
                    {
                        Point p = new Point(x, y);
                        if ((calcCount(btnArray) - calcErase(p, btnArray)) % 2 == 1 && calcErase(p, btnArray) > lastErase && virtualMove(p, btnArray)==true)
                        {
                            lastP.X = x;
                            lastP.Y = y;
                            lastErase = calcErase(p, btnArray);
                            find = true;
                        }
                    }
                }
            }
            if (find == true)
            {
                move(lastP);
                return true;
            }
            return false;
        }

        private void newGame()
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    btnArray[x, y].Visible = true;
                    btnArray[x, y].Enabled = true;
                }
            }
        }
    }
}
