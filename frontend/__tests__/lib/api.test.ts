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

    const result = await api.getItems();

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/Item/items",
      expect.objectContaining({
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(result).toHaveLength(1);
    expect(result[0].name).toBe("Milk");
  });

  it("getBottlesPaged calls the paged endpoint and returns data", async () => {
    mockFetch.mockResolvedValueOnce({
      ok: true,
      status: 200,
      headers: { get: (name: string) => (name === "content-type" ? "application/json" : null) },
      json: async () => ({
        items: [{ bottleID: 1, name: "Cabernet" }],
        pageNumber: 2,
        pageSize: 50,
        totalCount: 1,
        totalPages: 1,
      }),
    });

    const result = await api.getBottlesPaged(2, 50);

    expect(mockFetch).toHaveBeenCalledWith(
      "http://localhost:5059/api/Wine/bottles/paged?pageNumber=2&pageSize=50",
      expect.objectContaining({
        headers: expect.objectContaining({ Accept: "application/json" }),
      })
    );
    expect(result.items).toHaveLength(1);
    expect(result.items[0].bottleID).toBe(1);
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
