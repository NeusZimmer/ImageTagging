using System;
using System.Windows.Forms;

namespace ImageTagging.Classes
{
    public partial class SearchField : UserControl
    {
        public event EventHandler AddClick;  //El evento al que suscribirse para recibir el tag
        public event EventHandler UpdatedTag;  //El evento al que suscribirse para recibir el tag
        public string tag { get { return txtbox_Search.Text; } }


        public SearchField(bool test)
        {
            InitializeComponent();
            if (test)
            {
                this.AddClick += new System.EventHandler(this.tester); //Como suscribirse al evento
                this.UpdatedTag += new System.EventHandler(this.tester); //Como suscribirse al evento
            }
        }
        public SearchField()
        {
            InitializeComponent();
        }


        //{ get {return lbl_Add.Click(); } set {; } }



        private void lbl_Clear_Click(object sender, EventArgs e)
        {
            txtbox_Search.Text = "";
            if ((this.UpdatedTag != null)) //si hay eventos suscritos
                this.UpdatedTag(this, e);
        }

        private void tester(object sender, EventArgs e)
        {
            SearchField elemento = (SearchField)sender;
            MessageBox.Show("LanzaEvento:" + elemento.tag);
        }

        private void lbl_Add_Click(object sender, EventArgs e)
        {
            if (this.AddClick != null) //si hay eventos suscritos
            {
                this.AddClick(this, e);
            }
        }

        private void txtbox_Search_TextChanged(object sender, EventArgs e)
        {
            if ((txtbox_Search.Text.Length > 0) && (this.UpdatedTag != null)) //si hay eventos suscritos y es mas de dos caracteres
            {
                this.UpdatedTag(this, e);
            }
        }

        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        private void InitializeComponent()
        {
            this.txtbox_Search = new System.Windows.Forms.TextBox();
            this.lbl_Clear = new System.Windows.Forms.Label();
            this.lbl_Add = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtbox_Search
            // 
            this.txtbox_Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbox_Search.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.txtbox_Search.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbox_Search.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtbox_Search.Location = new System.Drawing.Point(0, 0);
            this.txtbox_Search.Margin = new System.Windows.Forms.Padding(0);
            this.txtbox_Search.MaxLength = 40;
            this.txtbox_Search.MinimumSize = new System.Drawing.Size(150, 30);
            this.txtbox_Search.Name = "txtbox_Search";
            this.txtbox_Search.Size = new System.Drawing.Size(198, 30);
            this.txtbox_Search.TabIndex = 0;
            this.txtbox_Search.TextChanged += new System.EventHandler(this.txtbox_Search_TextChanged);
            // 
            // lbl_Clear
            // 
            this.lbl_Clear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Clear.BackColor = System.Drawing.Color.PaleVioletRed;
            this.lbl_Clear.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Clear.Location = new System.Drawing.Point(201, 0);
            this.lbl_Clear.Name = "lbl_Clear";
            this.lbl_Clear.Size = new System.Drawing.Size(20, 30);
            this.lbl_Clear.TabIndex = 1;
            this.lbl_Clear.Text = "X";
            this.lbl_Clear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Clear.Click += new System.EventHandler(this.lbl_Clear_Click);
            // 
            // lbl_Add
            // 
            this.lbl_Add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_Add.BackColor = System.Drawing.Color.LightGreen;
            this.lbl_Add.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Add.Location = new System.Drawing.Point(224, 0);
            this.lbl_Add.Margin = new System.Windows.Forms.Padding(0);
            this.lbl_Add.Name = "lbl_Add";
            this.lbl_Add.Size = new System.Drawing.Size(20, 30);
            this.lbl_Add.TabIndex = 2;
            this.lbl_Add.Text = "+";
            this.lbl_Add.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl_Add.Click += new System.EventHandler(this.lbl_Add_Click);
            // 
            // SearchField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lbl_Add);
            this.Controls.Add(this.lbl_Clear);
            this.Controls.Add(this.txtbox_Search);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SearchField";
            this.Size = new System.Drawing.Size(244, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtbox_Search;
        private System.Windows.Forms.Label lbl_Clear;
        private System.Windows.Forms.Label lbl_Add;
    }
}
