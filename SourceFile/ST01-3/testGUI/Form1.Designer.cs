namespace testGUI {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            Zmp.Imuz.Draw.OrbitCamera orbitCamera1 = new Zmp.Imuz.Draw.OrbitCamera();
            this.imuzDraw1 = new Zmp.Imuz.Draw.ImuzDraw();
            this.SuspendLayout();
            // 
            // imuzDraw1
            // 
            orbitCamera1.AspectRatio = 1F;
            orbitCamera1.Center = new Microsoft.Xna.Framework.Vector3(0F, 0F, 0F);
            orbitCamera1.Distance = 2.446644F;
            orbitCamera1.FieldOfView = 0.7853982F;
            orbitCamera1.LookAt = new Microsoft.Xna.Framework.Vector3(0F, 0F, 0F);
            orbitCamera1.Pitch = 0.5235989F;
            orbitCamera1.Position = new Microsoft.Xna.Framework.Vector3(1.059427F, -1.834983F, 1.223322F);
            orbitCamera1.Up = new Microsoft.Xna.Framework.Vector3(0F, 0F, 1F);
            orbitCamera1.Yaw = -1.047198F;
            this.imuzDraw1.Camera = orbitCamera1;
            this.imuzDraw1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imuzDraw1.Location = new System.Drawing.Point(0, 0);
            this.imuzDraw1.Name = "imuzDraw1";
            this.imuzDraw1.Size = new System.Drawing.Size(656, 374);
            this.imuzDraw1.TabIndex = 0;
            this.imuzDraw1.Text = "imuzDraw1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 374);
            this.Controls.Add(this.imuzDraw1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Zmp.Imuz.Draw.ImuzDraw imuzDraw1;
    }
}

