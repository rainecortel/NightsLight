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
    internal class Level3 : Form
    {
        private List<PictureBox> maze;
        private PictureBox player;
        private Timer playerMovementTimer;

        private int mazeLevel;
        private bool moveUp, moveDown, moveLeft, moveRight, changedLevel, slowTriggered, resetTriggered;
        private int playerSpeed;

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
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;

            // Define maze initial attributes.
            this.maze = new List<PictureBox>();
            this.mazeLevel = 1;
            this.playerSpeed = 8;
            this.changedLevel = false;
            this.slowTriggered = false;
            this.resetTriggered = false;

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
                BackColor = Color.Transparent
            };
            this.Controls.Add(player); // Adds the player sprite to the screen.
        }

        private void LoadMaze()
        {
            List<int[]> walls = new List<int[]>();

            switch (mazeLevel)
            {
                case 1:
                    this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_Map_01.png");

                    // Create a multiple PictureBox items for each wall in the drawn map.
                    walls = new List<int[]>
                    {
                        new int[]{0, 225, 47, 23}, new int[]{0, 561, 47, 23}, new int[]{93, 90, 90, 23}, new int[]{93, 359, 90, 23},
                        new int[]{163, 561, 160, 23}, new int[]{304, 359, 150, 23}, new int[]{445, 158, 90, 23}, new int[]{445, 292, 90, 23},
                        new int[]{445, 560, 90, 23}, new int[]{513, 90, 372, 23}, new int[]{585, 425, 90, 23}, new int[]{655, 225, 90, 23},
                        new int[]{655, 560, 90, 23}, new int[]{793, 358, 90, 23}, new int[]{866, 225, 120, 23}, new int[]{862, 493, 120, 23},
                        new int[]{163, 0, 23, 382}, new int[]{93, 360, 23, 90}, new int[]{304, 72, 23, 600}, new int[]{445, 158, 23, 420},
                        new int[]{513, 90, 23, 91}, new int[]{652, 225, 23, 358}, new int[]{862, 225, 23, 156}
                     };

                    // Add level traps.
                    maze.Add(AddTraps(230, 600, 50, 75, "slow"));
                    maze.Add(AddTraps(900, 255, 80, 70, "slow"));
                    maze.Add(AddTraps(553, 528, 56, 51, "slow"));
                    maze.Add(AddTraps(0, 615, 22, 48, "reset"));
                    maze.Add(AddTraps(361, 292, 48, 28, "reset"));
                    maze.Add(AddTraps(722, 614, 28, 48, "reset"));

                    // Add level goal.
                    maze.Add(AddGoal(960, 600, 20, 70));

                    break;
                case 2:
                    this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_Map_02.png");
                    this.player.Location = new Point(0, 0);
                    changedLevel = true;

                    walls = new List<int[]>
                    {
                        new int[]{455, 0, 19, 80}, new int[]{553, 0, 19, 170}, new int[]{356, 65, 19, 300}, new int[]{797, 65, 19, 340},
                        new int[]{161, 162, 19, 245}, new int[]{62, 259, 19, 300}, new int[]{308, 352, 19, 60}, new int[]{505, 352, 19, 105},
                        new int[]{553, 257, 19, 105}, new int[]{699, 257, 19, 250}, new int[]{407, 449, 19, 105}, new int[]{308, 497, 19, 56},
                        new int[]{601, 497, 19, 105}, new int[]{797, 497, 19, 105}, new int[]{896, 497, 19, 105}, new int[]{161, 591, 19, 82}, 
                        new int[]{261, 637, 19, 40}, new int[]{699, 591, 19, 82}, new int[]{65, 65, 310, 19}, new int[]{65, 161, 218, 19}, 
                        new int[]{356, 161, 216, 19}, new int[]{653, 161, 262, 19}, new int[]{653, 65, 163, 19}, new int[]{0, 257, 81, 19}, 
                        new int[]{0, 300, 81, 23}, new int[]{261, 257, 114, 19}, new int[]{458, 257, 114, 19}, new int[]{653, 257, 65, 19}, 
                        new int[]{898, 300, 82, 19}, new int[]{161, 398, 166, 19}, new int[]{308, 352, 67, 19}, new int[]{505, 352, 113, 19}, 
                        new int[]{797, 398, 67, 19}, new int[]{161, 492, 166, 19}, new int[]{308, 539, 118, 19}, new int[]{407, 449, 117, 19},
                        new int[]{505, 591, 115, 19}, new int[]{797, 591, 118, 19}, new int[]{601, 492, 215, 19}
                     };

                    maze.Add(AddTraps(12, 491, 35, 37, "slow"));
                    maze.Add(AddTraps(389, 191, 35, 37, "slow"));
                    maze.Add(AddTraps(538, 616, 35, 51, "slow"));
                    maze.Add(AddTraps(497, 0, 30, 24, "reset"));
                    maze.Add(AddTraps(761, 106, 30, 30, "reset"));
                    maze.Add(AddTraps(498, 201, 30, 30, "reset"));
                    maze.Add(AddTraps(320, 297, 30, 30, "reset"));
                    maze.Add(AddTraps(680, 0, 58, 65, "lava"));
                    maze.Add(AddTraps(916, 515, 61, 70, "lava"));

                    maze.Add(AddGoal(965, 238, 20, 53));

                    break;
                default:
                    break;
            }

            if(mazeLevel > 2)
            {
                Console.WriteLine(Program.GetCurrentTime() + " - Level 3 completed.");

                // Load main menu form when exiting this level.
                Program.getGameMenuForm().Show();

                // Close this form.
                this.Close();
            }

            if(walls.Count > 0)
            {
                // Add walls.
                foreach (int[] w in walls)
                {
                    maze.Add(AddWalls(w[0], w[1], w[2], w[3]));
                }
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
                BackColor = Color.Transparent
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
                BackColor = Color.Transparent
            };
            this.Controls.Add(goal);

            return goal;
        }

        private PictureBox AddTraps(int x, int y, int w, int h, string s)
        {
            PictureBox trap = new PictureBox
            {
                Name = s,
                Tag = s,
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(x, y),
                Size = new Size(w, h),
                BackColor = Color.Transparent
            };
            this.Controls.Add(trap);

            return trap;
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
                if(changedLevel)
                {
                    this.player.Location = new Point(0, 0);
                    changedLevel = false;
                    playerMovement.Location = new Point(0, 0);
                }
                else
                {
                    if(resetTriggered)
                    {
                        if(mazeLevel == 1)
                        {
                            playerMovement.Location = new Point(0, 0);
                        }
                        else if(mazeLevel == 2)
                        {
                            playerMovement.Location = new Point(0, 325);
                        }
                        
                        resetTriggered = false;
                    }

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
                if ((c is PictureBox) && ((string)c.Tag == "wall") && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    hit = true;

                    break;
                }
                if ((c is PictureBox) && ((string)c.Tag == "slow") && (slowTriggered == false) && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    // Delay player's speed.
                    playerSpeed = 2;
                    slowTriggered = true;

                    // Create a task thread that will delay setting the playerSpeed back to normal.
                    Task.Delay(5000).ContinueWith((_) =>
                    {
                        playerSpeed = 8;
                        slowTriggered = false;
                    });

                    break;
                }
                if ((c is PictureBox) && ((string)c.Tag == "reset") && (resetTriggered == false) &&  (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    resetTriggered = true;

                    // Sound effect for death.
                    SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/PressurePlate.wav");
                    simpleSound.Play();

                    System.Threading.Thread.Sleep(500);

                    break;
                }
                if ((c is PictureBox) && ((string)c.Tag == "lava") && (resetTriggered == false) && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    mazeLevel -= 1;
                    maze.RemoveAll(isWall);

                    // Remove all controls in the form except the player.
                    for (int i = this.Controls.Count - 1; i > 0; i--)
                    {
                        this.Controls.RemoveAt(i);
                    }

                    // Sound effect for death.
                    SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/Lava.wav");
                    simpleSound.Play();

                    System.Threading.Thread.Sleep(1000);
                    LoadMaze();
                    this.player.Location = new Point(0, 0);
                    changedLevel = true;

                    break;
                }
                if ((c is PictureBox) && ((string)c.Tag == "goal") && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    mazeLevel += 1;
                    maze.RemoveAll(isWall);

                    // Remove all controls in the form except the player.
                    for (int i = this.Controls.Count - 1; i > 0; i--)
                    {
                        this.Controls.RemoveAt(i);
                    }

                    // Display level complete text.
                    /*PictureBox levelCompleted = new PictureBox
                    {
                        BackColor = Color.Black,
                        BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_LevelCompleted.png"),
                        BackgroundImageLayout = ImageLayout.Zoom,
                        Size = new Size(900, 153),
                        SizeMode = PictureBoxSizeMode.CenterImage
                    };
                    levelCompleted.Location = new Point((this.Width / 2) - (levelCompleted.Width / 2), (this.Height / 2) - (levelCompleted.Height / 2) - 25);*/
                    //this.Controls.Add(levelCompleted);

                    // Sound effect for level clear.
                    SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/LevelComplete.wav");
                    simpleSound.Play();

                    // Make the current thread sleep to give way for the level clear sound effects.
                    System.Threading.Thread.Sleep(3050);
                    LoadMaze();

                    break;
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
            if(e.KeyCode == Keys.W)
            {
                moveUp = true;
            }
            if(e.KeyCode == Keys.S)
            {
                moveDown = true;
            }
            if(e.KeyCode == Keys.A)
            {
                moveLeft = true;
            }
            if(e.KeyCode == Keys.D)
            {
                moveRight = true;
            }
        }

        // Used to prevent infinite movement in the direction pressed during a key press event.
        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            // When key is released, movement stops.
            playerMovementTimer.Enabled = false;

            if (e.KeyCode == Keys.W)
            {
                moveUp = false;
            }
            if (e.KeyCode == Keys.S)
            {
                moveDown = false;
            }
            if (e.KeyCode == Keys.A)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.D)
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
