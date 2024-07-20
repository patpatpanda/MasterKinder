using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MasterKinder.Data;
using MasterKinder.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;

namespace MasterKinder.Pages.Blog
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly MrDb _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(MrDb context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public PostBlog PostBlog { get; set; }

        
        public IFormFile Image { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Image != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(Image.FileName)}";
                var filePath = Path.Combine(_environment.WebRootPath, "images", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await Image.CopyToAsync(fileStream);
                }

                PostBlog.ImageUrl = $"/images/{fileName}";
                Console.WriteLine($"Image uploaded successfully. ImageUrl: {PostBlog.ImageUrl}");
            }
            else
            {
                Console.WriteLine("No image uploaded.");
            }

            // Log the PostBlog properties before validation
            Console.WriteLine($"After: Title: {PostBlog.Title}, Content: {PostBlog.Content}, PublishedDate: {PostBlog.PublishedDate}, ImageUrl: {PostBlog.ImageUrl}");

            if (!ModelState.IsValid)
            {
                // Log the validation errors
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { x.Key, x.Value.Errors })
                    .ToArray();

                foreach (var error in errors)
                {
                    Console.WriteLine($"Key: {error.Key}, Errors: {string.Join(", ", error.Errors.Select(e => e.ErrorMessage))}");
                }

                return Page();
            }

            PostBlog.PublishedDate = DateTime.Now; // Set the published date

            try
            {
                _context.PostBlogs.Add(PostBlog);
                await _context.SaveChangesAsync();
                Console.WriteLine("PostBlog saved successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving PostBlog: {ex.Message}");
            }

            return RedirectToPage("Index");
        }
    }
}
