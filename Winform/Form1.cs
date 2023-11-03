using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winform
{
    public partial class Form1 : Form
    {
        private List<Articulo> listaArticulo;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cargar();
            cboCampo.Items.Add("Código artículo");
            cboCampo.Items.Add("Marca");
            cboCampo.Items.Add("Categoría");
            cboCampo.Items.Add("Precio");
        }
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                listaArticulo = negocio.listar();
                dgvArticulos.DataSource = listaArticulo;
                ocultarColumnas();
                cargarImagen(listaArticulo[0].UrlImagen);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        private void ocultarColumnas()
        {
            dgvArticulos.Columns["UrlImagen"].Visible = false;
            dgvArticulos.Columns["Id"].Visible = false;
        }
        private void cargarImagen(string imagen)
        {
            try
            {
                ptbImagen.Load(imagen);
            }
            catch (Exception)
            {
                ptbImagen.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBQaH0iL3i2SDCvG3iUUbh6BrYFagdbZ1varZBiLzx34AzdukVCcI-cA4HgZXC7wuD7is&usqp=CAU");
            }
        }

        private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            if(dgvArticulos.CurrentRow != null) 
            {
                Articulo seleccionado;
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                cargarImagen(seleccionado.UrlImagen);
            }
        }

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion;

            opcion = cboCampo.SelectedItem.ToString();
            if(opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Igual a");
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
            }
            else 
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }

        private void btnFiltrar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            string campo;
            string criterio;
            string filtro;
            try
            {
                campo = cboCampo.SelectedItem.ToString();
                criterio = cboCriterio.SelectedItem.ToString();
                filtro = txtFiltro.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
