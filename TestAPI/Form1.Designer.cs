namespace TestAPI
{
	partial class Form1
	{
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

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido de este método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.imgEnviar = new System.Windows.Forms.PictureBox();
			this.imgRecibir = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSubir = new System.Windows.Forms.Button();
			this.cbOpciones = new System.Windows.Forms.ComboBox();
			this.btnEnviar = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.lbTiempo = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.lblObjetos = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imgEnviar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.imgRecibir)).BeginInit();
			this.SuspendLayout();
			// 
			// imgEnviar
			// 
			this.imgEnviar.Location = new System.Drawing.Point(257, 12);
			this.imgEnviar.Name = "imgEnviar";
			this.imgEnviar.Size = new System.Drawing.Size(389, 318);
			this.imgEnviar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.imgEnviar.TabIndex = 0;
			this.imgEnviar.TabStop = false;
			// 
			// imgRecibir
			// 
			this.imgRecibir.Location = new System.Drawing.Point(786, 12);
			this.imgRecibir.Name = "imgRecibir";
			this.imgRecibir.Size = new System.Drawing.Size(389, 318);
			this.imgRecibir.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.imgRecibir.TabIndex = 1;
			this.imgRecibir.TabStop = false;
			this.imgRecibir.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.imgRecibir_MouseDoubleClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(391, 353);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(99, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "IMAGEN ENVIADA";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(946, 353);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "IMAGEN RECIBIDA";
			// 
			// btnSubir
			// 
			this.btnSubir.Location = new System.Drawing.Point(24, 12);
			this.btnSubir.Name = "btnSubir";
			this.btnSubir.Size = new System.Drawing.Size(148, 23);
			this.btnSubir.TabIndex = 4;
			this.btnSubir.Text = "SUBIR IMAGEN";
			this.btnSubir.UseVisualStyleBackColor = true;
			this.btnSubir.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnSubir_MouseClick);
			// 
			// cbOpciones
			// 
			this.cbOpciones.FormattingEnabled = true;
			this.cbOpciones.Items.AddRange(new object[] {
            "ESCALA DE GRISES",
            "BINARIZAR",
            "DETECTAR BORDES",
            "ETIQUETADO",
            "INVARIANTES DE HU"});
			this.cbOpciones.Location = new System.Drawing.Point(12, 80);
			this.cbOpciones.Name = "cbOpciones";
			this.cbOpciones.Size = new System.Drawing.Size(169, 21);
			this.cbOpciones.TabIndex = 5;
			this.cbOpciones.Text = "SELECCIONE UNA OPCIÓN";
			this.cbOpciones.SelectedIndexChanged += new System.EventHandler(this.cbOpciones_SelectedIndexChanged);
			// 
			// btnEnviar
			// 
			this.btnEnviar.Enabled = false;
			this.btnEnviar.Location = new System.Drawing.Point(24, 116);
			this.btnEnviar.Name = "btnEnviar";
			this.btnEnviar.Size = new System.Drawing.Size(148, 23);
			this.btnEnviar.TabIndex = 6;
			this.btnEnviar.Text = "ENVIAR";
			this.btnEnviar.UseVisualStyleBackColor = true;
			this.btnEnviar.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnEnviar_MouseClick);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(21, 54);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(148, 13);
			this.label3.TabIndex = 7;
			this.label3.Text = "SELECCIONA UN PROCESO";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(21, 183);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(51, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "TIEMPO:";
			// 
			// lbTiempo
			// 
			this.lbTiempo.AutoSize = true;
			this.lbTiempo.Location = new System.Drawing.Point(78, 183);
			this.lbTiempo.Name = "lbTiempo";
			this.lbTiempo.Size = new System.Drawing.Size(51, 13);
			this.lbTiempo.TabIndex = 9;
			this.lbTiempo.Text = "TIEMPO:";
			this.lbTiempo.Visible = false;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(21, 210);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(145, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "OBJETOS ENCONTRADOS:";
			this.label5.Visible = false;
			// 
			// lblObjetos
			// 
			this.lblObjetos.AutoSize = true;
			this.lblObjetos.Location = new System.Drawing.Point(21, 238);
			this.lblObjetos.Name = "lblObjetos";
			this.lblObjetos.Size = new System.Drawing.Size(145, 13);
			this.lblObjetos.TabIndex = 11;
			this.lblObjetos.Text = "OBJETOS ENCONTRADOS:";
			this.lblObjetos.Visible = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1187, 375);
			this.Controls.Add(this.lblObjetos);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.lbTiempo);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnEnviar);
			this.Controls.Add(this.cbOpciones);
			this.Controls.Add(this.btnSubir);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.imgRecibir);
			this.Controls.Add(this.imgEnviar);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Form1";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "PROCESAMIENTO";
			((System.ComponentModel.ISupportInitialize)(this.imgEnviar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.imgRecibir)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox imgEnviar;
		private System.Windows.Forms.PictureBox imgRecibir;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSubir;
		private System.Windows.Forms.ComboBox cbOpciones;
		private System.Windows.Forms.Button btnEnviar;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lbTiempo;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label lblObjetos;
	}
}

