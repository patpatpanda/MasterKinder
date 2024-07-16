using MasterKinder.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace MasterKinder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var blogPosts = new List<BlogPost>
            {
                new BlogPost { Id = 1, Title = "First Post", Content = "This is the first blog post.", CreatedAt = DateTime.UtcNow },
                new BlogPost { Id = 2, Title = "Second Post", Content = "This is the second blog post.", CreatedAt = DateTime.UtcNow }
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(blogPosts, options);
            return new ContentResult
            {
                Content = json,
                ContentType = "application/json",
                StatusCode = 200
            };
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var blogPosts = new List<BlogPost>
            {
                new BlogPost { Id = 1, Title = "First Post", Content = "This is the first blog post.", CreatedAt = DateTime.UtcNow },
                new BlogPost { Id = 2, Title = "Second Post", Content = "This is the second blog post.", CreatedAt = DateTime.UtcNow }
            };

            var post = blogPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(post, options);
            return new ContentResult
            {
                Content = json,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
    }
}
