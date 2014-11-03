using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrbaHotel.Model;

namespace FrbaHotel
{
    static class InformacionLogin
    {
        private static Usuario _usuarioVar;

        public static Usuario UsuarioVar{
            get { return _usuarioVar; }
            set { _usuarioVar = value; }
        }
    }
}
