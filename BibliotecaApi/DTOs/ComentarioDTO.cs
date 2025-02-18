﻿using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.DTOs
{
    public class ComentarioDTO
    {
        public Guid Id { get; set; }
        public required string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
