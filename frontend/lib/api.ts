import {
  AuditableEntity,
  Item,
  Bottle,
  Country,
  Region,
  WineType,
  Vintage,
  FlavorProfile,
  FoodFlavor,
  FoodNutrient,
  NutrientType,
  GrapeVariety,
  BottleGrapeVariety,
  BottleFlavorProfile,
  Recipe,
  RecipeItem,
  RecipeStep,
} from "./types";

const API_BASE_URL =
  process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5059";

export class ApiError extends Error {
  status: number;

  constructor(status: number, message: string) {
    super(message);
    this.status = status;
    this.name = "ApiError";
  }
}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const url = `${API_BASE_URL}${path}`;

  const res = await fetch(url, {
    ...options,
    headers: {
      Accept: "application/json",
      ...(options?.body ? { "Content-Type": "application/json" } : {}),
      ...options?.headers,
    },
  });

  if (!res.ok) {
    const text = await res.text().catch(() => "");
    throw new ApiError(res.status, text || `HTTP ${res.status}`);
  }

  if (res.status === 204) return undefined as T;

  const contentType = res.headers.get("content-type");
  if (contentType && contentType.includes("application/json")) {
    return res.json() as Promise<T>;
  }

  throw new ApiError(res.status, "Expected JSON response");
}

