using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FrbaHotel.Model
{
    public class Funcionalidad
    {
        public int id;
        public string nombre;

        public Funcionalidad(int id, string nombre) {
            this.id = id;
            this.nombre = nombre;
        }

        public static List<Funcionalidad> buscarFuncionalidadesPorRol(int id)
        {
            List<Funcionalidad> nuevasFuncionalidades = new List<Funcionalidad>();
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {

                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT f.id, f.nombre
                                                FROM No_La_Recurso.Funcionalidades f
                                                INNER JOIN No_La_Recurso.Roles_Funcionalidades rf
                                                ON f.id = rf.id_funcionalidad
                                                WHERE rf.id_rol = @id", dbConn);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@id", SqlDbType.Int);

                //Le pongo los valores a los parametros
                cmd.Parameters["@id"].Value = id;

                //Ejecuto la query
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var nuevaFuncionalidad = new Funcionalidad((int) dr["id"], (string)dr["nombre"]);
                        nuevasFuncionalidades.Add(nuevaFuncionalidad);
                    }
                }
                dr.Close();
                return nuevasFuncionalidades;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbConn.Close();
            }
        }

        public static List<Funcionalidad> obtenerTodas()
        {
            List<Funcionalidad> nuevasFuncionalidades = new List<Funcionalidad>();
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {

                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT f.id, f.nombre
                                                FROM No_La_Recurso.Funcionalidades f", dbConn);

                //Ejecuto la query
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        var nuevaFuncionalidad = new Funcionalidad((int)dr["id"], (string)dr["nombre"]);
                        nuevasFuncionalidades.Add(nuevaFuncionalidad);
                    }
                }
                dr.Close();
                return nuevasFuncionalidades;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                dbConn.Close();
            }
        }

        public override string ToString()
        {
            return this.nombre;
        }
    }
}
