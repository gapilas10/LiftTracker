import type { Metadata } from 'next'
import { Header } from '../components/Header'
import './globals.css'

export const metadata: Metadata = {
  title: 'LiftTracker',
}

// layout.tsx is a Server Component, but it can include Client Components
// like Header — Next.js handles the boundary automatically.
export default function RootLayout({ children }: { children: React.ReactNode }) {
  return (
    <html lang="en">
      <body>
        <Header />
        <main>{children}</main>
      </body>
    </html>
  )
}
