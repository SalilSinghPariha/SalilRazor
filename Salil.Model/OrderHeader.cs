using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser applicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        //format for currency
        [DisplayFormat(DataFormatString ="{0:C}")]
        [DisplayName("Order Total")]
        public double OrderTotal { get; set; }

        [Required]
        [Display(Name ="Pick Up Time")]
        public DateTime PickupTime { get; set; }

        [Required]
        [NotMapped]
        public DateTime PickUpDate { get; set; }

        public string OrderStatus { get; set; }

        public string? Comment { get; set; }

        public string? SessionId { get; set; }

		public string? PaymentIntentId { get; set; }

		[Required]
        [Display(Name = "Pick Up Name")]
        public string PickUpName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }


    }
}
