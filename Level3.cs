using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightsLight
{
    internal class Level3 : Form
    {
        private List<PictureBox> maze;
        private List<Rectangle> mazeHitbox;
        private Rectangle screen;
        private PictureBox player;
        private Timer playerMovementTimer;
        private string playerMovement;
        private int playerSpeed = 8;
        public Level3()
        {
            Console.WriteLine(Program.GetCurrentTime() + " - Level 3 is loaded.");

            this.Text = "Night's Light (Level 3)";
            this.Width = 1000;
            this.Height = 715;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;

            this.BackColor = Color.FromArgb(65, 65, 65);
            this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_Map_01.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Prevent flickering of the background image.
            this.DoubleBuffered = true;

            screen = new Rectangle(new Point(0, 0), this.Size);

            InitializeMaze();
            InitializePlayer();
            InitializeTimer();
            InitializeEvents();

            // Add event to catch form closing.
            //this.FormClosing += Level3_FormClosing;
        }

        private void InitializeMaze()
        {
            maze = new List<PictureBox>();

            // All horizontal walls.
            maze.Add(AddWalls(0, 225, 47, 23));
            maze.Add(AddWalls(0, 561, 47, 23));
            maze.Add(AddWalls(93, 90, 90, 23));
            maze.Add(AddWalls(93, 359, 90, 23));
            maze.Add(AddWalls(163, 494, 160, 23));
            maze.Add(AddWalls(304, 359, 150, 23));
            maze.Add(AddWalls(445, 158, 90, 23));
            maze.Add(AddWalls(445, 292, 90, 23));
            maze.Add(AddWalls(445, 560, 90, 23));
            maze.Add(AddWalls(513, 90, 372, 23));
            maze.Add(AddWalls(585, 425, 90, 23));
            maze.Add(AddWalls(655, 225, 90, 23));
            maze.Add(AddWalls(655, 560, 90, 23));
            maze.Add(AddWalls(793, 358, 90, 23));
            maze.Add(AddWalls(866, 225, 120, 23));
            maze.Add(AddWalls(866, 493, 120, 23));

            // All vertical walls.
            maze.Add(AddWalls(163, 0, 23, 382)); 
            maze.Add(AddWalls(93, 360, 23, 90));
            maze.Add(AddWalls(163, 494, 23, 89));
            maze.Add(AddWalls(304, 72, 23, 600));
            maze.Add(AddWalls(445, 158, 23, 420));
            maze.Add(AddWalls(513, 90, 23, 91));
            maze.Add(AddWalls(655, 225, 23, 350));
            maze.Add(AddWalls(723, 560, 23, 120));
            maze.Add(AddWalls(866, 225, 23, 156));

            mazeHitbox = AddWallCollision();
        }

        private PictureBox AddWalls(int x, int y, int w, int h)
        {
            PictureBox wall = new PictureBox
            {
                Name = "Wall",
                Parent = this,
                Location = new Point(x, y),
                Width = w,
                Height = h,
                BackColor = Color.White
            };
            this.Controls.Add(wall);

            return wall;
        }

        private List<Rectangle> AddWallCollision()
        {
            List<Rectangle> hitbox = new List<Rectangle>();

            foreach (PictureBox wall in maze)
            {
                Rectangle rectangle = new Rectangle(wall.Location.X, wall.Location.Y, wall.Width, wall.Height);
                hitbox.Add(rectangle);
            }

            return hitbox;
        }

        private void InitializePlayer()
        {
            // Initialize player movement.
            playerMovement = "";

            player = new PictureBox
            {
                Name = "Player",
                Parent = this,
                SizeMode = PictureBoxSizeMode.AutoSize,
                Location = new Point(0, 0),
                Image = Image.FromFile(Program.currentDirectory + "/Assets/Player/character_01.png"),
                BackColor = Color.Transparent
            };
            this.Controls.Add(player); // Adds the player sprite to the screen.
            //player.BringToFront(); // Brings player's PictureBox to the front.
        }
        
        private void InitializeTimer()
        {
            playerMovementTimer = new Timer();
            playerMovementTimer.Tick += new EventHandler(MovementEvents);
            playerMovementTimer.Interval = 20; // Lower interval equals faster movement.
            playerMovementTimer.Start();
        }

        private void MovementEvents(object sender, EventArgs e)
        {
            if (playerMovement != "")
            {
                Point newPlayerMovement = player.Location;

                // Key events in relation to playerMovement that will be used to reference player sprite movement in screen.
                if (playerMovement == "UP")
                {
                    newPlayerMovement.Y = player.Location.Y - playerSpeed;
                }
                else if (playerMovement == "DOWN")
                {
                    newPlayerMovement.Y = player.Location.Y + playerSpeed;
                }
                else if (playerMovement == "LEFT")
                {
                    newPlayerMovement.X = player.Location.X - playerSpeed;
                }
                else if (playerMovement == "RIGHT")
                {
                    newPlayerMovement.X = player.Location.X + playerSpeed;
                }

                // Check if player movement is detectable isnide form screen.
                if (IsPlayerInsideForm(newPlayerMovement))
                {
                    player.Location = newPlayerMovement;
                }
            }
          
            // Causes the image to be redrawn every movement.
            //this.Invalidate();
        }

        private bool IsPlayerInsideForm(Point p)
        {
            return (screen.Contains(p)) && ((p.X + player.Width) <= (screen.Width - 10)) && ((p.Y + player.Height) <= (screen.Height - player.Height + 10));
        }

        private bool IsCollision(Point p)
        {
            // Define player's new hitbox.
            Rectangle playerHitbox = new Rectangle(player.Location.X, player.Location.Y, player.Width, player.Height);

            return true;
        }

        private void InitializeEvents()
        {
            // Add event handler for keypress.
            this.KeyDown += new KeyEventHandler(KeyDownMovement);
            KeyPreview = true;

            // Add event handler so keypress is not infinite.
            this.KeyUp += new KeyEventHandler(KeyUpMovements);
        }

        private void KeyDownMovement(object sender, KeyEventArgs e)
        {
            // Enable player movement time to allow moving.
            playerMovementTimer.Enabled = true;
            playerMovement = "";

            // Key press events to be remembered.
            if (e.KeyCode == Keys.Up)
            {
                playerMovement = "UP";
            }
            if (e.KeyCode == Keys.Down)
            {
                playerMovement = "DOWN";
            }
            if (e.KeyCode == Keys.Left)
            {
                playerMovement = "LEFT";
            }
            if (e.KeyCode == Keys.Right)
            {
                playerMovement = "RIGHT";
            }
            e.Handled = true;
        }

        // Used to prevent infinite movement in the direction pressed during a key press event.
        private void KeyUpMovements(object sender, KeyEventArgs e)
        {
            // When key is released, movement stops.
            playerMovementTimer.Enabled = false;
        }

        // X button has been clicked.
        private void Level3_FormClosing(object sender, FormClosingEventArgs e)
        {
            var x = MessageBox.Show(this, "Your progress will not be saved and you will return to the main menu. Are you sure you want to exit?", "Exiting Level 3", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (x == DialogResult.Yes)
            {
                Console.WriteLine(Program.GetCurrentTime() + " - Level 3 is closed.");

                // Load main menu form when exiting this level.
                Program.getGameMenuForm().Show();
            }
        }
    }
}
