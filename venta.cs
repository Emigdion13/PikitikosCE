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
    
    public partial class venta
    {
        public long idVenta { get; set; }
        public System.DateTime FechaVenta { get; set; }
        public string GenID { get; set; }
        public float Precio { get; set; }
        public int Cantidad { get; set; }
        public Nullable<float> Descuento { get; set; }
        public Nullable<float> Credito { get; set; }
        public Nullable<int> IdPersona { get; set; }
        public string DescripcionArticulo { get; set; }
    
        public virtual persona persona { get; set; }
    }
}
