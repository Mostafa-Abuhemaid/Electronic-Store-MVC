using System.ComponentModel.DataAnnotations;

namespace My_Shop.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreateTime { get; set; }= DateTime.Now;
    }
}
