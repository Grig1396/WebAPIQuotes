using System.Web;
using System.Web.Mvc;
using unirest_net.http;
using Newtonsoft.Json;


public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult Quotes()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RequestAPI(string character)
        {
            ParseQuote(character);
           var jsonResponse = Yodafication(ViewBag.Quote);
            ViewBag.Message = jsonResponse.Body.ToString();
            string js = jsonResponse.Body.ToString();
            NumbersFact(js.Length, js.Length);
            return PartialView("_result");
        }
    public void ParseQuote(string character) 
    {
        HttpResponse<string> response = Unirest.get("https://got-quotes.herokuapp.com/quotes?char="+character)
        .asJson<string>();
        var JsonQuote = JsonConvert.DeserializeObject<QuoteObject>(response.Body);
        ViewBag.Quote = JsonQuote.Quote;
        ViewBag.Character = JsonQuote.Character;
        
    }

        public HttpResponse<string> Yodafication(string quote)
        {
        HttpResponse<string> jsonResponse = Unirest.get("https://yoda.p.mashape.com/yoda?sentence=" + quote)
            .header("X-Mashape-Authorization", "ejWTgwZXAlmshfC6wOWKifd9Am2Lp1OrhYkjsnfOF0oNRh1L9f")
            .asJson<string>();
        return jsonResponse;

        }
    public void NumbersFact(int min, int max)
    {
        HttpResponse<string> response = Unirest
            .get("https://numbersapi.p.mashape.com/random/" + "trivia" + "?fragment=true&json=true&max=" + max +
                 "&min=" + min)
            .header("X-Mashape-Key", "GILTcZVs1dmsh5CIFViX3zp0NDdEp1hnk2djsnVMUYlTZ4uVdn")
            .header("Accept", "text/plain")
            .asString();
        var Numbers = JsonConvert.DeserializeObject<NumberObject>(response.Body);
        ViewBag.text = Numbers.text;
        ViewBag.type = Numbers.type;
        ViewBag.number = Numbers.number;
    }
}
