using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salil.Model
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Range(1,100,ErrorMessage ="Range between $1 to $100")]
        public double Price { get; set; }
        /// <summary>
        /// This 2 are foreign key and how we can tell EF that act this as foreign key
        /// </summary>
        [DisplayName("Food Type")]
        public int FoodTypeId { get; set; }
        [ForeignKey("FoodTypeId")]
        public FoodType foodType { get; set; }
        [DisplayName("Category")]
        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}
