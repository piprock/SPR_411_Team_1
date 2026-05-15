import { useEffect, useState } from 'react'
import './App.css'
import { api } from './api'

function App() {
  const [genres, setGenres] = useState([])

  useEffect(() => {
    api.get('genres')
      .then((res) => setGenres(res.data?.payload ?? []))
      .catch(() => setGenres([]))
  }, [])

  return (
    <>
      <h1>Genres</h1>
      <ul>
        {genres.map((genre) => (
          <li key={genre.id}>{genre.name}</li>
        ))}
      </ul>
    </>
  )
}

export default App
