using System;
using Microsoft.EntityFrameworkCore;

namespace web_novels_backend.Models
{
    public class WebnovelContext : DbContext
    {
        public DbSet<Webnovel> Webnovels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=webnovels.db");
        }
    }

    public class Webnovel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Translator { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}
