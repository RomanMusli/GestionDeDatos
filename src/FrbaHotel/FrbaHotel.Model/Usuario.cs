using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows.Forms;
using System.Security.Cryptography;

namespace FrbaHotel.Model
{
    public class Usuario
    {
        public int id { get; set; }
        public int intentos;
        public bool activo;
        public string username;
        public string password;
        public string nombre;
        public string apellido;
        public Identificacion identificacion;
        public string mail;
        public int telefono;
        public string direccion;
        public DateTime fechaNacimiento;
        public List<Rol> roles;

        public Usuario() {
            this.roles = new List<Rol>();
            this.identificacion = new Identificacion();
        }

        public Usuario(int id, int intentos, bool activo, 
            string username, string password, string nombre, string apellido, string mail, int telefono, string direccion, DateTime fechaNacimiento) {
            this.roles = new List<Rol>();
            this.identificacion = new Identificacion();
            this.id = id;
            this.intentos = intentos;
            this.activo = activo;
            this.username = username;
            this.password = password;
            this.nombre = nombre;
            this.apellido = apellido;
            this.mail = mail;
            this.telefono = telefono;
            this.direccion = direccion;
            this.fechaNacimiento = fechaNacimiento;
        }

        public static Usuario CheckUsuarioYPassword(string username, string password)
        {
            
            Usuario usuarioNuevo = null;
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT TOP(1) *
                                                FROM No_La_Recurso.Usuarios u
                                                WHERE u.username = @usuario
                                                AND u.password = @password", dbConn);
                
                //Encodeo la password
                Byte[] passwordEnBytes = Encoding.UTF8.GetBytes(password);
                Byte[] passwordEncriptada = new SHA256Managed().ComputeHash(passwordEnBytes);
                
                //Defino los parametros a utilizar
                cmd.Parameters.Add("@usuario", SqlDbType.VarChar, 30);
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 256);
                
                //Le pongo los valores a los parametros
                cmd.Parameters["@usuario"].Value = username;
                cmd.Parameters["@password"].Value = BitConverter.ToString(passwordEncriptada).ToString();

                //Ejecuto la query
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows) {
                    while (dr.Read()) {
                        usuarioNuevo = new Usuario((int)dr["id"], (int)dr["intentos"], (bool)dr["activo"],
                            (string)dr["username"], (string)dr["password"], (string)dr["nombre"], (string)dr["apellido"], (string)dr["mail"],
                            (int) dr["telefono"], (string)dr["direccion"], (DateTime) dr["fecha_nacimiento"]);
                        usuarioNuevo.identificacion = Identificacion.buscarPorId((int) dr["id_identificacion"]);
                        usuarioNuevo.roles = Rol.buscarPorIdDeUsuario((int) dr["id"]);
                    }
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbConn.Close();
            }
            return usuarioNuevo;
        }


        public void agregarIntentoFallido()
        {
            if ((this.intentos + 1) > 2)
            {
                //intento fallido, tengo que inhabilitar el usuario, update
            }
            else {
                this.intentos += 1;
            }
            
        }

        public void resetarIntentos()
        {
            this.intentos = 0;
            //update en la base
        }
    }
}
