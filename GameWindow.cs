using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace HigherLowerApp
{
    public partial class GameWindow : Form
    {
        // Private data memebers
        private int guessCounter;
        private int numberToGuess;
        private Random randomizer;
        private System.Timers.Timer timer;
        private bool afterTimer;
        private bool winner;

        // Constants
        private const int MIN_NUMBER = 0;
        private const int MAX_NUMBER = 100;
        private const double DELAY = 3000.0;
        
        // Properties
        public int GuessCounter
        {
            get
            {
                return guessCounter;
            }
            set
            {
                guessCounter = value;
            }
        }

        public int NumberToGuess
        {
            get
            {
                return numberToGuess;
            }
            set
            {
                numberToGuess = value;
            }
        }

        public GameWindow()
        {
            // Make the window appear
            InitializeComponent();

            // Initialize the timer
            timer = new System.Timers.Timer(DELAY);
            timer.Elapsed += TimerElapsed;
            timer.AutoReset = false;    // Should only fire once
            timer.Stop();               // Only enable when needed

            // Turn on the afterTimer flag because we haven't used it yet
            afterTimer = true;

            // Initialize the guessCounter to 0.
            guessCounter = 0;

            // Initialize the random number generator and pick a new number.
            randomizer = new Random();
            numberToGuess = randomizer.Next(MIN_NUMBER, MAX_NUMBER);

            // The player hasn't won yet!
            winner = false;

            // Ensure the status label is clear
            labelStatus.Text = "";

            // Place focus on the text box
            textAnswer.Focus();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            // We are starting a new game.

            // Reinitialize the guessCounter
            guessCounter = 0;

            // Set up the timer
            timer.Stop();
            afterTimer = true;

            // Clear the text in the status label
            labelStatus.Text = "";

            // Reseed the random number generator
            // Remember: Next's max value will not be selected, so add one!
            randomizer = new Random();
            numberToGuess = randomizer.Next(MIN_NUMBER, MAX_NUMBER + 1);

            // Reset the winner flag
            winner = false;

            // Restore the guess button
            btnGuess.Enabled = true;

            // Put focus on the text box
            textAnswer.Focus();
        }

        private void btnGuess_Click(object sender, EventArgs e)
        {
            // The user made a guess!

            // Increment the guess counter, even if it was a bad input.
            guessCounter++;

            // Highlight the text in the text box to allow for immediate guess entry
            textAnswer.SelectAll();

            // Store the answer in a temp integer.
            int answer = 0;
            if (int.TryParse(textAnswer.Text, out answer) == false)
            {
                labelStatus.Text = "Not a number!";
                return;
            }

            // Verify the guess is within the correct range.
            if (answer < MIN_NUMBER || answer > MAX_NUMBER)
            {
                labelStatus.Text = "Guess out of range!";
                return;
            }

            // Check the input versus the number to guess.
            if (answer < numberToGuess)
            {
                labelStatus.ForeColor = Color.Red;
                labelStatus.Text = "Higher!";
            }
            else if (answer > numberToGuess)
            {
                labelStatus.ForeColor = Color.Red;
                labelStatus.Text = "Lower!";
            }
            else if (answer == numberToGuess)
            {
                labelStatus.ForeColor = Color.Green;
                labelStatus.Text = "Correct! Score: " + guessCounter;
                // Disable guess button to force a new game.
                btnGuess.Enabled = false;
                // Set the winner flag so that the label stays up
                winner = true;
            }
            else
            {
                labelStatus.Text = "BUG!!!";
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // After 3 seconds, clear the status label's text.
            labelStatus.Text = "";

            // Allow the timer to be used again if the label changes
            afterTimer = true;
        }

        private void labelStatus_TextChanged(object sender, EventArgs e)
        {
            // Only use the timer to clear if not a winner
            if (winner == false)
            {
                // Start the timer so that it clears the text 3 seconds later
                if (afterTimer == true)
                {
                    // This means that the timer has been used and needs to be used again
                    timer.Start();
                    afterTimer = false;
                }
                else
                {
                    // This means that the timer is still in use so reset the timer
                    timer.Stop();
                    timer.Start();
                }
            }            
        }

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Release the timer
            timer.Dispose();
        }
    }
}
