namespace filecomp
{
    partial class FolderCompForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.FolderComp = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button6 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.Filter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.release = new System.Windows.Forms.Button();
            this.excel_button = new System.Windows.Forms.Button();
            this.CopyFileCreatrBtn = new System.Windows.Forms.Button();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.FileCopyBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(25, 30);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 29);
            this.button2.TabIndex = 17;
            this.button2.Text = "フォルダ1開く";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(25, 82);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 29);
            this.button4.TabIndex = 18;
            this.button4.Text = "フォルダ2開く";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(157, 38);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(1200, 22);
            this.textBox1.TabIndex = 19;
            this.textBox1.Click += new System.EventHandler(this.button2_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(157, 86);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(1200, 22);
            this.textBox2.TabIndex = 20;
            this.textBox2.Click += new System.EventHandler(this.button4_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(179, 182);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 15);
            this.label1.TabIndex = 24;
            this.label1.Text = "全項目数:";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(265, 178);
            this.textBox3.Margin = new System.Windows.Forms.Padding(4);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(103, 22);
            this.textBox3.TabIndex = 26;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "CSV";
            this.saveFileDialog1.Filter = "CSVファイル(*.CSV)|*.CSV;|すべてのファイル(*.*)|*.*";
            // 
            // FolderComp
            // 
            this.FolderComp.Location = new System.Drawing.Point(28, 284);
            this.FolderComp.Margin = new System.Windows.Forms.Padding(4);
            this.FolderComp.Name = "FolderComp";
            this.FolderComp.Size = new System.Drawing.Size(132, 52);
            this.FolderComp.TabIndex = 29;
            this.FolderComp.Text = "フォルダ比較";
            this.FolderComp.UseVisualStyleBackColor = true;
            this.FolderComp.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(472, 129);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(883, 400);
            this.dataGridView1.TabIndex = 30;
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDoubleClick);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(236, 284);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(133, 52);
            this.button6.TabIndex = 31;
            this.button6.Text = "ファイル書き込み";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(117, 594);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 60);
            this.button1.TabIndex = 32;
            this.button1.Text = "終了";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Filter
            // 
            this.Filter.Location = new System.Drawing.Point(157, 135);
            this.Filter.Margin = new System.Windows.Forms.Padding(4);
            this.Filter.Name = "Filter";
            this.Filter.Size = new System.Drawing.Size(132, 22);
            this.Filter.TabIndex = 33;
            this.Filter.Text = "*.*";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 15);
            this.label2.TabIndex = 34;
            this.label2.Text = "フィルター";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(27, 178);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(133, 19);
            this.checkBox1.TabIndex = 35;
            this.checkBox1.Text = "サブフォルダを含む";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(187, 252);
            this.textBox4.Margin = new System.Windows.Forms.Padding(4);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(225, 22);
            this.textBox4.TabIndex = 36;
            this.textBox4.Text = "C:\\GPCS-2700\\";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(184, 220);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 15);
            this.label3.TabIndex = 37;
            this.label3.Text = "フォルダ名";
            // 
            // release
            // 
            this.release.Location = new System.Drawing.Point(28, 482);
            this.release.Margin = new System.Windows.Forms.Padding(4);
            this.release.Name = "release";
            this.release.Size = new System.Drawing.Size(133, 72);
            this.release.TabIndex = 38;
            this.release.Text = "リリース審査用 CSVファイル";
            this.release.UseVisualStyleBackColor = true;
            this.release.Click += new System.EventHandler(this.release_Click);
            // 
            // excel_button
            // 
            this.excel_button.Location = new System.Drawing.Point(28, 362);
            this.excel_button.Margin = new System.Windows.Forms.Padding(4);
            this.excel_button.Name = "excel_button";
            this.excel_button.Size = new System.Drawing.Size(133, 72);
            this.excel_button.TabIndex = 39;
            this.excel_button.Text = "リリース審査用 EXCELファイル";
            this.excel_button.UseVisualStyleBackColor = true;
            this.excel_button.Click += new System.EventHandler(this.excel_button_Click);
            // 
            // CopyFileCreatrBtn
            // 
            this.CopyFileCreatrBtn.Location = new System.Drawing.Point(235, 364);
            this.CopyFileCreatrBtn.Margin = new System.Windows.Forms.Padding(4);
            this.CopyFileCreatrBtn.Name = "CopyFileCreatrBtn";
            this.CopyFileCreatrBtn.Size = new System.Drawing.Size(133, 72);
            this.CopyFileCreatrBtn.TabIndex = 40;
            this.CopyFileCreatrBtn.Text = "SelectFile作成";
            this.CopyFileCreatrBtn.UseVisualStyleBackColor = true;
            this.CopyFileCreatrBtn.Click += new System.EventHandler(this.CopyFileCreatrBtn_Click);
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.DefaultExt = "txt";
            this.saveFileDialog2.FileName = "SelectFile";
            this.saveFileDialog2.Filter = "\"テキストファイル(*.txt;*.text)|*.txt;*.text|すべてのファイル(*.*)|*.*\"";
            // 
            // FileCopyBtn
            // 
            this.FileCopyBtn.Location = new System.Drawing.Point(235, 482);
            this.FileCopyBtn.Margin = new System.Windows.Forms.Padding(4);
            this.FileCopyBtn.Name = "FileCopyBtn";
            this.FileCopyBtn.Size = new System.Drawing.Size(133, 72);
            this.FileCopyBtn.TabIndex = 41;
            this.FileCopyBtn.Text = "File Copy";
            this.FileCopyBtn.UseVisualStyleBackColor = true;
            // 
            // FolderCompForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1368, 660);
            this.Controls.Add(this.FileCopyBtn);
            this.Controls.Add(this.CopyFileCreatrBtn);
            this.Controls.Add(this.excel_button);
            this.Controls.Add(this.release);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Filter);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.FolderComp);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FolderCompForm";
            this.Text = "フォルダ比較";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button FolderComp;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Filter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button release;
        private System.Windows.Forms.Button excel_button;
        private System.Windows.Forms.Button CopyFileCreatrBtn;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.Button FileCopyBtn;
    }
}

