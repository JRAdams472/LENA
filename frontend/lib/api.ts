import {
  AuditableEntity,
  Item,
  Bottle,
  Category,
  Country,
  Region,
  WineType,
  Vintage,
  FlavorProfile,
  FoodFlavor,
  FoodNutrient,
  InStock,
  NutrientType,
  GrapeVariety,
  BottleGrapeVariety,
  BottleFlavorProfile,
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
    request<Item>(`/api/Item/items/${id}`, { method: "PUT", body: JSON.stringify(item) }),
  deleteItem: (id: number) => request<Item | null>(`/api/Item/items/${id}`, { method: "DELETE" }),

  // Inventory reference data
  getCategories: () => request<Category[]>("/api/Item/categories"),
  getFlavorProfiles: () => request<FlavorProfile[]>("/api/Item/flavorProfiles"),
  getFoodFlavors: () => request<FoodFlavor[]>("/api/Item/foodFlavors"),
  getFoodNutrients: () => request<FoodNutrient[]>("/api/Item/foodNutrients"),
  getInStock: () => request<InStock[]>("/api/Item/inStock"),
  getNutrientTypes: () => request<NutrientType[]>("/api/Item/nutrientTypes"),

  // Wine
  getBottles: () => request<Bottle[]>("/api/Wine/bottles"),
  getBottle: (id: number) => request<Bottle>(`/api/Wine/bottles/${id}`),
  createBottle: (bottle: Omit<Bottle, keyof AuditableEntity>) =>
    request<Bottle>("/api/Wine/bottles", { method: "POST", body: JSON.stringify(bottle) }),
  updateBottle: (id: number, bottle: Partial<Bottle>) =>
    request<Bottle>(`/api/Wine/bottles/${id}`, { method: "PUT", body: JSON.stringify(bottle) }),
  deleteBottle: (id: number) => request<Bottle | null>(`/api/Wine/bottles/${id}`, { method: "DELETE" }),

  // Wine reference data
  getCountries: () => request<Country[]>("/api/Wine/countries"),
  getRegions: () => request<Region[]>("/api/Wine/regions"),
  getTypes: () => request<WineType[]>("/api/Wine/types"),
  getVintages: () => request<Vintage[]>("/api/Wine/vintages"),
  getGrapeVarieties: () => request<GrapeVariety[]>("/api/Wine/grapeVarieties"),
  getBottleGrapeVarieties: () => request<BottleGrapeVariety[]>("/api/Wine/bottleGrapeVarieties"),
  getBottleFlavorProfiles: () => request<BottleFlavorProfile[]>("/api/Wine/bottleFlavorProfiles"),
};
