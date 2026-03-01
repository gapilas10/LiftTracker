import Link from 'next/link'

// TypeScript type for what the API returns
type WorkoutSession = {
  id: number
  date: string
  name: string
  notes: string
}

// No 'use client' directive = this is a Server Component.
// The function can be async, and fetch runs on the server before HTML is sent.
// The browser receives fully-rendered HTML — no loading spinner, good for SEO.
export default async function HomePage() {
  const res = await fetch(`${process.env.API_URL}/api/sessions`, {
    // 'no-store' disables Next.js caching — every request gets fresh data.
    // For a workout log this makes sense; for a marketing page you'd cache.
    cache: 'no-store',
  })

  const sessions: WorkoutSession[] = await res.json()

  return (
    <div>
      <h2>Sessions</h2>
      {sessions.map((session) => (
        <div key={session.id} className="card">
          <strong>{session.name}</strong>
          <p>{session.date}</p>
          <p>{session.notes}</p>
          {/* Link from next/link = client-side navigation, no full page reload */}
          <Link href={`/sessions/${session.id}`}>View →</Link>
        </div>
      ))}
    </div>
  )
}
