namespace CofrinhoDigital.UI
{
    partial class FrmCofrinho
    {
        private System.ComponentModel.IContainer? components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelTitulo = new Label();
            lblSaldoTitulo = new Label();
            lblSaldoValor = new Label();
            lblLimite = new Label();
            labelValor = new Label();
            txtValor = new TextBox();
            btnDepositar = new Button();
            btnSacar = new Button();
            lblMensagem = new Label();
            lblHistorico = new Label();
            lstHistorico = new ListBox();
            SuspendLayout();
            //
            // labelTitulo
            //
            labelTitulo.AutoSize = true;
            labelTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            labelTitulo.Location = new Point(110, 20);
            labelTitulo.Name = "labelTitulo";
            labelTitulo.Size = new Size(180, 30);
            labelTitulo.TabIndex = 0;
            labelTitulo.Text = "Cofrinho Digital";
            //
            // lblSaldoTitulo
            //
            lblSaldoTitulo.AutoSize = true;
            lblSaldoTitulo.Location = new Point(50, 70);
            lblSaldoTitulo.Name = "lblSaldoTitulo";
            lblSaldoTitulo.Size = new Size(71, 15);
            lblSaldoTitulo.TabIndex = 1;
            lblSaldoTitulo.Text = "Saldo Atual:";
            //
            // lblSaldoValor
            //
            lblSaldoValor.AutoSize = true;
            lblSaldoValor.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSaldoValor.ForeColor = Color.DarkBlue;
            lblSaldoValor.Location = new Point(130, 65);
            lblSaldoValor.Name = "lblSaldoValor";
            lblSaldoValor.Size = new Size(66, 21);
            lblSaldoValor.TabIndex = 2;
            lblSaldoValor.Text = "R$ 0,00";
            //
            // lblLimite
            //
            lblLimite.AutoSize = true;
            lblLimite.ForeColor = Color.DimGray;
            lblLimite.Location = new Point(50, 94);
            lblLimite.Name = "lblLimite";
            lblLimite.Size = new Size(200, 15);
            lblLimite.TabIndex = 3;
            lblLimite.Text = "Limite de saque restante hoje:";
            //
            // labelValor
            //
            labelValor.AutoSize = true;
            labelValor.Location = new Point(50, 128);
            labelValor.Name = "labelValor";
            labelValor.Size = new Size(36, 15);
            labelValor.TabIndex = 4;
            labelValor.Text = "Valor:";
            //
            // txtValor
            //
            txtValor.Location = new Point(130, 125);
            txtValor.Name = "txtValor";
            txtValor.Size = new Size(120, 23);
            txtValor.TabIndex = 5;
            //
            // btnDepositar
            //
            btnDepositar.BackColor = Color.LightGreen;
            btnDepositar.Location = new Point(50, 165);
            btnDepositar.Name = "btnDepositar";
            btnDepositar.Size = new Size(90, 30);
            btnDepositar.TabIndex = 6;
            btnDepositar.Text = "Depositar";
            btnDepositar.UseVisualStyleBackColor = false;
            btnDepositar.Click += btnDepositar_Click;
            //
            // btnSacar
            //
            btnSacar.BackColor = Color.LightCoral;
            btnSacar.Location = new Point(160, 165);
            btnSacar.Name = "btnSacar";
            btnSacar.Size = new Size(90, 30);
            btnSacar.TabIndex = 7;
            btnSacar.Text = "Sacar";
            btnSacar.UseVisualStyleBackColor = false;
            btnSacar.Click += btnSacar_Click;
            //
            // lblMensagem
            //
            lblMensagem.Location = new Point(50, 210);
            lblMensagem.Name = "lblMensagem";
            lblMensagem.Size = new Size(300, 40);
            lblMensagem.TabIndex = 8;
            lblMensagem.TextAlign = ContentAlignment.TopCenter;
            //
            // lblHistorico
            //
            lblHistorico.AutoSize = true;
            lblHistorico.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblHistorico.Location = new Point(50, 258);
            lblHistorico.Name = "lblHistorico";
            lblHistorico.Size = new Size(58, 15);
            lblHistorico.TabIndex = 9;
            lblHistorico.Text = "Histórico";
            //
            // lstHistorico
            //
            lstHistorico.FormattingEnabled = true;
            lstHistorico.ItemHeight = 15;
            lstHistorico.Location = new Point(50, 278);
            lstHistorico.Name = "lstHistorico";
            lstHistorico.Size = new Size(300, 154);
            lstHistorico.TabIndex = 10;
            lstHistorico.TabStop = false;
            //
            // FrmCofrinho
            //
            ClientSize = new Size(400, 455);
            Controls.Add(labelTitulo);
            Controls.Add(lblSaldoTitulo);
            Controls.Add(lblSaldoValor);
            Controls.Add(lblLimite);
            Controls.Add(labelValor);
            Controls.Add(txtValor);
            Controls.Add(btnDepositar);
            Controls.Add(btnSacar);
            Controls.Add(lblMensagem);
            Controls.Add(lblHistorico);
            Controls.Add(lstHistorico);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FrmCofrinho";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Cofrinho Digital";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelTitulo;
        private Label lblSaldoTitulo;
        private Label lblSaldoValor;
        private Label lblLimite;
        private Label labelValor;
        private TextBox txtValor;
        private Button btnDepositar;
        private Button btnSacar;
        private Label lblMensagem;
        private Label lblHistorico;
        private ListBox lstHistorico;
    }
}
