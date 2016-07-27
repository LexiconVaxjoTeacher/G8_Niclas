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
    }

    public class SubForum
    {
        public SubForum()
        {

        }

        [Key]
        public int ID { get; set; }
        public int ParentForumID { get; set; }
        public int SubForumID { get; set; }
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
    }

    public class Post
    {
        public Post()
        {

        }

        [Key]
        public int ID { get; set; }
        public int ThreadID { get; set; }
        public int UserID { get; set; }
        public DateTime PostTime { get; set; }
        public string Text { get; set; }
    }
}