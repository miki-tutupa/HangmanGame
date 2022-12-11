using System.ComponentModel;

namespace Hangman;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{
    #region UI Properties
    public string SpotLight
    {
        get => spotLight;
        set
        {
            spotLight = value;
            OnPropertyChanged();
        }
    }
    public List<char> Letters
    {
        get => letters; set
        {
            letters = value;
            OnPropertyChanged();
        }
    }
    public string Message
    {
        get => message; set
        {
            message = value;
            OnPropertyChanged();
        }
    }
    public string WinIcon
    {
        get => winIcon; set
        {
            winIcon = value;
            OnPropertyChanged();
        }
    }
    public string GameStatus
    {
        get => gameStatus; set
        {
            gameStatus = value;
            OnPropertyChanged();
        }
    }
    public string CurrentImage
    {
        get => currentImage; set
        {
            currentImage = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Fields
    List<string> words = new()
    {
        "andromeda",
        "antimatter",
        "asteroid",
        "astronaut",
        "astronomy",
        "blackhole",
        "comet",
        "constellation",
        "cosmos",
        "darkmatter",
        "eclipse",
        "galaxy",
        "gravity",
        "meteor",
        "meteorite",
        "moon",
        "nebula",
        "neutronstar",
        "nova",
        "planet",
        "quasar",
        "satellite",
        "solar",
        "star",
        "sun",
        "universe"
    };
    string answer;
    private string spotLight;
    List<char> guessedLetters = new();
    private List<char> letters = new();
    private string message;
    private string winIcon;
    int mistakes = 0;
    readonly int maxMistakes = 6;
    private string gameStatus;
    private string currentImage = "img0.png";
    #endregion

    public MainPage()
    {
        InitializeComponent();
        Letters.AddRange("ABCDEFGHIJKLMNÑOPQRSTUVWXYZ".ToCharArray());
        BindingContext = this;
        PickWord();
        CalculateWord(answer, guessedLetters);
    }

    #region Game Engine
    private async void PickWord()
    {
        //try
        //{
        //    HttpClient client = new();
        //    HttpResponseMessage response = await client.GetAsync("https://random-word-api.herokuapp.com/word?lang=es");

        //    if (response.IsSuccessStatusCode)
        //    {
        //        using var responseStream = await response.Content.ReadAsStreamAsync();
        //        answer = await JsonSerializer.DeserializeAsync<string>(responseStream);
        //    }
        //}
        //catch 
        //{
        //    answer = words[new Random().Next(0, words.Count)].ToUpper();
        //}
        answer = words[new Random().Next(0, words.Count)].ToUpper();

    }

    private void CalculateWord(string answer, List<char> guessedLetters)
    {
        var temp = answer.Select(x => guessedLetters.IndexOf(x) >= 0 ? x : '_').ToArray();
        SpotLight = string.Join(' ', temp);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (sender is Button btn)
        {
            var letter = btn.Text.ToUpper();
            btn.IsEnabled = false;
            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char letter)
    {
        if (guessedLetters.IndexOf(letter) == -1)
        {
            guessedLetters.Add(letter);
        }
        if (answer.Contains(letter))
        {
            CalculateWord(answer, guessedLetters);
            CheckIfGameWon();
        }
        else if (answer.IndexOf(letter) == -1)
        {
            mistakes++;
            UpdateGameStatus();
            CheckIfGameLost();
            CurrentImage = $"img{mistakes}.png";
        }
    }

    private void CheckIfGameLost()
    {
        if (mistakes >= maxMistakes)
        {
            Message = $"You lost! The word was: {answer}";
            WinIcon = "&#xE803;";
            DisableLetters();
        }
    }

    private void CheckIfGameWon()
    {
        if (SpotLight.Replace(" ", "") == answer)
        {
            Message = "You won!";
            WinIcon = "&#xE804;";
            DisableLetters();
        }
    }

    private void UpdateGameStatus()
    {
        GameStatus = $"Errors: {mistakes} of {maxMistakes}";
    }

    private void Reset_Clicked(object sender, EventArgs e)
    {
        mistakes = 0;
        guessedLetters = new();
        CurrentImage = "img0.png";
        PickWord();
        CalculateWord(answer, guessedLetters);
        Message = "";
        UpdateGameStatus();
        EnableLetters();
    }

    private void EnableLetters()
    {
        foreach (var button in LettersContainer.Children)
        {
            if (button is Button btn)
            {
                btn.IsEnabled = true;
            }
        }
    }

    private void DisableLetters()
    {
        foreach (var button in LettersContainer.Children)
        {
            if (button is Button btn)
            {
                btn.IsEnabled = false;
            }
        }
    }
    #endregion
}

