using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Controllers;

namespace Prueba
{
    public partial class Form1 : Form
    {
        public string sCon;
        public SqlConnection con = null;
        public SqlCommand cm = null;
        public SqlDataAdapter da = null;
        public DataSet dtsN = null;
        public static TreeView tvNodos;
        public static List<Nodo> nodos;
        public static List<Elemento> elementos;
        public TreeView TreeView2;

        public Form1()
        {
            InitializeComponent();

            CargarNodosEntity();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }


        public void CargarNodosEntity()
        {
            List<APIBanxicoNodos> listaApiBanxico = new List<APIBanxicoNodos>();
            List<BDAPIBanxico> listaCApiBanxico = new List<BDAPIBanxico>();

            using (Prueba.BD_CICE_UAEMexEntities db = new BD_CICE_UAEMexEntities())
            {
                listaApiBanxico = db.APIBanxicoNodos.ToList();
                listaCApiBanxico = db.C_BDAPIBanxico_.ToList();
            }

            listaApiBanxico = (from l in listaApiBanxico
                               orderby l.Raíz, l.Primer_hijo, l.Segundo_hijo, l.Tercer_hijo, l.Serie
                              select l).ToList();

            bool primerElemento = true;
            TreeNode r = new TreeNode("r");
            TreeNode t = new TreeNode();
            //Nodo primer hi
            TreeNode t1 = new TreeNode();

            //Nodo segundo
            TreeNode t2 = new TreeNode();

            TreeNode t3 = new TreeNode();

            TreeNode t4 = new TreeNode();

            foreach (APIBanxicoNodos api in listaApiBanxico)
            {
                BDAPIBanxico des = listaCApiBanxico.Where(n => n.ID == api.ID).FirstOrDefault();

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

                        t3.Nodes.Add(t4);
                        t2.Nodes.Add(t3);
                        t1.Nodes.Add(t2);
                        t.Nodes.Add(t1);
                        r.Nodes.Add(t);
                    }
                    else
                    {
                        if(t.Text == des.Raiz)
                        {
                            if(t1.Text == des.Primer_hijo)
                            {
                                if(t2.Text == des.Segundo_hijo)
                                {
                                    if(t3.Text == des.Tercer_hijo)
                                    {
                                        t4 = new TreeNode(des.NombreSerie);
                                        t4.Name = des.Serie_ID;

                                        t3.Nodes.Add(t4);

                                    }
                                    else
                                    {
                                        t3 = new TreeNode(des.Tercer_hijo);

                                        t4 = new TreeNode(des.NombreSerie);
                                        t4.Name = des.Serie_ID;

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
 

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeView node = (TreeView)sender;
        }
    }
}
