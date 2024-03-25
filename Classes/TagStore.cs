using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTagging.Classes
{
    public class TagStore
    {
        public string Name { get; internal set; }
        public int Order;
        public List<string> TagList { get; internal set; }
        public bool haveChildren { get; internal set; }
        public List<TagStore> ChildrenList { get; internal set; }

        public TagStore(string name)
        {
            this.haveChildren = false;
            this.ChildrenList = new List<TagStore>();
            this.Name = name;
            this.TagList = new List<string>();
            this.Order = 0;
        }

        public void add_children(TagStore children)
        {
            if (children.GetType() == typeof(TagStore))
            {
                this.haveChildren = true;
                TagStore exists=this.ChildrenList.Find(x => x.Name == children.Name);
                if (exists == null)
                    this.ChildrenList.Add(children);
                else MessageBox.Show("Se ha encontrado");
            }
        }

        public void remove_children(TagStore children)
        {
            if ( children.GetType() == typeof(TagStore))
            {
                if (this.haveChildren)
                { 
                    this.ChildrenList.RemoveAll(x => x == children);
                    if (this.ChildrenList.Count() <= 0)
                        this.haveChildren = false;
                }
            }
        }

        public void add_tag(List<string> lista_tags, string Destination)
        {
            foreach (string tag in lista_tags)
                add_tag(tag,Destination);
        }

        public void add_tag(string tag,string Destination)
        {
            if (this.Name == Destination)
            {
                List<string> resultado = TagList.Where(result => result == tag).ToList();
                if (resultado.Count == 0)
                {
                    this.TagList.Add(tag);
                }
            }
            else
            { 
                //if (this.haveChildren)
                if (true)
                    {
                    foreach (TagStore almacen in ChildrenList)
                        almacen.add_tag(tag,Destination);
                }
            }

        }

        public void remove_tag(List<string> lista_tags)
        {
            foreach (string tag in lista_tags)
                remove_tag(tag);
        }
        public void remove_tag(string tag)
        {

            List<string> resultado = TagList.Where(result => result == tag).ToList();
            if (resultado.Count > 0)
            {
                this.TagList.Remove(tag);
            }
        }
    }
}
