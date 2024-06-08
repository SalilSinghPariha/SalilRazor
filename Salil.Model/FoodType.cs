using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Salil.Model
{
    public class FoodType
    {
        //If Id is there then automatically EF will take this as primary key
        //or else we can use Key data annotation to tell EF that it is primary key
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

    }
}
