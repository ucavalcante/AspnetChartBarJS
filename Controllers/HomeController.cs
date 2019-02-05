using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetmvc002.Models;
using Raven.Embedded;

namespace aspnetmvc002.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var pessoas = new List<Pessoa>();
            var store = EmbeddedServer.Instance.GetDocumentStore("Pessoas");
            using (var session = store.OpenSession())
            {
                var p = session.Query<Pessoa>().ToList();
                pessoas.AddRange(p);
            }
            return View("Index", pessoas);
        }

        [HttpGet]
        public IActionResult Cadastro()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Pessoa pessoa)
        {
            pessoa.UF = pessoa.UF.ToUpper();
            var store = EmbeddedServer.Instance.GetDocumentStore("Pessoas");
            using (var session = store.OpenSession())
            {
                session.Store(pessoa);
                session.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
