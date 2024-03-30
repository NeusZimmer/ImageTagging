using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ImageTagging.Classes;
using SearchTag_controls;
using Newtonsoft.Json;
using ImageTagging.SubForms;
//using TagStore = ImageTagging.Classes.TagStore;
using TagStore = SearchTag_controls.TagStore;

namespace ImageTaggingApp
{
    public partial class ImageTagging : Form
    {
        private List<ImageData> lista_imagenes = new List<ImageData>();
        private int thumbnail_size = 210;
        private int ImageSessionID = 0;
        private TagStore TagStoreTemplate = new TagStore("MAIN");
        ImagePager Pager;


        /// Lista de colores a usar
        private System.Drawing.Color Selected = System.Drawing.Color.GreenYellow;
        //private System.Drawing.Color UnSelected = System.Drawing.Color.BlueViolet; 
        private System.Drawing.Color UnSelected = System.Drawing.Color.DimGray;
        public ImageTagging()
        {
            InitializeComponent();
        }


        private void ImageTagging_Load(object sender, EventArgs e)
        {
            panel_Image_preview.Visible = false;
            panel_Image_preview.Dock = System.Windows.Forms.DockStyle.Fill;
            textBox_path.Text = System.IO.Directory.GetCurrentDirectory();
            txtCategoriesDir.Text= System.IO.Directory.GetCurrentDirectory();
            SearchTag_controls.SearchField campobusqueda = new SearchTag_controls.SearchField();
            campobusqueda.Dock = DockStyle.Top;
            //campobusqueda.Visible = false;
            tabla_panel_derecho.Controls.Add(campobusqueda, 0, 0);
            //Panel_derecho.Controls.Add(campobusqueda);
            campobusqueda.AddClick += new System.EventHandler(tag_add_click_reveived);
            campobusqueda.UpdatedTag += new System.EventHandler(tag_text_update_reveived);

            //btn_Add_Tag.Visible = false;
            //lbl_SelectedTags.Visible = false;

            int categorias = 4;
            adjust_categories_table(categorias);
            add_categories_controls(categorias);
            update_category_destinations();
        }

        public void add_categories_controls(int categorias)
        {
            //List<string> lista_test_tags = new List<string> { "female", "girl", "man", "couple", "female", "girl", "man", "couple", "female", "girl", "man", "couple", "female", "girl", "man", "couple" };

            int Altura_Maxima = (int)tablaCategorias.Size.Height / categorias;

            for (int counter = 0; counter < categorias; counter++)
            {
                SearchTag_controls.TagCategory tagCategory_test = CrearCategoria("NewCategory" + counter.ToString(), Altura_Maxima);
                //tagCategory_test.Add_Tags(lista_test_tags);
                tagCategory_test.CategoryName = "Not Loaded:" + counter.ToString();
                this.tablaCategorias.Controls.Add(tagCategory_test, 0, counter);
            }
        }

        public TagCategory CrearCategoria(string Name, int height)
        {
            TagCategory tagCategory2 = new SearchTag_controls.TagCategory();
            tagCategory2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tagCategory2.BackColor = System.Drawing.Color.Transparent;
            tagCategory2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tagCategory2.CategoryName = Name;
            tagCategory2.Dock = System.Windows.Forms.DockStyle.Fill;
            tagCategory2.HeaderColor = System.Drawing.Color.Gainsboro;
            tagCategory2.Location = new System.Drawing.Point(0, 0);
            tagCategory2.Margin = new System.Windows.Forms.Padding(0);
            tagCategory2.MinimumSize = new System.Drawing.Size(100, 100);
            tagCategory2.MaximumSize = new System.Drawing.Size(0, height);
            tagCategory2.Name = Name;

            //tagCategory2.TabIndex = 0;

            return tagCategory2;
        }

