///Author:Andrew Winward
///Date:5/18/24
///
namespace Assignment_2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            StatsBox = new GroupBox();
            totalLossesLabel = new Label();
            totalWinsLabel = new Label();
            totalRollsLabel = new Label();
            numLossesLabel = new Label();
            numWonLabel = new Label();
            numPlaysLabel = new Label();
            userInputLabel = new Label();
            RollButton = new Button();
            ResetButton = new Button();
            dicePic = new PictureBox();
            diceStats = new ListView();
            userGuessInput = new TextBox();
            errorLabel = new Label();
            StatsBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dicePic).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            StatsBox.Controls.Add(totalLossesLabel);
            StatsBox.Controls.Add(totalWinsLabel);
            StatsBox.Controls.Add(totalRollsLabel);
            StatsBox.Controls.Add(numLossesLabel);
            StatsBox.Controls.Add(numWonLabel);
            StatsBox.Controls.Add(numPlaysLabel);
            StatsBox.Location = new Point(12, 12);
            StatsBox.Name = "groupBox1";
            StatsBox.Size = new Size(332, 217);
            StatsBox.TabIndex = 0;
            StatsBox.TabStop = false;
            StatsBox.Text = "Game Info";
            // 
            // label7
            // 
            totalLossesLabel.AutoSize = true;
            totalLossesLabel.Location = new Point(237, 132);
            totalLossesLabel.Name = "label7";
            totalLossesLabel.Size = new Size(0, 25);
            totalLossesLabel.TabIndex = 5;
            // 
            // label6
            // 
            totalWinsLabel.AutoSize = true;
            totalWinsLabel.Location = new Point(237, 85);
            totalWinsLabel.Name = "label6";
            totalWinsLabel.Size = new Size(0, 25);
            totalWinsLabel.TabIndex = 4;
            // 
            // label5
            // 
            totalRollsLabel.AutoSize = true;
            totalRollsLabel.Location = new Point(237, 43);
            totalRollsLabel.Name = "label5";
            totalRollsLabel.Size = new Size(0, 25);
            totalRollsLabel.TabIndex = 3;
            // 
            // label3
            // 
            numLossesLabel.AutoSize = true;
            numLossesLabel.Location = new Point(12, 132);
            numLossesLabel.Name = "label3";
            numLossesLabel.Size = new Size(192, 25);
            numLossesLabel.TabIndex = 2;
            numLossesLabel.Text = "Number of Times Lost:";
            // 
            // label2
            // 
            numWonLabel.AutoSize = true;
            numWonLabel.Location = new Point(12, 85);
            numWonLabel.Name = "label2";
            numWonLabel.Size = new Size(196, 25);
            numWonLabel.TabIndex = 1;
            numWonLabel.Text = "Number of Times Won:";
            // 
            // label1
            // 
            numPlaysLabel.AutoSize = true;
            numPlaysLabel.Location = new Point(12, 39);
            numPlaysLabel.Name = "label1";
            numPlaysLabel.Size = new Size(211, 25);
            numPlaysLabel.TabIndex = 0;
            numPlaysLabel.Text = "Number of Times Played:";
            // 
            // label4
            // 
            userInputLabel.AutoSize = true;
            userInputLabel.Location = new Point(24, 250);
            userInputLabel.Name = "label4";
            userInputLabel.Size = new Size(148, 25);
            userInputLabel.TabIndex = 1;
            userInputLabel.Text = "Enter your guess:";
            // 
            // button1
            // 
            RollButton.Location = new Point(297, 313);
            RollButton.Name = "button1";
            RollButton.Size = new Size(112, 34);
            RollButton.TabIndex = 2;
            RollButton.Text = "Roll";
            RollButton.UseVisualStyleBackColor = true;
            RollButton.Click += buttonRoll_Click;
            // 
            // button2
            // 
            ResetButton.Location = new Point(297, 403);
            ResetButton.Name = "button2";
            ResetButton.Size = new Size(112, 34);
            ResetButton.TabIndex = 3;
            ResetButton.Text = "Reset";
            ResetButton.UseVisualStyleBackColor = true;
            ResetButton.Click += buttonReset_Click;
            // 
            // pictureBox1
            // 
            dicePic.Location = new Point(54, 313);
            dicePic.Name = "pictureBox1";
            dicePic.Size = new Size(150, 124);
            dicePic.TabIndex = 4;
            dicePic.TabStop = false;
            // 
            // listView1
            // 
            diceStats.Location = new Point(54, 544);
            diceStats.Name = "listView1";
            diceStats.Size = new Size(705, 245);
            diceStats.TabIndex = 6;
            diceStats.UseCompatibleStateImageBehavior = false;
            // 
            // textBox2
            // 
            userGuessInput.Location = new Point(178, 250);
            userGuessInput.Name = "textBox2";
            userGuessInput.Size = new Size(57, 31);
            userGuessInput.TabIndex = 8;
            userGuessInput.TextChanged += userInput_TextBox;
            // 
            // label8
            // 
            errorLabel.AutoSize = true;
            errorLabel.Location = new Point(277, 253);
            errorLabel.Name = "errorLabel";
            errorLabel.Size = new Size(0, 25);
            errorLabel.TabIndex = 9;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 823);
            Controls.Add(errorLabel);
            Controls.Add(userGuessInput);
            Controls.Add(diceStats);
            Controls.Add(dicePic);
            Controls.Add(ResetButton);
            Controls.Add(RollButton);
            Controls.Add(userInputLabel);
            Controls.Add(StatsBox);
            Name = "Form1";
            Text = "Form1";
            StatsBox.ResumeLayout(false);
            StatsBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dicePic).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private GroupBox StatsBox;
        private Label numLossesLabel;
        private Label numWonLabel;
        private Label numPlaysLabel;
        private Label userInputLabel;
        private Button RollButton;
        private Button ResetButton;
        private PictureBox dicePic;
        private ListView diceStats;
        private Label totalLossesLabel;
        private Label totalWinsLabel;
        private Label totalRollsLabel;
        private TextBox userGuessInput;
        private Label errorLabel;
    }
}
