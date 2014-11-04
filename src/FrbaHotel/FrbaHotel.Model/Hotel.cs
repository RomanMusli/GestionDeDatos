using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace FrbaHotel.Model
{
    public class Hotel
    {
        public int id;
        public string nombre;
        public int telefono;
        public string calle;
        public int cantEstrellas;
        public string ciudad;
        public string pais;
        public DateTime fechaCreacion;
        public bool activo;
        public int nroCalle;

        public Hotel() { }
        public Hotel(int id, string nombre, int telefono, string calle, 
            int cantEstrellas, string ciudad, string pais, DateTime fechaCreacion, bool activo, int nroCalle) {
                this.id = id;
                this.nombre = nombre;
                this.telefono = telefono;
                this.calle = calle;
                this.cantEstrellas = cantEstrellas;
                this.ciudad = ciudad;
                this.pais = pais;
                this.fechaCreacion = fechaCreacion;
                this.activo = activo;
                this.nroCalle = nroCalle;
        }

        public override string ToString()
        {
            return this.nombre;
        }

        public static List<Hotel> obtenerTodosPorIdUsuario(int idUsuario)
        {
            List<Hotel> hoteles = new List<Hotel>();
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT * 
                                                 FROM No_La_Recurso.Hoteles h
                                                 INNER JOIN No_La_Recurso.Usuarios_Hoteles uh 
                                                 ON uh.id_hotel = h.id
                                                 WHERE uh.id_usuario = @idUsuario", dbConn);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@idUsuario", SqlDbType.Int);
                //Le pongo los valores a los parametros
                cmd.Parameters["@idUsuario"].Value = idUsuario;

                //Ejecuto la query
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var hotelNuevo = new Hotel((int)dr["id"], (string)dr["nombre"], (int)dr["telefono"],
                            (string)dr["calle"], (int)dr["cant_estrellas"], (string)dr["ciudad"], (string)dr["pais"], (DateTime)dr["fec_creacion"],
                            (bool)dr["activo"], (int)dr["nro_calle"]);
                        hoteles.Add(hotelNuevo);
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
            return hoteles;
        }
    }
}
