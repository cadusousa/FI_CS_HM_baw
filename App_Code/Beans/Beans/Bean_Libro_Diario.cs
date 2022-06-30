using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Libro_Diario
/// </summary>
public class Bean_Libro_Diario
{
	public Bean_Libro_Diario()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tdi_transaction_id = 0;
    public int tdi_transaction_id
    {
        get { return _tdi_transaction_id; }
        set { _tdi_transaction_id = value; }
    }
    private string _tdi_cue_id = "";
    public string tdi_cue_id
    {
        get { return _tdi_cue_id; }
        set { _tdi_cue_id = value; }
    }
    private string _tdi_fecha = "";
    public string tdi_fecha
    {
        get { return _tdi_fecha; }
        set { _tdi_fecha = value; }
    }
    private string _tdi_hora = "";
    public string tdi_hora
    {
        get { return _tdi_hora; }
        set { _tdi_hora = value; }
    }
    private string _tdi_usu_id = "";
    public string tdi_usu_id
    {
        get { return _tdi_usu_id; }
        set { _tdi_usu_id = value; }
    }
    private double _tdi_debe = 0;
    public double tdi_debe
    {
        get { return _tdi_debe; }
        set { _tdi_debe = value; }
    }
    private double _tdi_haber = 0;
    public double tdi_haber
    {
        get { return _tdi_haber; }
        set { _tdi_haber = value; }
    }
    private int _tdi_ttr_id = 0;
    public int tdi_ttr_id
    {
        get { return _tdi_ttr_id; }
        set { _tdi_ttr_id = value; }
    }
    private int _tdi_ref_id = 0;
    public int tdi_ref_id
    {
        get { return _tdi_ref_id; }
        set { _tdi_ref_id = value; }
    }
    private int _tdi_pai_id = 0;
    public int tdi_pai_id
    {
        get { return _tdi_pai_id; }
        set { _tdi_pai_id = value; }
    }
    private int _tdi_suc_id = 0;
    public int tdi_suc_id
    {
        get { return _tdi_suc_id; }
        set { _tdi_suc_id = value; }
    }
    private int _tdi_persona_id = 0;
    public int tdi_persona_id
    {
        get { return _tdi_persona_id; }
        set { _tdi_persona_id = value; }
    }
    private int _tdi_tpi_id = 0;
    public int tdi_tpi_id
    {
        get { return _tdi_tpi_id; }
        set { _tdi_tpi_id = value; }
    }
    private decimal _tdi_tipo_cambio = 0;
    public decimal tdi_tipo_cambio
    {
        get { return _tdi_tipo_cambio; }
        set { _tdi_tipo_cambio = value; }
    }
    private int _tdi_moneda_id = 0;
    public int tdi_moneda_id
    {
        get { return _tdi_moneda_id; }
        set { _tdi_moneda_id = value; }
    }
    private string _tdi_no_partida = "";
    public string tdi_no_partida
    {
        get { return _tdi_no_partida; }
        set { _tdi_no_partida = value; }
    }
    private string _tdi_fecha_documento = "";
    public string tdi_fecha_documento
    {
        get { return _tdi_fecha_documento; }
        set { _tdi_fecha_documento = value; }
    }
    private int _tdi_tcon_id = 0;
    public int tdi_tcon_id
    {
        get { return _tdi_tcon_id; }
        set { _tdi_tcon_id = value; }
    }
    private int _tdi_ttt_id = 0;
    public int tdi_ttt_id
    {
        get { return _tdi_ttt_id; }
        set { _tdi_ttt_id = value; }
    }
    private double _tdi_debe_equivalente = 0;
    public double tdi_debe_equivalente
    {
        get { return _tdi_debe_equivalente; }
        set { _tdi_debe_equivalente = value; }
    }
    private double _tdi_haber_equivalente = 0;
    public double tdi_haber_equivalente
    {
        get { return _tdi_haber_equivalente; }
        set { _tdi_haber_equivalente = value; }
    }
    private string _tdi_serie = "";
    public string tdi_serie
    {
        get { return _tdi_serie; }
        set { _tdi_serie = value; }
    }
    private string _tdi_correlativo = "";
    public string tdi_correlativo
    {
        get { return _tdi_correlativo; }
        set { _tdi_correlativo = value; }
    }
    private string _tdi_observacion = "";
    public string tdi_observacion
    {
        get { return _tdi_observacion; }
        set { _tdi_observacion = value; }
    }
    private string _tdi_nombre = "";
    public string tdi_nombre
    {
        get { return _tdi_nombre; }
        set { _tdi_nombre = value; }
    }
}