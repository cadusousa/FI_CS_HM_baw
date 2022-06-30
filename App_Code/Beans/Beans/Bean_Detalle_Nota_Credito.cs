using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Detalle_Nota_Credito
/// </summary>
public class Bean_Detalle_Nota_Credito
{
	public Bean_Detalle_Nota_Credito()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _dnc_tnc_id = 0;
    public int dnc_tnc_id
    {
        get { return _dnc_tnc_id; }
        set { _dnc_tnc_id = value; }
    }
    private double _dnc_monto = 0;
    public double dnc_monto
    {
        get { return _dnc_monto; }
        set { _dnc_monto = value; }
    }
    private int _dnc_rub_id = 0;
    public int dnc_rub_id
    {
        get { return _dnc_rub_id; }
        set { _dnc_rub_id = value; }
    }
    private double _dnc_monto_equivalente = 0;
    public double dnc_monto_equivalente
    {
        get { return _dnc_monto_equivalente; }
        set { _dnc_monto_equivalente = value; }
    }
    private int _dnc_moneda_id = 0;
    public int dnc_moneda_id
    {
        get { return _dnc_moneda_id; }
        set { _dnc_moneda_id = value; }
    }
    private int _dnc_ted_id = 0;
    public int dnc_ted_id
    {
        get { return _dnc_ted_id; }
        set { _dnc_ted_id = value; }
    }
    private int _dnc_tre_id = 0;
    public int dnc_tre_id
    {
        get { return _dnc_tre_id; }
        set { _dnc_tre_id = value; }
    }
    private int _dnc_str_id = 0;
    public int dnc_str_id
    {
        get { return _dnc_str_id; }
        set { _dnc_str_id = value; }
    }
    private int _dnc_tts_id = 0;
    public int dnc_tts_id
    {
        get { return _dnc_tts_id; }
        set { _dnc_tts_id = value; }
    }
    private double _dnc_montosinimpuesto = 0;
    public double dnc_montosinimpuesto
    {
        get { return _dnc_montosinimpuesto; }
        set { _dnc_montosinimpuesto = value; }
    }
    private double _dnc_impuesto = 0;
    public double dnc_impuesto
    {
        get { return _dnc_impuesto; }
        set { _dnc_impuesto = value; }
    }
    private double _dnc_montosinimpuesto_equivalente = 0;
    public double dnc_montosinimpuesto_equivalente
    {
        get { return _dnc_montosinimpuesto_equivalente; }
        set { _dnc_montosinimpuesto_equivalente = value; }
    }
    private double _dnc_impuesto_equivalente = 0;
    public double dnc_impuesto_equivalente
    {
        get { return _dnc_impuesto_equivalente; }
        set { _dnc_impuesto_equivalente = value; }
    }
}
