using System;
using System.Windows.Forms;
using System.Drawing;

namespace Breitensuche
{
    class Game : Form
    {
        private int xStart = 0;
        private int yStart = 0;
        private int xCoordinate = 0;
        private int yCoordinate = 0;
        string[,] LabirinthArray2D;

        public Game()
        {
            Width = 600;
            Height = 600;
            Text = "Breitensuche - manuell";
            FormBorderStyle = FormBorderStyle.FixedDialog;      // removes possibility to resize
            StartPosition = FormStartPosition.CenterScreen;     // starts window in center of the screen
            MaximizeBox = false;                                // removes the maximize Button

            if (!Console.IsInputRedirected)
            {
                Console.WriteLine("This example requires that input be redirected from a file.");
                Console.WriteLine("Please write:  4.Breitensuche < maze.dat");
                return;
            }

            Console.WriteLine("About to call Console.ReadLine in a loop.");
            Console.WriteLine("---");
            string s;
            string sLab = "";
            int i = 0;

            // extracting string from maze.dat
            do
            {
                i++;
                s = Console.ReadLine();
                Console.WriteLine("Line {0}: {1}", i, s);           // prints all extracted text from maze.dat
                sLab += s;
            } while (s != null);
            Console.WriteLine("---");

            // extracting column number
            string sColumn = sLab;
            sColumn = sColumn.Remove(2);                            // seperates first 2 characters from string
            int iColumn = Int32.Parse(sColumn);
            Console.WriteLine("iColumn " + iColumn);                // prints column number

            // extracting row number
            string sRow = sLab;
            sRow = sRow.Remove(4);                                  // seperates first 4 characters from string
            sRow = sRow.Remove(0, 2);                               // removes first 2 already used numbers from string
            int iRow = Int32.Parse(sRow);
            Console.WriteLine("iRow " + iRow);                      // prints row number

            string sLabyrinth = sLab;
            sLabyrinth = sLabyrinth.Remove(0, 4);

            // creating all rows seperatly
            // im sure there is an easier, cleaner way, but I didn't find one YET
            string row1 = sLabyrinth.Remove(19);
            string row2 = sLabyrinth.Remove(38);
            row2 = row2.Remove(0, 19);
            string row3 = sLabyrinth.Remove(57);
            row3 = row3.Remove(0, 38);
            string row4 = sLabyrinth.Remove(76);
            row4 = row4.Remove(0, 57);
            string row5 = sLabyrinth.Remove(95);
            row5 = row5.Remove(0, 76);
            string row6 = sLabyrinth.Remove(114);
            row6 = row6.Remove(0, 95);
            string row7 = sLabyrinth.Remove(133);
            row7 = row7.Remove(0, 114);
            string row8 = sLabyrinth.Remove(152);
            row8 = row8.Remove(0, 133);
            string row9 = sLabyrinth.Remove(171);
            row9 = row9.Remove(0, 152);
            string row10 = sLabyrinth.Remove(190);
            row10 = row10.Remove(0, 171);
            string row11 = sLabyrinth.Remove(209);
            row11 = row11.Remove(0, 190);
            string row12 = sLabyrinth.Remove(228);
            row12 = row12.Remove(0, 209);
            string row13 = sLabyrinth;
            row13 = row13.Remove(0, 228);

            string[] rows = { row1, row2, row3, row4, row5, row6, row7, row8, row9, row10, row11, row12, row13 };

            string[,] stringArr2D = new string[iRow, iColumn]; // creating 2D string Array

            var lines = rows;
            int row = 0;
            foreach (string line in lines)
            {
                int column = 0;
                foreach (char character in line)
                {
                    stringArr2D[row, column] = character.ToString();
                    column++;
                }
                row++;
            }

            // printing the Array in the Console
            int rowLength = stringArr2D.GetLength(0);
            int colLength = stringArr2D.GetLength(1);

            Console.WriteLine("StringArray2D");

            for (int rowl = 0; rowl < rowLength; rowl++)
            {
                for (int coll = 0; coll < colLength; coll++)
                {
                    Console.Write(string.Format("{0}", stringArr2D[rowl, coll]));
                }
                Console.Write(Environment.NewLine);
            }

            LabirinthArray2D = stringArr2D;

            FindPlayerPosition();

            this.xCoordinate = this.xStart;
            this.yCoordinate = this.yStart;
        }

