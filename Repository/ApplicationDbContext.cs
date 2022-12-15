
using MovieDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Options;

namespace Repository
{
    public class ApplicationDbContext : DbContext
    {
        
        public DbSet<Movie> Movies { get; set; }
        public ApplicationDbContext()
        {

        }
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        private IConfigurationRoot _configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                _configuration = builder.Build();
                string cnstr = _configuration.GetConnectionString("StringyConnection");
                optionsBuilder.UseSqlServer(cnstr);
            }
            
        }
    }
}
