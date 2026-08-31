import { api, ApiError } from "@/lib/api";

const mockFetch = global.fetch as jest.Mock;

describe("api client", () => {
  beforeEach(() => {
    mockFetch.mockReset();
  });

  it("getItems calls the correct endpoint and returns data", async () => {
    mockFetch.mockResolvedValueOnce({
      ok: true,
      status: 200,
      headers: { get: (name: string) => (name === "content-type" ? "application/json" : null) },
      json: async () => [{ itemID: 1, name: "Milk" }],
    });

    const items = await api.getItems();

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/Item/items",
      expect.objectContaining({
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(items).toHaveLength(1);
    expect(items[0].name).toBe("Milk");
  });

  it("throws ApiError on non-ok responses", async () => {
    mockFetch.mockResolvedValueOnce({
      ok: false,
      status: 400,
      headers: { get: () => null },
      text: async () => "Bad Request",
    });

    const error = await api.getItems().catch((e) => e);
    expect(error).toBeInstanceOf(ApiError);
    expect((error as ApiError).status).toBe(400);
    expect((error as ApiError).message).toContain("Bad Request");
  });
});
