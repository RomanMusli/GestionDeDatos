using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrbaHotel.Model
{
    class Cliente
    {
        public int id;
        public string nombre;
        public string apellido;
        public Identificacion identificacion;
        public string mail;
        public int telefono;
        public string direccion;
        public string nacionalidad;
        public DateTime fechaNacimiento;
        public bool activado;
        public char depto;
        public int piso;
    }
}
