using EntertainmentApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntertainmentApp.WebUI.Models
{
    public class EntertainmentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title must be at most 200 characters long")]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        [Required(ErrorMessage = "Creator is required")]
        [Display(Name = "Creator")]
        public int CreatorId { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public EntertainmentCategory? Category { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; } = DateTime.MinValue;

        [Display(Name = "Image URL")]
        public string? ImageURL { get; set; }


        [Display(Name = "Image File")]
        public IFormFile? ImageFile { get; set; }

        public string? NewCreatorName { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? Genres { get; set; }
        public IEnumerable<SelectListItem>? Creators { get; set; }
    }
}
