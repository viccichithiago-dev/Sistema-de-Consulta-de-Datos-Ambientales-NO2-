using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using tp1;
using Microsoft.VisualBasic.FileIO;

namespace tpfinal
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            

        }
        

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Estrategia estrategia = new Estrategia();
            progressBar1.Maximum = Utils.lineCount;
            progressBar1.Step = 1;
            using (TextFieldParser parser = new TextFieldParser(@Utils.get_patron()))
            {

                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                string[] columns = parser.ReadFields();
                string[] fields = parser.ReadFields();
                string titulo = Utils.RemoveSpecialCharacters(fields[2]);
                string descript = Utils.RemoveSpecialCharacters(fields[3]) + "-" + Utils.RemoveSpecialCharacters(fields[4]);
                Backend.datos.Add(titulo+"-"+descript);
                progressBar1.PerformStep();
                while (!parser.EndOfData)
                {
                    fields = parser.ReadFields();
                    titulo = Utils.RemoveSpecialCharacters(fields[2]);
                    descript = Utils.RemoveSpecialCharacters(fields[3]) + "-" + Utils.RemoveSpecialCharacters(fields[4]);
                    Backend.datos.Add(titulo+"-"+descript);
                    progressBar1.PerformStep();
                }
            }
            
            Backend.armarArbolDesdeDatos();
            
            Form1 buscador = new Form1();
            buscador.Show();
            this.Close();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();


        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);


        private void barra_MouseDown(object sender, MouseEventArgs e)
        {


                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);

            
        }

        private void caras_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
