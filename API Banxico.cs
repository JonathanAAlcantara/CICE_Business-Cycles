using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Json;
using Models;

namespace CICE_Business_Cycles
{
    public partial class API_Banxico : Form
    {
        public API_Banxico()
        {
            InitializeComponent();
            //InitializeTreeViewBanxico();
            CargarNodosEntity();
        }

        public void CargarNodosEntity()
        {
            List<Model.APIBanxicoNodos> listaApiBanxico = new List<Model.APIBanxicoNodos>();
            List<Model.BDAPIBanxico> listaCApiBanxico = new List<Model.BDAPIBanxico>();

            using (Model.BD_CICE_UAEMexEntities2 db = new Model.BD_CICE_UAEMexEntities2())
            {
                listaApiBanxico = db.APIBanxicoNodos.ToList();
                listaCApiBanxico = db.BDAPIBanxico.ToList();
            }

            listaApiBanxico = (from l in listaApiBanxico
                               orderby l.Raíz, l.Primer_hijo, l.Segundo_hijo, l.Tercer_hijo, l.Serie
                               select l).ToList();

            bool primerElemento = true;
            TreeNode r = new TreeNode("API Banxico BD");
            TreeNode t = new TreeNode();
            //Nodo primer hi
            TreeNode t1 = new TreeNode();

            //Nodo segundo
            TreeNode t2 = new TreeNode();

            TreeNode t3 = new TreeNode();

            TreeNode t4 = new TreeNode();

            foreach (Model.APIBanxicoNodos api in listaApiBanxico)
            {
                Model.BDAPIBanxico des = listaCApiBanxico.Where(n => n.ID == api.ID).FirstOrDefault();

                if (des != null)
                {
                    if (primerElemento)
                    {
                        primerElemento = false;

                        //Nodo raiz
                        t.Text = des.Raiz;

                        //Nodo primer hi
                        t1.Text = des.Primer_hijo;

                        //Nodo segundo
                        t2.Text = des.Segundo_hijo;

                        t3.Text = des.Tercer_hijo;

                        t4.Text = des.NombreSerie;
                        t4.Name = des.Serie_ID;
                        t4.Tag = des.Periodicidad;

                        t3.Nodes.Add(t4);
                        t2.Nodes.Add(t3);
                        t1.Nodes.Add(t2);
                        t.Nodes.Add(t1);
                        r.Nodes.Add(t);
                    }
                    else
                    {
                        if (t.Text == des.Raiz)
                        {
                            if (t1.Text == des.Primer_hijo)
                            {
                                if (t2.Text == des.Segundo_hijo)
                                {
                                    if (t3.Text == des.Tercer_hijo)
                                    {
                                        t4 = new TreeNode(des.NombreSerie);
                                        t4.Name = des.Serie_ID;
                                        t4.Tag = des.Periodicidad;

                                        t3.Nodes.Add(t4);

                                    }
                                    else
                                    {
                                        t3 = new TreeNode(des.Tercer_hijo);

                                        t4 = new TreeNode(des.NombreSerie);
                                        t4.Name = des.Serie_ID;
                                        t4.Tag = des.Periodicidad;

                                        t3.Nodes.Add(t4);
                                        t2.Nodes.Add(t3);

                                    }
                                }
                                else
                                {
                                    t2 = new TreeNode(des.Segundo_hijo);

                                    t3 = new TreeNode(des.Tercer_hijo);

                                    t4 = new TreeNode(des.NombreSerie);
                                    t4.Name = des.Serie_ID;
                                    t4.Tag = des.Periodicidad;

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

                                t4 = new TreeNode(des.NombreSerie);
                                t4.Name = des.Serie_ID;
                                t4.Tag = des.Periodicidad;

                                t3.Nodes.Add(t4);
                                t2.Nodes.Add(t3);
                                t1.Nodes.Add(t2);
                                t.Nodes.Add(t1);
                            }
                        }
                        else
                        {
                            t = new TreeNode(des.Raiz);

                            t1 = new TreeNode(des.Primer_hijo);

                            t2 = new TreeNode(des.Segundo_hijo);

                            t3 = new TreeNode(des.Tercer_hijo);

                            t4 = new TreeNode(des.NombreSerie);
                            t4.Name = des.Serie_ID;
                            t4.Tag = des.Periodicidad;

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

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            if(e.Node.Nodes.Count > 0)
            {
                SerieIdDefault.Text = string.Empty;
                SerieSeleccionada.Text = string.Empty;
                Periodicidad.Text = string.Empty;

                ConsultarAPI.Enabled = false;
            }
            else
            {
                SerieIdDefault.Text = e.Node.Name;
                SerieSeleccionada.Text = e.Node.Text;
                Periodicidad.Text = Convert.ToString(e.Node.Tag);

                ConsultarAPI.Enabled = true;
            }

        }

        List<Models.DataNode> listaseries = new List<DataNode>();
        public static List<Models.DescargaBanxico> Descarga = new List<DescargaBanxico>();
        private void ConsultarAPI_Click(object sender, EventArgs e)
        {
            try
            {
                string fechainicio;
                fechainicio = Convert.ToDateTime(FechaInicio.Text).ToString("yyyy-MM-dd");

                string fechafin;
                fechafin = Convert.ToDateTime(FechaFin.Text).ToString("yyyy-MM-dd");

                string SerieID;
                SerieID = SerieIdDefault.Text;

                //MÉTODO QUE HACE LA CONSULTA DE DATOS A BANCO DE MÉXICO
                Controllers.APIBanxicoController oAPIBanxicoController = new Controllers.APIBanxicoController();

                List<Models.DataSerie> resultado = oAPIBanxicoController.ReadSerie(SerieID, fechainicio, fechafin);

                //ALMACENA CADA CONSULTA EN UNA LISTA DE LISTAS 
                Models.DescargaBanxico descarga = new DescargaBanxico();
                descarga.Serie = resultado;
                descarga.IdSerie = SerieID;
                descarga.Nombre = SerieSeleccionada.Text;
                Descarga.Add(descarga);

                //ENVÍA LOS DATOS CONSULTADOS A UN GRIDVIEW PARA QUE EL CLIENTE OBSERVE SU SELECCIÓN DE SERIES
                Models.DataNode serie = new DataNode();
                serie.Serie = SerieSeleccionada.Text;
                serie.ID = SerieIdDefault.Text;
                serie.periodicidad = Periodicidad.Text;
                serie.fechaInicio = fechainicio;
                serie.fechafin = fechafin;
                listaseries.Add(serie);

                dataGridB.DataSource = null;
                dataGridB.DataSource = listaseries;

                label9.Text = "Consulta de series exitosa...";
            }

            catch (Exception error2)
            {
                label9.Text = error2.Message;
            }

            #region
            //ESTE ES OTRO MÉTODO PARA HACER LA CONSULTA A BANXICO
            //----------------------------------------------------------------------------------------------
            //string fechainicio;
            //fechainicio = Convert.ToDateTime(FechaInicio.Text).ToString("yyyy-MM-dd");

            //string fechafin;
            //fechafin = Convert.ToDateTime(FechaFin.Text).ToString("yyyy-MM-dd");

            //string SerieID;
            //SerieID = SerieIdDefault.Text;

            //Controllers.APIBanxicoController oAPIBanxicoController = new Controllers.APIBanxicoController();

            //List<Models.DataSerie> resultado = oAPIBanxicoController.ReadSerie(SerieID, fechainicio, fechafin);


            ////PRIMERA ITERACIÓN

            //Controllers.Algoritmo oseriecorregida = new Controllers.Algoritmo();

            //List<Models.Correccion> seriecorregida = oseriecorregida.CalcularAlgoritmo(resultado);

            ////SEGUNDA ITERACIÓN

            //List<Models.DataSerie> resultado2 = new List<Models.DataSerie>();

            //for (int i = 0; i < seriecorregida.Count; i++)
            //{
            //    Models.DataSerie res2 = new Models.DataSerie();

            //    Models.Correccion a = seriecorregida[i];

            //    res2.Data = Convert.ToString(a.datocorregido);
            //    res2.Date = Convert.ToString(a.fecha);

            //    resultado2.Add(res2);
            //}
            //Controllers.Algoritmo2 oseriecorregida2 = new Controllers.Algoritmo2();

            //List<Models.Correccion> seriecorregida2 = oseriecorregida2.CalcularAlgoritmo2(resultado2);

            ////TERCERA ITERACIÓN

            //List<Models.DataSerie> resultado3 = new List<Models.DataSerie>();

            //for (int i = 0; i < seriecorregida2.Count; i++)
            //{
            //    Models.DataSerie res3 = new Models.DataSerie();

            //    Models.Correccion aa = seriecorregida2[i];

            //    res3.Data = Convert.ToString(aa.datocorregido);
            //    res3.Date = Convert.ToString(aa.fecha);

            //    resultado3.Add(res3);
            //}
            //Controllers.Algoritmo3 oseriecorregida3 = new Controllers.Algoritmo3();

            //List<Models.Correccion> seriecorregida3 = oseriecorregida3.CalcularAlgoritmo3(resultado3);

            ////OBTENER PROMEDIOS MOVILES DE LA SERIE

            //Controllers.MediaMovilController omediamovil = new Controllers.MediaMovilController();

            //List<Models.MediaMovil> seriemediamovil = omediamovil.CalcularMediaMovil(seriecorregida3);

            ////DETECTAR PUNTOS DE QUIEBRE
            //Controllers.DetectorController odetector = new Controllers.DetectorController();

            //List<Models.Detector> seriequiebres = odetector.DetectordeQuiebres(seriemediamovil);

            //MANDAR EL NOMBRE DE LAS SERIES AL PANEL PRINCIPAL

            //Models.DataNode serie = new DataNode();

            //serie.Serie = SerieSeleccionada.Text;
            //serie.ID = SerieIdDefault.Text;
            //serie.fechaInicio = fechainicio;
            //serie.fechafin = fechafin;
            //listaseries.Add(serie);

            //dataGridB.DataSource = null;
            //dataGridB.DataSource = listaseries;
            //----------------------------------------------------------------------------------------------
            #endregion
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void API_Banxico_Load(object sender, EventArgs e)
        {
            
            try
            {
                Controllers.APIBanxicoController vAPIBanxicoController = new Controllers.APIBanxicoController();

                List<Models.DataSerie> validador = vAPIBanxicoController.ReadSerie("SF282", "1993-01-01", "1993-12-01");

                if(validador[1].Data != null)
                {
                    label8.Text = "Conexión establecida con el servidor...";
                }
                else
                {
                    label8.Text = "Ocurrió un error con la consulta al Web Service";
                }

            }
            catch (Exception error)
            {
                label8.Text = error.Message;
            }

        }

        private void API_Banxico_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 form = new Form1();
            var toolStrip = form.Controls[0].Controls[0];

            if(toolStrip != null)
            {
                ((ProgressBar)toolStrip).Value = 1;
            }

        }

        //-----------------------------------EVENTOS NO UTILIZADOS---------------------------
        #region
        private void FechaInicio_ValueChanged(object sender, EventArgs e)
        {

        }

        private void FechaFin_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SerieIdDefault_TextChanged(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        #endregion

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}

