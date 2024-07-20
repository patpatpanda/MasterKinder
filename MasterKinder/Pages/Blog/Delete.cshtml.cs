using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MasterKinder.Data;
using MasterKinder.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MasterKinder.Pages.Blog
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly MrDb _context;

        public DeleteModel(MrDb context)
        {
            _context = context;
        }

        [BindProperty]
        public PostBlog PostBlog { get; set; }

        public IActionResult OnGet(int id)
        {
            PostBlog = _context.PostBlogs.FirstOrDefault(m => m.Id == id);

            if (PostBlog == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var postBlog = await _context.PostBlogs.FindAsync(id);

            if (postBlog == null)
            {
                return NotFound();
            }

            _context.PostBlogs.Remove(postBlog);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
