using Business.Interfaces;
using Entity.DTA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;

namespace RuletaOnline.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RuletaController : ControllerBase
    {
        #region Properties
        private readonly ILogger<RuletaController> _logger;
        private readonly IRuletaBusiness _business;
        #endregion

        #region Constructor
        public RuletaController(ILogger<RuletaController> logger, IRuletaBusiness business)
        {
            _business = business ?? throw new ArgumentNullException(nameof(business));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        #endregion

        #region Methods
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CreateRoulette()
        {
            try
            {
                return Ok(await _business.CreateRouletteAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError("Se presento el siguiente error: {0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> OpenRouletteById([Required] string id)
        {
            try
            {
                return Ok(await _business.OpenRouletteByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("Se presento el siguiente error: {0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CreateBet([FromHeader(Name = "userId")][Required] string userId,
           [FromBody][Required] BetRequest betRequest)
        {
            try
            {
                betRequest.User = userId;
                return Ok(await _business.CreateBet(betRequest));
            }
            catch (Exception ex)
            {
                _logger.LogError("Se presento el siguiente error: {0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(RouletteResult), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<RouletteResult>> ClosedRouletteById([Required] string id)
        {
            try
            {
                return Ok(await _business.ClosedRouletteById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError("Se presento el siguiente error: {0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(List<RouletteDetail>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<RouletteDetail>>> GetRoulettes()
        {
            try
            {
                return Ok(await _business.GetRoulettes());
            }
            catch (Exception ex)
            {
                _logger.LogError("Se presento el siguiente error: {0}", ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message.ToString());
            }
        }
        #endregion   
    }
}
