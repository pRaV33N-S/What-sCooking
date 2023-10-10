using System;
using System.ComponentModel.DataAnnotations;

namespace CookingAppMVC.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a recipe name.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Please enter ingredients.")]
        public string Ingredients { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a category.")]
        public string Category { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a submission date.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime SubmissionDate { get; set; }

        [Required(ErrorMessage = "Please enter an image URL.")]
        [DataType(DataType.ImageUrl)]
        [Url(ErrorMessage = "Invalid image URL.")]
        public string ImageUrl { get; set; } = null!;
    }
}
