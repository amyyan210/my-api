using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps/{moniker}/talks")]
    public class TalksController : ApiController
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public TalksController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false)
        {
            try
            {
                var results = await _repository.GetTalksByMonikerAsync(moniker, includeSpeakers);

                // Mapping
                var mappedResult = _mapper.Map<IEnumerable<TalkModel>>(results);

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                // TODO: Add logging
                return InternalServerError(ex);
            }
        }

        [Route("{id:int}")]
        public async Task<IHttpActionResult> Get(string moniker, int id, bool includeSpeakers = false)
        {
            try
            {
                var result = await _repository.GetTalkByMonikerAsync(moniker, id, includeSpeakers);

                if (result == null)
                {
                    return NotFound();
                }

                var mappedResult = _mapper.Map<TalkModel>(result);

                return Ok(mappedResult);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}