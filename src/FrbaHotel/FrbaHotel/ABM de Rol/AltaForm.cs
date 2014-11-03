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
    public partial class AltaForm : Form
    {
        private int idRolAModificar;
        Rol rol = new Rol();
        public AltaForm()
        {
            InitializeComponent();
            lstFuncionalidades.DataSource = Funcionalidad.obtenerTodas();
        }

        public AltaForm(int id)
        {
            rol = Rol.obtenerRolPorId(id);
            this.idRolAModificar = id;
            InitializeComponent();
            lstFuncionalidades.DataSource = Funcionalidad.obtenerTodas();
            for (int index = 0; index < lstFuncionalidades.Items.Count; index++) {
                if(rol.funcionalidades.Exists(f => f.nombre == lstFuncionalidades.Items[index].ToString())){
                    lstFuncionalidades.SetSelected(index, true);
                }
            }
            lstFuncionalidades.Update();
            
            txtNombre.Text = rol.nombre;
            ckbActivo.Checked = rol.activo;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtNombre.Text.Trim()) || lstFuncionalidades.SelectedItems.Count == 0)
                    throw new Exception("Todos los campos son obligatorios");
            }
            catch (System.Exception excep)
            {
                MessageBox.Show(excep.Message);
                return;
            }
            List<Funcionalidad> funcionalidadesAgregadas = new List<Funcionalidad>();
            foreach (Funcionalidad funcionalidad in lstFuncionalidades.SelectedItems)
            {
                funcionalidadesAgregadas.Add(funcionalidad);
            }

            if (idRolAModificar != 0){
                //update
                rol.update(txtNombre.Text, ckbActivo.Checked, funcionalidadesAgregadas);
                MessageBox.Show(string.Format("Rol {0} modificado", rol.nombre));
            }
            else {
                Rol.agregarNuevoRol(txtNombre.Text, ckbActivo.Checked, funcionalidadesAgregadas);
                MessageBox.Show("Nuevo rol agregado");
            }

            
        }

    }
}
