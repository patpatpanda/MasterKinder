using MasterKinder.Data;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BlogController : ControllerBase
{
    private readonly MrDb _context;

    public BlogController(MrDb context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetPosts()
    {
        var posts = _context.PostBlogs.OrderByDescending(p => p.PublishedDate).ToList();
        return Ok(posts);
    }

    [HttpGet("{id}")]
    public IActionResult GetPost(int id)
    {
        var post = _context.PostBlogs.FirstOrDefault(p => p.Id == id);
        if (post == null)
        {
            return NotFound();
        }
        return Ok(post);
    }
}
