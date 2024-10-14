using System;
using System.Collections;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Contracts
{
    public interface IMoviesService
    {
        Task<List<MovieDetailReponse>> GetAllMovieDetails();
        Task<IEnumerable<MovieDetailReponse>> SearchMoviesAsync(string name, string actor, DateTime? releaseDateStart, DateTime? releaseDateEnd);
        Task<BaseResponseModel> AddMovie(MovieCreateRequest movieCreateRequest);
        Task<BaseResponseModel> EditMovie(MovieCreateRequest movieCreateRequest);
    }
}

