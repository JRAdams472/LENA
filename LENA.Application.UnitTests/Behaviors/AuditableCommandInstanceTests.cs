using LENA.Domain.Entity.Common;

using LENA.Application.Features.Grocery.GroceryLists.Commands;
using LENA.Application.Features.Inventory.Items.Commands;
using Xunit;

namespace LENA.Application.UnitTests.Behaviors
{
    /// <summary>
    /// The auditing behavior stamps <c>AuditableEntity</c> before the handler runs, so the property
    /// has to hand back the same instance every time it is read.
    /// </summary>
    public class AuditableCommandInstanceTests
    {
        [Fact]
        public void GenerateGroceryListCommand_Should_Expose_A_Stable_AuditableEntity()
        {
            var command = new GenerateGroceryListCommand(1);
            command.AuditableEntity.CreatedBy = "tester";

Assert.Same(command.AuditableEntity,             command.AuditableEntity);
Assert.Equal("tester",             command.AuditableEntity.CreatedBy);
        }

        [Fact]
        public void ToggleGroceryListItemCheckedCommand_Should_Expose_A_Stable_AuditableEntity()
        {
            var command = new ToggleGroceryListItemCheckedCommand(1);
            command.AuditableEntity.LastUpdatedBy = "tester";

Assert.Same(command.AuditableEntity,             command.AuditableEntity);
Assert.Equal("tester",             command.AuditableEntity.LastUpdatedBy);
        }

        [Fact]
        public void AdjustItemQuantityCommand_Should_Expose_A_Stable_AuditableEntity()
        {
            var command = new AdjustItemQuantityCommand(1, 0m, null);
            command.AuditableEntity.LastUpdatedBy = "tester";

Assert.Same(command.AuditableEntity,             command.AuditableEntity);
Assert.Equal("tester",             command.AuditableEntity.LastUpdatedBy);
        }
    }
}
