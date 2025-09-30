function showMovieDetails(imageUrl, title, description) {
  document.getElementById('modalImage').src = imageUrl;
  document.getElementById('modalTitle').textContent = title;
  document.getElementById('modalDescription').textContent = description;
  $('#movieModal').modal('show');
}
  const movieListContainer = document.getElementById("movieListContainer");

  movies.forEach(movie => {
    const col = document.createElement("div");
    col.className = "col-lg-3 col-md-4 col-12";

    col.innerHTML = `
      <div class="card movie-card h-100 shadow-sm">
        <div class="card-body d-flex flex-column">
          <div class="card-img-container">
            <img src="${movie.image}" alt="${movie.title}" />
          </div>
          <h5 class="card-title font-weight-bold">${movie.title}</h5>
          <p class="card-text">${movie.description}</p>
        </div>
        <div class="card-body d-flex flex-column">
          <div class="mt-auto icon-bar d-flex justify-content-between">
            <div class="left-icons">
              <i class="material-icons text-primary" title="Explore"
                 onclick="location.href='movie-details.html?id=${movie.id}'">
                 info
              </i>
            </div>
            <div class="right-icons">
              <i class="material-icons text-warning" title="Edit">edit</i>
              <i class="material-icons text-danger" title="Delete">delete</i>
            </div>
          </div>
        </div>
      </div>
    `;

    movieListContainer.appendChild(col);
  });


const urlParams = new URLSearchParams(window.location.search);
const movieId = urlParams.get('id');
const movie = movies.find(m => m.id === movieId);

if (movie) {
  document.getElementById('movieTitle').textContent = movie.title;
  document.getElementById('movieDescription').textContent = movie.description;
  document.getElementById('moviePoster').src = movie.image;
  document.getElementById('movieGenre').textContent = `Genre: ${movie.genre}`;
} else {
  document.getElementById('movieTitle').textContent = "Movie Not Found";
  document.getElementById('movieDescription').textContent = "Try selecting a movie again.";
  document.getElementById('moviePoster').style.display = 'none';
  document.getElementById('movieGenre').textContent = "";
}
