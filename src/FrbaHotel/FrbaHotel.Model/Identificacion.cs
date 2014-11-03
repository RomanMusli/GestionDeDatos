using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace FrbaHotel.Model
{
    public class Identificacion
    {
        public int id;
        public string tipo;
        public int numero;


        public Identificacion() { }
        public Identificacion(int id, string tipo, int numero) {
            this.id = id;
            this.tipo = tipo;
            this.numero = numero;
        }

        internal static Identificacion buscarPorId(int id)
        {
            Identificacion nuevaIdentificacion = null;
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT TOP(1) *
                                                FROM No_La_Recurso.Identificaciones i
                                                WHERE i.id = @id", dbConn);

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
                        nuevaIdentificacion = new Identificacion((int) dr["id"], (string) dr["tipo"], (int) dr["numero"]);
                    }
                }
                dr.Close();
                return nuevaIdentificacion;
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
    }
}
