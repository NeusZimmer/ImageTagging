using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagStore = SearchTag_controls.TagStore;

namespace ImageTagging.Classes
{
    class ImageData
    {
        private Image _image = null;
        private Image _thumbnail = null;
        private int? _ImageSessionID = null;

        public string ImageName;
        public bool Selected { get; set; }
        public int ImageSessionID { get { return (int)_ImageSessionID; } }
        public System.Drawing.Size ThumbNailSize = new System.Drawing.Size(256, 256);
        public TagStore Tags;

        public Image ThumbNail
        {
            get
            {
                if ((this._thumbnail == null) || (_thumbnail.Size != this.ThumbNailSize))
                    this.LoadThumbnail();

                return this._thumbnail;
            }
        }
        public Image Image
        {
            get
            {
                if (this._image == null)
                    this.LoadImage();
                return this._image;
            }
        }

        public ImageData(string ImageName_in, int SessionID)
        {
            this.ImageName = ImageName_in;
            this._ImageSessionID = SessionID;
            this.Selected = false;
            this.Tags = new TagStore("MAIN");
        }

        private System.Drawing.Image LoadThumbnail()
        {
            if (_thumbnail == null)
            {
                if (_image == null)
                    LoadImage();
                _thumbnail = createThumbnail(_image);
            }
            return _thumbnail;
        }

        private Image createThumbnail(Image image)
        {
            Image srcBmp = image;
            int width = this.ThumbNailSize.Width;
            int height = this.ThumbNailSize.Height;
            float tamano_w = (float)srcBmp.Size.Width;
            int tamano_h = srcBmp.Size.Height;

            if (tamano_w < tamano_h) ///mas alto que ancho
            {
                float proportion = (tamano_w / tamano_h);
                width = (int)(width * proportion);
            }
            else if (tamano_w > tamano_h)  ///mas ancho que alto
            {
                float proportion = (tamano_h / tamano_w);
                height = (int)(height * proportion);
            }
            /// else  Son iguales, no tocar
            Bitmap target = new Bitmap((int)width, (int)height);
            using (Graphics graphics = Graphics.FromImage(target))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(srcBmp, 0, 0, width, height);
            }
            return target;
        }

        private System.Drawing.Image LoadImage()
        {
            using (var stream = File.OpenRead(ImageName))
                this._image = System.Drawing.Image.FromStream(stream);
            return this._image;
        }

        public void freeImageMemory()
        {
            if (this._image != null)
            { 
                this._image.Dispose();
                this._image = null;
            }
        }
        internal void Dispose()
        {
            if (this._image != null)
                _image.Dispose();
            if (this._thumbnail != null)
                _thumbnail.Dispose();
            //ThumbNail.Dispose();
            //Image.Dispose();
            //ImageSessionID.Dispose();
            //ImageName.Dispose();
            //Selected.Dispose();
            //ImageSessionID.Dispose();
            //ThumbNailSize.Dispose();
        }
    } //end class

} //end namespace
