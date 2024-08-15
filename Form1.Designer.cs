namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            timer_Picture = new System.Windows.Forms.Timer(components);
            groupBox1 = new GroupBox();
            button3 = new Button();
            label2 = new Label();
            button2 = new Button();
            label1 = new Label();
            textBox1 = new TextBox();
            button1 = new Button();
            groupBox2 = new GroupBox();
            btnVideoManual = new Button();
            label3 = new Label();
            btnVideoStop = new Button();
            label4 = new Label();
            textBox2 = new TextBox();
            btnVideoStart = new Button();
            timer_Video = new System.Windows.Forms.Timer(components);
            textBox3 = new TextBox();
            button4 = new Button();
            button5 = new Button();
            txtTTS = new TextBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // timer_Picture
            // 
            timer_Picture.Interval = 1000;
            timer_Picture.Tick += timer1_Tick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(button2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(button1);
            groupBox1.Location = new Point(2, 2);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(426, 100);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Text/Picture Content";
            // 
            // button3
            // 
            button3.Location = new Point(193, 59);
            button3.Name = "button3";
            button3.Size = new Size(75, 23);
            button3.TabIndex = 11;
            button3.Text = "Manual Post";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_ClickAsync;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(233, 26);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 10;
            label2.Text = "Minute";
            // 
            // button2
            // 
            button2.Location = new Point(93, 59);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 9;
            button2.Text = "Stop";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 21);
            label1.Name = "label1";
            label1.Size = new Size(79, 15);
            label1.TabIndex = 8;
            label1.Text = "Timer Interval";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(93, 18);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 23);
            textBox1.TabIndex = 7;
            textBox1.Text = "5";
            // 
            // button1
            // 
            button1.Location = new Point(8, 59);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 6;
            button1.Text = "Start";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnVideoManual);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(btnVideoStop);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(btnVideoStart);
            groupBox2.Location = new Point(2, 118);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(426, 100);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Video Content";
            // 
            // btnVideoManual
            // 
            btnVideoManual.Location = new Point(193, 59);
            btnVideoManual.Name = "btnVideoManual";
            btnVideoManual.Size = new Size(75, 23);
            btnVideoManual.TabIndex = 11;
            btnVideoManual.Text = "Manual Post";
            btnVideoManual.UseVisualStyleBackColor = true;
            btnVideoManual.Click += btnVideoManual_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(233, 26);
            label3.Name = "label3";
            label3.Size = new Size(45, 15);
            label3.TabIndex = 10;
            label3.Text = "Minute";
            // 
            // btnVideoStop
            // 
            btnVideoStop.Location = new Point(93, 59);
            btnVideoStop.Name = "btnVideoStop";
            btnVideoStop.Size = new Size(75, 23);
            btnVideoStop.TabIndex = 9;
            btnVideoStop.Text = "Stop";
            btnVideoStop.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(8, 21);
            label4.Name = "label4";
            label4.Size = new Size(79, 15);
            label4.TabIndex = 8;
            label4.Text = "Timer Interval";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(93, 18);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 23);
            textBox2.TabIndex = 7;
            textBox2.Text = "5";
            // 
            // btnVideoStart
            // 
            btnVideoStart.Location = new Point(8, 59);
            btnVideoStart.Name = "btnVideoStart";
            btnVideoStart.Size = new Size(75, 23);
            btnVideoStart.TabIndex = 6;
            btnVideoStart.Text = "Start";
            btnVideoStart.UseVisualStyleBackColor = true;
            btnVideoStart.Click += btnVideoStart_Click;
            // 
            // timer_Video
            // 
            timer_Video.Tick += timer_Video_Tick;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(558, 131);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(201, 104);
            textBox3.TabIndex = 14;
            // 
            // button4
            // 
            button4.Location = new Point(14, 420);
            button4.Name = "button4";
            button4.Size = new Size(106, 23);
            button4.TabIndex = 12;
            button4.Text = "TextToSpeech";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // button5
            // 
            button5.Location = new Point(145, 420);
            button5.Name = "button5";
            button5.Size = new Size(75, 23);
            button5.TabIndex = 12;
            button5.Text = "Decrypt/Encrypt";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // txtTTS
            // 
            txtTTS.Location = new Point(14, 232);
            txtTTS.Multiline = true;
            txtTTS.Name = "txtTTS";
            txtTTS.Size = new Size(414, 182);
            txtTTS.TabIndex = 15;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 517);
            Controls.Add(txtTTS);
            Controls.Add(button5);
            Controls.Add(button4);
            Controls.Add(textBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer timer_Picture;
        private GroupBox groupBox1;
        private Button button3;
        private Label label2;
        private Button button2;
        private Label label1;
        private TextBox textBox1;
        private Button button1;
        private GroupBox groupBox2;
        private Button btnVideoManual;
        private Label label3;
        private Button btnVideoStop;
        private Label label4;
        private TextBox textBox2;
        private Button btnVideoStart;
        private System.Windows.Forms.Timer timer_Video;
        private TextBox textBox3;
        private Button button4;
        private Button button5;
        private TextBox txtTTS;
    }
}