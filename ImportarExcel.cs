using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SpreadsheetLight;

namespace CICE_Business_Cycles
{
    public partial class ImportarExcel : Form
    {
        public ImportarExcel()
        {
            InitializeComponent();
        }

        public static List<Models.DescargaBanxico> Descarga = new List<Models.DescargaBanxico>();
        private void button1_Click(object sender, EventArgs e)
        {
            string ruta = string.Empty;

            OpenFileDialog open = new OpenFileDialog();

            if (open.ShowDialog() == DialogResult.OK)
            {
                ruta = open.FileName;
            }

            textBox3.Text = ruta;
        }

        List<Models.DataNode> listaseries = new List<Models.DataNode>();
        private void button2_Click(object sender, EventArgs e)
        {
            SLDocument sl = new SLDocument(textBox3.Text);

            int iRow = 2;

            List<Models.DataSerie> lst = new List<Models.DataSerie>();

            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(iRow, 1)))
            {
                Models.DataSerie olst = new Models.DataSerie();

                olst.Date = sl.GetCellValueAsString(iRow, Convert.ToInt32(textBox1.Text));
                olst.Data = sl.GetCellValueAsString(iRow, Convert.ToInt32(textBox2.Text));

                lst.Add(olst);

                iRow++;
            }
            dataGridView1.DataSource = lst;

            Models.DescargaBanxico carga = new Models.DescargaBanxico();
            carga.Serie = lst;
            carga.IdSerie = textBox4.Text;
            carga.Nombre = textBox5.Text;
            Descarga.Add(carga);

            Models.DataNode serieExcel = new Models.DataNode();
            serieExcel.Serie = textBox5.Text;
            serieExcel.ID = textBox4.Text;
            serieExcel.periodicidad = Convert.ToString(comboBox1.Text);
            serieExcel.fechaInicio = lst[0].Date;
            serieExcel.fechafin = lst[lst.Count - 1].Date;
            listaseries.Add(serieExcel);

            dataGridView2.DataSource = null;
            dataGridView2.DataSource = listaseries;

            textBox4.Clear();
            textBox5.Clear();
            textBox1.Clear();
            textBox2.Clear();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 Principal = Owner as Form1;

            var mlistaseries = (from n in listaseries
                                where n.periodicidad == "Mensual"
                                select n).ToList();

            var tlistaseries = (from n in listaseries
                                where n.periodicidad == "Trimestral"
                                select n).ToList();

            Principal.dataGridView1.DataSource = null;
            Principal.dataGridView1.DataSource = mlistaseries;

            Principal.dataGridView3.DataSource = null;
            Principal.dataGridView3.DataSource = tlistaseries;


            this.Close();
        }
    }
}
