using System;
using System.Collections.Generic;

namespace Core.Entidades
{
    public class MenuEntity
    {
        public Int64 Menu_id { get; set; }
        public String Nombre_menu { get; set; }
        public String Tipo_menu { get; set; }
        public Int64 Menu_padre_id { get; set; }
        public String Cod_modulo { get; set; }
        public String Icono { get; set; }
        public String Ruta { get; set; }        
        public Boolean Activo { get; set; }
        public Boolean Collapsed { get; set; }
        public List<MenuEntity> Menus { get; set; }
    }
}
