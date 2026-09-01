import { api, ApiError } from "@/lib/api";

const mockFetch = global.fetch as jest.Mock;

function mockJson(response: object, status = 200) {
  return {
    ok: status >= 200 && status < 300,
    status,
    headers: {
      get: (name: string) =>
        name === "content-type" ? "application/json" : null,
    },
    json: async () => response,
  };
}

describe("meal plan and grocery api client", () => {
  beforeEach(() => {
    mockFetch.mockReset();
  });

  it("getMealPlan fetches the plan by id", async () => {
    mockFetch.mockResolvedValueOnce(
      mockJson({ mealPlanID: 1, planName: "Weekly" })
    );

    const plan = await api.getMealPlan(1);

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/MealPlan/plans/1",
      expect.objectContaining({
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(plan.mealPlanID).toBe(1);
    expect(plan.planName).toBe("Weekly");
  });

  it("getMealPlanNutrition fetches nutrition", async () => {
    mockFetch.mockResolvedValueOnce(
      mockJson({
        mealPlanId: 1,
        dailyTotals: [{ dayOfWeek: 0, nutrients: [] }],
        meals: [{ dayOfWeek: 0, mealType: 0, mealSlotId: 1, nutrients: [] }],
      })
    );

    const nutrition = await api.getMealPlanNutrition(1);

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/MealPlan/plans/1/nutrition",
      expect.objectContaining({
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(nutrition.mealPlanId).toBe(1);
    expect(nutrition.dailyTotals).toHaveLength(1);
  });

  it("generateGroceryList posts without a meal plan id", async () => {
    mockFetch.mockResolvedValueOnce(mockJson({ groceryListID: 1 }));

    const list = await api.generateGroceryList();

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/GroceryList/generate",
      expect.objectContaining({
        method: "POST",
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(list.groceryListID).toBe(1);
  });

  it("generateGroceryList posts with a meal plan id", async () => {
    mockFetch.mockResolvedValueOnce(mockJson({ groceryListID: 2 }));

    const list = await api.generateGroceryList(5);

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/GroceryList/generate?mealPlanId=5",
      expect.objectContaining({
        method: "POST",
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(list.groceryListID).toBe(2);
  });

  it("getGroceryLists fetches all lists", async () => {
    mockFetch.mockResolvedValueOnce(mockJson([{ groceryListID: 1 }]));

    const result = await api.getGroceryLists();

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/GroceryList",
      expect.objectContaining({
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(result).toHaveLength(1);
    expect(result[0].groceryListID).toBe(1);
  });
});
