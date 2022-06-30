using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Cargos
/// </summary>
public class Bean_Cargos
{
	public Bean_Cargos()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _Cargo_ID = 0;
    public int Cargo_ID
    {
        get { return _Cargo_ID; }
        set { _Cargo_ID = value; }
    }
    private int _Cargo_Terceros_ID = 0;
    public int Cargo_Terceros_ID
    {
        get { return _Cargo_Terceros_ID; }
        set { _Cargo_Terceros_ID = value; }
    }
    private int _Costo_Terceros_ID = 0;
    public int Costo_Terceros_ID
    {
        get { return _Costo_Terceros_ID; }
        set { _Costo_Terceros_ID = value; }
    }
    private int _Cargo_BLID = 0;
    public int Cargo_BLID
    {
        get { return _Cargo_BLID; }
        set { _Cargo_BLID = value; }
    }
    private int _Cargo_Rub_ID = 0;
    public int Cargo_Rub_ID
    {
        get { return _Cargo_Rub_ID; }
        set { _Cargo_Rub_ID = value; }
    }
    private int _Cargo_Servicio_ID = 0;
    public int Cargo_Servicio_ID
    {
        get { return _Cargo_Servicio_ID; }
        set { _Cargo_Servicio_ID = value; }
    }
    private int _Cargo_Moneda_Trafico_ID = 0;
    public int Cargo_Moneda_Trafico_ID
    {
        get { return _Cargo_Moneda_Trafico_ID; }
        set { _Cargo_Moneda_Trafico_ID = value; }
    }
    private int _Cargo_Moneda_ID = 0;
    public int Cargo_Moneda_ID
    {
        get { return _Cargo_Moneda_ID; }
        set { _Cargo_Moneda_ID = value; }
    }
    private double _Cargo_Monto = 0;
    public double Cargo_Monto
    {
        get { return _Cargo_Monto; }
        set { _Cargo_Monto = value; }
    }
    private string _Cargo_Tipo_BL = "";
    public string Cargo_Tipo_BL
    {
        get { return _Cargo_Tipo_BL; }
        set { _Cargo_Tipo_BL = value; }
    }
    private int _Factura_ID = 0;
    public int Factura_ID
    {
        get { return _Factura_ID; }
        set { _Factura_ID = value; }
    }
    private int _Tipo_Documento = 0;
    public int Tipo_Documento
    {
        get { return _Tipo_Documento; }
        set { _Tipo_Documento = value; }
    }
    private int _Tipo_Cobro = 0;
    public int Tipo_Cobro
    {
        get { return _Tipo_Cobro; }
        set { _Tipo_Cobro = value; }
    }
    private int _Tipo_Cargo = 0;
    public int Tipo_Cargo
    {
        get { return _Tipo_Cargo; }
        set { _Tipo_Cargo = value; }
    }
    private int _ID_Intercompany = 0;
    public int ID_Intercompany
    {
        get { return _ID_Intercompany; }
        set { _ID_Intercompany = value; }
    }
    private int _ID_Grupo = 0;
    public int ID_Grupo
    {
        get { return _ID_Grupo; }
        set { _ID_Grupo = value; }
    }
    private int _ID_Tipo_Persona = 0;
    public int ID_Tipo_Persona
    {
        get { return _ID_Tipo_Persona; }
        set { _ID_Tipo_Persona = value; }
    }
    private string _Cargo_Moneda_Simbolo = "";
    public string Cargo_Moneda_Simbolo
    {
        get { return _Cargo_Moneda_Simbolo; }
        set { _Cargo_Moneda_Simbolo = value; }
    }
    private double _Valor_Prepaid = 0;
    public double Valor_Prepaid
    {
        get { return _Valor_Prepaid; }
        set { _Valor_Prepaid = value; }
    }
    private double _Valor_Collect = 0;
    public double Valor_Collect
    {
        get { return _Valor_Collect; }
        set { _Valor_Collect = value; }
    }
    private int _Local_Internacional = 0;
    public int ID_Local_Internacional
    {
        get { return _Local_Internacional; }
        set { _Local_Internacional = value; }
    }
}