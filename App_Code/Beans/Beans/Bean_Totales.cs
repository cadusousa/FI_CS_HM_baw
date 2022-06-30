using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Bean_Totales
/// </summary>
public class Bean_Totales
{
	public Bean_Totales()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    private double _Total = 0;
    public double Total
    {
        get { return _Total; }
        set { _Total = value; }
    }
    private double _SubTotal = 0;
    public double SubTotal
    {
        get { return _SubTotal; }
        set { _SubTotal = value; }
    }
    private double _Impuesto = 0;
    public double Impuesto
    {
        get { return _Impuesto; }
        set { _Impuesto = value; }
    }
    private double _Total_Equivalente = 0;
    public double Total_Equivalente
    {
        get { return _Total_Equivalente; }
        set { _Total_Equivalente = value; }
    }
    private double _SubTotal_Equivalente = 0;
    public double SubTotal_Equivalente
    {
        get { return _SubTotal_Equivalente; }
        set { _SubTotal_Equivalente = value; }
    }
    private double _Impuesto_Equivalente = 0;
    public double Impuesto_Equivalente
    {
        get { return _Impuesto_Equivalente; }
        set { _Impuesto_Equivalente = value; }
    }
    private double _Afecto = 0;
    public double Afecto
    {
        get { return _Afecto; }
        set { _Afecto = value; }
    }
    private double _No_Afecto = 0;
    public double No_Afecto
    {
        get { return _No_Afecto; }
        set { _No_Afecto = value; }
    }
}