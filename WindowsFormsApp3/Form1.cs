using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        private List<Button> myButtons = new List<Button>();
        private bool isCreating = false;
        private const int minWidth = 30; // минимальная ширина кнопки, иначе она удаляется
        private const int minHeight = 30; // минимальная высота кнопки, иначе она удаляется
        private static int startPointX;
        private static int startPointY;
        private Timer form1Timer = null;
        private Color myBackColor = Color.White;
        private int colorChanel;
        Random rnd = new Random();


        public Form1()
        {
            InitializeComponent();
            InitializeForm1Timer();
            InitializeForm1MouseHandler();
        }

        private void InitializeForm1MouseHandler() // обработчик мыши для формы.
        {
            this.MouseDown += MouseDownHandler;
            this.MouseMove += MouseMoveHandler;
            this.MouseUp += MouseUpHandler;
            this.MouseWheel += MouseWheelHandler;
        }

        private void InitializeForm1Timer() // обработчик мыши для формы.
        {
            form1Timer = new Timer();
            form1Timer.Interval = 1; // Интервал таймера 10 милисекунд
            form1Timer.Tick += Form1TimerTick;
            colorChanel = rnd.Next(7);

        }

        private void MakeNewButton(int starPositionX, int startPositionY) // метод создания нового объекта кнопки
        {
            this.myButtons.Add(new Button());
            this.SuspendLayout();
            this.myButtons.Last().MouseUp += ButtonMouseUpHandler; //  обработчик события MouseUp для каждой кнопки, для удаления кн.                                                                  
            this.myButtons.Last().Name = "Button N" + this.myButtons.Count;
            this.myButtons.Last().Text = "Кнопка N" + this.myButtons.Count;
            this.myButtons.Last().TabIndex = 0;
            this.myButtons.Last().UseVisualStyleBackColor = true;
            this.myButtons.Last().Location = new Point(starPositionX, startPositionY);
            this.myButtons.Last().Size = new Size(10, 10);
            this.myButtons.Last().Visible = true;
            this.Controls.Add((Control)this.myButtons.Last());
            this.ResumeLayout();
        }


        private void Form1TimerTick(object sender, EventArgs e)
        {
            int green = this.BackColor.G;
            int blue = this.BackColor.B;
            int red = this.BackColor.R;

            switch (colorChanel)
            {
                case 6:
                    green = (green < 255) ? (green + 1) : 255;
                    if (green >= 255) colorChanel = rnd.Next(7);
                    break;
                case 1:
                    blue = (blue < 255) ? (blue + 1) : 255;
                    if (blue >= 255) colorChanel = rnd.Next(7);
                    break;
                case 2:
                    red = (red < 255) ? (red + 1) : 255;
                    if (red >= 255) colorChanel = rnd.Next(7);
                    break;
                case 3:
                    green = (green > 0) ? (green - 1) : 0;
                    if (green <= 0) colorChanel = rnd.Next(7);
                    break;
                case 4:
                    blue = (blue > 0) ? (blue - 1) : 0;
                    if(blue <= 0) colorChanel = rnd.Next(7);
                    break;
                case 5:
                    red = (red > 0) ? (red - 1) : 0;
                    if (red <= 0)colorChanel = rnd.Next(7);
                    break;
                default:
                    colorChanel = rnd.Next(7);
                    break;
            }

            Color newColor = Color.FromArgb(red, green, blue);
            this.BackColor = newColor;
        }


        private void ButtonMouseUpHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Button button = (Button)sender;
                DeleteButton(button);
            }
        }


        private void DeleteButton(Button button)
        {
            button.MouseUp -= ButtonMouseUpHandler; // Удаляем обработчик события MouseUp для кнопки
            myButtons.Remove(button);
            Controls.Remove(button);
            button.Dispose();
        }





        private void MouseDownHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.toolStripStatusLabel2.BackColor = Color.Green;
                startPointX = e.X;
                startPointY = e.Y;
                MakeNewButton(startPointX, startPointY);
                isCreating = true;
            }

            if (e.Button == MouseButtons.Right)
            {
                this.toolStripStatusLabel4.BackColor = Color.Green;
            }

            if (e.Button == MouseButtons.Middle)
            {
                this.toolStripStatusLabel3.BackColor = Color.Green;
            }
        }


        private void MouseUpHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isCreating = false;
                this.toolStripStatusLabel2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
                if (myButtons.Any() && (myButtons.Last().Size.Width < minWidth || myButtons.Last().Size.Height < minHeight))
                {
                    DeleteButton(myButtons.Last());
                }
            }

            if (e.Button == MouseButtons.Right)
            {
                this.toolStripStatusLabel4.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            }

            if (e.Button == MouseButtons.Middle)
            {
                this.toolStripStatusLabel3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            }
        }


        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = $"X: {e.X}, Y: {e.Y}";
            if (isCreating)
            {
                int newWidth;
                int newHeight;
                int newLocationX;
                int newLocationY;
                if (e.X < startPointX)
                {
                    if (e.X < 1)
                    {
                        newLocationX = 1;
                        newWidth = startPointX - 1;
                    }
                    else
                    {
                        newLocationX = e.X;
                        newWidth = startPointX - e.X;
                    }
                }
                else
                {
                    newLocationX = startPointX;
                    if (e.X > this.Width - 17)
                    {
                        newWidth = this.Width - 17 - startPointX;
                    }
                    else
                    {
                        newWidth = e.X - startPointX;
                    }
                }

                if (e.Y < startPointY)
                    if (e.Y < 1)
                    {
                        newLocationY = 1;
                        newHeight = startPointY - 1;
                    }
                    else
                    {
                        newLocationY = e.Y;
                        newHeight = startPointY - e.Y;
                    }
                else
                {
                    newLocationY = startPointY;
                    if (e.Y > this.Height - 64)
                    {
                        newHeight = this.Height - 64 - startPointY;
                    }
                    else
                    {
                        newHeight = e.Y - startPointY;
                    }
                }

                myButtons.Last().Location = new Point(newLocationX, newLocationY);
                myButtons.Last().Size = new Size(newWidth, newHeight);
            }
        }



        private void MouseWheelHandler(object sender, MouseEventArgs e)
        {
            if (myButtons.Any())
            {
                Random random = new Random();
                Color randomColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
                myButtons.Last().BackColor = randomColor;
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            form1Timer.Enabled = true;
        }


    }
}
