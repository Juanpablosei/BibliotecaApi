﻿using BibliotecaApi.Entidades;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaApi.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Autor>Autores  { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> comentarios { get; set; }
        public DbSet<AutorLibro>AutorLibros { get; set; }   

    }
}
