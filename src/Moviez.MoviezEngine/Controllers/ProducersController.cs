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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Moviez.MoviezEngine.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProducersController : Controller
    {
        private readonly MoviezDbContext _dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<ProducersService> logger;
        private readonly IProducersService producersService;

        public ProducersController(MoviezDbContext context, ILogger<ProducersService> logger, IMapper mapper,
            IProducersService producersService)
        {
            _dbContext = context;
            this.mapper = mapper;
            this.logger = logger;
            this.producersService = producersService;
        }
        
       /// <summary>
       /// To get the list of all producers
       /// </summary>
       /// <returns></returns>
        [HttpGet("producerslist", Name = "GetProducerList")]
        public async Task<ActionResult> GetProducerList()
        {
            List<ProducersListResponse>? producers = new List<ProducersListResponse>();

            var data = await producersService.GetProducerList().ConfigureAwait(false);

            if (data is { })
            {
                try
                {
                    producers = mapper.Map<List<ProducersListResponse>>(data);
                }
                catch (Exception e)
                {
                    producers = null;
                    logger.LogError("Error at GetAllMoviesDetails: " + e.Message);
                }


            }

            return Ok(producers);
        }


        /// <summary>
        /// To Add a new Producer
        /// </summary>
        /// <param name="producerCreateRequest"></param>
        /// <returns></returns>
        [HttpPost("producers", Name = "AddProducer")]
        public async Task<ActionResult> AddProducer(ProducerCreateRequest producerCreateRequest)
        {
            BaseResponseModelForUser baseResponseModelForUser  = new BaseResponseModelForUser();

            var data = await producersService.CreateProducer(producerCreateRequest).ConfigureAwait(false);

            if (data is { })
            {
                try
                {
                    baseResponseModelForUser = mapper.Map<BaseResponseModelForUser>(data);
                }
                catch (Exception e)
                {
                    baseResponseModelForUser.Status = false;
                    baseResponseModelForUser.Message = "Unable to save Producer";
                    logger.LogError("Error at AddProducer: " + e.Message);
                }


            }

            return Ok(baseResponseModelForUser);
        }

    }
}

