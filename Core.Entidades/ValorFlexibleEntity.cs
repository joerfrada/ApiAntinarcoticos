using System;

namespace Core.Entidades
{
    public class TipoValorEntity
    {
        public Int64 Id_tipo_valor { get; set; }
        public String Tipo { get; set; }
        public String Descripcion { get; set; }
        public Boolean Activo { get; set; }
        public Int64 Id_tipo_valor_padre { get; set; }
        public String Tìpo_valor_padre { get; set; }
        public String Usuario_creador { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public String Usuario_modificador { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
        public String Tabla { get; set; }
    }

    public class ValorFlexibleEntity
    {
        public Int64 Id_valor_flexible { get; set; }
        public String Valor { get; set; }
        public String Sigla { get; set; }
        public Int64 Orden { get; set; }
        public Boolean Activo { get; set; }
        public Int64 Id_valor_flexible_padre { get; set; }
        public String Valor_flexible_padre { get; set; }
        public String Atributo1 { get; set; }
        public String Atributo2 { get; set; }
        public String Atributo3 { get; set; }
        public String Atributo4 { get; set; }
        public Int64 Id_tipo_valor { get; set; }
        public String Tipo { get; set; }
        public String Usuario_creador { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public String Usuario_modificador { get; set; }
        public DateTime? Fecha_modificacion { get; set; }
        public String Tabla { get; set; }
    }
}
