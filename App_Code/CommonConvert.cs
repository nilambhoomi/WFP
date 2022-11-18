using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommonConvert
/// </summary>
public class CommonConvert
{
    public CommonConvert()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static bool ToBoolean(dynamic variable)
    {
        try
        {
            // if (Convert.IsDBNull(variable) ||variable == "") return variable = false;
            if (variable != null || variable != "")
            { return Convert.ToBoolean(variable); }
            else
            { return false; }
        }
        catch (Exception ex)
        {
			Convert.ToString(ex);
            return false;
			
        }
    }
}