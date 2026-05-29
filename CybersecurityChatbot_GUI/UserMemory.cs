using System;
using System.Collections.Generic;

namespace CybersecurityAwarenessBot
{ 
    public class UserMemory
    {
        private Dictionary<string, string> userData;
        private string userName;

        public UserMemory()
        {
            userData = new Dictionary<string, string>();
            userName = null;
        }

        public bool IsNameStatement(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("my name is") ||
                   lowerInput.Contains("i am ") ||
                   lowerInput.Contains("i'm ");
        }

        public string ExtractName(string input)
        {
            string extractedName = "";

            if (input.ToLower().Contains("my name is"))
            {
                extractedName = input.Split(new[] { "my name is" }, StringSplitOptions.None)[1].Trim();
            }
            else if (input.ToLower().Contains("i am "))
            {
                extractedName = input.Split(new[] { "i am " }, StringSplitOptions.None)[1].Trim();
            }
            else if (input.ToLower().Contains("i'm "))
            {
                extractedName = input.Split(new[] { "i'm " }, StringSplitOptions.None)[1].Trim();
            }

            if (!string.IsNullOrWhiteSpace(extractedName))
            {
                string[] nameParts = extractedName.Split(' ');
                return char.ToUpper(nameParts[0][0]) + nameParts[0].Substring(1).ToLower();
            }

            return null;
        }

        public void StoreName(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                userName = name;
                userData["name"] = name;
            }
        }

        public string GetName()
        {
            return userName;
        }

        public bool IsNameRecallRequest(string input)
        {
            string lowerInput = input.ToLower();
            return lowerInput.Contains("what is my name") ||
                   lowerInput.Contains("remember me") ||
                   lowerInput.Contains("do you remember");
        }

        public string RecallName()
        {
            if (!string.IsNullOrEmpty(userName))
            {
                return $"Of course! Your name is {userName}. How can I help you stay safe online today?";
            }
            return "I don't think you've told me your name yet. What's your name?";
        }

        public void Reset()
        {
            userData.Clear();
            userName = null;
        }
    }
}
