using MovieDTO;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;

namespace MovieApiHandler
{
    public class MovieApi
    {
        private HttpClient _client { get; set; }
        private HttpRequestMessage _search {get; set;}
        private HttpRequestMessage _details { get; set; }
        public MovieApi()
        {
            _client = new HttpClient();
        }
        public Movie Orchestrate(string title)
        {
            GetSearchMessage(title);
            Task<JObject> result = ProcessRepositoriesAsync(_client, _search);
            if(result.Result == null || result.Result["data"]["search"]["movies"].Count() == 0) // 2nd on is a JArray, probably throw exception
            {
                Console.WriteLine($"no data was found for {title}");
                return new Movie();
            }
            string emsVersionId = GetListOfTitles(result);
            if(emsVersionId == "-1")
            {
                Console.WriteLine($"no data was found for {title}");
                return new Movie();
            }
            GetDetailsMessage(emsVersionId);
            return SerializeMovieFromJObject(ProcessRepositoriesAsync(_client, _details));
        }
        public void GetSearchMessage(string title)
        {
            _search = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://flixster.p.rapidapi.com/search?query={title}"), // maybe need to do something with optional params here
                Headers =
                {
                    {"X-RapidAPI-Key", "8a1920a098mshfc90a10a8464b5ap1cf15ejsndf3b1770a944" },
                    {"X-RapidAPI-Host", "flixster.p.rapidapi.com" },
                },
            };
        }
        public void GetDetailsMessage(string id)
        {
            _details = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://flixster.p.rapidapi.com/movies/detail?emsVersionId={id}"),
                Headers =
                {
                    { "X-RapidAPI-Key", "8a1920a098mshfc90a10a8464b5ap1cf15ejsndf3b1770a944" },
                    { "X-RapidAPI-Host", "flixster.p.rapidapi.com" },
                },
            };
        }
        public static async Task<JObject> ProcessRepositoriesAsync(HttpClient client, HttpRequestMessage request)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add
                (
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
            foreach (var header in request.Headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value.ToArray()[0]);
            }
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(body);
                return jObject ?? new();
            }
        }
        private string GetListOfTitles(Task<JObject> titles)
        {
            JObject json = titles.Result;
            
            JArray movies = (JArray)json["data"]["search"]["movies"];
            int count = 1;
            foreach(JObject movie in movies)
            {
                string title = (string)movie["name"];
                string id = (string)movie["emsVersionId"];
                
                string rating = !movie.GetValue("tomatoRating").HasValues ? string.Empty : (string)movie["tomatoRating"]["tomatometer"];
                Console.WriteLine($"{count}) Title: {title} Rating: {rating}\n");
                count++;
            }
            Console.WriteLine("Please select which movie you would like to add or press any non numerical key to skip...");
            string index = Console.ReadLine();
            if (index.All(c => char.IsNumber(c)))
            {
                return ParseEmsVersionId(titles)[int.Parse(index) - 1];
            }
            return "-1";
        }
        private string[] ParseEmsVersionId(Task<JObject> result)
        {
            JObject json = result.Result;
            JArray movies = (JArray)json["data"]["search"]["movies"];
            return movies
                .Select(m => (string)m["emsVersionId"])
                .ToArray().ToArray();
        }
        private Movie SerializeMovieFromJObject(Task<JObject> result)
        {
            
            JObject json = result.Result;
            JObject movieData = JObject.Parse(json["data"]["movie"].ToString());
            JObject genres = JObject.Parse(json["data"]["movie"]["genres"][0].ToString());
            //string genre = !movieData.GetValue("genre").HasValues ? string.Empty : (string)genres["name"];
            JToken? trouble = null;
            Console.WriteLine(movieData.GetValue("durationMinutes"));
            double runTime;
            try
            {
                runTime = (double)movieData.SelectToken(@"$.['durationMinutes']", errorWhenNoMatch: true);
            }
            catch
            {
                runTime= 0;
            }
            Movie movie = new()
            {
                Title = (string)movieData["name"],
                Genre = (string)genres["name"],
                Runtime = runTime
            };
            Console.WriteLine(movie.ToString());
            return movie;
        }
    }
}