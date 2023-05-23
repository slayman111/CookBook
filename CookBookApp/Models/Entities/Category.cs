using System;
using System.Collections.Generic;

namespace CookBookApp.Models.Entities
{
    public partial class Category
    {
        public Category()
        {
            Receipts = new HashSet<Receipt>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}