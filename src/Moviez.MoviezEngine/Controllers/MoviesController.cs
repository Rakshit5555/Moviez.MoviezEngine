using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Moviez.MoviezEngine.Contracts;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;
using Moviez.MoviezEngine.Services;
using Newtonsoft.Json;

namespace Moviez.MoviezEngine.Controllers
{
    [ApiController]
    [Route("api")]
    public class MoviesController : Controller
    {
        private readonly MoviezDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<MoviesService> logger;
        private readonly IMoviesService moviesService;
        private readonly IDistributedCache _cache;

        public MoviesController(MoviezDbContext context, ILogger<MoviesService> logger, IMapper mapper,
            IMoviesService moviesService, IDistributedCache cache)
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
            this.moviesService = moviesService;
            this._cache = cache;
        }

       

        /// <summary>
        /// To Get Complete Movie Details
        /// </summary>
        /// <returns></returns>
        [HttpGet("movies", Name = "GetAllMoviesDetails")]
        public async Task<ActionResult> GetAllMoviesDetails()
        {
            List<MovieDetailReponseForUser>? movieDetailReponseForUsers = new List<MovieDetailReponseForUser>();

            var data = await moviesService.GetAllMovieDetails().ConfigureAwait(false);

             if (data is { })
            {
                try
                {
                    movieDetailReponseForUsers = mapper.Map<List<MovieDetailReponseForUser>>(data);
                }
                catch(Exception e) {
                    movieDetailReponseForUsers = null;
                    logger.LogError("Error at GetAllMoviesDetails: " + e.Message);
                }

                
            }

            return Ok(movieDetailReponseForUsers);
        }

        /// <summary>
        /// To get movies based on filters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="actor"></param>
        /// <param name="releaseDateStart"></param>
        /// <param name="releaseDateEnd"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchMovies(
            string? name, 
            string? actor, 
            DateTime? releaseDateStart, 
            DateTime? releaseDateEnd)
        {
            // Create a unique cache key based on the filter parameters
            string cacheKey = $"movies_{name}_{actor}_{releaseDateStart?.ToShortDateString()}_{releaseDateEnd?.ToShortDateString()}";

            // Check if data is cached
            var cachedMovies = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedMovies))
            {
                var moviesFromCache = JsonConvert.DeserializeObject<IEnumerable<Movie>>(cachedMovies);
                return Ok(moviesFromCache);
            }

            // If no cache, retrieve from service
            var moviesFromService = await moviesService.SearchMoviesAsync(name, actor, releaseDateStart, releaseDateEnd);

            // Cache the result with a 30-minute expiration
            var serializedMovies = JsonConvert.SerializeObject(moviesFromService);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };

            await _cache.SetStringAsync(cacheKey, serializedMovies, cacheOptions);

            return Ok(moviesFromService);
        }

        /// <summary>
        /// To Add a movie using pre-existing actors and producers
        /// </summary>
        /// <param name="movieCreateRequest"></param>
        /// <returns></returns>
        [HttpPost("movies", Name = "AddMovie")]
        public async Task<ActionResult> AddMovie(MovieCreateRequest movieCreateRequest)
        {
            BaseResponseModelForUser baseResponseModelForUser = new BaseResponseModelForUser();

            var data = await moviesService.AddMovie(movieCreateRequest).ConfigureAwait(false);

            if (data is { })
            {
                try
                {
                    baseResponseModelForUser = mapper.Map<BaseResponseModelForUser>(data);
                }
                catch (Exception e)
                {
                    baseResponseModelForUser.Status = false;
                    baseResponseModelForUser.Message = "Unable to save Movie";
                    logger.LogError("Error at AddMovie: " + e.Message);
                }


            }

            return Ok(baseResponseModelForUser);
        }

        /// <summary>
        /// To Edit the Movie details including producers and linked actors
        /// </summary>
        /// <param name="movieCreateRequest"></param>
        /// <returns></returns>
        [HttpPut("movies", Name = "EditMovie")]
        public async Task<ActionResult> EditMovie(MovieCreateRequest movieCreateRequest)
        {
            BaseResponseModelForUser baseResponseModelForUser = new BaseResponseModelForUser();

            var data = await moviesService.EditMovie(movieCreateRequest).ConfigureAwait(false);

            if (data is { })
            {
                try
                {
                    baseResponseModelForUser = mapper.Map<BaseResponseModelForUser>(data);
                }
                catch (Exception e)
                {
                    baseResponseModelForUser.Status = false;
                    baseResponseModelForUser.Message = "Unable to edit Movie";
                    logger.LogError("Error at EditMovie: " + e.Message);
                }


            }

            return Ok(baseResponseModelForUser);
        }

    }
}