        static void Main(string[] args)
        {
            Application.Run(new Game());
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF bounds = e.Graphics.VisibleClipBounds;       // um die Groesse des sichtbaren Bereichs zu ermitteln

            int x = 0, y = 0;

            Rectangle rectangle = new Rectangle(x, y, 400, 310);

            Font drawFont = new Font("Century Gothic", 17);
            SolidBrush drawBrush = new SolidBrush(Color.Green);

            // printing labyrinth in Form
            for (int i = 0; i < this.LabirinthArray2D.GetLength(0); i++)
            {
                for (int j = 0; j < this.LabirinthArray2D.GetLength(1); j++)
                {
                    rectangle = new Rectangle(x + (20 * j), y + (20 * i), 380, 260);

                    e.Graphics.DrawString(this.LabirinthArray2D[i, j], drawFont, drawBrush, rectangle);
                }
            }
        }

        private void FindPlayerPosition()
        {
            // find position of player character @
            string string2find = "@";

            for (int o = 0; o < this.LabirinthArray2D.GetLength(0); o++)
            {
                for (int p = 0; p < this.LabirinthArray2D.GetLength(1); p++)
                {
                    if (string2find == this.LabirinthArray2D[o, p])
                    {
                        this.xStart = p;
                        this.yStart = o;
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                GoUp();
            }
            if (e.KeyCode == Keys.Down)
            {
                GoDown();
            }
            if (e.KeyCode == Keys.Right)
            {
                GoRight();
            }
            if (e.KeyCode == Keys.Left)
            {
                GoLeft();
            }
        }

        private void GoUp()
        {
            // if the new position is a point ".", then replace point "." with player "@",
            // and replace old player "@" on old position with emtpy space " "
            if (this.LabirinthArray2D[this.yCoordinate - 1, this.xCoordinate] == ".")
            {
                this.yCoordinate -= 1;
                int old = this.yCoordinate + 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[old, this.xCoordinate] = " ";
                Refresh();
            }
            // if the new position is a wall "#", then do nothing
            else if (this.LabirinthArray2D[this.yCoordinate - 1, this.xCoordinate] == "#")
            {
                Refresh();
            }
            // if the new position is a empty space " ", then replace emtpy space " " with player "@",
            // and replace old player "@" on old position with emtpy space " "
            else if (this.LabirinthArray2D[this.yCoordinate - 1, this.xCoordinate] == " ")
            {
                this.yCoordinate -= 1;
                int old = this.yCoordinate + 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[old, this.xCoordinate] = " ";
                Refresh();
            }
        }

        private void GoDown()
        {

            if (this.LabirinthArray2D[this.yCoordinate + 1, this.xCoordinate] == ".")
            {
                this.yCoordinate += 1;
                int old = this.yCoordinate - 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[old, this.xCoordinate] = " ";
                Refresh();
            }
            else if (this.LabirinthArray2D[this.yCoordinate + 1, this.xCoordinate] == "#")
            {
                Refresh();
            }
            else if (this.LabirinthArray2D[this.yCoordinate + 1, this.xCoordinate] == " ")
            {
                this.yCoordinate += 1;
                int old = this.yCoordinate - 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[old, this.xCoordinate] = " ";
                Refresh();
            }
        }

        private void GoRight()
        {
            if (this.LabirinthArray2D[this.yCoordinate, this.xCoordinate + 1] == ".")
            {
                this.xCoordinate += 1;
                int old = this.xCoordinate - 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[this.yCoordinate, old] = " ";
                Refresh();
            }
            else if (this.LabirinthArray2D[this.yCoordinate, this.xCoordinate + 1] == "#")
            {
                Refresh();
            }
            else if (this.LabirinthArray2D[this.yCoordinate, this.xCoordinate + 1] == " ")
            {
                this.xCoordinate += 1;
                int old = this.xCoordinate - 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[this.yCoordinate, old] = " ";
                Refresh();
            }
        }

        private void GoLeft()
        {
            if (this.LabirinthArray2D[this.yCoordinate, this.xCoordinate - 1] == ".")
            {
                this.xCoordinate -= 1;
                int old = this.xCoordinate + 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[this.yCoordinate, old] = " ";
                Refresh();
            }
            else if (this.LabirinthArray2D[this.yCoordinate, this.xCoordinate - 1] == "#")
            {
                Refresh();
            }
            else if (this.LabirinthArray2D[this.yCoordinate, this.xCoordinate - 1] == " ")
            {
                this.xCoordinate -= 1;
                int old = this.xCoordinate + 1;
                this.LabirinthArray2D[this.yCoordinate, this.xCoordinate] = "@";
                this.LabirinthArray2D[this.yCoordinate, old] = " ";
                Refresh();
            }
        }
    }
}
