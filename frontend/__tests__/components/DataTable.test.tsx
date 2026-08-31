import "@testing-library/jest-dom";
import { render, screen, fireEvent } from "@testing-library/react";
import DataTable from "@/app/components/DataTable";

const rows = [
  { itemID: 1, name: "Milk", isFavorite: false },
  { itemID: 2, name: "Cheese", isFavorite: true },
];

describe("DataTable", () => {
  it("renders title, rows, and the create button", () => {
    const onCreate = jest.fn();
    const onEdit = jest.fn();
    const onDelete = jest.fn();

    render(
      <DataTable
        title="Items"
        rows={rows}
        isLoading={false}
        error={null}
        onCreate={onCreate}
        onEdit={onEdit}
        onDelete={onDelete}
      />
    );

    expect(screen.getByRole("heading", { name: "Items" })).toBeInTheDocument();
    expect(screen.getByText("Milk")).toBeInTheDocument();
    expect(screen.getByText("Cheese")).toBeInTheDocument();

    fireEvent.click(screen.getByRole("button", { name: /create/i }));
    expect(onCreate).toHaveBeenCalledTimes(1);
  });

  it("shows no-data message when rows is empty", () => {
    render(
      <DataTable
        title="Empty"
        rows={[]}
        isLoading={false}
        error={null}
        onCreate={jest.fn()}
        onEdit={jest.fn()}
        onDelete={jest.fn()}
      />
    );

    expect(screen.getByText("No data")).toBeInTheDocument();
  });

  it("renders an error alert", () => {
    const error = new Error("Connection failed");
    render(
      <DataTable
        title="Items"
        rows={[]}
        isLoading={false}
        error={error}
        onCreate={jest.fn()}
        onEdit={jest.fn()}
        onDelete={jest.fn()}
      />
    );

    expect(screen.getByRole("alert")).toHaveTextContent("Connection failed");
  });
});
