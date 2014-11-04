using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrbaHotel.Model;
using System.Configuration;
using System.Data.SqlClient;

namespace FrbaHotel.Login
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            //Intento Loguearme
            Usuario usuarioALoguear = Usuario.CheckUsuarioYPassword(this.txtUsuario.Text, this.txtPassword.Text);
            if (usuarioALoguear != null){
                if(usuarioALoguear.activo){
                    this.accederMostrarRoles(usuarioALoguear);
                }else {
                    MessageBox.Show("Usted se encuentra inhabilitado");
                }
            }
            else {
                usuarioALoguear.agregarIntentoFallido();
                MessageBox.Show("Error de logueo");
            }
            //A los dos los llevo a un menu
            
        }

        private void accederMostrarRoles(Usuario usuarioALoguear)
        {
            List<Rol> rolesHabilitados = new List<Rol>();
            usuarioALoguear.resetarIntentos();
            InformacionLogin.UsuarioDeSesion = usuarioALoguear;
            foreach( Rol rol in usuarioALoguear.roles){
                if(rol.activo){
                    rolesHabilitados.Add(rol);
                }
            }
            if (rolesHabilitados.Count > 1)
            {
                //Le enableo el cmb y elige
                cbRoles.DataSource = usuarioALoguear.roles;
                cbRoles.DisplayMember = "Nombre";
                cbRoles.SelectedIndex = 0;
                cbRoles.Enabled = true;
                pnlRoles.Show();
            }
            else if (rolesHabilitados.Count < 1)
            {
                //entrar con guest
                this.accederConRol("Guest");
            }
            else
            {
                //Si tiene un solo rol ==> entra directamente al sistema poniendo la cantidad de intentos en cero
                this.accederConRol(usuarioALoguear.roles[0].nombre);
            }
        }

        private void accederConRol(string rol)
        {
            new MenuFuncionalidadesForm(rol).ShowDialog();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            this.accederConRol(cbRoles.SelectedItem.ToString());
        }

        private void btnGuest_Click(object sender, EventArgs e)
        {
            this.accederConRol("Guest");
        }

    }
}
