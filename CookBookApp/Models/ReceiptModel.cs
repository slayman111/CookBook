using System.IO;
using CookBookApp.Models.Entities;
using System.Linq;
using System.Windows.Media.Imaging;

namespace CookBookApp.Models
{
    internal class ReceiptModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Caloric { get; set; } = null!;
        public string Squirrels { get; set; } = null!;
        public string Fatness { get; set; } = null!;
        public string Carbohydrates { get; set; } = null!;
        public string Categories { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string IngridientCount { get; set; } = null!;
        public BitmapImage? Image { get; set; } = null!;

        public ReceiptModel(Receipt receipt)
        {
            Id = receipt.Id;
            Name = receipt.Name;
            Description = receipt.Description;
            Caloric = receipt.Caloric;
            Squirrels = receipt.Squirrels;
            Fatness = receipt.Fatness;
            Carbohydrates = receipt.Carbohydrates;

            Categories = string.Join("  •  ", receipt.Categories.Take(3).Select(c => c.Name.ToUpper()));
            Author = receipt.Users.FirstOrDefault()!.GetFullname();
            IngridientCount = receipt.Ingridients.Count.ToString();

            if (receipt.Image is not null) Image = ToImage(receipt.Image);
        }

        public static implicit operator ReceiptModel(Receipt receipt) => new(receipt);

        private BitmapImage ToImage(byte[] array)
        {
            using var ms = new MemoryStream(array);
            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = ms;
            image.EndInit();

            return image;
        }
    }
}
