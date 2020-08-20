using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Org.Sdmx.Resources.SdmxMl.Schemas.V21;

using System.Windows.Forms.DataVisualization.Charting;

using Syncfusion.XlsIO;
using System.IO;
using SpreadsheetLight;
using System.Drawing.Printing;
using Newtonsoft.Json;
using Controllers;
using FastMember;

namespace CICE_Business_Cycles
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            InicializarComponente();
        }

        private void InicializarComponente()
        {
            label17.Text = string.Empty;
            label4.Text = string.Empty;
            label12.Text = string.Empty;
            label13.Text = string.Empty;
            label14.Text = string.Empty;
            label15.Text = string.Empty;
            label16.Text = string.Empty;
        }
        private void imprimirReporteCompletoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //printDocument1 = new PrintDocument();
            //PrinterSettings ps = new PrinterSettings();
            //printDocument1.PrinterSettings = ps;
            //printDocument1.PrintPage += Imprimir;
            //printDocument1.Print();

            PrintDocument doc = new PrintDocument();
            doc.DefaultPageSettings.Landscape = true;
            doc.PrinterSettings.PrinterName = "Microsoft Print to PDF";

            PrintPreviewDialog ppd = new PrintPreviewDialog { Document = doc };
            ((Form)ppd).WindowState = FormWindowState.Maximized;

            doc.PrintPage += delegate (object ev, PrintPageEventArgs ep)
            {
                const int DGV_ALTO = 30;
                int left = ep.MarginBounds.Left, top = ep.MarginBounds.Top;

                //Imprime la tabla con los puntos de quiebre
                foreach (DataGridViewColumn col in dataGridView5.Columns)
                {
                    ep.Graphics.DrawString(col.HeaderText, new Font("Century Gothic", 12, FontStyle.Bold), Brushes.DarkGreen, left, top);
                    left += col.Width;
                }
                left = ep.MarginBounds.Left;
                ep.Graphics.FillRectangle(Brushes.Black, left, top + 30, ep.MarginBounds.Right - left, 2);
                top += 35;

                foreach (DataGridViewRow row in dataGridView5.Rows)
                {
                    if (row.Index == dataGridView5.RowCount - 1) break;
                    left = ep.MarginBounds.Left;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        ep.Graphics.DrawString(Convert.ToString(cell.Value), new Font("Century Gothic", 11), Brushes.Black, left, top + 4);
                        left += cell.OwningColumn.Width;
                    }
                    top += DGV_ALTO;
                }

                //Imprime la gráfica con el periodo completo
                Bitmap objBmp = new Bitmap(chart2.Width, chart2.Height);
                chart2.DrawToBitmap(objBmp, new Rectangle(0, 0, chart2.Width, chart2.Height));

                ep.Graphics.DrawImage(objBmp, 250, 90);
            };
            ppd.ShowDialog();

        }

        private void Imprimir(object sender, PrintPageEventArgs e)
        {
            Font font = new Font("Century Gothic", 14, FontStyle.Regular, GraphicsUnit.Point);

            //e.Graphics.DrawString()

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel2.Text = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            toolStripStatusLabel3.Text = Convert.ToString(DateTime.Now);
        }

        private void bancoDeMéxicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Abriendo conexión con el 'Data Service' de Banco de México";
            timer1.Enabled = true;

            API_Banxico Banxico = new API_Banxico();
            AddOwnedForm(Banxico);

            Banxico.Show();
        }

        //List<Models.DescargaBanxico> DescargaCompleta = new List<Models.DescargaBanxico>();
        //public Form1 (List<Models.DescargaBanxico> oDescargaBanxico)
        //{
        //    DescargaCompleta.AddRange(oDescargaBanxico);
        //}
        //private static string _path = @"C:\Users\jalva\OneDrive\Escritorio\TestDescarga5.json";

        public static List<Models.DescargaBanxico> DescargaRecuperada = new List<Models.DescargaBanxico>();
        public void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //AbrirArchivo Abrir = new AbrirArchivo();

            //Abrir.ShowDialog();

            //var DescargaRecuperada = GetDescargaJsonFromFile();
            //DeserializeJsonFile(DescargaRecuperada);

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "json",
                Filter = "txt files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string dir = @openFileDialog1.FileName;

                string DescargaJsonFromFile;
                using (var reader = new StreamReader(dir))
                {
                    DescargaJsonFromFile = reader.ReadToEnd();
                }

                List<Models.DescargaBanxico> oDescargaRecuperada = JsonConvert.DeserializeObject<List<Models.DescargaBanxico>>(DescargaJsonFromFile);

                DescargaRecuperada = oDescargaRecuperada;

                var ooDescargaRecuperada = (from d in DescargaRecuperada
                                            select new { Nombre = d.Nombre, SerieID = d.IdSerie }).ToList();

                dataGridView1.DataSource = ooDescargaRecuperada;
            }
  
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            DialogResult r = MessageBox.Show("Se eliminarán las series de datos ¿Está seguro de esto?", "Nuevo cálculo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            
            if (r == DialogResult.OK)
            {
                API_Banxico.Descarga.Clear();
                APIInegi.DescargaINEGI.Clear();
                ImportarExcel.Descarga.Clear();
            }
            else
            {
                
            }
        }

        private void iNEGIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Abriendo conexión con el 'Data Service' de INEGI";
            APIInegi Inegi = new APIInegi();
            AddOwnedForm(Inegi);

            Inegi.ShowDialog();
        }

        public static List<Models.MediaMovil> SerieCorrMediaMovil = new List<Models.MediaMovil>();

        public static List<Models.DataSerie> resultado_ = new List<Models.DataSerie>();

        public static List<Models.Correccion> seriecorregida_ = new List<Models.Correccion>();

        public static List<Models.Correccion> seriecorregida2_ = new List<Models.Correccion>();

        public static List<Models.Correccion> seriecorregida3_ = new List<Models.Correccion>();

        public static List<Models.MediaMovil> seriemediamovil_ = new List<Models.MediaMovil>();

        public static List<Models.Detector> seriecorregidaquiebres_ = new List<Models.Detector>();

        public static List<Models.Detector> serieoriginalquiebres_ = new List<Models.Detector>();

        public void cicloEconómicoAKAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Estimando ciclo económico";
            chart2.Series["Series1"].Points.Clear();
            chart2.Series["Series2"].Points.Clear();

            List<Models.DataSerie> resultado = new List<Models.DataSerie>();

            bool isEmpty = !API_Banxico.Descarga.Any();
            bool isEmpty2 = !APIInegi.DescargaINEGI.Any();
            bool isEmpty3 = !ImportarExcel.Descarga.Any();
            if (isEmpty)
            {
                if (isEmpty2)
                {
                    if (isEmpty3)
                    {
                        var ooresultado = (from d in DescargaRecuperada
                                           where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                           select d).ToList();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }
                    }
                    else
                    {
                        var ooresultado = (from d in ImportarExcel.Descarga
                                           where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                           select d).ToList();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }
                    }

                }
                else
                {
                    var ooresultado = (from d in APIInegi.DescargaINEGI
                                       where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                       select d).ToList();

                    for (int i = 0; i < ooresultado.Count(); i++)
                    {
                        foreach (Models.DataSerie dat in ooresultado[i].Serie)
                        {
                            Models.DataSerie res = new Models.DataSerie();
                            res.Data = dat.Data;
                            res.Date = dat.Date;

                            resultado.Add(res);
                        }
                    }
                }

            }
            else
            {
                var ooresultado = (from d in API_Banxico.Descarga
                                   where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                   select d).ToList();

                for (int i = 0; i < ooresultado.Count(); i++)
                {
                    foreach (Models.DataSerie dat in ooresultado[i].Serie)
                    {
                        Models.DataSerie res = new Models.DataSerie();
                        res.Data = dat.Data;
                        res.Date = dat.Date;

                        resultado.Add(res);
                    }
                }
            }

            resultado_ = resultado;

            //PRIMERA ITERACIÓN

            Controllers.Algoritmo oseriecorregida = new Controllers.Algoritmo();

            List<Models.Correccion> seriecorregida = oseriecorregida.CalcularAlgoritmo(resultado);

            seriecorregida_ = seriecorregida;

            //SEGUNDA ITERACIÓN

            List<Models.DataSerie> resultado2 = new List<Models.DataSerie>();

            for (int i = 0; i < seriecorregida.Count; i++)
            {
                Models.DataSerie res2 = new Models.DataSerie();

                Models.Correccion a = seriecorregida[i];

                res2.Data = Convert.ToString(a.datocorregido);
                res2.Date = Convert.ToString(a.fecha);

                resultado2.Add(res2);
            }

            List<Models.Correccion> seriecorregida2 = oseriecorregida.CalcularAlgoritmo(resultado2);

            seriecorregida2_ = seriecorregida2;

            //TERCERA ITERACIÓN

            List<Models.DataSerie> resultado3 = new List<Models.DataSerie>();

            for (int i = 0; i < seriecorregida2.Count; i++)
            {
                Models.DataSerie res3 = new Models.DataSerie();

                Models.Correccion aa = seriecorregida2[i];

                res3.Data = Convert.ToString(aa.datocorregido);
                res3.Date = Convert.ToString(aa.fecha);

                resultado3.Add(res3);
            }

            List<Models.Correccion> seriecorregida3 = oseriecorregida.CalcularAlgoritmo(resultado3);

            seriecorregida3_ = seriecorregida3;

            //OBTENER PROMEDIOS MOVILES DE LA SERIE

            Controllers.MediaMovilController omediamovil = new Controllers.MediaMovilController();

            List<Models.MediaMovil> seriemediamovil = omediamovil.CalcularMediaMovil(seriecorregida3);

            seriemediamovil_ = seriemediamovil;

            //DETECTAR PUNTOS DE QUIEBRE EN SERIE ORIGINAL Y CORREGIDA
            Controllers.DetectorController odetector = new Controllers.DetectorController();

            List<Models.Detector> seriecorregidaquiebres = odetector.DetectordeQuiebres(seriemediamovil);

            List<Models.Detector> serieorgininalquiebres = odetector.DetectordeQuiebres(resultado);

            seriecorregidaquiebres_ = seriecorregidaquiebres;
            serieoriginalquiebres_ = serieorgininalquiebres;

            List<Models.Detector> serieTemp = new List<Models.Detector>();
            List<Models.Detector> serieFinal = new List<Models.Detector>();

            //VALIDAR PUNTOS DE QUIEBRE Y CALCULAR ESTADÍSTICOS

            Controllers.ValidadorController ovalidador = new ValidadorController();

            List<Models.Salida> resumen = ovalidador.ValidadordeSalida(serieorgininalquiebres, seriecorregidaquiebres, resultado);

      
            //CREAMOS LISTA CON LA SERIE CORREGIDA POR MEDIAS MÓVILES
            List<Models.MediaMovil> oSerieCorrMediaMovil = new List<Models.MediaMovil>();

            oSerieCorrMediaMovil = seriemediamovil;

            SerieCorrMediaMovil = oSerieCorrMediaMovil;

            //CALCULAMOS LOS ESTADÍSTICOS QUE SE PRESENTARÁN EN LA TABLA RESUMEN

            var oresumen = (from d in resumen
                                  where d.cambioMensual != 0
                                  select new { Fase = d.fase, Fecha = d.fecha, 
                                      CambioMensual = decimal.Round(Convert.ToDecimal(d.cambioMensual),2), 
                                      CambioTotal = decimal.Round(Convert.ToDecimal(d.cambioTotal),2), 
                                      Varianza = decimal.Round(Convert.ToDecimal(d.varianza),2), Duracion = d.duracion, Indice = d.indice }).ToList();

            dataGridView5.DataSource = oresumen;


            //*Variables con camel se agregaron después
            double? crecimientoprom_ = 1;
            double? crecimientoMensProm_ = 1;
            double? caidaprom_ = 1;
            double? caidaMensProm_ = 1;
            double? varianzaCrecProm_ = 1;
            double? varianzaCaidaProm_ = 1;
            double? duracionCrecProm_ = 0;
            double? duracionCaidaProm_ = 0;

            double cont1 = 0;
            double cont2 = 0;

            double ncont1 = 0;
            double ncont2 = 0;

            for (int i = 0; i < resumen.Count; i++)
            {

                if (resumen[i].cambioTotal > 0)
                {
                    crecimientoprom_ *= (((resumen[i].cambioTotal) / 100) + 1);
                    crecimientoMensProm_ *= (((resumen[i].cambioMensual) / 100) + 1);
                    varianzaCrecProm_ *= (((resumen[i].varianza) / 100) + 1);
                    duracionCrecProm_ += (resumen[i].duracion);
                    cont1 += 1;
                    ncont1 = 1 / cont1;
                }
                else
                {
                    caidaprom_ *= (((resumen[i].cambioTotal) / 100) + 1);
                    caidaMensProm_ *= (((resumen[i].cambioMensual) / 100) + 1);
                    varianzaCaidaProm_ *= (((resumen[i].varianza) / 100) + 1);
                    duracionCaidaProm_ += (resumen[i].duracion);
                    cont2 += 1;
                    ncont2 = 1 / cont2;
                }
                
            }

            double crecimientoprom = Math.Round(((Math.Pow(Convert.ToDouble(crecimientoprom_), (ncont1)))-1),4)*100;
            double caidaprom = Math.Round(((Math.Pow(Convert.ToDouble(caidaprom_), (ncont2)))-1),4)*100;
            double crecimientoMensProm = Math.Round(((Math.Pow(Convert.ToDouble(crecimientoMensProm_), (ncont1))) - 1), 4) * 100;
            double caidaMensProm = Math.Round(((Math.Pow(Convert.ToDouble(caidaMensProm_), (ncont2))) - 1), 4) * 100;
            double varianzaCrecProm = Math.Round(((Math.Pow(Convert.ToDouble(varianzaCrecProm_), (ncont1))) - 1), 4) * 100;
            double varianzaCaidaProm = Math.Round(((Math.Pow(Convert.ToDouble(varianzaCaidaProm_), (ncont2))) - 1), 4) * 100;
            double duracionCrecProm = Math.Round((Convert.ToDouble(duracionCrecProm_)/cont1),0);
            double duracionCaidaProm = Math.Round((Convert.ToDouble(duracionCaidaProm_) / cont1), 0);

            dataGridView4.Rows.Add("Crecimiento Promedio Total", crecimientoprom, caidaprom);
            dataGridView4.Rows.Add("varianza Promedio", varianzaCrecProm, varianzaCaidaProm);
            dataGridView4.Rows.Add("Crecimiento Promedio Mensual", crecimientoMensProm, caidaMensProm);
            dataGridView4.Rows.Add("Duración Promedio", duracionCrecProm, duracionCaidaProm);

            //CREAMOS SERIE CON DATOS QUE SE PERDIERON AL INICIO EN LA CORRECCIÓN

            List<Models.MediaMovil> nulos = new List<Models.MediaMovil>();

            nulos.Add(new Models.MediaMovil() { mediamovil = null, fecha = Convert.ToDateTime(resultado[0].Date) });
            nulos.Add(new Models.MediaMovil() { mediamovil = null, fecha = Convert.ToDateTime(resultado[1].Date) });
            nulos.Add(new Models.MediaMovil() { mediamovil = null, fecha = Convert.ToDateTime(resultado[2].Date) });

            oSerieCorrMediaMovil.InsertRange(0, nulos);
    

            //CREAR GRÁFICA PARA EL TOTAL DEL PERIODO PARA LA SERIE CORREGIDA
            for (int i = 0; i < seriemediamovil.Count; i++)
            {
                Series seriegraph = chart2.Series["Series1"];

                seriegraph.Points.AddXY(Convert.ToDateTime(resultado[i].Date).ToString("yyyy-MM"), oSerieCorrMediaMovil[i].mediamovil);
                seriegraph.LegendText = "Serie corregida";
            }

            //CREAR GRÁFICA PARA EL TOTAL DEL PERIODO PARA LA SERIE ORIGINAL
            for (int i = 0; i < resultado.Count; i++)
            {
                Series seriegraph2 = chart2.Series["Series2"];

                seriegraph2.Points.AddXY(Convert.ToDateTime(resultado[i].Date).ToString("yyyy-MM"), resultado[i].Data);
                seriegraph2.LegendText = "Serie original";
            }

            //CREAR TABLA CON LOS PUNTOS QUE SE DESCARTARON 
            List<Models.Excluidos> excluidos1 = new List<Models.Excluidos>();

            for (int i = 0; i < seriecorregida.Count; i++)
            {
                Models.Excluidos ex1 = new Models.Excluidos();

                if(seriecorregida[i].datofactorizado > 1 || seriecorregida[i].datofactorizado < -1)
                {
                    string ex1_ = "Corregido";

                    ex1.primera = ex1_;
                    ex1.fecha = seriecorregida[i].fecha;

                    excluidos1.Add(ex1);
                }
                else
                {
                    ex1.primera = "-";
                    ex1.fecha = seriecorregida[i].fecha;

                    excluidos1.Add(ex1);
                }
            }

            List<Models.Excluidos> excluidos2 = new List<Models.Excluidos>();   

            for (int i = 0; i < seriecorregida2.Count; i++)
            {
                Models.Excluidos ex2 = new Models.Excluidos();

                if (seriecorregida2[i].datofactorizado > 1 || seriecorregida2[i].datofactorizado < -1)
                {
                    string ex2_ = "Corregido";

                    ex2.segunda = ex2_;
                    ex2.primera = excluidos1[i].primera;
                    ex2.fecha = seriecorregida[i].fecha;

                    excluidos2.Add(ex2);
                }
                else
                {
                    ex2.segunda = "-";
                    ex2.primera = excluidos1[i].primera;
                    ex2.fecha = seriecorregida[i].fecha;

                    excluidos2.Add(ex2);
                }
            }

            List<Models.Excluidos> excluidos3 = new List<Models.Excluidos>();

            for (int i = 0; i < seriecorregida3.Count; i++)
            {
                Models.Excluidos ex3 = new Models.Excluidos();

                if (seriecorregida3[i].datofactorizado > 1 || seriecorregida3[i].datofactorizado < -1)
                {
                    string ex3_ = "Corregido";

                    ex3.tercera = ex3_;
                    ex3.primera = excluidos2[i].primera;
                    ex3.segunda = excluidos2[i].segunda;
                    ex3.fecha = seriecorregida[i].fecha;

                    excluidos3.Add(ex3);
                }
                else
                {
                    ex3.tercera = "-";
                    ex3.primera = excluidos2[i].primera;
                    ex3.segunda = excluidos2[i].segunda;
                    ex3.fecha = seriecorregida[i].fecha;

                    excluidos3.Add(ex3);
                }
            }

            var oexcluidos = (from d in excluidos3
                            where d.primera == "Corregido" || d.segunda == "Corregido" || d.tercera == "Corregido"
                              select new
                            {
                                Fecha = d.fecha,
                                Iteracion1 = d.primera,
                                Iteracion2 = d.segunda,
                                Iteracion3 = d.tercera,
                            }).ToList();

            dataGridView6.DataSource = oexcluidos;

        }
        
        private void cicloEconómicoTrimestralAKOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Estimando ciclo económico";
            chart2.Series["Series1"].Points.Clear();
            chart2.Series["Series2"].Points.Clear();

            List<Models.DataSerie> resultado = new List<Models.DataSerie>();

            bool isEmpty = !API_Banxico.Descarga.Any();
            bool isEmpty2 = !APIInegi.DescargaINEGI.Any();
            bool isEmpty3 = !ImportarExcel.Descarga.Any();
            if (isEmpty)
            {
                if (isEmpty2)
                {
                    if (isEmpty3)
                    {
                        var ooresultado = (from d in DescargaRecuperada
                                            where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                            select d).ToList();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }
                    }
                    else
                    {
                        var ooresultado = (from d in ImportarExcel.Descarga
                                            where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                            select d).ToList();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }
                    }

                }
                else
                {
                    var ooresultado = (from d in APIInegi.DescargaINEGI
                                        where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                        select d).ToList();

                    for (int i = 0; i < ooresultado.Count(); i++)
                    {
                        foreach (Models.DataSerie dat in ooresultado[i].Serie)
                        {
                            Models.DataSerie res = new Models.DataSerie();
                            res.Data = dat.Data;
                            res.Date = dat.Date;

                            resultado.Add(res);
                        }
                    }
                }

            }
            else
            {
                var ooresultado = (from d in API_Banxico.Descarga
                                    where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                    select d).ToList();

                for (int i = 0; i < ooresultado.Count(); i++)
                {
                    foreach (Models.DataSerie dat in ooresultado[i].Serie)
                    {
                        Models.DataSerie res = new Models.DataSerie();
                        res.Data = dat.Data;
                        res.Date = dat.Date;

                        resultado.Add(res);
                    }
                }
            }

            resultado_ = resultado;

            //PRIMERA ITERACIÓN

            Controllers.Algoritmo oseriecorregida = new Controllers.Algoritmo();

            List<Models.Correccion> seriecorregida = oseriecorregida.CalcularAlgoritmo(resultado);

            seriecorregida_ = seriecorregida;

            //SEGUNDA ITERACIÓN

            List<Models.DataSerie> resultado2 = new List<Models.DataSerie>();

            for (int i = 0; i < seriecorregida.Count; i++)
            {
                Models.DataSerie res2 = new Models.DataSerie();

                Models.Correccion a = seriecorregida[i];

                res2.Data = Convert.ToString(a.datocorregido);
                res2.Date = Convert.ToString(a.fecha);

                resultado2.Add(res2);
            }

            List<Models.Correccion> seriecorregida2 = oseriecorregida.CalcularAlgoritmo(resultado2);

            seriecorregida2_ = seriecorregida2;

            //TERCERA ITERACIÓN

            List<Models.DataSerie> resultado3 = new List<Models.DataSerie>();

            for (int i = 0; i < seriecorregida2.Count; i++)
            {
                Models.DataSerie res3 = new Models.DataSerie();

                Models.Correccion aa = seriecorregida2[i];

                res3.Data = Convert.ToString(aa.datocorregido);
                res3.Date = Convert.ToString(aa.fecha);

                resultado3.Add(res3);
            }

            List<Models.Correccion> seriecorregida3 = oseriecorregida.CalcularAlgoritmo(resultado3);

            seriecorregida3_ = seriecorregida3;

            //OBTENER PROMEDIOS MOVILES DE LA SERIE

            Controllers.MediaMovilTrimController omediamovil = new Controllers.MediaMovilTrimController();

            List<Models.MediaMovil> seriemediamovil = omediamovil.CalcularMediaMovilTrimestral(seriecorregida3);

            seriemediamovil_ = seriemediamovil;

            //DETECTAR PUNTOS DE QUIEBRE EN SERIE ORIGINAL Y CORREGIDA
            Controllers.DetectorTrimController odetector = new Controllers.DetectorTrimController();

            List<Models.Detector> seriecorregidaquiebres = odetector.DetectordeQuiebresTrimestral(seriemediamovil);

            List<Models.Detector> serieorgininalquiebres = odetector.DetectordeQuiebresTrimestral(resultado);

            List<Models.Detector> serieTemp = new List<Models.Detector>();
            List<Models.Detector> serieFinal = new List<Models.Detector>();

            seriecorregidaquiebres_ = seriecorregidaquiebres;
            serieoriginalquiebres_ = serieorgininalquiebres;

            //VALIDAR PUNTOS DE QUIEBRE Y CALCULAR ESTADÍSTICOS

            Controllers.ValidadorTrimController ovalidador = new ValidadorTrimController();

            List<Models.Salida> resumen = ovalidador.ValidadordeSalidaTrimestral(serieorgininalquiebres, seriecorregidaquiebres, resultado);

            //CREAMOS LISTA CON LA SERIE CORREGIDA POR MEDIAS MÓVILES
            List<Models.MediaMovil> oSerieCorrMediaMovil = new List<Models.MediaMovil>();

            oSerieCorrMediaMovil = seriemediamovil;

            SerieCorrMediaMovil = oSerieCorrMediaMovil;

            //CALCULAMOS LOS ESTADÍSTICOS QUE SE PRESENTARÁN EN LA TABLA RESUMEN

            var oresumen = (from d in resumen
                            where d.cambioMensual != 0
                            select new
                            {
                                Fase = d.fase,
                                Fecha = d.fecha,
                                CambioMensual = decimal.Round(Convert.ToDecimal(d.cambioMensual), 2),
                                CambioTotal = decimal.Round(Convert.ToDecimal(d.cambioTotal), 2),
                                Varianza = decimal.Round(Convert.ToDecimal(d.varianza), 2),
                                Duracion = d.duracion,
                                Indice = d.indice
                            }).ToList();

            dataGridView5.DataSource = oresumen;


            //*Variables con camel se agregaron después
            double? crecimientoprom_ = 1;
            double? crecimientoMensProm_ = 1;
            double? caidaprom_ = 1;
            double? caidaMensProm_ = 1;
            double? varianzaCrecProm_ = 1;
            double? varianzaCaidaProm_ = 1;
            double? duracionCrecProm_ = 0;
            double? duracionCaidaProm_ = 0;

            double cont1 = 0;
            double cont2 = 0;

            double ncont1 = 0;
            double ncont2 = 0;

            for (int i = 0; i < resumen.Count; i++)
            {

                if (resumen[i].cambioTotal > 0)
                {
                    crecimientoprom_ *= (((resumen[i].cambioTotal) / 100) + 1);
                    crecimientoMensProm_ *= (((resumen[i].cambioMensual) / 100) + 1);
                    varianzaCrecProm_ *= (((resumen[i].varianza) / 100) + 1);
                    duracionCrecProm_ += (resumen[i].duracion);
                    cont1 += 1;
                    ncont1 = 1 / cont1;
                }
                else
                {
                    caidaprom_ *= (((resumen[i].cambioTotal) / 100) + 1);
                    caidaMensProm_ *= (((resumen[i].cambioMensual) / 100) + 1);
                    varianzaCaidaProm_ *= (((resumen[i].varianza) / 100) + 1);
                    duracionCaidaProm_ += (resumen[i].duracion);
                    cont2 += 1;
                    ncont2 = 1 / cont2;
                }

            }

            double crecimientoprom = Math.Round(((Math.Pow(Convert.ToDouble(crecimientoprom_), (ncont1))) - 1), 4) * 100;
            double caidaprom = Math.Round(((Math.Pow(Convert.ToDouble(caidaprom_), (ncont2))) - 1), 4) * 100;
            double crecimientoMensProm = Math.Round(((Math.Pow(Convert.ToDouble(crecimientoMensProm_), (ncont1))) - 1), 4) * 100;
            double caidaMensProm = Math.Round(((Math.Pow(Convert.ToDouble(caidaMensProm_), (ncont2))) - 1), 4) * 100;
            double varianzaCrecProm = Math.Round(((Math.Pow(Convert.ToDouble(varianzaCrecProm_), (ncont1))) - 1), 4) * 100;
            double varianzaCaidaProm = Math.Round(((Math.Pow(Convert.ToDouble(varianzaCaidaProm_), (ncont2))) - 1), 4) * 100;
            double duracionCrecProm = Math.Round((Convert.ToDouble(duracionCrecProm_) / cont1), 0);
            double duracionCaidaProm = Math.Round((Convert.ToDouble(duracionCaidaProm_) / cont1), 0);

            dataGridView4.Rows.Add("Crecimiento Promedio Total", crecimientoprom, caidaprom);
            dataGridView4.Rows.Add("varianza Promedio", varianzaCrecProm, varianzaCaidaProm);
            dataGridView4.Rows.Add("Crecimiento Promedio Mensual", crecimientoMensProm, caidaMensProm);
            dataGridView4.Rows.Add("Duración Promedio", duracionCrecProm, duracionCaidaProm);

            //CREAR GRÁFICA PARA EL TOTAL DEL PERIODO PARA LA SERIE CORREGIDA
            for (int i = 0; i < seriemediamovil.Count; i++)
            {
                Series seriegraph = chart2.Series["Series1"];

                seriegraph.Points.AddXY(Convert.ToDateTime(resultado[i].Date), oSerieCorrMediaMovil[i].mediamovil);
                seriegraph.LegendText = "Serie corregida";

                //seriegraph.Points.AddXY(Convert.ToDateTime(resultado[i].Date).ToString("yyyy-MM"), oSerieCorrMediaMovil[i].mediamovil);
                //seriegraph.LegendText = "Serie corregida";

            }

            //CREAR GRÁFICA PARA EL TOTAL DEL PERIODO PARA LA SERIE ORIGINAL
            for (int i = 0; i < resultado.Count; i++)
            {
                Series seriegraph2 = chart2.Series["Series2"];

                seriegraph2.Points.AddXY(Convert.ToDateTime(resultado[i].Date).ToString("yyyy-MM"), resultado[i].Data);
                seriegraph2.LegendText = "Serie original";
            }

        }
       
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool isEmpty = !API_Banxico.Descarga.Any();
            bool isEmpty2 = !APIInegi.DescargaINEGI.Any();
            bool isEmpty3 = !ImportarExcel.Descarga.Any();
            if (isEmpty)
            {
                if (isEmpty2)
                {
                    if (isEmpty3)
                    {
                        textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                        var ooresultado = (from d in DescargaRecuperada
                                           where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                           select d).ToList();

                        List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }

                        dataGridView2.DataSource = resultado;
                    }
                    else
                    {
                        textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                        textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                        var ooresultado = (from d in ImportarExcel.Descarga
                                           where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                           select d).ToList();

                        List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }

                        dataGridView2.DataSource = resultado;
                    }
                }
                else
                {
                    textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                    textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                    var ooresultado = (from d in APIInegi.DescargaINEGI
                                       where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                       select d).ToList();

                    List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                    for (int i = 0; i < ooresultado.Count(); i++)
                    {
                        foreach (Models.DataSerie dat in ooresultado[i].Serie)
                        {
                            Models.DataSerie res = new Models.DataSerie();
                            res.Data = dat.Data;
                            res.Date = dat.Date;

                            resultado.Add(res);
                        }
                    }

                    dataGridView2.DataSource = resultado;
                }
   
            }
            else
            {
                textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                textBox2.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();

                var ooresultado = (from d in API_Banxico.Descarga
                                   where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                   select d).ToList();

                List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                for (int i = 0; i < ooresultado.Count(); i++)
                {
                    foreach (Models.DataSerie dat in ooresultado[i].Serie)
                    {
                        Models.DataSerie res = new Models.DataSerie();
                        res.Data = dat.Data;
                        res.Date = dat.Date;

                        resultado.Add(res);
                    }
                }

                dataGridView2.DataSource = resultado;

                List<Model.BDAPIBanxico> listaCApiBanxico = new List<Model.BDAPIBanxico>();

                using (CICE_Business_Cycles.Model.BD_CICE_UAEMexEntities2 db = new Model.BD_CICE_UAEMexEntities2())
                {
                    listaCApiBanxico = db.BDAPIBanxico.ToList();
                }

                listaCApiBanxico = (from n in listaCApiBanxico
                                    orderby n.ID
                                    select n).ToList();

                var nperiodo0 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.NombreSerie).ToList();

                label17.Text = nperiodo0.ElementAt(0);

                var nperiodo1 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Serie_ID).ToList();

                label4.Text = nperiodo1.ElementAt(0);

                var nperiodo2 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Periodo_disponible).ToList();

                label12.Text = nperiodo2.ElementAt(0);

                var nperiodo3 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Periodicidad).ToList();

                //label13.Text = nperiodo3.ElementAt(0);

                var nperiodo4 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Cifra).ToList();

                //label14.Text = nperiodo4.ElementAt(0);

                var nperiodo5 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Unidad).ToList();

                //label15.Text = nperiodo5.ElementAt(0);

                var nperiodo6 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Tipo_de_información).ToList();

                //label16.Text = nperiodo6.ElementAt(0);
            }
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            bool isEmpty = !API_Banxico.Descarga.Any();
            bool isEmpty2 = !APIInegi.DescargaINEGI.Any();
            bool isEmpty3 = !ImportarExcel.Descarga.Any();
            if (isEmpty)
            {
                if (isEmpty2)
                {
                    if (isEmpty3)
                    {
                        textBox1.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                        textBox2.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();

                        var ooresultado = (from d in DescargaRecuperada
                                           where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                           select d).ToList();

                        List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }

                        dataGridView2.DataSource = resultado;
                    }
                    else
                    {
                        textBox1.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                        textBox2.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();

                        var ooresultado = (from d in ImportarExcel.Descarga
                                           where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                           select d).ToList();

                        List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                        for (int i = 0; i < ooresultado.Count(); i++)
                        {
                            foreach (Models.DataSerie dat in ooresultado[i].Serie)
                            {
                                Models.DataSerie res = new Models.DataSerie();
                                res.Data = dat.Data;
                                res.Date = dat.Date;

                                resultado.Add(res);
                            }
                        }

                        dataGridView2.DataSource = resultado;
                    }
                }
                else
                {
                    textBox1.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                    textBox2.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();

                    var ooresultado = (from d in APIInegi.DescargaINEGI
                                       where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                       select d).ToList();

                    List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                    for (int i = 0; i < ooresultado.Count(); i++)
                    {
                        foreach (Models.DataSerie dat in ooresultado[i].Serie)
                        {
                            Models.DataSerie res = new Models.DataSerie();
                            res.Data = dat.Data;
                            res.Date = dat.Date;

                            resultado.Add(res);
                        }
                    }

                    dataGridView2.DataSource = resultado;
                }

            }
            else
            {
                textBox1.Text = dataGridView3.CurrentRow.Cells[0].Value.ToString();
                textBox2.Text = dataGridView3.CurrentRow.Cells[1].Value.ToString();

                var ooresultado = (from d in API_Banxico.Descarga
                                   where d.IdSerie.ToUpper() == textBox2.Text.Trim().ToUpper()
                                   select d).ToList();

                List<Models.DataSerie> resultado = new List<Models.DataSerie>();

                for (int i = 0; i < ooresultado.Count(); i++)
                {
                    foreach (Models.DataSerie dat in ooresultado[i].Serie)
                    {
                        Models.DataSerie res = new Models.DataSerie();
                        res.Data = dat.Data;
                        res.Date = dat.Date;

                        resultado.Add(res);
                    }
                }

                dataGridView2.DataSource = resultado;

                List<Model.BDAPIBanxico> listaCApiBanxico = new List<Model.BDAPIBanxico>();

                using (Model.BD_CICE_UAEMexEntities2 db = new Model.BD_CICE_UAEMexEntities2())
                {
                    listaCApiBanxico = db.BDAPIBanxico.ToList();
                }

                listaCApiBanxico = (from n in listaCApiBanxico
                                    orderby n.ID
                                    select n).ToList();

                var nperiodo0 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.NombreSerie).ToList();

                label17.Text = nperiodo0.ElementAt(0);

                var nperiodo1 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Serie_ID).ToList();

                label4.Text = nperiodo1.ElementAt(0);

                var nperiodo2 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Periodo_disponible).ToList();

                label12.Text = nperiodo2.ElementAt(0);

                var nperiodo3 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Periodicidad).ToList();

                //label13.Text = nperiodo3.ElementAt(0);

                var nperiodo4 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Cifra).ToList();

                //label14.Text = nperiodo4.ElementAt(0);

                var nperiodo5 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Unidad).ToList();

                //label15.Text = nperiodo5.ElementAt(0);

                var nperiodo6 = (from l in listaCApiBanxico
                                 where l.Serie_ID == textBox2.Text
                                 select l.Tipo_de_información).ToList();

                //label16.Text = nperiodo6.ElementAt(0);
            }
        }

        private void dataGridView5_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            //MÉTODO PARA GRAFICAR PUNTOS DE QUIEBRE A PARTIR DE CLIC EN DATAGRIDVIEW
            #region
            //chart1.Series["Serie1"].Points.Clear();
            //string a = dataGridView5.CurrentRow.Cells["Fecha"].Value.ToString();

            ////CREAR ZOOM DE GRÁFICA PARA PUNTOS DE QUIEBRE
            //DateTime fechaobjetivo = new DateTime();
            //DateTime fechaobjetivoInicial = new DateTime();
            //DateTime fechaobjetivoFinal = new DateTime();

            //fechaobjetivo = Convert.ToDateTime(a);
            //fechaobjetivoFinal = fechaobjetivo.AddMonths(5);
            //fechaobjetivoInicial = fechaobjetivo.AddMonths(-10);

            //var zoom = (from n in SerieCorrMediaMovil
            //            where n.fecha >= fechaobjetivoInicial && n.fecha <= fechaobjetivoFinal
            //            select n).ToList();

            //for (int i = 0; i < zoom.Count; i++)
            //{
            //    Series seriegraph2 = chart1.Series["Serie1"];

            //    //seriegraph2.AxisLabel.Contains(Convert.ToDateTime(zoom[i].fecha).ToString("yyyy-MM"));
            //    //seriegraph2.Points.Add(zoom[i].mediamovil);
            //    seriegraph2.Points.AddXY(Convert.ToDateTime(zoom[i].fecha).ToString("yyyy-MM"), zoom[i].mediamovil);
            //    seriegraph2.LegendText = textBox1.Text;
            //}
            #endregion

            //MÉTODO QUE AGREGA STRIPLINES AL GRÁFICO EN LOS PUNTOS DE QUIEBRE

            if (Convert.ToString(dataGridView5.CurrentRow.Cells["Fase"].Value) == "Valle")
            {
                var delimiter = new StripLine
                {
                    IntervalOffset = Convert.ToDouble(dataGridView5.CurrentRow.Cells["Indice"].Value) - Convert.ToDouble(dataGridView5.CurrentRow.Cells["Duracion"].Value),
                    StripWidth = Convert.ToDouble(dataGridView5.CurrentRow.Cells["Duracion"].Value),
                    BackColor = Color.LightGray,
                };
                chart2.ChartAreas["ChartArea1"].AxisX.StripLines.Add(delimiter);
                
            }
            else
            {
                MessageBox.Show("Debe seleccionar un Valle", "Agregar sombras a la gráfica", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
        }

        private void archivoxmlxToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SLDocument sl = new SLDocument();

            int iC = 1;
            foreach (DataGridViewColumn column in dataGridView2.Columns)
            {
                sl.SetCellValue(1, iC, column.HeaderText.ToString());
                iC++;
            }

            int iR = 2;
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                sl.SetCellValue(iR, 1, row.Cells[0].Value.ToString());
                sl.SetCellValue(iR, 2, row.Cells[1].Value.ToString());
                iR++;
            }

            System.Data.DataTable SerieCorregidaTable = new System.Data.DataTable();

            using (var reader = ObjectReader.Create(seriecorregida3_, "datoLog", "dif", 
                "datofactorizado", "identificador", "datocorregido","data", "fecha"))
            {
                SerieCorregidaTable.Load(reader);
            }

            int iR1 = 2;
            sl.AddWorksheet("Serie Corregida");
            foreach (DataRow row in SerieCorregidaTable.Rows)
            {
                sl.SetCellValue(1, 1, "Fecha");
                sl.SetCellValue(1, 2, "Dato");
                sl.SetCellValue(1, 3, "Logaritmo");
                sl.SetCellValue(1, 4, "Diferencias");
                sl.SetCellValue(1, 5, "Dato Factorizado");
                sl.SetCellValue(1, 6, "Dato Corregido");

                sl.SetCellValue(iR1, 1, row[6].ToString());
                sl.SetCellValue(iR1, 2, Math.Round((Convert.ToDouble(row[5].ToString())),2));
                sl.SetCellValue(iR1, 3, Math.Round((Convert.ToDouble(row[0].ToString())),2));
                sl.SetCellValue(iR1, 4, Math.Round((Convert.ToDouble(row[1].ToString())),4));
                sl.SetCellValue(iR1, 5, Math.Round((Convert.ToDouble(row[2].ToString())),4));
                sl.SetCellValue(iR1, 6, Math.Round((Convert.ToDouble(row[4].ToString())),2));
                iR1++;
            }

            System.Data.DataTable SerieMediaMovilTable = new System.Data.DataTable();

            using (var reader = ObjectReader.Create(seriemediamovil_, "fecha", "mediamovil"))
            {
                SerieMediaMovilTable.Load(reader);
            }

            int iR2 = 2;
            sl.AddWorksheet("Serie Media Movil");
            foreach (DataRow row in SerieMediaMovilTable.Rows)
            {
                sl.SetCellValue(1, 1, "Fecha");
                sl.SetCellValue(1, 2, "Dato");

                sl.SetCellValue(iR2, 2, row[1].ToString());
                sl.SetCellValue(iR2, 1, row[0].ToString());
                iR2++;
            }

            System.Data.DataTable SerieCorregidaQuiebresTable = new System.Data.DataTable();

            using (var reader = ObjectReader.Create(seriecorregidaquiebres_, "mediamovil", "fecha",
                "detectormax", "detectormin", "indice", "criterioMax1", "criterioMin1", "criterio2",
                "criterio3", "punto", "fase"))
            {
                SerieCorregidaQuiebresTable.Load(reader);
            }

            int iR3 = 2;
            sl.AddWorksheet("Corregida Quiebres");
            foreach (DataRow row in SerieCorregidaQuiebresTable.Rows)
            {
                sl.SetCellValue(1, 1, "Índice");
                sl.SetCellValue(1, 2, "Fecha");
                sl.SetCellValue(1, 3, "Media Movil");
                sl.SetCellValue(1, 4, "Máximo");
                sl.SetCellValue(1, 5, "Mínimo");
                sl.SetCellValue(1, 6, "AmplitudMaximo");
                sl.SetCellValue(1, 7, "AmplitudMínimo");
                sl.SetCellValue(1, 8, "Criterio 2");
                sl.SetCellValue(1, 9, "Criterio 3");
                sl.SetCellValue(1, 10, "Punto");
                sl.SetCellValue(1, 11, "Fase");

                sl.SetCellValue(iR3, 1, row[4].ToString());
                sl.SetCellValue(iR3, 2, row[1].ToString());
                sl.SetCellValue(iR3, 3, row[0].ToString());
                sl.SetCellValue(iR3, 4, row[2].ToString());
                sl.SetCellValue(iR3, 5, row[3].ToString());
                sl.SetCellValue(iR3, 6, row[5].ToString());
                sl.SetCellValue(iR3, 7, row[6].ToString());
                sl.SetCellValue(iR3, 8, row[7].ToString());
                sl.SetCellValue(iR3, 9, row[8].ToString());
                sl.SetCellValue(iR3, 10, row[9].ToString());
                sl.SetCellValue(iR3, 11, row[10].ToString());
                iR3++;
            }

            System.Data.DataTable SerieOriginalQuiebresTable = new System.Data.DataTable();

            using (var reader = ObjectReader.Create(serieoriginalquiebres_, "mediamovil", "fecha",
                "detectormax", "detectormin", "indice", "criterioMax1", "criterioMin1", "criterio2",
                "criterio3", "punto", "fase"))
            {
                SerieOriginalQuiebresTable.Load(reader);
            }

            int iR4 = 2;
            sl.AddWorksheet("Orginal Quiebres");
            foreach (DataRow row in SerieOriginalQuiebresTable.Rows)
            {
                sl.SetCellValue(1, 1, "Índice");
                sl.SetCellValue(1, 2, "Fecha");
                sl.SetCellValue(1, 3, "Media Movil");
                sl.SetCellValue(1, 4, "Máximo");
                sl.SetCellValue(1, 5, "Mínimo");
                sl.SetCellValue(1, 6, "AmplitudMaximo");
                sl.SetCellValue(1, 7, "AmplitudMínimo");
                sl.SetCellValue(1, 8, "Criterio 2");
                sl.SetCellValue(1, 9, "Criterio 3");
                sl.SetCellValue(1, 10, "Punto");
                sl.SetCellValue(1, 11, "Fase");

                sl.SetCellValue(iR4, 1, row[4].ToString());
                sl.SetCellValue(iR4, 2, row[1].ToString());
                sl.SetCellValue(iR4, 3, row[0].ToString());
                sl.SetCellValue(iR4, 4, row[2].ToString());
                sl.SetCellValue(iR4, 5, row[3].ToString());
                sl.SetCellValue(iR4, 6, row[5].ToString());
                sl.SetCellValue(iR4, 7, row[6].ToString());
                sl.SetCellValue(iR4, 8, row[7].ToString());
                sl.SetCellValue(iR4, 9, row[8].ToString());
                sl.SetCellValue(iR4, 10, row[9].ToString());
                sl.SetCellValue(iR4, 11, row[10].ToString());
                iR4++;
            }

            SaveFileDialog saveFileDialog2 = new SaveFileDialog
            {
                DefaultExt = "xlsx",
                Filter = "txt files (*.xlsx)|*.xlsx",
                FilterIndex = 2,
                RestoreDirectory = true,

                InitialDirectory = @"C:\",
                Title = "Descarga de procedimiento_",

                CheckFileExists = true,
                CheckPathExists = true,
            };

            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string dir = saveFileDialog2.FileName;

                sl.SaveAs(dir);
            };
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string DescargaJson = JsonConvert.SerializeObject(API_Banxico.Descarga.ToArray(), Formatting.Indented);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                DefaultExt = "json",
                Filter = "txt files (*.json)|*.json",
                FilterIndex = 2,
                RestoreDirectory = true,
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, DescargaJson);
            };

        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult res = MessageBox.Show("¿Está seguro que quiere cerrar el sistema?", "Cerrar",
            MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (res == DialogResult.Cancel)
                e.Cancel = true;
        }

        private void propiedadesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Propiedades propiedades = new Propiedades();
            AddOwnedForm(propiedades);

            propiedades.ShowDialog();
        }

        private void dataGridView1_MouseHover(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Seleccione una serie de datos para visualizarla";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar1.Value <= toolStripProgressBar1.Maximum - 1)
            {
                toolStripProgressBar1.Value += 1;
            }
            else
            {
                timer1.Enabled = false;
            }  
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chart2.Series["Series1"].Points.Clear();
            chart2.Series["Series2"].Points.Clear();

            int indice = comboBox1.SelectedIndex;

            if (indice == 0)
            {
                for (int i = 0; i < resultado_.Count; i++)
                {
                    Series seriegraph1 = chart2.Series["Series2"];

                    seriegraph1.Points.AddXY(Convert.ToDateTime(resultado_[i].Date).ToString("yyyy-MM"), resultado_[i].Data);
                    seriegraph1.LegendText = "Serie original";

                    Series seriegraph2 = chart2.Series["Series1"];

                    seriegraph2.Points.AddXY(Convert.ToDateTime(resultado_[i].Date).ToString("yyyy-MM"), seriecorregida3_[i].datocorregido);
                    seriegraph2.LegendText = "Serie Corregida";
                }
            }
            else
            {
                //CREAR GRÁFICA PARA EL TOTAL DEL PERIODO PARA LA SERIE CORREGIDA
                for (int i = 0; i < SerieCorrMediaMovil.Count; i++)
                {
                    Series seriegraph = chart2.Series["Series1"];

                    seriegraph.Points.AddXY(Convert.ToDateTime(resultado_[i].Date).ToString("yyyy-MM"), SerieCorrMediaMovil[i].mediamovil);
                    seriegraph.LegendText = "Serie Media Móvil";
                }

                //CREAR GRÁFICA PARA EL TOTAL DEL PERIODO PARA LA SERIE ORIGINAL
                for (int i = 0; i < resultado_.Count; i++)
                {
                    Series seriegraph2 = chart2.Series["Series2"];

                    seriegraph2.Points.AddXY(Convert.ToDateTime(resultado_[i].Date).ToString("yyyy-MM"), resultado_[i].Data);
                    seriegraph2.LegendText = "Serie original";
                }
            }
        }

        private void importarBDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Estableciendo conexión con Microsoft Excel";
            timer1.Enabled = true;

            ImportarExcel Excel = new ImportarExcel();
            AddOwnedForm(Excel);

            Excel.Show();

        }

        private void gráficaComoImagenToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog3 = new SaveFileDialog
            {
                DefaultExt = "gif",
                Filter = "gif files (*.gif)|*.gif",
                FilterIndex = 2,
                RestoreDirectory = true,

                InitialDirectory = @"C:\",
            };

            if (saveFileDialog3.ShowDialog() == DialogResult.OK)
            {
                
                string dir = saveFileDialog3.FileName;
                
                chart2.SaveImage(dir, ChartImageFormat.Gif);
               
            };

        }
}
}
