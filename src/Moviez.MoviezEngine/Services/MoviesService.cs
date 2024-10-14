using System;
using System.Collections;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviez.MoviezEngine.Contracts;
using Moviez.MoviezEngine.Controllers;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly MoviezDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<MoviesService> logger;
        private readonly IProducersService producersService;
        private readonly IActorsService actorsService;

        public MoviesService(MoviezDbContext context, ILogger<MoviesService> logger, IMapper mapper,
            IProducersService producersService, IActorsService actorsService) 
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
            this.producersService = producersService;
            this.actorsService = actorsService;
        }

        public async Task<List<MovieDetailReponse>> GetAllMovieDetails()
        {

            var moviesInfo = await _dbContext.Movies.Join(_dbContext.Producers,
                            m => m.ProducerId,
                            p => p.ProducerId,
                            (m, p) => new { m, p }).Where(x => x.m.IsActive && x.p.IsActive).ToListAsync().ConfigureAwait(false);



            List<MovieDetailReponse> movieData = moviesInfo
               .GroupJoin(_dbContext.Castings.Where(x => x.IsActive),
               mp => mp.m.MovieId,
               c => c.MovieId,
               (movieData, castingInfo) => new
               {
                   movieData,
                   castingInfo
               }).Select(data => new MovieDetailReponse
               {
                   MovieId = data.movieData.m.MovieId,
                   MovieName = data.movieData.m.MovieName,
                   DateOfRelease = data.movieData.m.DateOfRelease,
                   PosterLink = data.movieData.m.PosterLink,
                   Plot = data.movieData.m.Plot,

                   ProducerName = data.movieData.p.ProducerName,

                   //Getting Actor Details as list
                   Actors = data.castingInfo.Join(_dbContext.Actors.Where(x => x.IsActive),
                   x => x.ActorId,
                   y => y.ActorId,
                   (x, y) => new { x, y }).Select(ActorInfo => new ActorsResponse
                   {
                       ActorName = ActorInfo.y.ActorName
                   }).ToList()
               }).ToList();

            return movieData;
        }


        public async Task<IEnumerable<MovieDetailReponse>> SearchMoviesAsync(string name, string actor, DateTime? releaseDateStart, DateTime? releaseDateEnd)
        {
            List<MovieDetailReponse> filteredMovies = await GetAllMovieDetails();

            if (!string.IsNullOrEmpty(name))
                filteredMovies = filteredMovies
                    .Where(m => m.MovieName.Contains(name, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (!string.IsNullOrEmpty(actor))
                filteredMovies = filteredMovies
                    .Where(m => m.Actors
                    .Any(x => x.ActorName.Contains(actor, StringComparison.OrdinalIgnoreCase)))
                    .ToList();

            if (releaseDateStart.HasValue)
                filteredMovies = filteredMovies
                    .Where(m => m.DateOfRelease >= releaseDateStart)
                    .ToList();

            if (releaseDateEnd.HasValue)
                filteredMovies = filteredMovies
                    .Where(m => m.DateOfRelease <= releaseDateEnd)
                    .ToList();

            return await Task.FromResult(filteredMovies.ToList());
        }

        /// <summary>
        /// Method which enable user to add a movies using existing actors and producer
        /// </summary>
        /// <param name="movieCreateRequest"></param>
        /// <returns></returns>
        public async Task<BaseResponseModel> AddMovie(MovieCreateRequest movieCreateRequest)
        {

            BaseResponseModel movieBaseResponse = new BaseResponseModel();

            if (movieCreateRequest == null)
            {
                movieBaseResponse.Status = false;
                movieBaseResponse.Message = "No request found!";
                return movieBaseResponse;
            }
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    
                    //Creating Movie
                    Movie movie = new Movie();
                    movie.MovieName = movieCreateRequest.MovieName;
                    movie.DateOfRelease = movieCreateRequest.DateOfRelease;
                    movie.PosterLink = movieCreateRequest.PosterLink;
                    movie.Plot = movieCreateRequest.Plot;
                    movie.ProducerId = movieCreateRequest.ProducerId;
                    movie.IsActive = true;

                    _dbContext.Movies.Add(movie);
                    await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                    //Adding the movie Cast
                    foreach (var actorRequestId in movieCreateRequest.ActorRequestIds)
                    {
                        Casting casting = new Casting();
                        casting.MovieId = movie.MovieId;
                        casting.ActorId = actorRequestId;
                        casting.IsActive = true;
                        _dbContext.Castings.Add(casting);
                        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                    }
                    
                    movieBaseResponse.Status = true;
                    movieBaseResponse.Message = "Movie Added Successfully";
                    movieBaseResponse.Id = movie.MovieId;

                    if (movieBaseResponse.Status)
                    {
                        await transaction.CommitAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                    }
                }
                catch(Exception e)
                {
                    movieBaseResponse.Status = false;
                    movieBaseResponse.Message = "Failed to add movie";

                    await transaction.RollbackAsync().ConfigureAwait(false);
                    logger.LogError("Error at CreateMovie: " + e.Message);
                }
                
            }
            return movieBaseResponse;
        }

        public async Task<BaseResponseModel> EditMovie(MovieCreateRequest movieCreateRequest)
        {
            
            BaseResponseModel movieBaseResponse = new BaseResponseModel();

            if (movieCreateRequest == null)
            {
                movieBaseResponse.Status = false;
                movieBaseResponse.Message = "No request found!";
                return movieBaseResponse;
            }

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {

                    var existingMovieDetails = await _dbContext.Movies
                        .Where(x => x.MovieName == movieCreateRequest.MovieName).FirstOrDefaultAsync();
                    if(existingMovieDetails is { })
                    {
                        existingMovieDetails.MovieName = movieCreateRequest.MovieName;
                        existingMovieDetails.DateOfRelease = movieCreateRequest.DateOfRelease;
                        existingMovieDetails.PosterLink = movieCreateRequest.PosterLink;
                        existingMovieDetails.Plot = movieCreateRequest.Plot;
                        existingMovieDetails.ProducerId = movieCreateRequest.ProducerId;

                        _dbContext.Movies.Update(existingMovieDetails);
                        await _dbContext.SaveChangesAsync().ConfigureAwait(false);


                        var existingCastDetails = await _dbContext.Castings
                                .Where(x => existingMovieDetails.MovieId == x.MovieId
                                && x.IsActive).ToListAsync().ConfigureAwait(false);


                        List<long> actorsIds = movieCreateRequest.ActorRequestIds;
                        
                        foreach (var castDetail in existingCastDetails)
                        {
                            if (actorsIds.Contains(castDetail.ActorId))
                            {
                                actorsIds.Remove(castDetail.ActorId);
                            }
                            else
                            {
                                castDetail.IsActive = false;
                                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                            }
                        }

                        //Adding added actors
                        foreach (var actorRequestId in actorsIds)
                        {
                            Casting casting = new Casting();
                            casting.MovieId = existingMovieDetails.MovieId;
                            casting.ActorId = actorRequestId;
                            casting.IsActive = true;
                            _dbContext.Castings.Add(casting);
                            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                        }

                        movieBaseResponse.Status = true;
                        movieBaseResponse.Message = "Movie Details Updated Successfully";
                        movieBaseResponse.Id = existingMovieDetails.MovieId;
                    }
                    

                    

                    if (movieBaseResponse.Status)
                    {
                        await transaction.CommitAsync().ConfigureAwait(false);
                    }
                    else
                    {
                        await transaction.RollbackAsync().ConfigureAwait(false);
                    }
                }
                catch (Exception e)
                {
                    movieBaseResponse.Status = false;
                    movieBaseResponse.Message = "Movie Updation failed";

                    await transaction.RollbackAsync().ConfigureAwait(false);
                    logger.LogError("Error at EditMovie: " + e.Message);
                }

            }
            return movieBaseResponse;
        }

    }
}

