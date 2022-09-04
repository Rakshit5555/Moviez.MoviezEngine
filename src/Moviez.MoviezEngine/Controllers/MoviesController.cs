using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moviez.MoviezEngine.Contracts;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;
using Moviez.MoviezEngine.Services;

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

        public MoviesController(MoviezDbContext context, ILogger<MoviesService> logger, IMapper mapper,
            IMoviesService moviesService)
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
            this.moviesService = moviesService;
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
        /// To Add a movie using pre-existing actors and producer
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

