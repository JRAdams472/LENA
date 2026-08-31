export interface AuditableEntity {
  createdBy: string;
  createDate: string;
  lastUpdatedBy: string | null;
  lastUpdatedDate: string | null;
}

export interface Category extends AuditableEntity {
  categoryID: number;
  categoryName: string;
  description: string | null;
  isActive: boolean;
}

export interface FlavorProfile extends AuditableEntity {
  flavorId: number;
  flavorName: string;
  foodFlavors: FoodFlavor[] | null;
}

export interface FoodFlavor extends AuditableEntity {
  foodId: number;
  flavorId: number;
  intensityScore: number;
  item: Item | null;
  flavorProfile: FlavorProfile | null;
}

export interface FoodNutrient extends AuditableEntity {
  foodId: number;
  nutrientId: number;
  amountPerServing: number;
  nutrientType: NutrientType | null;
}

export interface InStock extends AuditableEntity {
  stockID: number;
  itemID: number;
  quantityOnHand: number;
  lastUpdated: string;
  item: Item | null;
}

export interface Item extends AuditableEntity {
  itemID: number;
  name: string;
  brand: string | null;
  upc12: string | null;
  upc14: string | null;
  categoryID: number;
  unit: string;
  currentQuantity: number;
  minQuantity: number | null;
  purchaseDate: string;
  expiryDate: string | null;
  notes: string | null;
  isFavorite: boolean;
  category: Category | null;
  foodNutrients: FoodNutrient[] | null;
  foodFlavors: FoodFlavor[] | null;
}

export interface NutrientType extends AuditableEntity {
  nutrientId: number;
  nutrientName: string;
  unitOfMeasure: string;
}

export interface Bottle extends AuditableEntity {
  bottleID: number;
  bottleNumber: number | null;
  typeID: number;
  countryID: number;
  regionID: number;
  vintageYear: number;
  vineyard: string | null;
  abv: number | null;
  acidity: number | null;
  tanninLevel: number | null;
  body: number | null;
  sweetness: number | null;
  oakIntegration: boolean | null;
  bottleSize: string;
  quantity: number;
  purchaseDate: string;
  purchasePrice: number | null;
  storageTemp: number | null;
  location: string | null;
  notes: string | null;
  isFavorite: boolean;
  type: WineType | null;
  country: Country | null;
  region: Region | null;
  vintage: Vintage | null;
  bottleGrapeVarieties: BottleGrapeVariety[];
  bottleFlavorProfiles: BottleFlavorProfile[];
}

export interface BottleFlavorProfile extends AuditableEntity {
  flavorProfileID: number;
  flavorProfileName: string;
  description: string | null;
  isActive: boolean;
}

export interface BottleGrapeVariety extends AuditableEntity {
  bottleID: number;
  grapeVarietyID: number;
  percentage: number | null;
  bottle: Bottle;
  grapeVariety: GrapeVariety;
}

export interface Country extends AuditableEntity {
  countryID: number;
  countryName: string;
  isoCode: string;
  description: string | null;
  isActive: boolean;
  regions: Region[];
  bottles: Bottle[];
}

export interface GrapeVariety extends AuditableEntity {
  grapeVarietyID: number;
  grapeVarietyName: string;
  description: string | null;
  isActive: boolean;
  bottleGrapeVarieties: BottleGrapeVariety[];
}

export interface Region extends AuditableEntity {
  regionID: number;
  regionName: string;
  description: string | null;
  isActive: boolean;
  country: Country | null;
  bottles: Bottle[];
}

export interface WineType extends AuditableEntity {
  typeID: number;
  typeName: string;
  description: string | null;
  isActive: boolean;
  bottles: Bottle[];
}

export interface Vintage extends AuditableEntity {
  vintageID: number;
  year: number;
  description: string | null;
  isActive: boolean;
  bottles: Bottle[];
}
