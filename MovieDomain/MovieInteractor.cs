using MovieDTO;
using Repository;

namespace MovieDomain
{
    public class MovieInteractor
    {
        private MovieRepository _repository;
        public MovieInteractor()
        {
            _repository = new MovieRepository();
        }
        public bool AddNewMovie(Movie movieToAdd)
        {
            if(movieToAdd == null || string.IsNullOrEmpty(movieToAdd.Title) || string.IsNullOrEmpty(movieToAdd.Id.ToString()))
            {
                return false;
            }
            return _repository.AddMovie(movieToAdd);
        }
        public List<Movie> GetAllMovies()
        {
            
            return _repository
                .GetAllMovies();
        }
        public List<Movie> GetMovieByTitle(string title)
        {
            return _repository
                .GetAllMovies()
                .Where(m => m.Title.ToLower() == title.ToLower() || m.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        public List<Movie> GetMovieByGenre(string genre)
        {
            return _repository
                .GetAllMovies()
                .Where(m => m.Genre.ToLower() == genre.ToLower())
                .ToList();
        }
        public string[] GetListOfGenres()
        {
            return _repository
                .GetAllMovies()
                .Select(m => m.Genre)
                .Distinct()
                .ToArray();
        }
        public string[] GetListOfTitles()
        {
            return _repository
                .GetAllMovies()
                .Select(m=>m.Title)
                .Distinct()
                .ToArray();
        }
    }
}