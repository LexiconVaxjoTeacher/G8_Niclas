using CommunityArena.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CommunityArena.Controllers
{
    public class FighterController : Controller
    {
        // GET: Fighter
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateFighter(string username)
        {
            Tuple<string, bool> stringView = new Tuple<string, bool>(username, false);
            return View(stringView);
        }

        [HttpPost]
        public ActionResult CreateFighter(string username, int hp, int str, int skl, int def, int spd, int luk, int con, int sen)
        {
            if (hp + str + skl + def + spd + luk + con + sen > 8)
            {
                return View(new Tuple<string, bool>(username, true));
            }
            Fighter newFighter = new Fighter()
            {
                Username = username,
                MaxHP = hp * 10 + 20,
                Strength = str + 2,
                Skill = skl + 2,
                Defense = def + 2,
                Speed = spd + 2,
                Luck = luk,
                Constitution = con + 2,
                Sense = sen + 2,
                Level = 1,
                Experience = 0,
                Gold = 0,
                Points = 0
            };

            newFighter.HP = newFighter.MaxHP;

            Context.context.Fighters.Add(newFighter);
            (from u in Context.context.Users
             where u.UserName.Equals(username)
             select u).First().HasFighter = true;

            var user = Context.context.Users.Find(User.Identity.GetUserId());
            user.CurrentForumID = 17;
            user.CurrentThreadID = 17;
            Context.context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetFighter(string _username)
        {
            Fighter fighter = (from u in Context.context.Fighters
                               where u.Username.Equals(_username)
                               select u).First();

            return Json(fighter, JsonRequestBehavior.AllowGet);
        }
    }
}