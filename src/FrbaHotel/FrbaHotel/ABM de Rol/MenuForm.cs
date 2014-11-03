using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrbaHotel.Model;

namespace FrbaHotel.ABM_de_Rol
{
    public partial class MenuForm : Form
    {
        private BindingSource bindingSource1 = new BindingSource();

        public MenuForm()
        {
            InitializeComponent();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {
            dgvRoles.DataSource = Rol.obtenerTodos();
        }

        private void btnBaja_Click(object sender, EventArgs e)
        {
            if (dgvRoles.SelectedRows == null || dgvRoles.SelectedRows.Count == 0) return;
            //var row = dgvRoles.CurrentCell.RowIndex;
            DataGridViewRow row = dgvRoles.SelectedRows[0];
            var rol = Rol.obtenerRolPorId((int) row.Cells[0].Value);
            MessageBox.Show(rol.ToString());
            if (MessageBox.Show(string.Format("Confirma que desea eliminar el rol {0}?", rol.nombre)
                , "Eliminar rol", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    //Elimino el rol
                    rol.eliminar();
                    dgvRoles.DataSource = Rol.obtenerTodos();
                    MessageBox.Show(string.Format("Rol {0} eliminado", rol.nombre));

                }
                catch (System.Exception excep)
                {
                    MessageBox.Show(excep.Message);
                }
            }
        }

        private void btnAlta_Click(object sender, EventArgs e)
        {
            new AltaForm().ShowDialog();
        }

        private void btnModificacion_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = dgvRoles.SelectedRows[0];
            new AltaForm((int)row.Cells[0].Value).ShowDialog();
        }

    }
}
