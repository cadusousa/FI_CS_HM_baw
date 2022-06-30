using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;

public partial class _Default : System.Web.UI.Page
{
    UsuarioBean user;
    int fac_id = 0, tipo = 1;
    bool Empresa_Especial = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["usuario"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Nombre", "<script> refresh(); </script>");
        }
        user = (UsuarioBean)Session["usuario"];
        if (Request.QueryString["fac_id"] != null)
        {
            fac_id = int.Parse(Request.QueryString["fac_id"].ToString());
            tipo = int.Parse(Request.QueryString["tipo"].ToString());
            tb_factID.Text = fac_id.ToString();
            lb_tipo.Text = tipo.ToString();
        }
        if (Request.QueryString["s"] != null)//Serie
        { lb_serie.Text = Request.QueryString["s"].ToString(); }
        if (Request.QueryString["c"] != null)//Correlativo
        { lb_correlativo.Text = Request.QueryString["c"].ToString(); }
        if (!IsPostBack)
        {
            PopulateInstalledPrintersCombo();
        }
    }
    protected void btn_next_Click(object sender, EventArgs e)
    {
        user = (UsuarioBean)Session["usuario"];
        user.PrinterName = lb_impresora.SelectedItem.Text;
        Session["usuario"] = user;
        String csname1 = "PopupScript";
        Type cstype = this.GetType();
        ClientScriptManager cs2 = Page.ClientScript;
        string script = "";
        if ((Request.QueryString["tipo"].ToString() != "6") && (Request.QueryString["tipo"].ToString() != "20"))//Factura
        {
            //script = "window.open('print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));";
            script = "window.open('print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));parent.close();";
            Response.Redirect("print.aspx?fac_id=" + tb_factID.Text + "&tipo=" + lb_tipo.Text + "");
        }
        else if (Request.QueryString["tipo"].Equals("6"))//Cheque
        {
            //script = "window.open('print_cheque.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75));";
            script = "window.open('print_cheque.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "','Imprimir','toolbar=0, status=1, resizable=1, scrollbars=1, top=0, left=0, width=' + (window.screen.width -10) + ', height=' + (window.screen.height -75))";
            Response.Redirect("print_cheque.aspx?ctaID=" + Request.QueryString["ctaID"].ToString() + "&correlativo=" + Request.QueryString["correlativo"].ToString() + "&fac_id=" + Request.QueryString["fac_id"] + "");
        }
        else if (Request.QueryString["tipo"].Equals("20"))//Retencion
        {
            Response.Redirect("../operations/print_retencion.aspx?chequeID=" + Request.QueryString["ctaID"].ToString() + "");
        }
        if (!cs2.IsStartupScriptRegistered(cstype, csname1))
        {
            cs2.RegisterStartupScript(cstype, csname1, script, true);
        }
    }
    
    private void PopulateInstalledPrintersCombo()
    {
        // Add list of installed printers found to the combo box.
        // The pkInstalledPrinters string will be used to provide the display string.
        
        ImpersonationSettings settings = new ImpersonationSettings();
        UserImpersonation userImpersonation = new UserImpersonation(settings);
        try
        {
            userImpersonation.Impersonate();
            //Actividad que deseemos realizar con mayores permisos
            String pkInstalledPrinters;
            ListItem item = new ListItem();
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                #region Definir Impresoras por Pais
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                char[] arr = pkInstalledPrinters.ToCharArray();
                char[] pais = new char[2];
                string parametro_empresaespecial = "";
                string printer = pkInstalledPrinters;
                int longitud = printer.Length;
                if (user.PaisID == 11)//GRH
                {
                    pais = ("GR").ToCharArray();
                }
                else if (user.PaisID == 12)//ISI
                {
                    pais = ("IS").ToCharArray();
                }
                else if (user.PaisID == 13)//MAYAN
                {
                    pais = ("MA").ToCharArray();
                }
                else if (user.PaisID == 18)//ISI GUATEMALA
                {
                    parametro_empresaespecial = "ISIGT-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 15)//LATING FREIGHT GUATEMALA
                {
                    parametro_empresaespecial = "GTLTF-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 26)//LATING FREIGHT EL SALVADOR
                {
                    parametro_empresaespecial = "SVLTF-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 23)//LATING FREIGHT HONDURAS
                {
                    parametro_empresaespecial = "HNLTF-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 24)//LATING FREIGHT NICARAGUA
                {
                    parametro_empresaespecial = "NILTF-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 21)//LATING FREIGHT COSTA RICA
                {
                    parametro_empresaespecial = "CRLTF-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 25)//LATING FREIGHT PANAMA
                {
                    parametro_empresaespecial = "PALTF-";
                    Empresa_Especial = true;
                }
                else if (user.PaisID == 22)//LATING FREIGHT BELICE
                {
                    parametro_empresaespecial = "BZLTF-";
                    Empresa_Especial = true;
                }
                else
                {
                    pais = Utility.InttoISOPais(user.PaisID).ToCharArray();
                }
                int ban_pais = 0;
                int[] pa1 = new int[1];
                int[] pa2 = new int[1];
                int[] pos1 = new int[1];
                int[] pos2 = new int[1];
                int[] pos3 = new int[1];
                int[] CR_mayusculas = new int[] { 67, 82 };
                int[] CR_minusculas = new int[] { 99, 114 };
                int[] PA_mayusculas = new int[] { 80, 65 };
                int[] PA_minusculas = new int[] { 112, 97 };
                int[] NI_mayusculas = new int[] { 78, 73 };
                int[] NI_minusculas = new int[] { 110, 105 };
                int[] GT_mayusculas = new int[] { 71, 84 };
                int[] GT_minusculas = new int[] { 103, 116 };
                int[] HN_mayusculas = new int[] { 72, 78 };
                int[] HN_minusculas = new int[] { 104, 110 };
                int[] SV_mayusculas = new int[] { 83, 86 };
                int[] SV_minusculas = new int[] { 115, 118 };
                int[] BZ_mayusculas = new int[] { 66, 90 };
                int[] BZ_minusculas = new int[] { 98, 122 };
                int[] GRH_mayusculas = new int[] { 71, 82 };//GR
                int[] GRH_minusculas = new int[] { 103, 114 };//GR
                int[] ISI_mayusculas = new int[] { 73, 83 };//ISI
                int[] ISI_minusculas = new int[] { 105, 115 };//ISI
                int[] MAYAN_mayusculas = new int[] { 77, 65 };//MAYAN
                int[] MAYAN_minusculas = new int[] { 109, 97 };//MAYAN
                int[] Mayusculas = new int[2];
                int[] Minusculas = new int[2];
                if (user.PaisID == 5)//CR
                {
                    Mayusculas = CR_mayusculas;
                    Minusculas = CR_minusculas;
                }
                if ((user.PaisID == 1) || (user.PaisID == 14))//GT
                {
                    Mayusculas = GT_mayusculas;
                    Minusculas = GT_minusculas;
                }
                if ((user.PaisID == 2)||(user.PaisID == 9))//SV
                {
                    Mayusculas = SV_mayusculas;
                    Minusculas = SV_minusculas;
                }
                if (user.PaisID == 3)//HN
                {
                    Mayusculas = HN_mayusculas;
                    Minusculas = HN_minusculas;
                }
                if (user.PaisID == 4)//NI
                {
                    Mayusculas = NI_mayusculas;
                    Minusculas = NI_minusculas;
                }
                if (user.PaisID == 6)//PA
                {
                    Mayusculas = PA_mayusculas;
                    Minusculas = PA_minusculas;
                }
                if (user.PaisID == 11)//GRH
                {
                    Mayusculas = GRH_mayusculas;
                    Minusculas = GRH_minusculas;
                }
                if (user.PaisID == 12)//ISI
                {
                    Mayusculas = ISI_mayusculas;
                    Minusculas = ISI_minusculas;
                }
                if (user.PaisID == 13)//MAYAN
                {
                    Mayusculas = MAYAN_mayusculas;
                    Minusculas = MAYAN_minusculas;
                }
                if (user.PaisID == 7)//BZ
                {
                    Mayusculas = BZ_mayusculas;
                    Minusculas = BZ_minusculas;
                }
                if (Empresa_Especial == false)
                {
                    for (int j = 0; j < (longitud - 1); j++)
                    {
                        if ((j + 2) < longitud)
                        {
                            pos1[0] = DB.Asc(arr[j].ToString());
                            pos2[0] = DB.Asc(arr[j + 1].ToString());
                            pos3[0] = DB.Asc(arr[j + 2].ToString());
                            if ((((Mayusculas[0] == pos1[0]) && (Mayusculas[1] == pos2[0])) || ((Minusculas[0] == pos1[0]) && (Minusculas[1] == pos2[0]))) && (pos3[0] == 45))
                            {
                                ban_pais++;
                                lb_impresora.Items.Add(pkInstalledPrinters);
                            }
                        }
                    }
                }
                else
                {
                    if (pkInstalledPrinters.ToUpper().Contains(parametro_empresaespecial.ToUpper()))
                    {
                        lb_impresora.Items.Add(pkInstalledPrinters);
                    }
                }
                #endregion
            }
        }
        finally
        {
            userImpersonation.UndoImpersonation();
        }
    }
}
