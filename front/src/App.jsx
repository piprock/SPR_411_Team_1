import { useEffect, useMemo, useState } from 'react'
import './App.css'

const initialSongs = [
  { id: 1, title: 'Blinding Lights', artist: 'The Weeknd', genre: 'Pop' },
  { id: 2, title: 'Save Your Tears', artist: 'The Weeknd', genre: 'Pop' },
  { id: 3, title: 'Shape of You', artist: 'Ed Sheeran', genre: 'Pop' },
  { id: 4, title: 'Perfect', artist: 'Ed Sheeran', genre: 'Ballad' },
  { id: 5, title: 'Bad Guy', artist: 'Billie Eilish', genre: 'Alternative' },
  { id: 6, title: 'Lovely', artist: 'Billie Eilish', genre: 'Alternative' },
  { id: 7, title: 'Believer', artist: 'Imagine Dragons', genre: 'Rock' },
  { id: 8, title: 'Thunder', artist: 'Imagine Dragons', genre: 'Rock' },
  { id: 9, title: 'Levitating', artist: 'Dua Lipa', genre: 'Dance' },
  { id: 10, title: "Don't Start Now", artist: 'Dua Lipa', genre: 'Dance' },
]

function App() {
  const [songs, setSongs] = useState([])
  const [selectedArtist, setSelectedArtist] = useState('')

  useEffect(() => {
    setSongs(initialSongs)
  }, [])

  const artists = useMemo(() => {
    return [...new Set(songs.map((song) => song.artist))].sort()
  }, [songs])

  useEffect(() => {
    if (!selectedArtist && artists.length > 0) {
      setSelectedArtist(artists[0])
    }
  }, [artists, selectedArtist])

  const selectedSongs = useMemo(() => {
    return songs.filter((song) => song.artist === selectedArtist)
  }, [songs, selectedArtist])

  const genres = useMemo(() => {
    return songs.reduce((items, song) => {
      const current = items.find((item) => item.name === song.genre)

      if (current) {
        current.count += 1
        return items
      }

      return [...items, { name: song.genre, count: 1 }]
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
          <h1>{selectedArtist}</h1>
          <p className="hero-text">{selectedSongs.length} songs in this artist playlist</p>
        </section>

        <section className="songs-panel">
          <div className="panel-heading">
            <h2>Songs</h2>
            <span>{songs.length} tracks loaded</span>
          </div>

          <div className="songs-list">
            {selectedSongs.map((song) => (
              <article className="song-row" key={song.id}>
                <span className="song-number">{song.id}</span>
                <div>
                  <h3>{song.title}</h3>
                  <p>{song.artist}</p>
                </div>
                <strong>{song.genre}</strong>
              </article>
            ))}
          </div>
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
      </div>
    </main>
  )
}

export default App
