using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


namespace ModeloParcial_Lab3
{
    public partial class Venta : Form
    {
        public Venta()
        {
            InitializeComponent();
        }
        CVenta venta = new CVenta();
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            try
          {
            int IdEquipo = (int)cmbEquipo.SelectedValue;
            int IdPlan = (int)cmbPlan.SelectedValue;
            DateTime Fecha = dtpFecha.Value.Date;
            Single Importe = Single.Parse(txtImporte.Text);
            venta.Grabar(Fecha, IdEquipo, IdPlan, Importe);
            cmbEquipo.SelectedIndex = 0;
            cmbPlan.SelectedIndex = 0;
            txtImporte.Text = "";
         }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Venta_Load(object sender, EventArgs e)
        {
            try
            {
                cmbEquipo.DisplayMember = "Marca_Modelo";
                cmbEquipo.ValueMember = "IdEquipo";
                cmbEquipo.DataSource = venta.GetEquipos();

                cmbPlan.DisplayMember = "Nombre";
                cmbPlan.ValueMember = "IdPlan";
                cmbPlan.DataSource = venta.GetPlanes();
                venta.Dispose();

                cmbEquipo.SelectedIndex = 0;
                cmbPlan.SelectedIndex = 0;
                txtImporte.Text = "";
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
