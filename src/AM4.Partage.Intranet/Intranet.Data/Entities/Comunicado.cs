﻿using Intranet.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Intranet.Data.Entities
{
    [Table("Comunicado")]
    public class Comunicado
    {
        
        public Comunicado()
        {
            Anexos = new HashSet<ComunicadoAnexo>();
        }

        public int Id { get; set; }

        [Display(Name = "Título *")]
        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Cultura *")]
        public TipoComunicado Tipo { get; set; }

        public DateTime Data { get; set; }

        [DataType(DataType.Text)]
        public string Texto { get; set; }

        public bool Destaque { get; set; }

        [Display(Name = "Imagem Destaque")]
        [StringLength(250)]
        public string Imagem { get; set; }

        public string Shoppings { get; set; }
        public string Sexo { get; set; }
        public string Cargos { get; set; }

        [Required(ErrorMessage = "Anexo é Obrigatório")]
        [Display(Name = "Anexos *")]
        public virtual ICollection<ComunicadoAnexo> Anexos { get; set; }
    }
}