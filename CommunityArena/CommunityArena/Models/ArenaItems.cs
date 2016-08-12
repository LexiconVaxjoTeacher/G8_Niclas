using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommunityArena.Models
{
    public class Fighter
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int Strength { get; set; }
        public int Skill { get; set; }
        public int Defense { get; set; }
        public int Speed { get; set; }
        public int Luck { get; set; }
        public int Constitution { get; set; }
        public int Sense { get; set; }

        public int Level { get; set; }
        public int Experience { get; set; }

        public int Points { get; set; }

        public int Gold { get; set; }
    }

    public class Alert
    {
        [Key]
        public int ID { get; set; }
        public int ThreadID { get; set; }
        public string User { get; set; }
        public bool Viewed { get; set; }
        public string Message { get; set; }
    }

    public class Item
    {
        [Key]
        public int ID { get; set; }
        public string StatBoosted { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
        public int Weight { get; set; }
    }

    public class Ownership
    {
        public int ID { get; set; }
        public int FighterID { get; set; }
        public int ItemID { get; set; }
    }
}