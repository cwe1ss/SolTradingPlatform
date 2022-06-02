using FormDraftService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FormDraftService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DraftController : ControllerBase
    {
        //DB aufsetzen und ein Form installieren
        private static List<FormDraft> _allFormDrafts = new List<FormDraft>()
        {
            new FormDraft()
            {
                FormId = "0815",
                Name = "Motivationsumfrage",
                Status = "draft",
                Description = "Umfrage zur allgemeinen Motivation am heutigen Tag.",
                Questions = new[]
                {
                    new Question()
                    {
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


        // GET api/Draft/{id}
        [HttpGet("{id}")]
        public ActionResult<FormDraft> Get(string id)
        {
            return _allFormDrafts.FirstOrDefault(x => x.FormId == id) is FormDraft draft
                ? Ok(draft)
                : NotFound();
        }

        // POST api/Draft
        [HttpPost]
        public ActionResult<FormDraft> Post([FromBody] FormDraft value)
        {
            if (_allFormDrafts.Any(x => x.FormId == value.FormId))
            {
                return Conflict();
            }

            _allFormDrafts.Add(value);

            return value;
        }

        // PUT api/Draft/{id}
        [HttpPut("{id}")]
        public ActionResult<FormDraft> Put(int id, [FromBody] FormDraft value)
        {
            // Replace the object in our list
            _allFormDrafts.RemoveAll(x => x.FormId == value.FormId);
            _allFormDrafts.Add(value);

            return value;
        }
    }
}
