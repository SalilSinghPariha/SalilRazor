using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Salil.Model
{
    public class Category
    {
        //If Id is there then automatically EF will take this as primary key
        //or else we can use Key data annotation to tell EF that it is primary key
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1,100,ErrorMessage ="Display order wil be between 1 to 100")]
        public int DisplayOrder { get; set; }
    }
}
