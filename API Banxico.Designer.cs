namespace CICE_Business_Cycles
{
    partial class API_Banxico
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FechaInicio = new System.Windows.Forms.DateTimePicker();
            this.FechaFin = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SerieSeleccionada = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SerieIdDefault = new System.Windows.Forms.TextBox();
            this.ConsultarAPI = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dataGridB = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.Periodicidad = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridB)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // FechaInicio
            // 
            this.FechaInicio.Location = new System.Drawing.Point(18, 38);
            this.FechaInicio.Name = "FechaInicio";
            this.FechaInicio.Size = new System.Drawing.Size(200, 20);
            this.FechaInicio.TabIndex = 3;
            this.FechaInicio.ValueChanged += new System.EventHandler(this.FechaInicio_ValueChanged);
            // 
            // FechaFin
            // 
            this.FechaFin.Location = new System.Drawing.Point(18, 94);
            this.FechaFin.Name = "FechaFin";
            this.FechaFin.Size = new System.Drawing.Size(200, 20);
            this.FechaFin.TabIndex = 4;
            this.FechaFin.ValueChanged += new System.EventHandler(this.FechaFin_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Fecha de Inicio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Fecha Final";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Periodicidad);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.SerieSeleccionada);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.SerieIdDefault);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.FechaInicio);
            this.groupBox1.Controls.Add(this.FechaFin);
            this.groupBox1.Location = new System.Drawing.Point(406, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(298, 194);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rango de serie";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(15, 130);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Periodicidad";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Serie ID";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // SerieSeleccionada
            // 
            this.SerieSeleccionada.Enabled = false;
            this.SerieSeleccionada.Location = new System.Drawing.Point(18, 168);
            this.SerieSeleccionada.Name = "SerieSeleccionada";
            this.SerieSeleccionada.Size = new System.Drawing.Size(128, 20);
            this.SerieSeleccionada.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 152);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Serie Seleccionada";
            // 
            // SerieIdDefault
            // 
            this.SerieIdDefault.Enabled = false;
            this.SerieIdDefault.Location = new System.Drawing.Point(164, 168);
            this.SerieIdDefault.Name = "SerieIdDefault";
            this.SerieIdDefault.Size = new System.Drawing.Size(128, 20);
            this.SerieIdDefault.TabIndex = 7;
            this.SerieIdDefault.TextChanged += new System.EventHandler(this.SerieIdDefault_TextChanged);
            // 
            // ConsultarAPI
            // 
            this.ConsultarAPI.Location = new System.Drawing.Point(455, 212);
            this.ConsultarAPI.Name = "ConsultarAPI";
            this.ConsultarAPI.Size = new System.Drawing.Size(75, 23);
            this.ConsultarAPI.TabIndex = 8;
            this.ConsultarAPI.Text = "Consultar";
            this.ConsultarAPI.UseVisualStyleBackColor = true;
            this.ConsultarAPI.Click += new System.EventHandler(this.ConsultarAPI_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(570, 212);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(94, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Cerrar Conexión";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(683, 89);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Detalles de la conexión";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(16, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "...";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(16, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "...";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(0, 13);
            this.label7.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(204, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Comprobando conexión al Web Service...";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(331, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -" +
    " - - - - - - - - - - - - - ";
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(13, 12);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(377, 396);
            this.treeView1.TabIndex = 11;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect_1);
            // 
            // dataGridB
            // 
            this.dataGridB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridB.Location = new System.Drawing.Point(406, 241);
            this.dataGridB.Name = "dataGridB";
            this.dataGridB.Size = new System.Drawing.Size(298, 167);
            this.dataGridB.TabIndex = 13;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 414);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(692, 98);
            this.flowLayoutPanel1.TabIndex = 14;
            // 
            // Periodicidad
            // 
            this.Periodicidad.Enabled = false;
            this.Periodicidad.Location = new System.Drawing.Point(118, 126);
            this.Periodicidad.Name = "Periodicidad";
            this.Periodicidad.Size = new System.Drawing.Size(128, 20);
            this.Periodicidad.TabIndex = 12;
            // 
            // API_Banxico
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 518);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridB);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ConsultarAPI);
            this.Controls.Add(this.groupBox1);
            this.Name = "API_Banxico";
            this.Text = " ";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.API_Banxico_FormClosed);
            this.Load += new System.EventHandler(this.API_Banxico_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridB)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DateTimePicker FechaInicio;
        private System.Windows.Forms.DateTimePicker FechaFin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button ConsultarAPI;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox SerieIdDefault;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox SerieSeleccionada;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.DataGridView dataGridB;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Periodicidad;
    }
}