using System;
using System.Collections.Generic;

namespace CookBookApp.Models.Entities
{
    public partial class Ingredient
    {
        public Ingredient()
        {
            Receips = new HashSet<Receipt>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Receipt> Receips { get; set; }

        public override string? ToString()
        {
            return Name;
        }
    }
}