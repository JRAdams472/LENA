"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function NutrientTypesPage() {
  return (
    <EntityListPage
      title="Nutrient Types"
      queryKey={["nutrient-types"]}
      queryFn={api.getNutrientTypes}
    />
  );
}
