using System.Collections.Generic;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Recipe
{
    public class Recipe : AuditableEntity
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string? Description { get; set; }
        public int? Servings { get; set; }
        public int? PrepTimeMinutes { get; set; }
        public int? CookTimeMinutes { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<RecipeItem> RecipeItems { get; set; } = new List<RecipeItem>();
        public ICollection<RecipeStep> RecipeSteps { get; set; } = new List<RecipeStep>();
    }
}
