using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_Shop.Entities.Models
{
    public class Product
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        [ValidateNever]
        public string Name { get; set; }
        [ValidateNever]
        public string Description { get; set; }
        [Required]
        [ValidateNever]
        public Decimal  Price { get; set; }
        [DisplayName("Image")]
        [ValidateNever]
        public string Img { get; set; }
        [DisplayName("Category")]
        [ValidateNever]
        public int CategoryId { get; set; }
        [ValidateNever]
        public Category Category { get; set; }
    }
}
