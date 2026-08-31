"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function VintagesPage() {
  return (
    <EntityListPage
      title="Vintages"
      queryKey={["vintages"]}
      queryFn={api.getVintages}
    />
  );
}
