using System;
using System.ComponentModel.DataAnnotations;

namespace MasterKinder.Models
{
    public class PostBlog
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        public string? ImageUrl { get; set; } // Gör ImageUrl valfritt
    }
}
