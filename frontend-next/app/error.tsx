'use client'

// error.tsx must be a Client Component — it receives the error object
// and a reset function. 'use client' is required here by Next.js.
// It catches errors thrown by any page or component in this directory.
export default function Error({
  error,
  reset,
}: {
  error: Error
  reset: () => void
}) {
  return (
    <div>
      <p>Something went wrong: {error.message}</p>
      {/* reset() re-renders the page — useful if the error was transient */}
      <button onClick={reset}>Try again</button>
    </div>
  )
}
