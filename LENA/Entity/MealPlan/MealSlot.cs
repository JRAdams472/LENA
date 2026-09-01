using System;
using System.Collections.Generic;
using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.MealPlan
{
    public class MealSlot : AuditableEntity
    {
        public int MealSlotID { get; set; }
        public int MealPlanID { get; set; }
        public byte DayOfWeek { get; set; }
        public byte MealType { get; set; }
        public int? RecipeID { get; set; }
        public decimal Servings { get; set; } = 1m;
        public string? ReplacementNote { get; set; }

        public MealPlan? MealPlan { get; set; }
        public Recipe.Recipe? Recipe { get; set; }
        public ICollection<MealSlotItem>? MealSlotItems { get; set; }
    }
}
