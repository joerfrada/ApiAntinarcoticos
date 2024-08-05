using System;
using System.Collections.Generic;

namespace Core.Entidades
{
    public class UsuarioEntity
    {
        public Int64 Usuario_id { get; set; }
        public string Usuario { get; set; }
        public string Nombre_completo { get; set; }
        public string Email_institucional { get; set; }
        public Boolean Activo { get; set; }
        public List<MenuEntity> Menus { get; set; }
    }
}
