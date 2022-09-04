using System;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Contracts
{
    public interface IProducersService
    {
        Task<BaseResponseModel> CreateProducer(ProducerCreateRequest producerRequest);
        Task<BaseResponseModel> CreateProducerEngine(ProducerCreateRequest producerRequest);

        Task<List<ProducersListResponse>> GetProducerList();

    }
}

