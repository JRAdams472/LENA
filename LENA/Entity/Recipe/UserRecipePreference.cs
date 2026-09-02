using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Recipe
{
    public class UserRecipePreference : AuditableEntity
    {
        public int UserID { get; set; }
        public int RecipeID { get; set; }
        public bool IsFavorite { get; set; } = false;
    }
}
