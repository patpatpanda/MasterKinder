using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using MasterKinder.Data;
using MasterKinder.Models;

namespace MasterKinder.Pages.Blog
{
    
    public class IndexModel : PageModel
    {
        private readonly MrDb _context;

        public IndexModel(MrDb context)
        {
            _context = context;
        }

        public List<BlogPost> BlogPosts { get; set; }

        public void OnGet()
        {
            BlogPosts = _context.BlogPosts.OrderByDescending(post => post.PublishedDate).ToList();
        }
    }
}
