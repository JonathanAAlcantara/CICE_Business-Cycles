﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Prueba
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BD_CICE_UAEMexEntities : DbContext
    {
        public BD_CICE_UAEMexEntities()
            : base("name=BD_CICE_UAEMexEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<APIBanxicoNodos> APIBanxicoNodos { get; set; }
        public virtual DbSet<BDAPIBanxico> C_BDAPIBanxico_ { get; set; }
    }
}