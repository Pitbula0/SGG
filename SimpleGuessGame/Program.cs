using Spectre.Console;

var difficulty = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("What difficulty do you want to play?")
                .HighlightStyle("bold cyan")
                .AddChoices("[bold green]Easy[/]", "[bold yellow]Medium[/]", "[bold red]Hard[/]"));

HashSet<string> wordsE = new HashSet<string> { "cat", "dog", "bat", "rat", "mat" };
HashSet<string> wordsM = new HashSet<string> { "apple", "grape", "peach", "plum", "berry" };
HashSet<string> wordsH = new HashSet<string> { "banana", "cherry", "orange", "kiwi", "mango" };

var random = new Random();

var chosenWord = difficulty switch
{
    "[bold green]Easy[/]" => wordsE.ElementAt(random.Next(wordsE.Count)),
    "[bold yellow]Medium[/]" => wordsM.ElementAt(random.Next(wordsM.Count)),
    "[bold red]Hard[/]" => wordsH.ElementAt(random.Next(wordsH.Count)),
    _ => throw new ArgumentException("Invalid difficulty level")
};

var wordLength = chosenWord.Length;
var attempts = 0;
var guessedLetters = new HashSet<char>();
var correctGuesses = new HashSet<char>();
var incorrectGuesses = new HashSet<char>();

var wordDisplay = new string('_', wordLength).ToCharArray();
var gameOver = false;
var maxAttempts = 10;

while (!gameOver)
{
    AnsiConsole.MarkupLine($"\nWord: [bold]{new string(wordDisplay)}[/]");
    AnsiConsole.MarkupLine($"Guessed Letters: [yellow]{string.Join(", ", guessedLetters)}[/]");
    AnsiConsole.MarkupLine($"Incorrect Guesses: [red]{string.Join(", ", incorrectGuesses)}[/]");
    AnsiConsole.MarkupLine($"Attempts Left: [bold cyan]{maxAttempts - attempts}[/]");

    var input = AnsiConsole.Ask<string>("Guess a letter:").ToLower();

    if (string.IsNullOrWhiteSpace(input) || input.Length != 1 || !char.IsLetter(input[0]))
    {
        AnsiConsole.MarkupLine("[red]Please enter a single valid letter.[/]");
        continue;
    }

    char guess = input[0];

    if (guessedLetters.Contains(guess))
    {
        AnsiConsole.MarkupLine("[grey]You already guessed that letter.[/]");
        continue;
    }

    guessedLetters.Add(guess);

    if (chosenWord.Contains(guess))
    {
        AnsiConsole.MarkupLine("[green]Correct![/]");
        correctGuesses.Add(guess);
        for (int i = 0; i < chosenWord.Length; i++)
        {
            if (chosenWord[i] == guess)
            {
                wordDisplay[i] = guess;
            }
        }
    }
    else
    {
        AnsiConsole.MarkupLine("[red]Wrong![/]");
        incorrectGuesses.Add(guess);
        attempts++;
    }

    if (!wordDisplay.Contains('_'))
    {
        AnsiConsole.MarkupLine($"\n[bold green]Congratulations! You guessed the word: {chosenWord}[/]");
        gameOver = true;
    }
    else if (attempts >= maxAttempts)
    {
        AnsiConsole.MarkupLine($"\n[bold red]Game Over! The word was: {chosenWord}[/]");
        gameOver = true;
    }
}