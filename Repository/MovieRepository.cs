using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MovieDTO;

namespace Repository
{
    public class MovieRepository
    {
        private IConfigurationRoot _configuration;
        private DbContextOptionsBuilder<ApplicationDbContext> _optionsBuilder;
        public MovieRepository()
        {
            BuildOptions();
        }
        private void BuildOptions()
        {
            _configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
            _optionsBuilder= new DbContextOptionsBuilder<ApplicationDbContext>();
            _optionsBuilder.UseSqlServer(_configuration.GetConnectionString("StringyConnection"));
        }

        public bool AddMovie(Movie movieToAdd)
        {
            using(ApplicationDbContext db = new ApplicationDbContext(_optionsBuilder.Options))
            {
                //determine if Movie exists
                Movie existingMovie = db.Movies.FirstOrDefault(m => m.Title.ToLower() == movieToAdd.Title.ToLower());
                if(existingMovie == null)
                {
                    // movie doesn't exist, add it
                    db.Movies.Add(movieToAdd);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
        }
        public Movie GetMovieById(int id)
        {
            using(ApplicationDbContext db = new ApplicationDbContext(_optionsBuilder.Options))
            {
                return db.Movies.FirstOrDefault(m => m.Id == id); 
            }
        }
        public List<Movie> GetAllMovies()
        {
            using (ApplicationDbContext db = new ApplicationDbContext(_optionsBuilder.Options))
            {
                return db.Movies.ToList();
            }
        }
    }
}