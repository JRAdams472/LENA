using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Recipe
{
    public class RecipeStep : AuditableEntity
    {
        public int RecipeStepID { get; set; }
        public int RecipeID { get; set; }
        public int StepNumber { get; set; }
        public required string Instruction { get; set; }

        public Recipe? Recipe { get; set; }
    }
}