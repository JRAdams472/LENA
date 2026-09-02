# LENA Mobile

A Flutter companion app for LENA. It replaces the legacy native Android app with a cross-platform Flutter implementation.

## Features

- Google Sign-In using an ID token.
- Secure token persistence with `flutter_secure_storage`.
- Authenticated HTTP client that attaches `Authorization: Bearer <idToken>` and routes back to the sign-in screen on 401.
- Login screen that calls `GET /api/auth/me` to prove the login loop.
- Grocery list screen: load a list by ID, check off items, and increment stock for checked inventory-linked items.

## Setup

1. Install Flutter and ensure `flutter doctor` passes for the platforms you want to build.
2. Generate the platform project files if they are not present:

   ```bash
   cd mobile
   flutter create --platforms=android,ios .
   ```

3. Configure the Google Sign-In Web/Server client ID. This value must match the API's `Authentication:Google:ClientId` so the returned ID token's `aud` is accepted.

   See [docs/google-oauth-client-id.md](../docs/google-oauth-client-id.md) for how to create the client ID and download `google-services.json` / `GoogleService-Info.plist`.

   - For Android: add `GOOGLE_SERVER_CLIENT_ID` as a `--dart-define` when running:

     ```bash
     flutter run --dart-define=API_BASE_URL=http://10.0.2.2:5059 \
                 --dart-define=GOOGLE_SERVER_CLIENT_ID=<your-web-client-id>
     ```

     You can also set the value permanently in your IDE run configuration or in `android/app/src/main/res/values/strings.xml` and reference it from Dart if desired.

   - For iOS: add the Web/Server client ID to `GoogleService-Info.plist` or pass it via `--dart-define`.

4. Run the app against a local API:

   ```bash
   flutter run --dart-define=API_BASE_URL=http://10.0.2.2:5059 \
               --dart-define=GOOGLE_SERVER_CLIENT_ID=<your-web-client-id>
   ```

   - Android emulator loopback: `http://10.0.2.2:5059`
   - Physical device on the same network: `http://<your-machine-ip>:5059`
   - iOS simulator: `http://localhost:5059`

## API endpoints used

- `GET /api/auth/me` - current user profile (proves the login loop)
- `GET /api/GroceryList/{id}` - loads the grocery list
- `POST /api/Item/items/{id}/quantity?quantity=...&purchaseDate=...` - increments stock for each checked item

## Notes

- Manual items (`itemID == null`) are skipped when updating stock.
- The ID token is read on startup. If it is expired or invalid, the user is treated as signed out and routed to the login screen.
- The app uses `google_sign_in`'s `serverClientId` to obtain an ID token for the API's configured audience.
