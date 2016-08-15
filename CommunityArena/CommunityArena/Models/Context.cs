using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Web.Services.Description;

namespace CommunityArena.Models
{
    public class Context : IdentityDbContext<AppUser>
    {
        static public UserStore<AppUser> _userstore;
        static public UserManager<AppUser> UserManager { get; set; }

        public Context() : base("CommunityArenaDb")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public static Context Create()
        {
            return new Context();
        }

        public DbSet<Forum> Forums { get; set; }
        public DbSet<Thread> Threads { get; set; }
        public DbSet<Post> Posts { get; set; }

        public DbSet<Fighter> Fighters { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Ownership> Ownerships { get; set; }

        public DbSet<Alert> Alerts { get; set; }
    }
}