"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function TypesPage() {
  return (
    <EntityListPage
      title="Types"
      queryKey={["types"]}
      queryFn={api.getTypes}
    />
  );
}
