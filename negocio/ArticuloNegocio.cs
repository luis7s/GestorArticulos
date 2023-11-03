using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo>listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("Select Codigo,Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion Categoria,M.Descripcion Marca,A.Id,M.Id IdMarca,C.Id IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id and A.IdCategoria = C.Id");
                datos.ejecutarLectura();
                while(datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    }
                    
                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally 
            {
                datos.cerrarConexion();
            }
            
        }
        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            string consulta;
            try
            {
                consulta = "Select Codigo,Nombre, A.Descripcion, ImagenUrl, Precio, C.Descripcion Categoria,M.Descripcion Marca,A.Id,M.Id IdMarca,C.Id IdCategoria from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdMarca = M.Id and A.IdCategoria = C.Id and ";
                switch (campo)
                {
                    case "Código artículo":

                        switch (criterio)
                        {
                            case "Comienza con":

                                consulta += "Codigo like '" + filtro + "%'";

                            break;
                            case "Termina con":

                                consulta += "Codigo like '%" + filtro + "'";

                            break;
                            default:

                                consulta += "Codigo like '%" + filtro + "%'";

                            break;
                        }

                    break;
                    case "Marca":

                        switch (criterio)
                        {
                            case "Comienza con":

                                consulta += "M.Descripcion like '" + filtro + "%'";

                                break;
                            case "Termina con":

                                consulta += "M.Descripcion like '%" + filtro + "'";

                                break;
                            default:

                                consulta += "M.Descripcion like '%" + filtro + "%'";

                            break;
                        }

                    break;
                    case "Categoría":

                        switch (criterio)
                        {
                            case "Comienza con":

                                consulta += "C.Descripcion like '" + filtro + "%'";

                                break;
                            case "Termina con":

                                consulta += "C.Descripcion like '%" + filtro + "'";

                                break;
                            default:

                                consulta += "C.Descripcion like '%" + filtro + "%'";

                            break;
                        }
                    break;

                    case "Precio":

                        switch (criterio)
                        {
                            case "Igual a":

                                consulta += "Precio = " + filtro;

                                break;
                            case "Mayor a":

                                consulta += "Precio > " + filtro;

                                break;

                            default:

                                consulta += "Precio < " + filtro;

                                break;
                        }

                    break;

                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.CodigoArticulo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                    {
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    }

                    aux.Precio = (decimal)datos.Lector["Precio"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];


                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
