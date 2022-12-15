using MovieDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieConsole
{
    public class UserInterface
    {
        public string AddMotionPictureVideoFilmData()
        {
            Console.WriteLine("Enter a movie title now okay?");
            string title = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Here we go...");
            return title;
            //movieInteractor.AddnewMovie(apiHandler.Orchestrate(title));
        }
        public string[] GetMotionPictureVideoFilmStringData(List<Movie> movies)
        {
            return movies
                .Select(m => m.ToString())
                .ToArray();
        }
        public string GetDataByIndex(int index, string[] menu)
        {
            return menu[index - 1];
        }
        public bool ManageInteractorResponse(bool response)
        {
            return true;
        }
        public string[] FormatStringyData(string[] data)
        {
            int index = 1;
            int magicNumber = 8;
            string[] stringyData = new string[data.Length];
            foreach (string stringy in data) 
            {
                string output = string.Empty;
                int length = stringy.Length + index.ToString().Length;
                output += $"{new string('-', length + magicNumber)}\n";
                output += $"|  {index}. {stringy}  |\n";
                output += $"{new string('-', length + magicNumber)}";
                stringyData[index-1] = output;
                index++;
            }
            //Console.WriteLine(stringyData.Length);
            return stringyData;
        }
        public void SetConsoleColor()
        {
            var currentColor = Console.ForegroundColor;
            Random random = new Random();
            while(true)
            {
                if(Console.ForegroundColor.Equals(currentColor))
                {
                    Console.ForegroundColor = (ConsoleColor)random.Next(9, 15);
                }
                else
                {
                    break;
                }
            }
        }
        public void DisplayMotionPictureVideoFilmData(List<Movie> movies)
        {
            for (int i = 0; i < movies.Count; i++)
            {
                SetConsoleColor();
                Console.WriteLine(movies[i].ToString());
                if (i != 0 && i % 5 == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to see the next set of video film data...");
                    Console.ReadKey();
                    Console.Clear();

                }
            }
        }
    }
}
