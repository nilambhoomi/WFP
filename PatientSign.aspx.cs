using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class PatientSign : System.Web.UI.Page
{
    protected void Save(object sender, EventArgs e)
    {
        //Read the Base64 string from Hidden Field.
        string base64 = Request.Form[hfImageData.UniqueID].Split(',')[1];

        //Convert Base64 string to Byte Array.
        byte[] bytes = Convert.FromBase64String(base64);

        //Save the Byte Array as Image File.
        string filePath = string.Format("~/PatientSign/{0}.pfx", Path.GetRandomFileName());
        File.WriteAllBytes(Server.MapPath(filePath), bytes);
    }
}