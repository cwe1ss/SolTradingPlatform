using FormDraftService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FormDraftService.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class DraftController : ControllerBase
    {
        //DB aufsetzen und ein Form installieren
        private static IEnumerable<FormDraft> _allFormDrafts = new List<FormDraft>()
        {
            new FormDraft(){ 
                FormId = "0815",
                Name = "Motivationsumfrage",
                Description = "Umfrage zur allgemeinen Motivation am heutigen Tag.",
                Questions = new[]
                {
                    new Question(){
                        QuestionId = "1",
                        Options = new[]
                        {
                            "Wenig motiviert",
                            "neutral",
                            "Voll motiviert!"
                        },
                        Text = "Wie fühlen Sie sich heute?"
                        }
                }
            }
        };


        // GET api/<DraftController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return _allFormDrafts.ToString();
        }

        // POST api/<DraftController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<DraftController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<DraftController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
