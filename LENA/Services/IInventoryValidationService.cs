using System;
using System.Collections.Generic;
using System.Text;
using LENA.Domain.Entity.Inventory;

namespace LENA.Domain.Services
{
    public interface IInventoryValidationService
    {
        ValidationResult ValidateRequirement(Item item);
        ValidationResult ValidateRequirementForUpdate(Item existingItem, Item newItem);
        List<string> GetValidationErrors(Item item);
    }
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}
