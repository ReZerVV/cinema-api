# cinema-api

## APIs

### APIs movies

-   `POST api/v1/movies`
    -   создать фильм.
-   `GET api/v1/movies`
    -   фильмы по названию. фильмы по жанру. фильмы по типу.
-   `GET api/v1/movies/:movie_id`
    -   получить фильм.
-   `PUT api/v1/movies/:movie_id`
    -   обновить информацию о фильме.
-   `DELETE api/v1/movies/:movie_id`
    -   удалить фильма.

### APIs movies/downloads

-   `POST api/v1/movies/downloads`
    -   загрузить фильм по ссылке kinogo.film на фильм.
-   `GET api/v1/movies/downloads`
    -   получить все фильмы у которых медиа в статусе загрузки.
-   `DELETE api/v1/movies/downloads/:download_id`
    -   удалить ссылку.

### APIs movies/genres

-   `GET api/v1/genres`
    -   получить все категории.
