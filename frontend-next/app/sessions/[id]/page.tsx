import Link from 'next/link'

type LoggedSet = {
  id: number
  exerciseName: string
  muscleGroup: string
  setNumber: number
  reps: number
  weightKg: number
}

type SessionDetail = {
  session: { id: number; date: string; name: string; notes: string }
  sets: LoggedSet[]
}

// [id] in the folder name becomes a dynamic segment.
// Next.js passes it as params.id — always a string from the URL.
// App Router: params is now a Promise in Next.js 15+, so we await it.
export default async function SessionDetailPage({
  params,
}: {
  params: Promise<{ id: string }>
}) {
  const { id } = await params

  const res = await fetch(`${process.env.API_URL}/api/sessions/${id}`, {
    cache: 'no-store',
  })

  if (!res.ok) return <p>Session not found.</p>

  const { session, sets }: SessionDetail = await res.json()

  return (
    <div>
      <Link href="/">← Back</Link>
      <h2>{session.name}</h2>
      <p>{session.date} — {session.notes}</p>

      <h3>Sets</h3>
      {sets.map((set) => (
        <div key={set.id} className="card">
          <strong>{set.exerciseName}</strong> ({set.muscleGroup})
          <p>Set {set.setNumber}: {set.reps} reps @ {set.weightKg}kg</p>
        </div>
      ))}

      <Link href={`/sessions/${id}/log`}>+ Log a set</Link>
    </div>
  )
}
