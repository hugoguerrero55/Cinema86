//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cinema86Movie.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CN86_Estatus_Usuario
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CN86_Estatus_Usuario()
        {
            this.CN86_Usuario = new HashSet<CN86_Usuario>();
        }
    
        public int Id_Estatus { get; set; }
        public string Descripcion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CN86_Usuario> CN86_Usuario { get; set; }
    }
}
