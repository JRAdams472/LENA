import "@testing-library/jest-dom";
import { render, screen } from "@testing-library/react";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ItemRow } from "@/app/grocery-lists/[id]/page";
import { GroceryListItem, Item } from "@/lib/types";

const queryClient = new QueryClient();

function Wrapper({ children }: { children: React.ReactNode }) {
  return <QueryClientProvider client={queryClient}>{children}</QueryClientProvider>;
}

const baseGroceryItem = (overrides: Partial<GroceryListItem> = {}): GroceryListItem => ({
  groceryListItemID: 1,
  groceryListID: 1,
  itemID: null,
  itemName: null,
  manualItemName: null,
  quantityNeeded: 1,
  unitOfMeasure: "unit",
  source: "Manual",
  isChecked: false,
  createdBy: "test",
  createDate: "2024-01-01T00:00:00Z",
  lastUpdatedBy: null,
  lastUpdatedDate: null,
  ...overrides,
});

const baseInventoryItem = (overrides: Partial<Item> = {}): Item => ({
  itemID: 1,
  name: "Milk",
  brand: null,
  upc12: null,
  upc14: null,
  categoryID: 1,
  unit: "g",
  currentQuantity: 0,
  minQuantity: null,
  purchaseDate: "2024-01-01",
  expiryDate: null,
  notes: null,
  isFavorite: false,
  category: null,
  foodNutrients: [],
  foodFlavors: [],
  createdBy: "test",
  createDate: "2024-01-01T00:00:00Z",
  lastUpdatedBy: null,
  lastUpdatedDate: null,
  ...overrides,
});

describe("ItemRow", () => {
  it("uses manualItemName when present", () => {
    const item = baseGroceryItem({
      itemID: null,
      itemName: null,
      manualItemName: "Custom Manual Entry",
    });

    render(<ItemRow item={item} items={[]} listId={1} />, { wrapper: Wrapper });

    expect(screen.getByText("Custom Manual Entry")).toBeInTheDocument();
  });

  it("falls back to the matched item's name from the items list", () => {
    const item = baseGroceryItem({
      itemID: 1,
      itemName: null,
      manualItemName: null,
    });
    const items = [baseInventoryItem({ itemID: 1, name: "Whole Milk" })];

    render(<ItemRow item={item} items={items} listId={1} />, { wrapper: Wrapper });

    expect(screen.getByText("Whole Milk")).toBeInTheDocument();
  });

  it("falls back to 'Item {itemID}' when no match is found", () => {
    const item = baseGroceryItem({
      itemID: 99,
      itemName: null,
      manualItemName: null,
    });
    const items = [baseInventoryItem({ itemID: 1, name: "Milk" })];

    render(<ItemRow item={item} items={items} listId={1} />, { wrapper: Wrapper });

    expect(screen.getByText("Item 99")).toBeInTheDocument();
  });
});
