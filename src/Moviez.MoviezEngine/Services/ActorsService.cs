using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moviez.MoviezEngine.Contracts;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Services
{
    public class ActorsService : IActorsService
    {
        private readonly MoviezDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ActorsService> logger;

        public ActorsService(MoviezDbContext context, IMapper mapper, ILogger<ActorsService> logger)
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BaseResponseModel> CreateActor(ActorCreateRequest actorRequest)
        {

            BaseResponseModel baseResponse = new BaseResponseModel();

            using(var transaction = _dbContext.Database.BeginTransaction())
            {
               baseResponse = await CreateActorEngine(actorRequest).ConfigureAwait(false);
                if (baseResponse.Status)
                {
                    await transaction.CommitAsync().ConfigureAwait(false);
                }
                else
                {
                    await transaction.RollbackAsync().ConfigureAwait(false);
                }
            }
            return baseResponse;
        }

        public async Task<BaseResponseModel> CreateActorEngine(ActorCreateRequest actorRequest) {

            BaseResponseModel baseResponse = new BaseResponseModel();
            
            try
            {
                Actor actor = new Actor();
                actor.ActorName = actorRequest.ActorName;
                actor.DateOfBirth = actorRequest.DateOfBirth;
                actor.GenderCode = actorRequest.GenderCode;
                actor.Bio = actorRequest.Bio;
                actor.IsActive = true;

                _dbContext.Actors.Add(actor);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                baseResponse.Status = true;
                baseResponse.Message = "Actor Created Successfully";
                baseResponse.Id = actor.ActorId;
            }
            catch(Exception e)
            {
                baseResponse.Status = false;
                baseResponse.Message = "Actor Creation failed";

                logger.LogError("Error at CreateActorEngine: " + e.Message);
                
            }

            return baseResponse;
        }

        public async Task<List<ActorsListResponse>> GetActorsList()
        {
            List<ActorsListResponse> actors = new List<ActorsListResponse>();

            actors = await _dbContext.Actors.Where(x => x.IsActive)
                .Select(data => new ActorsListResponse
                {
                    ActorId = data.ActorId,
                    ActorName = data.ActorName
                }).ToListAsync().ConfigureAwait(false);

            return actors;
        }
    }
}

