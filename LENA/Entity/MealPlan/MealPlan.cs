using System;
using System.Collections.Generic;

using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.MealPlan
{
    public class MealPlan : AuditableEntity
    {
        public int MealPlanID { get; set; }
        public int UserID { get; set; }
        public required string PlanName { get; set; }
        public DateTime WeekStartDate { get; set; }
        public byte WeekStartDayOfWeek { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<MealSlot>? MealSlots { get; set; }
    }
}