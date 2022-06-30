using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;

/// <summary>
/// Summary description for BAW_Provisionar_BL
/// </summary>
//[WebService(Namespace = "http://localhost:63599/BAWSERVER/WebServices/BAW_Provisionar_BL.asmx/",Description="Web Service para provisionar BL'S desde Traficos Aereo, Terrestre y Maritimo.")]
[WebService(Namespace = "http://10.10.1.7:8181/WebServices/BAW_Provisionar_BL.asmx/", Description = "Web Service para provisionar BL'S desde Traficos Aereo, Terrestre y Maritimo.")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class BAW_Provisionar_BL : System.Web.Services.WebService 
{
    int blID = 0;
    int sisID = 0;
    int Tipo_Operacion = 0;
    string paisID = "";
    int Conta_Id = 0;
    PaisBean Pais_Bean = new PaisBean();
    string usuID = "";
    string[,] M_Monedas = new string[6, 2] { { "1", "GTQ" }, { "3", "LPS" }, { "4", "NIO" }, { "5", "CRC" }, { "7", "BZD" }, { "8", "USD" } };
    string[,] M_Tipo_Persona = new string[4, 3] { { "Linea Aerea", "0", "6" }, { "Agente", "1", "2" }, { "Naviera", "2", "5" }, { "Proveedor", "3", "4" } };//Tipo-ID_Master,ID_BAW
    string[,] M_Pais_Impuestos = new string[23, 3] { { "1", "1", "2" }, { "1", "2", "1" }, { "2", "1", "2" }, { "3", "1", "2" }, { "3", "2", "1" }, { "4", "1", "2" }, { "5", "1", "2" }, { "5", "2", "2" }, { "6", "1", "2" }, { "11", "1", "2" }, { "11", "2", "1" }, { "7", "1", "2" }, { "7", "2", "1" }, { "15", "1", "2" }, { "15", "2", "1" }, { "21", "1", "2" }, { "21", "2", "2" }, { "25", "1", "2" }, { "24", "1", "2" }, { "26", "1", "2" }, { "23", "1", "2" }, { "23", "2", "1" }, { "9", "1", "2" } };//PAISID-CONTABILIDAD-ImpuestoProveedor: 1=EXCENCETO 2=CONTRIBUYENTE
    string[,] M_Contabilidad = new string[25, 3] { { "1", "GTQ", "1" }, { "1", "USD", "1" }, { "2", "USD", "1" }, { "3", "LPS", "1" }, { "3", "USD", "1" }, { "4", "NIO", "1" }, { "4", "USD", "1" }, { "5", "CRC", "1" }, { "5", "USD", "1" }, { "6", "USD", "1" }, { "11", "NIO", "1" }, { "11", "USD", "1" }, { "7", "BZD", "1" }, { "7", "USD", "1" }, { "15", "GTQ", "1" }, { "15", "USD", "1" }, { "21", "CRC", "1" }, { "21", "USD", "1" }, { "25", "USD", "1" }, { "24", "NIO", "1" }, { "24", "USD", "1" }, { "26", "USD", "1" }, { "23", "LPS", "1" }, { "23", "USD", "1" }, { "9", "USD", "1" } };//PAISID-MONEDA-CONTA
    public BAW_Provisionar_BL () 
    {
        
    }
    [WebMethod]
    public int Provisionar_Costos(int _blID, int _sisID, int _Tipo_Operacion, string _paisID, string _usuID)
    {
        int bandera = 0;
        blID = _blID;
        sisID = _sisID;
        Tipo_Operacion = _Tipo_Operacion;
        paisID = _paisID;
        usuID = _usuID.ToLower();
        if (Existe_Pais(_paisID) == true)
        {
            Pais_Bean = Cargar_Pais(paisID);
            if (Existe_Tipo_Cambio(paisID) == true)
            {
                ArrayList Arr_Costos = (ArrayList)DB.Get_CostosBy_Traficos(sisID, Tipo_Operacion, blID, Pais_Bean);
                bandera = Validar_Restricciones(blID, sisID, Tipo_Operacion, Pais_Bean);
                if (bandera == 5)
                {
                    return bandera;
                }
                if ((Arr_Costos != null) && (Arr_Costos.Count > 0))
                {
                    int Cantidad_Costos = Arr_Costos.Count;
                    RE_GenericBean Bean_I = new RE_GenericBean();
                    RE_GenericBean Bean_J = new RE_GenericBean();
                    RE_GenericBean Provision = new RE_GenericBean();
                    RE_GenericBean Totales = null;
                    Rubros Rubro_Bean = null;   
                    int Tipo_Persona = 0;
                    int ImpuestoProveedor = 0;
                    if (Cantidad_Costos > 0)
                    {
                        for (int i = 0; i < (Cantidad_Costos); i++)
                        {
                            Bean_I = (RE_GenericBean)Arr_Costos[i];
                            #region Validar que no se Contabilicen Costos de Importacion en Empresas con SCA - Carga Maritima
                            /*if (sisID == 1)
                            {
                                if ((paisID == "1") || (paisID == "2") || (paisID == "3") || (paisID == "5") || (paisID == "7") || (paisID == "15") || (paisID == "21") || (paisID == "23") || (paisID == "26") || (paisID == "6") || (paisID == "25"))
                                {
                                    int imp_Exp_TEMPORAL = 0;
                                    imp_Exp_TEMPORAL = Traducir_Imp_Exp(Bean_I.strC10);//tpr_imp_exp_id
                                    if (imp_Exp_TEMPORAL == 1)
                                    {
                                        //Este Embarque es una Importacion y no puede ser Contabilizado desde Trafico, debe ser Contabilizado desde SCA
                                        return 18;
                                    }
                                }
                            }*/
                            #endregion
                            #region Agregar Costos
                            Conta_Id = Definir_Contabilidad(paisID, Bean_I.strC21);
                            #region Validar Contabilidad Definida
                            if (Conta_Id == 0)
                            {
                                log4net ErrLog = new log4net();
                                ErrLog.ErrorLog("No se pudo realizar la Provision del BL.:" + blID + ", Operacion.: " + Tipo_Operacion + " y Sistema.: " + sisID + " ,Porque la moneda del CostoID.: " + Bean_I.strC15 + " no tiene Contabilidad Configurada");
                                return 8;
                            }
                            #endregion
                            Tipo_Persona = Traducir_Tipo_Persona(Bean_I.strC1);
                            ImpuestoProveedor = DB.getProveedorRegimen(Tipo_Persona, Bean_I.strC2);
                            ImpuestoProveedor = Setear_Cobro_Impuesto(Pais_Bean.ID.ToString(), Conta_Id, int.Parse(Bean_I.strC26));
                            if (Bean_I.strC11 == "False")
                            {
                                Rubro_Bean = Calcular_Impuestos(long.Parse(Bean_I.strC3), double.Parse(Bean_I.strC5), Conta_Id, ImpuestoProveedor, int.Parse(Bean_I.strC23), Tipo_Operacion, sisID, double.Parse(Bean_I.strC15), Traducir_Tipo_Moneda(Bean_I.strC21));
                                Rubro_Bean.rubroTipo_Contribuyente = ImpuestoProveedor;
                                if (Provision.arr1 == null) Provision.arr1 = new ArrayList();
                                Provision.arr1.Add(Rubro_Bean);
                                Bean_I.strC11 = "True";
                                Arr_Costos[i] = Bean_I;//Actualizo el Arreglo para indicar que el costo ya fue Provisionado
                            }
                            #endregion
                            for (int j = i+1; j < Cantidad_Costos; j++)
                            {
                                Bean_J = (RE_GenericBean)Arr_Costos[j];
                                #region Validar Restricciones
                                /*
                                if (Bean_J.strC13 == "")//No tiene Routing=CIF
                                {
                                    bandera = Validar_Restricciones("Provisionar_Costos", Pais_Bean.ID, Conta_Id, Bean_J.strC13, Bean_J.strC23, Bean_J.strC25);
                                    if (bandera == 5)
                                    {
                                        return bandera;
                                    }
                                }
                                 */ 
                                #endregion
                                if ((Bean_I.strC1 == Bean_J.strC1) && (Bean_I.strC2 == Bean_J.strC2) && (Bean_I.strC7 == Bean_J.strC7))
                                {
                                    #region Agregar Costos
                                    ImpuestoProveedor = Setear_Cobro_Impuesto(Pais_Bean.ID.ToString(), Conta_Id, int.Parse(Bean_J.strC26));
                                    if (Bean_J.strC11 == "False")
                                    {
                                        if (Bean_I.strC21 == Bean_J.strC21)
                                        {
                                            Rubro_Bean = Calcular_Impuestos(long.Parse(Bean_J.strC3), double.Parse(Bean_J.strC5), Conta_Id, ImpuestoProveedor, int.Parse(Bean_J.strC23), Tipo_Operacion, sisID, double.Parse(Bean_J.strC15), Traducir_Tipo_Moneda(Bean_J.strC21));
                                            Rubro_Bean.rubroTipo_Contribuyente = ImpuestoProveedor;
                                            if (Provision.arr1 == null) Provision.arr1 = new ArrayList();
                                            Provision.arr1.Add(Rubro_Bean);
                                            Bean_J.strC11 = "True";
                                            Arr_Costos[j] = Bean_J;//Actualizo el Arreglo para indicar que el costo ya fue Provisionado
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #region Cargar Info Provision
                            if (Provision.arr1 != null)
                            {
                                if (Provision.arr1.Count > 0)
                                {
                                    Provision.intC1 = int.Parse(Bean_I.strC2);//tpr_proveedor_id
                                    Provision.intC2 = Pais_Bean.ID;//tpr_pai_id
                                    Provision.strC1 = usuID;//tpr_usu_creacion
                                    Provision.intC3 = 1;//tpr_ted_id
                                    Provision.strC2 = "";//tpr_serie
                                    Provision.strC3 = "";//tpr_correlativo
                                    Provision.intC4 = Tipo_Operacion;//tpr_tto_id
                                    Provision.strC4 = Bean_I.strC8;//tpr_mbl
                                    Provision.strC5 = Bean_I.strC12;//tpr_hbl
                                    Provision.strC6 = Bean_I.strC13;//tpr_routing
                                    Provision.strC7 = Bean_I.strC9;//tpr_contenedor
                                    Provision.intC5 = Tipo_Persona;
                                    Provision.intC6 = Traducir_Tipo_Moneda(Bean_I.strC21);//tpr_mon_id
                                    Provision.intC7 = Traducir_Imp_Exp(Bean_I.strC10);//tpr_imp_exp_id
                                    Provision.intC8 = Conta_Id;//tpr_tcon_id
                                    Provision.strC8 = DB.Get_Persona_Nombre(Tipo_Persona, Provision.intC1);//tpr_nombre
                                    Provision.intC9 = Cargar_Datos_Sucursal(Conta_Id, Provision.intC6);//tpr_suc_id
                                    Totales = Calcular_Totales(Provision, Conta_Id, ImpuestoProveedor);
                                    Provision.decC1 = Totales.decC1;//Total
                                    Provision.decC2 = Totales.decC2;//Afeto
                                    Provision.decC3 = Totales.decC3;//No Afecto
                                    Provision.decC4 = Totales.decC4;//Impuesto
                                    Provision.decC5 = Totales.decC5;//Total Equivalente
                                    Provision.intC10 = blID;//BLID
                                    ImpuestoProveedor = DB.getProveedorRegimen(Tipo_Persona, Bean_I.strC2);
                                    Provision.intC11 = ImpuestoProveedor;//Tipo de Contribuyente
                                    Provision.strC9 = Bean_I.strC7;//tpr_fact_corr
                                    Insertar_Provision(Provision, Pais_Bean, blID);
                                    Provision.arr1 = null;
                                }
                            }
                            #endregion
                        }
                    }
                    return 1;
                }
                else
                {
                    log4net ErrLog = new log4net();
                    ErrLog.ErrorLog("No se puedo realizar la Provision del BL.:" + blID + " y Sistema.: " + sisID + " ,Porque no tiene ningun costo asociado ");
                    return 2;
                }
            }
            else
            {
                log4net ErrLog = new log4net();
                ErrLog.ErrorLog("No se puedo realizar la Provision del BL.:" + blID + " y Sistema.: " + sisID + " ,Porque no existe tipo de cambio para el pais.: " + Pais_Bean.Nombre + " ");
                return 3;
            }
        }
        else
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog("No se pudo realizar la Provision del BL.:" + blID + " y Sistema.: " + sisID + " ,Porque el Pais.: " + Pais_Bean.Nombre + " no se encuentra configurado");
            return 4;
        }
    }
    protected bool Existe_Pais(string ID)
    {
        #region Validar que exista el Pais
        ArrayList Arr = (ArrayList)DB.getPaises("");
        bool Resultado = false;
        foreach (PaisBean Pais in Arr)
        {
            if (Pais.ID.ToString() == ID)
            {
                Resultado = true;
            }
        }
        return Resultado;
        #endregion
    }
    protected PaisBean Cargar_Pais(string ID)
    {
        #region Cargar Datos del Pais
        ArrayList Arr = (ArrayList)DB.getPaises("");
        foreach (PaisBean Pais in Arr)
        {
            if (Pais.ID.ToString() == ID)
            {
                Pais_Bean = Pais;
                Pais_Bean.TipoCambio = DB.getTipoCambioHoy(Pais_Bean.ID);
            }
        }
        return Pais_Bean;
        #endregion
    }
    protected bool Existe_Tipo_Cambio(string ID)
    {
        #region Validar que exista Tipo de Cambio
        bool Resultado = false;
        decimal Tipo_Cambio = 0;
        Tipo_Cambio = DB.getTipoCambioHoy(int.Parse(ID));
        if (Tipo_Cambio > 0)
        {
            Resultado = true;
        }
        return Resultado;
        #endregion
    }
    protected int Traducir_Tipo_Moneda(string Moneda)
    {
        #region Traducir Moneda
        int MonID = 0;
        for (int i = 0; i < 6; i++)
        {
            if (M_Monedas[i, 1] == Moneda)
            {
                MonID = int.Parse(M_Monedas[i, 0]);
            }
        }
        return MonID;
        #endregion
    }
    protected int Traducir_Tipo_Persona(string ID)
    {
        #region Traducir Tipo de Persona
        int TPI = 0;
        for (int i = 0; i < 4; i++)
        {
            if (M_Tipo_Persona[i, 1] == ID)
            {
                TPI = int.Parse(M_Tipo_Persona[i, 2]);
            }
        }
        return TPI;
        #endregion
    }
    protected int Traducir_Imp_Exp(string Imp_Exp)
    {
        #region Traducir Importacion Exportacion
        int resultado = 0;
        if (Imp_Exp == "True")
        {
            resultado = 1;//Importacion
        }
        else
        {
            resultado = 2;//Exportacion
        }
        return resultado;
        #endregion
    }
    protected Rubros Calcular_Impuestos(long RubroID, double Monto, int ContaID, int Impuesto_Proveedor, int servicioID, int operacionID, int sistemaID, double CostoID, int monID)
    {
        #region Calcular Subtotal, Impuesto de Rubro por Pais
        Rubros Rubros_Bean = null;
        Rubros Rubro = null;
        Rubro = new Rubros();
        Rubro.rubroID = RubroID;
        Rubros_Bean = (Rubros)DB.ExistRubroPais(Rubro, Pais_Bean.ID);
        Rubros_Bean.rubroTypeID = servicioID;
        Rubros_Bean.rubroOperacion = operacionID;
        Rubros_Bean.rubroSistema = sistemaID;
        Rubros_Bean.RubroCostoID = CostoID;
        if (Rubros_Bean == null)
        {
            log4net ErrLog = new log4net();
            ErrLog.ErrorLog("Error, El BL no tiene todos los rubros registrados en contabilidad para este pais.");
            return null;
        }
        if ((Impuesto_Proveedor != 1))//si debe cobrar iva y el rubro no esta en dolares y no es excento
        {
            if (Rubros_Bean.rubroTypeID != 14)//Hace calculo de impuestos a menos que el servicio sea de Terceros
            {
                //
                if (Rubros_Bean.CobIva == 1)
                {
                    if (Rubros_Bean.IvaInc == 1)
                    {
                        Rubros_Bean.rubroTot = Monto;
                        Rubros_Bean.rubroSubTot = Math.Round(Monto * (double)(1 / (1 + Pais_Bean.Impuesto)), 2);
                        Rubros_Bean.rubroImpuesto = Math.Round(Monto - Rubros_Bean.rubroSubTot, 2);
                    }
                    else
                    {
                        Rubros_Bean.rubroImpuesto = Math.Round(Monto * (double)Pais_Bean.Impuesto, 2);
                        Rubros_Bean.rubroSubTot = Math.Round(Monto, 2);
                        Rubros_Bean.rubroTot = Math.Round(Rubros_Bean.rubroSubTot + Rubros_Bean.rubroImpuesto, 2);
                    }
                }
                else if (Rubros_Bean.CobIva == 0)
                {
                    Rubros_Bean.rubroTot = Monto;
                    Rubros_Bean.rubroSubTot = Monto;
                    Rubros_Bean.rubroImpuesto = 0;
                }
                //
            }
            else
            {
                Rubros_Bean.rubroTot = Monto;
                Rubros_Bean.rubroSubTot = Monto;
                Rubros_Bean.rubroImpuesto = 0;
            }
        }
        else
        {
            Rubros_Bean.rubroTot = Monto;
            Rubros_Bean.rubroSubTot = Monto;
            Rubros_Bean.rubroImpuesto = 0;
        }
        if (monID == 8)
        {
            Rubros_Bean.rubroTotD = Math.Round((double
                )Rubros_Bean.rubroTot * (double)Pais_Bean.TipoCambio, 2);
        }
        else
        {
            Rubros_Bean.rubroTotD = Math.Round((double)Rubros_Bean.rubroTot / (double)Pais_Bean.TipoCambio, 2);
        }
        return Rubros_Bean;
        #endregion
    }
    protected int Cargar_Datos_Sucursal(int ContaID, int monID)
    {
        #region Cargar Sucursal
        int SucID = 0;
        ArrayList Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Pais_Bean.ID + " and suc_nombre='SISTEMAS' ");
        if (Arr_Sucursales.Count == 0)
        {
            #region Crear Sucursal
            SucursalBean sucbean = new SucursalBean();
            sucbean.Nombre = "SISTEMAS";
            sucbean.paisID = Pais_Bean.ID;
            int result = DB.InsertUpdateSucursal(sucbean);
            #endregion
            #region Obtener ID de la Sucursal
            Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Pais_Bean.ID + " and suc_nombre='SISTEMAS' ");
            foreach (SucursalBean Bean in Arr_Sucursales)
            {
                SucID = Bean.ID;
            }
            #endregion
            #region Crear Series y Secuencias
            RE_GenericBean Bean_Suc = (RE_GenericBean)DB.Get_Serie_CorrelativoBy_Traficos(SucID, Conta_Id, monID);
            if (Bean_Suc == null)
            {
                int Resultado = DB.Crear_Series_Provisiones_Automaticas(SucID, Pais_Bean, Tipo_Operacion, Conta_Id, monID);
            }
            #endregion
        }
        else
        {
            #region Obtener ID de la Sucursal
            Arr_Sucursales = (ArrayList)DB.getSucursales(" and  suc_pai_id=" + Pais_Bean.ID + " and suc_nombre='SISTEMAS' ");
            foreach (SucursalBean Bean in Arr_Sucursales)
            {
                SucID = Bean.ID;
            }
            #endregion
            #region Crear Series y Secuencias
            RE_GenericBean Bean_Suc = (RE_GenericBean)DB.Get_Serie_CorrelativoBy_Traficos(SucID, Conta_Id, monID);
            if (Bean_Suc == null)
            {
                int Resultado = DB.Crear_Series_Provisiones_Automaticas(SucID, Pais_Bean, Tipo_Operacion, Conta_Id, monID);
            }
            #endregion
        }
        return SucID;
        #endregion
    }
    protected RE_GenericBean Calcular_Totales(RE_GenericBean Provision, int Conta_Id, int ImpuestoProveedor)
    {
        #region Calcular Totales
        RE_GenericBean Resultado = new RE_GenericBean();
        double Total = 0, Total_Equivalente = 0, Impuesto = 0, Afecto = 0, Noafecto = 0;
        foreach (Rubros rubro in Provision.arr1)
        {
            if (rubro.rubroTipo_Contribuyente != 1)
            {
                if (rubro.CobIva == 1)
                {
                    Afecto += rubro.rubroSubTot;
                }
                else if (rubro.CobIva == 0)
                {
                    Noafecto += rubro.rubroSubTot;
                }
            }
            else
            {
                Noafecto += rubro.rubroSubTot;
            }
            Total += rubro.rubroTot;
            Impuesto += rubro.rubroImpuesto;

        }
        if (Conta_Id == 2)
        {
            Total_Equivalente = Math.Round((double)Total * (double)Pais_Bean.TipoCambio, 2);
        }
        else
        {
            if (Provision.intC6 == 8)
            {
                Total_Equivalente = Math.Round((double)Total * (double)Pais_Bean.TipoCambio, 2);
            }
            else
            {
                Total_Equivalente = Math.Round((double)Total / (double)Pais_Bean.TipoCambio, 2);
            }
        }
        Resultado.decC1 = (decimal)Total;
        Resultado.decC2 = (decimal)Afecto;
        Resultado.decC3 = (decimal)Noafecto;
        Resultado.decC4 = (decimal)Impuesto;
        Resultado.decC5 = (decimal)Total_Equivalente;
        return Resultado;
        #endregion
    }
    protected void Insertar_Provision(RE_GenericBean Provision, PaisBean Pais, int blID)
    {
        #region Provisionar
        ArrayList Arr_Resultado = null;
        if (Provision.arr1.Count > 0)// Si tiene Costos
        {
            Arr_Resultado = (ArrayList)DB.Insertar_Provision_Automatica(Provision, Pais, blID);
            if (Arr_Resultado == null)
            {
                log4net ErrLog = new log4net();
                ErrLog.ErrorLog("No se puedo realizar la Provision del BL.:" + blID);
            }
        }
        #endregion
    }
    protected int Setear_Cobro_Impuesto(string paiID, int contaID, int ImpuestoProveedor)
    {
        #region Setear si se Cobra o no Impuesto Dependiendo de la Conta
        int resultado = 0;
        for (int i = 0; i < 22; i++)
        {
            if ((paiID == M_Pais_Impuestos[i,0])&&(contaID.ToString()==M_Pais_Impuestos[i,1]))
            {
                if (contaID == 1)
                {
                    resultado = ImpuestoProveedor;
                }
                else if (contaID == 2)
                {
                    if (paiID == "5")
                    {
                        resultado = ImpuestoProveedor;
                    }
                    else
                    {
                        resultado = int.Parse(M_Pais_Impuestos[i, 2]);
                    }
                }
            }
        }
        return resultado;
        #endregion
    }
    protected int Definir_Contabilidad(string paiID, string moneda)
    {
        #region Definir_Contabilidad
        int resultado = 0;
        for (int i = 0; i < 25; i++)
        {
            if ((M_Contabilidad[i, 0] == paiID) && (M_Contabilidad[i, 1] == moneda))
            {
                resultado = int.Parse(M_Contabilidad[i, 2]);
            }
        }
        return resultado;
        #endregion
    }
    protected int Validar_Pagos_Terceros(int rubID, int servID)
    {
        #region Validar la Clasificacion de Servicios para los Fletes
        int resultado = 0;
        if ((rubID == 28) || (rubID == 11))//OCEAN FREIGHT-AIR FREIGHT
        {
            if (servID != 14)
            {
                resultado = 5;
            }
        }
        resultado = 0;
        return resultado;
        #endregion
    }
    protected int Validar_Restricciones(int r_blID, int r_sisID, int r_Tipo_Operacion, PaisBean r_Pais)
    {
        int resultado=0;
        ArrayList Arr_Costos = (ArrayList)DB.Get_CostosBy_Traficos(r_sisID, r_Tipo_Operacion, r_blID, r_Pais);
        int Cantidad_Costos = Arr_Costos.Count;
        RE_GenericBean Bean_I = new RE_GenericBean();
        if ((Arr_Costos != null) && (Arr_Costos.Count > 0))
        {
            for (int i = 0; i < (Cantidad_Costos); i++)
            {
                Bean_I = (RE_GenericBean)Arr_Costos[i];
                if (Bean_I.strC13.Trim().Equals(""))
                {
                    if ((Bean_I.strC25 == "1")||(Bean_I.strC25 == "True"))//PAGO A TERCEROS = VIENE EN EL BL
                    {
                        if (Bean_I.strC23 != "14")//SI NO ES PAGO A TERCEROS
                        {
                            return 5;
                        }
                    }
                }

            }
        }
        return resultado;
    }
    [WebMethod]
    public string Validar_Cobros_Pendientes(int v_BLID, int v_Tipo_Operacion, string _paisID)
    {
        //0= Cargo Internacional 1=Cargo Local
        string resultado = "0";
        PaisBean Pais_Bean_ = Cargar_Pais(_paisID);
        ArrayList Cargos_BL = DB.Get_CargosXBL_Operacion(v_BLID, v_Tipo_Operacion, Pais_Bean_);
        if (Cargos_BL == null)
        {
            resultado= "7";//Error al momento de validar Cargos

        }
        else if (Cargos_BL.Count == 0)
        {
            resultado = "Recordatorio, No existe ningun Cargo Asociado al Documento BL, porfavor ingreselos. ";//BL sin Cargos
        }
        else
        { 
            int Cargos_Internacionales = 0;
            int Cargos_Locales = 0;
            int Cargos_Sin_Clasificacion = 0;
            int Cargos_Pagados = 0;
            foreach (RE_GenericBean Bean in Cargos_BL)
            {
                resultado = "";
                if (Bean.strC7 == "-1")
                {
                    Cargos_Sin_Clasificacion++;//Bl sin Clasificacion
                }
                if (Bean.strC6 != "0")
                {
                    Cargos_Pagados++;
                }
                if ((Bean.strC6 == "0") && (Bean.strC7 == "0"))//No esta Cobrado y es Cargo Internacional
                {
                    Cargos_Internacionales++;
                }
                else if ((Bean.strC6 == "0") && (Bean.strC7 == "1"))//No esta Cobrado y es Cargo Local
                {
                    Cargos_Locales++;
                }
            }
            if (Cargos_Internacionales > 0)
            {
                resultado = "Existen " + Cargos_Internacionales + " Cargo(s) Internacional(e)s pendiente(s) de Cobrar ";
            }
            if (Cargos_Locales > 0)
            {
                if (resultado == "")
                {
                    resultado = "Existen " + Cargos_Locales + " Cargo(s) Local(es) pendiente(s) de Cobrar ";
                }
                else
                {
                    resultado += " y " + Cargos_Locales + " Cargo(s) Local(es) pendiente(s) de cobrar";
                }
            }
            if (Cargos_Sin_Clasificacion > 0)
            {
                if (resultado == "")
                {
                    resultado = "Existen " + Cargos_Sin_Clasificacion + " Cargo(s) sin Clasficacion pendiente(s) de Cobrar ";
                }
                else
                {
                    resultado += " y " + Cargos_Sin_Clasificacion + " Cargo(s) sin Clasficacion pendiente(s) de cobrar";
                }
            }
        }
        return resultado;
    }
    [WebMethod]
    public string Get_Alerta(int ID)
    {
        string resultado = "";
        resultado = DB.Get_Alerta(ID);
        return resultado;
    }
    [WebMethod]
    public ArrayList Validar_Contabilizacion_SCA(string _paisID, int _sisID, int _Tipo_Operacion, int _blID)
    {
        ArrayList resultado = new ArrayList();
        RE_GenericBean Bean_Costos = null;
        int Importacion_Exportacion = 0;
        if (Existe_Pais(_paisID) == false)
        {
            resultado = new ArrayList();
            resultado.Add("0");
            resultado.Add("La Empresa no existe");
            return resultado;
        }
        PaisBean _Pais_Temporal = null;
        _Pais_Temporal = Cargar_Pais(_paisID);
        ArrayList Arr_Costos = (ArrayList)DB.Get_CostosBy_Traficos(_sisID, _Tipo_Operacion, _blID, _Pais_Temporal);
        if (Arr_Costos == null)
        {
            resultado = new ArrayList();
            resultado.Add("0");
            resultado.Add("Existio un error al momento de Tratar de validar el Embarque");
            return resultado;
        }
        if (Arr_Costos.Count == 0)
        {
            resultado = new ArrayList();
            resultado.Add("0");
            resultado.Add("El Embarque no tiene ningun costo para Contabilizar");
            return resultado;
        }
        if ((_paisID == "1") || (_paisID == "2") || (_paisID == "3") || (_paisID == "5") || (_paisID == "7") || (_paisID == "15") || (_paisID == "21") || (_paisID == "23") || (_paisID == "26"))
        {
            int Cantidad_Costos = Arr_Costos.Count;
            for (int i = 0; i < (Cantidad_Costos); i++)
            {
                Bean_Costos = (RE_GenericBean)Arr_Costos[i];
                Importacion_Exportacion = Traducir_Imp_Exp(Bean_Costos.strC10);
            }
            if (Importacion_Exportacion == 1)
            {
                resultado = new ArrayList();
                resultado.Add("0");
                resultado.Add("El Embarque seleccionado es una Importacion y no debe ser Contabilizado desde Trafico, por favor utilice el SCA.");
                return resultado;
            }
            else
            {
                resultado = new ArrayList();
                resultado.Add("1");
                resultado.Add("El Embarque seleccionado es una Exportacion y puede ser Contabilizado desde Trafico.");
                return resultado;
            }
        }
        else
        {
            resultado = new ArrayList();
            resultado.Add("1");
            resultado.Add("El Embarque seleccionado puede Contabilizado desde Trafico.");
            return resultado;
        }
        return resultado;
    }
}

