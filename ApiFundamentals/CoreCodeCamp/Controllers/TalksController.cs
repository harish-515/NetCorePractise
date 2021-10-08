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
    [Route("api/camps/{moniker}/[controller]")]
    [ApiController]
    public class TalksController : ControllerBase
    {
        private readonly ICampRepository campRepository;
        private readonly IMapper mapper;
        private readonly LinkGenerator linkGenerator;

        public TalksController(ICampRepository campRepository, IMapper mapper, LinkGenerator linkGenerator)
        {
            this.campRepository = campRepository;
            this.mapper = mapper;
            this.linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<TalkModel[]>> Get(string moniker)
        {
            try
            {
                var talks = await campRepository.GetTalksByMonikerAsync(moniker, true);
                if (talks == null)
                {
                    return NotFound($"No talks found for Camp moniker {moniker}");
                }

                return mapper.Map<TalkModel[]>(talks);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }
        }

        [HttpGet("{talkId:int}")]
        public async Task<ActionResult<TalkModel>> GetTalk(string moniker, int talkId)
        {
            try
            {
                var talk = await campRepository.GetTalkByMonikerAsync(moniker, talkId, true);
                if (talk == null)
                {
                    return NotFound($"Unable to find talk with id {talkId} under the camp with moniker : {moniker}");
                }

                return mapper.Map<TalkModel>(talk);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TalkModel>> Post(string moniker, TalkModel model)
        {
            try
            {
                var camp = await campRepository.GetCampAsync(moniker);
                if (camp == null)
                {
                    return NotFound($"Unable to find Camp with moniker {moniker}.");
                }

                var speaker = await campRepository.GetSpeakerAsync(model.Speaker.SpeakerId);
                if (speaker == null)
                {
                    return NotFound($"Unable to find speaker with id {model.Speaker.SpeakerId}.");
                }

                var talk = mapper.Map<Talk>(model);
                talk.Camp = camp;
                talk.Speaker = speaker;
                campRepository.Add(talk);

                if (await campRepository.SaveChangesAsync())
                {
                    var location = linkGenerator.GetPathByAction("Get", "Talks", new { moniker = moniker, id = talk.TalkId });
                    if (string.IsNullOrEmpty(location))
                    {
                        return BadRequest("Unable to create a url for this requeat");
                    }
                    return Created(location, mapper.Map<TalkModel>(talk));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }

            return BadRequest("Unable to process the request.");
        }

        [HttpPut("{talkId:int}")]
        public async Task<ActionResult<TalkModel>> Put(string moniker, int talkId, TalkModel model)
        {
            try
            {
                var talk = await campRepository.GetTalkByMonikerAsync(moniker, talkId,true);
                if (talk == null)
                {
                    return NotFound($"Unable to find talk with id {talkId}.");
                }

                mapper.Map<TalkModel, Talk>(model, talk);
                
                if(model.Speaker != null)
                {
                    var speaker = await campRepository.GetSpeakerAsync(model.Speaker.SpeakerId);
                    if (speaker == null)
                    {
                        return NotFound($"Unable to find speaker with id {model.Speaker.SpeakerId}.");
                    }
                    talk.Speaker = speaker;
                }

                if (await campRepository.SaveChangesAsync())
                {
                    return Ok(mapper.Map<TalkModel>(talk));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Error.");
            }
            return BadRequest("Unable to process the request.");

        }


        [HttpDelete("{talkId:int}")]
        public async Task<IActionResult> Delete(string moniker,int talkId)
        {
            try
            {
                var exisitngTalk = await campRepository.GetTalkByMonikerAsync(moniker,talkId);
                if (exisitngTalk == null)
                    return NotFound($"Unable to find talk with id {talkId} under the camp with moniker : {moniker}");

                campRepository.Delete(exisitngTalk);

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