export const api = {
  get: <T>(path: string) => request<T>(path),

  post: <T>(path: string, body: unknown) =>
    request<T>(path, {
      method: "POST",
      body: JSON.stringify(body),
    }),

  put: <T>(path: string, body: unknown) =>
    request<T>(path, {
      method: "PUT",
      body: JSON.stringify(body),
    }),

  remove: <T>(path: string) =>
    request<T>(path, {
      method: "DELETE",
    }),

  // Items
  getItems: () => request<Item[]>("/api/Item/items"),
  getItem: (id: number) => request<Item>(`/api/Item/items/${id}`),
  createItem: (item: Omit<Item, keyof AuditableEntity>) =>
    request<Item>("/api/Item/items", { method: "POST", body: JSON.stringify(item) }),
  updateItem: (id: number, item: Partial<Item>) =>
    request<Item>(`/api/Item/items/${id}`, { method: "PUT", body: JSON.stringify({ ...item, itemID: id }) }),
  deleteItem: (id: number) => request<Item | null>(`/api/Item/items/${id}`, { method: "DELETE" }),

  changeItemCategory: (id: number, categoryId: number) =>
    request<void>(`/api/Item/items/${id}/category/${categoryId}`, { method: "POST" }),
  setItemUPC12: (id: number, upc12: string) =>
    request<void>(`/api/Item/items/${id}/upc12`, { method: "POST", body: JSON.stringify(upc12) }),
  setItemUPC14: (id: number, upc14: string) =>
    request<void>(`/api/Item/items/${id}/upc14`, { method: "POST", body: JSON.stringify(upc14) }),
  adjustItemQuantity: (id: number, quantity: number, purchaseDate?: string) => {
    const qs = new URLSearchParams({ quantity: String(quantity) });
    if (purchaseDate) qs.append("purchaseDate", purchaseDate);
    return request<void>(`/api/Item/items/${id}/quantity?${qs.toString()}`, { method: "POST" });
  },
  setItemFavorite: (id: number, isFavorite: boolean) =>
    request<void>(`/api/Item/items/${id}/favorite?isFavorite=${isFavorite}`, { method: "POST" }),

  // Inventory reference data
  getFlavorProfiles: () => request<FlavorProfile[]>("/api/Item/flavorprofiles"),
  getActiveFlavorProfiles: () => request<FlavorProfile[]>("/api/Item/flavorprofiles/active"),
  createFlavorProfile: (profile: Omit<FlavorProfile, keyof AuditableEntity>) =>
    request<FlavorProfile>("/api/Item/flavorprofiles", { method: "POST", body: JSON.stringify(profile) }),
  updateFlavorProfile: (id: number, profile: Partial<FlavorProfile>) =>
    request<FlavorProfile>(`/api/Item/flavorprofiles/${id}`, { method: "PUT", body: JSON.stringify({ ...profile, flavorId: id }) }),
  deleteFlavorProfile: (id: number) => request<FlavorProfile | null>(`/api/Item/flavorprofiles/${id}`, { method: "DELETE" }),

  getFoodFlavors: () => request<FoodFlavor[]>("/api/Item/foodflavors"),
  createFoodFlavor: (foodFlavor: Omit<FoodFlavor, keyof AuditableEntity>) =>
    request<FoodFlavor>("/api/Item/foodflavors", { method: "POST", body: JSON.stringify(foodFlavor) }),
  updateFoodFlavor: (foodId: number, flavorId: number, foodFlavor: Partial<FoodFlavor>) =>
    request<FoodFlavor>(`/api/Item/foodflavors/food/${foodId}/flavor/${flavorId}`, { method: "PUT", body: JSON.stringify(foodFlavor) }),
  deleteFoodFlavor: (foodId: number, flavorId: number) =>
    request<FoodFlavor | null>(`/api/Item/foodflavors/food/${foodId}/flavor/${flavorId}`, { method: "DELETE" }),

  getFoodNutrients: () => request<FoodNutrient[]>("/api/Item/foodnutrients"),
  createFoodNutrient: (foodNutrient: Omit<FoodNutrient, keyof AuditableEntity>) =>
    request<FoodNutrient>("/api/Item/foodnutrients", { method: "POST", body: JSON.stringify(foodNutrient) }),
  updateFoodNutrient: (foodId: number, nutrientId: number, foodNutrient: Partial<FoodNutrient>) =>
    request<FoodNutrient>(`/api/Item/foodnutrients/food/${foodId}/nutrient/${nutrientId}`, { method: "PUT", body: JSON.stringify(foodNutrient) }),
  deleteFoodNutrient: (foodId: number, nutrientId: number) =>
    request<FoodNutrient | null>(`/api/Item/foodnutrients/food/${foodId}/nutrient/${nutrientId}`, { method: "DELETE" }),

  getNutrientTypes: () => request<NutrientType[]>("/api/Item/nutrienttypes"),
  createNutrientType: (nutrientType: Omit<NutrientType, keyof AuditableEntity>) =>
    request<NutrientType>("/api/Item/nutrienttypes", { method: "POST", body: JSON.stringify(nutrientType) }),
  updateNutrientType: (id: number, nutrientType: Partial<NutrientType>) =>
    request<NutrientType>(`/api/Item/nutrienttypes/${id}`, { method: "PUT", body: JSON.stringify({ ...nutrientType, nutrientId: id }) }),
  deleteNutrientType: (id: number) => request<NutrientType | null>(`/api/Item/nutrienttypes/${id}`, { method: "DELETE" }),

  // Wine
  getBottles: () => request<Bottle[]>("/api/Wine/bottles"),
  getBottle: (id: number) => request<Bottle>(`/api/Wine/bottles/${id}`),
  createBottle: (bottle: Omit<Bottle, keyof AuditableEntity>) =>
    request<Bottle>("/api/Wine/bottles", { method: "POST", body: JSON.stringify(bottle) }),
  updateBottle: (id: number, bottle: Partial<Bottle>) =>
    request<Bottle>(`/api/Wine/bottles/${id}`, { method: "PUT", body: JSON.stringify({ ...bottle, bottleID: id }) }),
  deleteBottle: (id: number) => request<Bottle | null>(`/api/Wine/bottles/${id}`, { method: "DELETE" }),

  getBottlesByCountryId: (countryId: number) => request<Bottle[]>(`/api/Wine/bottles/country/${countryId}`),
  getBottlesByRegionId: (regionId: number) => request<Bottle[]>(`/api/Wine/bottles/region/${regionId}`),
  getBottlesByTypeId: (typeId: number) => request<Bottle[]>(`/api/Wine/bottles/type/${typeId}`),
  getBottlesByVintageYear: (year: number) => request<Bottle[]>(`/api/Wine/bottles/vintage/${year}`),
  getFavoriteBottles: () => request<Bottle[]>("/api/Wine/bottles/favorites"),
  searchBottles: (searchTerm: string) => request<Bottle[]>(`/api/Wine/bottles/search?searchTerm=${encodeURIComponent(searchTerm)}`),
  getBottleCount: () => request<number>("/api/Wine/bottles/count"),

  // Wine reference data
  getCountries: () => request<Country[]>("/api/Wine/countries"),
  getActiveCountries: () => request<Country[]>("/api/Wine/countries/active"),
  createCountry: (country: Omit<Country, keyof AuditableEntity>) =>
    request<Country>("/api/Wine/countries", { method: "POST", body: JSON.stringify(country) }),
  updateCountry: (id: number, country: Partial<Country>) =>
    request<Country>(`/api/Wine/countries/${id}`, { method: "PUT", body: JSON.stringify({ ...country, countryID: id }) }),
  deleteCountry: (id: number) => request<Country | null>(`/api/Wine/countries/${id}`, { method: "DELETE" }),

  getRegions: () => request<Region[]>("/api/Wine/regions"),
  getRegionsByCountryId: (countryId: number) => request<Region[]>(`/api/Wine/regions/country/${countryId}`),
  createRegion: (region: Omit<Region, keyof AuditableEntity>) =>
    request<Region>("/api/Wine/regions", { method: "POST", body: JSON.stringify(region) }),
  updateRegion: (id: number, region: Partial<Region>) =>
    request<Region>(`/api/Wine/regions/${id}`, { method: "PUT", body: JSON.stringify({ ...region, regionID: id }) }),
  deleteRegion: (id: number) => request<Region | null>(`/api/Wine/regions/${id}`, { method: "DELETE" }),

  getTypes: () => request<WineType[]>("/api/Wine/types"),
  createType: (type: Omit<WineType, keyof AuditableEntity>) =>
    request<WineType>("/api/Wine/types", { method: "POST", body: JSON.stringify(type) }),
  updateType: (id: number, type: Partial<WineType>) =>
    request<WineType>(`/api/Wine/types/${id}`, { method: "PUT", body: JSON.stringify({ ...type, typeID: id }) }),
  deleteType: (id: number) => request<WineType | null>(`/api/Wine/types/${id}`, { method: "DELETE" }),

  getVintages: () => request<Vintage[]>("/api/Wine/vintages"),
  getActiveVintages: () => request<Vintage[]>("/api/Wine/vintages/active"),
  createVintage: (vintage: Omit<Vintage, keyof AuditableEntity>) =>
    request<Vintage>("/api/Wine/vintages", { method: "POST", body: JSON.stringify(vintage) }),
  updateVintage: (id: number, vintage: Partial<Vintage>) =>
    request<Vintage>(`/api/Wine/vintages/${id}`, { method: "PUT", body: JSON.stringify({ ...vintage, vintageID: id }) }),
  deleteVintage: (id: number) => request<Vintage | null>(`/api/Wine/vintages/${id}`, { method: "DELETE" }),

  // Recipes
  getRecipes: () => request<Recipe[]>("/api/Recipe/recipes"),
  getRecipe: (id: number) => request<Recipe>(`/api/Recipe/recipes/${id}`),
  createRecipe: (recipe: Omit<Recipe, keyof AuditableEntity>) =>
    request<Recipe>("/api/Recipe/recipes", { method: "POST", body: JSON.stringify(recipe) }),
  updateRecipe: (id: number, recipe: Partial<Recipe>) =>
    request<Recipe>(`/api/Recipe/recipes/${id}`, { method: "PUT", body: JSON.stringify({ ...recipe, recipeID: id }) }),
  deleteRecipe: (id: number) => request<Recipe | null>(`/api/Recipe/recipes/${id}`, { method: "DELETE" }),

  getRecipeItems: (recipeId: number) => request<RecipeItem[]>(`/api/Recipe/recipes/${recipeId}/items`),
  addRecipeItem: (recipeId: number, item: { itemId: number; portion: number; unit: string | null }) =>
    request<RecipeItem>(`/api/Recipe/recipes/${recipeId}/items`, { method: "POST", body: JSON.stringify(item) }),
  removeRecipeItem: (recipeId: number, itemId: number) =>
    request<void>(`/api/Recipe/recipes/${recipeId}/items/${itemId}`, { method: "DELETE" }),

  getRecipeSteps: (recipeId: number) => request<RecipeStep[]>(`/api/Recipe/recipes/${recipeId}/steps`),
  addRecipeStep: (recipeId: number, step: { stepNumber: number; instruction: string }) =>
    request<RecipeStep>(`/api/Recipe/recipes/${recipeId}/steps`, { method: "POST", body: JSON.stringify(step) }),
  updateRecipeStep: (recipeId: number, stepId: number, step: { stepNumber: number; instruction: string }) =>
    request<RecipeStep>(`/api/Recipe/recipes/${recipeId}/steps/${stepId}`, { method: "PUT", body: JSON.stringify(step) }),
  deleteRecipeStep: (recipeId: number, stepId: number) =>
    request<void>(`/api/Recipe/recipes/${recipeId}/steps/${stepId}`, { method: "DELETE" }),
};
