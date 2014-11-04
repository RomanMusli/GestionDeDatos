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

        public static DataTable obtenerTodosPorIdHotel(int idHotel)
        {
            List<Usuario> usuarios = new List<Usuario>();
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT u.id, u.nombre, u.apellido, u.id_identificacion, u.mail, 
                                                u.telefono, u.direccion, u.fecha_nacimiento, u.activo
                                                FROM No_La_Recurso.Usuarios u
                                                INNER JOIN No_La_Recurso.Usuarios_Hoteles uh 
                                                ON uh.id_usuario = u.id
                                                WHERE id_hotel = @idHotel", dbConn);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@idHotel", SqlDbType.Int);

                //Le pongo los valores a los parametros
                cmd.Parameters["@idHotel"].Value = idHotel;

                //Ejecuto la query
                //SqlDataReader dr = cmd.ExecuteReader();

                //Creo el dataAdapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                //Creo el dataSet
                DataSet ds = new DataSet();
                //Lleno el dataSet
                da.Fill(ds, "usuarios");
                //Obtengo la info de la tabla
                DataTable dt = ds.Tables["usuarios"];
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
            //return usuarios;
        }

        public static Usuario obtenerUsuarioPorId(int id)
        {
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            Usuario usuario = new Usuario();
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"SELECT *
                                                FROM No_La_Recurso.Usuarios u
                                                WHERE u.id = @id", dbConn);

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
                        usuario = new Usuario((int)dr["id"], (int)dr["intentos"], (bool)dr["activo"],
                             (string)dr["username"], (string)dr["password"], (string)dr["nombre"], (string)dr["apellido"], (string)dr["mail"],
                             (int)dr["telefono"], (string)dr["direccion"], (DateTime)dr["fecha_nacimiento"]);
                        usuario.identificacion = Identificacion.buscarPorId((int)dr["id_identificacion"]);
                        usuario.roles = Rol.buscarPorIdDeUsuario((int)dr["id"]);
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
            return usuario;
        }

        public void eliminar()
        {
            this.activo = false;
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"UPDATE No_La_Recurso.Usuarios
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

        public static void agregarNuevoUsuario(string username, string password, string nombre, string apellido, 
            string documentoTipo, int documentoNro, string mail, int telefono, string direccion, DateTime fechaNacimiento, List<Rol> roles)
        {
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Crear la identificacion
                SqlCommand cmdI = new SqlCommand(@"INSERT INTO No_La_Recurso.Identificaciones
                                                 (tipo
                                                 ,numero)
                                                OUTPUT Inserted.id
                                                VALUES
                                                (@tipo,
                                                @numero)", dbConn);

                //Defino los parametros a utilizar
                cmdI.Parameters.Add("@tipo", SqlDbType.VarChar, 50);
                cmdI.Parameters.Add("@numero", SqlDbType.Int);

                //Le pongo los valores a los parametros
                cmdI.Parameters["@tipo"].Value = documentoTipo;
                cmdI.Parameters["@numero"].Value = documentoNro;

                //Ejecuto la query
                var idIdentificacion = cmdI.ExecuteScalar();


                //update de usuarios
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"INSERT INTO No_La_Recurso.Usuarios
                                                (username,
                                                 password,
                                                 nombre,
                                                 apellido,
                                                 id_identificacion,
                                                 mail,
                                                 telefono,
                                                 direccion,
                                                 fecha_nacimiento)
                                                OUTPUT Inserted.id
                                                VALUES
                                                (@username,
                                                 @password,
                                                 @nombre,
                                                 @apellido,
                                                 @id_identificacion,
                                                 @mail,
                                                 @telefono,
                                                 @direccion,
                                                 @fecha_nacimiento)", dbConn);

                //Defino los parametros a utilizar
                //Encodeo la password
                Byte[] passwordEnBytes = Encoding.UTF8.GetBytes(password);
                Byte[] passwordEncriptada = new SHA256Managed().ComputeHash(passwordEnBytes);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 256);
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@id_identificacion", SqlDbType.Int);
                cmd.Parameters.Add("@mail", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@telefono", SqlDbType.Int);
                cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@fecha_nacimiento", SqlDbType.Date);


                //Le pongo los valores a los parametros
                cmd.Parameters["@username"].Value = username;
                cmd.Parameters["@password"].Value = BitConverter.ToString(passwordEncriptada).ToString();
                cmd.Parameters["@nombre"].Value = nombre;
                cmd.Parameters["@apellido"].Value = apellido;
                cmd.Parameters["@id_identificacion"].Value = idIdentificacion;
                cmd.Parameters["@mail"].Value = mail;
                cmd.Parameters["@telefono"].Value = telefono;
                cmd.Parameters["@direccion"].Value = direccion;
                cmd.Parameters["@fecha_nacimiento"].Value = fechaNacimiento;

                //Ejecuto la query
                var idUsuario = cmd.ExecuteScalar();

                SqlCommand cmdDR = new SqlCommand(@"DELETE FROM No_La_Recurso.Usuarios_Roles
                                                    WHERE id_usuario = @id", dbConn);

                cmdDR.Parameters.Add("@id", SqlDbType.Int);
                cmdDR.Parameters["@id"].Value = idUsuario;

                cmdDR.ExecuteNonQuery();

                foreach (Rol rol in roles)
                {
                    SqlCommand cmdR = new SqlCommand(@"INSERT INTO No_La_Recurso.Usuarios_Roles
                                                     (id_rol
                                                     ,id_usuario)
                                                    VALUES
                                                    (@id_rol,
                                                    @id_usuario)", dbConn);

                    //Defino los parametros a utilizar
                    cmdR.Parameters.Add("@id_rol", SqlDbType.Int);
                    cmdR.Parameters.Add("@id_usuario", SqlDbType.Int);

                    //Le pongo los valores a los parametros
                    cmdR.Parameters["@id_rol"].Value = rol.id;
                    cmdR.Parameters["@id_usuario"].Value = idUsuario;

                    cmdR.ExecuteNonQuery();
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

        public void update(string username, string password, string nombre, string apellido,
            string documentoTipo, int documentoNro, string mail, int telefono, string direccion, DateTime fechaNacimiento, List<Rol> roles)
        {
            //TODO: Hacerlo en un SP
            SqlConnection dbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["StringConexion"].ConnectionString);
            try
            {
                dbConn.Open();
                //Crear la identificacion
                SqlCommand cmdI = new SqlCommand(@"INSERT INTO No_La_Recurso.Identificaciones
                                                 (tipo
                                                 ,numero)
                                                OUTPUT Inserted.id
                                                VALUES
                                                (@tipo,
                                                @numero)", dbConn);

                //Defino los parametros a utilizar
                cmdI.Parameters.Add("@tipo", SqlDbType.VarChar, 50);
                cmdI.Parameters.Add("@numero", SqlDbType.Int);

                //Le pongo los valores a los parametros
                cmdI.Parameters["@tipo"].Value = documentoTipo;
                cmdI.Parameters["@numero"].Value = documentoNro;

                //Ejecuto la query
                var idIdentificacion = cmdI.ExecuteScalar();

                //Delete e insert de los roles
                SqlCommand cmdDR = new SqlCommand(@"DELETE FROM No_La_Recurso.Usuarios_Roles
                                                    WHERE id_usuario = @id", dbConn);

                cmdDR.Parameters.Add("@id", SqlDbType.Int);
                cmdDR.Parameters["@id"].Value = this.id;

                cmdDR.ExecuteNonQuery();

                foreach (Rol rol in roles)
                {
                    SqlCommand cmdR = new SqlCommand(@"INSERT INTO No_La_Recurso.Usuarios_Roles
                                                     (id_rol
                                                     ,id_usuario)
                                                    VALUES
                                                    (@id_rol,
                                                    @id_usuario)", dbConn);

                    //Defino los parametros a utilizar
                    cmdR.Parameters.Add("@id_rol", SqlDbType.Int);
                    cmdR.Parameters.Add("@id_usuario", SqlDbType.Int);

                    //Le pongo los valores a los parametros
                    cmdR.Parameters["@id_rol"].Value = rol.id;
                    cmdR.Parameters["@id_usuario"].Value = this.id;

                    cmdR.ExecuteNonQuery();
                }

                //update de usuarios
                //Armo la query
                SqlCommand cmd = new SqlCommand(@"UPDATE No_La_Recurso.Usuarios
                                                 SET username = @username,
                                                 password = @password,
                                                 nombre = @nombre,
                                                 apellido = @apellido,
                                                 id_identificacion = @id_identificacion,
                                                 mail = @mail,
                                                 telefono = @telefono,
                                                 direccion = @direccion,
                                                 fecha_nacimiento = @fecha_nacimiento
                                                 WHERE id = @id", dbConn);

                //Defino los parametros a utilizar
                //Encodeo la password
                Byte[] passwordEnBytes = Encoding.UTF8.GetBytes(password);
                Byte[] passwordEncriptada = new SHA256Managed().ComputeHash(passwordEnBytes);

                //Defino los parametros a utilizar
                cmd.Parameters.Add("@username", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@password", SqlDbType.VarChar, 256);
                cmd.Parameters.Add("@nombre", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@apellido", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@id_identificacion", SqlDbType.Int);
                cmd.Parameters.Add("@mail", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@telefono", SqlDbType.Int);
                cmd.Parameters.Add("@direccion", SqlDbType.VarChar, 50);
                cmd.Parameters.Add("@fecha_nacimiento", SqlDbType.Date);
                cmd.Parameters.Add("@id", SqlDbType.Int);


                //Le pongo los valores a los parametros
                cmd.Parameters["@username"].Value = username;
                cmd.Parameters["@password"].Value = BitConverter.ToString(passwordEncriptada).ToString();
                cmd.Parameters["@nombre"].Value = nombre;
                cmd.Parameters["@apellido"].Value = apellido;
                cmd.Parameters["@id_identificacion"].Value = idIdentificacion;
                cmd.Parameters["@mail"].Value = mail;
                cmd.Parameters["@telefono"].Value = telefono;
                cmd.Parameters["@direccion"].Value = direccion;
                cmd.Parameters["@fecha_nacimiento"].Value = fechaNacimiento;
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

    }
}
