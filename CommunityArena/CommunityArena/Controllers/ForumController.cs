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
        static Random rand = new Random(DateTime.Now.Millisecond);

        // GET: Forum
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Function that creates a forum.
        /// </summary>
        /// <param name="_name">Name of new forum.</param>
        /// <param name="_parentForum">ID of forum this is a subforum to.</param>
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

        /// <summary>
        /// Function that creates a thread.
        /// </summary>
        /// <param name="_name">Name of the thread.</param>
        /// <param name="_forum">Forum which contains the thread.</param>
        /// <returns>The new thread.</returns>
        public Thread CreateThread(string _name, int _forum)
        {
            Thread newThread = new Thread()
            {
                Name = _name,
                ForumID = _forum,
                Public = false,
                Target = ""
            };
            using (var context = new Context())
            {
                context.Threads.Add(newThread);
                context.SaveChanges();
            }
            return newThread;
        }

        /// <summary>
        /// Function that creates a thread with a target.
        /// </summary>
        /// <param name="_name">Name of the thread.</param>
        /// <param name="_forum">Forum which contains the thread.</param>
        /// <param name="_target">The user said thread is an interaction with.</param>
        /// <returns>The new thread.</returns>
        public Thread CreateThreadWithTarget(string _name, int _forum, string _target)
        {
            Thread newThread = new Thread()
            {
                Name = _name,
                ForumID = _forum,
                Public = true,
                Target = _target
            };
            using (var context = new Context())
            {
                context.Threads.Add(newThread);
                context.SaveChanges();
            }
            return newThread;
        }

        
    
        /// <summary>
        /// Function that creates a post.
        /// </summary>
        /// <param name="_user">Username of the one who created the post.</param>
        /// <param name="_thread">Thread which contains the post.</param>
        /// <param name="_text">Text which the post is to contain.</param>
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

        public JsonResult OpenInteraction(string _targetUser, int _currentForum)
        {
            Thread thread = CreateThreadWithTarget("Interaction", _currentForum, _targetUser);

            return Json(thread.ID, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetThreadTarget(int _threadId)
        {
            Thread thread = new Thread();
            using (var context = new Context())
            {
                thread = (from t in context.Threads
                          where t.ID.Equals(_threadId)
                          select t).First();
            }

            return Json(thread.Target, JsonRequestBehavior.AllowGet);
        }

        public void Attack(string _targetUser, int _currentForum, int _currentThread)
        {
            using (var context = new Context())
            {
                AppUser target = (from u in context.Users
                                  where u.UserName.Equals(_targetUser)
                                  select u).First();

                if (target.CurrentForumID == _currentForum)
                {
                    Fighter attacker = (from f in context.Fighters
                                        where User.Identity.Name.Equals(f.Username)
                                        select f).First();
                    Fighter defender = (from f in context.Fighters
                                        where _targetUser.Equals(f.Username)
                                        select f).First();

                    bool hit = false;
                    float modifier = 1f;

                    // Sneak-attack check.
                    if (target.CurrentThreadID != _currentThread)
                    {
                        hit = ResolveIfHit(attacker.Skill, defender.Sense);
                        modifier = 2f;

                        if (hit == false)
                        {
                            hit = ResolveIfHit(attacker.Skill * 2, defender.Speed + defender.Skill);
                            modifier = 1.5f;
                        }
                    }
                    else
                    {
                        hit = ResolveIfHit(attacker.Skill, defender.Skill);
                    }
                    if (hit == true)
                    {
                        int damage = ResolveDamage(attacker, defender, modifier, context);
                        CreatePost(User.Identity.Name, _currentThread, User.Identity.Name + " struck " + target.UserName + " for " + damage + " damage!");
                    }
                    else
                    {
                        CreatePost(User.Identity.Name, _currentThread, User.Identity.Name + " struck at " + target.UserName + "! " + target.UserName + " dodged!");
                    }
                }
                else
                {
                    CreatePost(User.Identity.Name, _currentThread, target.UserName + " left the room before " + User.Identity.Name + " could strike!");
                }
            }
        }

        bool ResolveIfHit(int attackingValue, int defendingValue)
        {
            int result = rand.Next(0, attackingValue + defendingValue);

            // Example. attackingValue = 4, defendingValue = 1
            // Possible results, 0 to 4, as 4 + 1 = 5.
            // 0 to 3 is a hit, as that's lower than attackingValue. Hitting is likely.
            // On the opposite, if attackingValue was 1 and defendingValue was 4.
            // Possible results still 0 to 4.
            // attackingValue will now only succeed on a 0.
            if (result < attackingValue)
            {
                return true;
            }

            // ... System needs to be reconsidered. As it is, at higher levels it will make almost no difference.
            // Someone with 55 skill and someone with 48 skill will have almost the same chance of hitting / missing.

            return false;
        }

        int ResolveDamage(Fighter attacker, Fighter defender, float modifier, Context context)
        {
            float balance = (float)attacker.Strength / (float)defender.Defense;
            float weaponStrength = 10;
            float levelBonus = (2 * (float)attacker.Level + 10) / 250;
            float damageFloat = (levelBonus * balance * weaponStrength + 2) * modifier;
            int damage = (int)Math.Round((decimal)damageFloat);

            defender.HP -= damage;

            context.SaveChanges();

            if (defender.HP < 1)
            {
                ResolveDeath(defender);
            }

            return damage;
        }

        void ResolveDeath(Fighter defender)
        {

        }

        /// <summary>
        /// Function that deletes a forum.
        /// </summary>
        /// <param name="_forum">Forum to delete.</param>
        public void DeleteForum(int _forum)
        {
            using (var context = new Context())
            {
                context.Forums.Remove(context.Forums.Find(_forum));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Function which deletes a thread.
        /// </summary>
        /// <param name="_thread">Thread to delete.</param>
        public void DeleteThread(int _threadId, Context context)
        {
            List<int> posts = (from p in context.Posts
                              where _threadId.Equals(p.ThreadID)
                              select p.ID).ToList();

            foreach (int post in posts)
            {
                context.Posts.Remove(context.Posts.Find(post));
            }
            context.Threads.Remove(context.Threads.Find(_threadId));
            context.SaveChanges();
        }

        /// <summary>
        /// Function which deletes a post.
        /// </summary>
        /// <param name="_post">Post to delete.</param>
        public void DeletePost(int _post)
        {
            using (var context = new Context())
            {
                context.Posts.Remove(context.Posts.Find(_post));
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Function which finds the forum named "Main Hall" and returns it.
        /// </summary>
        /// <returns>Returns the Main Hall.</returns>
        public JsonResult GetMainForum()
        {
            Forum forum = new Forum();

            using (var context = new Context())
            {
                forum = (from f in context.Forums
                         where f.Name.Equals("Main Hall")
                         select f).First();
            }

            return Json(forum, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Function that returns the name of a Forum.
        /// </summary>
        /// <param name="_forumId">The ID of the forum we want the name of.</param>
        /// <returns>The name of the forum.</returns>
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

        /// <summary>
        /// Get a forum.
        /// </summary>
        /// <param name="_forumId">Id of the forum we're getting.</param>
        /// <returns>The forum.</returns>
        public JsonResult GetForum(int _forumId)
        {
            Forum result = new Forum();
            using (var context = new Context())
            {
                result = context.Forums.Find(_forumId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get a thread.
        /// </summary>
        /// <param name="_threadId">ID of thread we're getting.</param>
        /// <returns>The thread.</returns>
        public JsonResult GetThread(int _threadId)
        {
            Thread result = new Thread();
            using (var context = new Context())
            {
                result = context.Threads.Find(_threadId);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all subforums that belong to a specific forum.
        /// </summary>
        /// <param name="_forumId">The forum we want the subforums of.</param>
        /// <returns>A list of subforums.</returns>
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

        /// <summary>
        /// Gets all the threads in a forum.
        /// </summary>
        /// <param name="_forumId">The ID of the forum we're getting threads from.</param>
        /// <returns>A list of threads in the forum.</returns>
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

        /// <summary>
        /// Gets all the posts in a thread.
        /// </summary>
        /// <param name="_threadId">ID of the thread we're getting the posts from.</param>
        /// <returns>A list of the posts that was in the thread.</returns>
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

        /// <summary>
        /// Function which updates the position of the current user.
        /// It also checks if the thread behind the user is empty. If it is, it is deleted.
        /// </summary>
        /// <param name="_forumId">Forum which the position is to be updated to.</param>
        public void UpdateForumPosition(int _forumId)
        {
            int lastThread = -1;

            using (var context = new Context())
            {
                var users = from u in context.Users
                            where u.UserName.Equals(User.Identity.Name)
                            select u;
                    //context.Users.(User.Identity.Name);
                if (0 < users.Count())
                {
                    var user = users.First();

                    lastThread = user.CurrentThreadID;

                    user.CurrentForumID = _forumId;
                    user.CurrentThreadID = -1;
                    context.SaveChanges();

                    if (lastThread != -1)
                    {
                        List<string> listOfUsersLeftInThread = GetThreadUserList(lastThread);
                        if (listOfUsersLeftInThread.Count < 1)
                        {
                            DeleteThread(lastThread, context);
                        }
                    }

                    context.SaveChanges();
                }
            }
        }
        /// <summary>
        /// Update the thread-position of the current user.
        /// </summary>
        /// <param name="_threadId">The thread which the position is to be updated to.</param>
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

        /// <summary>
        /// Gets a list of all the users currently on a specific forum.
        /// </summary>
        /// <param name="_forumId">The forum which we're checking.</param>
        /// <returns>A list of users currently browsing this forum.</returns>
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

        /// <summary>
        /// A function that returns the users that are currently examining this thread.
        /// </summary>
        /// <param name="_threadId">The thread we're checking.</param>
        /// <returns>A list of all users in this thread.</returns>
        public JsonResult GetThreadUsers(int _threadId = 0)
        {
            if (_threadId == 0)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            List<string> result = GetThreadUserList(_threadId);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public List<string> GetThreadUserList(int _threadId)
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

            return result;
        }

        [HttpPost]
        public JsonResult GetThreadUsers(int[] _threadIds)
        {
            List<List<string>> usernamesPerThread = new List<List<string>>();

            if (_threadIds != null)
            {
                using (var context = new Context())
                {
                    for (int i = 0; i < _threadIds.Length; i++)
                    {
                        int threadId = _threadIds[i];

                        List<string> users = (from u in context.Users
                                              where threadId.Equals(u.CurrentThreadID)
                                              select u.UserName).ToList();
                        usernamesPerThread.Add(users);
                    }
                }
            }

            return Json(usernamesPerThread);
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