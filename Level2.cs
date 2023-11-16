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
    public class Player : PictureBox
    {
        public int Speed { get; set; }
        public bool MoveUp { get; set; }
        public bool MoveDown { get; set; }
        public bool MoveLeft { get; set; }
        public bool MoveRight { get; set; }

        public Player()
        {
            // Initialize Player properties.
            this.Name = "player";
            this.BackColor = Color.Transparent;
            this.Image = Image.FromFile(Program.currentDirectory + "/Assets/Player/character_01.png");
            this.SizeMode = PictureBoxSizeMode.AutoSize;
            this.Location = new Point(0, 0); // Set initial location. Location adjusts in the code.

            // Define Player initial attributes.
            Speed = 8;
        }

        // Set which move was triggered by the player.
        public void MoveLocation(Keys keyCode, string eventName)
        {
            if (eventName == "KeyDown")
            {
                switch (keyCode)
                {
                    case Keys.W:
                        MoveUp = true;
                        break;
                    case Keys.S:
                        MoveDown = true;
                        break;
                    case Keys.A:
                        MoveLeft = true;
                        break;
                    case Keys.D:
                        MoveRight = true;
                        break;
                    default:
                        break;
                }
            }
            else if (eventName == "KeyUp")
            {
                switch (keyCode)
                {
                    case Keys.W:
                        MoveUp = false;
                        break;
                    case Keys.S:
                        MoveDown = false;
                        break;
                    case Keys.A:
                        MoveLeft = false;
                        break;
                    case Keys.D:
                        MoveRight = false;
                        break;
                    default:
                        break;
                }
            }
        }

        // Change Player's speed.
        public void ChangeSpeed(int speed)
        {
            Speed = speed;
        }
    }

    public class Maze : List<PictureBox>
    {
        public int Level { get; set; }

        public Maze(int level)
        {
            // Initialize Maze properties.
            Level = level;
        }

        public Image GetBackgroundImage()
        {
            if (Level == 1)
            {
                return Image.FromFile(Program.currentDirectory + "/Assets/Images/Level2_Map_01.png");
            }
            else if (Level == 2)
            {
                return Image.FromFile(Program.currentDirectory + "/Assets/Images/Level2_Map_02.png");
            }

            return null;
        }

        public void AddToMaze(int[] coordinate, string name, string tag)
        {
            PictureBox pb = new PictureBox
            {
                Name = name,
                Tag = tag,
                SizeMode = PictureBoxSizeMode.Normal,
                Location = new Point(coordinate[0], coordinate[1]),
                Size = new Size(coordinate[2], coordinate[3]),
                BackColor = Color.Transparent
            };

            this.Add(pb);
        }

        public void ClearMaze()
        {
            this.Clear();
        }
    }

    internal class Level2 : Form
    {
        private Player player;
        private Maze maze;
        private Timer timer;

        private bool slowTriggered;

        public Level2()
        {
            Console.WriteLine(Program.GetCurrentTime() + " - Level 2 is loaded.");

            // Define form properties.
            this.Text = "Night's Light(Level 2)";

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

            // Define Level2 initial attributes.
            player = new Player();
            maze = new Maze(1);
            timer = new Timer();
            slowTriggered = false;

            this.Controls.Add(player); // Adds the player sprite to the screen.

            LoadMaze();
            InitializeEvents();
            SetTimer();
        }

        private void LoadMaze()
        {
            if (maze.Level > 2)
            {
                Console.WriteLine(Program.GetCurrentTime() + " - Level 2 completed.");

                // Load main menu form when exiting this level.
                Program.getGameMenuForm().Show();

                // Dispose the timer.
                timer.Dispose();

                // Close this form.
                this.Close();
            }
            else
            {
                List<int[]> walls = new List<int[]>();

                // Load background image based on maze level.
                this.BackgroundImage = maze.GetBackgroundImage();

                switch (maze.Level)
                {
                    case 1:
                        // Create a multiple int[] for each wall to be drawn in the maze.
                        walls = new List<int[]>
                        {
                            new int[] {0, 225, 47, 23}, new int[] {0, 561, 47, 23}, new int[] {93, 90, 90, 23}, new int[] {93, 359, 90, 23},
                            new int[] {163, 561, 160, 23}, new int[] {445, 158, 90, 23}, new int[] {445, 292, 90, 23}, new int[] {445, 560, 90, 23}, 
                            new int[] {513, 90, 372, 23}, new int[] {585, 425, 90, 23}, new int[] {655, 225, 90, 23}, new int[] {655, 560, 90, 23}, 
                            new int[] {793, 358, 90, 23}, new int[] {866, 225, 120, 23}, new int[] {862, 493, 120, 23}, new int[] {163, 0, 23, 382}, 
                            new int[] {93, 360, 23, 90}, new int[] {304, 72, 23, 600}, new int[] {445, 158, 23, 420}, new int[] {513, 90, 23, 91}, 
                            new int[] {652, 225, 23, 358}, new int[] {862, 225, 23, 156}
                         };

                        // Add level traps.
                        maze.AddToMaze(new int[] { 230, 600, 50, 75 }, "trap", "slow");
                        maze.AddToMaze(new int[] { 900, 255, 80, 70 }, "trap", "slow");
                        maze.AddToMaze(new int[] { 553, 528, 56, 51 }, "trap", "slow");
                        maze.AddToMaze(new int[] { 0, 615, 22, 48 }, "trap", "reset");
                        maze.AddToMaze(new int[] { 361, 292, 48, 28 }, "trap", "reset");
                        maze.AddToMaze(new int[] { 722, 614, 28, 48 }, "trap", "reset");

                        // Add level goal.
                        maze.AddToMaze(new int[] { 940, 600, 40, 70 }, "goal", "goal");
                        break;
                    case 2:
                        walls = new List<int[]>
                        {
                            new int[] {455, 0, 19, 80}, new int[] {553, 0, 19, 170}, new int[] {356, 65, 19, 300}, new int[] {797, 65, 19, 340},
                            new int[] {161, 162, 19, 245}, new int[] {62, 259, 19, 300}, new int[] {308, 352, 19, 60}, new int[] {505, 352, 19, 105},
                            new int[] {553, 257, 19, 105}, new int[] {699, 257, 19, 250}, new int[] {407, 449, 19, 105}, new int[] {308, 497, 19, 56},
                            new int[] {601, 497, 19, 105}, new int[] {797, 497, 19, 105}, new int[] {896, 497, 19, 105}, new int[] {161, 591, 19, 82},
                            new int[] {261, 637, 19, 40}, new int[] {699, 591, 19, 82}, new int[] {65, 65, 310, 19}, new int[] {65, 161, 218, 19},
                            new int[] {356, 161, 216, 19}, new int[] {653, 161, 262, 19}, new int[] {653, 65, 163, 19}, new int[] {0, 257, 81, 19},
                            new int[] {0, 300, 81, 23}, new int[] {261, 257, 114, 19}, new int[] {458, 257, 114, 19}, new int[] {653, 257, 65, 19},
                            new int[] {898, 300, 82, 19}, new int[] {161, 398, 166, 19}, new int[] {308, 352, 67, 19}, new int[] {505, 352, 113, 19},
                            new int[] {797, 398, 67, 19}, new int[] {161, 492, 166, 19}, new int[] {308, 539, 118, 19}, new int[] {407, 449, 117, 19},
                            new int[] {505, 591, 115, 19}, new int[] {797, 591, 118, 19}, new int[] {601, 492, 215, 19}
                         };

                        maze.AddToMaze(new int[] { 12, 491, 35, 37 }, "trap", "slow");
                        maze.AddToMaze(new int[] { 389, 191, 35, 37 }, "trap", "slow");
                        maze.AddToMaze(new int[] { 538, 616, 35, 51 }, "trap", "slow");
                        maze.AddToMaze(new int[] { 497, 0, 30, 24 }, "trap", "reset");
                        maze.AddToMaze(new int[] { 761, 106, 30, 30 }, "trap", "reset");
                        maze.AddToMaze(new int[] { 498, 201, 30, 30 }, "trap", "reset");
                        maze.AddToMaze(new int[] { 320, 297, 30, 30 }, "trap", "reset");
                        maze.AddToMaze(new int[] { 680, 0, 58, 65 }, "trap", "lava");
                        maze.AddToMaze(new int[] { 916, 515, 61, 70 }, "trap", "lava");

                        maze.AddToMaze(new int[] { 955, 238, 40, 53 }, "goal", "goal");

                        break;
                    default:
                        break;
                }


                // Add level walls.
                foreach (int[] w in walls)
                {
                    maze.AddToMaze(new int[] { w[0], w[1], w[2], w[3] }, "wall", "wall");
                }

                // Add maze to form controls.
                foreach (PictureBox pb in maze)
                {
                    this.Controls.Add(pb);
                }
            }
        }

        private void InitializeEvents()
        {
            // Add event handler for keypress.
            this.KeyDown += KeyDownEvent;

            // Add event handler so keypress is not infinite.
            this.KeyUp += KeyUpEvent;
        }

        private void KeyDownEvent(object sender, KeyEventArgs e)
        {
            // Enable player movement time to allow moving.
            timer.Enabled = true;

            player.MoveLocation(e.KeyCode, "KeyDown");
        }

        private void KeyUpEvent(object sender, KeyEventArgs e)
        {
            // When key is released, movement stops.
            timer.Enabled = false;

            player.MoveLocation(e.KeyCode, "KeyUp");
        }

        private void SetTimer()
        {
            timer.Interval = 20; // Lower interval equals faster movement.
            timer.Tick += new EventHandler(MovePlayer);
        }

        private void MovePlayer(object sender, EventArgs e)
        {
            // Make a new PictureBox to detect the change in location and used for collision testing.
            PictureBox movement = new PictureBox
            {
                Location = player.Location,
                Size = player.Size,
                BackColor = Color.Transparent
            };
            this.Controls.Add(movement); // Add PictureBox to controls.

            if (player.MoveUp && player.Top > 0)
            {
                movement.Top -= player.Speed;
            }
            if (player.MoveDown && player.Top < 612)
            {
                movement.Top += player.Speed;
            }
            if (player.MoveLeft && player.Left > 0)
            {
                movement.Left -= player.Speed;
            }
            if (player.MoveRight && player.Left < 928)
            {
                movement.Left += player.Speed;
            }

            // Check for collison between Player and Maze elements.
            CheckForCollision(ref movement);

            // Update the location of Player based on collision results.
            player.Location = movement.Location;

            this.Controls.Remove(movement); // Remove PictureBox after using it.
        }

        private void CheckForCollision(ref PictureBox p)
        {
            foreach (Control c in this.Controls)
            {
                // If collision is with wall, prevent the next movement.
                if ((c is PictureBox) && ((string)c.Tag == "wall") && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    p.Location = player.Location;
                    break;
                }

                // If collision is with stone, slow the movement speed.
                if ((c is PictureBox) && ((string)c.Tag == "slow") && (slowTriggered == false) && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    // Delay player's speed.
                    player.Speed = 2;
                    slowTriggered = true;

                    // Create a task thread that will delay setting the player's speed back to normal.
                    Task.Delay(5000).ContinueWith((_) =>
                    {
                        player.Speed = 8;
                        slowTriggered = false;
                    });
                }

                // If collision is with pressure plate, return the player to starting point (if level 1) or to teleporter area (if level 2).
                if ((c is PictureBox) && ((string)c.Tag == "reset") && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    // Sound effect for hitting pressure plate.
                    SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/PressurePlate.wav");
                    simpleSound.Play();

                    // Pause the application for 0.5s.
                    System.Threading.Thread.Sleep(500);

                    switch (maze.Level)
                    {
                        case 1:
                            p.Location = new Point(0, 0);
                            break;
                        case 2:
                            p.Location = new Point(0, 325);
                            break;
                        default:
                            break;
                    }

                    break;
                }

                // If collision is with lava (only for level 2), return player to level 1's starting point.
                if ((c is PictureBox) && ((string)c.Tag == "lava") && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    maze.Level = 1;
                    maze.ClearMaze();

                    // Remove all controls in the form except the player.
                    for (int i = this.Controls.Count - 1; i > 0; i--)
                    {
                        this.Controls.RemoveAt(i);
                    }

                    // Sound effect for hitting lava.
                    SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/Lava.wav");
                    simpleSound.Play();

                    // Pause the application for 1s.
                    System.Threading.Thread.Sleep(1000);

                    LoadMaze();
                    p.Location = new Point(0, 0);

                    break;
                }

                // If collision is with control, proceed to next level (if level 1). Else, exit.
                if ((c is PictureBox) && ((string)c.Tag == "goal") && (p.Bounds.IntersectsWith(c.Bounds)))
                {
                    maze.Level += 1;
                    maze.ClearMaze();

                    for (int i = this.Controls.Count - 1; i > 0; i--)
                    {
                        this.Controls.RemoveAt(i);
                    }

                    // Sound effect for level clear.
                    SoundPlayer simpleSound = new SoundPlayer(Program.currentDirectory + "/Assets/Audio/LevelComplete.wav");
                    simpleSound.Play();

                    // Make the current thread sleep to give way for the level clear sound effects.
                    System.Threading.Thread.Sleep(3050);

                    LoadMaze();
                    p.Location = new Point(0, 0);

                    break;
                }
            }
        }
    }
}
