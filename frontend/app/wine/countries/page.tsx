"use client";

import CrudPage from "@/app/components/CrudPage";
import { api, asEntity } from "@/lib/api";

export default function CountriesPage() {
  return (
    <CrudPage
      title="Countries"
      queryKey={["countries"]}
      listFn={api.getCountries}
      activeOnlyFn={api.getActiveCountries}
      fields={[
        { key: "countryName", label: "Country Name" },
        { key: "isoCode", label: "ISO Code" },
        { key: "description", label: "Description" },
        { key: "isActive", label: "Active", type: "boolean" },
      ]}
      createFn={(row) => api.createCountry(asEntity(row))}
      updateFn={(row) =>
        api.updateCountry(row.countryID as number, asEntity(row))
      }
      deleteFn={(row) => api.deleteCountry(row.countryID)}
    />
  );
}
