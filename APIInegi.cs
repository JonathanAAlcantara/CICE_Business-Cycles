using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CICE_Business_Cycles
{
    public partial class APIInegi : Form
    {
        public APIInegi()
        {
            InitializeComponent();
            //InitializeTreeViewINEGI();
            CargarNodosEntity();
        }

        public void CargarNodosEntity()
        {
            List<Model.INEGINodos> listaApiINEGI = new List<Model.INEGINodos>();
            List<Model.BDINEGI> listaCApiINEGI = new List<Model.BDINEGI>();

            using (Model.BD_CICE_UAEMexEntities2 db = new Model.BD_CICE_UAEMexEntities2())
            {
                listaApiINEGI = db.INEGINodos.ToList();
                listaCApiINEGI = db.BDINEGI.ToList();
            }

            listaApiINEGI = (from l in listaApiINEGI
                               orderby l.Raíz, l.Primer_hijo, l.Segundo_hijo, l.Tercer_hijo, l.Serie
                               select l).ToList();

            bool primerElemento = true;
            TreeNode r = new TreeNode("API INEGI BD");
            TreeNode t = new TreeNode();
            //Nodo primer hi
            TreeNode t1 = new TreeNode();

            //Nodo segundo
            TreeNode t2 = new TreeNode();

            TreeNode t3 = new TreeNode();

            TreeNode t4 = new TreeNode();

            foreach (Model.INEGINodos api in listaApiINEGI)
            {
                Model.BDINEGI des = listaCApiINEGI.Where(n => n.ID == api.ID).FirstOrDefault();

                if (des != null)
                {
                    if (primerElemento)
                    {
                        primerElemento = false;

                        //Nodo raiz
                        t.Text = des.Raíz;

                        //Nodo primer hi
                        t1.Text = des.Primer_hijo;

                        //Nodo segundo
                        t2.Text = des.Segundo_hijo;

                        t3.Text = des.Tercer_hijo;

                        t4.Text = des.Serie;
                        t4.Name = Convert.ToString(des.Serie_ID);

                        t3.Nodes.Add(t4);
                        t2.Nodes.Add(t3);
                        t1.Nodes.Add(t2);
                        t.Nodes.Add(t1);
                        r.Nodes.Add(t);
                    }
                    else
                    {
                        if (t.Text == des.Raíz)
                        {
                            if (t1.Text == des.Primer_hijo)
                            {
                                if (t2.Text == des.Segundo_hijo)
                                {
                                    if (t3.Text == des.Tercer_hijo)
                                    {
                                        t4 = new TreeNode(des.Serie);
                                        t4.Name = Convert.ToString(des.Serie_ID);

                                        t3.Nodes.Add(t4);

                                    }
                                    else
                                    {
                                        t3 = new TreeNode(des.Tercer_hijo);

                                        t4 = new TreeNode(des.Serie);
                                        t4.Name = Convert.ToString(des.Serie_ID);

                                        t3.Nodes.Add(t4);
                                        t2.Nodes.Add(t3);

                                    }
                                }
                                else
                                {
                                    t2 = new TreeNode(des.Segundo_hijo);

                                    t3 = new TreeNode(des.Tercer_hijo);

                                    t4 = new TreeNode(des.Serie);
                                    t4.Name = Convert.ToString(des.Serie_ID);

                                    t3.Nodes.Add(t4);
                                    t2.Nodes.Add(t3);
                                    t1.Nodes.Add(t2);
                                }
                            }
                            else
                            {
                                t1 = new TreeNode(des.Primer_hijo);

                                t2 = new TreeNode(des.Segundo_hijo);

                                t3 = new TreeNode(des.Tercer_hijo);

                                t4 = new TreeNode(des.Serie);
                                t4.Name = Convert.ToString(des.Serie_ID);

                                t3.Nodes.Add(t4);
                                t2.Nodes.Add(t3);
                                t1.Nodes.Add(t2);
                                t.Nodes.Add(t1);
                            }
                        }
                        else
                        {
                            t = new TreeNode(des.Raíz);

                            t1 = new TreeNode(des.Primer_hijo);

                            t2 = new TreeNode(des.Segundo_hijo);

                            t3 = new TreeNode(des.Tercer_hijo);

                            t4 = new TreeNode(des.Serie);
                            t4.Name = Convert.ToString(des.Serie_ID);

                            t3.Nodes.Add(t4);
                            t2.Nodes.Add(t3);
                            t1.Nodes.Add(t2);
                            t.Nodes.Add(t1);
                            r.Nodes.Add(t);
                        }


                    }
                }
            }

            treeView1.Nodes.Add(r);

        }

        private void APIInegi_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        List<Models.DataNode> listaseriesINEGI = new List<Models.DataNode>();
        public static List<Models.DescargaINEGI> DescargaINEGI = new List<Models.DescargaINEGI>();
        private void button2_Click(object sender, EventArgs e)
        {
            string fechainicio;
            fechainicio = Convert.ToDateTime(this.fechainicio.Text).ToString("yyyy-MM-dd");

            string fechafin;
            fechafin = Convert.ToDateTime(this.fechafin.Text).ToString("yyyy-MM-dd");

            string SerieId;
            SerieId = SerieIdDefault.Text;

            Controllers.APIInegiController oAPIInegiController = new Controllers.APIInegiController();

            List<Models.DataSerie> resultado = oAPIInegiController.ReadSerieINEGI(SerieId, fechainicio, fechafin);

            //ALMACENA CADA CONSULTA EN UNA LISTA DE LISTAS
            Models.DescargaINEGI descarga = new Models.DescargaINEGI();
            descarga.Serie = resultado;
            descarga.IdSerie = SerieId;
            descarga.Nombre = SerieSeleccionada.Text;
            DescargaINEGI.Add(descarga);

            //ENVIA LOS DATOS CONSULTADOS AL DATAGRIDVIEW
            Models.DataNode serie = new Models.DataNode();
            serie.Serie = SerieSeleccionada.Text;
            serie.ID = SerieIdDefault.Text;
            serie.fechaInicio = fechainicio;
            serie.fechafin = fechafin;
            listaseriesINEGI.Add(serie);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = listaseriesINEGI;

        }

        private void FechaInicio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FechaFin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SerieID_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SerieIdDefault.Text = e.Node.Name;
            SerieSeleccionada.Text = e.Node.Text;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 Principal2 = Owner as Form1;

            Principal2.dataGridView1.DataSource = null;
            Principal2.dataGridView1.DataSource = listaseriesINEGI;

            this.Close();
        }
    }
}
