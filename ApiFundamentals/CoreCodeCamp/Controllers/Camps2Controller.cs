using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/camps")]
    [ApiVersion("2.0")]
    [ApiController]
    public class Camps2Controller : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;

        public Camps2Controller(ICampRepository campRepository,IMapper mapper,LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            LinkGenerator = linkGenerator;
        }

        public LinkGenerator LinkGenerator { get; }

        [HttpGet]
        public async Task<IActionResult> Get(bool includeTalks=false)
        {
            try
            {
                var results = await this.campRepository.GetAllCampsAsync(includeTalks);
                if (results == null)
                {
                    return NotFound();
                }
                return Ok(new {
                    count = results.Count(),
                    results = mapper.Map<CampModel[]>(results)
                });
            }
            catch(Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }
        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker,bool includeTalks = false)
        {
            try
            {
                var results = await this.campRepository.GetCampAsync(moniker,includeTalks);
                if (results == null)
                {
                    return NotFound();
                }
                return mapper.Map<CampModel>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> Get(DateTime theDate,bool includeTalks = false)
        {
            try
            {
                var results = await this.campRepository.GetAllCampsByEventDate(theDate,includeTalks);
                if (!results.Any())
                {
                    return NotFound();
                }
                return mapper.Map<CampModel[]>(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }
        }

        public async Task<ActionResult<CampModel>> Post(CampModel model)
        {
            try
            {
                //search for exisitng camp with same moniker
                var exisitng = await campRepository.GetCampAsync(model.Moniker);
                if (exisitng != null)
                    return BadRequest("Camp with same moniker already exists.");

                var location = LinkGenerator.GetPathByAction("GET",
                                "Camps", new { moniker = model.Moniker });

                if (string.IsNullOrEmpty(location))
                    return BadRequest("Unable to generate location url");

                var camp = mapper.Map<Camp>(model);
                campRepository.Add(camp);
                if(await campRepository.SaveChangesAsync())
                {
                    return Created(location, mapper.Map<CampModel>(camp));
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }

            return BadRequest("Unable to process the request.");
        }

        [HttpPut("{moniker}")]
        public async Task<ActionResult<CampModel>> Put(string moniker,CampModel model)
        {
            try
            {
                var exisitngCamp = await campRepository.GetCampAsync(moniker);
                if (exisitngCamp == null)
                    return NotFound($"Unable to find camp with moniker : {moniker}");

                // updating values from model to exisitng object
                mapper.Map<CampModel,Camp>(model, exisitngCamp);
                var res = await campRepository.SaveChangesAsync();
                if (res)
                {
                    return mapper.Map<CampModel>(exisitngCamp);
                }                
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }

            return BadRequest("Unable to process the request.");
        }

        [HttpDelete("{moniker}")]
        public async Task<IActionResult> Delete(string moniker)
        {
            try
            {
                var exisitngCamp = await campRepository.GetCampAsync(moniker);
                if (exisitngCamp == null)
                    return NotFound($"Unable to find camp with moniker : {moniker}");

                campRepository.Delete(exisitngCamp);

                if (await campRepository.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }

            return BadRequest("Unable to process the request.");
        }

    }
}
