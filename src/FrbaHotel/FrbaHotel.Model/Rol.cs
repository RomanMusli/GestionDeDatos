using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FrbaHotel.Model
{
    public class Rol
    {
        public int id;
        public string nombre;
        public bool activo;
        public List<Funcionalidad> funcionalidades;

        public Rol() {
            this.funcionalidades = new List<Funcionalidad>();
        }

        public Rol(int id, string nombre, bool activo) {
            this.id = id;
            this.nombre = nombre;
            this.activo = activo;
            this.funcionalidades = new List<Funcionalidad>();
        }

        public static List<Rol> buscarPorIdDeUsuario(int id)
        {
            List<Rol> nuevosRoles = new List<Rol>();
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {

                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT r.id, r.nombre, r.activo
                                                FROM No_La_Recurso.roles r
                                                INNER JOIN No_La_Recurso.Usuarios_Roles ur ON r.id = ur.id_rol
                                                WHERE ur.id_usuario = @id", dbConn);

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
                        var nuevoRol = new Rol((int)dr["id"], (string)dr["nombre"], (bool)dr["activo"]);
                        nuevoRol.funcionalidades = Funcionalidad.buscarFuncionalidadesPorRol(nuevoRol.id);
                        nuevosRoles.Add(nuevoRol);
                    }
                }
                dr.Close();
                return nuevosRoles;
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

        public static DataTable obtenerTodos()
        {
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT *
                                                FROM No_La_Recurso.Roles", dbConn);

                //Creo el dataAdapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //Creo el dataSet
                DataSet ds = new DataSet();
                //Lleno el dataSet
                da.Fill(ds, "roles");
                //Obtengo la info de la tabla
                DataTable dt = ds.Tables["roles"];
                //La muestro en el datagrid
                return dt;
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

        public void eliminar()
        {
            this.activo = false;
            //updatear en la base
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"UPDATE No_La_Recurso.Roles
                                                SET activo = 0
                                                WHERE id = @id", dbConn);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@id", SqlDbType.Int);

                //Le pongo los valores a los parametros
                cmd.Parameters["@id"].Value = this.id;

                //Ejecuto la query
                cmd.ExecuteNonQuery();
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


        public static Rol obtenerRolPorId(int id)
        {

            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            Rol nuevoRol = new Rol();
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT *
                                                FROM No_La_Recurso.Roles r
                                                WHERE r.id = @id", dbConn);

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
                        nuevoRol.id = (int)dr["id"];
                        nuevoRol.nombre = (string)dr["nombre"];
                        nuevoRol.activo = (bool)dr["activo"];
                        nuevoRol.funcionalidades = Funcionalidad.buscarFuncionalidadesPorRol(nuevoRol.id);
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
            return nuevoRol;
        }

        public static void agregarNuevoRol(string nombre, bool activo, List<Funcionalidad> funcionalidades)
        {
            //TODO: Hacerlo en un SP
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"INSERT INTO No_La_Recurso.Roles
                                                 (nombre
                                                 ,activo)
                                                OUTPUT Inserted.id
                                                VALUES
                                                (@nombre,
                                                @activo)", dbConn);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@activo", SqlDbType.Bit);

                //Le pongo los valores a los parametros
                cmd.Parameters["@nombre"].Value = nombre;
                cmd.Parameters["@activo"].Value = activo;

                //Ejecuto la query
                var nuevoIdRol = cmd.ExecuteScalar();
                foreach (Funcionalidad funcionalidad in funcionalidades){
                    SqlCommand cmdF = new SqlCommand(@"INSERT INTO No_La_Recurso.Roles_Funcionalidades
                                                     (id_rol
                                                     ,id_funcionalidad)
                                                    VALUES
                                                    (@id_rol,
                                                    @id_funcionalidad)", dbConn);

                    //Defino los parametros a utilizar
                    cmdF.Parameters.Add("@id_rol", SqlDbType.Int);
                    cmdF.Parameters.Add("@id_funcionalidad", SqlDbType.Int);

                    //Le pongo los valores a los parametros
                    cmdF.Parameters["@id_rol"].Value = nuevoIdRol;
                    cmdF.Parameters["@id_funcionalidad"].Value = funcionalidad.id;

                    cmdF.ExecuteNonQuery();
                }

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

        public void update(string nombre, bool activo, List<Funcionalidad> funcionalidades)
        {
            //TODO: Hacerlo en un SP
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"UPDATE No_La_Recurso.Roles
                                                 SET nombre = @nombre,
                                                 activo = @activo
                                                 WHERE id = @id", dbConn);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@activo", SqlDbType.Bit);
                cmd.Parameters.Add("@id", SqlDbType.Int);

                //Le pongo los valores a los parametros
                cmd.Parameters["@nombre"].Value = nombre;
                cmd.Parameters["@activo"].Value = activo;
                cmd.Parameters["@id"].Value = this.id;

                //Ejecuto la query
                cmd.ExecuteNonQuery();
                SqlCommand cmdDF = new SqlCommand(@"DELETE FROM No_La_Recurso.Roles_Funcionalidades
                                                    WHERE id_rol = @id", dbConn);

                cmdDF.Parameters.Add("@id", SqlDbType.Int);
                cmdDF.Parameters["@id"].Value = this.id;

                cmdDF.ExecuteNonQuery();

                foreach (Funcionalidad funcionalidad in funcionalidades)
                {
                    SqlCommand cmdF = new SqlCommand(@"INSERT INTO No_La_Recurso.Roles_Funcionalidades
                                                     (id_rol
                                                     ,id_funcionalidad)
                                                    VALUES
                                                    (@id_rol,
                                                    @id_funcionalidad)", dbConn);

                    //Defino los parametros a utilizar
                    cmdF.Parameters.Add("@id_rol", SqlDbType.Int);
                    cmdF.Parameters.Add("@id_funcionalidad", SqlDbType.Int);

                    //Le pongo los valores a los parametros
                    cmdF.Parameters["@id_rol"].Value = this.id;
                    cmdF.Parameters["@id_funcionalidad"].Value = funcionalidad.id;

                    cmdF.ExecuteNonQuery();
                }

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
