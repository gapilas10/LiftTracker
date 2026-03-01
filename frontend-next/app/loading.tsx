// loading.tsx is shown automatically while any async Server Component
// in this directory is fetching. Next.js wraps the page in a Suspense
// boundary behind the scenes — you don't need to add Suspense manually.
export default function Loading() {
  return <p>Loading...</p>
}
