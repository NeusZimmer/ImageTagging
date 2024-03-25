using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTagging.SubForms
{
    public partial class TextInput : Form
    {

        public string Result;
        public TextInput(string Nombre, string Mensaje)
        {
            InitializeComponent();

            this.Text = Nombre;
            label1.Text = Mensaje;
            label1.AutoSize = true;

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.AcceptButton = buttonOk;
            this.CancelButton = buttonCancel;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Result = textBox1.Text;
        }
    }
}
