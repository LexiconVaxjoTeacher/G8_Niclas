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
                context.Forums.Add(new Forum()
                {
                    Name = _name
                });

                context.SubForums.Add(new SubForum()
                {
                    ParentForumID = _parentForum,
                    SubForumID = context.Forums.Last().ID
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

        public void CreatePost(int _user, int _thread, string _text)
        {
            using (var context = new Context())
            {
                context.Posts.Add(new Post()
                {
                    UserID = _user,
                    ThreadID = _thread,
                    PostTime = DateTime.Now,
                    Text = _text
                });

                context.SaveChanges();
            }
        }

        public List<SubForum> GetSubForums(int _forumId)
        {
            List<SubForum> result = new List<SubForum>();

            using (var context = new Context())
            {
                var subforums = from sf in context.SubForums
                              where _forumId.Equals(sf.ParentForumID)
                              select sf;

                result = subforums.ToList();
            }

            return result;
        }

        public List<Thread> GetThreads(int _forumId)
        {
            List<Thread> result = new List<Thread>();

            using (var context = new Context())
            {
                var threads = from t in context.Threads
                         where _forumId.Equals(t.ForumID)
                         select t;

                result = threads.ToList();
            }

            return result;
        }
    }
}