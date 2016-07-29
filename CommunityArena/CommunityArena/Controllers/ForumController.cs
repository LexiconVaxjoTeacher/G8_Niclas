using CommunityArena.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityArena.Controllers
{
    public class ForumController : Controller
    {
        // GET: Forum
        public ActionResult Index()
        {
            return View();
        }

        public void CreateForum(string _name, int _parentForum)
        {
            using (var context = new Context())
            {
                context.Forums.Add(new Forum() {
                    Name = _name,
                    ParentForumID = _parentForum
                });
                context.SaveChanges();                
            }
        }
        
        public void CreateThread(string _name, int _forum)
        {
            using (var context = new Context())
            {
                context.Threads.Add(new Thread()
                {
                    Name = _name,
                    ForumID = _forum
                });

                context.SaveChanges();
            }
        }

        public void CreatePost(string _user, int _thread, string _text)
        {
            using (var context = new Context())
            {
                context.Posts.Add(new Post()
                {
                    Poster = _user,
                    ThreadID = _thread,
                    PostTime = DateTime.Now,
                    Text = _text
                });

                context.SaveChanges();
            }
        }

        public void DeleteForum(int _forum)
        {
            using (var context = new Context())
            {
                context.Forums.Remove(context.Forums.Find(_forum));
                context.SaveChanges();
            }
        }

        public void DeleteThread(int _thread)
        {
            using (var context = new Context())
            {
                context.Threads.Remove(context.Threads.Find(_thread));
                context.SaveChanges();
            }
        }

        public void DeletePost(int _post)
        {
            using (var context = new Context())
            {
                context.Posts.Remove(context.Posts.Find(_post));
                context.SaveChanges();
            }
        }

        public JsonResult GetMainForum()
        {
            Forum forum = new Forum();

            using (var context = new Context())
            {
                forum = (from f in context.Forums
                         where f.Name.Equals("Main Forum")
                         select f).First();
            }

            return Json(forum, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetForumName(int _forumId)
        {
            string result = "";
            using (var context = new Context())
            {
                if (_forumId == -1 || context.Forums.Count() < 1)
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                result = context.Forums.Find(_forumId).Name;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetForum(int _forumId)
        {
            Forum result = new Forum();
            using (var context = new Context())
            {
                result = context.Forums.Find(_forumId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThread(int _threadId)
        {
            Thread result = new Thread();
            using (var context = new Context())
            {
                result = context.Threads.Find(_threadId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubForums(int _forumId)
        {
            List<Forum> result = new List<Forum>();

            using (var context = new Context())
            {
                var forums = from f in context.Forums
                              where _forumId.Equals(f.ParentForumID)
                              select f;

                result = forums.ToList();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThreads(int _forumId)
        {
            List<Thread> result = new List<Thread>();

            using (var context = new Context())
            {
                var threads = from t in context.Threads
                         where _forumId.Equals(t.ForumID)
                         select t;

                result = threads.ToList();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPosts(int _threadId)
        {
            List<Post> result = new List<Post>();

            using (var context = new Context())
            {
                var posts = from p in context.Posts
                              where _threadId.Equals(p.ThreadID)
                              select p;

                result = posts.ToList();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public void UpdateForumPosition(int _forumId)
        {
            using (var context = new Context())
            {
                var users = from u in context.Users
                            where u.UserName.Equals(User.Identity.Name)
                            select u;
                    //context.Users.(User.Identity.Name);
                if (0 < users.Count())
                {
                    var user = users.First();

                    user.CurrentForumID = _forumId;
                    user.CurrentThreadID = -1;
                    context.SaveChanges();
                }
            }
        }

        public void UpdateThreadPosition(int _threadId)
        {
            using(var context = new Context())
            {
                var users = from u in context.Users
                            where u.UserName.Equals(User.Identity.Name)
                            select u;
                    //context.Users.Find(User.Identity.Name);
                if (0 < users.Count())
                {
                    var user = users.First();

                    user.CurrentThreadID = _threadId;
                    context.SaveChanges();
                }
            }
        }

        public JsonResult GetForumUsers(int _forumId)
        {
            List<string> result = new List<string>();

            using (var context = new Context())
            {
                var users = from u in context.Users
                                where _forumId.Equals(u.CurrentForumID)
                                select u;

                foreach (var user in users)
                {
                    result.Add(user.UserName);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThreadUsers(int _threadId)
        {
            List<string> result = new List<string>();

            using (var context = new Context())
            {
                var users = from u in context.Users
                            where _threadId.Equals(u.CurrentThreadID)
                            select u;

                foreach (var user in users)
                {
                    result.Add(user.UserName);
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Admin")]
        public void SeedForum()
        {
            /*CreateForum("Main Forum", 1, -1);
            CreateForum("Subforum 1", 2, 1);
            CreateForum("Subforum 2", 3, 1);
            CreateForum("Subforum 3", 4, 1);
            CreateForum("Subforum 1-1", 5, 2);
            CreateForum("Subforum 2-1", 6, 3);
            CreateForum("Subforum 2-2", 7, 3);
            CreateForum("Subforum 1-1-1", 8, 5);*/
            /*CreateThread("Thread 1", 1);
            CreateThread("Thread 2", 1);
            CreateThread("Thread 3", 1);
            CreateThread("Thread 4", 2);
            CreateThread("Thread 5", 2);
            CreateThread("Thread 6", 3);
            CreateThread("Thread 7", 4);
            CreateThread("Thread 8", 5);
            CreateThread("Thread 9", 6);
            CreateThread("Thread 10", 8);*/
        }
    }
}