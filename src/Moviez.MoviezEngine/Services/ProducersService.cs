using System;
using System.Numerics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moviez.MoviezEngine.Contracts;
using Moviez.MoviezEngine.Entities;
using Moviez.MoviezEngine.Models.RequestModels;
using Moviez.MoviezEngine.Models.ResponseModels;

namespace Moviez.MoviezEngine.Services
{
    public class ProducersService : IProducersService
    {
        private readonly MoviezDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ProducersService> logger;

        public ProducersService(MoviezDbContext context, IMapper mapper, ILogger<ProducersService> logger)
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<BaseResponseModel> CreateProducer(ProducerCreateRequest producerRequest)
        {

            BaseResponseModel baseResponse = new BaseResponseModel();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                baseResponse = await CreateProducerEngine(producerRequest).ConfigureAwait(false);
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

        public async Task<BaseResponseModel> CreateProducerEngine(ProducerCreateRequest producerRequest)
        {

            BaseResponseModel baseResponse = new BaseResponseModel();

            try
            {
                Producer producer = new Producer();
                producer.ProducerName = producerRequest.ProducerName;
                producer.DateOfBirth = Convert.ToDateTime(producerRequest.DateOfBirth);
                producer.GenderCode = producerRequest.GenderCode;
                producer.Company = producerRequest.Company;
                producer.Bio = producerRequest.Bio;
                producer.IsActive = true;

                _dbContext.Producers.Add(producer);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

                baseResponse.Status = true;
                baseResponse.Message = "Producer Created Successfully";
                baseResponse.Id = producer.ProducerId;
            }
            catch (Exception e)
            {
                baseResponse.Status = false;
                baseResponse.Message = "Producer Creation failed";

                logger.LogError("Error at CreateProducerEngine: " + e.Message);
                logger.LogError("Error at CreateProducerEngine: " + e.StackTrace);

            }

            return baseResponse;
        }

        public async Task<List<ProducersListResponse>> GetProducerList()
        {
            List<ProducersListResponse> producers = new List<ProducersListResponse>();

            producers = await _dbContext.Producers.Where(x => x.IsActive)
                .Select(data => new ProducersListResponse {
                    ProducerId = data.ProducerId,
                    ProducerName =  data.ProducerName
                }).ToListAsync().ConfigureAwait(false);

            return producers;
        }
    }
}

