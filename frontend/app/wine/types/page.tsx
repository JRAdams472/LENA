"use client";

import CrudPage from "@/app/components/CrudPage";
import { api } from "@/lib/api";

export default function TypesPage() {
  return (
    <CrudPage
      title="Types"
      queryKey={["types"]}
      listFn={api.getTypes}
      fields={[
        { key: "typeName", label: "Type Name" },
        { key: "description", label: "Description" },
        { key: "isActive", label: "Active", type: "boolean" },
      ]}
      createFn={(row) => api.createType(row as any)}
      updateFn={(row) =>
        api.updateType(row.typeID as number, row as any)
      }
      deleteFn={(row) => api.deleteType(row.typeID as number)}
    />
  );
}
