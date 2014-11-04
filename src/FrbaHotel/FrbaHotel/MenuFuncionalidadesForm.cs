using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrbaHotel;


namespace FrbaHotel
{
    public partial class MenuFuncionalidadesForm : Form
    {
        public MenuFuncionalidadesForm()
        {
            InitializeComponent();
        }

        public MenuFuncionalidadesForm(string rol) {
            InitializeComponent();
            //Pegarle al rol y hacer un foreach y saber cuales son las funcionalidades
            iniciarToolStripMenuItem.Enabled = false;
            if (rol == "")
            {
                aBMRolToolStripMenuItem.Visible = false;
            }
            else if (rol == "Administrador")
            {
                aBMRolToolStripMenuItem.Visible = true;
                aBMUsuarioToolStripMenuItem.Visible = true;
            }
            else if (rol == "Recepcionista")
            {
                aBMRolToolStripMenuItem.Visible = false;
            }
            else if (rol == "Guest")
            {
            }
            else
            {
            }
        }

        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Login.LoginForm().ShowDialog();
        }

        private void menuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ABM_de_Rol.MenuForm().ShowDialog();
        }

        private void aBMRolToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void menuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new ABM_de_Usuario.MenuForm().ShowDialog();
        }
    }
}
