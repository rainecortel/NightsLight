using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace NightsLight
{
    // Instantiate a new instance of the form for every call of MainPage.
    internal class GameMenu : Form
    {
        Button btPlay = new Button();
        Button btLeaderboards = new Button();
        Button btExit = new Button();
        Image backgroundImage = Image.FromFile(Program.currentDirectory + "/Assets/Images/Game Project BG.png"); // Get the base directory of where the file is executed to retrieve contents of Assets folder.

        // Constructor. Defines the initial property of the class.
        public GameMenu()
        {
            // Used for game logs.
            OnApplicationStart();

            this.Text = "Night's Light";
            this.Width = 1000;
            this.Height = 600;
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.BackgroundImage = backgroundImage;

            // Set the background color to a fantasy-style background color.
            this.BackColor = Color.FromArgb(33, 32, 59);

            // Set opacity of white for hover in button.
            Color transparentWhite = Color.FromArgb(75, Color.White);

            // Define the properties of buttons.
            btPlay.Text = "Play";
            btPlay.Font = new Font("Arial", 13, FontStyle.Italic);
            btPlay.Width = 200;
            btPlay.Height = 50;
            btPlay.Top = 175;
            btPlay.Left = (500 - btPlay.Width) / 2;
            btPlay.FlatStyle = FlatStyle.Flat; // Set button style to flat.
            btPlay.FlatAppearance.MouseOverBackColor = transparentWhite; // Set opacity of white for hover in button.
            btPlay.ForeColor = Color.Transparent; // Set to transparent to allow hover color.
            btPlay.BackColor = Color.Transparent; // Set to transparent to allow hover color.
            btPlay.FlatAppearance.BorderSize = 0; // Remove button border.
            this.Controls.Add(btPlay);

            btLeaderboards.Text = "Leaderboards";
            btLeaderboards.Font = new Font("Arial", 13, FontStyle.Italic);
            btLeaderboards.Width = 200;
            btLeaderboards.Height = 50;
            btLeaderboards.Top = btPlay.Bottom + 20;
            btLeaderboards.Left = (500 - btLeaderboards.Width) / 2;
            btLeaderboards.FlatStyle = FlatStyle.Flat;
            btLeaderboards.FlatAppearance.MouseOverBackColor = transparentWhite;
            btLeaderboards.ForeColor = Color.Transparent;
            btLeaderboards.BackColor = Color.Transparent;
            btLeaderboards.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btLeaderboards);

            btExit.Text = "Exit";
            btExit.Font = new Font("Arial", 13, FontStyle.Italic);
            btExit.Width = 200;
            btExit.Height = 50;
            btExit.Top = btLeaderboards.Bottom + 20;
            btExit.Left = (500 - btExit.Width) / 2;
            btExit.FlatStyle = FlatStyle.Flat;
            btExit.FlatAppearance.MouseOverBackColor = transparentWhite;
            btExit.ForeColor = Color.Transparent;
            btExit.BackColor = Color.Transparent;
            btExit.FlatAppearance.BorderSize = 0;
            this.Controls.Add(btExit);

            btPlay.Click += new EventHandler(btPlay_Click);
            btLeaderboards.Click += new EventHandler(btLeaderboards_Click);
            btExit.Click += new EventHandler(btExit_Click);
        }

        private void OnApplicationStart()
        {
            Console.WriteLine(Program.GetCurrentTime() + " - Game application is started.");
        }

        public void OnApplicationExit(object sender, EventArgs e)
        {
            Console.WriteLine(Program.GetCurrentTime() + " - Game application is terminated.");
        }

        private void btPlay_Click(object sender, EventArgs e)
        {
            // When the "Play" button is clicked, create and show the Level1 form.
            Level1 level1Form = new Level1();
            level1Form.Show();

            // Hide game menu when loading levels.
            this.Hide();
        }

        private void btLeaderboards_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Viewing leaderboards...");
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
