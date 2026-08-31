"use client";

import CrudPage from "@/app/components/CrudPage";
import { api } from "@/lib/api";

export default function NutrientTypesPage() {
  return (
    <CrudPage
      title="Nutrient Types"
      queryKey={["nutrient-types"]}
      listFn={api.getNutrientTypes}
      fields={[
        { key: "nutrientName", label: "Nutrient Name" },
        { key: "unitOfMeasure", label: "Unit of Measure" },
      ]}
      createFn={(row) => api.createNutrientType(row as any)}
      updateFn={(row) =>
        api.updateNutrientType(row.nutrientId as number, row as any)
      }
      deleteFn={(row) => api.deleteNutrientType(row.nutrientId as number)}
    />
  );
}
