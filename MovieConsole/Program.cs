
// See https://aka.ms/new-console-template for more information
using MovieDTO;
using MovieDomain;
using MovieApiHandler;
using MovieConsole;
List<string> consoleInputs = new List<string>();

Console.WindowWidth = 130;
Console.WindowHeight= 50;
MovieInteractor movieInteractor = new MovieInteractor();
UserInterface ui = new();

ui.SetConsoleColor();

//AddAllRockyAndKarateKidTitlesToDbPlusBloodsportBecauseSomehowThereAreNotFifteenRockyAndKarateKidMoviesCombined();
static void AddAllRockyAndKarateKidTitlesToDbPlusBloodsportBecauseSomehowThereAreNotFifteenRockyAndKarateKidMoviesCombined()
{
    MovieApi apiHandler = new MovieApi();
    MovieInteractor movieInteractor = new MovieInteractor();
    string[] videoFilmTitles = new string[]
    {
    "Rocky",
    "Rocky II",
    "Rocky III",
    "Rocky IV",
    "Rocky V",
    "Rocky Balboa",
    "Creed",
    "Creed II",
    "Creed III",
    "The Karate Kid",
    "The Karate Kid Part II",
    "The Karate Kid Part III",
    "The Next Karate Kid",
    //"The Karate Kid 2", not found in api :(
    "Bloodsport",
    };
    foreach (string title in videoFilmTitles)
    {
        if (movieInteractor.GetMovieByTitle(title) == null || movieInteractor.GetMovieByTitle(title).Count == 0)
        {
            Movie movieToAdd = apiHandler
            .Orchestrate(title);
            string message = movieInteractor
                .AddNewMovie(movieToAdd) ? "" : " not";
            Console.WriteLine($"{title} was successfully{message} entered into the database...\n");
        }
    }
}
DisplayMenu();
void DisplayMenu()
{
    
while (true)
{
    Console.Clear();
    string[] options = ui.FormatStringyData(new string[]
    {
    "Get Motion Picture Video Film by Title",
    "Get Motion Picture Video Film By Genre",
    });
    Console.WriteLine("Hello Video film enthusiest, please select from the following options okay?\n\n");
    foreach (string option in options)
    {
        ui.SetConsoleColor();
        Console.WriteLine(option);
    }
    
    string currentSelection = Console.ReadLine();
        if (currentSelection == null || currentSelection != "1" && currentSelection != "2")
    {
        ui.SetConsoleColor();
        Console.WriteLine("That was not a valid selection, press any key and please try again...");
        Console.ReadKey();
        continue;
    }
    string menuParam = currentSelection == "1" ? "Title" : "Genre";
    string[] menuOptions = ui.FormatStringyData(new string[]
    {
    $"Select From list of {menuParam}'s",
    $"Enter {menuParam}"

    });
    DisplaySecondMenu(menuParam, menuOptions);
        
}
    
}
void QueryByEnteringTitleOrGenre(string menuParam)
{
    ui.SetConsoleColor();
    Console.WriteLine($"Enter the {menuParam} of the motion picture video film you would like to query...");
    string query = Console.ReadLine();
    List<Movie> movieList = menuParam == "Title" ? movieInteractor.GetMovieByTitle(query) : movieInteractor.GetMovieByGenre(query);
    Console.Clear();
    foreach (Movie movie in movieList)
    {
        ui.SetConsoleColor();
        Console.WriteLine(movie.ToString());
    }
    Console.WriteLine("Press any key to return to the main menu...");
    Console.ReadKey();
    DisplayMenu();
}
void DisplaySecondMenu(string menuParam, string[] menuOptions)
{
    Console.Clear();
    while (true)
    {
        foreach (string option in menuOptions)
        {
            ui.SetConsoleColor();
            Console.WriteLine(option);
        }
        string currentSelection = Console.ReadLine();
        if (currentSelection == null || currentSelection != "1" && currentSelection != "2")
        {
            ui.SetConsoleColor();
            Console.WriteLine("That was not a valid selection, press any key and please try again...");
            Console.ReadKey();
            continue;
        }
        if (currentSelection != "1")
        {
            QueryByEnteringTitleOrGenre(menuParam);
            break;
        }
        menuOptions = menuParam == "Title" ?
            ui
            .FormatStringyData(movieInteractor.GetListOfTitles()) :
            ui
            .FormatStringyData(movieInteractor.GetListOfGenres());
        //Console.ReadKey();
        //Console.Clear();
        DisplayListOfTitlesOrGenres(menuParam, menuOptions);
    }
    DisplayMenu();
}
void DisplayListOfTitlesOrGenres(string menuParam, string[] menuOptions)
{
    ui.SetConsoleColor();
    Console.WriteLine($"Please select from the following {menuParam}'s...");
    int count = 0;
    string currentSelection = string.Empty;
    foreach (string option in menuOptions)
    {
        if (count != 0 && count % 5 == 0)
        {
            ui.SetConsoleColor();
            Console.WriteLine("Enter a selection or Press n to display the next set of options...");
            currentSelection = Console.ReadLine();
            if (currentSelection == null || currentSelection.Length == 0)
            {
                ui.SetConsoleColor();
                Console.WriteLine("That was not a valid selection, start over!");
                DisplayMenu();
            }
            else if (currentSelection.All(c => char.IsNumber(c)))
            {
                int numericSelection = int.Parse(currentSelection) - 1;
                ValidateNumericSelection(numericSelection, menuOptions.Length);
                DisplayQueryResults(menuParam, numericSelection);
                Console.ReadKey();
                Console.Clear();
            }
        }
        
        
            
        

        ui.SetConsoleColor();
        Console.WriteLine(option);
        count++;
    }
    Console.WriteLine("Enter a selection...");
    currentSelection = Console.ReadLine();
    if (currentSelection.All(c => char.IsNumber(c)))
    {
        int numericSelection = int.Parse(currentSelection) - 1;
        ValidateNumericSelection(numericSelection, menuOptions.Length);
        DisplayQueryResults(menuParam, numericSelection);
    }
    Console.ReadKey();
}
void ValidateNumericSelection(int numericSelection, int length)
{
    if (numericSelection < 0 || numericSelection > length - 1)
    {
        ui.SetConsoleColor();
        Console.WriteLine("That was not a valid selection press any key and start over!");
        Console.ReadKey();
        DisplayMenu();
    }
}
void DisplayQueryResults(string menuParam, int numericSelection)
{
    
    List<Movie> movies = menuParam == "Title" ?
                    movieInteractor
                    .GetMovieByTitle(movieInteractor.GetListOfTitles()[numericSelection]) :
                    movieInteractor
                    .GetMovieByGenre(movieInteractor.GetListOfGenres()[numericSelection]);
    ui.SetConsoleColor();
    Console.WriteLine("Here is the result of the query...");
    foreach (Movie movie in movies)
    {
        ui.SetConsoleColor();
        Console.WriteLine(movie.ToString());
    }
    Console.ReadKey();
    Console.Clear();
    DisplayMenu();
}