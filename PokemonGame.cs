using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PokemonGame.Panels;

namespace PokemonGame
{
    public partial class PokemonGame : Form
    {
        private MainMenuPanel mainMenuPanel;
        private CharacterCreationPanel characterCreationPanel;
        private StoryTextPanel storyTextPanel;
        private SavedGamesPanel savedGamesPanel;
        private OptionsPanel optionsPanel;
        private LoadingScreen loadingScreen;
        private GamePanel gamePanel;

        public PokemonGame()
        {
            InitializeComponent();
            InitializeLoadingScreen();
            InitializeUI();
            InitializeApplication();
        }

        private void InitializeLoadingScreen()
        {
            loadingScreen = new LoadingScreen();
            loadingScreen.Dock = DockStyle.Fill;
            this.Controls.Add(loadingScreen);
            loadingScreen.StartLoading();
        }

        private void InitializeUI()
        {

            mainMenuInitialize();
            characterCreationInitialize();
            storyInitialize();
            savedGamesInitialize();
            optionsInitialize();
            
        }

        private void mainMenuInitialize()
        {
            // Initialize main menu panel
            mainMenuPanel = new MainMenuPanel();
            mainMenuPanel.Dock = DockStyle.Fill;
            mainMenuPanel.StartClicked += StartButton_Click;
            mainMenuPanel.LoadClicked += LoadButton_Click;
            mainMenuPanel.ExitClicked += ExitButton_Click;
            mainMenuPanel.OptionsClicked += OptionsButton_Click;

            // Set background image for main menu panel
            mainMenuPanel.BackgroundImage = Image.FromFile("Assets/background.jpg"); // Load image from file
            mainMenuPanel.BackgroundImageLayout = ImageLayout.Stretch;
            mainMenuPanel.BackColor = Color.Transparent; // Ensure transparency
            mainMenuPanel.Visible = false;
            this.Controls.Add(mainMenuPanel);
        }

        private void characterCreationInitialize()
        {
            characterCreationPanel = new CharacterCreationPanel();
            characterCreationPanel.NameConfirmed += CharacterCreationPanel_NameConfirmed;
            this.Controls.Add(characterCreationPanel);

            characterCreationPanel.Visible = false;
        }

        private void storyInitialize()
        {
            storyTextPanel = new StoryTextPanel();
            storyTextPanel.StoryCompleted += StoryTextPanel_StoryCompleted;
            this.Controls.Add(storyTextPanel);
            storyTextPanel.Visible = false;
        }

        private void savedGamesInitialize()
        {
            // Initialize saved games panel
            savedGamesPanel = new SavedGamesPanel();
            savedGamesPanel.Dock = DockStyle.Fill;
            savedGamesPanel.Visible = false;
            savedGamesPanel.BackClicked += BackButton_Click;

            // Set background image for saved games panel
            savedGamesPanel.BackgroundImage = Image.FromFile("Assets/background.jpg"); // Load image from file
            savedGamesPanel.BackgroundImageLayout = ImageLayout.Stretch;
            savedGamesPanel.BackColor = Color.Transparent; // Ensure transparency

            this.Controls.Add(savedGamesPanel);
        }

        private void optionsInitialize()
        {
            // Initialize saved games panel
            optionsPanel = new OptionsPanel();
            optionsPanel.Dock = DockStyle.Fill;
            optionsPanel.Visible = false;
            optionsPanel.BackClicked += BackButton_Click;

            // Set background image for options panel
            optionsPanel.BackgroundImage = Image.FromFile("Assets/background.jpg"); // Load image from file
            optionsPanel.BackgroundImageLayout = ImageLayout.Stretch;
            optionsPanel.BackColor = Color.Transparent; // Ensure transparency

            this.Controls.Add(optionsPanel);
        }

        private void gameInitialize(GameDetails details)
        {
            gamePanel = new GamePanel(details);
            this.Controls.Add(gamePanel);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            mainMenuPanel.Visible = false;
            savedGamesPanel.Visible = false;
            optionsPanel.Visible = false;
            characterCreationPanel.Visible = true;
            storyTextPanel.Visible = false;
            // Add your game initialization logic here
            // For example, creating player, generating Pokémon, etc.
        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            // Show the saved games panel
            mainMenuPanel.Visible = false;
            savedGamesPanel.Visible = true;
            optionsPanel.Visible = false;
            characterCreationPanel.Visible = false;
            storyTextPanel.Visible = false;
            // Simulate loading saved games
            // Replace this with actual logic to load saved games
            // MessageBox.Show("Loading saved games...");
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            // Show the main menu panel
            savedGamesPanel.Visible = false;
            mainMenuPanel.Visible = true;
            optionsPanel.Visible = false;
            characterCreationPanel.Visible = false;
            storyTextPanel.Visible = false;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            // Show the main menu panel
            savedGamesPanel.Visible = false;
            mainMenuPanel.Visible = false;
            optionsPanel.Visible = true;
            characterCreationPanel.Visible = false;
            storyTextPanel.Visible = false;
        }

        private void CharacterCreationPanel_NameConfirmed(object sender, string playerName)
        {
            characterCreationPanel.Visible = false;
            StartStory(playerName);
        }

        private void StartStory(string playerName)
        {
            storyTextPanel.Visible = true;
            storyTextPanel.Focus();

            // Define the story parts
            var storyParts = new List<(string text, Image image)>
            {
                ("Welcome, " + playerName + "!", Image.FromFile("Assets/player.png")),
                ("This is the beginning of your adventure...", Image.FromFile("Assets/pokeball.png")),
                ("You will face many challenges and meet many Pokémon.", Image.FromFile("Assets/PokedexEmby.png")),
                ("Prepare yourself for an unforgettable journey!", Image.FromFile("Assets/player.png"))
            };

            storyTextPanel.StartStory(storyParts);
        }

        private void StoryTextPanel_StoryCompleted(object sender, EventArgs e)
        {
            storyTextPanel.Visible = false;
            //set default start when new character
            var details = new GameDetails(new Point(1230,700), "PelletTown", true, 70, 40);
            gameInitialize(details);
            LoadGame();
        }

        private async void LoadGame()
        {
            loadingScreen.Visible = true;
            loadingScreen.StartLoading();
            await Task.Delay(2000); // Simulate initialization delay
            loadingScreen.StopLoading();
            loadingScreen.Visible = false;
            gamePanel.Visible = true;
            gamePanel.Focus();
            this.KeyDown += Form1_KeyDown;
            this.KeyUp += Form1_KeyUp;
            // Load the game screen or initialize game logic here
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            gamePanel.HandleKeyDown(e); // Pass the key event to GamePanel
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            gamePanel.HandleKeyUp(e); // Pass the key event to GamePanel
        }

        private void PokemonGame_Load(object sender, EventArgs e)
        {
            // Form Load event handler
            // You can put any initialization logic for the form here
        }

        private async void InitializeApplication()
        {
            this.KeyPreview = true; // Ensures that the form receives key events before the controls
            // Show loading screen
            loadingScreen.Visible = true;

            // Perform initialization tasks (e.g., loading resources)
            await Task.Delay(2000); // Simulate initialization delay
            loadingScreen.StopLoading();
            // Hide loading screen and show main menu
            loadingScreen.Visible = false;
            mainMenuPanel.Visible = true;
            characterCreationPanel.Visible = false;
            storyTextPanel.Visible = false;
        }

    }
}
