"use client";

import CrudPage from "@/app/components/CrudPage";
import { api } from "@/lib/api";

export default function FlavorProfilesPage() {
  return (
    <CrudPage
      title="Flavor Profiles"
      queryKey={["flavor-profiles"]}
      listFn={api.getFlavorProfiles}
      activeOnlyFn={api.getActiveFlavorProfiles}
      fields={[
        { key: "flavorName", label: "Flavor Name" },
        { key: "description", label: "Description" },
        { key: "isActive", label: "Active", type: "boolean" },
      ]}
      createFn={(row) => api.createFlavorProfile(row as any)}
      updateFn={(row) =>
        api.updateFlavorProfile(row.flavorId as number, row as any)
      }
      deleteFn={(row) => api.deleteFlavorProfile(row.flavorId as number)}
    />
  );
}
