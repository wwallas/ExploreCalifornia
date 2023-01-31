using ExploreCalifornia.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExploreCalifornia.Controllers
{
    
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly BlogDataContext _db;

        public BlogController(BlogDataContext db)
        {
            _db = db;
        }

        [Route("")]
        public IActionResult Index(int page = 0)
        {
            var pageSize = 2;
            var totalPosts = _db.Posts.Count();
            var totalPages = totalPosts / pageSize;
            var previousPage = page - 1;
            var nextPage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;

            var posts =
                _db.Posts
                    .OrderByDescending(x => x.Posted)
                    .Skip(pageSize * page)
                    .Take(pageSize)
                    .ToArray();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView(posts);

            //var posts = _db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();

            //return new ContentResult { Content = "Blog posts"};
            // var posts = new[]
            //{
            //     new Post
            //     {
            //         Title = "My blog Post",
            //         Posted = DateTime.Now,
            //         Author = "Jess Chadwick",
            //         Body = "This is a great blog post, don't you think",
            //     },
            //     new Post
            //     {
            //         Title = "My blog Post",
            //         Posted = DateTime.Now,
            //         Author = "William Alfonso",
            //         Body = "This is a great blog post, don't you think",
            //     },
            // };
            return View(posts);
            
        }

        [Route("{year:min(2022)}/{month:range(1,12)}/{key}")] 
        public IActionResult Post(int year, int month, string key)
        {
            var post = _db.Posts.FirstOrDefault(x => x.Key == key);

            //var post = new Post
            //{
            //    Title = "My blog post",
            //    Posted = DateTime.Now,
            //    Author = "Jess Chadwick",
            //    Body = "This is a great blog post, don't you think?",
            //};

            return View(post);
            //ViewBag.Title = "My blog post";
            //ViewBag.Posted = DateTime.Now;
            //ViewBag.Author = "Jess Chadwick";
            //ViewBag.Body = "This is a great blog post, don't you think";

            //return new ContentResult { 
            //    Content = string.Format("Year:{0}; Month:{1}; Key: {2}", year, month, key) 
            //};
            //if(id==null)
            //    return new ContentResult { Content = "null" };
            //else
            //    return new ContentResult { Content = id.ToString() };
        }

        [Authorize]
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            if (!ModelState.IsValid)             
                return View();
            
            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;

            _db.Posts.Add(post);
            _db.SaveChanges();

            return RedirectToAction("Post", "Blog", new 
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });

        }
    }
}
