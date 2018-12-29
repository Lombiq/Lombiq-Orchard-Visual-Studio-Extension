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
            this.fieldTypeTextBox = new System.Windows.Forms.TextBox();
            this.fieldTypeLabel = new System.Windows.Forms.Label();
            this.parameterNameTextBox = new System.Windows.Forms.TextBox();
            this.parameterNameLabel = new System.Windows.Forms.Label();
            this.parameterTypeTextBox = new System.Windows.Forms.TextBox();
            this.parameterTypeLabel = new System.Windows.Forms.Label();
            this.injectedDependencyAndFieldDataGroupBox = new System.Windows.Forms.GroupBox();
            this.injectedDependencyAndFieldDataGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(224, 267);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(305, 267);
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
            this.dependencyNameTextBox.Location = new System.Drawing.Point(114, 49);
            this.dependencyNameTextBox.Name = "dependencyNameTextBox";
            this.dependencyNameTextBox.Size = new System.Drawing.Size(260, 20);
            this.dependencyNameTextBox.TabIndex = 3;
            this.dependencyNameTextBox.TextChanged += new System.EventHandler(this.DependencyNameTextBoxTextChanged);
            // 
            // separatorLabel
            // 
            this.separatorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Location = new System.Drawing.Point(-1, 254);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(400, 2);
            this.separatorLabel.TabIndex = 4;
            this.separatorLabel.Text = " ";
            // 
            // fieldNameTextBox
            // 
            this.fieldNameTextBox.Location = new System.Drawing.Point(102, 19);
            this.fieldNameTextBox.Name = "fieldNameTextBox";
            this.fieldNameTextBox.Size = new System.Drawing.Size(260, 20);
            this.fieldNameTextBox.TabIndex = 6;
            // 
            // fieldNameLabel
            // 
            this.fieldNameLabel.AutoSize = true;
            this.fieldNameLabel.Location = new System.Drawing.Point(6, 22);
            this.fieldNameLabel.Name = "fieldNameLabel";
            this.fieldNameLabel.Size = new System.Drawing.Size(61, 13);
            this.fieldNameLabel.TabIndex = 5;
            this.fieldNameLabel.Text = "Field name:";
            // 
            // generateShortFieldNameCheckBox
            // 
            this.generateShortFieldNameCheckBox.AutoSize = true;
            this.generateShortFieldNameCheckBox.Location = new System.Drawing.Point(114, 75);
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
            // fieldTypeTextBox
            // 
            this.fieldTypeTextBox.Location = new System.Drawing.Point(102, 45);
            this.fieldTypeTextBox.Name = "fieldTypeTextBox";
            this.fieldTypeTextBox.Size = new System.Drawing.Size(260, 20);
            this.fieldTypeTextBox.TabIndex = 14;
            // 
            // fieldTypeLabel
            // 
            this.fieldTypeLabel.AutoSize = true;
            this.fieldTypeLabel.Location = new System.Drawing.Point(6, 48);
            this.fieldTypeLabel.Name = "fieldTypeLabel";
            this.fieldTypeLabel.Size = new System.Drawing.Size(55, 13);
            this.fieldTypeLabel.TabIndex = 13;
            this.fieldTypeLabel.Text = "Field type:";
            // 
            // parameterNameTextBox
            // 
            this.parameterNameTextBox.Location = new System.Drawing.Point(102, 71);
            this.parameterNameTextBox.Name = "parameterNameTextBox";
            this.parameterNameTextBox.Size = new System.Drawing.Size(260, 20);
            this.parameterNameTextBox.TabIndex = 16;
            // 
            // parameterNameLabel
            // 
            this.parameterNameLabel.AutoSize = true;
            this.parameterNameLabel.Location = new System.Drawing.Point(6, 74);
            this.parameterNameLabel.Name = "parameterNameLabel";
            this.parameterNameLabel.Size = new System.Drawing.Size(87, 13);
            this.parameterNameLabel.TabIndex = 15;
            this.parameterNameLabel.Text = "Parameter name:";
            // 
            // parameterTypeTextBox
            // 
            this.parameterTypeTextBox.Location = new System.Drawing.Point(102, 97);
            this.parameterTypeTextBox.Name = "parameterTypeTextBox";
            this.parameterTypeTextBox.Size = new System.Drawing.Size(260, 20);
            this.parameterTypeTextBox.TabIndex = 18;
            // 
            // parameterTypeLabel
            // 
            this.parameterTypeLabel.AutoSize = true;
            this.parameterTypeLabel.Location = new System.Drawing.Point(6, 100);
            this.parameterTypeLabel.Name = "parameterTypeLabel";
            this.parameterTypeLabel.Size = new System.Drawing.Size(81, 13);
            this.parameterTypeLabel.TabIndex = 17;
            this.parameterTypeLabel.Text = "Parameter type:";
            // 
            // injectedDependencyAndFieldDataGroupBox
            // 
            this.injectedDependencyAndFieldDataGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.fieldNameTextBox);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.parameterTypeTextBox);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.fieldNameLabel);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.parameterTypeLabel);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.parameterNameTextBox);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.fieldTypeLabel);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.parameterNameLabel);
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.fieldTypeTextBox);
            this.injectedDependencyAndFieldDataGroupBox.Location = new System.Drawing.Point(12, 112);
            this.injectedDependencyAndFieldDataGroupBox.Name = "injectedDependencyAndFieldDataGroupBox";
            this.injectedDependencyAndFieldDataGroupBox.Size = new System.Drawing.Size(371, 128);
            this.injectedDependencyAndFieldDataGroupBox.TabIndex = 19;
            this.injectedDependencyAndFieldDataGroupBox.TabStop = false;
            this.injectedDependencyAndFieldDataGroupBox.Text = "Injected dependency and field data";
            // 
            // InjectDependencyDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(395, 302);
            this.Controls.Add(this.injectedDependencyAndFieldDataGroupBox);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.dependencyNameTextBox);
            this.Controls.Add(this.generateShortFieldNameCheckBox);
            this.Controls.Add(this.dependencyLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InjectDependencyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inject Dependency";
            this.injectedDependencyAndFieldDataGroupBox.ResumeLayout(false);
            this.injectedDependencyAndFieldDataGroupBox.PerformLayout();
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
        private System.Windows.Forms.TextBox fieldTypeTextBox;
        private System.Windows.Forms.Label fieldTypeLabel;
        private System.Windows.Forms.TextBox parameterNameTextBox;
        private System.Windows.Forms.Label parameterNameLabel;
        private System.Windows.Forms.TextBox parameterTypeTextBox;
        private System.Windows.Forms.Label parameterTypeLabel;
        private System.Windows.Forms.GroupBox injectedDependencyAndFieldDataGroupBox;
    }
}