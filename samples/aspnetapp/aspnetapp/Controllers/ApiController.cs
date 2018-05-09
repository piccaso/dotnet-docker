using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using aspnetapp.Database;
using aspnetapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aspnetapp.Controllers
{
    [Route("[controller]/[action]")]
    public class ApiController : Controller
    {
        private readonly Repository _db;
        private readonly IApplicationLifetime _applicationLifetime;

        public ApiController(Repository db, IApplicationLifetime applicationLifetime)
        {
            _db = db;
            _applicationLifetime = applicationLifetime;
        }

        [HttpPost]
        public JsonResult InsertDummyData()
        {
            var question = new QuestionNode
            {
                Image = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="),
                Header = "A Cab hits a biker and drives off",
                Question = "You whant to report a crime but don't want to mention your name, choose your pain:",
                Answers = new List<AnswerNode>
                {
                    new AnswerNode() {
                        Answer = "Talk to an officer and refuse to show my id",
                        Score = 10,
                        Conclusion = "You will be charged with obstruction of justice",
                    },
                    new AnswerNode() {
                        Answer = "Use my Phone, disable Caller Id and quickly hang up",
                        Score = 8,
                        Conclusion = "They will call you right back - Caller Id suppression is an opt in feature at the terminatig end"
                    },
                    new AnswerNode() {
                        Answer = "Send an Email",
                        Score = 10,
                        Conclusion = "Emails are by no means anonymous - Even if advertised as secure these days, that is only transport security"
                    },
                }
            };

            _db.InsertQuestion(question);

            return Json(question);
        }

        [HttpGet]
        public JsonResult GetAllQuestions()
        {
            return Json(_db.GetAllQuestions());
        }

        [HttpGet]
        public JsonResult GetNextQuestions(int? ammount = null)
        {
            return Json(_db.GetNextQuestions(ammount ?? 1));
        }

        [HttpDelete]
        public IActionResult DeleteDatabase()
        {
            _db.Truncate();
            return Ok();
        }

        [HttpPatch]
        public IActionResult StopApplication()
        {
            _applicationLifetime.StopApplication();
            return Ok();
        }

        [HttpGet]
        public IActionResult GetHeaders()
        {
            dynamic r = new ExpandoObject();
            
            if(HttpContext.Request.Headers.TryGetValue("Accept-Language", out var acceptLanguage))
            {
                var req = new HttpRequestMessage();
                if (req.Headers.AcceptLanguage.TryParseAdd(acceptLanguage))
                {
                    r.lang = req.Headers.AcceptLanguage;
                }
            }

            r.headers = HttpContext.Request.Headers;
            return Json(r);
        }

        public static bool BadHealth = false;

        [HttpGet]
        public IActionResult HealthCheck()
        {
            if(BadHealth) throw new Exception("bad health");
            return Ok();
        }

        [HttpPut]
        public IActionResult SetBadHealth(bool value)
        {
            BadHealth = value;
            return Json(BadHealth);
        }

    }
}
