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
            this.visualizePanel = new System.Windows.Forms.Panel();
            this.visualizeConstructorClosingParenthesisLabel = new System.Windows.Forms.Label();
            this.visualizeInjectedNameLabel = new System.Windows.Forms.Label();
            this.visualizeFieldNameLabel = new System.Windows.Forms.Label();
            this.visualizeInjectedTypeLabel = new System.Windows.Forms.Label();
            this.visualizeClassNameLabel = new System.Windows.Forms.Label();
            this.visualizeConstructorPublicLabel = new System.Windows.Forms.Label();
            this.visualizeFieldTypeLabel = new System.Windows.Forms.Label();
            this.visualizePrivateReadonlyTextLabel = new System.Windows.Forms.Label();
            this.injectedDependencyAndFieldDataGroupBox.SuspendLayout();
            this.visualizePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(330, 368);
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
            this.cancelButton.Location = new System.Drawing.Point(411, 368);
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
            this.dependencyNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dependencyNameTextBox.Location = new System.Drawing.Point(114, 49);
            this.dependencyNameTextBox.Name = "dependencyNameTextBox";
            this.dependencyNameTextBox.Size = new System.Drawing.Size(366, 20);
            this.dependencyNameTextBox.TabIndex = 3;
            this.dependencyNameTextBox.TextChanged += new System.EventHandler(this.DependencyNameTextBoxTextChanged);
            this.dependencyNameTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.DependencyNameTextBoxPreviewKeyDown);
            // 
            // separatorLabel
            // 
            this.separatorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.separatorLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.separatorLabel.Location = new System.Drawing.Point(-1, 355);
            this.separatorLabel.Name = "separatorLabel";
            this.separatorLabel.Size = new System.Drawing.Size(506, 2);
            this.separatorLabel.TabIndex = 4;
            this.separatorLabel.Text = " ";
            // 
            // fieldNameTextBox
            // 
            this.fieldNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldNameTextBox.Location = new System.Drawing.Point(102, 19);
            this.fieldNameTextBox.Name = "fieldNameTextBox";
            this.fieldNameTextBox.Size = new System.Drawing.Size(366, 20);
            this.fieldNameTextBox.TabIndex = 6;
            this.fieldNameTextBox.TextChanged += new System.EventHandler(this.DependencyInjectionDataTextBoxTextChanged);
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
            this.fieldTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fieldTypeTextBox.Location = new System.Drawing.Point(102, 45);
            this.fieldTypeTextBox.Name = "fieldTypeTextBox";
            this.fieldTypeTextBox.Size = new System.Drawing.Size(366, 20);
            this.fieldTypeTextBox.TabIndex = 14;
            this.fieldTypeTextBox.TextChanged += new System.EventHandler(this.DependencyInjectionDataTextBoxTextChanged);
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
            this.parameterNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterNameTextBox.Location = new System.Drawing.Point(102, 71);
            this.parameterNameTextBox.Name = "parameterNameTextBox";
            this.parameterNameTextBox.Size = new System.Drawing.Size(366, 20);
            this.parameterNameTextBox.TabIndex = 16;
            this.parameterNameTextBox.TextChanged += new System.EventHandler(this.DependencyInjectionDataTextBoxTextChanged);
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
            this.parameterTypeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterTypeTextBox.Location = new System.Drawing.Point(102, 97);
            this.parameterTypeTextBox.Name = "parameterTypeTextBox";
            this.parameterTypeTextBox.Size = new System.Drawing.Size(366, 20);
            this.parameterTypeTextBox.TabIndex = 18;
            this.parameterTypeTextBox.TextChanged += new System.EventHandler(this.DependencyInjectionDataTextBoxTextChanged);
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
            this.injectedDependencyAndFieldDataGroupBox.Controls.Add(this.visualizePanel);
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
            this.injectedDependencyAndFieldDataGroupBox.Size = new System.Drawing.Size(477, 229);
            this.injectedDependencyAndFieldDataGroupBox.TabIndex = 19;
            this.injectedDependencyAndFieldDataGroupBox.TabStop = false;
            this.injectedDependencyAndFieldDataGroupBox.Text = "Injected dependency and field data";
            // 
            // visualizePanel
            // 
            this.visualizePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.visualizePanel.BackColor = System.Drawing.Color.White;
            this.visualizePanel.Controls.Add(this.visualizeConstructorClosingParenthesisLabel);
            this.visualizePanel.Controls.Add(this.visualizeInjectedNameLabel);
            this.visualizePanel.Controls.Add(this.visualizeFieldNameLabel);
            this.visualizePanel.Controls.Add(this.visualizeInjectedTypeLabel);
            this.visualizePanel.Controls.Add(this.visualizeClassNameLabel);
            this.visualizePanel.Controls.Add(this.visualizeConstructorPublicLabel);
            this.visualizePanel.Controls.Add(this.visualizeFieldTypeLabel);
            this.visualizePanel.Controls.Add(this.visualizePrivateReadonlyTextLabel);
            this.visualizePanel.Location = new System.Drawing.Point(9, 123);
            this.visualizePanel.Name = "visualizePanel";
            this.visualizePanel.Size = new System.Drawing.Size(459, 100);
            this.visualizePanel.TabIndex = 19;
            // 
            // visualizeConstructorClosingParenthesisLabel
            // 
            this.visualizeConstructorClosingParenthesisLabel.AutoSize = true;
            this.visualizeConstructorClosingParenthesisLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeConstructorClosingParenthesisLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeConstructorClosingParenthesisLabel.Location = new System.Drawing.Point(276, 59);
            this.visualizeConstructorClosingParenthesisLabel.Name = "visualizeConstructorClosingParenthesisLabel";
            this.visualizeConstructorClosingParenthesisLabel.Size = new System.Drawing.Size(13, 13);
            this.visualizeConstructorClosingParenthesisLabel.TabIndex = 23;
            this.visualizeConstructorClosingParenthesisLabel.Text = ")";
            // 
            // visualizeInjectedNameLabel
            // 
            this.visualizeInjectedNameLabel.AutoSize = true;
            this.visualizeInjectedNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeInjectedNameLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeInjectedNameLabel.Location = new System.Drawing.Point(187, 59);
            this.visualizeInjectedNameLabel.Name = "visualizeInjectedNameLabel";
            this.visualizeInjectedNameLabel.Size = new System.Drawing.Size(91, 13);
            this.visualizeInjectedNameLabel.TabIndex = 22;
            this.visualizeInjectedNameLabel.Text = "contentManager";
            // 
            // visualizeFieldNameLabel
            // 
            this.visualizeFieldNameLabel.AutoSize = true;
            this.visualizeFieldNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeFieldNameLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeFieldNameLabel.Location = new System.Drawing.Point(214, 24);
            this.visualizeFieldNameLabel.Name = "visualizeFieldNameLabel";
            this.visualizeFieldNameLabel.Size = new System.Drawing.Size(103, 13);
            this.visualizeFieldNameLabel.TabIndex = 21;
            this.visualizeFieldNameLabel.Text = "_contentManager;";
            // 
            // visualizeInjectedTypeLabel
            // 
            this.visualizeInjectedTypeLabel.AutoSize = true;
            this.visualizeInjectedTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeInjectedTypeLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeInjectedTypeLabel.ForeColor = System.Drawing.Color.DarkCyan;
            this.visualizeInjectedTypeLabel.Location = new System.Drawing.Point(92, 59);
            this.visualizeInjectedTypeLabel.Name = "visualizeInjectedTypeLabel";
            this.visualizeInjectedTypeLabel.Size = new System.Drawing.Size(97, 13);
            this.visualizeInjectedTypeLabel.TabIndex = 20;
            this.visualizeInjectedTypeLabel.Text = "IContentManager";
            // 
            // visualizeClassNameLabel
            // 
            this.visualizeClassNameLabel.AutoSize = true;
            this.visualizeClassNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeClassNameLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeClassNameLabel.Location = new System.Drawing.Point(58, 59);
            this.visualizeClassNameLabel.Name = "visualizeClassNameLabel";
            this.visualizeClassNameLabel.Size = new System.Drawing.Size(37, 13);
            this.visualizeClassNameLabel.TabIndex = 4;
            this.visualizeClassNameLabel.Text = "Test(";
            // 
            // visualizeConstructorPublicLabel
            // 
            this.visualizeConstructorPublicLabel.AutoSize = true;
            this.visualizeConstructorPublicLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeConstructorPublicLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeConstructorPublicLabel.ForeColor = System.Drawing.Color.Blue;
            this.visualizeConstructorPublicLabel.Location = new System.Drawing.Point(17, 59);
            this.visualizeConstructorPublicLabel.Name = "visualizeConstructorPublicLabel";
            this.visualizeConstructorPublicLabel.Size = new System.Drawing.Size(43, 13);
            this.visualizeConstructorPublicLabel.TabIndex = 3;
            this.visualizeConstructorPublicLabel.Text = "public";
            // 
            // visualizeFieldTypeLabel
            // 
            this.visualizeFieldTypeLabel.AutoSize = true;
            this.visualizeFieldTypeLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizeFieldTypeLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizeFieldTypeLabel.ForeColor = System.Drawing.Color.DarkCyan;
            this.visualizeFieldTypeLabel.Location = new System.Drawing.Point(119, 24);
            this.visualizeFieldTypeLabel.Name = "visualizeFieldTypeLabel";
            this.visualizeFieldTypeLabel.Size = new System.Drawing.Size(97, 13);
            this.visualizeFieldTypeLabel.TabIndex = 1;
            this.visualizeFieldTypeLabel.Text = "IContentManager";
            // 
            // visualizePrivateReadonlyTextLabel
            // 
            this.visualizePrivateReadonlyTextLabel.AutoSize = true;
            this.visualizePrivateReadonlyTextLabel.BackColor = System.Drawing.Color.Transparent;
            this.visualizePrivateReadonlyTextLabel.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visualizePrivateReadonlyTextLabel.ForeColor = System.Drawing.Color.Blue;
            this.visualizePrivateReadonlyTextLabel.Location = new System.Drawing.Point(17, 24);
            this.visualizePrivateReadonlyTextLabel.Name = "visualizePrivateReadonlyTextLabel";
            this.visualizePrivateReadonlyTextLabel.Size = new System.Drawing.Size(109, 13);
            this.visualizePrivateReadonlyTextLabel.TabIndex = 0;
            this.visualizePrivateReadonlyTextLabel.Text = "private readonly ";
            // 
            // InjectDependencyDialog
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(501, 403);
            this.Controls.Add(this.injectedDependencyAndFieldDataGroupBox);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.separatorLabel);
            this.Controls.Add(this.dependencyNameTextBox);
            this.Controls.Add(this.generateShortFieldNameCheckBox);
            this.Controls.Add(this.dependencyLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "InjectDependencyDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inject Dependency";
            this.injectedDependencyAndFieldDataGroupBox.ResumeLayout(false);
            this.injectedDependencyAndFieldDataGroupBox.PerformLayout();
            this.visualizePanel.ResumeLayout(false);
            this.visualizePanel.PerformLayout();
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
        private System.Windows.Forms.Panel visualizePanel;
        private System.Windows.Forms.Label visualizePrivateReadonlyTextLabel;
        private System.Windows.Forms.Label visualizeFieldNameLabel;
        private System.Windows.Forms.Label visualizeInjectedTypeLabel;
        private System.Windows.Forms.Label visualizeClassNameLabel;
        private System.Windows.Forms.Label visualizeConstructorPublicLabel;
        private System.Windows.Forms.Label visualizeFieldTypeLabel;
        private System.Windows.Forms.Label visualizeConstructorClosingParenthesisLabel;
        private System.Windows.Forms.Label visualizeInjectedNameLabel;
    }
}
