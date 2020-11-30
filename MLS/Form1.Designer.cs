
namespace MLS
{
    partial class Form1
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
            this.musicPlayerListBox = new System.Windows.Forms.ListBox();
            this.selectorButton = new System.Windows.Forms.Button();
            this.playlistsListBox = new System.Windows.Forms.ListBox();
            this.syncButton = new System.Windows.Forms.Button();
            this.backToLoginBt = new System.Windows.Forms.Button();
            this.backToListsBt = new System.Windows.Forms.Button();
            this.tipLabel = new System.Windows.Forms.Label();
            this.songConflictsListBox = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.conflictResolverListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // musicPlayerListBox
            // 
            this.musicPlayerListBox.BackColor = System.Drawing.Color.Black;
            this.musicPlayerListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.musicPlayerListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.musicPlayerListBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.musicPlayerListBox.FormattingEnabled = true;
            this.musicPlayerListBox.ItemHeight = 20;
            this.musicPlayerListBox.Location = new System.Drawing.Point(12, 12);
            this.musicPlayerListBox.MaximumSize = new System.Drawing.Size(240, 320);
            this.musicPlayerListBox.MinimumSize = new System.Drawing.Size(120, 160);
            this.musicPlayerListBox.Name = "musicPlayerListBox";
            this.musicPlayerListBox.Size = new System.Drawing.Size(240, 320);
            this.musicPlayerListBox.TabIndex = 0;
            // 
            // selectorButton
            // 
            this.selectorButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.selectorButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.selectorButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectorButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.selectorButton.Location = new System.Drawing.Point(172, 386);
            this.selectorButton.MaximumSize = new System.Drawing.Size(120, 50);
            this.selectorButton.MinimumSize = new System.Drawing.Size(60, 25);
            this.selectorButton.Name = "selectorButton";
            this.selectorButton.Size = new System.Drawing.Size(80, 25);
            this.selectorButton.TabIndex = 1;
            this.selectorButton.Text = "Go to Login";
            this.selectorButton.UseVisualStyleBackColor = false;
            this.selectorButton.Click += new System.EventHandler(this.selectorButton_Click);
            // 
            // playlistsListBox
            // 
            this.playlistsListBox.BackColor = System.Drawing.Color.Black;
            this.playlistsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.playlistsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.playlistsListBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.playlistsListBox.FormattingEnabled = true;
            this.playlistsListBox.ItemHeight = 20;
            this.playlistsListBox.Location = new System.Drawing.Point(12, 12);
            this.playlistsListBox.MaximumSize = new System.Drawing.Size(240, 320);
            this.playlistsListBox.MinimumSize = new System.Drawing.Size(120, 160);
            this.playlistsListBox.Name = "playlistsListBox";
            this.playlistsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.playlistsListBox.Size = new System.Drawing.Size(240, 320);
            this.playlistsListBox.TabIndex = 2;
            // 
            // syncButton
            // 
            this.syncButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.syncButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.syncButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.syncButton.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.syncButton.Location = new System.Drawing.Point(172, 386);
            this.syncButton.MaximumSize = new System.Drawing.Size(120, 50);
            this.syncButton.MinimumSize = new System.Drawing.Size(60, 25);
            this.syncButton.Name = "syncButton";
            this.syncButton.Size = new System.Drawing.Size(80, 25);
            this.syncButton.TabIndex = 3;
            this.syncButton.Text = "Sync Lists";
            this.syncButton.UseVisualStyleBackColor = false;
            this.syncButton.Click += new System.EventHandler(this.syncButton_Click);
            // 
            // backToLoginBt
            // 
            this.backToLoginBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.backToLoginBt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.backToLoginBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backToLoginBt.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.backToLoginBt.Location = new System.Drawing.Point(12, 386);
            this.backToLoginBt.MaximumSize = new System.Drawing.Size(120, 50);
            this.backToLoginBt.MinimumSize = new System.Drawing.Size(60, 25);
            this.backToLoginBt.Name = "backToLoginBt";
            this.backToLoginBt.Size = new System.Drawing.Size(80, 25);
            this.backToLoginBt.TabIndex = 4;
            this.backToLoginBt.Text = "Log Out";
            this.backToLoginBt.UseVisualStyleBackColor = false;
            this.backToLoginBt.Click += new System.EventHandler(this.backToLoginBt_Click);
            // 
            // backToListsBt
            // 
            this.backToListsBt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.backToListsBt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.backToListsBt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backToListsBt.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.backToListsBt.Location = new System.Drawing.Point(12, 386);
            this.backToListsBt.MaximumSize = new System.Drawing.Size(120, 50);
            this.backToListsBt.MinimumSize = new System.Drawing.Size(60, 25);
            this.backToListsBt.Name = "backToListsBt";
            this.backToListsBt.Size = new System.Drawing.Size(80, 25);
            this.backToListsBt.TabIndex = 5;
            this.backToListsBt.Text = "Back to Lists";
            this.backToListsBt.UseVisualStyleBackColor = false;
            this.backToListsBt.Click += new System.EventHandler(this.backToListsBt_Click);
            // 
            // tipLabel
            // 
            this.tipLabel.AutoEllipsis = true;
            this.tipLabel.AutoSize = true;
            this.tipLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.tipLabel.Location = new System.Drawing.Point(12, 339);
            this.tipLabel.MaximumSize = new System.Drawing.Size(240, 0);
            this.tipLabel.MinimumSize = new System.Drawing.Size(120, 0);
            this.tipLabel.Name = "tipLabel";
            this.tipLabel.Size = new System.Drawing.Size(178, 13);
            this.tipLabel.TabIndex = 6;
            this.tipLabel.Text = "Select music service to synchronize.";
            // 
            // songConflictsListBox
            // 
            this.songConflictsListBox.BackColor = System.Drawing.Color.Black;
            this.songConflictsListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.songConflictsListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.songConflictsListBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.songConflictsListBox.FormattingEnabled = true;
            this.songConflictsListBox.ItemHeight = 20;
            this.songConflictsListBox.Location = new System.Drawing.Point(12, 12);
            this.songConflictsListBox.MaximumSize = new System.Drawing.Size(240, 320);
            this.songConflictsListBox.MinimumSize = new System.Drawing.Size(120, 160);
            this.songConflictsListBox.Name = "songConflictsListBox";
            this.songConflictsListBox.Size = new System.Drawing.Size(240, 320);
            this.songConflictsListBox.TabIndex = 7;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 355);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(240, 23);
            this.progressBar1.TabIndex = 8;
            this.progressBar1.Visible = false;
            // 
            // conflictResolverListBox
            // 
            this.conflictResolverListBox.BackColor = System.Drawing.Color.Black;
            this.conflictResolverListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.conflictResolverListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.conflictResolverListBox.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.conflictResolverListBox.FormattingEnabled = true;
            this.conflictResolverListBox.ItemHeight = 20;
            this.conflictResolverListBox.Location = new System.Drawing.Point(12, 12);
            this.conflictResolverListBox.MaximumSize = new System.Drawing.Size(240, 320);
            this.conflictResolverListBox.MinimumSize = new System.Drawing.Size(120, 160);
            this.conflictResolverListBox.Name = "conflictResolverListBox";
            this.conflictResolverListBox.Size = new System.Drawing.Size(240, 320);
            this.conflictResolverListBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(18)))), ((int)(((byte)(18)))));
            this.ClientSize = new System.Drawing.Size(260, 423);
            this.Controls.Add(this.conflictResolverListBox);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.songConflictsListBox);
            this.Controls.Add(this.tipLabel);
            this.Controls.Add(this.musicPlayerListBox);
            this.Controls.Add(this.playlistsListBox);
            this.Controls.Add(this.syncButton);
            this.Controls.Add(this.backToLoginBt);
            this.Controls.Add(this.selectorButton);
            this.Controls.Add(this.backToListsBt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox musicPlayerListBox;
        private System.Windows.Forms.Button selectorButton;
        private System.Windows.Forms.ListBox playlistsListBox;
        private System.Windows.Forms.Button syncButton;
        private System.Windows.Forms.Button backToLoginBt;
        private System.Windows.Forms.Button backToListsBt;
        private System.Windows.Forms.Label tipLabel;
        private System.Windows.Forms.ListBox songConflictsListBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ListBox conflictResolverListBox;
    }
}

