using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using OpenAI;
//using OpenAI.Chat;
using Mscc.GenerativeAI;

namespace FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AIAssistantController : ControllerBase
    {
        IConfiguration _con;
        public AIAssistantController(IConfiguration con)
        {
            _con = con;
        }

        [HttpGet("Start-AI-Assistant")]
        public IActionResult Get()
        {


            var googleAI = new GoogleAI(apiKey: _con["OpenAiKey"]);
            var model = googleAI.GenerativeModel(model: Model.Gemini10Pro);
            var prompt = "say: Hello, How can I assist you?";


            var response = model.GenerateContent(prompt).Result;
            return Ok(response.Text);

        }
        [HttpPost("Ask-Any-Question")]
        public IActionResult AskAnyQuestion(string quesiton)
        {


            var googleAI = new GoogleAI(apiKey: _con["OpenAiKey"]);
            var model = googleAI.GenerativeModel(model: Model.Gemini10Pro);


            var response = model.GenerateContent(quesiton).Result;
            return Ok(response.Text);

        }

    }
}

//private readonly OpenAIClient _openAIClient;

//public AIAssistantController(OpenAIClient openAIClient)
//{
//    _openAIClient = openAIClient;
//}

//[HttpGet("AskAnyQuestion")]
//public IActionResult AskAnyQuestion()
//{

//    ChatClient client = 
//        new(model: "gpt-3.5-turbo", "sk-proj-mfFZOZRd51JTiR2BpRnWT3BlbkFJnuXVl7i82kk7YnjmTJHM");
//    var completion = client.CompleteChat("Say this is a test.");

//    return Ok(completion.Value);
//}