using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tarefas.Extended;
using Tarefas.Models;

namespace tarefa.Controllers
{
    public class TimesController : Controller
    {
        private ContextoDB db = new ContextoDB();
        public ActionResult Entrar()
        {
            var usuarioTarefaModel = new UsuariosTarefasModel
            {
                NomeCompleto = User.GetFullName(),
                Apelido = User.GetNickName(),
                Email = User.GetEmail()
            };

            db.Usuarios.Add(usuarioTarefaModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public  ActionResult Index()
        {
            var apelido = User.GetNickName();
            var times = db.Times.Where(w => w.Dono == apelido).ToList();

            return View(times);
        }

        public ActionResult Cadastro(int? id)
        {
            var timeModel = new TimeModel();

            timeModel.Dono = User.GetNickName();
            if (id != null || id > 0)
            {
                timeModel = db.Times.Find(id);
                if (timeModel.Equals(null))
                {
                    return HttpNotFound();
                }
            }
            return View(timeModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro([Bind(Include = "Id,Nome,Dono")] TimeModel timeModel)
        {
            if (ModelState.IsValid)
            {
                timeModel.Dono = User.GetNickName();
                if (timeModel.Id > 0 )
                {
                    db.Entry(timeModel).State = EntityState.Modified;
                }
                else
                {
                    db.Times.Add(timeModel);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timeModel);
        }

        [HttpPost]
        public ActionResult Excluir(int Id)
        {
            var excluir = true;
            var msg = string.Empty;
            using (var transac = db.Database.BeginTransaction())
            {
                try
                {
                    var sqlExclusaoTarefas = $@"DELETE FROM TarefaModel WHERE  TimeModelId={Id}";
                    var sqlExclusaoUsuario = $@"DELETE FROM UsuarioTimesModel WHERE  TimeModelId = {Id}";

                    TimeModel timeModel = db.Times.Find(Id);
                    db.Times.Remove(timeModel);

                    db.Database.ExecuteSqlCommand(sqlExclusaoTarefas);
                    db.Database.ExecuteSqlCommand(sqlExclusaoUsuario);
                    db.SaveChanges();

                    transac.Commit();
                }
                catch (Exception ex)
                {
                    msg = string.IsNullOrWhiteSpace(ex.Message) ? ex.InnerException.Message : ex.Message;
                    excluir = false;
                }
            }
            return Json(new { excluir, msg });
        }
        #region Gerenciamento  de usuarios

        public  ActionResult UsuariosTimes(int Id)
        {
            var SQL = $@"SELECT U.* FROM UsuariosTimesModel ut
                       JOIN UsuariosTarefasNodel u ON ut.usuariosTarefasModelId = u.Id
                        WHERE ut.TimesModelId= {Id}";

            var usuarios = db.Database.SqlQuery<UsuariosTarefasModel>(SQL);
            ViewBag.Time = Id;

            return View(usuarios);
        }

        public ActionResult BuscarUsuarios(int Id)
        {
            ViewBag.Time = Id;

            return View();
        }

        public ActionResult PesquisarUsuario(string Apelido)
        {
            var dono = User.GetNickName();
            var sql = string.Format($@"SELECT * FROM  UsuarioTarefasModel WHERE Apelido LIKE %'{Apelido}'% and Apelido not like '{dono}");
            var usuarios = db.Database.SqlQuery<UsuariosTarefasModel>(sql).ToList();
            return Json(usuarios, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddUsuarioTimes(int id, int IdUsuario)
        {
            var gravou = true;
            var msg = string.Empty;

            if (id >0 && IdUsuario > 0 )
            {
                var sql = string.Format($@"IF EXISTS(SELECT * FROM UsuariosTimeModel WHERE TimeModelId = {id} and UsuariosTarefasModelId = {IdUsuario})
                                            BEGUIN
                                            DELETE UsuariosTimesModel Where TimeModelId = {id} AND Usuarios TarefasModelId = {IdUsuario}
                                            END
                                          ELSE
                                            BEGUIN
                                                INSERT INTRO Usuarios TimesModel VALUES({IdUsuario},{id})
                                            END)");
                try
                {
                    db.Database.ExecuteSqlCommand(sql);
                }
                catch (Exception ex)
                {
                    msg = string.IsNullOrWhiteSpace(ex.Message)
                        ? ex.InnerException.Message
                        : ex.Message;
                    gravou = false;
                }
                return Json(new { gravou, msg });
            }
            return Json(new { gravou = false, msg = "Paramentros Incorretos!" });
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }


    }
}
