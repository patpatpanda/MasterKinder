using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class DetailsModel : PageModel
{
    private readonly MrDb _context;

    public DetailsModel(MrDb context)
    {
        _context = context;
    }

    public BlogPost BlogPost { get; set; }

    public async Task<IActionResult> OnGetAsync(string slug)
    {
        BlogPost = await _context.BlogPosts
                                 .Include(b => b.Category)
                                 .FirstOrDefaultAsync(m => m.Slug == slug);

        if (BlogPost == null)
        {
            return NotFound();
        }

        return Page();
    }
}
