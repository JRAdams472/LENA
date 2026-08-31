"use client";

import CrudPage from "@/app/components/CrudPage";
import { api } from "@/lib/api";

export default function FoodFlavorsPage() {
  return (
    <CrudPage
      title="Food Flavors"
      queryKey={["food-flavors"]}
      listFn={api.getFoodFlavors}
      fields={[
        { key: "foodId", label: "Food ID", type: "number" },
        { key: "flavorId", label: "Flavor ID", type: "number" },
        { key: "intensityScore", label: "Intensity Score", type: "number" },
      ]}
      createFn={(row) => api.createFoodFlavor(row as any)}
      updateFn={(row) =>
        api.updateFoodFlavor(
          row.foodId as number,
          row.flavorId as number,
          row as any
        )
      }
      deleteFn={(row) =>
        api.deleteFoodFlavor(row.foodId as number, row.flavorId as number)
      }
    />
  );
}
