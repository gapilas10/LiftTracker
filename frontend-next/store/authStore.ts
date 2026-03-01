import { create } from 'zustand'

type User = { name: string; email: string }

type AuthState = {
  user: User | null
  isAuthenticated: boolean
  // Actions defined alongside state — the Zustand pattern
  login: (user: User) => void
  logout: () => void
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  isAuthenticated: false,

  login: (user) => set({ user, isAuthenticated: true }),
  logout: () => set({ user: null, isAuthenticated: false }),
}))
