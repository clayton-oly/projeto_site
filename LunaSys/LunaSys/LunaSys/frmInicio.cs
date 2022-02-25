﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace WindowsFormsApp1
{
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        public int IdUsuario { get; set; }

        public static string usuarioConectado;
        public static string usuarioConectadoNome;
        public static DateTime dataAtual;




        int lx, ly;
        int sw, sh;
        //METODO PARA ARRASTAR FORMULARIO---------------------------------------------------------------------
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void btnListaClientes_Click(object sender, EventArgs e)
        {
            frmGridClientes fm = new frmGridClientes();
            fm.FormClosed += new FormClosedEventHandler(MostrarFormLogoAlCerrarForms);
            AbrirFormEnPanel(fm);
        }

        //METODO PARA ABRIR FORM DENTRO DO PANEL-----------------------------------------------------
        private void AbrirFormEnPanel(object formHijo)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            Form fh = formHijo as Form;
            fh.TopLevel = false;
            fh.FormBorderStyle = FormBorderStyle.None;
            fh.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();
        }
        //METODO PARA MOSTRAR FORMULARIO DE LOGO AO INICIAR ----------------------------------------------------------
        private void MostrarFormLogo()
        {
            AbrirFormEnPanel(new WindowsFormsApp1.frmLogo());
        }
        private void MostrarFormLogoAlCerrarForms(object sender, FormClosedEventArgs e)
        {
            MostrarFormLogo();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            //-------COM EFEITO SLIDE
            if (panelMenu.Width == 230)
            {
                this.tmOcultarMenu.Enabled = true;
            }
            else if (panelMenu.Width == 55)
                this.tmMostrarMenu.Enabled = true;
        }

        private void btnNormal_Click(object sender, EventArgs e)
        {
            this.Size = new Size(sw, sh);
            this.Location = new Point(lx, ly);
            btnNormal.Visible = false;
            btnMaximizar.Visible = true;
        }


        private void BtnSair_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Sair?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void tmFechaHora_Tick(object sender, EventArgs e)
        {
            if (panelMenu.Width >= 230)
                this.tmMostrarMenu.Enabled = false;
            else
                panelMenu.Width = panelMenu.Width + 35;
        }

        private void tmMostrarMenu_Tick(object sender, EventArgs e)
        {
            if (panelMenu.Width <= 55)
                this.tmOcultarMenu.Enabled = false;
            else
                panelMenu.Width = panelMenu.Width - 35;
        }

        private void tmOcultarMenu_Tick(object sender, EventArgs e)
        {
            lbFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToString("HH:mm:ssss");
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {
            MostrarFormLogo();

            CURSOEntitiesCasa banco = new CURSOEntitiesCasa();
            //Consultei o usuario logado

            Usuario user = banco.Usuarios.FirstOrDefault(x => x.ID_Usuario == IdUsuario);

            lbLogin.Text = user.Login;
            lbNome.Text = user.Nome;
            lbCargo.Text = user.Funcao;
            try
            {
                System.IO.MemoryStream stream =
                    new System.IO.MemoryStream(user.photo);
                picFoto.Image = Bitmap.FromStream(stream);

            }
            catch (Exception)
            {

               
            }
           

            usuarioConectado = lbNome.Text;

            //exibe mensagem de boas vindas
            //MessageBox.Show("Olá " + user.Nome);

            ////se o usuário existe mostra o botao
            //if (user.Nivel.Value)
            //{
            //    //  usuarioToolStripMenuItem.Visible = false;
            //}
            //else
            //{
            //    // usuarioToolStripMenuItem.Visible = true;
            //}
        }

        private void btnEncerrar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Deseja Sair?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnProduto_Click(object sender, EventArgs e)
        {
            frmGridProdutos fm = new frmGridProdutos();
            fm.FormClosed += new FormClosedEventHandler(MostrarFormLogoAlCerrarForms);
            AbrirFormEnPanel(fm);
        }


        private void btnVendas_Click(object sender, EventArgs e)
        {
            frmTelaVenda fm = new frmTelaVenda();
            fm.FormClosed += new FormClosedEventHandler(MostrarFormLogoAlCerrarForms);
            AbrirFormEnPanel(fm);
        }


        private void BarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            lx = this.Location.X;
            ly = this.Location.Y;
            sw = this.Size.Width;
            sh = this.Size.Height;
            this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            btnMaximizar.Visible = false;
            btnNormal.Visible = true;
           
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //METODO PARA REDIMENSIONAR/TAMANHO DO FORMULARIO  TEMPO DE EXECUCAO ----------------------------------------------------------
        private int tolerance = 15;
        private const int WM_NCHITTEST = 132;
        private const int HTBOTTOMRIGHT = 17;
        private Rectangle sizeGripRectangle;

        //protected override void WndProc(ref Message m)
        //{
        //    switch (m.Msg)
        //    {
        //        case WM_NCHITTEST:
        //            base.WndProc(ref m);
        //            var hitPoint = this.PointToClient(new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16));
        //            if (sizeGripRectangle.Contains(hitPoint))
        //                m.Result = new IntPtr(HTBOTTOMRIGHT);
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}

        private void tmMostrarMenu_Tick_1(object sender, EventArgs e)
        {
            if (panelMenu.Width >= 230)
                this.tmMostrarMenu.Enabled = false;
            else
                panelMenu.Width = panelMenu.Width + 35;
        }

        private void tmOcultarMenu_Tick_1(object sender, EventArgs e)
        {
            if (panelMenu.Width <= 55)
                this.tmOcultarMenu.Enabled = false;
            else
                panelMenu.Width = panelMenu.Width - 35;
        }

        private void tmFechaHora_Tick_1(object sender, EventArgs e)
        {
            lbFecha.Text = DateTime.Now.ToLongDateString();
            lblHora.Text = DateTime.Now.ToString("HH:mm:ssss");
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            frmGridUsuarios fm = new frmGridUsuarios();
            fm.FormClosed += new FormClosedEventHandler(MostrarFormLogoAlCerrarForms);
            AbrirFormEnPanel(fm);
        }

        private void btnFornecedores_Click(object sender, EventArgs e)
        {
            frmGridFornecedor fm = new frmGridFornecedor();
            fm.FormClosed += new FormClosedEventHandler(MostrarFormLogoAlCerrarForms);
            AbrirFormEnPanel(fm);

        }

        private void btnRelaVenda_Click(object sender, EventArgs e)
        {
            frmGridVendas fm = new frmGridVendas();
            fm.FormClosed += new FormClosedEventHandler(MostrarFormLogoAlCerrarForms);
            AbrirFormEnPanel(fm);
        }

        //----------------ESQUINA DO FORM / EXCLUIR ESQUINA PANEL 
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            var region = new Region(new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height));

            sizeGripRectangle = new Rectangle(this.ClientRectangle.Width - tolerance, this.ClientRectangle.Height - tolerance, tolerance, tolerance);

            region.Exclude(sizeGripRectangle);
            this.panel1ContenedorPrincipal.Region = region;
            this.Invalidate();
        }

        //----------------COR  RETANGULO INFERIOR
        protected override void OnPaint(PaintEventArgs e)
        {

            SolidBrush blueBrush = new SolidBrush(Color.FromArgb(55, 61, 69));
            e.Graphics.FillRectangle(blueBrush, sizeGripRectangle);

            base.OnPaint(e);
            ControlPaint.DrawSizeGrip(e.Graphics, Color.Transparent, sizeGripRectangle);
        }
    }
}
