using System;
using System.ComponentModel.DataAnnotations;

namespace CookingAppAPI.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackID { get; set; }

        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        public string DishName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Category { get; set; }

        public string Comment { get; set; }

        public double? Rating { get; set; }
    }


}