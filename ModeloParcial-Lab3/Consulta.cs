using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ModeloParcial_Lab3
{
    public partial class Consulta : Form
    {
        public Consulta()
        {
            InitializeComponent();
        }
        CVenta venta = new CVenta();
        private void Consulta_Load(object sender, EventArgs e)
        {
            try
            {
                cmbPlan.DisplayMember = "Nombre";
                cmbPlan.ValueMember = "IdPlan";
                cmbPlan.DataSource = venta.GetPlanes();
                venta.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnConsulta_Click(object sender, EventArgs e)
        {
            int cod = (int)cmbPlan.SelectedValue;
            string plan = cmbPlan.Text;
            try
            {
                venta.Consultar(cod, dgvConsulta);
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.Title = "Guardar Reporte";
                dlg.Filter = "Archivos de Reporte (.txt)|*.txt";
                dlg.FilterIndex = 0;
                dlg.InitialDirectory = Application.StartupPath;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    venta.GenerarReporte(dlg.FileName, plan);
                    venta.Dispose();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
