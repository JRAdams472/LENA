"use client";

import EntityListPage from "@/app/components/EntityListPage";
import { api } from "@/lib/api";

export default function FlavorProfilesPage() {
  return (
    <EntityListPage
      title="Flavor Profiles"
      queryKey={["flavor-profiles"]}
      queryFn={api.getFlavorProfiles}
    />
  );
}
