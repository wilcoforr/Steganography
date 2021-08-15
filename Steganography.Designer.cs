
namespace Steganography
{
    partial class Steganography
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Steganography));
            this.pb_image = new System.Windows.Forms.PictureBox();
            this.txtbx_message = new System.Windows.Forms.TextBox();
            this.btn_encode = new System.Windows.Forms.Button();
            this.btn_decode = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pb_image)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_image
            // 
            this.pb_image.Image = ((System.Drawing.Image)(resources.GetObject("pb_image.Image")));
            this.pb_image.Location = new System.Drawing.Point(12, 86);
            this.pb_image.Name = "pb_image";
            this.pb_image.Size = new System.Drawing.Size(1240, 583);
            this.pb_image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_image.TabIndex = 0;
            this.pb_image.TabStop = false;
            // 
            // txtbx_message
            // 
            this.txtbx_message.Location = new System.Drawing.Point(13, 13);
            this.txtbx_message.Multiline = true;
            this.txtbx_message.Name = "txtbx_message";
            this.txtbx_message.Size = new System.Drawing.Size(972, 67);
            this.txtbx_message.TabIndex = 1;
            // 
            // btn_encode
            // 
            this.btn_encode.Location = new System.Drawing.Point(992, 13);
            this.btn_encode.Name = "btn_encode";
            this.btn_encode.Size = new System.Drawing.Size(260, 23);
            this.btn_encode.TabIndex = 2;
            this.btn_encode.Text = "Encode";
            this.btn_encode.UseVisualStyleBackColor = true;
            this.btn_encode.Click += new System.EventHandler(this.btn_encode_Click);
            // 
            // btn_decode
            // 
            this.btn_decode.Location = new System.Drawing.Point(992, 56);
            this.btn_decode.Name = "btn_decode";
            this.btn_decode.Size = new System.Drawing.Size(260, 23);
            this.btn_decode.TabIndex = 3;
            this.btn_decode.Text = "Decode";
            this.btn_decode.UseVisualStyleBackColor = true;
            this.btn_decode.Click += new System.EventHandler(this.btn_decode_Click);
            // 
            // Steganography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.btn_decode);
            this.Controls.Add(this.btn_encode);
            this.Controls.Add(this.txtbx_message);
            this.Controls.Add(this.pb_image);
            this.Name = "Steganography";
            this.Text = "Steganography";
            ((System.ComponentModel.ISupportInitialize)(this.pb_image)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_image;
        private System.Windows.Forms.TextBox txtbx_message;
        private System.Windows.Forms.Button btn_encode;
        private System.Windows.Forms.Button btn_decode;
    }
}

