﻿using FormDraftService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuestionnaireAnswersService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireAnswersServiceController : ControllerBase
    {


        private static List<FormDraft> _allFormDraft = new List<FormDraft>()
        {
           // new FormDraft()
           
        };
        public static string FormDraftServiceBaseAddress = "https://configservice20220507144709.azurewebsites.net/";       


        [HttpGet("{serviceType}")]
        public IActionResult Get(string serviceType)
        {
            //FormDraftService
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(FormDraftServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(FormDraftServiceBaseAddress + "/api/LoadBalancer/"+ serviceType).Result;
            response.EnsureSuccessStatusCode();

            return Ok(response.Content.ReadAsStringAsync().Result);


        }
        [HttpGet]
        public object Get()
        {
            return _allFormDraft;
        }

        // POST api/<QuestionnaireAnswersServiceController>
        [HttpPost]
        public IActionResult Post([FromBody] FormDraft formDraft)
        {
            //check FormDraft for error
            //checkFormDraft(_allFormDraft);
            //Ausgefüllten Fragebogen speichern 
            _allFormDraft.Add(formDraft);
            return Ok(formDraft);
            //send event to FragebogenAusgefülltEvent 

        }
    }
}
