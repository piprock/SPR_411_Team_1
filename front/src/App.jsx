import { useEffect, useMemo, useState } from 'react'
import './App.css'
import { api } from './api'
import AlbumsPage from './pages/albums/AlbumsPage'
import ArtistSearch from './pages/artists/ArtistSearch'

function App() {
  const [songs, setSongs] = useState([])
  const [selectedArtist, setSelectedArtist] = useState('')
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  useEffect(() => {
    const loadSongs = async () => {
      setLoading(true)
      setError('')

      try {
        const response = await api.get('songs')
        setSongs(response.data?.payload ?? [])
      } catch {
        setSongs([])
        setError('Не вдалося завантажити пісні з API.')
      } finally {
        setLoading(false)
      }
    }

    loadSongs()
  }, [])

  const artists = useMemo(() => {
    return [...new Set(songs.map((song) => song.artist?.name).filter(Boolean))].sort()
  }, [songs])

  useEffect(() => {
    if (!selectedArtist && artists.length > 0) {
      setSelectedArtist(artists[0])
    }
  }, [artists, selectedArtist])

  const selectedSongs = useMemo(() => {
    return songs.filter((song) => song.artist?.name === selectedArtist)
  }, [songs, selectedArtist])

  const genres = useMemo(() => {
    return songs.reduce((items, song) => {
      song.genres.forEach((genre) => {
        const current = items.find((item) => item.name === genre.name)

        if (current) {
          current.count += 1
          return
        }

        items.push({ name: genre.name, count: 1 })
      })

      return items
    }, []).sort((a, b) => a.name.localeCompare(b.name))
  }, [songs])

  return (
    <main className="page">
      <aside className="sidebar">
        <div className="logo">SoundMix</div>
        <nav className="artist-list" aria-label="Artists list">
          <p className="section-label">Artists</p>
            {artists.map((artist) => (
              <button
                className={artist === selectedArtist ? 'artist-button active' : 'artist-button'}
                key={artist}
                onClick={() => setSelectedArtist(artist)}
                type="button"
              >
                <span className="artist-avatar">{artist.slice(0, 1)}</span>
                {artist}
              </button>
            ))}
        </nav>
      </aside>

      <div className="content">
        <section className="hero-panel">
          <p className="section-label">Now browsing</p>
          <h1>{selectedArtist || 'No artist selected'}</h1>
          <p className="hero-text">
            {loading
              ? 'Loading songs from API...'
              : error || `${selectedSongs.length} songs in this artist playlist`}
          </p>
        </section>

        <section className="songs-panel">
          <div className="panel-heading">
            <h2>Songs</h2>
            <span>{songs.length} tracks loaded</span>
          </div>

          {loading && <p className="songs-state">Loading songs...</p>}
          {!loading && error && <p className="songs-state error">{error}</p>}
          {!loading && !error && selectedSongs.length === 0 && (
            <p className="songs-state">No songs found for the selected artist.</p>
          )}

          {!loading && !error && selectedSongs.length > 0 && (
            <div className="songs-list">
              {selectedSongs.map((song) => (
                <article className="song-row" key={song.id}>
                  <span className="song-number">{song.id}</span>
                  <div>
                    <h3>{song.title}</h3>
                    <p>{song.artist?.name}</p>
                  </div>
                  <strong>{song.genres.map((genre) => genre.name).join(', ') || 'No genre'}</strong>
                </article>
              ))}
            </div>
          )}
        </section>

        <section className="genres-panel">
          <div className="panel-heading">
            <h2>Genres</h2>
            <span>Counted on client</span>
          </div>

          <div className="genres-grid">
            {genres.map((genre) => (
              <article className="genre-tile" key={genre.name}>
                <h3>{genre.name}</h3>
                <p>{genre.count} songs</p>
              </article>
            ))}
          </div>
          </section>

        <AlbumsPage />
        <ArtistSearch />
      </div>
    </main>
  )
}

export default App
