import type { Metadata } from "next";
import { ReactNode } from "react";
import Providers from "./providers";
import AdminLayout from "./components/AdminLayout";

export const metadata: Metadata = {
  title: "LENA Admin",
  description: "LENA Inventory and Wine Admin",
};

export default function RootLayout({ children }: { children: ReactNode }) {
  return (
    <html lang="en">
      <body>
        <Providers>
          <AdminLayout>{children}</AdminLayout>
        </Providers>
      </body>
    </html>
  );
}
