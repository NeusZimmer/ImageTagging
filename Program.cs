using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageTagging;
using ImageTagging.Classes;

namespace ImageTaggingApp
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            //Codigo de pruebas

            //TagStore Principal = new TagStore("MAIN");
            //TagStore Nivel1_1 = new TagStore("Nivel1-Objeto1");
            //TagStore Nivel1_2 = new TagStore("Nivel1-Objeto2");
            //TagStore Nivel2_1 = new TagStore("Nivel2-Objeto1");
            //TagStore Nivel2_2 = new TagStore("Nivel2-Objeto2");
            //TagStore Nivel2_3 = new TagStore("Nivel2-Objeto3");
            //Principal.add_children(Nivel1_1);
            //Principal.add_children(Nivel1_2);
            //Principal.ChildrenList[0].add_children(Nivel2_1);
            //Principal.ChildrenList[0].add_children(Nivel2_2);
            //Principal.ChildrenList[1].add_children(Nivel2_3);
            //AddSubcategories form = new AddSubcategories(Principal);
            //form.ShowDialog();

            Application.Run(new ImageTagging());
        }
    }
}
