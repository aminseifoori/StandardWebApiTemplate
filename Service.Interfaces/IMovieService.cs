﻿using Shared.Dtos.Movies;

namespace Service.Interfaces
{
    public interface IMovieService
    {
        Task<IEnumerable<MovieDto>> GetAllMoviesAsync(bool trackChanges);
        Task<MovieDto> GetMovieByIdAsync(Guid id, bool trackChanges);
        Task<MovieDto> CreateMovieAsync(CreateMovieDto company);
        Task<IEnumerable<MovieDto>> GetMoviesByIdsAsync(IEnumerable<Guid> ids, bool trackChanges);
        Task<(IEnumerable<MovieDto> movies, string ids)> CreateMovieCollectionAsync(IEnumerable<CreateMovieDto> createMovieCollection);
        Task DeleteMovieAsync(Guid id, bool trackChanges);
        Task UpdateMovieAsync(Guid id, UpdateMovieDto updateMovie, bool trackChanges);
    }
}
