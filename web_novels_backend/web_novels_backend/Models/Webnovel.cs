using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>()
                .HasOne(p => p.Webnovel)
                .WithMany(b => b.Chapters)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class Webnovel
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string Translator { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime LastUpdated { get; set; }

        public ICollection<Chapter> Chapters { get; set; }

    }

}
