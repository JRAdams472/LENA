"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function BottlesPage() {
  return (
    <EntityListPage
      title="Bottles"
      queryKey={["bottles"]}
      queryFn={api.getBottles}
    />
  );
}
