using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class invoice_re_print : System.Web.UI.Page
{
    UsuarioBean user = null;
    int fac_id = 0, tipo = 1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        if (Request.QueryString["fac_id"] != null)
        {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            tb_factID.Text = fac_id.ToString();
            lb_tipo.Text = tipo.ToString();
        }
        if (Request.QueryString["s"] != null)//Serie
        {lb_serie.Text = Request.QueryString["s"].ToString();}
        if (Request.QueryString["c"] != null)//Correlativo
        { lb_correlativo.Text = Request.QueryString["c"].ToString(); }
        if ((tipo == 6)||(tipo == 20))
        {
            Label2.Visible = false;
            lb_serie.Visible = false;
            Label3.Text = "";
            lb_correlativo.Text = "Banco: " + Request.QueryString["bco"].ToString() + "<br>" + "&nbsp;&nbsp;Cuenta: " + Request.QueryString["ctaID"].ToString() + "<br>" + "&nbsp;&nbsp;Numero: " + Request.QueryString["correlativo"].ToString();
        }

        //2020-06-05
        user = (UsuarioBean)Session["usuario"];
        if ((user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 26) || (user.PaisID == 38)) // sv sv2 svltf svtla
        {
            btn_imprimirSV.Visible = true;
           // btn_imprimir.Visible = true;
        }
        else
        {
            btn_imprimirSV.Visible = false;
            //btn_imprimir.Visible = true;
        }


    }


    protected void btn_imprimir_Click(object sender, EventArgs e)
    {
            print("DLL");
    }

    protected void btn_imprimir_ClickSV(object sender, EventArgs e)
    {
            print("HTML");
    }


    protected void print(string tipo) { 


        
            user = (UsuarioBean)Session["usuario"];
            DB.insertReimpresionLog(int.Parse(tb_factID.Text), int.Parse(lb_tipo.Text), tb_motivo.Text, user);
            
                String csname1 = "PopupScript";
                Type cstype = this.GetType();
                // Get a ClientScriptManager reference from the Page class.
                ClientScriptManager cs2 = Page.ClientScript;
                string script = "";
                if ((Request.QueryString["tipo"].ToString() != "6") && (Request.QueryString["tipo"].ToString() != "20"))//Factura
                {
                    //Descomentar este
                    //script = "window.open('printersettings.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    
                    //script = "window.open('print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                    //script = "window.open('print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //if ((lb_tipo.Text == "1") || (lb_tipo.Text == "2") || (lb_tipo.Text == "3") || (lb_tipo.Text == "4"))
                    //{
                        /*int ID = 0;
                        string path = "";
                        RE_GenericBean rgb = new RE_GenericBean();
                        ID = int.Parse(tb_factID.Text.ToString());

                        if (lb_tipo.Text == "1")
                        {
                            rgb = (RE_GenericBean)DB.getFacturaData(ID);
                            path = DB.getpathImpresion(1, rgb.intC2, rgb.strC28);
                        }
                        if (lb_tipo.Text == "2")
                        {
                            rgb = (RE_GenericBean)DB.getRcptData(ID);
                            path = DB.getpathImpresion(2, rgb.intC2, rgb.strC32);
                        }
                        if (lb_tipo.Text == "3")
                        {
                            rgb = (RE_GenericBean)DB.getNotaCreditoData(ID);
                            path = DB.getpathImpresion(3, rgb.intC2, rgb.strC32);
                        }
                        if (lb_tipo.Text == "4")
                        {
                            rgb = (RE_GenericBean)DB.getNotaDebitoData_forprint(ID);
                            path = DB.getpathImpresion(4, rgb.intC2, rgb.strC32);
                        }*/


                    //if ((user.PaisID == 1) || (user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 15) || (user.PaisID == 3) || (user.PaisID == 5) || (user.PaisID == 23) || (user.PaisID == 20) || (user.PaisID == 21) || (user.PaisID == 26))
                    //    {
                            //2020-06-05
                            //script = "window.open('../ImpresionDocumentos.html?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "&pais="+user.PaisID.ToString()+"&idioma="+user.Idioma.ToString()+"&tipo_cambio="+user.pais.TipoCambio.ToString()+"&contaId="+user.contaID.ToString()+"','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //    }-
                    //else
                    //{
                    //    script = "window.open('printersettings.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //}
                    //}
                    //else
                    //{
                    //    script = "window.open('printersettings.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //}


                    //2020-06-05
                    if (tipo == "HTML") // sv sv2 svltf svtla
                    {
                        //script = "window.open('printersettings.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                        //2020-06-05 se pasaron las funcionalidades de printersettings aca, para evitar la lectura de local printers, en todo el proyecto con esta fecha
                        script = "window.open('print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));parent.close();";
                    }
                    else
                    {
                        script = "window.open('../ImpresionDocumentos.html?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "&pais=" + user.PaisID.ToString() + "&idioma=" + user.Idioma.ToString() + "&tipo_cambio=" + user.pais.TipoCambio.ToString() + "&contaId=" + user.contaID.ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    }

                }
                else if (Request.QueryString["tipo"].Equals("6"))//Cheque
                {

                    //RE_GenericBean ChequeBean = (RE_GenericBean)DB.getChequeData(Request.QueryString["ctaID"].ToString(), int.Parse(Request.QueryString["correlativo"].ToString()), "");
                    //script = "window.open('../invoice/print_cheque.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "','Imprimir','menubar=1,resizable=1,scrollbars=1,toolbar=1,width=750');";
                    //script = "window.open('../invoice/print_cheque.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //if ((user.PaisID == 1) || (user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 15) || (user.PaisID == 3) || (user.PaisID == 5) || (user.PaisID == 23) || (user.PaisID == 20) || (user.PaisID == 21) || (user.PaisID == 26))
                    //{
                        script = "window.open('../ImpresionDocumentos.html?tipo_cambio=" + user.pais.TipoCambio.ToString() + "&id=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "&bcoId=" + Request.QueryString["bco"].ToString() + "&fac_id=" + Request.QueryString["fac_id"].ToString() + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //    //script = "window.open('../plantillas/impresion.aspx?id=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "&bcoId=" + Request.QueryString["bcoId"].ToString() + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
                    //}
                    //else
                    //{
                    //    script = "window.open('../invoice/printersettings.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&tipo=" + lb_tipo.Text + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "&fac_id=" + Request.QueryString["fac_id"] + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //}
                }
                else if (Request.QueryString["tipo"].Equals("20"))//Retencion
                {
                    //if ((user.PaisID == 1) || (user.PaisID == 2) || (user.PaisID == 9) || (user.PaisID == 15) || (user.PaisID == 3) || (user.PaisID == 5) || (user.PaisID == 23) || (user.PaisID == 20) || (user.PaisID == 21) || (user.PaisID == 26))
                    //{
                        script = "window.open('../ImpresionDocumentos.html?chequeID=" + Request.QueryString["fac_id"].ToString() + "&sucursalID=" + user.SucursalID.ToString() + "&userID=" + user.ID + "&contaId=" + user.contaID.ToString() + "&tipo=20','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                        //script = "window.open('../plantillas/impresion.aspx?id=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "&bcoId=" + Request.QueryString["bcoId"].ToString() + "&tipo=6','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
                    //}
                    //else
                    //{
                    //    script = "window.open('../invoice/printersettings.aspx?ctaID=" + Request.QueryString["fac_id"].ToString() + "&tipo=" + lb_tipo.Text + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "','Re-Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));window.close();";
                    //}
                }
                if (!cs2.IsStartupScriptRegistered(cstype, csname1))
                {
                    cs2.RegisterStartupScript(cstype, csname1, script, true);
                }
                btn_imprimir.Enabled = false;
            
    }
}
