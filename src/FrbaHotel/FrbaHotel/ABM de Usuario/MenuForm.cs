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
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
            cbHoteles.DataSource = Hotel.obtenerTodosPorIdUsuario(InformacionLogin.UsuarioDeSesion.id);
        }

        private void cbHoteles_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: Cambiar la forma en la que viene, como la identificacion
            Hotel hotel = (Hotel) cbHoteles.SelectedValue;
            dgvUsuarios.DataSource = Usuario.obtenerTodosPorIdHotel(hotel.id);
            
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            Hotel hotel = (Hotel)cbHoteles.SelectedValue;
            if (dgvUsuarios.SelectedRows == null || dgvUsuarios.SelectedRows.Count == 0) return;
            //var row = dgvRoles.CurrentCell.RowIndex;
            DataGridViewRow row = dgvUsuarios.SelectedRows[0];
            var usuario = Usuario.obtenerUsuarioPorId((int)row.Cells[0].Value);
            if (MessageBox.Show(string.Format("Confirma que desea eliminar el usuario {0}?", usuario.nombre)
                , "Eliminar usuario", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    //Elimino el rol
                    usuario.eliminar();
                    dgvUsuarios.DataSource = Usuario.obtenerTodosPorIdHotel(hotel.id);
                    MessageBox.Show(string.Format("Usuario {0} eliminado", usuario.nombre));

                }
                catch (System.Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            new ABM_de_Usuario.AltaForm().ShowDialog();
        }

        private void btnModificacion_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvUsuarios.SelectedRows[0];
            new ABM_de_Usuario.AltaForm((int)row.Cells[0].Value).ShowDialog();
        }

    }
}
