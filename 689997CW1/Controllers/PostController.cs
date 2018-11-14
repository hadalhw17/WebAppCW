//Created by Aleksandr Slobodov, student number 689997

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _689997CW1.Data;
using _689997CW1.Models;
using Microsoft.AspNetCore.Identity;

namespace _689997CW1
{
    public class PostController : Controller
    {
        // Our database.
        private readonly ApplicationDbContext _context;

        // User manager, that is used to automatically set author name.
        private readonly UserManager<User> _userManager;

        // Returns user that is currently logged in.
        private Task<User> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // Constructor.
        public PostController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET
        // Print the list of all posts.
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.ToListAsync());
        }

        // GET
        //Create a prewiew of a single post in Details view.
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            // 404 if id isn't valid. 
            if (id == null)
            {
                return NotFound();
            }

            // Returns first post with matching Id, or if is not found returns null. 
            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostId == id);

            // 404 if post isn't valid.
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // GET
        // Creates a /Post/Create view.
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        // Adds newly created article to the databse.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,PostAuthor,PostText")] Post post)
        {
            // Set name of post author to the name of current user
            var user = await GetCurrentUserAsync();
            post.PostAuthor = _userManager.GetUserName(User);

            // Set timestamp to current date and time
            post.TimeStamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Add post to the database
                _context.Add(post);

                //Apply changes
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(post);
        }

        // GET
        // Takes id and a post from a database and passes them to Edit View.
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            // 404 if id isn't valid.
            if (id == null)
            {
                return NotFound();
            }

            // Select post from database by id.
            var post = await _context.Post.FindAsync(id);

            // 404 if post isn't valid.
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST
        // Updates database with edited post.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,PostAuthor,PostText")] Post post)
        {
            // 404 if ids not match.
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Apply changes to the database.
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                // If database was not changed, then execute code in catch
                catch (DbUpdateConcurrencyException)
                {
                    // 404 if post doesn't exst
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET
        // Delete post from database. Selects post and
        // Passes it to Delete view.
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //404 if id isn't valid.
            if (id == null)
            {
                return NotFound();
            }

            // Returns first post with matching Id, or if is not found returns null.
            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.PostId == id);

            // 404 if post isn't valid.
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST
        // Renders confirmation window, after which it
        // Removes a post from a database.
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Select post by Id.
            var post = await _context.Post.FindAsync(id);

            // Remove post from database.
            _context.Post.Remove(post);

            // Apply changes.
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //Validate if post with the same id already exists in the database.
        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }
    }
}