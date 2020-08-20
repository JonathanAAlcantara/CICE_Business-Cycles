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
    public partial class CrearArchivo : Form
    {
        public CrearArchivo()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string Nombre = NombreString.Text;
            
            //Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = ".txt";
            saveFileDialog1.FileName = Nombre;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveUbicationString.Text = saveFileDialog1.FileName;
                //if ((myStream = saveFileDialog1.OpenFile()) != null)
                {
                    // Code to write the stream goes here.
                    //myStream.Close();
                }
            }
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void NombreString_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
