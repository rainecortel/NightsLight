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
            this.Height = 600;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;

            this.BackColor = Color.FromArgb(65, 65, 65);
            this.BackgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Level3_Map_01.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Prevent flickering of the background image.
            this.DoubleBuffered = true;

            this.screen = new Rectangle(new Point(0, 0), this.Size);

            InitializePlayer();
            InitilizeTimer();
            InitilizeEvents();

            // Add event to catch form closing.
            this.FormClosing += Level3_FormClosing;
        }

        private void InitializePlayer()
        {
            // Initialize player movement.
            playerMovement = "";

            player = new PictureBox
            {
                Name = "Player",
                SizeMode = PictureBoxSizeMode.AutoSize,
                Location = new Point(0, 0),
                Image = Image.FromFile(Program.currentDirectory + "/Assets/Player/character_01.png"),
                BackColor = Color.Transparent
            };
            // Adds the player sprite to the screen.
            this.Controls.Add(player);
        }
        
        private void InitilizeTimer()
        {
            playerMovementTimer = new Timer();
            playerMovementTimer.Tick += new EventHandler(MovementEvents);
            playerMovementTimer.Interval = 20; // Lower interval equals faster movement.
            playerMovementTimer.Start();
        }

        private void MovementEvents(object sender, EventArgs e)
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

            if (playerMovement != "")
            {
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

        private void InitilizeEvents()
        {
            this.KeyDown += new KeyEventHandler(KeyDownMovement);
            KeyPreview = true;

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
