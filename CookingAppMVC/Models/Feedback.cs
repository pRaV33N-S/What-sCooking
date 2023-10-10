using System;
using System.ComponentModel.DataAnnotations;

namespace CookingAppMVC.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }


        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Username must be between 5 and 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter a dish name.")]
        [MaxLength(40, ErrorMessage = "Dish name cannot exceed 40 characters.")]
        public string DishName { get; set; }

        [Required(ErrorMessage = "Please enter a category.")]
        [MaxLength(20, ErrorMessage = "Category cannot exceed 20 characters.")]
        public string Category { get; set; }


        [MaxLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }


        [Required(ErrorMessage = "Please enter your rating")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public double? Rating { get; set; }
    }
}