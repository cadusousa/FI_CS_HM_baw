using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Costos
/// </summary>
public class Bean_Costos
{
	public Bean_Costos()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _Costo_ID = 0;
    public int Costo_ID
    {
        get { return _Costo_ID; }
        set { _Costo_ID = value; }
    }
    private int _Costo_Rub_ID = 0;
    public int Costo_Rub_ID
    {
        get { return _Costo_Rub_ID; }
        set { _Costo_Rub_ID = value; }
    }
    private int _Costo_Servicio_ID = 0;
    public int Costo_Servicio_ID
    {
        get { return _Costo_Servicio_ID; }
        set { _Costo_Servicio_ID = value; }
    }
    private int _Costo_Moneda_ID = 0;
    public int Costo_Moneda_ID
    {
        get { return _Costo_Moneda_ID; }
        set { _Costo_Moneda_ID = value; }
    }
    private string _Costo_Moneda = "";
    public string Costo_Moneda
    {
        get { return _Costo_Moneda; }
        set { _Costo_Moneda = value; }
    }
    private double _Costo_Monto = 0;
    public double Costo_Monto
    {
        get { return _Costo_Monto; }
        set { _Costo_Monto = value; }
    }
    private int _Costo_Proveedor_ID = 0;
    public int Costo_Proveedor_ID
    {
        get { return _Costo_Proveedor_ID; }
        set { _Costo_Proveedor_ID = value; }
    }
    private int _Costo_Tipo_Proveedor_ID = 0;
    public int Costo_Tipo_Proveedor_ID
    {
        get { return _Costo_Tipo_Proveedor_ID; }
        set { _Costo_Tipo_Proveedor_ID = value; }
    }
    private string _Costo_Orden_Compra = "";
    public string Costo_Orden_Compra
    {
        get { return _Costo_Orden_Compra; }
        set { _Costo_Orden_Compra = value; }
    }
    private string _Costo_Referencia = "";
    public string Costo_Referencia
    {
        get { return _Costo_Referencia; }
        set { _Costo_Referencia = value; }
    }
    private int _Costo_Pago_Terceros = 0;
    public int Costo_Pago_Terceros
    {
        get { return _Costo_Pago_Terceros; }
        set { _Costo_Pago_Terceros = value; }
    }
    private int _Costo_Es_Afecto = 0;
    public int Costo_Es_Afecto
    {
        get { return _Costo_Es_Afecto; }
        set { _Costo_Es_Afecto = value; }
    }
    private bool _Costo_Provisionado = false;
    public bool Costo_Provisionado
    {
        get { return _Costo_Provisionado; }
        set { _Costo_Provisionado = value; }
    }
    private string _Costo_Nombre_Proveedor = "";
    public string Costo_Nombre_Proveedor
    {
        get { return _Costo_Nombre_Proveedor; }
        set { _Costo_Nombre_Proveedor = value; }
    }
    private string _Costo_Documento_House = "";
    public string Costo_Documento_House
    {
        get { return _Costo_Documento_House; }
        set { _Costo_Documento_House = value; }
    }
    private string _Costo_Documento_Master = "";
    public string Costo_Documento_Master
    {
        get { return _Costo_Documento_Master; }
        set { _Costo_Documento_Master = value; }
    }
    private string _Costo_Routing = "";
    public string Costo_Routing
    {
        get { return _Costo_Routing; }
        set { _Costo_Routing = value; }
    }
}
