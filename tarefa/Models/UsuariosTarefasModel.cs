using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarefas.Models
{
    public class UsuariosTarefasModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="Obrigatório")]
        public string Apelido { get; set; }

        [Required(ErrorMessage ="Obrigatório")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage ="Obrigatório")]
        public string Email { get; set; }

        public virtual ICollection<TimeModel> Times { get; set; }

    }
}