//Created by Aleksandr Slobodov, student number 689997

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppCW.Data;
using WebAppCW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace WebAppCW
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
        [Authorize(Policy = "IsBlogger")]
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
            var comments = _context.Comment.Where(x => x.PostID == id).ToList();
            var likes = _context.Likes.Where(x => x.PostId == id).ToList();

            PostComments pc = new PostComments
            {
                Post = post,
                Comments = comments,
                Likes = likes
            };

            // 404 if post isn't valid.
            if (post == null)
            {
                return NotFound();
            }
            return View(pc);
        }

        // GET
        // Creates a /Post/Create view.
        [HttpGet]
        [Authorize(Policy = "IsBlogger")]
        public IActionResult Create()
        {
            return View();
        }

        // POST
        // Adds newly created article to the databse.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsBlogger")]
        public async Task<IActionResult> Create([Bind("PostId, PostTitle, PostText")]Post post)
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
        [Authorize(Policy = "IsBlogger")]
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
        [Authorize(Policy = "IsBlogger")]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,PostTitle,PostText")] Post post)
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
                    // Set name of post author to the name of current user
                    var user = await GetCurrentUserAsync();
                    post.PostAuthor = _userManager.GetUserName(User);
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
        [Authorize(Policy = "IsBlogger")]
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
        [Authorize(Policy = "IsBlogger")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Select post by Id.
            var post = await _context.Post.FindAsync(id);
            var likes =  _context.Likes.Where(x => x.PostId == id).ToList();

            // Remove post from database.
            _context.Post.Remove(post);

            // Delete all likes related to this post
           _context.Likes.RemoveRange(likes);

            // REmove all comments related to this post 
           _context.Comment.RemoveRange(_context.Comment.Where(x => x.PostID == id ).ToList());

            // Apply changes.
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        //Validate if post with the same id already exists in the database.
        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.PostId == id);
        }

        // GET
        // Returns a view that contains Comment creation form.
        [HttpGet]
        [Authorize(Policy = "IsCommenter")]
        public async Task<IActionResult> AddComment(int? id)
        {
            // Takes all comments from the database for post with id given
            var comments = _context.Comment.Where(x => x.PostID == id).ToArray();

            return View("Index");
        }

        // POST
        // Logic for comment creation
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "IsCommenter")]
        public async Task<IActionResult> AddComment([Bind("CommentID, CommentTitle, CommentText, PostID")]Comment comment, int? id)
        {
            // Set name of comment author to the name of current user.
            var user = await GetCurrentUserAsync();
            comment.AuthorName = _userManager.GetUserName(User);

            // Adds comment to the database.
            _context.Comment.Add(comment);
            _context.SaveChanges();

            // Selects post with the given id from the database.
            var post = await _context.Post.FirstOrDefaultAsync(m => m.PostId == id);

            // Selects all of the comments for the post with the given id.
            var comments = _context.Comment.Where(x => x.PostID == id).ToArray();

            // Selects all of the likes for the post with the given id
            var likes = _context.Likes.Where(x => x.PostId == id).ToList();

            // Packs everything to the view specific model that has post
            // And a list of comments and likes for this post.
            PostComments pc = new PostComments
            {
                Post = post,
                Comments = comments,
                Likes = likes
            };

            return RedirectToAction("Details", new { id = post.PostId });
        }


        // GET
        // Returns a view that contains Comment creation form.
        [Authorize(Policy = "IsCommenter")]
        public async Task<IActionResult> CreateComment(int? id)
        {
            // Selects post from the database with the given id.
            var post = await _context.Post
               .FirstOrDefaultAsync(m => m.PostId == id);

            // Selects all of the comments for the post with the given id.
            var comments = _context.Comment.Where(x => x.PostID == id).ToArray();

            // Creates a view specific model with an empty comment.
            PostComment pc = new PostComment
            {
                Post = post,
                Comment = new Comment()
            };

            // Links comment and post by assigning post id to the comment's post id.
            pc.Comment.PostID = post.PostId;

            return View("AddComment", pc);
        }

        // POST
        // Contains logic for toggling likes on posts.
        [Authorize(Policy = "IsCommenter")]
        public async Task<IActionResult> AddLike(int? id)
        {
            // Selects post from the database with the given id.
            var post = await _context.Post.FindAsync(id);

            // Gets username of the current user.
            var username = HttpContext.User.Identity.Name;

            // Selects all of the comments for the post with the given id.
            var comments = _context.Comment.Where(x => x.PostID == id).ToArray();

            // Selects all of the likes for current post that user that is currently logged in left (can only be one).
            var likes = _context.Likes.Where(x => x.PostId == id && x.UserName == username).ToList();

            bool likeExists = likes.Count() != 0;


            // If there is one then remove it from the database, add to it otherwise.
            if(likeExists)
            {
                _context.RemoveRange(likes);
            }
            else
            {
                // Creates like entity and adds it to the database.
                Like like = new Like
                {
                    PostId = post.PostId,
                    UserName = username
                };
                _context.Add(like);

            }
            var _likes = _context.Likes.Where(x => x.PostId == id).ToList();

            // Packs updated likes to the view specific model.
            PostComments pc = new PostComments
            {
                Post = post,
                Comments = comments,
                Likes = _likes
            };

            await _context.SaveChangesAsync();


            // 404 if post isn't valid.
            if (post == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", new { id = post.PostId});
        }
    }
}