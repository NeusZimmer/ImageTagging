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

namespace ImageTagging
{
    public partial class AddSubcategories : Form
    {
        public AddSubcategories(TagStore almacen)
        {
            InitializeComponent();
            //create_nodes(almacen);
            List<TagStore> almacen1 = new List<TagStore>();
            almacen1.Add(almacen);
            create_nodes(almacen1,treeView1.Nodes);
        }

        private void create_nodes(List<TagStore> childrenList, TreeNodeCollection nodes)
        {
            foreach (TagStore almacen in childrenList)
            {
                System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode(almacen.Name);
                treeNode1.Name = almacen.Name;
                treeNode1.Text = almacen.Name;
                treeNode1.ContextMenuStrip = this.contextMenuStrip1;
                nodes.Add(treeNode1);

                if (almacen.haveChildren)
                    create_nodes(almacen.ChildrenList, treeNode1.Nodes);
            }
        }


        private void ADD_click(object sender, EventArgs e)
        {
            TreeView arbol = treeView1;

            MessageBox.Show("Click ADD" + arbol.SelectedNode);
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("PRUEBA");
            
            treeNode1.ContextMenuStrip = this.contextMenuStrip1;
            arbol.SelectedNode.Nodes.Add(treeNode1);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeView arbol = treeView1;
            MessageBox.Show("Click Remove" + arbol.SelectedNode);
            arbol.SelectedNode.Nodes.Clear();
            arbol.Nodes.Remove(arbol.SelectedNode);
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeView arbol = treeView1;
            MessageBox.Show("Click Remove" + arbol.SelectedNode);
            arbol.SelectedNode.Name = "Cambiado";
            arbol.SelectedNode.Text = "Cambiado";
        }

        private void AddSubcategories_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Antes de cerrar"+this.treeView1.Name);
        }

        private void AddSubcategories_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessageBox.Show("cerrado" + this.treeView1.Name);
        }
    }
}
