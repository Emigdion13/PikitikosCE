//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PikitikosCE
{
    using System;
    using System.Collections.Generic;
    
    public partial class inventario
    {
        public int idInventario { get; set; }
        public string GenId { get; set; }
        public int ArticuloLink { get; set; }
        public int CaracteristicasLink { get; set; }
        public int ColorLink { get; set; }
        public int TallaLink { get; set; }
        public int TipoArticuloLink { get; set; }
        public int TipoTelaLink { get; set; }
        public int Disponible { get; set; }
        public Nullable<float> Precio { get; set; }
        public sbyte visible { get; set; }
    
        public virtual articulo articulo { get; set; }
        public virtual caracteristica caracteristica { get; set; }
        public virtual color color { get; set; }
        public virtual talla talla { get; set; }
        public virtual tipoarticulo tipoarticulo { get; set; }
        public virtual tipotela tipotela { get; set; }
    }
}