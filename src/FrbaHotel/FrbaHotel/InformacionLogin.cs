using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrbaHotel.Model;

namespace FrbaHotel
{
    static class InformacionLogin
    {
        private static Usuario _usuarioDeSesion;

        public static Usuario UsuarioDeSesion{
            get { return _usuarioDeSesion; }
            set { _usuarioDeSesion = value; }
        }
    }
}
