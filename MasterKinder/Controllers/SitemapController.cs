using MasterKinder.Data;
using MasterKinder.Models;
using MasterKinder.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterKinder.Controllers
{
    [Route("sitemap.xml")]
    public class SitemapController : Controller
    {
        private readonly MrDb _context;

        public SitemapController(MrDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPosts.ToListAsync();
            var sitemapItems = new List<SitemapItem>();

            foreach (var post in blogPosts)
            {
                // Generate the URL for each blog post
                string url = Url.Page("/Blog/Details", null, new { slug = post.Slug }, Request.Scheme);

                sitemapItems.Add(new SitemapItem
                {
                    Url = url,
                    LastModified = post.PublishedDate
                });
            }

            var sitemap = new Sitemap(sitemapItems);
            return Content(sitemap.ToXml(), "application/xml");
        }
    }
}
