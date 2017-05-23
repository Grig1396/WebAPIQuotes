﻿using System.Web;
using System.Web.Mvc;
using unirest_net.http;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;

public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };

    public ActionResult Quotes()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RequestAPI(string character)
        {
            ParseQuote(character);
            string jsonResponse = Yodafication(ViewBag.Quote);
            var Numbers = NumbersFact(RandomType(), jsonResponse.Length);
        // var Numbers = NumbersFact(RandomType(),js.Length);
        Dictionary<int, string> result = new Dictionary<int, string>();
        List<string> typeList = new List<string>();
        int i = 0;
        for (int k = 0; k < jsonResponse.Length-1; k++)
        {
            if (vowels.Contains(jsonResponse[k]))
            {
                i++;
                typeList.Add(RandomType());
                result.Add(i * Numbers.number, NumbersFact(typeList[i - 1], (i * Numbers.number)).text);
            }
        }
        ViewBag.Message = jsonResponse;
        ViewBag.text = Numbers.text;
        ViewBag.type = Numbers.type;
        ViewBag.number = Numbers.number;
        ViewBag.list = typeList;
        ViewBag.result = result;

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

        public string Yodafication(string quote)
        {
        // HttpResponse<string> jsonResponse = Unirest.get("https://yoda.p.mashape.com/yoda?sentence=" + quote)
        HttpResponse<string> jsonResponse = Unirest.get("https://yoda.p.mashape.com/yoda?sentence=" + quote)
             .header("X-Mashape-Authorization", "ejWTgwZXAlmshfC6wOWKifd9Am2Lp1OrhYkjsnfOF0oNRh1L9f")
            .asJson<string>();
        return Regex.Replace(jsonResponse.Body, "[ ]+", " ");

        }
    public dynamic NumbersFact(string type, int number)
    {
        HttpResponse<string> response = Unirest
            .get("https://numbersapi.p.mashape.com/"+ number + "/" + type + "?fragment=true&json=true")
            .header("X-Mashape-Key", "GILTcZVs1dmsh5CIFViX3zp0NDdEp1hnk2djsnVMUYlTZ4uVdn")
            .header("Accept", "text/plain")
            .asString();
        var Numbers = JsonConvert.DeserializeObject<NumberObject>(response.Body);
        return Numbers;
    }
    public string RandomType()
    {
        Random rnd = new Random();
        switch (rnd.Next(0, 3))
        {
            case 0:
                {
                    return "trivia";
                }
            case 1:
                {
                    return "math";
                }
            case 2:
                {
                    return "date";
                }
            default:
                {
                    return "year";
                }
        }

    }
}
