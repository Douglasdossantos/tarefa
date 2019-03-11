using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tarefas.Models
{
    public class TarefasModel
    {
        [Key]
        public int Id { get; set; }

        [Required (ErrorMessage ="* Obrigatório")]
        [Display(Name ="Título")]
        public string Titulo { get; set; }

        [Display(Name ="Descrição")]
        public string Descricao { get; set; }

        [Required(ErrorMessage ="Obrigatorio")]
        public string Criador{ get; set; }

        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }

        [Required(ErrorMessage = "* Obrigatório")]
        [Display(Name = "Criado em:")]
        public DateTime DataCriacao { get; set; }

        [Display(Name = "Concluído em:")]
        public DateTime? DataConclusao { get; set; }

        [Display(Name = "Agendado Para:")]
        public DateTime? DataAgendamento { get; set; }

        public int TiemModelId { get; set; }

        public virtual TimeModel  Time { get; set; }
    }
}