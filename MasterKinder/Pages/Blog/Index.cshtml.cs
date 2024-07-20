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

        public List<PostBlog> PostBlogs { get; set; }

        public void OnGet()
        {
            PostBlogs = _context.PostBlogs.ToList();
        }
    }
}
