using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightsLight
{
    internal class Level1 : Form
    {
        private Image player;
        private List<string> playerMovements = new List<string>();
        private int steps = 0;
        private int slowDownFrameRate = 0; // To avoid super speed of the character
        private bool left, right, up, down;
        private int playerX = 380; // place the character image horizontal
        private int playerY = 160; // place the character image vertical
        private int playerHeight = 70; // size of the image character height
        private int playerWidth = 70; // size of the image character width
        private int playerSpeed = 8;

        public Level1()
        {
            InitializeForm();
            InitializeEvents();
            InitializeTimer();
        }

        private void InitializeForm()
        {
            this.Text = "Night's Light (Level 1)";
            this.Width = 980;
            this.Height = 700;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Map001.png");

            // Set the sound for Level 1
            SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/Town1.wav");
            simpleSound.Play();

            playerImages();
        }

        private void InitializeEvents()
        {
            this.Paint += new PaintEventHandler(FormPaintEvent);
            this.KeyDown += new KeyEventHandler(KeyDownMovement);
            this.KeyUp += new KeyEventHandler(KeyUpMovement);
        }

        private void InitializeTimer()
        {
            Timer timer = new Timer();
            timer.Tick += new EventHandler(TimerEvent);
            timer.Interval = 1000 / 30;
            timer.Start();
        }

        private void KeyDownMovement(object sender, KeyEventArgs e)
        {
            // Handle key down events
            HandleKeyDown(e.KeyCode);
        }

        private void KeyUpMovement(object sender, KeyEventArgs e)
        {
            // Handle key up events
            HandleKeyUp(e.KeyCode);
        }

        private void HandleKeyDown(Keys keyCode)
        {
            if (keyCode == Keys.Left)
                left = true;
            if (keyCode == Keys.Right)
                right = true;
            if (keyCode == Keys.Up)
                up = true;
            if (keyCode == Keys.Down)
                down = true;
        }

        private void HandleKeyUp(Keys keyCode)
        {
            if (keyCode == Keys.Left)
                left = false;
            if (keyCode == Keys.Right)
                right = false;
            if (keyCode == Keys.Up)
                up = false;
            if (keyCode == Keys.Down)
                down = false;
        }

        private void FormPaintEvent(object sender, PaintEventArgs e)
        {
            // Paint the player character
            Graphics Canvas = e.Graphics;
            Canvas.DrawImage(player, playerX, playerY, playerWidth, playerHeight);
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            // Handle timer tick events for animation and movement
            HandleTimerTick();
            this.Invalidate();
        }

        private void HandleTimerTick()
        {
            if (left && playerX > 0)
            {
                playerX -= playerSpeed;
                AnimationPlayer(4, 7); // images index list
            }
            else if (right && playerX + playerWidth < this.ClientSize.Width)
            {
                playerX += playerSpeed;
                AnimationPlayer(8, 11); // images index list
            }
            else if (up && playerY > 0)
            {
                playerY -= playerSpeed;
                AnimationPlayer(12, 15); // images index list
            }
            else if (down && playerY + playerHeight < this.ClientSize.Height)
            {
                playerY += playerSpeed;
                AnimationPlayer(0, 3); // images index list
            }
            else
            {
                AnimationPlayer(0, 0);
            }
        }

        private void playerImages()
        {
            // To avoid flashing of screen
            this.DoubleBuffered = true;
            // Load the player images files to the list
            playerMovements = Directory.GetFiles(Program.currentDirectory + "/Assets/Player/", "*.png").ToList();
            player = Image.FromFile(playerMovements[0]);
        }

        private void AnimationPlayer(int start, int end)
        {
            slowDownFrameRate += 1;

            if (slowDownFrameRate == 4)
            {
                steps++;
                slowDownFrameRate = 0;
            }
            if (steps > end || steps < start)
            {
                steps = start;
            }

            player = Image.FromFile(playerMovements[steps]);
        }
    }
}
