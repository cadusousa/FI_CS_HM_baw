using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Reports_CompraVenta : System.Web.UI.Page
{
    UsuarioBean user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            Response.Redirect("../default.aspx");
        }
        #region Definir Fechas
        DateTime Fecha = DateTime.Now;
        if (!IsPostBack)
        {
            //tb_fechaini.Text =  Fecha.Month.ToString()  +"/01/" + Fecha.Year.ToString();
            //tb_fechafin.Text = DateTime.Now.ToString("MM/dd/yyyy");
        }
        #endregion
        user = (UsuarioBean)Session["usuario"];
        
        ArrayList arr = null;
        if (!Page.IsPostBack)
        {
            obtengo_lista();
            //arr = null;
            //arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
            //foreach (RE_GenericBean rgb in arr)
            //{
            //    item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            //    lb_moneda.Items.Add(item);
            //}
            //************************RESTRICCION DE CONTABILIDAD USUARIOS****************************//
            lb_contabilidad.Items.Clear();
            int bandera = 0; // verifica si el usuario tiener contabilidad consolidada.
            int fiscal = 0;
            int financiera = 0;
            ListItem item = null;
            ArrayList arruser = (ArrayList)DB.getContaUsuario(user.ID, user.PaisID);
            ArrayList arrbloqueo = (ArrayList)DB.getContaPais(user.PaisID);
            foreach (RE_GenericBean rgbp in arrbloqueo)
            {
                if (rgbp.intC1 == 1)
                {
                    fiscal = 1; //desbloqueo fiscal
                }

                if (rgbp.intC2 == 1)
                {
                    financiera = 0; // desbloqueo financiera.
                }
            }
            foreach (RE_GenericBean rgb in arruser)
            {
                if ((rgb.intC1 == 1) && (fiscal == 1))
                {
                    bandera++;
                    item = new ListItem("FISCAL", "1");
                    lb_contabilidad.Items.Add(item);
                }

                if ((rgb.intC2 == 1) && (financiera == 1))
                {
                    bandera++;
                    item = new ListItem("FINANCIERA", "2");
                    lb_contabilidad.Items.Add(item);
                }
            }
            if (bandera == 2)
            {
                //item = new ListItem("CONSOLIDADO", "3");
                //lb_contabilidad.Items.Add(item);
            }
            if (bandera == 1)
            {
                lb_contabilidad.Visible = false;
                l_contabilidad.Visible = false;
            }
            else
            {
                lb_contabilidad.Visible = true;
                l_contabilidad.Visible = true;
            }

            //*********************************FIN RESTRICCION********************************************//

            arr = null;
            //arr = (ArrayList)DB.getEmpresasbyPais(user.PaisID)
            arr = (ArrayList)DB.getSucursales_pais(" and suc_pai_id=" + user.PaisID);
            item = new ListItem("Todas", "0");
            lb_empresa.Items.Add(item);
            foreach (SucursalBean rgb in arr)
            {
                item = new ListItem(rgb.Nombre, rgb.ID.ToString());
                lb_empresa.Items.Add(item);
            }
        }
    }
    protected void obtengo_lista()
    {
        ArrayList arr = null;
        ListItem item = null;
        user = (UsuarioBean)Session["usuario"];
        arr = (ArrayList)DB.getMonedasbyPais(user.PaisID, user.contaID);
        foreach (RE_GenericBean rgb in arr)
        {
            item = new ListItem(rgb.strC1, rgb.intC1.ToString());
            lb_moneda.Items.Add(item);
        }
    }
    
    protected void bt_generar_Click(object sender, EventArgs e)
    {
        int h = 0;
        h = int.Parse(DateTime.Now.Hour.ToString());
        StreamWriter sw;
        string Estado_Doc = "";
        string documentos = "";
        int ban_Documentos = 0;
        string fechaini = tb_fechaini.Text;
        string fechafin = tb_fechafin.Text;
        int monID = int.Parse(lb_moneda.SelectedValue);
        string empresaID = lb_empresa.SelectedValue;
        string moneda = Utility.TraducirMonedaInt(monID);
        string tipo_reprote = lb_reporte.SelectedValue; //1 compras -- 2 ventas
        string nombre_rep = "";
        if (tipo_reprote == "1") nombre_rep = "Compras";
        if (tipo_reprote == "2") nombre_rep = "Ventas";
        if (fechafin == "")
        {
            WebMsgBox.Show("Debe seleccionar un rango de fechas");
            return;
        }
        if (fechaini == "")
        {
            WebMsgBox.Show("Debe seleccionar un rango de fechas");
            return;
        }
        #region Formatear Fechas
        //Fecha Inicio
        int fe_dia = int.Parse(fechaini.Substring(3, 2));
        int fe_mes = int.Parse(fechaini.Substring(0, 2));
        int fe_anio = int.Parse(fechaini.Substring(6, 4));
        fechaini = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fechaini += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fechaini += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fechaini += "0" + fe_dia.ToString();
        }
        else
        {
            fechaini += fe_dia.ToString();
        }
        //Fecha Fin
        fe_dia = int.Parse(fechafin.Substring(3, 2));
        fe_mes = int.Parse(fechafin.Substring(0, 2));
        fe_anio = int.Parse(fechafin.Substring(6, 4));
        fechafin = fe_anio.ToString() + "/";
        if (fe_mes < 10)
        {
            fechafin += "0" + fe_mes.ToString() + "/";
        }
        else
        {
            fechafin += fe_mes.ToString() + "/";
        }
        if (fe_dia < 10)
        {
            fechafin += "0" + fe_dia.ToString();
        }
        else
        {
            fechafin += fe_dia.ToString();
        }
        #endregion
        for (int i = 0; i < CB_Estados.Items.Count; i++)
        {
            if (CB_Estados.Items[i].Selected == true)
            {
                documentos += CB_Estados.Items[i].Value + ",";
                ban_Documentos++;
            }
        }
        if (ban_Documentos == 0)
        {
            WebMsgBox.Show("Debe seleccionar el estado de los Documentos");
            return;
        }
        int cantidad = documentos.Length;
        if (ban_Documentos >=  1)
        {
            documentos = documentos.Substring(0, cantidad - 1);
        }
        LibroDiarioDS comprasDS = null;
        LibroDiarioDS ventasDS = null;
        LibroDiarioDS AsisteDS = null;
        int tcon = int.Parse(Session["Contabilidad"].ToString());
        user = (UsuarioBean)Session["usuario"];
        if (tipo_reprote == "1")
        {
            comprasDS = DB.GetAsisteLibros(user, fechaini, fechafin, empresaID, monID, tipo_reprote, documentos, "1");// 1 porque solo se pueden generer tipo fiscal
        }
        else if (tipo_reprote == "2")
        {
            ventasDS = DB.GetAsisteLibros(user, fechaini, fechafin, empresaID, monID, tipo_reprote, documentos, "1");// 1 porque solo se pueden generer tipo fiscal
        }
        else if (tipo_reprote == "3")
        {
            AsisteDS = DB.GetAsisteLibros(user, fechaini, fechafin, empresaID, monID, tipo_reprote, documentos, "1");// 1 porque solo se pueden generer tipo fiscal
        }
        DateTime Fecha = DateTime.Now;
        string nit = "12105562";
        string NoEstablecimiento = empresaID;
        string ano = fechaini.Substring(0, 4);
        string mes = fechaini.Substring(5,2);
        string filename = "Asistelibro_"+nombre_rep+Fecha.Millisecond+Fecha.Second+Fecha.Minute+Fecha.Hour+Fecha.Day+Fecha.Month+Fecha.Year+tipo_reprote+".txt";
        string path = "D:\\BAWReportAsisteLibros\\" + filename;
        int exbien = 0;  
        if (comprasDS != null)//compras
        {
            sw = new StreamWriter(path, true);
            //sw.WriteLine(nit + "|" + NoEstablecimiento + "|"+ mes +"|"+ ano );
            //sw.Flush();
            foreach (DataRow dr in comprasDS.Tables["asistecompras"].Rows)
            {
                exbien = int.Parse(dr[8].ToString());
                if (int.Parse(dr[11].ToString()) == 3)
                {
                    Estado_Doc = "A";
                }
                else
                {
                    Estado_Doc = "E";
                }
                if (exbien == 1) // bien
                {
                    if (int.Parse(dr[13].ToString()) == 1) //excento
                    {
                        sw.WriteLine("1" + "|" + "C" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "|" + "BIEN " + "  |" + Estado_Doc + "|| | ||0|0|0|0|" + dr[5] + "|0|");
                    }
                    else if (int.Parse(dr[13].ToString()) == 2) //contribuyente
                    {
                        sw.WriteLine("1" + "|" + "C" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "|" + "BIEN " + "  |" + Estado_Doc + "|| | ||" + dr[5] + "|0|0|0|0|0|");
                    }
                }
                else if (exbien == 2) //servicio
                {
                    if (int.Parse(dr[13].ToString()) == 1) //excento
                    {
                        sw.WriteLine("1" + "|" + "C" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "|" + "BIEN " + " |" + Estado_Doc + "|| | |0|0|0|0|0|0|0|" + dr[5] + "|0|");
                    }
                    else if (int.Parse(dr[13].ToString()) == 2) //contribuyente
                    {
                        sw.WriteLine("1" + "|" + "C" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "|" + "BIEN " + " |" + Estado_Doc + "|| | |0|0|0|" + dr[5] + "|0|0|0|0|0|");
                    }
                }
                sw.Flush();
            }
            sw.Close();        
        }
        if (ventasDS != null)
        {
            sw = new StreamWriter(path, true);
            //sw.WriteLine(nit + "|" + NoEstablecimiento + "|" + mes + "|" + ano);
            //sw.Flush();
            foreach (DataRow dr in ventasDS.Tables["asistecompras"].Rows)
            {
                exbien = int.Parse(dr[8].ToString());
                if (int.Parse(dr[11].ToString()) == 3)
                {
                    Estado_Doc = "A";
                }
                else
                {
                    Estado_Doc = "E";
                }
                if (exbien == 1) // bien
                {
                    if (int.Parse(dr[13].ToString()) == 1) //excento
                    {
                        sw.WriteLine(dr[12] + "|" + "V" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "||" + Estado_Doc + "|" + "|| | |0|0|0|0|" + dr[5] + "|0|0|");
                    }
                    else if (int.Parse(dr[13].ToString()) == 2) //contribuyente
                    {
                        sw.WriteLine(dr[12] + "|" + "V" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "||" + Estado_Doc + "|" + "|| | |" + dr[5] + "|0|0|0|0|0|0|");
                    }
                   
                }
                else if (exbien == 2) //servicio
                {

                    if (int.Parse(dr[13].ToString()) == 1) //excento
                    {
                        sw.WriteLine(dr[12] + "|" + "V" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "| |" + Estado_Doc + "|" + "|| | |0|0|0|0|0|0|" + dr[5] + "|0|");
                    }
                    else if (int.Parse(dr[13].ToString()) == 2) //contribuyente
                    {
                        sw.WriteLine(dr[12] + "|" + "V" + "|" + dr[0] + "|" + dr[1] + "|" + dr[2] + "|" + dr[3] + "|" + dr[9] + "|" + dr[10] + "|" + dr[4] + "| |" + Estado_Doc + "|" + "|| | |0|0|" + dr[5] + "|0|0|0|0|0|");
                    }
                }
                sw.Flush();
            }
            sw.Close();
        }
        if (File.Exists(path) == true)
        {
            #region Descargar Archivo
            Response.Clear();
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.Flush();
            Response.WriteFile(path);
            Response.End();
            #endregion
        }
        else
        {
            WebMsgBox.Show("Ruta de acceso al archivo es  invalida");
        }
        
    }
}
