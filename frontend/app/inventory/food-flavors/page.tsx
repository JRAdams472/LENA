"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function FoodFlavorsPage() {
  return (
    <EntityListPage
      title="Food Flavors"
      queryKey={["food-flavors"]}
      queryFn={api.getFoodFlavors}
    />
  );
}
