using System;
using System.Collections.Generic;

using LENA.Domain.Entity.Common;

namespace LENA.Domain.Entity.Grocery
{
    public class GroceryList : AuditableEntity
    {
        public int GroceryListID { get; set; }
        public int UserID { get; set; }
        public int? MealPlanID { get; set; }
        public DateTime GeneratedDate { get; set; }

        public ICollection<GroceryListItem>? GroceryListItems { get; set; }
    }
}