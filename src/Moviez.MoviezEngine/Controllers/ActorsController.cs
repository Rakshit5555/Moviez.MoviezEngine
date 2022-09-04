using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moviez.MoviezEngine.Contracts;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;
using Moviez.MoviezEngine.Services;

namespace Moviez.MoviezEngine.Controllers
{
    [ApiController]
    [Route("api")]
    public class ActorsController : Controller
    {

        private readonly MoviezDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ActorsService> logger;
        private readonly IActorsService actorsService;

        public ActorsController(MoviezDbContext context, ILogger<ActorsService> logger, IMapper mapper,
            IActorsService actorsService)
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
            this.actorsService = actorsService;
        }

        /// <summary>
        /// To get list of all actors
        /// </summary>
        /// <returns></returns>
        [HttpGet("actorslist", Name = "GetActorsList")]
        public async Task<ActionResult> GetActorsList()
        {
            List<ActorsListResponse>? actors = new List<ActorsListResponse>();

            var data = await actorsService.GetActorsList().ConfigureAwait(false);

            if (data is { })
            {
                try
                {
                    actors = mapper.Map<List<ActorsListResponse>>(data);
                }
                catch (Exception e)
                {
                    actors = null;
                    logger.LogError("Error at GetAllMoviesDetails: " + e.Message);
                }
            }

            return Ok(actors);
        }

        /// <summary>
        /// To Add a new actor
        /// </summary>
        /// <param name="actorCreateRequest"></param>
        /// <returns></returns>
        [HttpPost("actors", Name = "AddActor")]
        public async Task<ActionResult> AddActor(ActorCreateRequest actorCreateRequest)
        {
            BaseResponseModelForUser baseResponseModelForUser = new BaseResponseModelForUser();

            var data = await actorsService.CreateActor(actorCreateRequest).ConfigureAwait(false);

            if (data is { })
            {
                try
                {
                    baseResponseModelForUser = mapper.Map<BaseResponseModelForUser>(data);
                }
                catch (Exception e)
                {
                    baseResponseModelForUser.Status = false;
                    baseResponseModelForUser.Message = "Unable to save Actor";
                    logger.LogError("Error at AddActor: " + e.Message);
                }
            }

            return Ok(baseResponseModelForUser);
        }
        
    }
}

