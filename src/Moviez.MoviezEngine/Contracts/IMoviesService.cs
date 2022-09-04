using System;
using System.Collections;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Contracts
{
    public interface IMoviesService
    {
        Task<IEnumerable> GetAllMovieDetails();
        Task<BaseResponseModel> AddMovie(MovieCreateRequest movieCreateRequest);
        Task<BaseResponseModel> EditMovie(MovieCreateRequest movieCreateRequest);
    }
}

