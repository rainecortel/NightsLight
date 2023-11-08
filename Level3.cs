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
        private PictureBox player;
        private Timer playerMovementTimer;
        private bool _moveUp, _moveDown, _moveLeft, _moveRight;
        private string playerMovement;
        private int playerSpeed = 5;

        public Level3()
        {
            Console.WriteLine(Program.GetCurrentTime() + " - Level 3 is loaded.");

            this.Text = "Night's Light (Level 3)";
            this.Width = 1000;
            this.Height = 600;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;

            this.BackColor = Color.FromArgb(65, 65, 65);

            InitializePlayer();
            InitilizeTimer();
            InitilizeEvents();

            // Add event to catch form closing.
            this.FormClosing += Level3_FormClosing;
        }

        private void InitializePlayer()
        {
            player = new PictureBox
            {
                Name = "Player",
                SizeMode = PictureBoxSizeMode.AutoSize,
                Location = new Point(0, 0),
                Image = Image.FromFile(Program.currentDirectory + "/Assets/Player/character_01.png")
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
            // Key events in relation to playerMovement that will be used to reference player sprite movement in screen.
            if (playerMovement == "UP")
            {
                player.Top -= playerSpeed;
            }
            if (playerMovement == "DOWN")
            {
                player.Top += playerSpeed;
            }
            if (playerMovement == "LEFT")
            {
                player.Left -= playerSpeed;
            }
            if (playerMovement == "RIGHT")
            {
                player.Left += playerSpeed;
            }
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

            // Key press events to be remembered.
            switch (e.KeyCode)
            {
                case Keys.Up:
                    _moveUp = true;
                    break;
                case Keys.Down:
                    _moveDown = true;
                    break;
                case Keys.Left:
                    _moveLeft = true;
                    break;
                case Keys.Right:
                    _moveRight = true;
                    break;
            }

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
