"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function RegionsPage() {
  return (
    <EntityListPage
      title="Regions"
      queryKey={["regions"]}
      queryFn={api.getRegions}
    />
  );
}
