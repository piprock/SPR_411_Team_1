import { useEffect, useState } from 'react'
import './App.css'
import { api } from './api'
import AlbumsPage from './pages/albums/AlbumsPage'
import ArtistSearch from './pages/artists/ArtistSearch'

function App() {
  const [genres, setGenres] = useState([])

  useEffect(() => {
    api.get('genres')
      .then((res) => setGenres(res.data?.payload ?? []))
      .catch(() => setGenres([]))
  }, [])

  return (
    <>
      <h1>Жанри</h1>
      <ul>
        {genres.map((genre) => (
          <li key={genre.id}>{genre.name}</li>
        ))}
      </ul>
      <AlbumsPage />
      <ArtistSearch />
    </>
  )
}

export default App
