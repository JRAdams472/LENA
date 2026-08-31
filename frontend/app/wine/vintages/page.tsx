"use client";

import CrudPage from "@/app/components/CrudPage";
import { api } from "@/lib/api";

export default function VintagesPage() {
  return (
    <CrudPage
      title="Vintages"
      queryKey={["vintages"]}
      listFn={api.getVintages}
      activeOnlyFn={api.getActiveVintages}
      fields={[
        { key: "year", label: "Year", type: "number" },
        { key: "description", label: "Description" },
        { key: "isActive", label: "Active", type: "boolean" },
      ]}
      createFn={(row) => api.createVintage(row as any)}
      updateFn={(row) =>
        api.updateVintage(row.vintageID as number, row as any)
      }
      deleteFn={(row) => api.deleteVintage(row.vintageID as number)}
    />
  );
}
