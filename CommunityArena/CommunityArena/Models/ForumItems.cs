using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityArena.Models
{
    public class Forum
    {
        public Forum()
        {

        }

        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentForumID { get; set; }
    }

    public class Thread
    {
        public Thread()
        {

        }

        [Key]
        public int ID { get; set; }
        public int ForumID { get; set; }
        public string Name { get; set; }
        public bool Public { get; set; }
        public string Target { get; set; }
    }

    public class Post
    {
        public Post()
        {

        }

        [Key]
        public int ID { get; set; }
        public int ThreadID { get; set; }
        public string Poster { get; set; }
        public DateTime PostTime { get; set; }
        public string Text { get; set; }
    }
}