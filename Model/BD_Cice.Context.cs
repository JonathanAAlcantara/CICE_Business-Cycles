﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CICE_Business_Cycles.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BD_CICE_UAEMexEntities2 : DbContext
    {
        public BD_CICE_UAEMexEntities2()
            : base("name=BD_CICE_UAEMexEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<APIBanxicoNodos> APIBanxicoNodos { get; set; }
        public virtual DbSet<BDAPIBanxico> BDAPIBanxico { get; set; }
        public virtual DbSet<BDINEGI> BDINEGI { get; set; }
        public virtual DbSet<INEGINodos> INEGINodos { get; set; }
    }
}
