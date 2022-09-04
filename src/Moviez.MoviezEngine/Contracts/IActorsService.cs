using System;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Contracts
{
    public interface IActorsService
    {
        Task<BaseResponseModel> CreateActor(ActorCreateRequest actorRequest);
        Task<BaseResponseModel> CreateActorEngine(ActorCreateRequest actorRequest);

        Task<List<ActorsListResponse>> GetActorsList();
    }
}

