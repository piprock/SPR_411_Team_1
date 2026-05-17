export default function AlbumCard({ album }) {
  const songCount = album.songs?.length ?? 0
  const coverLabel = album.title?.slice(0, 1)?.toUpperCase() || 'A'
  const songCountLabel = songCount % 10 == 1 && songCount % 100 !== 11 ? 'пісня'
      : songCount % 10 >= 2 && songCount % 10 <= 4 && (songCount % 100 < 12 || songCount % 100 > 14)
        ? 'пісні'
        : 'пісень'
    // це було зроблено для покращення з ШІ

  return (
    <article className="album-card">


      <div className="album-cover">
        {album.coverUrl ? (<img src={album.coverUrl} alt={`Обкладинка альбому ${album.title}`} />) 
        : 
        (<img src="src\storage\noimage.jpg" alt={`Обкладинка альбому ${album.title}`} />)}
      </div>

      <div className="album-content">
        <h2>{album.title}</h2>
        <p>{album.artist?.name ?? 'Невідомий артист'}</p>
        <span>{songCount} {songCountLabel}</span>
      </div>


    </article>
  )
}


//оптимізовано з ШІ
