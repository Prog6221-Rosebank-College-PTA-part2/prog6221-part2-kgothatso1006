using System;
using System.Collections.Generic; 

namespace CybersecurityAwarenessBot
{
    public class SentimentAnalyzer
    {
        private Dictionary<string, List<string>> sentimentKeywords;

        public SentimentAnalyzer()
        {
            InitializeSentimentKeywords();
        }
        
        private void InitializeSentimentKeywords()
        {
            sentimentKeywords = new Dictionary<string, List<string>>
            {
                ["worried"] = new List<string> {
                    "worried", "nervous", "anxious", "concerned",
                    "scared", "afraid", "frightened", "panic"
                },
                ["frustrated"] = new List<string> {
                    "frustrated", "annoyed", "confused", "difficult",
                    "hard", "complicated", "angry", "upset"
                },
                ["curious"] = new List<string> {
                    "curious", "interested", "want to learn", "tell me",
                    "explain", "how to", "what is", "teach me"
                }
            };
        }

        public string DetectSentiment(string input)
        {
            string lowerInput = input.ToLower();

            foreach (var sentiment in sentimentKeywords)
            {
                foreach (var keyword in sentiment.Value)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        return sentiment.Key;
                    }
                }
            }
            return "neutral";
        }

        public string ApplySentimentAdjustment(string response, string sentiment)
        {
            switch (sentiment)
            {
                case "worried":
                    return "I understand your concern. " + response + " Remember, awareness is the first step to protection!";
                case "frustrated":
                    return "I hear your frustration. Let me simplify this for you. " + response + " Take it one step at a time!";
                case "curious":
                    return "Great question! I love your curiosity! " + response;
                default:
                    return response;
            }
        }
    }
}
