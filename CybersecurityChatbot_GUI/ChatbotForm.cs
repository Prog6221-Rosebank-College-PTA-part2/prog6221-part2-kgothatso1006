using System;
using System.Drawing;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace CybersecurityAwarenessBot
{
    public partial class ChatbotForm : Form
    {
        private ChatbotEngine chatbotEngine;
        private UserMemory userMemory;
        private SentimentAnalyzer sentimentAnalyzer;
        private SpeechSynthesizer speechSynthesizer;
        private Timer typingTimer;
        private string currentResponse;
        private int typingIndex;

        // GUI Controls
        private TextBox txtUserInput;
        private RichTextBox rtxtChatHistory;
        private Button btnSend;
        private Button btnSpeak;
        private Button btnClear;
        private Label lblStatus;
        private Panel headerPanel;
        private Panel inputPanel;
        private Label lblUserInfo;
        private ProgressBar typingProgress;

        public ChatbotForm()
        {
            InitializeComponent();
            chatbotEngine = new ChatbotEngine();
            userMemory = new UserMemory();
            sentimentAnalyzer = new SentimentAnalyzer();
            speechSynthesizer = new SpeechSynthesizer();
            InitializeTypingEffect();

            // Force display messages immediately
            DisplayWelcomeMessages();
        }

        private void InitializeComponent()
        {
            this.Text = "Cybersecurity Awareness Bot - South Africa";
            this.Size = new Size(1000, 750);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(10, 20, 30);
            this.Font = new Font("Segoe UI", 10);

            // HEADER PANEL
            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(0, 86, 179),
                Padding = new Padding(10)
            };

            Label titleLabel = new Label
            {
                Text = "CYBERSECURITY AWARENESS BOT",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            Label subtitleLabel = new Label
            {
                Text = "Educating South Africa - One Chat at a Time",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(200, 230, 255),
                Location = new Point(20, 50),
                AutoSize = true
            };

            lblUserInfo = new Label
            {
                Text = "Not logged in",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(200, 255, 200),
                Location = new Point(870, 15),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            headerPanel.Controls.AddRange(new Control[] { titleLabel, subtitleLabel, lblUserInfo });

            // CHAT HISTORY - THIS IS WHERE TEXT APPEARS
            rtxtChatHistory = new RichTextBox
            {
                Location = new Point(20, 120),
                Size = new Size(960, 480),
                ReadOnly = true,
                BackColor = Color.FromArgb(20, 30, 40),
                ForeColor = Color.FromArgb(220, 220, 220),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            // TYPING PROGRESS
            typingProgress = new ProgressBar
            {
                Location = new Point(20, 605),
                Size = new Size(960, 5),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            // INPUT PANEL
            inputPanel = new Panel
            {
                Location = new Point(20, 615),
                Size = new Size(960, 60),
                BackColor = Color.FromArgb(30, 40, 50)
            };

            txtUserInput = new TextBox
            {
                Location = new Point(10, 15),
                Size = new Size(730, 30),
                BackColor = Color.FromArgb(40, 50, 60),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11)
            };
            txtUserInput.KeyPress += TxtUserInput_KeyPress;

            btnSend = new Button
            {
                Text = "SEND",
                Location = new Point(750, 14),
                Size = new Size(70, 32),
                BackColor = Color.FromArgb(0, 86, 179),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnSend.Click += BtnSend_Click;

            btnSpeak = new Button
            {
                Text = "SPEAK",
                Location = new Point(825, 14),
                Size = new Size(70, 32),
                BackColor = Color.FromArgb(70, 70, 70),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            btnSpeak.Click += BtnSpeak_Click;

            btnClear = new Button
            {
                Text = "CLEAR",
                Location = new Point(900, 14),
                Size = new Size(60, 32),
                BackColor = Color.FromArgb(180, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 8, FontStyle.Bold)
            };
            btnClear.Click += BtnClear_Click;

            inputPanel.Controls.AddRange(new Control[] { txtUserInput, btnSend, btnSpeak, btnClear });

            // STATUS BAR
            Panel statusPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(15, 25, 35)
            };

            lblStatus = new Label
            {
                Text = "Ready | Cybersecurity Awareness Bot Online",
                ForeColor = Color.FromArgb(150, 150, 150),
                Location = new Point(10, 5),
                AutoSize = true,
                Font = new Font("Segoe UI", 8)
            };

            statusPanel.Controls.Add(lblStatus);

            this.Controls.AddRange(new Control[] { headerPanel, rtxtChatHistory, typingProgress, inputPanel, statusPanel });
            this.Resize += ChatbotForm_Resize;
        }

        private void ChatbotForm_Resize(object sender, EventArgs e)
        {
            rtxtChatHistory.Width = this.ClientSize.Width - 40;
            rtxtChatHistory.Height = this.ClientSize.Height - 220;
            typingProgress.Width = this.ClientSize.Width - 40;
            inputPanel.Width = this.ClientSize.Width - 40;
            txtUserInput.Width = inputPanel.Width - 240;
            btnSend.Location = new Point(inputPanel.Width - 210, 14);
            btnSpeak.Location = new Point(inputPanel.Width - 135, 14);
            btnClear.Location = new Point(inputPanel.Width - 65, 14);
            lblUserInfo.Location = new Point(this.ClientSize.Width - 180, 15);
        }

        private void InitializeTypingEffect()
        {
            typingTimer = new Timer();
            typingTimer.Interval = 25;
            typingTimer.Tick += TypingTimer_Tick;
        }

        private void TypingTimer_Tick(object sender, EventArgs e)
        {
            if (typingIndex < currentResponse.Length)
            {
                rtxtChatHistory.AppendText(currentResponse[typingIndex].ToString());
                typingIndex++;
                rtxtChatHistory.ScrollToCaret();
            }
            else
            {
                typingTimer.Stop();
                typingProgress.Visible = false;
                rtxtChatHistory.AppendText("\n\n");
                lblStatus.Text = "Ready | Cybersecurity Awareness Bot Online";
                EnableInput(true);
            }
        }

        private void DisplayWelcomeMessages()
        {
            // Directly add text without typing effect for initial messages
            rtxtChatHistory.AppendText(@"
  ╔════════════════════════════════════════════════════════════════════════════╗
  ║                    CYBERSECURITY AWARENESS BOT                             ║
  ║                   Educating South Africa - One Chat at a Time              ║
  ╚════════════════════════════════════════════════════════════════════════════╝

");

            rtxtChatHistory.AppendText(@"
    ╔════════════════════════════════════════════════════════════════╗
    ║                    WELCOME TO THE CHATBOT!                     ║
    ╚════════════════════════════════════════════════════════════════╝

");

            rtxtChatHistory.AppendText("Bot: I'm your Cybersecurity Awareness Assistant.\n");
            rtxtChatHistory.AppendText("Bot: I'm here to help you stay safe online, especially with the growing cyber threats in South Africa.\n\n");

            rtxtChatHistory.AppendText(@"
  ┌─────────────────────────────────────────────────────────────────┐
  │  I can help you with:                                           │
  │  - Password safety and best practices                           │
  │  - Identifying phishing attempts and scams                      │
  │  - Safe browsing habits                                         │
  │  - South Africa specific cyber threats                          │
  │  Try asking: ""Tell me about passwords"", ""What is phishing?"" │
  │  Type 'exit' to quit.                                           │
  └─────────────────────────────────────────────────────────────────┘

 ");

            rtxtChatHistory.AppendText("Bot: May I have your name?\n\n");
            rtxtChatHistory.ScrollToCaret();

            // Play voice greeting
            PlayVoiceGreeting();
        }

        private async void PlayVoiceGreeting()
        {
            try
            {
                string greeting = "Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online in South Africa.";
                await Task.Run(() =>
                {
                    using (var synthesizer = new SpeechSynthesizer())
                    {
                        synthesizer.Volume = 100;
                        synthesizer.Rate = 1;
                        synthesizer.Speak(greeting);
                    }
                });
            }
            catch (Exception ex)
            {
                // Voice failed, but text still works
                Console.WriteLine("Voice error: " + ex.Message);
            }
        }

        private void AppendBotMessage(string message, bool withTyping = true)
        {
            if (withTyping)
            {
                rtxtChatHistory.SelectionColor = Color.FromArgb(100, 200, 255);
                rtxtChatHistory.AppendText("Bot: ");
                rtxtChatHistory.SelectionColor = Color.White;
                currentResponse = message;
                typingIndex = 0;
                typingTimer.Start();
                typingProgress.Visible = true;
                EnableInput(false);
                lblStatus.Text = "Bot is typing...";
            }
            else
            {
                rtxtChatHistory.SelectionColor = Color.FromArgb(100, 200, 255);
                rtxtChatHistory.AppendText("Bot: ");
                rtxtChatHistory.SelectionColor = Color.White;
                rtxtChatHistory.AppendText(message + "\n\n");
                rtxtChatHistory.ScrollToCaret();
            }
        }

        private void AppendSystemMessage(string message)
        {
            rtxtChatHistory.SelectionColor = Color.FromArgb(255, 200, 100);
            rtxtChatHistory.AppendText("Info: " + message + "\n");
            rtxtChatHistory.SelectionColor = Color.White;
            rtxtChatHistory.ScrollToCaret();
        }

        private void AppendUserMessage(string message)
        {
            rtxtChatHistory.SelectionColor = Color.FromArgb(100, 255, 150);
            rtxtChatHistory.AppendText("You: ");
            rtxtChatHistory.SelectionColor = Color.White;
            rtxtChatHistory.AppendText(message + "\n\n");
            rtxtChatHistory.ScrollToCaret();
        }

        private void EnableInput(bool enabled)
        {
            txtUserInput.Enabled = enabled;
            btnSend.Enabled = enabled;
            btnSpeak.Enabled = enabled;
            if (enabled) txtUserInput.Focus();
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            SendUserInput();
        }

        private void TxtUserInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                SendUserInput();
                e.Handled = true;
            }
        }

        private void SendUserInput()
        {
            string userInput = txtUserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(userInput)) return;

            AppendUserMessage(userInput);
            string sentiment = sentimentAnalyzer.DetectSentiment(userInput);

            if (userMemory.IsNameStatement(userInput))
            {
                string name = userMemory.ExtractName(userInput);
                userMemory.StoreName(name);
                string response = $"Nice to meet you, {name}! I'll remember that. How can I help you?";
                response = sentimentAnalyzer.ApplySentimentAdjustment(response, sentiment);
                AppendBotMessage(response);
                if (userMemory.GetName() != null) lblUserInfo.Text = "User: " + userMemory.GetName();
            }
            else if (userMemory.IsNameRecallRequest(userInput))
            {
                string response = userMemory.RecallName();
                response = sentimentAnalyzer.ApplySentimentAdjustment(response, sentiment);
                AppendBotMessage(response);
            }
            else if (chatbotEngine.IsFollowUpRequest(userInput))
            {
                string currentTopic = chatbotEngine.GetCurrentTopic();
                string response = chatbotEngine.GetFollowUpResponse(currentTopic);
                response = sentimentAnalyzer.ApplySentimentAdjustment(response, sentiment);
                AppendBotMessage(response);
            }
            else
            {
                string response = chatbotEngine.GetResponse(userInput, sentiment);
                string detectedTopic = chatbotEngine.GetDetectedTopic(userInput);
                if (!string.IsNullOrEmpty(detectedTopic)) chatbotEngine.SetCurrentTopic(detectedTopic);
                AppendBotMessage(response);
            }

            txtUserInput.Clear();

            if (userInput.ToLower() == "exit" || userInput.ToLower() == "quit" || userInput.ToLower() == "bye")
            {
                ShowGoodbyeMessage();
            }
        }

        private void BtnSpeak_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUserInput.Text))
            {
                try
                {
                    using (var synthesizer = new SpeechSynthesizer())
                    {
                        synthesizer.Volume = 100;
                        synthesizer.Rate = 1;
                        synthesizer.SpeakAsync(txtUserInput.Text);
                    }
                }
                catch (Exception ex)
                {
                    AppendSystemMessage("Cannot speak: " + ex.Message);
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            rtxtChatHistory.Clear();
            DisplayWelcomeMessages();
            userMemory.Reset();
            chatbotEngine.Reset();
            AppendSystemMessage("Chat history cleared and memory reset!");
        }

        private void ShowGoodbyeMessage()
        {
            string name = userMemory.GetName() ?? "friend";
            string goodbye = $@"
Thank you for learning about cybersecurity, {name}! Stay safe!
Remember: Think before you click!
";
            AppendBotMessage(goodbye, false);
            EnableInput(false);
            btnSend.Enabled = false;
            btnClear.Enabled = false;
            lblStatus.Text = "Session ended. Close window to exit.";
        }
    }
}