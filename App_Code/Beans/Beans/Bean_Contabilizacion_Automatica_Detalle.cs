using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Contabilizacion_Automatica_Detalle
/// </summary>
public class Bean_Contabilizacion_Automatica_Detalle
{
	public Bean_Contabilizacion_Automatica_Detalle()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private int _tcad_id = 0;
    public int tcad_id
    {
        get { return _tcad_id; }
        set { _tcad_id = value; }
    }
    private int _tcad_ttr_id = 0;
    public int tcad_ttr_id
    {
        get { return _tcad_ttr_id; }
        set { _tcad_ttr_id = value; }
    }
    private int _tcad_tpi_id = 0;
    public int tcad_tpi_id
    {
        get { return _tcad_tpi_id; }
        set { _tcad_tpi_id = value; }
    }
    private int _tcad_contabilidad_id = 0;
    public int tcad_contabilidad_id
    {
        get { return _tcad_contabilidad_id; }
        set { _tcad_contabilidad_id = value; }
    }
    private int _tcad_moneda_id_origen = 0;
    public int tcad_moneda_id_origen
    {
        get { return _tcad_moneda_id_origen; }
        set { _tcad_moneda_id_origen = value; }
    }
    private int _tcad_moneda_id_destino = 0;
    public int tcad_moneda_id_destino
    {
        get { return _tcad_moneda_id_destino; }
        set { _tcad_moneda_id_destino = value; }
    }
    private bool _tcad_tiene_hijos = false;
    public bool tcad_tiene_hijos
    {
        get { return _tcad_tiene_hijos; }
        set { _tcad_tiene_hijos = value; }
    }
    private int _tcad_id_padre = 0;
    public int tcad_id_padre
    {
        get { return _tcad_id_padre; }
        set { _tcad_id_padre = value; }
    }
    private int _tcad_estado = 0;
    public int tcad_estado
    {
        get { return _tcad_estado; }
        set { _tcad_estado = value; }
    }
    private int _tcad_tca_id = 0;
    public int tcad_tca_id
    {
        get { return _tcad_tca_id; }
        set { _tcad_tca_id = value; }
    }
    private bool _tcad_automatizar = false;
    public bool tcad_automatizar
    {
        get { return _tcad_automatizar; }
        set { _tcad_automatizar = value; }
    }
    private string _tcad_descripcion = "";
    public string tcad_descripcion
    {
        get { return _tcad_descripcion; }
        set { _tcad_descripcion = value; }
    }
    private int _tcad_tct_id = 0;
    public int tcad_tct_id
    {
        get { return _tcad_tct_id; }
        set { _tcad_tct_id = value; }
    }
    private bool _tcad_terceros = false;
    public bool tcad_terceros
    {
        get { return _tcad_terceros; }
        set { _tcad_terceros = value; }
    }
    private int _tcad_suc_id = 0;
    public int tcad_suc_id
    {
        get { return _tcad_suc_id; }
        set { _tcad_suc_id = value; }
    }
    private int _tcad_operacion_id = 0;
    public int tcad_operacion_id
    {
        get { return _tcad_operacion_id; }
        set { _tcad_operacion_id = value; }
    }
    private string _tcad_serie = "";
    public string tcad_serie
    {
        get { return _tcad_serie; }
        set { _tcad_serie = value; }
    }
    private bool _tcad_genera_partida = false;
    public bool tcad_genera_partida
    {
        get { return _tcad_genera_partida; }
        set { _tcad_genera_partida = value; }
    }
    private bool _tcad_transaccion_automatizada = false;
    public bool tcad_transaccion_automatizada
    {
        get { return _tcad_transaccion_automatizada; }
        set { _tcad_transaccion_automatizada = value; }
    }
}