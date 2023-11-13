using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightsLight
{
    internal class Level3 : Form
    {
        private List<PictureBox> maze;
        private PictureBox player;
        private Timer playerMovementTimer;

        private int mazeLevel;
        private bool moveUp, moveDown, moveLeft, moveRight;
        private int playerSpeed = 8;

        public Level3()
        {
            Console.WriteLine(Program.GetCurrentTime() + " - Level 3 is loaded.");

            // Define form properties.
            this.Text = "Night's Light(Level 3)";

            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoScroll = false;
            this.AutoSize = false;
            this.BackColor = Color.FromArgb(65, 65, 65);
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.CausesValidation = true;
            this.DoubleBuffered = true;
            this.Enabled = true;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.KeyPreview = false;
            this.Location = new Point(0, 0);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Size = new Size(1000, 715);
            this.StartPosition = FormStartPosition.Manual;
            this.WindowState = FormWindowState.Normal;

            // Define maze initial attributes.
            this.maze = new List<PictureBox>();
            this.mazeLevel = 1;

            InitializePlayer();
            LoadMaze();
            InitializeTimer();
            InitializeEvents();
        }

        private void InitializePlayer()
        {
            player = new PictureBox
            {
                Name = "player",
                SizeMode = PictureBoxSizeMode.AutoSize,
                Location = new Point(0, 0),
                Image = Image.FromFile(Program.currentDirectory + "/Assets/Player/character_01.png"),
                BackColor = Color.DarkBlue
            };
            this.Controls.Add(player); // Adds the player sprite to the screen.
        }

        private void LoadMaze()
        {
            switch (mazeLevel)
            {
                case 1:
                    this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_Map_01.png");

                    // Create a multiple PictureBox items for each wall in the drawn map.
                    List<int[]> walls = new List<int[]>
                    {
                        new int[]{0, 225, 47, 23}, new int[]{0, 561, 47, 23}, new int[]{93, 90, 90, 23}, new int[]{93, 359, 90, 23},
                        new int[]{163, 561, 160, 23}, new int[]{304, 359, 150, 23}, new int[]{445, 158, 90, 23}, new int[]{445, 292, 90, 23},
                        new int[]{445, 560, 90, 23}, new int[]{513, 90, 372, 23}, new int[]{585, 425, 90, 23}, new int[]{655, 225, 90, 23},
                        new int[]{655, 560, 90, 23}, new int[]{793, 358, 90, 23}, new int[]{866, 225, 120, 23}, new int[]{862, 493, 120, 23},
                        new int[]{163, 0, 23, 382}, new int[]{93, 360, 23, 90}, new int[]{304, 72, 23, 600}, new int[]{445, 158, 23, 420},
                        new int[]{513, 90, 23, 91}, new int[]{652, 225, 23, 358}, new int[]{723, 560, 23, 120}, new int[]{862, 225, 23, 156}
                     };

                    // Add walls.
                    foreach (int[] w in walls)
                    {
                        maze.Add(AddWalls(w[0], w[1], w[2], w[3]));
                    }

                    // Add level goal.
                    maze.Add(AddGoal(940, 600, 50, 70));

                    break;
                case 2:
                    this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_Map_02.png");

                    break;
                default:
                    break;
            }
        }

        private PictureBox AddWalls(int x, int y, int w, int h)
        {
            PictureBox wall = new PictureBox
            {
                Name = "wall",
                Tag = "wall",
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.Black
            };
            this.Controls.Add(wall);

            return wall;
        }

        private PictureBox AddGoal(int x, int y, int w, int h)
        {
            PictureBox goal = new PictureBox
            {
                Name = "goal",
                Tag = "goal",
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.Red
            };
            this.Controls.Add(goal);

            return goal;
        }

        private void InitializeTimer()
        {
            playerMovementTimer = new Timer();
            playerMovementTimer.Interval = 20; // Lower interval equals faster movement.
            playerMovementTimer.Tick += new EventHandler(playerMovementTimerEvent);
        }

        private void playerMovementTimerEvent(object sender, EventArgs e)
        {
            // Make a new PictureBox to detect the change in location and used for collision testing.
            PictureBox playerMovement = new PictureBox
            {
                Location = player.Location,
                Size = player.Size,
                BackColor = Color.Transparent
            };
            this.Controls.Add(playerMovement); // Add PB to controls.

            if (moveUp && player.Top > 0)
            {
                playerMovement.Top -= playerSpeed;
            }
            if(moveDown && player.Top < 612)
            {
                playerMovement.Top += playerSpeed;
            }
            if(moveLeft && player.Left > 0)
            {
                playerMovement.Left -= playerSpeed;
            }
            if(moveRight && player.Left < 928)
            {
                playerMovement.Left += playerSpeed;
            }

            // If collision PictureBox did not collide with any elements, pass its Location to the player.
            if(IsCollision(playerMovement) == false)
            {
                if (maze.Count == 0)
                {
                    this.player.Location = new Point(0, 0);
                }
                else
                {
                    player.Location = playerMovement.Location;
                }
            }

            this.Controls.Remove(playerMovement); // Remove PB after using it.
        }

        private bool IsCollision(PictureBox p)
        {
            var hit = false;

            foreach (Control c in this.Controls)
            {
                if ((c is PictureBox) && ((string)c.Tag == "wall"))
                {
                    if (p.Bounds.IntersectsWith(c.Bounds))
                    {
                        hit = true;
                        break;
                    }
                }
                if ((c is PictureBox) && ((string)c.Tag == "goal"))
                {
                    if (p.Bounds.IntersectsWith(c.Bounds))
                    {
                        //SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/LevelComplete.wav");
                        //simpleSound.Play();

                        mazeLevel += 1;
                        maze.RemoveAll(isWall);

                        for(int i = this.Controls.Count - 1; i > 0; i--)
                        {
                            this.Controls.RemoveAt(i);
                        }

                        LoadMaze();

                        break;
                    }
                }
            }

            return hit;
        }

        private bool isWall(PictureBox pb)
        {
            return (string)pb.Tag != "player";
        }

        private void InitializeEvents()
        {
            // Add event handler for keypress.
            this.KeyDown += KeyDownEvent;

            // Add event handler so keypress is not infinite.
            this.KeyUp += KeyUpEvent;

            // Add event to catch form closing.
            //this.FormClosing += FormClosingEvent;
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            // Enable player movement time to allow moving.
            playerMovementTimer.Enabled = true; 

            // Key press events to trigger.
            if(e.KeyCode == Keys.Up)
            {
                moveUp = true;
            }
            if(e.KeyCode == Keys.Down)
            {
                moveDown = true;
            }
            if(e.KeyCode == Keys.Left)
            {
                moveLeft = true;
            }
            if(e.KeyCode == Keys.Right)
            {
                moveRight = true;
            }
        }

        // Used to prevent infinite movement in the direction pressed during a key press event.
        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            // When key is released, movement stops.
            playerMovementTimer.Enabled = false;

            if (e.KeyCode == Keys.Up)
            {
                moveUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                moveDown = false;
            }
            if (e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                moveRight = false;
            }
        }

        // X button has been clicked.
        private void FormClosingEvent(object sender, FormClosingEventArgs e)
        {
            var x = MessageBox.Show(this, "Your progress will not be saved and you will return to the main menu. Are you sure you want to exit?", "Exiting Level 3", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if(x == DialogResult.Yes)
            {
                Console.WriteLine(Program.GetCurrentTime() + " - Level 3 is closed.");

                // Load main menu form when exiting this level.
                Program.getGameMenuForm().Show();
            }
        }
    }
}
