using System;
using System.Collections.Generic;

namespace CookBookApp.Models.Entities
{
    public partial class Receipt
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Caloric { get; set; } = null!;
        public string Squirrels { get; set; } = null!;
        public string Fatness { get; set; } = null!;
        public string Carbohydrates { get; set; } = null!;
        public byte[]? Image { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Ingredient> Ingridients { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Receipt()
        {
            Categories = new HashSet<Category>();
            Ingridients = new HashSet<Ingredient>();
            Instructions = new HashSet<Instruction>();
            Users = new HashSet<User>();
        }

        public Receipt(string name, string description, string caloric, string squirrels, string fatness, string carbohydrates)
        {
            Name = name;
            Description = description;
            Caloric = caloric;
            Squirrels = squirrels;
            Fatness = fatness;
            Carbohydrates = carbohydrates;
        }
    }
}
