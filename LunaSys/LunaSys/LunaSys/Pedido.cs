//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WindowsFormsApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pedido
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pedido()
        {
            this.ItensPedidos = new HashSet<ItensPedido>();
        }
    
        public int ID_Pedido { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public Nullable<int> ID_PedidoCliente { get; set; }
        public Nullable<int> ID_PedidoUsuario { get; set; }
    
        public virtual Cliente Cliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItensPedido> ItensPedidos { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
