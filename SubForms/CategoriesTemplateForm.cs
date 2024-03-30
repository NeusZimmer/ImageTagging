using ImageTagging.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagStore = SearchTag_controls.TagStore;

namespace ImageTagging.SubForms
{
    public partial class CategoriesTemplateForm : Form
    {
        public TagStore Result { get; private set; }
        public CategoriesTemplateForm()
        {
            InitializeComponent();

        }

        public CategoriesTemplateForm(TagStore almacen)
        {
            InitializeComponent(almacen);
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Result = this.categoriesTemplate1.Template;
        }
    }
}
