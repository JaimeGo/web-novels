using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace web_novels_backend.Models
{

    public class Chapter
    {
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public Webnovel Webnovel { get; set; }


    }
}
