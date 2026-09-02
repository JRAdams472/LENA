# LENA Grocery Android

A small Android app that lets you read an existing grocery list, check off items as you purchase them, and save the results back to the LENA API.

## Features

- Load a grocery list once by ID.
- Tap checkboxes to mark items purchased.
- Save once; the app then increments stock for all checked inventory-linked items.

## API endpoints used

- `GET /api/auth/me` - current user profile
- `GET /api/GroceryList/{id}` - loads the list
- `POST /api/Item/items/{id}/quantity?quantity=...&purchaseDate=...` - increments stock for each checked item

## Setup

1. Copy `local.properties.example` to `local.properties`.
2. Update `api.base.url` to your API base URL.
   - For the Android emulator pointing at a local API: `http://10.0.2.2:5000/`
   - For a device on the same network: `http://<your-machine-ip>:5000/`
3. Open the `android` folder in Android Studio and sync.
4. Run the app.

## Notes

- Manual items (`itemID == null`) are skipped when incrementing stock.
- The list is fetched once when you press **Load**.
- The **Save** callback is invoked exactly once after all updates complete.
