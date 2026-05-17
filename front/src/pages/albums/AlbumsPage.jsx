import { useEffect, useState } from "react";
import { api } from "../../api";
import AlbumCard from "./AlbumCard";

export default function AlbumsPage() {
  const [albums, setAlbums] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    api
      .get("albums")
      .then((response) => {
        setAlbums(response.data?.payload ?? []);
        setError("");
      })
      .catch(() => {
        setAlbums([]);
        setError("Не вдалося завантажити альбоми.");
      })
      .finally(() => setLoading(false));
  }, []);

  return (
    <main className="albums-page">
      <header className="albums-header">
        <p>Бібліотека</p>
        <h1>Альбоми</h1>
      </header>

      {loading && <p className="albums-state">Завантаження альбомів...</p>}
      {!loading && error && <p className="albums-state error">{error}</p>}
      {!loading && !error && albums.length == 0 && ( <p className="albums-state">Альбомів не знайдено.</p>)}


      {!loading && !error && albums.length > 0 && (
        <section className="albums-grid" aria-label="Список альбомів">
          {albums.map((album) => (
            <AlbumCard key={album.id} album={album} />
          ))}
        </section>
      )}
    </main>

    // оптимізовано з ШІ (було це написано через if)
  );
}
