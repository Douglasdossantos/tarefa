using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarefas.Extended;
using Tarefas.Models;

namespace Tarefas.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var fullName = string.Empty;
            var nickName = string.Empty;
            var email = string.Empty;
            var usarTarefas = false;

            if (User.Identity.IsAuthenticated)
            {
                fullName = User.GetFullName();
                nickName = User.GetNickName();
                email = User.GetEmail();

                var db = new ContextoDB();
                var apelido = User.GetNickName();

                usarTarefas = db.Usuarios.Where(w => w.Apelido == apelido).Count() > 0;
            }
            ViewBag.FullName = fullName;
            ViewBag.nickName = nickName;
            ViewBag.email = email;
            ViewBag.usarTarefas = usarTarefas;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}