namespace Lombiq.Vsix.Orchard.Forms
{
    partial class InjectDependencyDialog
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
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.dependencyLabel = new System.Windows.Forms.Label();
            this.dependencyNameTextBox = new System.Windows.Forms.TextBox();
            this.separatorLabel = new System.Windows.Forms.Label();
            this.fieldNameTextBox = new System.Windows.Forms.TextBox();
            this.fieldNameLabel = new System.Windows.Forms.Label();
            this.generateShortFieldNameCheckBox = new System.Windows.Forms.CheckBox();
            this.hintLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(193, 154);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(274, 154);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // dependencyLabel
            // 
            this.dependencyLabel.AutoSize = true;
            this.dependencyLabel.Location = new System.Drawing.Point(12, 52);
            this.dependencyLabel.Name = "dependencyLabel";
            this.dependencyLabel.Size = new System.Drawing.Size(71, 13);
            this.dependencyLabel.TabIndex = 2;
            this.dependencyLabel.Text = "Dependency:";
            // 
            // dependencyNameTextBox
            // 
            this.dependencyNameTextBox.Location = new System.Drawing.Point(89, 49);
            this.dependencyNameTextBox.Name = "dependencyNameTextBox";
            this.dependencyNameTextBox.Size = new System.Drawing.Size(260, 20);
            this.dependencyNameTextBox.TabIndex = 3;
            this.dependencyNameTextBox.TextChanged += new System.EventHandler(this.DependencyNameTextBoxTextChanged);
            // 
            // separatorLabel
            // 
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Location = new System.Drawing.Point(-1, 136);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(362, 2);
            this.separatorLabel.TabIndex = 4;
            this.separatorLabel.Text = " ";
            // 
            // fieldNameTextBox
            // 
            this.fieldNameTextBox.Location = new System.Drawing.Point(89, 73);
            this.fieldNameTextBox.Name = "fieldNameTextBox";
            this.fieldNameTextBox.Size = new System.Drawing.Size(260, 20);
            this.fieldNameTextBox.TabIndex = 6;
            // 
            // fieldNameLabel
            // 
            this.fieldNameLabel.AutoSize = true;
            this.fieldNameLabel.Location = new System.Drawing.Point(12, 76);
            this.fieldNameLabel.Name = "fieldNameLabel";
            this.fieldNameLabel.Size = new System.Drawing.Size(61, 13);
            this.fieldNameLabel.TabIndex = 5;
            this.fieldNameLabel.Text = "Field name:";
            // 
            // generateShortFieldNameCheckBox
            // 
            this.generateShortFieldNameCheckBox.AutoSize = true;
            this.generateShortFieldNameCheckBox.Location = new System.Drawing.Point(89, 99);
            this.generateShortFieldNameCheckBox.Name = "generateShortFieldNameCheckBox";
            this.generateShortFieldNameCheckBox.Size = new System.Drawing.Size(147, 17);
            this.generateShortFieldNameCheckBox.TabIndex = 7;
            this.generateShortFieldNameCheckBox.Text = "Generate short field name";
            this.generateShortFieldNameCheckBox.UseVisualStyleBackColor = true;
            this.generateShortFieldNameCheckBox.CheckedChanged += new System.EventHandler(this.DependencyNameTextBoxTextChanged);
            // 
            // hintLabel
            // 
            this.hintLabel.AutoSize = true;
            this.hintLabel.Location = new System.Drawing.Point(12, 12);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(257, 13);
            this.hintLabel.TabIndex = 12;
            this.hintLabel.Text = "Give the name of the dependency you want to inject.";
            // 
            // InjectDependencyDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(361, 189);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.generateShortFieldNameCheckBox);
            this.Controls.Add(this.fieldNameTextBox);
            this.Controls.Add(this.fieldNameLabel);
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.dependencyNameTextBox);
            this.Controls.Add(this.dependencyLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InjectDependencyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inject Dependency";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label dependencyLabel;
        private System.Windows.Forms.TextBox dependencyNameTextBox;
        private System.Windows.Forms.Label separatorLabel;
        private System.Windows.Forms.TextBox fieldNameTextBox;
        private System.Windows.Forms.Label fieldNameLabel;
        private System.Windows.Forms.CheckBox generateShortFieldNameCheckBox;
        private System.Windows.Forms.Label hintLabel;
    }
}