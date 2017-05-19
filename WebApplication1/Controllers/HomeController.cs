using System.Web.Mvc;
using unirest_net.http;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    }
public class RootObject
{
    public string quote { get; set; }
    public string character { get; set; }
}
public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Quotes()
        {


          

            return View();
        }

    [HttpPost]

    public PartialViewResult ParseQuote(string character) 
    {
        HttpResponse<string> response = Unirest.get("https://got-quotes.herokuapp.com/quotes?char="+character)
        .asJson<string>();
        var RndQtGoT = JsonConvert.DeserializeObject<RootObject>(response.Body);
        HttpResponse<string> jsonResponse = Unirest.get("https://yoda.p.mashape.com/yoda?sentence=" + RndQtGoT.quote)
  .header("X-Mashape-Authorization", "ejWTgwZXAlmshfC6wOWKifd9Am2Lp1OrhYkjsnfOF0oNRh1L9f")
  .asJson<string>();
        ViewBag.Quote = RndQtGoT.quote;
        ViewBag.Character = RndQtGoT.character;
        ViewBag.Message = jsonResponse.Body;
        return PartialView("_result");
    }

}
