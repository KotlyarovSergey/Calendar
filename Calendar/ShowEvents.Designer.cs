namespace Calendar
{
    partial class ShowEvents
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlCaption = new System.Windows.Forms.Panel();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblUnFind = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.btnExpImpAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.lblType = new System.Windows.Forms.Label();
            this.lblEventCapt = new System.Windows.Forms.Label();
            this.lblSignal = new System.Windows.Forms.Label();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblDate = new System.Windows.Forms.Label();
            this.chbAllChek = new System.Windows.Forms.CheckBox();
            this.pnlCaption.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCaption
            // 
            this.pnlCaption.BackColor = System.Drawing.Color.PowderBlue;
            this.pnlCaption.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCaption.Controls.Add(this.lblCaption);
            this.pnlCaption.Location = new System.Drawing.Point(-1, 0);
            this.pnlCaption.Name = "pnlCaption";
            this.pnlCaption.Size = new System.Drawing.Size(350, 27);
            this.pnlCaption.TabIndex = 0;
            // 
            // lblCaption
            // 
            this.lblCaption.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblCaption.ForeColor = System.Drawing.Color.Crimson;
            this.lblCaption.Location = new System.Drawing.Point(3, 2);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(341, 23);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "24 март 2012";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMain.Controls.Add(this.lblUnFind);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Location = new System.Drawing.Point(-1, 46);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(350, 49);
            this.pnlMain.TabIndex = 1;
            // 
            // lblUnFind
            // 
            this.lblUnFind.AutoSize = true;
            this.lblUnFind.Location = new System.Drawing.Point(109, 16);
            this.lblUnFind.Name = "lblUnFind";
            this.lblUnFind.Size = new System.Drawing.Size(136, 13);
            this.lblUnFind.TabIndex = 6;
            this.lblUnFind.Text = "Событий не обанаружено";
            this.lblUnFind.Visible = false;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Menu;
            this.panel4.Controls.Add(this.textBox1);
            this.panel4.Controls.Add(this.label12);
            this.panel4.Controls.Add(this.label13);
            this.panel4.Controls.Add(this.label14);
            this.panel4.Controls.Add(this.checkBox2);
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(348, 46);
            this.panel4.TabIndex = 5;
            this.panel4.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(49, 3);
            this.textBox1.MaxLength = 100;
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(241, 42);
            this.textBox1.TabIndex = 8;
            this.textBox1.Text = "При каждом изменении значения форматированного текста элемент управления MaskedTe" +
    "xtBox вызывает событие Tex";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(14, 13);
            this.label12.TabIndex = 7;
            this.label12.Text = "С";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(292, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "в 23:00";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(292, 8);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "за 100 дн.";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(8, 16);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(15, 14);
            this.checkBox2.TabIndex = 0;
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // btnExpImpAdd
            // 
            this.btnExpImpAdd.Location = new System.Drawing.Point(8, 96);
            this.btnExpImpAdd.Name = "btnExpImpAdd";
            this.btnExpImpAdd.Size = new System.Drawing.Size(75, 23);
            this.btnExpImpAdd.TabIndex = 2;
            this.btnExpImpAdd.Text = "Добавить";
            this.btnExpImpAdd.UseVisualStyleBackColor = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Enabled = false;
            this.btnDelete.Location = new System.Drawing.Point(175, 96);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Удалить";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblType.Location = new System.Drawing.Point(22, 30);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(29, 13);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "Тип";
            // 
            // lblEventCapt
            // 
            this.lblEventCapt.AutoSize = true;
            this.lblEventCapt.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblEventCapt.Location = new System.Drawing.Point(141, 30);
            this.lblEventCapt.Name = "lblEventCapt";
            this.lblEventCapt.Size = new System.Drawing.Size(42, 13);
            this.lblEventCapt.TabIndex = 3;
            this.lblEventCapt.Text = "Текст";
            // 
            // lblSignal
            // 
            this.lblSignal.AutoSize = true;
            this.lblSignal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblSignal.Location = new System.Drawing.Point(293, 30);
            this.lblSignal.Name = "lblSignal";
            this.lblSignal.Size = new System.Drawing.Size(49, 13);
            this.lblSignal.TabIndex = 4;
            this.lblSignal.Text = "Сигнал";
            // 
            // btnEdit
            // 
            this.btnEdit.Enabled = false;
            this.btnEdit.Location = new System.Drawing.Point(92, 96);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Изменить";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDate.Location = new System.Drawing.Point(66, 30);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(37, 13);
            this.lblDate.TabIndex = 5;
            this.lblDate.Text = "Дата";
            this.lblDate.Visible = false;
            // 
            // chbAllChek
            // 
            this.chbAllChek.AutoSize = true;
            this.chbAllChek.Location = new System.Drawing.Point(7, 30);
            this.chbAllChek.Name = "chbAllChek";
            this.chbAllChek.Size = new System.Drawing.Size(15, 14);
            this.chbAllChek.TabIndex = 6;
            this.chbAllChek.UseVisualStyleBackColor = true;
            this.chbAllChek.CheckedChanged += new System.EventHandler(this.chbAllChek_CheckedChanged);
            // 
            // ShowEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 122);
            this.Controls.Add(this.chbAllChek);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.lblSignal);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.lblEventCapt);
            this.Controls.Add(this.btnExpImpAdd);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlCaption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "ShowEvents";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Просмотр событий";
            this.Load += new System.EventHandler(this.ShowEvents_Load);
            this.pnlCaption.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlCaption;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Button btnExpImpAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label lblSignal;
        private System.Windows.Forms.Label lblEventCapt;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Label lblUnFind;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.CheckBox chbAllChek;
    }
}