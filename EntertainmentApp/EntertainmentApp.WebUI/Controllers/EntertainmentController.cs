using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EntertainmentApp.Application.Services.Interfaces;
using EntertainmentApp.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using EntertainmentApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using EntertainmentApp.Infrastructure.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EntertainmentApp.WebUI.Controllers
{
    public class EntertainmentController : Controller
    {
        private readonly IEntertainmentRepository _entertainmentRepository;
        private readonly ICreatorRepository _creatorRepository;
        private readonly IGenreRepository _genreRepository;
        private readonly IFileUploadService _fileUploadService;
        private readonly IReviewRepository _reviewRepository;  // Add review repository
        private readonly IUserLikeRepository _userLikeRepository;  // Add user like repository
        private readonly UserManager<ApplicationUser> _userManager;

        public EntertainmentController(
            IEntertainmentRepository entertainmentRepository,
            ICreatorRepository creatorRepository,
            IGenreRepository genreRepository,
            IFileUploadService fileUploadService,
            IReviewRepository reviewRepository,
            IUserLikeRepository userLikeRepository,
            UserManager<ApplicationUser> userManager)
        {
            _entertainmentRepository = entertainmentRepository;
            _creatorRepository = creatorRepository;
            _genreRepository = genreRepository;
            _fileUploadService = fileUploadService;
            _reviewRepository = reviewRepository;
            _userLikeRepository = userLikeRepository;
            _userManager = userManager;
        }


  




        public IActionResult Index()
        {
            var entertainments = _entertainmentRepository.GetAllEntertainments();
            return View(entertainments);
        }



        [HttpGet]
     
        public async Task<IActionResult> Detail(int id)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var entertainment = _entertainmentRepository.GetEntertainmentById(id);
            if (entertainment == null)
            {
                return NotFound();
            }
            EntertainmentDetailViewModel viewModel;
            var reviews = await _reviewRepository.GetReviewsByEntertainmentIdAsync(id);
            double? averageRating = reviews.Any() ? (double?)reviews.Average(r => r.Rating) : null;


            if (userId != null)
            {



                 viewModel = new EntertainmentDetailViewModel
                {
                    Entertainment = entertainment,
                    Reviews = reviews,
                    NewReview = new Review() { EntertainmentId = id },
                    UserName = user.FirstName + user.LastName,
                    AverageRating = averageRating
                 };
            } else
            {
                 viewModel = new EntertainmentDetailViewModel
                {
                    Entertainment = entertainment,
                    Reviews = reviews,
                    NewReview = new Review() { EntertainmentId = id },
                   
                };
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview(EntertainmentDetailViewModel viewModel)
        {
            
            var user = await _userManager.GetUserAsync(User);

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = Url.Action(nameof(Detail), new { id = viewModel.NewReview.EntertainmentId }) });
            }

            var userId = _userManager.GetUserId(User);
            var thisuser = await _userManager.FindByIdAsync(userId);

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

              
                viewModel.Reviews = await _reviewRepository.GetReviewsByEntertainmentIdAsync(viewModel.NewReview.EntertainmentId);
                return View("Detail", viewModel);
            }

            var review = new Review
            {
                EntertainmentId = viewModel.NewReview.EntertainmentId,
                Comment = viewModel.NewReview.Comment,
                Rating = viewModel.NewReview.Rating,
                UserId = userId,
                IsDeleted = false,
                reviewerName = $"{thisuser.FirstName} {thisuser.LastName}"
            };

            await _reviewRepository.AddReviewAsync(review);

            return RedirectToAction(nameof(Detail), new { id = viewModel.NewReview.EntertainmentId });
        }


  
        [HttpPost]
        public async Task<IActionResult> LikeReview(int reviewId, int entertainmentId)
        {
          
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", ReturnUrl = Url.Action(nameof(Detail), new { id = entertainmentId}) });
            }

            var userId = _userManager.GetUserId(User);
            var userLike = _userLikeRepository.GetUserLikeByReviewAndUser(reviewId, userId);

            if (userLike == null)
            {
                userLike = new UserLike
                {
                    ReviewId = reviewId,
                    UserId = userId,
                    IsLiked = true,
                    IsDeleted = false
                };
                _userLikeRepository.AddUserLike(userLike);
            }
            else
            {
                userLike.IsLiked = !userLike.IsLiked;
                _userLikeRepository.UpdateUserLike(userLike);
            }

            return RedirectToAction(nameof(Detail), new { id = entertainmentId });
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int reviewId, int entertainmentId)
        {
            var review = await _reviewRepository.GetReviewByIdAsync(reviewId);
            if (review == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var canManageCatalog = User.HasClaim(c => c.Type == "CanManageCatalog" && c.Value == "true");

            if (review.UserId != userId && !canManageCatalog)
            {
                return Forbid();
            }

            await _reviewRepository.DeleteReviewAsync(reviewId);

            return RedirectToAction(nameof(Detail), new { id = entertainmentId });
        }




        [HttpGet]
        [Authorize(Policy = "ManageCatalogPolicy")]
        public IActionResult Create()
        {
            var viewModel = new EntertainmentViewModel
            {
                Genres = GetGenresSelectList(),
                Creators = GetCreatorsSelectList(),
                Categories = GetCategoriesSelectList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageCatalogPolicy")]
        public IActionResult Create(EntertainmentViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Error: {error.ErrorMessage}");
                }

                // Repopulate select lists in case of error
                viewModel.Genres = GetGenresSelectList();
                viewModel.Creators = GetCreatorsSelectList();
                viewModel.Categories = GetCategoriesSelectList();

                return View(viewModel); // Return the Create view with validation errors
            }

            var entertainment = new Entertainment
            {
                Title = viewModel.Title,
                Description = viewModel.Description,
                GenreId = viewModel.GenreId,
                CreatorId = viewModel.CreatorId,
                ReleaseDate = viewModel.ReleaseDate,
                Category = (EntertainmentCategory)viewModel.Category,
                IsDeleted = false
            };

            // Handle image file upload if needed
            if (viewModel.ImageFile != null)
            {
                var fileName = viewModel.ImageFile.FileName;
                using (var stream = viewModel.ImageFile.OpenReadStream())
                {
                    _fileUploadService.UploadFile(stream, fileName);
                }
                entertainment.ImageURL = _fileUploadService.GetLocalFilePath();
            }
            else
            {
                entertainment.ImageURL = "uploads\\default-movie.png";
            }

            _entertainmentRepository.AddEntertainment(entertainment);

            return RedirectToAction(nameof(Index));
        }







        [HttpGet]
        [Authorize(Policy = "ManageCatalogPolicy")]
        public IActionResult Edit(int id)
        {
            var entertainment = _entertainmentRepository.GetEntertainmentById(id);
            if (entertainment == null)
            {
                return NotFound();
            }

            var viewModel = new EntertainmentViewModel
            {
                Id = entertainment.Id,
                Title = entertainment.Title,
                Description = entertainment.Description,
                GenreId = entertainment.GenreId,
                CreatorId = entertainment.CreatorId,
                ReleaseDate = entertainment.ReleaseDate,
                ImageURL = entertainment.ImageURL,
                Category = entertainment.Category,
                Categories = GetCategoriesSelectList(),
                Genres = GetGenresSelectList(),
                Creators = GetCreatorsSelectList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageCatalogPolicy")]
        public IActionResult Edit(EntertainmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entertainment = new Entertainment
                {
                    Id = viewModel.Id,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    GenreId = viewModel.GenreId,
                    CreatorId = viewModel.CreatorId,
                    ReleaseDate = viewModel.ReleaseDate,
                    Category = (EntertainmentCategory)viewModel.Category,
                    ImageURL = viewModel.ImageURL,
                    IsDeleted = false // Ensure IsDeleted remains false on update
                };

                // Handle image file upload if needed
                if (viewModel.ImageFile != null)
                {
                    var fileName = viewModel.ImageFile.FileName;
                    using (var stream = viewModel.ImageFile.OpenReadStream())
                    {
                        _fileUploadService.UploadFile(stream, fileName);
                    }
                    entertainment.ImageURL = _fileUploadService.GetLocalFilePath();
                }

                _entertainmentRepository.UpdateEntertainment(entertainment);

                return RedirectToAction(nameof(Detail), new { id = viewModel.Id });
            }

            // If model state is not valid, re-populate select lists
            viewModel.Categories = GetCategoriesSelectList();
            viewModel.Genres = GetGenresSelectList();
            viewModel.Creators = GetCreatorsSelectList();

            return View(viewModel);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "ManageCatalogPolicy")]
        public IActionResult Delete(int id)
        {
            var entertainment = _entertainmentRepository.GetEntertainmentById(id);
            if (entertainment == null)
            {
                return NotFound();
            }

            _entertainmentRepository.DeleteEntertainment(id);

            return RedirectToAction(nameof(Index));
        }

        private IEnumerable<SelectListItem> GetGenresSelectList()
        {
            var genres = _genreRepository.GetAllGenres()
                .Select(g => new SelectListItem
                {
                    Value = g.Id.ToString(),
                    Text = g.Name
                });
            return new SelectList(genres, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetCreatorsSelectList()
        {
            var creators = _creatorRepository.GetAllCreators()
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
            return new SelectList(creators, "Value", "Text");
        }

        private IEnumerable<SelectListItem> GetCategoriesSelectList()
        {
            var categories = Enum.GetValues(typeof(EntertainmentCategory))
                .Cast<EntertainmentCategory>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                });
            return new SelectList(categories, "Value", "Text");
        }
    }
}
