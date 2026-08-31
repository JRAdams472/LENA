"use client";

import CrudPage from "@/app/components/CrudPage";
import { api } from "@/lib/api";

export default function RegionsPage() {
  return (
    <CrudPage
      title="Regions"
      queryKey={["regions"]}
      listFn={api.getRegions}
      filterBy={{
        label: "Country",
        optionsFn: async () =>
          (await api.getCountries()).map((c) => ({
            id: c.countryID,
            name: c.countryName,
          })),
        filterFn: (countryId) => api.getRegionsByCountryId(countryId),
      }}
      fields={[
        { key: "regionName", label: "Region Name" },
        { key: "description", label: "Description" },
        { key: "countryID", label: "Country ID", type: "number" },
        { key: "isActive", label: "Active", type: "boolean" },
      ]}
      createFn={(row) => api.createRegion(row as any)}
      updateFn={(row) =>
        api.updateRegion(row.regionID as number, row as any)
      }
      deleteFn={(row) => api.deleteRegion(row.regionID as number)}
    />
  );
}
