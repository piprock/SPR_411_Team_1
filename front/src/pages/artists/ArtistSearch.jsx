import { useState } from "react";
import { api } from "../../api";

export default function ArtistSearch({ onSelectArtist }) {
  const [query, setQuery] = useState("");
  const [artists, setArtists] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [searched, setSearched] = useState(false);

  const handleSubmit = async (event) => {
    event.preventDefault();

    const trimmedQuery = query.trim();

    if (!trimmedQuery) {
      setArtists([]);
      setError("Введіть ім’я артиста для пошуку.");
      setSearched(false);
      return;
    }

    setLoading(true);
    setError("");
    setSearched(true);

    try {
      const response = await api.get("artists/search", {
        params: { query: trimmedQuery },
      });

      setArtists(response.data?.payload ?? []);
    } catch {
      setArtists([]);
      setError("Не вдалося виконати пошук артистів.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <section className="artist-search">
      <header className="artist-search-header">
        <p>Каталог</p>
        <h1>Пошук артистів</h1>
      </header>

      <form className="artist-search-form" onSubmit={handleSubmit}>
        <input
          type="text"
          value={query}
          onChange={(event) => setQuery(event.target.value)}
          placeholder="Введіть ім’я артиста"
        />
        <button type="submit">Знайти</button>
      </form>

      {loading && <p className="artist-search-state">Пошук...</p>}
      {!loading && error && (
        <p className="artist-search-state error">{error}</p>
      )}
      {!loading && searched && !error && artists.length === 0 && (
        <p className="artist-search-state">Артистів не знайдено.</p>
      )}

      {!loading && artists.length > 0 && (
        <div className="artist-results">
          {artists.map((artist) => (
            <button
              className="artist-card"
              key={artist.id}
              type="button"
              onClick={() => onSelectArtist?.(artist.name)}
            >
              <h2>{artist.name}</h2>
              <p>{artist.bio || "Опис відсутній."}</p>
            </button>
          ))}
        </div>
      )}
    </section>
  );
}
//вся перевірка з допомогою ШІ