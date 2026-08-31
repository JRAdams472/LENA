"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function FoodNutrientsPage() {
  return (
    <EntityListPage
      title="Food Nutrients"
      queryKey={["food-nutrients"]}
      queryFn={api.getFoodNutrients}
    />
  );
}
