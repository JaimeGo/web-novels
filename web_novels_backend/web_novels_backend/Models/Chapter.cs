using System;
using Microsoft.EntityFrameworkCore;

namespace web_novels_backend.Models
{
    public class ChapterContext : DbContext
    {
        public DbSet<Chapter> Chapters { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=blogging.db");
        }
    }

    public class Chapter
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

    }
}
