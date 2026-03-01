'use client'

// This component needs 'use client' because it reads from a Zustand store
// and has onClick handlers — both require browser-side React.
// layout.tsx is a Server Component, but it can render Client Components inside it.

import Link from 'next/link'
import { useAuthStore } from '../store/authStore'

export function Header() {
  const { user, isAuthenticated, login, logout } = useAuthStore()

  return (
    <header style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '2rem' }}>
      <Link href="/" style={{ fontSize: '1.5rem', fontWeight: 'bold' }}>
        LiftTracker
      </Link>

      <div>
        {isAuthenticated ? (
          <>
            <span style={{ marginRight: '1rem' }}>Hi, {user?.name}</span>
            <button onClick={logout}>Log out</button>
          </>
        ) : (
          // Stub login — in a real app this would redirect to an auth provider
          <button onClick={() => login({ name: 'Leo', email: 'leo@example.com' })}>
            Log in
          </button>
        )}
      </div>
    </header>
  )
}
