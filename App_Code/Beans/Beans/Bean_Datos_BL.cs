using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Datos_BL
/// </summary>
public class Bean_Datos_BL
{
	public Bean_Datos_BL()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private string _Hbl = "";
    public string Hbl
    {
        get { return _Hbl; }
        set { _Hbl = value; }
    }
    private string _Mbl = "";
    public string Mbl
    {
        get { return _Mbl; }
        set { _Mbl = value; }
    }
    private string _Routing = "";
    public string Routing
    {
        get { return _Routing; }
        set { _Routing = value; }
    }
    private string _Contenedor = "";
    public string Contenedor
    {
        get { return _Contenedor; }
        set { _Contenedor = value; }
    }
    private int _ContenedorID = 0;
    public int ContenedorID
    {
        get { return _ContenedorID; }
        set { _ContenedorID = value; }
    }
    private int _Import_Export = 0;
    public int Import_Export
    {
        get { return _Import_Export; }
        set { _Import_Export = value; }
    }
    private bool _MBL_Prepaid_Collect = false;
    public bool MBL_Prepaid_Collect
    {
        get { return _MBL_Prepaid_Collect; }
        set { _MBL_Prepaid_Collect = value; }
    }
    private bool _HBL_Prepaid_Collect = false;
    public bool HBL_Prepaid_Collect
    {
        get { return _HBL_Prepaid_Collect; }
        set { _HBL_Prepaid_Collect = value; }
    }
    private int _BLID = 0;
    public int BLID
    {
        get { return _BLID; }
        set { _BLID = value; }
    }
    private int _ttoID = 0;
    public int ttoID
    {
        get { return _ttoID; }
        set { _ttoID = value; }
    }
    private string _Vapor = "";
    public string Vapor
    {
        get { return _Vapor; }
        set { _Vapor = value; }
    }
    private string _Peso = "";
    public string Peso
    {
        get { return _Peso; }
        set { _Peso = value; }
    }
    private string _Volumen = "";
    public string Volumen
    {
        get { return _Volumen; }
        set { _Volumen = value; }
    }
    private string _Paquetes = "";
    public string Paquetes
    {
        get { return _Paquetes; }
        set { _Paquetes = value; }
    }
    private string _Paquetes2 = "";
    public string Paquetes2
    {
        get { return _Paquetes2; }
        set { _Paquetes2 = value; }
    }
    private int _Agente = 0;
    public int Agente
    {
        get { return _Agente; }
        set { _Agente = value; }
    }
    private int _Shipper = 0;
    public int Shipper
    {
        get { return _Shipper; }
        set { _Shipper = value; }
    }
    private int _Consignatario = 0;
    public int Consignatario
    {
        get { return _Consignatario; }
        set { _Consignatario = value; }
    }
    private int _Cliente = 0;
    public int Cliente
    {
        get { return _Cliente; }
        set { _Cliente = value; }
    }
    private int _Naviera = 0;
    public int Naviera
    {
        get { return _Naviera; }
        set { _Naviera = value; }
    }
    private int _Comodity = 0;
    public int Comodity
    {
        get { return _Comodity; }
        set { _Comodity = value; }
    }
    private int _No_Piezas = 0;
    public int No_Piezas
    {
        get { return _No_Piezas; }
        set { _No_Piezas = value; }
    }
    private int _Tipo_Paquete = 0;
    public int Tipo_Paquete
    {
        get { return _Tipo_Paquete; }
        set { _Tipo_Paquete = value; }
    }
    private string _Vendedor1 = "";
    public string Vendedor1
    {
        get { return _Vendedor1; }
        set { _Vendedor1 = value; }
    }
    private string _Vendedor2 = "";
    public string Vendedor2
    {
        get { return _Vendedor2; }
        set { _Vendedor2 = value; }
    }
    private string _Fecha_Arribo = "";
    public string Fecha_Arribo
    {
        get { return _Fecha_Arribo; }
        set { _Fecha_Arribo = value; }
    }
    private string _Fecha_Ingreso_Sistema = "";
    public string Fecha_Ingreso_Sistema
    {
        get { return _Fecha_Ingreso_Sistema; }
        set { _Fecha_Ingreso_Sistema = value; }
    }
    private int _viajeID = 0;
    public int viajeID
    {
        get { return _viajeID; }
        set { _viajeID = value; }
    }
    private int _RoutingID = 0;
    public int RoutingID
    {
        get { return _RoutingID; }
        set { _RoutingID = value; }
    }
    private string _Destino_Final = "";
    public string Destino_Final
    {
        get { return _Destino_Final; }
        set { _Destino_Final = value; }
    }
}
