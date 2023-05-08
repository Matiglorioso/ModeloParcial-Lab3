using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace ModeloParcial_Lab3
{
    internal class CVenta
    {
        OleDbConnection CNN;
        DataSet DS;
        OleDbDataAdapter DAEquipos;
        OleDbDataAdapter DAVentas;
        OleDbDataAdapter DAPlanes;
        OleDbCommand cmdEquipos;
        OleDbCommand cmdVentas;
        OleDbCommand cmdPlanes;
        String TablaEquipos = "Equipos";
        String TablaVentas = "Ventas";
        String TablaPlanes = "Planes";

        public CVenta()
        {
            try
            {
                CNN = new OleDbConnection();
                CNN.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0; Data Source=Moviles.mdb";
                CNN.Open();
                DS = new DataSet();
                //tabla equipos
                cmdEquipos = new OleDbCommand();
                cmdEquipos.Connection = CNN;
                cmdEquipos.CommandType = CommandType.TableDirect;
                cmdEquipos.CommandText = TablaEquipos;
                DAEquipos = new OleDbDataAdapter(cmdEquipos);
                DAEquipos.Fill(DS, TablaEquipos);
                DataColumn[] pkE = new DataColumn[1];
                pkE[0] = DS.Tables[TablaEquipos].Columns["IdEquipo"];
                DS.Tables[TablaEquipos].PrimaryKey = pkE;
                OleDbCommandBuilder cbE = new OleDbCommandBuilder(DAEquipos);

                // Tabla de Ventas
                cmdVentas = new OleDbCommand();
                cmdVentas.Connection = CNN;
                cmdVentas.CommandType = CommandType.TableDirect;
                cmdVentas.CommandText = TablaVentas;
                DAVentas = new OleDbDataAdapter(cmdVentas);
                DAVentas.Fill(DS, TablaVentas);
                DataColumn[] pkV = new DataColumn[1];
                pkV[0] = DS.Tables[TablaVentas].Columns["IdVenta"];
                DS.Tables[TablaVentas].PrimaryKey = pkV;
                OleDbCommandBuilder cbV = new OleDbCommandBuilder(DAVentas);

                // Tabla de Planes
                cmdPlanes = new OleDbCommand();
                cmdPlanes.Connection = CNN;
                cmdPlanes.CommandType = CommandType.TableDirect;
                cmdPlanes.CommandText = TablaPlanes;
                DAPlanes = new OleDbDataAdapter(cmdPlanes);
                DAPlanes.Fill(DS, TablaPlanes);
                DataColumn[] pkP = new DataColumn[1];
                pkP[0] = DS.Tables[TablaVentas].Columns["IdVenta"];
                DS.Tables[TablaVentas].PrimaryKey = pkP;
                OleDbCommandBuilder cbP = new OleDbCommandBuilder(DAPlanes);
                CNN.Close();
            }
            catch (Exception e)
            {
                String msgerr = "CVentas: " + e.Message;
                throw new Exception(msgerr);
            }
        }
        public DataTable GetEquipos()
        {
            if(DS.Tables.Count == 3)
            {
                return DS.Tables[TablaEquipos];
            }
            return null;
        }
        public DataTable GetPlanes()
        {
            if (DS.Tables.Count == 3)
            {
                return DS.Tables[TablaPlanes];
            }
            return null;
        }
        public void Grabar(DateTime fecha, int equipo, int plan, Single importe)
        {
            try
            {
                int id = 1;
                if(plan != 0 && equipo != 0 && fecha != null && importe > 0)
                {
                    DataRow dr = DS.Tables[TablaVentas].NewRow();
                    foreach(DataRow drR in DS.Tables[TablaVentas].Rows)
                    {
                        id++;
                    }
                    dr["IdVenta"] = id;
                    dr["Fecha"] = fecha;
                    dr["IdEquipo"] = equipo;
                    dr["IdPlan"] = plan;
                    dr["Importe"] = importe;
                    DS.Tables[TablaVentas].Rows.Add(dr);
                }
                DAVentas.Update(DS, TablaVentas);
            }
            catch (Exception ex)
            {
                String msgerr = "Error: " + ex.Message;
                throw new Exception(msgerr);
            }
        }
        public void Consultar(int codigoPlan, DataGridView dgv)
        {
            try
            {
                dgv.Rows.Clear();
                foreach(DataRow drR in DS.Tables[TablaVentas].Rows)
                {
                    if ((int)drR["IdPlan"] == codigoPlan)
                    {
                        dgv.Rows.Add(drR["IdVenta"].ToString(), drR["Fecha"].ToString(),
                            drR["IdEquipo"].ToString(), drR["IdPlan"].ToString(), drR["Importe"].ToString());

                    }
                }
            }
            catch (Exception ex)
            {
                String msgerr = "Error: " + ex.Message;
                throw new Exception (msgerr);
            }
        }
        public void GenerarReporte(String NombreArchivo, String plan)
        {
            try
            {
                StreamWriter sw = new StreamWriter(NombreArchivo);
                sw.WriteLine("Reporte de ventas del " + plan);
                sw.WriteLine("-----------------------------------");
                sw.WriteLine("IdVenta   Fecha             IdEquipo   IdPlan       Importe");
                foreach (DataRow dr in DS.Tables[TablaVentas].Rows)
                {
                    string linea = dr["IdVenta"].ToString() + "          ";
                    linea += dr["Fecha"].ToString() + "  ";
                    linea += dr["IdEquipo"].ToString() + "      ";
                    linea += dr["IdPlan"].ToString() + "    ";
                    linea += String.Format("{0,8:F2}", dr["Importe"]);
                    sw.WriteLine(linea);
                }
                sw.WriteLine();
                sw.WriteLine("Fecha del reporte: " + DateTime.Now.ToString());
                sw.Close();
                sw.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Dispose()
        {
            DS.Dispose();
        }
    }
}
