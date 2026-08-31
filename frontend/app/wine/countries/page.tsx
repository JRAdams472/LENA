"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function CountriesPage() {
  return (
    <EntityListPage
      title="Countries"
      queryKey={["countries"]}
      queryFn={api.getCountries}
    />
  );
}
