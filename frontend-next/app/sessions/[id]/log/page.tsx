'use client'

// 'use client' marks this as a Client Component.
// Required here because we use useState (React hooks) and an onClick handler.
// Rule of thumb: default to Server Component, add 'use client' only when needed.

import { useState } from 'react'
import { useRouter, useParams } from 'next/navigation'

export default function LogSetPage() {
  const router = useRouter()
  // useParams() reads the [id] segment from the URL — same concept as React Router
  const params = useParams()
  const sessionId = params.id as string

  // Controlled form state — each field is a piece of React state.
  // React owns the value; the input displays what React says.
  const [exercise, setExercise] = useState('')
  const [muscleGroup, setMuscleGroup] = useState('')
  const [reps, setReps] = useState(5)
  const [weight, setWeight] = useState(60)
  const [submitting, setSubmitting] = useState(false)

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault() // prevents the default full-page form POST
    setSubmitting(true)

    // Client-side fetch — runs in the browser, so CORS applies.
    // The .NET API allows localhost:3000 via the CORS policy we set up.
    await fetch(`${process.env.NEXT_PUBLIC_API_URL}/api/sessions/${sessionId}/sets`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        exerciseName: exercise,
        muscleGroup,
        reps,
        weightKg: weight,
      }),
    })

    // Navigate back to the session detail page after a successful POST
    router.push(`/sessions/${sessionId}`)
  }

  return (
    <div>
      <h2>Log a Set</h2>
      <form onSubmit={handleSubmit}>
        <label>Exercise</label>
        <input
          value={exercise}
          onChange={(e) => setExercise(e.target.value)}
          placeholder="e.g. Bench Press"
          required
        />

        <label>Muscle Group</label>
        <input
          value={muscleGroup}
          onChange={(e) => setMuscleGroup(e.target.value)}
          placeholder="e.g. Chest"
        />

        <label>Reps</label>
        <input
          type="number"
          value={reps}
          min={1}
          onChange={(e) => setReps(Number(e.target.value))}
        />

        <label>Weight (kg)</label>
        <input
          type="number"
          value={weight}
          step={0.5}
          onChange={(e) => setWeight(Number(e.target.value))}
        />

        <button type="submit" disabled={submitting}>
          {submitting ? 'Saving...' : 'Log Set'}
        </button>
      </form>
    </div>
  )
}
