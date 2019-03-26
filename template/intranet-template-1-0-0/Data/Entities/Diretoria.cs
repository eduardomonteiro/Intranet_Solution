﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using $safeprojectname$.Enums;

namespace $safeprojectname$.Entities
{
    [Table("Diretoria")]
    public class Diretoria
    {
        public int Id { get; set; }

        public TipoDiretoria Tipo { get; set; }

        [StringLength(250)]
        public string Nome { get; set; }

        [StringLength(250)]
        public string Foto { get; set; }

        [StringLength(250)]
        public string Cargo { get; set; }

        [StringLength(300)]
        public string Descricao { get; set; }
    }
}