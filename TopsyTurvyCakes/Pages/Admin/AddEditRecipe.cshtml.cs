using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{
    [Authorize]
    public class AddEditRecipeModel : PageModel
    {
        [FromRoute]
        public long? Id { get; set; }
        public bool IsNewRecipe
        {
            get { return Id == null; }
        }
        [BindProperty]
        public Recipe Recipe { get; set; }
        //[BindProperty]
        //public Image Image { get; set; }
        public IRecipesService recipesService { get; set; }
        [BindProperty]
        public IFormFile Image { get; set; }
        public AddEditRecipeModel(IRecipesService recipesService)
        {
            this.recipesService = recipesService;
        }
        [HttpGet]
        public async Task OnGetAsync()
        {
            Recipe = await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();
        }
        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Recipe.Id = Id.GetValueOrDefault();
            var recipe = await recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();

            recipe.Name = Recipe.Name;
            recipe.Description = Recipe.Description;
            recipe.Ingredients = Recipe.Ingredients;
            recipe.Directions = Recipe.Directions;
            if (Image != null)
            {
                recipe.SetImage(Image);
            }
            await recipesService.SaveAsync(recipe);
            return RedirectToPage("/Recipe", new { id = recipe.Id });
        }
        public async Task<IActionResult> OnPostDelete()
        {
            await recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }
    }
}