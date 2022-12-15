using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace MovieDTO
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public double Runtime { get; set; }

        public override string ToString()
        {
            string movie = string.Empty;
            int length = Title.Length > Genre.Length ? Title.Length : Genre.Length;
            bool isEven = length % 2 == 0;
            int[] padding = new int[]
            {
                3 +  (int)Math.Floor((double)(length) / 2),
                (int)Math.Floor((double)(length + 6) / 2) ,
                (int)Math.Floor((double)(Math.Abs(Title.Length - length) + 6) / 2),
                (int)Math.Floor((double)(Math.Abs(Genre.Length - length) + 6) / 2),
                (int)Math.Floor((double)(Math.Abs(Runtime.ToString().Length - length) + 6) / 2),
                (int)Math.Floor((double)(length + 6) / 2) ,
                3 +  (int)Math.Floor((double)(length) / 2)

            };
            string[] props = new string[]
            {
                new string('-', 1),
                new string(' ', 1),
                Title,
                Genre,
                Runtime.ToString(),
                new string(' ', 1),
                new string('-', 1)
                
            };
            int magicNumber = (3 + (int)Math.Floor((double)(length) / 2) * 2);
            int count = 0;
            foreach(int i in padding)
            {
                
                string paddingString = count == 0 || count == padding.Length -1?new string('-',i): new string(' ', i);
                string toAdd = $"|{paddingString}{props[count]}{paddingString}|\n";
                string mod = string.Empty;
                while(true)
                {
                    if (toAdd.Length + mod.Length -7 >= magicNumber)
                    {
                        break;
                    }
                    mod += " ";
                }
                toAdd = $"|{paddingString}{props[count]}{mod}{paddingString}|\n";
                movie += toAdd;
                count++;
            }
            return movie;
        }
    }

    
}