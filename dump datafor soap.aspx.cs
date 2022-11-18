using IntakeSheet.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO.Compression;
using System.Xml;
using System.Reflection;
using System.Globalization;
using IntakeSheet.DAL;

public partial class dump_datafor_soap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            try
            {


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select * from tblsoap", con);

                    cmd.CommandType = CommandType.Text;
                    con.Open();
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    con.Close();
                    string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                    string updatequery = string.Empty;
                    int counter = 0;
                    foreach (DataRow row in dt.Rows)
                    {

                        string objective = string.Empty;
                        string printobjective = string.Empty;
                        int Soapid = Convert.ToInt32(row["ID"]);
                        objective = Convert.ToString(row["Objective"]);
                        string[] bodyparts = objective.Split('$');
                        string BodyPart = string.Empty;
                        string BodyPartPrint = string.Empty;

                        #region Printbodypart
                        foreach (var item in bodyparts)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                string bodypart = item.Split('|')[0];
                                switch (bodypart)
                                {
                                    case "Cervical":
                                        string selected = string.Empty;
                                        string selectedPrintIncrease = string.Empty;
                                        string selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionNeck-Increase"))
                                        { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionNeck-Decrease"))
                                        { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|extensionNeck-Increase"))
                                        { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionNeck-Decrease"))
                                        { selectedPrintDecrease += " extension,"; }

                                        if (item.Contains("|tilttoleftNeck-Increase"))
                                        { selectedPrintIncrease += " tilt to left,"; }
                                        if (item.Contains("|tilttoleftNeck-Decrease"))
                                        { selectedPrintDecrease += " tilt to left,"; }

                                        if (item.Contains("|tilttorightNeck-Increase"))
                                        { selectedPrintIncrease += " tilt to right,"; }
                                        if (item.Contains("|tilttorightNeck-Decrease"))
                                        { selectedPrintDecrease += " tilt to right,"; }

                                        if (item.Contains("|leftrotationNeck-Increase"))
                                        { selectedPrintIncrease += " left rotation,"; }
                                        if (item.Contains("|leftrotationNeck-Decrease"))
                                        { selectedPrintDecrease += " left rotation,"; }

                                        if (item.Contains("|RightrotationNeck-Increase"))
                                        { selectedPrintIncrease += " right rotation,"; }
                                        if (item.Contains("|RightrotationNeck-Decrease"))
                                        { selectedPrintDecrease += " right rotation,"; }

                                        if (item.Contains("|spasmNeck-chk")) { }

                                        BodyPart += "$Cervical" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "C/S: ";
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                            //if (item.Contains("|spasmNeck-chk"))
                                            //{ BodyPartPrint += "spasm present,"; }
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += " Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                            BodyPartPrint += "spasm present,"; 
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "lumber":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionLumber-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLumber-Decrease")) { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|extensionLumber-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionLumber-Decrease")) { selectedPrintDecrease += " extension,"; }

                                        if (item.Contains("|tilttoleftLumber-Increase")) { selectedPrintIncrease += " tilt to left,"; }
                                        if (item.Contains("|tilttoleftLumber-Decrease")) { selectedPrintDecrease += " tilt to left,"; }

                                        if (item.Contains("|tilttorightLumber-Increase")) { selectedPrintIncrease += " tilt to right,"; }
                                        if (item.Contains("|tilttorightLumber-Decrease")) { selectedPrintDecrease += " tilt to right,"; }

                                        if (item.Contains("|spasmLumber-chk")) { }

                                        BodyPart += "$lumber" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L/S: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                            //if (item.Contains("|spasmLumber-chk"))
                                            //{ BodyPartPrint += "spasm present,"; }
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += " Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                            BodyPartPrint += "spasm present,"; 
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        break;
                                    case "thoracic":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|mildlyTrousic-Increase")) { selectedPrintIncrease += " ROM is mildly increase,"; }
                                        if (item.Contains("|mildlyTrousic-Decrease")) { selectedPrintDecrease += " ROM is mildly decreased,"; }

                                        if (item.Contains("|moderatelyTrousic-Increase")) { selectedPrintIncrease += " ROM is moderately increase,"; }
                                        if (item.Contains("|moderatelyTrousic-Decrease")) { selectedPrintDecrease += " ROM is moderately decreased,"; }

                                        if (item.Contains("|severelyTrousic-Increase")) { selectedPrintIncrease += " ROM is severely increase,"; }
                                        if (item.Contains("|severelyTrousic-Decrease")) { selectedPrintDecrease += " ROM is severely decreased,"; }

                                        if (item.Contains("|spasmTrousic-chk")) { }
                                        BodyPart += "$thoracic" + selected;

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "T/S: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += selectedPrintIncrease.Replace(",", ", ");
                                            //if (item.Contains("|spasmTrousic-chk"))
                                            //{ BodyPartPrint += "spasm present,"; }
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ").Trim();
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += selectedPrintDecrease.Replace(",", ", ");
                                             BodyPartPrint += "spasm present,"; 
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ").Trim();
                                        }

                                        break;
                                    case "R. Shoulder":

                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionRSh-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionRSh-Decrease")) { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|abductionRSh-Increase")) { selectedPrintIncrease += " abduction,"; }
                                        if (item.Contains("|abductionRSh-Decrease")) { selectedPrintDecrease += " abduction,"; }

                                        if (item.Contains("|trotationRSh-Increase")) { selectedPrintIncrease += " internal rotation,"; }
                                        if (item.Contains("|trotationRSh-Decrease")) { selectedPrintDecrease += " internal rotation,"; }

                                        if (item.Contains("|extrotationRSh-Increase")) { selectedPrintIncrease += " external rotation,"; }
                                        if (item.Contains("|extrotationRSh-Decrease")) { selectedPrintDecrease += " external rotation,"; }

                                        BodyPart += "$R. Shoulder" + selected;

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "R. Shoulder: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        break;
                                    case "L. Shoulder":

                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionRLh-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLSh-Decrease")) { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|abductionLSh-Increase")) { selectedPrintIncrease += " abduction,"; }
                                        if (item.Contains("|abductionLSh-Decrease")) { selectedPrintDecrease += " abduction,"; }

                                        if (item.Contains("|trotationLSh-Increase")) { selectedPrintIncrease += " internal rotation,"; }
                                        if (item.Contains("|trotationLSh-Decrease")) { selectedPrintDecrease += " internal rotation,"; }

                                        if (item.Contains("|extrotationLSh-Increase")) { selectedPrintIncrease += " external rotation,"; }
                                        if (item.Contains("|extrotationLSh-Decrease")) { selectedPrintDecrease += " external rotation,"; }

                                        BodyPart += "$L. Shoulder" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L. Shoulder: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "L. Knee":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionLK-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLK-Decrease")) { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|extensionLK-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionLK-Decrease")) { selectedPrintDecrease += " extension,"; }

                                        BodyPart += "$L. Knee" + selected;

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L. Knee: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "R. Knee":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionLK-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLK-Decrease")) { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|extensionLK-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionLK-Decrease")) { selectedPrintDecrease += " extension,"; }

                                        BodyPart += "$R. Knee" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "R. Knee: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "L. Elbow":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionLE-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLE-Decrease")) { selectedPrintDecrease += " flexion,"; }



                                        if (item.Contains("|extensionLE-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionLE-Decrease")) { selectedPrintDecrease += " extension,"; }



                                        BodyPart += "$L. Elbow" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L. Elbow: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        break;
                                    case "R. Elbow":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionRE-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionRE-Decrease")) { selectedPrintDecrease += " flexion,"; }



                                        if (item.Contains("|extensionRE-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionRE-Decrease")) { selectedPrintDecrease += " extension,"; }



                                        BodyPart += "$R. Elbow" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "R. Elbow: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "L. Wrist":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionLW-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLW-Decrease")) { selectedPrintDecrease += " flexion,"; }



                                        if (item.Contains("|dorsiflexionLW-Increase")) { selectedPrintIncrease += " dorsiflexion,"; }
                                        if (item.Contains("|dorsiflexionLW-Decrease")) { selectedPrintDecrease += " dorsiflexion,"; }



                                        if (item.Contains("|ulnardeviationLW-Increase")) { selectedPrintIncrease += " ulnar deviation,"; }
                                        if (item.Contains("|ulnardeviationLW-Decrease")) { selectedPrintDecrease += " ulnar deviation,"; }



                                        if (item.Contains("|radialdeviationLW-Increase")) { selectedPrintIncrease += " radial deviation,"; }
                                        if (item.Contains("|radialdeviationLW-Decrease")) { selectedPrintDecrease += " radial deviation,"; }



                                        BodyPart += "$L. Wrist" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L. Wrist: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }


                                        break;
                                    case "R. Wrist":

                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionRW-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionRW-Decrease")) { selectedPrintDecrease += " flexion,"; }



                                        if (item.Contains("|dorsiflexionRW-Increase")) { selectedPrintIncrease += " dorsiflexion,"; }
                                        if (item.Contains("|dorsiflexionRW-Decrease")) { selectedPrintDecrease += " dorsiflexion,"; }



                                        if (item.Contains("|ulnardeviationRW-Increase")) { selectedPrintIncrease += " ulnar deviation,"; }
                                        if (item.Contains("|ulnardeviationRW-Decrease")) { selectedPrintDecrease += " ulnar deviation,"; }



                                        if (item.Contains("|radialdeviationRW-Increase")) { selectedPrintIncrease += " radial deviation,"; }
                                        if (item.Contains("|radialdeviationRW-Decrease")) { selectedPrintDecrease += " radial deviation,"; }



                                        BodyPart += "$R. Wrist" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "R. Wrist: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "L. Hip":

                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionLHIP-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionLHIP-Decrease")) { selectedPrintDecrease += " flexion,"; }



                                        if (item.Contains("|extensionLHIP-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionLHIP-Decrease")) { selectedPrintDecrease += " extension,"; }



                                        if (item.Contains("|abductionLHIP-Increase")) { selectedPrintIncrease += " abduction,"; }
                                        if (item.Contains("|abductionLHIP-Decrease")) { selectedPrintDecrease += " abduction,"; }



                                        if (item.Contains("|introtationLHIP-Increase")) { selectedPrintIncrease += " internal rotation,"; }
                                        if (item.Contains("|introtationLHIP-Decrease")) { selectedPrintDecrease += " internal rotation,"; }



                                        BodyPart += "$L. Hip" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L. Hip: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }


                                        break;
                                    case "R. Hip":
                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|flexionRHIP-Increase")) { selectedPrintIncrease += " flexion,"; }
                                        if (item.Contains("|flexionRHIP-Decrease")) { selectedPrintDecrease += " flexion,"; }

                                        if (item.Contains("|extensionRHIP-Increase")) { selectedPrintIncrease += " extension,"; }
                                        if (item.Contains("|extensionRHIP-Decrease")) { selectedPrintDecrease += " extension,"; }

                                        if (item.Contains("|abductionRHIP-Increase")) { selectedPrintIncrease += " abduction,"; }
                                        if (item.Contains("|abductionRHIP-Decrease")) { selectedPrintDecrease += " abduction,"; }

                                        if (item.Contains("|introtationRHIP-Increase")) { selectedPrintIncrease += " internal rotation,"; }
                                        if (item.Contains("|introtationRHIP-Decrease")) { selectedPrintDecrease += " internal rotation,"; }

                                        BodyPart += "$R. Hip" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "R. Hip: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }


                                        break;
                                    case "L. Ankle":

                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|dorsiflexionLA-Increase")) { selectedPrintIncrease += " dorsiflexion,"; }
                                        if (item.Contains("|dorsiflexionLA-Decrease")) { selectedPrintDecrease += " dorsiflexion,"; }



                                        if (item.Contains("|plantarflexionLA-Increase")) { selectedPrintIncrease += " plantar flexion,"; }
                                        if (item.Contains("|plantarflexionLA-Decrease")) { selectedPrintDecrease += " plantar flexion,"; }



                                        if (item.Contains("|inversionLA-Increase")) { selectedPrintIncrease += " inversion,"; }
                                        if (item.Contains("|inversionLA-Decrease")) { selectedPrintDecrease += " inversion,"; }



                                        if (item.Contains("|eversionLA-Increase")) { selectedPrintIncrease += " eversion,"; }
                                        if (item.Contains("|eversionLA-Decrease")) { selectedPrintDecrease += " eversion,"; }



                                        BodyPart += "$L. Ankle" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "L. Ankle: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }

                                        break;
                                    case "R. Ankle":

                                        selected = string.Empty;
                                        selectedPrintIncrease = string.Empty;
                                        selectedPrintDecrease = string.Empty;

                                        if (item.Contains("|dorsiflexionRA-Increase")) { selectedPrintIncrease += " dorsiflexion,"; }
                                        if (item.Contains("|dorsiflexionRA-Decrease")) { selectedPrintDecrease += " dorsiflexion,"; }

                                        if (item.Contains("|plantarflexionRA-Increase")) { selectedPrintIncrease += " plantar flexion,"; }
                                        if (item.Contains("|plantarflexionRA-Decrease")) { selectedPrintDecrease += " plantar flexion,"; }

                                        if (item.Contains("|inversionRA-Increase")) { selectedPrintIncrease += " inversion,"; }
                                        if (item.Contains("|inversionRA-Decrease")) { selectedPrintDecrease += " inversion,"; }



                                        if (item.Contains("|eversionRA-Increase")) { selectedPrintIncrease += " eversion,"; }
                                        if (item.Contains("|eversionRA-Decrease")) { selectedPrintDecrease += " eversion,"; }



                                        BodyPart += "$R. Ankle" + selected;
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease) || !string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += System.Environment.NewLine + "R. Ankle: ";
                                        }

                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint += "Increase:" + selectedPrintIncrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintIncrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint += "Decrease:" + selectedPrintDecrease.Replace(",", ", ");
                                        }
                                        if (!string.IsNullOrEmpty(selectedPrintDecrease))
                                        {
                                            BodyPartPrint = ReplaceLastOccurrence(BodyPartPrint, ",", " ");
                                        }
                                        break;
                                }

                            }
                        }
                        #endregion

                        DBHelperClass dB = new DBHelperClass();
                        //string ieid = string.Empty, soapid = string.Empty;

                        //int val = dB.executeQuery("update tblsoap set PrintObjective= '" + BodyPartPrint + "' where ID=" + Soapid);
                        message += Environment.NewLine + "update tblsoap set PrintObjective='" + BodyPartPrint + "' where ID=" + Soapid + ";";

                        counter++;
                    }
                    message += "Counter:" + counter; 
                    string path = HttpContext.Current.Server.MapPath("~/SoapRecordsLog.txt");
                    using (StreamWriter writer = new StreamWriter(path, true))
                    {
                        writer.WriteLine(message);
                        writer.Close();
                    }


                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
    {
        int place = Source.LastIndexOf(Find);

        if (place == -1)
            return Source;

        string result = Source.Remove(place, Find.Length).Insert(place, Replace);
        return result;
    }
}
