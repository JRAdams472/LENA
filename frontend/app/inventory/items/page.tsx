"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function ItemsPage() {
  return (
    <EntityListPage
      title="Items"
      queryKey={["items"]}
      queryFn={api.getItems}
    />
  );
}
