//using MasterKinder.Data;
//using MasterKinder.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MasterKinder.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class BlogController : ControllerBase
//    {
//        private readonly MrDb _context;

//        public BlogController(MrDb context)
//        {
//            _context = context;
//        }

//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<BlogPost>>> GetBlogPosts()
//        {
//            return await _context.BlogPosts.ToListAsync();
//        }

//        [HttpGet("{id}")]
//        public async Task<ActionResult<BlogPost>> GetBlogPost(int id)
//        {
//            var blogPost = await _context.BlogPosts.FindAsync(id);

//            if (blogPost == null)
//            {
//                return NotFound();
//            }

//            return blogPost;
//        }

//        [Authorize]
//        [HttpPost]
//        public async Task<ActionResult<BlogPost>> CreateBlogPost(BlogPost blogPost)
//        {
//            _context.BlogPosts.Add(blogPost);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
//        }

//        [Authorize]
//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateBlogPost(int id, BlogPost blogPost)
//        {
//            if (id != blogPost.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(blogPost).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!BlogPostExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        [Authorize]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteBlogPost(int id)
//        {
//            var blogPost = await _context.BlogPosts.FindAsync(id);
//            if (blogPost == null)
//            {
//                return NotFound();
//            }

//            _context.BlogPosts.Remove(blogPost);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool BlogPostExists(int id)
//        {
//            return _context.BlogPosts.Any(e => e.Id == id);
//        }
//    }
//}
