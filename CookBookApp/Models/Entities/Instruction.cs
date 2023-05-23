using System;
using System.Collections.Generic;

namespace CookBookApp.Models.Entities
{
    public partial class Instruction
    {
        public Instruction()
        {
            Receipts = new HashSet<Receipt>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Receipt> Receipts { get; set; }
    }
}
