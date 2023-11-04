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
                verDetalle(seleccionado);

            }

        }

        private void verDetalle(Articulo seleccionado)
        {
            try
            {

                lblCodigo.Text = "Codigo: " + seleccionado.CodigoArticulo;
                lblNombre.Text = seleccionado.Nombre;
                lblDescripcion.Text = seleccionado.Descripcion;
                lblMarca.Text = "Marca: " + seleccionado.Marca.Descripcion.ToString();
                lblPrecio.Text = "$ " + seleccionado.Precio.ToString("0.00");

                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void NoVerDetalles()
        {
            try
            {
                lblCodigo.Text = "";
                lblNombre.Text = "";
                lblDescripcion.Text = "";
                lblMarca.Text = "";
                lblPrecio.Text = "";
                ptbImagen.Load("https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBQaH0iL3i2SDCvG3iUUbh6BrYFagdbZ1varZBiLzx34AzdukVCcI-cA4HgZXC7wuD7is&usqp=CAU");
            }
            catch (Exception ex)
            {

                throw ex; 
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
                if(validarFiltro())
                {
                    return;
                }
                campo = cboCampo.SelectedItem.ToString();
                criterio = cboCriterio.SelectedItem.ToString();
                filtro = txtFiltro.Text;
                dgvArticulos.DataSource = negocio.filtrar(campo, criterio, filtro);

                if (dgvArticulos.RowCount == 0)
                {

                    NoVerDetalles();

                    MessageBox.Show("No se encontraron resultados");


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Elija un campo para filtrar");
                return true;
            }
            if(cboCriterio.SelectedIndex <0)
            {
                MessageBox.Show("Elija un criterio para filtrar");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
                if(string.IsNullOrEmpty(txtFiltro.Text))
                {
                    MessageBox.Show("Escriba en el filtro");
                    return true;
                }
                if(!(SoloNumeros(txtFiltro.Text)))
                {
                    MessageBox.Show("Solo Número en campo precio");
                    return true;
                }
            }
            return false;
        }

        private bool SoloNumeros(string cadena)
        {
            foreach (char caracter  in cadena)
            {
                if(!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            cargar();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Form2 aux = new Form2();
            aux.ShowDialog();
            cargar();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccionado;
            try
            {
                seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                Form2 aux = new Form2(seleccionado);
                aux.ShowDialog();
                cargar();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo seleccionado;
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Desea eliminar?","Eliminar",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if(respuesta == DialogResult.Yes)
                {
                    seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                    negocio.eliminar(seleccionado.Id);
                    cargar();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }


    }
}