        private void adjust_categories_table(int numCategories)
        {
            TableLayoutPanel tabla_temporal = new TableLayoutPanel();


            tabla_temporal.ColumnCount = 1;
            tabla_temporal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tabla_temporal.Dock = System.Windows.Forms.DockStyle.Fill;
            tabla_temporal.Name = "tablaCategorias";
            //tabla_temporal.TabIndex = 8;
            tabla_temporal.Location = new System.Drawing.Point(0, 62);


            int numero_cat = numCategories;

            int resto = 100 % numero_cat;
            Single altura = ((100 - resto) / numero_cat);

            tabla_temporal.RowCount = numero_cat;
            tabla_temporal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, altura + resto));
            for (int counter = 1; counter < numero_cat; counter++)
                tabla_temporal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, altura));

            tabla_panel_derecho.Controls.Remove(this.tablaCategorias);
            //Panel_derecho.Controls.Remove(this.tablaCategorias);
            this.tablaCategorias = tabla_temporal;
            //Panel_derecho.Controls.Add(this.tablaCategorias);
            tabla_panel_derecho.Controls.Add(tablaCategorias, 0, 3);

        }



        // ############################ EVENT SUBSCRIPTIONS ############################
        private void tag_add_click_reveived(object sender, EventArgs e)
        {
            SearchTag_controls.SearchField elemento = (SearchTag_controls.SearchField)sender;
            MessageBox.Show("Recepcion de:" + elemento.tag);
        }
        private void tag_text_update_reveived(object sender, EventArgs e)
        {
            SearchTag_controls.SearchField elemento = (SearchTag_controls.SearchField)sender;
            //MessageBox.Show("Recepcion update de:" + elemento.tag);
            foreach (TagCategory tagCategory1 in tablaCategorias.Controls.OfType<TagCategory>())
            {
                tagCategory1.FilterByTxt(elemento.tag);
            }
        }
        // ############################ DYNAMIC ASSIGNED EVENTS ############################
        /// Eventos del formulario
        private void Elemento_Click2(object sender, MouseEventArgs e)
        {
            int vuelta = e.Delta;
            MessageBox.Show("Wheel: " + vuelta);///positivo o negativo

        }
        private void Elemento_Click(object sender, MouseEventArgs e)
        {
            PictureBox imagebox = sender as PictureBox;
            Control panel_interno = imagebox.Parent;
            Control panel_externo = panel_interno.Parent;


            string[] subs = imagebox.Name.Split(':');
            int image_session_id = int.Parse(subs[1]);
            ImageData imagen_seleccionada = lista_imagenes.Find(result => result.ImageSessionID == image_session_id);


            if (e.Button == MouseButtons.Left)
            {
                if (imagen_seleccionada.Selected)
                {
                    panel_externo.BackColor = UnSelected;
                    imagen_seleccionada.Selected = false;
                }
                else
                {
                    panel_externo.BackColor = Selected;
                    imagen_seleccionada.Selected = true;
                }
            }
            // Esto se ha pasado a un menu contextual
            //else if (e.Button == MouseButtons.Right)
            //{
            //    List<int> list_session_ids = Pager.get_active_ids_for_page();
            //    foreach (int session_id in list_session_ids)
            //    {
            //        ImageData datoimagen = lista_imagenes.Find(x => x.ImageSessionID == session_id);
            //        if (session_id == image_session_id)
            //            datoimagen.Selected = true;
            //        else
            //            datoimagen.Selected = false;
            //    }
            //    //Lo siguiente (mas rapido) o repintarlas completamente.
            //    foreach (Control control in PanelImagenes.Controls)
            //    {
            //        if (control.Name == panel_externo.Name) control.BackColor = Selected;
            //        else control.BackColor = UnSelected;
            //    }
            //}
            //else if (e.Button == MouseButtons.Middle)
            //{
            //    imagen_seleccionada.Selected = false;
            //    Pager.hide_image(image_session_id);
            //    panel_interno.Controls.Remove(imagebox);
            //    panel_externo.Controls.Remove(panel_interno);
            //    PanelImagenes.Controls.Remove(panel_externo);
            //    imagebox.Dispose();
            //    panel_interno.Dispose();
            //    panel_externo.Dispose();
            //}
        }
        private void button12_Click(object sender, EventArgs e)
        {
            close_preview(sender, e);

        }

        // ############################ PANEL PREVIEW ############################
        #region Panel Preview
        private void Elemento_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox imagebox = sender as PictureBox;
            List<string> information = new List<string>();



            string[] subs = imagebox.Name.Split(':');
            //MessageBox.Show("Splitted, session id:"+subs[1]);
            int image_session_id = int.Parse(subs[1]);

            var selection = lista_imagenes.Where(result =>
                result.ImageSessionID == image_session_id
                );

            //pictureBox_Image_Preview.Image = imagebox.Image;

            information.Add("Information for image:" + imagebox.Name);
            information.Add("SessionID:" + image_session_id.ToString());
            information.Add("File:" + selection.First().ImageName);
            information.Add("Size:" + selection.First().Image.Width.ToString() + "*" + selection.First().Image.Height.ToString());
            string tags = JsonConvert.SerializeObject(selection.First().Tags);

            information.Add("Tags:" + tags);
            txtImageInfo.Text = String.Join(System.Environment.NewLine, information);
            pictureBox_Image_Preview.Image = selection.First().Image;

            panel_Image_preview.Visible = !panel_Image_preview.Visible;
            pictureBox_Image_Preview.MaximumSize = panel_Image_preview.Size;
            PanelImagenes.Visible = !PanelImagenes.Visible;
            pictureBox_Image_Preview.MaximumSize = new Size(panel_Image_preview.Size.Width, panel_Image_preview.Size.Height);
        }

        private void close_preview(object sender, EventArgs e)
        {
            panel_Image_preview.Visible = !panel_Image_preview.Visible;
            PanelImagenes.Visible = !PanelImagenes.Visible;
        }

        #endregion



        // ############################ CONFIG BUTTONS ############################
        /// Botones config
        #region Botones Formulario- Configuracion

        private void select_path_btn_Click(object sender, EventArgs e)
        {
            Eliminate_PanelImagenes_SubControls();

            lista_imagenes.Clear();
            GC.Collect();
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();


            dialog.InitialDirectory = textBox_path.Text;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                textBox_path.Text = dialog.FileName;
            }

            string filter = @"*.png";
            string[] fileEntries = Directory.GetFiles(textBox_path.Text, filter);

            filter = @"*.jpg";
            string[] fileEntries2 = Directory.GetFiles(textBox_path.Text, filter);

            fileEntries=fileEntries.Concat(fileEntries2).ToArray();
            string filter2 = @"*.json";
            string[] json_fileEntries = Directory.GetFiles(textBox_path.Text, filter2);

            textBox1.Text = fileEntries.Length > 0 ? string.Join("\r\n", fileEntries) : "No files found";

            if (fileEntries.Length > 0)
            {
                for (int contador = 0; contador < fileEntries.Length; contador++)
                {
                    string filename = Path.GetFileNameWithoutExtension(fileEntries[contador]);
                    string filepath = Path.GetDirectoryName(fileEntries[contador]); //ruta sin el fichero y sin el slash al final

                    ImageData DatosImagen = new ImageData(fileEntries[contador], ImageSessionID++);
                    string json_file = filepath + "\\" + filename + ".json";
                    if (json_fileEntries.Contains(json_file))
                    {
                        //MessageBox.Show("JSON encontrado");
                        using (StreamReader r = new StreamReader(json_file))
                        {
                            string json = r.ReadToEnd();
                            DatosImagen.Tags = JsonConvert.DeserializeObject<TagStore>(json);
                        }
                    }
                    //TagStore test1 = new TagStore("MAIN2");
                    //test1.Order = 1;
                    //TagStore test2 = new TagStore("objeto2");
                    //test2.Order = 2;
                    //TagStore test4 = new TagStore("objeto4");
                    //test4.Order = 4;
                    //DatosImagen.Tags.add_children(test1);
                    //DatosImagen.Tags.add_children(test1);
                    //DatosImagen.Tags.add_children(test2);
                    //DatosImagen.Tags.ChildrenList[0].add_children(test4);
                    //DatosImagen.Tags.remove_children(test1);
                    lista_imagenes.Add(DatosImagen);
                }
                Pager = new ImagePager(lista_imagenes);
                Pager.pager_limit = ((int)intPager_Limit.Value); // añadir a la inicializacion?
                Pager.repagination();
                List<int> list_session_ids = Pager.get_all_ids_for_page(0);
                add_imagenes_to_panel_by_ids(list_session_ids);
            }
        }

        private void btn_ThumSize_Click(object sender, EventArgs e)
        {
            thumbnail_size = (int)numericUpDown_ThumbSize.Value;
        }

        private void Config_btn_Click_1(object sender, EventArgs e)
        {
            PanelConfig.Visible = !PanelConfig.Visible;
        }

        private void btn_ApplySizeCategories_Click(object sender, EventArgs e)
        {
            Panel_derecho.Size = new Size((int)SizeOfCategoriesPanelUpDown.Value, 0);
        }
        #endregion

        private void btn_number_categories_Click(object sender, EventArgs e)
        {
            int valor = (int)NumCategoriesUpDown.Value;
            adjust_categories_table(valor);
            add_categories_controls(valor);
        }


        // ############################ IMAGES PANEL MANAGEMENT ############################
        #region Gestion Panel Imagenes
        private void Eliminate_PanelImagenes_SubControls()
        {
            //panel_ext
            for (int ix = this.PanelImagenes.Controls.Count - 1; ix >= 0; ix--)
            {
                //panel_int
                for (int ix2 = this.PanelImagenes.Controls[ix].Controls.Count - 1; ix2 >= 0; ix2--)
                {
                    //imagebox
                    for (int ix3 = this.PanelImagenes.Controls[ix].Controls[ix2].Controls.Count - 1; ix3 >= 0; ix3--)
                    {
                        this.PanelImagenes.Controls[ix].Controls[ix2].Controls[ix3].Dispose();
                    }
                    this.PanelImagenes.Controls[ix].Controls[ix2].Dispose();
                    this.PanelImagenes.Controls[ix].Controls.Clear();
                }

                this.PanelImagenes.Controls[ix].Dispose();

            }
        }

        private void add_imagenes_to_panel_by_ids(List<int> list_session_ids)
        {
            foreach (int image_session_id in list_session_ids)
            {
                ImageData DatosImagen = lista_imagenes.Find(result => result.ImageSessionID == image_session_id);

                add_imagen_to_panel(DatosImagen);
            }
        }

        private void add_imagen_to_panel(ImageData DatosImagen)
        {
            DatosImagen.ThumbNailSize = new System.Drawing.Size(thumbnail_size, thumbnail_size);
            Image thumbnail = DatosImagen.ThumbNail;

            System.Windows.Forms.Panel Panel_temp_int = new System.Windows.Forms.Panel(); //contenedor_interior
            System.Windows.Forms.Panel Panel_temp_ext = new System.Windows.Forms.Panel(); //contenedor_exterior

            Panel_temp_ext.Name = "panel_exterior_" + DatosImagen.ImageSessionID.ToString();
            Panel_temp_ext.Size = new System.Drawing.Size(thumbnail_size + 8, thumbnail_size + 8);
            Panel_temp_ext.ContextMenuStrip = this.contextMenuStrip1;
            if (DatosImagen.Selected)
                Panel_temp_ext.BackColor = Selected;
            else
                Panel_temp_ext.BackColor = UnSelected;

            Panel_temp_int.Name = "panel_interior_" + DatosImagen.ImageSessionID.ToString();
            Panel_temp_int.Size = new System.Drawing.Size(thumbnail_size, thumbnail_size);
            Panel_temp_int.BackColor = System.Drawing.Color.Gray;
            Panel_temp_int.Location = new System.Drawing.Point(4, 4);



            PictureBox Elemento = new System.Windows.Forms.PictureBox();
            Elemento.Name = "picture:" + DatosImagen.ImageSessionID.ToString();
            Elemento.Size = new System.Drawing.Size(thumbnail.Size.Width, thumbnail.Size.Height);
            Elemento.Dock = DockStyle.Fill;
            Elemento.Location = new System.Drawing.Point(0, 0);
            //Elemento.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            Elemento.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            Elemento.Image = thumbnail;

            Elemento.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Elemento_Click);
            //Elemento.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Elemento_Click2);
            Elemento.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(Elemento_MouseDoubleClick);

            Panel_temp_int.Controls.Add(Elemento);
            Panel_temp_ext.Controls.Add(Panel_temp_int);
            PanelImagenes.Controls.Add(Panel_temp_ext);
        }

        #endregion

        private void btn_NextPage_Click(object sender, EventArgs e)
        {
            Eliminate_PanelImagenes_SubControls();
            if (chk_MemorySaver.Checked)
            {
                List<int> Images_to_free_memory = Pager.get_active_ids_for_page();
                foreach (int session_id in Images_to_free_memory)
                {
                    ImageData Image_to_free_memory = lista_imagenes.Find(x => x.ImageSessionID == session_id);
                    Image_to_free_memory.freeImageMemory();
                }
            }

            btn_NotUsed.Text = Pager.Page.ToString();
            Pager.next_page();
            List<int> list_active_ids=Pager.get_active_ids_for_page();
            add_imagenes_to_panel_by_ids(list_active_ids);
            btn_NotUsed.Text = Pager.Page.ToString();
        }

        private void btn_PrevPage_Click(object sender, EventArgs e)
        {
            Eliminate_PanelImagenes_SubControls();
            Pager.previous_page();
            List<int> list_active_ids = Pager.get_active_ids_for_page();
            add_imagenes_to_panel_by_ids(list_active_ids);
            btn_NotUsed.Text = Pager.Page.ToString();
        }

        private void btn_Repagination_Click(object sender, EventArgs e)
        {
            Eliminate_PanelImagenes_SubControls();
            //Faltaria hace dispose de las imagenes descargadas de memoria , De donde sacarlas? -> De la lista de inactivas.
            List<int> inactive_ids=Pager.get_all_inactive_ids();

            foreach (int inactive_id in inactive_ids)
            {
                ImageData imagendata = lista_imagenes.Find(x => x.ImageSessionID == inactive_id);
                //imagendata = null;
                imagendata.Dispose();
                imagendata = null;  //realmente sigue manteniendo los datos de los tags.
            }
            GC.Collect();
            //for (int counter=0;counter<Pager. ;counter++)
            //List<int> list_inactive_ids = Pager.get_inactive_ids_for_page();
            Pager.repagination();
            List<int> list_active_ids = Pager.get_active_ids_for_page(0);
            add_imagenes_to_panel_by_ids(list_active_ids);
            btn_NotUsed.Text = Pager.Page.ToString();
        }

        private void btn_PagerLimit_Click(object sender, EventArgs e)
        {
            Pager.pager_limit=((int)intPager_Limit.Value);
        }

        private void btn_Add_Tag_Click(object sender, EventArgs e)
        {
            List<string> list_of_active_tags = new List<string>();
            DialogResult result_dialog = MessageBox.Show("Add tag to all selected images?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result_dialog == DialogResult.OK)
            {
                //List<string> list_of_active_tags = flowLayoutTags.Controls.OfType<TagElement>().Where(result => result.selected).Select(x=>x.tag).ToList();

                foreach (TagCategory tagCategory1 in tablaCategorias.Controls.OfType<TagCategory>())
                {
                    list_of_active_tags.AddRange(tagCategory1.Get_Active_Tags());
                    foreach (ImageData imageData in lista_imagenes.Where(x => x.Selected))
                        imageData.Tags.add_tag(list_of_active_tags,tagCategory1.Destination);
                    list_of_active_tags.Clear();
                }

            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            IEnumerable<ImageData> imagenes_a_salvar = lista_imagenes.Where(x => ((x.Tags.TagList.Count > 0) || x.Tags.haveChildren));

            foreach (ImageData imageData in imagenes_a_salvar)
            {
                string filename = Path.GetFileNameWithoutExtension( imageData.ImageName);
                string filepath = Path.GetDirectoryName(imageData.ImageName); //ruta sin el fichero y sin el slash al final
                //string filepath2 = Path.GetFullPath(imageData.ImageName);   //ruta con el nombre de fichero

                string save_to_file = filepath + "\\" + filename + ".json";
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(filepath, filename+".json")))
                {
                    outputFile.Write(JsonConvert.SerializeObject(imageData.Tags));
                }

            }

        }

        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            List<int> list_session_ids = Pager.get_active_ids_for_page();
            foreach (int session_id in list_session_ids)
            {
                ImageData datoimagen = lista_imagenes.Find(x => x.ImageSessionID == session_id);
                datoimagen.Selected = true;
            }
            foreach (Control control in PanelImagenes.Controls)
            {
                control.BackColor = Selected;
            }
        }

        private void btn_Select_None_Click(object sender, EventArgs e)
        {
            List<int> list_session_ids = Pager.get_active_ids_for_page();
            foreach (int session_id in list_session_ids)
            {
                ImageData datoimagen = lista_imagenes.Find(x => x.ImageSessionID == session_id);
                datoimagen.Selected = false;
            }
            foreach (Control control in PanelImagenes.Controls)
            {
                control.BackColor = UnSelected;
            }
        }

        private void btn_HideAllSelected_Click(object sender, EventArgs e)
        {
            List<int> activeids = Pager.get_active_ids_for_page();
            List<int> lista_seleccionadas=lista_imagenes.Where(x=>x.Selected).Select(y=>y.ImageSessionID).ToList();
            List<int> interseccion = lista_seleccionadas.Intersect(activeids).ToList();

            for (int counter = PanelImagenes.Controls.Count; counter > 0; counter--)
            {
                Panel panel_externo =(Panel)PanelImagenes.Controls[counter-1];
                Panel panel_interno =(Panel)panel_externo.Controls[0];
                PictureBox imagebox = (PictureBox)panel_interno.Controls[0];
                string[] subs = imagebox.Name.Split(':');
                int image_session_id = int.Parse(subs[1]);

                if (interseccion.Contains(image_session_id))
                {
                    ImageData imagen_seleccionada = lista_imagenes.Find(result => result.ImageSessionID == image_session_id);
                    imagen_seleccionada.Selected = false;
                    Pager.hide_image(image_session_id);
                    panel_interno.Controls.Remove(imagebox);
                    panel_externo.Controls.Remove(panel_interno);
                    PanelImagenes.Controls.Remove(panel_externo);
                    imagebox.Dispose();
                    panel_interno.Dispose();
                    panel_externo.Dispose();
                    //imagen_seleccionada.Dispose();
                }
            }
        }

        private void btn_AddObjectToImg_Click(object sender, EventArgs e)
        {
            string nombre_objeto;
            bool check = false;
            string message = "If you continue, you will add a new object to the current selected images under its top level. Current Template and tags will not be modified,Are you sure?";
            using (QuestionForm question = new QuestionForm("Add New Object to Selected Images",message ))
            {
                //question.IsOnlyInformation();
                DialogResult result = question.ShowDialog();
                nombre_objeto = question.Result;
                if (result == DialogResult.OK)
                    check = true;

            }
            if (check)
            {
                List<ImageData> lista_seleccionadas = lista_imagenes.Where(x => x.Selected).ToList();
                foreach (ImageData datosimagen in lista_seleccionadas)
                {
                    TagStore test = new TagStore(nombre_objeto);
                    datosimagen.Tags.add_children(test);
                }
            }
        }
        private void toolStripSelect_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MenuItem = (ToolStripMenuItem)sender;
            ContextMenuStrip MenuContext = (ContextMenuStrip)MenuItem.GetCurrentParent();
            Control panel_externo = MenuContext.SourceControl;
            Control panel_interno = panel_externo.Controls[0];
            Control imagebox = panel_interno.Controls[0];

            string[] subs = imagebox.Name.Split(':');
            int image_session_id = int.Parse(subs[1]);
            ImageData imagen_seleccionada = lista_imagenes.Find(result => result.ImageSessionID == image_session_id);


            List<int> list_session_ids = Pager.get_active_ids_for_page();
            foreach (int session_id in list_session_ids)
            {
                ImageData datoimagen = lista_imagenes.Find(x => x.ImageSessionID == session_id);
                if (session_id == image_session_id)
                    datoimagen.Selected = true;
                else
                    datoimagen.Selected = false;
            }
            //Lo siguiente (mas rapido) o repintarlas completamente.
            foreach (Control control in PanelImagenes.Controls)
            {
                if (control.Name == panel_externo.Name) control.BackColor = Selected;
                else control.BackColor = UnSelected;
            }



        }

        private void toolStripHide_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem MenuItem = (ToolStripMenuItem)sender;
            ContextMenuStrip MenuContext = (ContextMenuStrip)MenuItem.GetCurrentParent();
            Control panel_externo = MenuContext.SourceControl;
            Control panel_interno = panel_externo.Controls[0];
            Control imagebox = panel_interno.Controls[0];

            string[] subs = imagebox.Name.Split(':');
            int image_session_id = int.Parse(subs[1]);
            ImageData imagen_seleccionada = lista_imagenes.Find(result => result.ImageSessionID == image_session_id);

            imagen_seleccionada.Selected = false;
            Pager.hide_image(image_session_id);
            panel_interno.Controls.Remove(imagebox);
            panel_externo.Controls.Remove(panel_interno);
            PanelImagenes.Controls.Remove(panel_externo);
            imagebox.Dispose();
            panel_interno.Dispose();
            panel_externo.Dispose();
        }


        private void update_category_destinations()
        {
            foreach (TagCategory category in tablaCategorias.Controls.OfType<TagCategory>())
            {
                category.update_destinations(this.TagStoreTemplate.getObjects());
            }
        }


        private void btn_TemplateTagStore_Click(object sender, EventArgs e)
        {
            using (CategoriesTemplateForm form1 = new CategoriesTemplateForm(this.TagStoreTemplate))
            {
                DialogResult confirmation=form1.ShowDialog();
                if (DialogResult.OK == confirmation)
                { 
                    this.TagStoreTemplate =form1.Result;
                    update_category_destinations();
                }
                
            }

        }

        private void btn_ExtractTemplateTagStore_Click(object sender, EventArgs e)
        {
            List<ImageData> lista_seleccionadas = lista_imagenes.Where(x => x.Selected).ToList();
            ImageData dato_imagen;
            if (lista_seleccionadas.Count > 0)
            {
                dato_imagen = lista_seleccionadas.First();
                using (CategoriesTemplateForm form1 = new CategoriesTemplateForm(dato_imagen.Tags))
                {
                    DialogResult confirmation = form1.ShowDialog();
                    if (DialogResult.OK == confirmation)
                    {
                        this.TagStoreTemplate = form1.Result;
                        update_category_destinations();
                    }

                }
            }
            else MessageBox.Show("Select an image");
                
        }

        private void btn_ApplyTemplate_Click(object sender, EventArgs e)
        {
            //var test = delegado_Objetos(this.TagStoreTemplate);
            bool check = false;
            string message = "If you continue, you will apply the current template to all selected images. Current Template and tags will be lost,Are you sure?";
            using (QuestionForm question = new QuestionForm("Apply Template to Selected Images", message))
            {
                question.IsOnlyInformation();
                DialogResult result = question.ShowDialog();
                if (result == DialogResult.OK)
                    check = true;
            }
            if (check)
            {
                List<ImageData> lista_seleccionadas = lista_imagenes.Where(x => x.Selected).ToList();
                foreach (ImageData datosimagen in lista_seleccionadas)
                {
                    datosimagen.Tags = this.TagStoreTemplate.Clone();
                }
            }
        }

        private void btn_categories_dir_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = txtCategoriesDir.Text;
            dialog.IsFolderPicker = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txtCategoriesDir.Text = dialog.FileName;
            }

            string filter = @"*.txt";
            string[] tag_fileEntries = Directory.GetFiles(txtCategoriesDir.Text, filter);

            textBox1.Text = tag_fileEntries.Length > 0 ? string.Join("\r\n", tag_fileEntries) : "No tag files found";

            if (tag_fileEntries.Length > 0)
            {
                foreach(TagCategory category in tablaCategorias.Controls.OfType<TagCategory>())
                {
                    category.ListOfFiles = tag_fileEntries.ToList();
                }
            }
        }

        private void btn_Export_Click(object sender, EventArgs e)
        {
            IEnumerable<ImageData> imagenes_a_salvar = lista_imagenes.Where(x => ((x.Tags.TagList.Count > 0) || x.Tags.haveChildren));

            foreach (ImageData imageData in imagenes_a_salvar)
            {
                string filename = Path.GetFileNameWithoutExtension(imageData.ImageName);
                string filepath = Path.GetDirectoryName(imageData.ImageName); //ruta sin el fichero y sin el slash al final
                //string filepath2 = Path.GetFullPath(imageData.ImageName);   //ruta con el nombre de fichero

                string save_to_file = filepath + "\\" + filename + ".txt";
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(filepath, filename + ".txt")))
                {
                    string resultado = "";
                    IEnumerable<Tuple<string, List<string>>> information = imageData.Tags.getFullObjects().Select(x => new Tuple<string, List<string>>( x.Item1, x.Item3));
                    //se puede quitar la seleccion anterior y trabajar directo con las tuplas resultado
                    foreach (Tuple<string, List<string>> tupla in information)
                    {
                        resultado += tupla.Item1;
                        resultado += ':';
                        foreach (string cadena in tupla.Item2)
                        {
                            resultado += cadena;
                            resultado += ',';
                        }
                        resultado += "\r";
                    }
                    outputFile.Write(resultado);
                }

            }
        }

        private void chk_MemorySaver_CheckedChanged(object sender, EventArgs e)
        {

        }


        //public delegate List<string> GetObjects(TagStore objeto);
        //public GetObjects delegado_Objetos = new GetObjects(DelegateGetObjects);
        //public static List<string> DelegateGetObjects(TagStore TagStoreTemplate1)
        //{
        //    return TagStoreTemplate1.getObjects();
        //}



        /// PASADOS A OTROS SITIOS Y NO USADOS, borrar
        /// 



    } ///Fin de la clase form
}///Fin del namespace




