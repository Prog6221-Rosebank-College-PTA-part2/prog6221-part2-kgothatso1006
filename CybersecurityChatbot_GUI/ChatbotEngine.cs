using System;
using System.Collections.Generic;
using System.Linq;

namespace CybersecurityAwarenessBot 
{
    public class ChatbotEngine
    {
        private Dictionary<string, List<string>> keywordResponses; 
        private Dictionary<string, List<string>> randomResponseSets;
        private string currentTopic;
        private Random random;
        private List<string> conversationHistory;

        public ChatbotEngine()
        {
            random = new Random();
            conversationHistory = new List<string>();
            InitializeResponses();
        }

        private void InitializeResponses()
        {
            keywordResponses = new Dictionary<string, List<string>>
            {
                ["password"] = new List<string>
                {
                    "Use a strong passphrase with at least 12 characters, mixing uppercase, lowercase, numbers, and symbols!",
                    "Never reuse passwords across different accounts. If one gets compromised, others are at risk!",
                    "Consider using a password manager to generate and store complex passwords securely.",
                    "Enable Two-Factor Authentication (2FA) whenever possible for an extra layer of security.",
                    "Avoid using personal information like birthdates or pet names in your passwords."
                },
                ["phish"] = new List<string>
                {
                    "Phishing emails often create urgency. Always check the sender's email address carefully!",
                    "Never click on links in unsolicited emails. Hover over links to see the actual URL first.",
                    "Legitimate companies will never ask for your password or OTP via email or SMS.",
                    "Look for spelling mistakes and generic greetings like 'Dear Customer' - these are red flags!",
                    "If you receive a suspicious message claiming to be from your bank, call them directly using their official number."
                },
                ["scam"] = new List<string>
                {
                    "South Africa has seen a 300% increase in scams since 2020. Be vigilant!",
                    "Smishing (SMS phishing) is common in SA - never click links in SMS messages from unknown numbers.",
                    "'Millionaire' or lottery scams promise big money but ask for fees upfront - these are always fake.",
                    "Beware of fake rental listings on Facebook Marketplace and Gumtree.",
                    "No legitimate company will ask you to pay via gift cards or Bitcoin - that's a scam!"
                },
                ["brows"] = new List<string>
                {
                    "Look for 'https://' and the padlock icon in the address bar before entering sensitive information.",
                    "Be cautious of shortened URLs (like bit.ly) - they can hide malicious destinations.",
                    "Keep your browser and antivirus software updated to protect against the latest threats.",
                    "Avoid using public Wi-Fi for banking or shopping. If you must, use a VPN.",
                    "Clear your browsing data regularly and be mindful of what information you share online."
                },
                ["south africa"] = new List<string>
                {
                    "South Africa has seen a 300 percent increase in cyberattacks since 2020.",
                    "South African banks will NEVER call you asking for your OTP or password.",
                    "Vishing (voice phishing) is common in SA - scammers pretend to be from your bank."
                }
            };

            randomResponseSets = new Dictionary<string, List<string>>
            {
                ["greeting"] = new List<string>
                {
                    "Hello there! How can I help you stay safe online today?",
                    "Hi! Ready to learn about cybersecurity in South Africa?",
                    "Greetings! I'm here to help protect you from online threats."
                },
                ["thanks"] = new List<string>
                {
                    "You're welcome! Stay safe online!",
                    "Happy to help! Remember: Think before you click!",
                    "Anytime! Knowledge is power against cyber threats!"
                },
                ["default"] = new List<string>
                {
                    "I didn't quite understand that. Could you rephrase? You can ask me about password safety, phishing, scams, safe browsing, or type 'help' for options.",
                    "Hmm, I'm not sure about that. Try asking about passwords, phishing, scams in South Africa, or safe browsing!"
                },
                ["encouragement"] = new List<string>
                {
                    "Great question! Keep learning - cybersecurity awareness saves lives!",
                    "Excellent curiosity! Every question makes you more cyber-safe!"
                }
            };
        }

        public string GetResponse(string userInput, string sentiment)
        {
            string lowerInput = userInput.ToLower().Trim();

            conversationHistory.Add(lowerInput);
            if (conversationHistory.Count > 10) conversationHistory.RemoveAt(0);

            if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "bye")
            {
                return "Thank you for learning about cybersecurity! Stay safe!";
            }

            if (lowerInput.Contains("help"))
            {
                return GetHelpMessage();
            }

            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                return randomResponseSets["greeting"][random.Next(randomResponseSets["greeting"].Count)];
            }

            if (lowerInput.Contains("thank") || lowerInput.Contains("thanks"))
            {
                return randomResponseSets["thanks"][random.Next(randomResponseSets["thanks"].Count)];
            }

            if (lowerInput.Contains("how are you"))
            {
                return "I'm functioning well and ready to help you learn about cybersecurity!";
            }

            string keywordResponse = GetKeywordResponse(lowerInput);
            if (keywordResponse != null)
            {
                return keywordResponse;
            }

            if (lowerInput.Contains("tip") || lowerInput.Contains("advice") || lowerInput.Contains("suggestion"))
            {
                var allTopics = keywordResponses.Keys.ToList();
                string randomTopic = allTopics[random.Next(allTopics.Count)];
                var responses = keywordResponses[randomTopic];
                currentTopic = randomTopic;
                return $"Here's a {randomTopic} tip: {responses[random.Next(responses.Count)]}";
            }

            return randomResponseSets["default"][random.Next(randomResponseSets["default"].Count)];
        }

        public string GetKeywordResponse(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;
                    var responses = keywordResponses[keyword];
                    return responses[random.Next(responses.Count)];
                }
            }
            return null;
        }

        public string GetDetectedTopic(string input)
        {
            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    return keyword;
                }
            }
            return null;
        }

        public bool IsFollowUpRequest(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("another tip") ||
                   lowerInput.Contains("tell me more") ||
                   lowerInput.Contains("explain more") ||
                   lowerInput.Contains("give me another") ||
                   lowerInput.Contains("more information");
        }

        public string GetFollowUpResponse(string currentTopic)
        {
            if (!string.IsNullOrEmpty(currentTopic) && keywordResponses.ContainsKey(currentTopic))
            {
                var responses = keywordResponses[currentTopic];
                return $"Here's another tip about {currentTopic}: {responses[random.Next(responses.Count)]}";
            }
            else if (conversationHistory.Count > 0)
            {
                foreach (var keyword in keywordResponses.Keys)
                {
                    if (conversationHistory.LastOrDefault()?.Contains(keyword) == true)
                    {
                        currentTopic = keyword;
                        var responses = keywordResponses[keyword];
                        return $"Let me share more about {keyword}: {responses[random.Next(responses.Count)]}";
                    }
                }
            }
            return randomResponseSets["encouragement"][random.Next(randomResponseSets["encouragement"].Count)];
        }

        public string GetCurrentTopic()
        {
            return currentTopic;
        }

        public void SetCurrentTopic(string topic)
        {
            currentTopic = topic;
        }

        private string GetHelpMessage()
        {
            return @"Here's what I can help you with:

PASSWORD SAFETY - Ask me about passwords or passphrases
PHISHING & SCAMS - Ask about phishing, scams, or suspicious emails
SOUTH AFRICA - Ask about cyber threats specific to SA
SAFE BROWSING - Ask about browsing safely or suspicious links

Type 'exit' when you're done. Stay safe online!";
        }

        public void Reset()
        {
            currentTopic = null;
            conversationHistory.Clear();
        }
    }
}
