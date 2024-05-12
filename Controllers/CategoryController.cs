using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sport_Accessories.Data;
using Sport_Accessories.Models;
using Sport_Accessories.ViewModels;

namespace Sport_Accessories.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private static Guid _category_id;
        public CategoryController(ApplicationDbContext dbContext,
                                  IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Policy = "Create")]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "Create")]
        public async Task<IActionResult> SubmitAddCategoryAsync(CategoryViewModel Input)
        {
            if(ModelState.IsValid)
            {

                var category = _mapper.Map<Category>(Input);

                await _dbContext.Categories.AddAsync(category);

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ShowCategories", "Admin");

            }

            return View("AddCategory", Input);
        }


        [Authorize(Policy = "Edit")]
        public async Task<IActionResult> EditCategoryAsync(Guid Id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c =>
                                                    c.CategoryId == Id);

            _category_id = Id;

            var category_view_model = _mapper.Map<CategoryViewModel>(category);

            return View(category_view_model);
        }

        [Authorize(Policy = "Edit")]
        public async Task<IActionResult> SubmitEditCategoryAsync(CategoryViewModel Input)
        {
            if(ModelState.IsValid)
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c =>
                                                    c.CategoryId == _category_id);

                if(category.CategoryName == Input.CategoryName &&
                        category.IsActive == Input.IsActive &&
                        category.CategoryId != _category_id)
                {

                    ModelState.AddModelError(string.Empty, "There is already a category" +
                        " with this credentials!");

                    return View(Input);
                }

                _mapper.Map(Input, category);

                await _dbContext.SaveChangesAsync();

                return RedirectToAction("ShowCategories", "Admin");
            }

            return View(Input);
        }

        [HttpPost]
        [Authorize(Policy = "Delete")]
        public async Task<IActionResult> DeleteCategoryAsync(Guid Id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c =>
                                                    c.CategoryId == Id);

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("ShowCategories", "Admin");
        }
    }
}
