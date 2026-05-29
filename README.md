
## Class Descriptions
 
| Class | File | Responsibility |
|-------|------|----------------|
| Program | Program.cs | Entry point - starts the application |
| ChatbotForm | ChatbotForm.cs | Creates GUI, handles button clicks, typing animation |
| ChatbotEngine | ChatbotEngine.cs | Manages keyword responses, random selection, conversation flow |
| UserMemory | UserMemory.cs | Stores and recalls user's name |
| SentimentAnalyzer | SentimentAnalyzer.cs | Detects user mood, adjusts responses accordingly |

## Technologies Used

- C# .NET 8.0 - Programming language
- Windows Forms (WinForms) - GUI framework
- System.Speech - Text-to-speech functionality
- .NET Generic Collections - Dictionary and List for data organization
- Delegates (Event Handlers) - Button click and keyboard events

## Installation and Setup

### Prerequisites
- Windows operating system
- Visual Studio 2022 or later
- .NET 8.0 SDK

### Steps to Run
1. Clone the repository
2. Open the project in Visual Studio
3. Install System.Speech NuGet package (right-click project → Manage NuGet Packages → search "System.Speech" → Install)
4. Press Ctrl + Shift + B to build
5. Press F5 to run

## Usage Examples

| User Input | Bot Response |
|------------|--------------|
| "Tell me about passwords" | Provides a random password safety tip |
| "What is phishing?" | Explains phishing and how to avoid it |
| "another tip" | Gives additional tip on the current topic |
| "tell me more" | Provides more information on the same topic |
| "My name is Thabo" | Stores the name and confirms |
| "What is my name?" | Recalls and displays the stored name |
| "I'm worried about scams" | Responds empathetically then provides scam advice |
| "I'm frustrated with passwords" | Acknowledges frustration and simplifies explanation |
| "help" | Displays the help menu |
| "exit" | Ends the conversation with a goodbye message |

## Code Optimization Highlights

### Generic Collections Used
- Dictionary<string, List<string>> - Maps keywords to multiple responses for fast lookups
- List<string> - Stores random responses and conversation history
- Dictionary<string, string> - Stores user data for memory feature

### Object-Oriented Programming Principles
- Encapsulation - Private fields with public methods
- Inheritance - ChatbotForm inherits from Form class
- Composition - ChatbotForm contains instances of other classes
- Separation of Concerns - Each class has a single, clear responsibility

### Delegates Used
- Button Click events (BtnSend_Click, BtnSpeak_Click, BtnClear_Click)
- Keyboard events (TxtUserInput_KeyPress)
- Timer events (TypingTimer_Tick)

## Requirements Traceability Matrix

| Requirement | Implementation | Location |
|-------------|----------------|----------|
| GUI using WinForms | ChatbotForm inheriting from Form | ChatbotForm.cs |
| 5+ classes | Program, ChatbotForm, ChatbotEngine, UserMemory, SentimentAnalyzer | All files |
| Methods in classes | Multiple methods per class | All files |
| Generic collections | Dictionary and List used throughout | ChatbotEngine.cs, UserMemory.cs |
| Delegates | Event handlers for buttons and timer | ChatbotForm.cs |
| Keyword recognition | Dictionary mapping keywords to responses | ChatbotEngine.cs |
| Random responses | List with Random index selection | ChatbotEngine.cs |
| Conversation flow | IsFollowUpRequest and GetFollowUpResponse methods | ChatbotEngine.cs |
| Memory | UserMemory class with StoreName and RecallName | UserMemory.cs |
| Sentiment detection | SentimentAnalyzer class | SentimentAnalyzer.cs |
| Error handling | Try-catch blocks and default responses | ChatbotForm.cs, ChatbotEngine.cs |
| Voice greeting | SpeechSynthesizer in PlayVoiceGreeting method | ChatbotForm.cs |
| ASCII art | Displayed in chat area on startup | ChatbotForm.cs |

## Author

Student POE Part 2 Submission
Cybersecurity Awareness Project
May 2026
