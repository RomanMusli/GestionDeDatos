using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrbaHotel.Model;

namespace FrbaHotel.ABM_de_Usuario
{
    public partial class AltaForm : Form
    {
        private int idUsuarioAModificar;
        Usuario usuario = new Usuario();

        public AltaForm()
        {
            InitializeComponent();
            lstRoles.DataSource = Rol.obtenerTodosComoLista();
        }

        public AltaForm(int idUsuario)
        {
            usuario = Usuario.obtenerUsuarioPorId(idUsuario);
            InitializeComponent();
            this.idUsuarioAModificar = idUsuario;
            lstRoles.DataSource = Rol.obtenerTodosComoLista();
            for (int index = 0; index < lstRoles.Items.Count; index++)
            {
                if (usuario.roles.Exists(r => r.nombre == lstRoles.Items[index].ToString()))
                {
                    lstRoles.SetSelected(index, true);
                }
            }
            lstRoles.Update();
            
            txtUsername.Text = usuario.username;
            txtPassword.Text = usuario.password;
            txtNombre.Text = usuario.nombre;
            txtApellido.Text = usuario.apellido;
            txtDocumentoTipo.Text = usuario.identificacion.tipo;
            txtDocumentoNro.Text = usuario.identificacion.numero.ToString();
            txtMail.Text = usuario.mail;
            txtTelefono.Text = usuario.telefono.ToString();
            txtDireccion.Text = usuario.direccion;
            dtpFecNac.Value = usuario.fechaNacimiento;
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO: hacer todas las validaciones correspondientes antes de guardar
                //TODO: Agregar al hotel donde se lo crea
                if (string.IsNullOrEmpty(txtNombre.Text.Trim()))
                    throw new Exception("Todos los campos son obligatorios");
            }
            catch (System.Exception excep)
            {
                MessageBox.Show(excep.Message);
                return;
            }

            List<Rol> rolesAsignados = new List<Rol>();
            foreach (Rol rol in lstRoles.SelectedItems)
            {
                rolesAsignados.Add(rol);
            }

            if (this.idUsuarioAModificar != 0)
            {
                //update
                usuario.update(txtUsername.Text, txtPassword.Text, txtNombre.Text, txtApellido.Text, txtDocumentoTipo.Text, Convert.ToInt32(txtDocumentoNro.Text),
                    txtMail.Text, Convert.ToInt32(txtTelefono.Text), txtDireccion.Text, dtpFecNac.Value, rolesAsignados);
                MessageBox.Show(string.Format("Usuario {0} modificado", usuario.nombre));
            }
            else
            {
                Usuario.agregarNuevoUsuario(txtUsername.Text, txtPassword.Text, txtNombre.Text, txtApellido.Text, txtDocumentoTipo.Text, Convert.ToInt32(txtDocumentoNro.Text),
                    txtMail.Text, Convert.ToInt32(txtTelefono.Text), txtDireccion.Text, dtpFecNac.Value, rolesAsignados);
                MessageBox.Show("Nuevo usuario agregado");
            }
        }
    }
}
