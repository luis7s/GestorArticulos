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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Configuration;
using System.IO;

namespace Winform
{
    public partial class Form2 : Form
    {
        private Articulo articulo = null;
        private OpenFileDialog archivo = null;
        public Form2()
        {
            InitializeComponent();
        }
        public Form2(Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar Artículo";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                if (articulo == null)
                {
                    articulo = new Articulo();
                }
                articulo.CodigoArticulo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.UrlImagen = txtUrlImagen.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txtPrecio.Text);

                if(txtCodigo.BackColor != Color.Red)
                {              

                    if (articulo.Id != 0)
                    { 
                        negocio.modificar(articulo);
                        guardarImagen();
                        Close();
                    }
                    else
                    {
                        negocio.agregar(articulo);
                        guardarImagen();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("Ingrese datos obligatorios");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
             MarcaNegocio negocioMarca = new MarcaNegocio();
            CategoriaNegocio negocioCategoria = new CategoriaNegocio();

            try
            {
                cboMarca.DataSource = negocioMarca.listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";
                cboCategoria.DataSource = negocioCategoria.listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                if(articulo != null)
                {
                    txtCodigo.Text = articulo.CodigoArticulo;
                    txtNombre.Text = articulo.Nombre;
                    txtDescripcion.Text = articulo.Descripcion;
                    txtUrlImagen.Text = articulo.UrlImagen;
                    cargarImagen(articulo.UrlImagen);
                    txtPrecio.Text = articulo.Precio.ToString("0.00");
                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


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

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }
        
        private void txtPrecio_Leave(object sender, EventArgs e)
        {

            validarPrecio();
        }
        public void validarPrecio()
        {

            if (!decimal.TryParse(txtPrecio.Text, out decimal result))
            {

                txtPrecio.BackColor = Color.Red;
                MessageBox.Show("Ingresar numero decimal");
                txtPrecio.Text = string.Empty;

            }
            else
            {
                txtPrecio.BackColor = Color.White;
            }
        }
        
        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            if(txtCodigo.Text == "")
            {
                txtCodigo.BackColor = Color.Red;

            }
            else
            {
                txtCodigo.BackColor = Color.White;
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            archivo = new OpenFileDialog();
            archivo.Filter = "png|*.png|jpg|*.jpg";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txtUrlImagen.Text = archivo.FileName;
                cargarImagen(archivo.FileName);
            }

        }
        private void guardarImagen()
        {
            if (archivo != null && !(txtUrlImagen.Text.ToUpper().Contains("HTTP")))
            {
 
              File.Copy(archivo.FileName, ConfigurationManager.AppSettings["imagenes"] + archivo.SafeFileName);
      
                
            }
                
        }
    }
}
