using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MasterKinder.Pages.Blog
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly MrDb _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(MrDb context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public PostBlog PostBlog { get; set; }

       
        public IFormFile Image { get; set; }

        public IActionResult OnGet(int id)
        {
            PostBlog = _context.PostBlogs.FirstOrDefault(m => m.Id == id);

            if (PostBlog == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var postToUpdate = await _context.PostBlogs.FindAsync(PostBlog.Id);

            if (postToUpdate == null)
            {
                return NotFound();
            }

            postToUpdate.Title = PostBlog.Title;
            postToUpdate.Content = PostBlog.Content;
            postToUpdate.PublishedDate = PostBlog.PublishedDate;

            if (Image != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                postToUpdate.ImageUrl = $"/images/{fileName}";
            }

            try
            {
                await _context.SaveChangesAsync();
                Console.WriteLine("BlogPost updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating BlogPost: {ex.Message}");
            }

            return RedirectToPage("Index");
        }
    }
}
