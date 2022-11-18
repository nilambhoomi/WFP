using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using IntakeSheet;
using System.Configuration;
using System.IO;
using log4net;

public partial class EditFuLowback : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Lowback";
    DBHelperClass gDbhelperobj = new DBHelperClass();
    ILog log = log4net.LogManager.GetLogger(typeof(EditFuLowback));


    protected void Page_Load(object sender, EventArgs e)
    {
        Session["PageName"] = "Lowback";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            checkTP();
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {
                bindDropdown();
                //BindROM();
                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpLowBack WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpLowBack WHERE PatientIE_ID= " + _CurIEid + "");
                SqlCommand cm1 = new SqlCommand(query1, cn);
                SqlDataAdapter IEda = new SqlDataAdapter(cm1);
                cn.Open();
                DataSet IEds = new DataSet();
                IEda.Fill(IEds);
                cn.Close();
                DataRow FUrw = FUds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("FuCount") == 0);
                DataRow IErw = IEds.Tables[0].AsEnumerable().FirstOrDefault(tt => tt.Field<int>("IECount") == 0);
                if (FUrw == null)
                {

                    PopulateUI(_FuId);
                    BindDCDataGrid();
                    BindDataGrid();
                    // row exists
                    // PopulateUIDefaults();
                    //BindDataGrid();
                }
                else if (IErw == null)
                {
                    PopulateIEUI(_CurIEid);
                    BindDCDataGrid();
                    BindDataGrid();
                }
                else
                {

                    //_CurIEid = Session["PatientIE_ID"].ToString();
                    //patientID.Value = Session["PatientIE_ID"].ToString();
                    PopulateUIDefaults();
                    BindDataGrid();
                    //PopulateUI(_CurIEid);
                    //BindDCDataGrid();
                    //BindDataGrid();
                }

            }
            else
            {
                Response.Redirect("EditFU.aspx");
            }
        }

        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuLowback for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }
    public string SaveUI(string FuID, string ieMode, bool bpChecked)
    {
        long _FuID = Convert.ToInt64(FuID);
        string _ieMode = "";
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpLowBack WHERE PatientFU_ID = " + FuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && bpChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && bpChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && bpChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["PatientFU_ID"] = _FuID;
            TblRow["PainScale"] = txtPainScale.Text.ToString();
            TblRow["Constant"] = chkContent.Checked;
            TblRow["Intermittent"] = chkIntermittent.Checked;
            TblRow["Sharp"] = chkSharp.Checked;
            TblRow["Electric"] = chkelectric.Checked;
            TblRow["Shooting"] = chkshooting.Checked;
            TblRow["Throbbling"] = chkthrobbing.Checked;
            TblRow["Pulsating"] = chkpulsating.Checked;
            TblRow["Dull"] = chkdull.Checked;
            TblRow["Achy"] = chkachy.Checked;
            TblRow["Radiates"] = txtRadiates.Text.ToString();
            TblRow["Numbness"] = chknumbness.Checked;
            TblRow["Tingling"] = chktingling.Checked;
            TblRow["Burning"] = chkBurning.Checked;
            TblRow["BurningTo"] = txtburningto.Text.ToString();
            TblRow["SideRight1"] = chkSideRight1.Checked;
            TblRow["ButtockRight1"] = chkButtockRight1.Checked;
            TblRow["GroinRight1"] = chkGroinRight1.Checked;
            TblRow["HipRight1"] = chkHipRight1.Checked;
            TblRow["ThighRight1"] = chkThighRight1.Checked;
            TblRow["LegRight1"] = chkLegRight1.Checked;
            TblRow["KneeRight1"] = chkKneeRight1.Checked;
            TblRow["AnkleRight1"] = chkAnkleRight1.Checked;
            TblRow["FeetRight1"] = chkFeetRight1.Checked;
            TblRow["ToeRight1"] = chkToeRight1.Checked;
            TblRow["SideLeft1"] = chkSideLeft1.Checked;
            TblRow["ButtockLeft1"] = chkButtockLeft1.Checked;
            TblRow["GroinLeft1"] = chkGroinLeft1.Checked;
            TblRow["HipLeft1"] = chkHipLeft1.Checked;
            TblRow["ThighLeft1"] = chkThighLeft1.Checked;
            TblRow["LegLeft1"] = chkLegLeft1.Checked;
            TblRow["KneeLeft1"] = chkKneeLeft1.Checked;
            TblRow["AnkleLeft1"] = chkAnkleLeft1.Checked;
            TblRow["FeetLeft1"] = chkFeetLeft1.Checked;
            TblRow["ToeLeft1"] = chkToeLeft1.Checked;
            TblRow["SideBilateral1"] = chkSideBilateral1.Checked;
            TblRow["ButtockBilateral1"] = chkButtockBilateral1.Checked;
            TblRow["GroinBilateral1"] = chkGroinBilateral1.Checked;
            TblRow["HipBilateral1"] = chkHipBilateral1.Checked;
            TblRow["ThighBilateral1"] = chkThighBilateral1.Checked;
            TblRow["LegBilateral1"] = chkLegBilateral1.Checked;
            TblRow["KneeBilateral1"] = chkKneeBilateral1.Checked;
            TblRow["AnkleBilateral1"] = chkAnkleBilateral1.Checked;
            TblRow["FeetBilateral1"] = chkFeetBilateral1.Checked;
            TblRow["ToeBilateral1"] = chkToeBilateral1.Checked;
            TblRow["SideLeft2"] = chkSideLeft2.Checked;
            TblRow["ButtockLeft2"] = chkButtockLeft2.Checked;
            TblRow["GroinLeft2"] = chkGroinLeft2.Checked;
            TblRow["HipLeft2"] = chkHipLeft2.Checked;
            TblRow["ThighLeft2"] = chkThighLeft2.Checked;
            TblRow["LegLeft2"] = chkLegLeft2.Checked;
            TblRow["KneeLeft2"] = chkKneeLeft2.Checked;
            TblRow["AnkleLeft2"] = chkAnkleLeft2.Checked;
            TblRow["FeetLeft2"] = chkFeetLeft2.Checked;
            TblRow["ToeLeft2"] = chkToeLeft2.Checked;
            TblRow["SideRight2"] = chkSideRight2.Checked;
            TblRow["ButtockRight2"] = chkButtockRight2.Checked;
            TblRow["GroinRight2"] = chkGroinRight2.Checked;
            TblRow["HipRight2"] = chkHipRight2.Checked;
            TblRow["ThighRight2"] = chkThighRight2.Checked;
            TblRow["LegRight2"] = chkLegRight2.Checked;
            TblRow["SideBilateral2"] = chkSideBilateral2.Checked;
            TblRow["ButtockBilateral2"] = chkButtockBilateral2.Checked;
            TblRow["GroinBilateral2"] = chkGroinBilateral2.Checked;
            TblRow["HipBilateral2"] = chkHipBilateral2.Checked;
            TblRow["ThighBilateral2"] = chkThighBilateral2.Checked;
            TblRow["LegBilateral2"] = chkLegBilateral2.Checked;
            TblRow["KneeBilateral2"] = chkKneeBilateral2.Checked;
            TblRow["AnkleBilateral2"] = chkAnkleBilateral2.Checked;
            TblRow["FeetBilateral2"] = chkFeetBilateral2.Checked;
            TblRow["ToeBilateral2"] = chkToeBilateral2.Checked;
            TblRow["WeeknessIn"] = txtWeeknessIn.Text.ToString();
            TblRow["WorseSitting"] = chkWorseSitting.Checked;
            TblRow["WorseStanding"] = chkWorseStanding.Checked;
            TblRow["WorseLyingDown"] = chkWorseLyingDown.Checked;
            TblRow["WorseMovement"] = chkWorseMovement.Checked;
            TblRow["WorseBending"] = chkWorseBending.Checked;
            TblRow["WorseLifting"] = chkWorseLifting.Checked;
            TblRow["WorseWalking"] = chkWorseWalking.Checked;
            TblRow["WorseClimbingStairs"] = chkWorseClimbingStairs.Checked;
            TblRow["WorseDriving"] = chkWorseDriving.Checked;
            TblRow["WorseWorking"] = chkWorseWorking.Checked;
            TblRow["WorseOther"] = chkWorseOther.Checked;
            TblRow["WorseOtherText"] = txtWorseOtherText.Text.ToString();
            TblRow["ImprovedResting"] = chkImprovedResting.Checked;
            TblRow["ImprovedMedication"] = chkImprovedMedication.Checked;
            TblRow["ImprovedTherapy"] = chkImprovedTherapy.Checked;
            TblRow["ImprovedSleeping"] = chkImprovedSleeping.Checked;
            TblRow["ImprovedMovement"] = chkImprovedMovement.Checked;
            TblRow["FwdFlex"] = txtFwdFlex.Text.ToString();
            TblRow["FwdFlexNormal"] = txtFwdFlexNormal.Text.ToString();
            TblRow["Extension"] = txtExtension.Text.ToString();
            TblRow["ExtensionNormal"] = txtExtensionNormal.Text.ToString();
            TblRow["RotationRight"] = txtRotationRight.Text.ToString();
            TblRow["RotationNormal"] = txttxtRotationNormal.Text.ToString();
            TblRow["RotationLeft"] = txtRotationLeft.Text.ToString();
            TblRow["LateralFlexNormal"] = txtLateralFlexNormal.Text.ToString();
            TblRow["LateralFlexRight"] = txtLateralFlexRight.Text.ToString();
            TblRow["LateralFlexLeft"] = txtLateralFlexLeft.Text.ToString();
            TblRow["PalpationAt"] = txtPalpationAt.Text.ToString();
            TblRow["Levels"] = cboLevels.Text.ToString();
            TblRow["LegRaisedExamLeft"] = chkLegRaisedExamLeft.Checked;
            TblRow["BraggardLeft"] = chkBraggardLeft.Checked;
            TblRow["KernigLeft"] = chkKernigLeft.Checked;
            TblRow["BrudzinskiLeft"] = chkBrudzinskiLeft.Checked;
            TblRow["SacroiliacLeft"] = chkSacroiliacLeft.Checked;
            TblRow["SacralNotchLeft"] = chkSacralNotchLeft.Checked;
            TblRow["OberLeft"] = chkOberLeft.Checked;
            TblRow["LegRaisedExamRight"] = chkLegRaisedExamRight.Checked;
            TblRow["BraggardRight"] = chkBraggardRight.Checked;
            TblRow["KernigRight"] = chkKernigRight.Checked;
            TblRow["BrudzinskiRight"] = chkBrudzinskiRight.Checked;
            TblRow["SacroiliacRight"] = chkSacroiliacRight.Checked;
            TblRow["SacralNotchRight"] = chkSacralNotchRight.Checked;
            TblRow["OberRight"] = chkOberRight.Checked;
            TblRow["LegRaisedExamBilateral"] = chkLegRaisedExamBilateral.Checked;

            TblRow["LegRaisedExamBilateralText"] = txtLegRaisedExamBilateral.Text;
            TblRow["chkLegRaisedExamRightText"] = txtchkLegRaisedExamRight.Text;
            TblRow["LegRaisedExamLeftText"] = txtLegRaisedExamLeft.Text;

            TblRow["BraggardBilateral"] = chkBraggardBilateral.Checked;
            TblRow["KernigBilateral"] = chkKernigBilateral.Checked;
            TblRow["BrudzinskiBilateral"] = chkBrudzinskiBilateral.Checked;
            TblRow["SacroiliacBilateral"] = chkSacroiliacBilateral.Checked;
            TblRow["SacralNotchBilateral"] = chkSacralNotchBilateral.Checked;
            TblRow["OberBilateral"] = chkOberBilateral.Checked;
            TblRow["TPSide"] = cboTPSide1.Text.ToString();
            TblRow["TPText"] = txtTPText1.Text.ToString();
            TblRow["FreeForm"] = txtFreeForm.Text.ToString();
            TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
            TblRow["FreeFormA"] = txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty);
            TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
            TblRow["WorseSeatingtoStandingUp"] = chkWorseSeatingtoStandingUp.Checked;
            TblRow["WorseDescendingStairs"] = chkWorseDescendingStairs.Checked;
            TblRow["ISFirst"] = true;
            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

        if (_ieMode == "New")
            return "LowBack has been added...";
        else if (_ieMode == "Update")
            return "LowBack has been updated...";
        else if (_ieMode == "Delete")
            return "LowBack has been deleted...";
        else
            return "";
    }
    public void PopulateStrightFwd()
    {
        //tbRomIs.Text = "ROM";
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtensionWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFwdFlexWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLateralFlexLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtLateralFlexRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal1.Visibility = System.Windows.Visibility.Visible;
        //tbNormal2.Visibility = System.Windows.Visibility.Visible;
        //txtExtensionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFwdFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txtLateralFlexNormal.Visibility = System.Windows.Visibility.Visible;
        //txttxtRotationNormal.Visibility = System.Windows.Visibility.Visible;
    }

    public void PopulateUI(string fuID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblFubpLowBack WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];

            txtPainScale.Text = TblRow["PainScale"].ToString().Trim();
            if (!string.IsNullOrEmpty(TblRow["Constant"].ToString()))
            { chkContent.Checked = CommonConvert.ToBoolean(TblRow["Constant"].ToString()); }
            if (!string.IsNullOrEmpty(TblRow["Intermittent"].ToString()))
            { chkIntermittent.Checked = CommonConvert.ToBoolean(TblRow["Intermittent"].ToString()); }
            chkSharp.Checked = CommonConvert.ToBoolean(TblRow["Sharp"].ToString());
            chkelectric.Checked = CommonConvert.ToBoolean(TblRow["Electric"].ToString());
            chkshooting.Checked = CommonConvert.ToBoolean(TblRow["Shooting"].ToString());
            chkthrobbing.Checked = CommonConvert.ToBoolean(TblRow["Throbbling"].ToString());
            chkpulsating.Checked = CommonConvert.ToBoolean(TblRow["Pulsating"].ToString());
            chkdull.Checked = CommonConvert.ToBoolean(TblRow["Dull"].ToString());
            chkachy.Checked = CommonConvert.ToBoolean(TblRow["Achy"].ToString());
            txtRadiates.Text = TblRow["Radiates"].ToString().Trim();
            chknumbness.Checked = CommonConvert.ToBoolean(TblRow["Numbness"].ToString());
            chktingling.Checked = CommonConvert.ToBoolean(TblRow["Tingling"].ToString());
            chkBurning.Checked = CommonConvert.ToBoolean(TblRow["Burning"].ToString());
            txtburningto.Text = TblRow["BurningTo"].ToString().Trim();
            chkSideRight1.Checked = CommonConvert.ToBoolean(TblRow["SideRight1"].ToString());
            chkButtockRight1.Checked = CommonConvert.ToBoolean(TblRow["ButtockRight1"].ToString());
            chkGroinRight1.Checked = CommonConvert.ToBoolean(TblRow["GroinRight1"].ToString());
            chkHipRight1.Checked = CommonConvert.ToBoolean(TblRow["HipRight1"].ToString());
            chkThighRight1.Checked = CommonConvert.ToBoolean(TblRow["ThighRight1"].ToString());
            chkLegRight1.Checked = CommonConvert.ToBoolean(TblRow["LegRight1"].ToString());
            chkKneeRight1.Checked = CommonConvert.ToBoolean(TblRow["KneeRight1"].ToString());
            chkAnkleRight1.Checked = CommonConvert.ToBoolean(TblRow["AnkleRight1"].ToString());
            chkFeetRight1.Checked = CommonConvert.ToBoolean(TblRow["FeetRight1"].ToString());
            chkToeRight1.Checked = CommonConvert.ToBoolean(TblRow["ToeRight1"].ToString());
            chkSideLeft1.Checked = CommonConvert.ToBoolean(TblRow["SideLeft1"].ToString());
            chkButtockLeft1.Checked = CommonConvert.ToBoolean(TblRow["ButtockLeft1"].ToString());
            chkGroinLeft1.Checked = CommonConvert.ToBoolean(TblRow["GroinLeft1"].ToString());
            chkHipLeft1.Checked = CommonConvert.ToBoolean(TblRow["HipLeft1"].ToString());
            chkThighLeft1.Checked = CommonConvert.ToBoolean(TblRow["ThighLeft1"].ToString());
            chkLegLeft1.Checked = CommonConvert.ToBoolean(TblRow["LegLeft1"].ToString());
            chkKneeLeft1.Checked = CommonConvert.ToBoolean(TblRow["KneeLeft1"].ToString());
            chkAnkleLeft1.Checked = CommonConvert.ToBoolean(TblRow["AnkleLeft1"].ToString());
            chkFeetLeft1.Checked = CommonConvert.ToBoolean(TblRow["FeetLeft1"].ToString());
            chkToeLeft1.Checked = CommonConvert.ToBoolean(TblRow["ToeLeft1"].ToString());
            chkSideBilateral1.Checked = CommonConvert.ToBoolean(TblRow["SideBilateral1"].ToString());
            chkButtockBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ButtockBilateral1"].ToString());
            chkGroinBilateral1.Checked = CommonConvert.ToBoolean(TblRow["GroinBilateral1"].ToString());
            chkHipBilateral1.Checked = CommonConvert.ToBoolean(TblRow["HipBilateral1"].ToString());
            chkThighBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ThighBilateral1"].ToString());
            chkLegBilateral1.Checked = CommonConvert.ToBoolean(TblRow["LegBilateral1"].ToString());
            chkKneeBilateral1.Checked = CommonConvert.ToBoolean(TblRow["KneeBilateral1"].ToString());
            chkAnkleBilateral1.Checked = CommonConvert.ToBoolean(TblRow["AnkleBilateral1"].ToString());
            chkFeetBilateral1.Checked = CommonConvert.ToBoolean(TblRow["FeetBilateral1"].ToString());
            chkToeBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ToeBilateral1"].ToString());
            chkSideLeft2.Checked = CommonConvert.ToBoolean(TblRow["SideLeft2"].ToString());
            chkButtockLeft2.Checked = CommonConvert.ToBoolean(TblRow["ButtockLeft2"].ToString());
            chkGroinLeft2.Checked = CommonConvert.ToBoolean(TblRow["GroinLeft2"].ToString());
            chkHipLeft2.Checked = CommonConvert.ToBoolean(TblRow["HipLeft2"].ToString());
            chkThighLeft2.Checked = CommonConvert.ToBoolean(TblRow["ThighLeft2"].ToString());
            chkLegLeft2.Checked = CommonConvert.ToBoolean(TblRow["LegLeft2"].ToString());
            chkKneeLeft2.Checked = CommonConvert.ToBoolean(TblRow["KneeLeft2"].ToString());
            chkAnkleLeft2.Checked = CommonConvert.ToBoolean(TblRow["AnkleLeft2"].ToString());
            chkFeetLeft2.Checked = CommonConvert.ToBoolean(TblRow["FeetLeft2"].ToString());
            chkToeLeft2.Checked = CommonConvert.ToBoolean(TblRow["ToeLeft2"].ToString());
            chkSideRight2.Checked = CommonConvert.ToBoolean(TblRow["SideRight2"].ToString());
            chkButtockRight2.Checked = CommonConvert.ToBoolean(TblRow["ButtockRight2"].ToString());
            chkGroinRight2.Checked = CommonConvert.ToBoolean(TblRow["GroinRight2"].ToString());
            chkHipRight2.Checked = CommonConvert.ToBoolean(TblRow["HipRight2"].ToString());
            chkThighRight2.Checked = CommonConvert.ToBoolean(TblRow["ThighRight2"].ToString());
            chkLegRight2.Checked = CommonConvert.ToBoolean(TblRow["LegRight2"].ToString());
            chkSideBilateral2.Checked = CommonConvert.ToBoolean(TblRow["SideBilateral2"].ToString());
            chkButtockBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ButtockBilateral2"].ToString());
            chkGroinBilateral2.Checked = CommonConvert.ToBoolean(TblRow["GroinBilateral2"].ToString());
            chkHipBilateral2.Checked = CommonConvert.ToBoolean(TblRow["HipBilateral2"].ToString());
            chkThighBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ThighBilateral2"].ToString());
            chkLegBilateral2.Checked = CommonConvert.ToBoolean(TblRow["LegBilateral2"].ToString());
            chkKneeBilateral2.Checked = CommonConvert.ToBoolean(TblRow["KneeBilateral2"].ToString());
            chkAnkleBilateral2.Checked = CommonConvert.ToBoolean(TblRow["AnkleBilateral2"].ToString());
            chkFeetBilateral2.Checked = CommonConvert.ToBoolean(TblRow["FeetBilateral2"].ToString());
            chkToeBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ToeBilateral2"].ToString());
            txtWeeknessIn.Text = TblRow["WeeknessIn"].ToString().Trim();
            chkWorseSitting.Checked = CommonConvert.ToBoolean(TblRow["WorseSitting"].ToString());
            chkWorseStanding.Checked = CommonConvert.ToBoolean(TblRow["WorseStanding"].ToString());
            chkWorseLyingDown.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingDown"].ToString());
            chkWorseMovement.Checked = CommonConvert.ToBoolean(TblRow["WorseMovement"].ToString());
            chkWorseBending.Checked = CommonConvert.ToBoolean(TblRow["WorseBending"].ToString());
            chkWorseLifting.Checked = CommonConvert.ToBoolean(TblRow["WorseLifting"].ToString());
            chkWorseWalking.Checked = CommonConvert.ToBoolean(TblRow["WorseWalking"].ToString());
            chkWorseClimbingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseClimbingStairs"].ToString());
            chkWorseDriving.Checked = CommonConvert.ToBoolean(TblRow["WorseDriving"].ToString());
            chkWorseWorking.Checked = CommonConvert.ToBoolean(TblRow["WorseWorking"].ToString());
            chkWorseOther.Checked = CommonConvert.ToBoolean(TblRow["WorseOther"].ToString());
            txtWorseOtherText.Text = TblRow["WorseOtherText"].ToString().Trim();
            chkImprovedResting.Checked = CommonConvert.ToBoolean(TblRow["ImprovedResting"].ToString());
            chkImprovedMedication.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedication"].ToString());
            chkImprovedTherapy.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapy"].ToString());
            chkImprovedSleeping.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleeping"].ToString());
            chkImprovedMovement.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMovement"].ToString());
            // txtFwdFlexWas.Text = TblRow["FwdFlex"].ToString().Trim();
            txtFwdFlex.Text = TblRow["FwdFlex"].ToString().Trim();
            txtFwdFlexNormal.Text = TblRow["FwdFlexNormal"].ToString().Trim();
            // txtExtensionWas.Text = TblRow["Extension"].ToString().Trim();
            txtExtension.Text = TblRow["Extension"].ToString().Trim();
            txtExtensionNormal.Text = TblRow["ExtensionNormal"].ToString().Trim();
            // txtRotationRightWas.Text = TblRow["RotationRight"].ToString().Trim();
            txtRotationRight.Text = TblRow["RotationRight"].ToString().Trim();
            txttxtRotationNormal.Text = TblRow["RotationNormal"].ToString().Trim();
            //   txtRotationLeftWas.Text = TblRow["RotationLeft"].ToString().Trim();
            txtRotationLeft.Text = TblRow["RotationLeft"].ToString().Trim();
            txtLateralFlexRight.Text = TblRow["LateralFlexRight"].ToString().Trim();
            txtLateralFlexNormal.Text = TblRow["LateralFlexNormal"].ToString().Trim();
            //   txtLateralFlexRightWas.Text = TblRow["LateralFlexRight"].ToString().Trim();
            //    txtLateralFlexLeftWas.Text = TblRow["LateralFlexLeft"].ToString().Trim();
            txtLateralFlexLeft.Text = TblRow["LateralFlexLeft"].ToString().Trim();
            txtPalpationAt.Text = TblRow["PalpationAt"].ToString().Trim();
            cboLevels.Text = TblRow["Levels"].ToString().Trim();
            chkLegRaisedExamLeft.Checked = CommonConvert.ToBoolean(TblRow["LegRaisedExamLeft"].ToString());
            chkBraggardLeft.Checked = CommonConvert.ToBoolean(TblRow["BraggardLeft"].ToString());
            chkKernigLeft.Checked = CommonConvert.ToBoolean(TblRow["KernigLeft"].ToString());
            chkBrudzinskiLeft.Checked = CommonConvert.ToBoolean(TblRow["BrudzinskiLeft"].ToString());
            chkSacroiliacLeft.Checked = CommonConvert.ToBoolean(TblRow["SacroiliacLeft"].ToString());
            chkSacralNotchLeft.Checked = CommonConvert.ToBoolean(TblRow["SacralNotchLeft"].ToString());
            chkOberLeft.Checked = CommonConvert.ToBoolean(TblRow["OberLeft"].ToString());
            chkLegRaisedExamRight.Checked = CommonConvert.ToBoolean(TblRow["LegRaisedExamRight"].ToString());
            chkBraggardRight.Checked = CommonConvert.ToBoolean(TblRow["BraggardRight"].ToString());
            chkKernigRight.Checked = CommonConvert.ToBoolean(TblRow["KernigRight"].ToString());
            chkBrudzinskiRight.Checked = CommonConvert.ToBoolean(TblRow["BrudzinskiRight"].ToString());
            chkSacroiliacRight.Checked = CommonConvert.ToBoolean(TblRow["SacroiliacRight"].ToString());
            chkSacralNotchRight.Checked = CommonConvert.ToBoolean(TblRow["SacralNotchRight"].ToString());
            chkOberRight.Checked = CommonConvert.ToBoolean(TblRow["OberRight"].ToString());
            chkLegRaisedExamBilateral.Checked = CommonConvert.ToBoolean(TblRow["LegRaisedExamBilateral"].ToString());

            txtLegRaisedExamBilateral.Text = Convert.ToString(TblRow["LegRaisedExamBilateralText"]);
            txtchkLegRaisedExamRight.Text = Convert.ToString(TblRow["chkLegRaisedExamRightText"]);
            txtLegRaisedExamLeft.Text = Convert.ToString(TblRow["LegRaisedExamLeftText"]);

            chkBraggardBilateral.Checked = CommonConvert.ToBoolean(TblRow["BraggardBilateral"].ToString());
            chkKernigBilateral.Checked = CommonConvert.ToBoolean(TblRow["KernigBilateral"].ToString());
            chkBrudzinskiBilateral.Checked = CommonConvert.ToBoolean(TblRow["BrudzinskiBilateral"].ToString());
            chkSacroiliacBilateral.Checked = CommonConvert.ToBoolean(TblRow["SacroiliacBilateral"].ToString());
            chkSacralNotchBilateral.Checked = CommonConvert.ToBoolean(TblRow["SacralNotchBilateral"].ToString());
            chkOberBilateral.Checked = CommonConvert.ToBoolean(TblRow["OberBilateral"].ToString());
            cboTPSide1.Text = TblRow["TPSide"].ToString().Trim();
            txtTPText1.Text = TblRow["TPText"].ToString().Trim();
            txtFreeForm.Text = TblRow["freeform"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["freeformcc"].ToString().Trim();
            txtFreeFormA.Text = TblRow["freeforma"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["freeformp"].ToString().Trim();
            chkWorseSeatingtoStandingUp.Checked = CommonConvert.ToBoolean(TblRow["WorseSeatingtoStandingUp"].ToString());
            chkWorseDescendingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseDescendingStairs"].ToString());
            _fldPop = false;
        }

        sqlTbl1.Dispose();
        sqlCmdBuilder1.Dispose();
        sqlAdapt1.Dispose();
        oSQLConn.Close();


    }
    public void PopulateIEUI(string IEID)
    {

        string sProvider1 = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr1 = "";
        oSQLConn.ConnectionString = sProvider1;
        oSQLConn.Open();
        SqlStr1 = "Select * from tblbpLowBack WHERE PatientIE_ID = " + IEID;
        SqlDataAdapter sqlAdapt1 = new SqlDataAdapter(SqlStr1, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder1 = new SqlCommandBuilder(sqlAdapt1);
        DataTable sqlTbl1 = new DataTable();
        sqlAdapt1.Fill(sqlTbl1);
        DataRow TblRow;

        if (sqlTbl1.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl1.Rows[0];

            txtPainScale.Text = TblRow["PainScale"].ToString().Trim();
            if (!string.IsNullOrEmpty(TblRow["Constant"].ToString()))
            { chkContent.Checked = CommonConvert.ToBoolean(TblRow["Constant"].ToString()); }
            if (!string.IsNullOrEmpty(TblRow["Intermittent"].ToString()))
            { chkIntermittent.Checked = CommonConvert.ToBoolean(TblRow["Intermittent"].ToString()); }
            chkSharp.Checked = CommonConvert.ToBoolean(TblRow["Sharp"].ToString());
            chkelectric.Checked = CommonConvert.ToBoolean(TblRow["Electric"].ToString());
            chkshooting.Checked = CommonConvert.ToBoolean(TblRow["Shooting"].ToString());
            chkthrobbing.Checked = CommonConvert.ToBoolean(TblRow["Throbbling"].ToString());
            chkpulsating.Checked = CommonConvert.ToBoolean(TblRow["Pulsating"].ToString());
            chkdull.Checked = CommonConvert.ToBoolean(TblRow["Dull"].ToString());
            chkachy.Checked = CommonConvert.ToBoolean(TblRow["Achy"].ToString());
            txtRadiates.Text = TblRow["Radiates"].ToString().Trim();
            chknumbness.Checked = CommonConvert.ToBoolean(TblRow["Numbness"].ToString());
            chktingling.Checked = CommonConvert.ToBoolean(TblRow["Tingling"].ToString());
            chkBurning.Checked = CommonConvert.ToBoolean(TblRow["Burning"].ToString());
            txtburningto.Text = TblRow["BurningTo"].ToString().Trim();
            chkSideRight1.Checked = CommonConvert.ToBoolean(TblRow["SideRight1"].ToString());
            chkButtockRight1.Checked = CommonConvert.ToBoolean(TblRow["ButtockRight1"].ToString());
            chkGroinRight1.Checked = CommonConvert.ToBoolean(TblRow["GroinRight1"].ToString());
            chkHipRight1.Checked = CommonConvert.ToBoolean(TblRow["HipRight1"].ToString());
            chkThighRight1.Checked = CommonConvert.ToBoolean(TblRow["ThighRight1"].ToString());
            chkLegRight1.Checked = CommonConvert.ToBoolean(TblRow["LegRight1"].ToString());
            chkKneeRight1.Checked = CommonConvert.ToBoolean(TblRow["KneeRight1"].ToString());
            chkAnkleRight1.Checked = CommonConvert.ToBoolean(TblRow["AnkleRight1"].ToString());
            chkFeetRight1.Checked = CommonConvert.ToBoolean(TblRow["FeetRight1"].ToString());
            chkToeRight1.Checked = CommonConvert.ToBoolean(TblRow["ToeRight1"].ToString());
            chkSideLeft1.Checked = CommonConvert.ToBoolean(TblRow["SideLeft1"].ToString());
            chkButtockLeft1.Checked = CommonConvert.ToBoolean(TblRow["ButtockLeft1"].ToString());
            chkGroinLeft1.Checked = CommonConvert.ToBoolean(TblRow["GroinLeft1"].ToString());
            chkHipLeft1.Checked = CommonConvert.ToBoolean(TblRow["HipLeft1"].ToString());
            chkThighLeft1.Checked = CommonConvert.ToBoolean(TblRow["ThighLeft1"].ToString());
            chkLegLeft1.Checked = CommonConvert.ToBoolean(TblRow["LegLeft1"].ToString());
            chkKneeLeft1.Checked = CommonConvert.ToBoolean(TblRow["KneeLeft1"].ToString());
            chkAnkleLeft1.Checked = CommonConvert.ToBoolean(TblRow["AnkleLeft1"].ToString());
            chkFeetLeft1.Checked = CommonConvert.ToBoolean(TblRow["FeetLeft1"].ToString());
            chkToeLeft1.Checked = CommonConvert.ToBoolean(TblRow["ToeLeft1"].ToString());
            chkSideBilateral1.Checked = CommonConvert.ToBoolean(TblRow["SideBilateral1"].ToString());
            chkButtockBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ButtockBilateral1"].ToString());
            chkGroinBilateral1.Checked = CommonConvert.ToBoolean(TblRow["GroinBilateral1"].ToString());
            chkHipBilateral1.Checked = CommonConvert.ToBoolean(TblRow["HipBilateral1"].ToString());
            chkThighBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ThighBilateral1"].ToString());
            chkLegBilateral1.Checked = CommonConvert.ToBoolean(TblRow["LegBilateral1"].ToString());
            chkKneeBilateral1.Checked = CommonConvert.ToBoolean(TblRow["KneeBilateral1"].ToString());
            chkAnkleBilateral1.Checked = CommonConvert.ToBoolean(TblRow["AnkleBilateral1"].ToString());
            chkFeetBilateral1.Checked = CommonConvert.ToBoolean(TblRow["FeetBilateral1"].ToString());
            chkToeBilateral1.Checked = CommonConvert.ToBoolean(TblRow["ToeBilateral1"].ToString());
            chkSideLeft2.Checked = CommonConvert.ToBoolean(TblRow["SideLeft2"].ToString());
            chkButtockLeft2.Checked = CommonConvert.ToBoolean(TblRow["ButtockLeft2"].ToString());
            chkGroinLeft2.Checked = CommonConvert.ToBoolean(TblRow["GroinLeft2"].ToString());
            chkHipLeft2.Checked = CommonConvert.ToBoolean(TblRow["HipLeft2"].ToString());
            chkThighLeft2.Checked = CommonConvert.ToBoolean(TblRow["ThighLeft2"].ToString());
            chkLegLeft2.Checked = CommonConvert.ToBoolean(TblRow["LegLeft2"].ToString());
            chkKneeLeft2.Checked = CommonConvert.ToBoolean(TblRow["KneeLeft2"].ToString());
            chkAnkleLeft2.Checked = CommonConvert.ToBoolean(TblRow["AnkleLeft2"].ToString());
            chkFeetLeft2.Checked = CommonConvert.ToBoolean(TblRow["FeetLeft2"].ToString());
            chkToeLeft2.Checked = CommonConvert.ToBoolean(TblRow["ToeLeft2"].ToString());
            chkSideRight2.Checked = CommonConvert.ToBoolean(TblRow["SideRight2"].ToString());
            chkButtockRight2.Checked = CommonConvert.ToBoolean(TblRow["ButtockRight2"].ToString());
            chkGroinRight2.Checked = CommonConvert.ToBoolean(TblRow["GroinRight2"].ToString());
            chkHipRight2.Checked = CommonConvert.ToBoolean(TblRow["HipRight2"].ToString());
            chkThighRight2.Checked = CommonConvert.ToBoolean(TblRow["ThighRight2"].ToString());
            chkLegRight2.Checked = CommonConvert.ToBoolean(TblRow["LegRight2"].ToString());
            chkSideBilateral2.Checked = CommonConvert.ToBoolean(TblRow["SideBilateral2"].ToString());
            chkButtockBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ButtockBilateral2"].ToString());
            chkGroinBilateral2.Checked = CommonConvert.ToBoolean(TblRow["GroinBilateral2"].ToString());
            chkHipBilateral2.Checked = CommonConvert.ToBoolean(TblRow["HipBilateral2"].ToString());
            chkThighBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ThighBilateral2"].ToString());
            chkLegBilateral2.Checked = CommonConvert.ToBoolean(TblRow["LegBilateral2"].ToString());
            chkKneeBilateral2.Checked = CommonConvert.ToBoolean(TblRow["KneeBilateral2"].ToString());
            chkAnkleBilateral2.Checked = CommonConvert.ToBoolean(TblRow["AnkleBilateral2"].ToString());
            chkFeetBilateral2.Checked = CommonConvert.ToBoolean(TblRow["FeetBilateral2"].ToString());
            chkToeBilateral2.Checked = CommonConvert.ToBoolean(TblRow["ToeBilateral2"].ToString());
            txtWeeknessIn.Text = TblRow["WeeknessIn"].ToString().Trim();
            chkWorseSitting.Checked = CommonConvert.ToBoolean(TblRow["WorseSitting"].ToString());
            chkWorseStanding.Checked = CommonConvert.ToBoolean(TblRow["WorseStanding"].ToString());
            chkWorseLyingDown.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingDown"].ToString());
            chkWorseMovement.Checked = CommonConvert.ToBoolean(TblRow["WorseMovement"].ToString());
            chkWorseBending.Checked = CommonConvert.ToBoolean(TblRow["WorseBending"].ToString());
            chkWorseLifting.Checked = CommonConvert.ToBoolean(TblRow["WorseLifting"].ToString());
            chkWorseWalking.Checked = CommonConvert.ToBoolean(TblRow["WorseWalking"].ToString());
            chkWorseClimbingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseClimbingStairs"].ToString());
            chkWorseDriving.Checked = CommonConvert.ToBoolean(TblRow["WorseDriving"].ToString());
            chkWorseWorking.Checked = CommonConvert.ToBoolean(TblRow["WorseWorking"].ToString());
            chkWorseOther.Checked = CommonConvert.ToBoolean(TblRow["WorseOther"].ToString());
            txtWorseOtherText.Text = TblRow["WorseOtherText"].ToString().Trim();
            chkImprovedResting.Checked = CommonConvert.ToBoolean(TblRow["ImprovedResting"].ToString());
            chkImprovedMedication.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedication"].ToString());
            chkImprovedTherapy.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapy"].ToString());
            chkImprovedSleeping.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleeping"].ToString());
            chkImprovedMovement.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMovement"].ToString());
            // txtFwdFlexWas.Text = TblRow["FwdFlex"].ToString().Trim();
            txtFwdFlex.Text = TblRow["FwdFlex"].ToString().Trim();
            txtFwdFlexNormal.Text = TblRow["FwdFlexNormal"].ToString().Trim();
            // txtExtensionWas.Text = TblRow["Extension"].ToString().Trim();
            txtExtension.Text = TblRow["Extension"].ToString().Trim();
            txtExtensionNormal.Text = TblRow["ExtensionNormal"].ToString().Trim();
            // txtRotationRightWas.Text = TblRow["RotationRight"].ToString().Trim();
            txtRotationRight.Text = TblRow["RotationRight"].ToString().Trim();
            txttxtRotationNormal.Text = TblRow["RotationNormal"].ToString().Trim();
            //   txtRotationLeftWas.Text = TblRow["RotationLeft"].ToString().Trim();
            txtRotationLeft.Text = TblRow["RotationLeft"].ToString().Trim();
            txtLateralFlexRight.Text = TblRow["LateralFlexRight"].ToString().Trim();
            txtLateralFlexNormal.Text = TblRow["LateralFlexNormal"].ToString().Trim();
            //   txtLateralFlexRightWas.Text = TblRow["LateralFlexRight"].ToString().Trim();
            //    txtLateralFlexLeftWas.Text = TblRow["LateralFlexLeft"].ToString().Trim();
            txtLateralFlexLeft.Text = TblRow["LateralFlexLeft"].ToString().Trim();
            txtPalpationAt.Text = TblRow["PalpationAt"].ToString().Trim();
            cboLevels.Text = TblRow["Levels"].ToString().Trim();
            chkLegRaisedExamLeft.Checked = CommonConvert.ToBoolean(TblRow["LegRaisedExamLeft"].ToString());
            chkBraggardLeft.Checked = CommonConvert.ToBoolean(TblRow["BraggardLeft"].ToString());
            chkKernigLeft.Checked = CommonConvert.ToBoolean(TblRow["KernigLeft"].ToString());
            chkBrudzinskiLeft.Checked = CommonConvert.ToBoolean(TblRow["BrudzinskiLeft"].ToString());
            chkSacroiliacLeft.Checked = CommonConvert.ToBoolean(TblRow["SacroiliacLeft"].ToString());
            chkSacralNotchLeft.Checked = CommonConvert.ToBoolean(TblRow["SacralNotchLeft"].ToString());
            chkOberLeft.Checked = CommonConvert.ToBoolean(TblRow["OberLeft"].ToString());
            chkLegRaisedExamRight.Checked = CommonConvert.ToBoolean(TblRow["LegRaisedExamRight"].ToString());
            chkBraggardRight.Checked = CommonConvert.ToBoolean(TblRow["BraggardRight"].ToString());
            chkKernigRight.Checked = CommonConvert.ToBoolean(TblRow["KernigRight"].ToString());
            chkBrudzinskiRight.Checked = CommonConvert.ToBoolean(TblRow["BrudzinskiRight"].ToString());
            chkSacroiliacRight.Checked = CommonConvert.ToBoolean(TblRow["SacroiliacRight"].ToString());
            chkSacralNotchRight.Checked = CommonConvert.ToBoolean(TblRow["SacralNotchRight"].ToString());
            chkOberRight.Checked = CommonConvert.ToBoolean(TblRow["OberRight"].ToString());
            chkLegRaisedExamBilateral.Checked = CommonConvert.ToBoolean(TblRow["LegRaisedExamBilateral"].ToString());

            txtLegRaisedExamBilateral.Text = Convert.ToString(TblRow["LegRaisedExamBilateralText"]);
            txtchkLegRaisedExamRight.Text = Convert.ToString(TblRow["chkLegRaisedExamRightText"]);
            txtLegRaisedExamLeft.Text = Convert.ToString(TblRow["LegRaisedExamLeftText"]);

            chkBraggardBilateral.Checked = CommonConvert.ToBoolean(TblRow["BraggardBilateral"].ToString());
            chkKernigBilateral.Checked = CommonConvert.ToBoolean(TblRow["KernigBilateral"].ToString());
            chkBrudzinskiBilateral.Checked = CommonConvert.ToBoolean(TblRow["BrudzinskiBilateral"].ToString());
            chkSacroiliacBilateral.Checked = CommonConvert.ToBoolean(TblRow["SacroiliacBilateral"].ToString());
            chkSacralNotchBilateral.Checked = CommonConvert.ToBoolean(TblRow["SacralNotchBilateral"].ToString());
            chkOberBilateral.Checked = CommonConvert.ToBoolean(TblRow["OberBilateral"].ToString());
            cboTPSide1.Text = TblRow["TPSide"].ToString().Trim();
            txtTPText1.Text = TblRow["TPText"].ToString().Trim();
            txtFreeForm.Text = TblRow["freeform"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["freeformcc"].ToString().Trim();
            txtFreeFormA.Text = TblRow["freeforma"].ToString().Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = TblRow["freeformp"].ToString().Trim();
            chkWorseSeatingtoStandingUp.Checked = CommonConvert.ToBoolean(TblRow["WorseSeatingtoStandingUp"].ToString());
            chkWorseDescendingStairs.Checked = CommonConvert.ToBoolean(TblRow["WorseDescendingStairs"].ToString());
            _fldPop = false;
        }

        sqlTbl1.Dispose();
        sqlCmdBuilder1.Dispose();
        sqlAdapt1.Dispose();
        oSQLConn.Close();


    }
    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/LowBack");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;
            txtPainScale.Text = node.SelectSingleNode("PainScale") == null ? txtPainScale.Text.ToString().Trim() : node.SelectSingleNode("PainScale").InnerText;
            chkSharp.Checked = node.SelectSingleNode("Sharp") == null ? chkSharp.Checked : Convert.ToBoolean(node.SelectSingleNode("Sharp").InnerText);
            chkelectric.Checked = node.SelectSingleNode("Electric") == null ? chkelectric.Checked : Convert.ToBoolean(node.SelectSingleNode("Electric").InnerText);
            chkshooting.Checked = node.SelectSingleNode("Shooting") == null ? chkshooting.Checked : Convert.ToBoolean(node.SelectSingleNode("Shooting").InnerText);
            chkthrobbing.Checked = node.SelectSingleNode("Throbbling") == null ? chkthrobbing.Checked : Convert.ToBoolean(node.SelectSingleNode("Throbbling").InnerText);
            chkpulsating.Checked = node.SelectSingleNode("Pulsating") == null ? chkpulsating.Checked : Convert.ToBoolean(node.SelectSingleNode("Pulsating").InnerText);
            chkdull.Checked = node.SelectSingleNode("Dull") == null ? chkdull.Checked : Convert.ToBoolean(node.SelectSingleNode("Dull").InnerText);
            chkachy.Checked = node.SelectSingleNode("Achy") == null ? chkachy.Checked : Convert.ToBoolean(node.SelectSingleNode("Achy").InnerText);
            txtRadiates.Text = node.SelectSingleNode("Radiates") == null ? txtRadiates.Text.ToString().Trim() : node.SelectSingleNode("Radiates").InnerText;
            chknumbness.Checked = node.SelectSingleNode("Numbness") == null ? chknumbness.Checked : Convert.ToBoolean(node.SelectSingleNode("Numbness").InnerText);
            chktingling.Checked = node.SelectSingleNode("Tingling") == null ? chktingling.Checked : Convert.ToBoolean(node.SelectSingleNode("Tingling").InnerText);
            chkBurning.Checked = node.SelectSingleNode("Burning") == null ? chkBurning.Checked : Convert.ToBoolean(node.SelectSingleNode("Burning").InnerText);
            txtburningto.Text = node.SelectSingleNode("BurningTo") == null ? txtburningto.Text.ToString().Trim() : node.SelectSingleNode("BurningTo").InnerText;
            chkSideRight1.Checked = node.SelectSingleNode("SideRight1") == null ? chkSideRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("SideRight1").InnerText);
            chkButtockRight1.Checked = node.SelectSingleNode("ButtockRight1") == null ? chkButtockRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ButtockRight1").InnerText);
            chkGroinRight1.Checked = node.SelectSingleNode("GroinRight1") == null ? chkGroinRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("GroinRight1").InnerText);
            chkHipRight1.Checked = node.SelectSingleNode("HipRight1") == null ? chkHipRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("HipRight1").InnerText);
            chkThighRight1.Checked = node.SelectSingleNode("ThighRight1") == null ? chkThighRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ThighRight1").InnerText);
            chkLegRight1.Checked = node.SelectSingleNode("LegRight1") == null ? chkLegRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("LegRight1").InnerText);
            chkKneeRight1.Checked = node.SelectSingleNode("KneeRight1") == null ? chkKneeRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("KneeRight1").InnerText);
            chkAnkleRight1.Checked = node.SelectSingleNode("AnkleRight1") == null ? chkAnkleRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("AnkleRight1").InnerText);
            chkFeetRight1.Checked = node.SelectSingleNode("FeetRight1") == null ? chkFeetRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("FeetRight1").InnerText);
            chkToeRight1.Checked = node.SelectSingleNode("ToeRight1") == null ? chkToeRight1.Checked : Convert.ToBoolean(node.SelectSingleNode("ToeRight1").InnerText);
            chkSideLeft1.Checked = node.SelectSingleNode("SideLeft1") == null ? chkSideLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("SideLeft1").InnerText);
            chkButtockLeft1.Checked = node.SelectSingleNode("ButtockLeft1") == null ? chkButtockLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ButtockLeft1").InnerText);
            chkGroinLeft1.Checked = node.SelectSingleNode("GroinLeft1") == null ? chkGroinLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("GroinLeft1").InnerText);
            chkHipLeft1.Checked = node.SelectSingleNode("HipLeft1") == null ? chkHipLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("HipLeft1").InnerText);
            chkThighLeft1.Checked = node.SelectSingleNode("ThighLeft1") == null ? chkThighLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ThighLeft1").InnerText);
            chkLegLeft1.Checked = node.SelectSingleNode("LegLeft1") == null ? chkLegLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("LegLeft1").InnerText);
            chkKneeLeft1.Checked = node.SelectSingleNode("KneeLeft1") == null ? chkKneeLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("KneeLeft1").InnerText);
            chkAnkleLeft1.Checked = node.SelectSingleNode("AnkleLeft1") == null ? chkAnkleLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("AnkleLeft1").InnerText);
            chkFeetLeft1.Checked = node.SelectSingleNode("FeetLeft1") == null ? chkFeetLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("FeetLeft1").InnerText);
            chkToeLeft1.Checked = node.SelectSingleNode("ToeLeft1") == null ? chkToeLeft1.Checked : Convert.ToBoolean(node.SelectSingleNode("ToeLeft1").InnerText);
            chkSideBilateral1.Checked = node.SelectSingleNode("SideBilateral1") == null ? chkSideBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("SideBilateral1").InnerText);
            chkButtockBilateral1.Checked = node.SelectSingleNode("ButtockBilateral1") == null ? chkButtockBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ButtockBilateral1").InnerText);
            chkGroinBilateral1.Checked = node.SelectSingleNode("GroinBilateral1") == null ? chkGroinBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("GroinBilateral1").InnerText);
            chkHipBilateral1.Checked = node.SelectSingleNode("HipBilateral1") == null ? chkHipBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("HipBilateral1").InnerText);
            chkThighBilateral1.Checked = node.SelectSingleNode("ThighBilateral1") == null ? chkThighBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ThighBilateral1").InnerText);
            chkLegBilateral1.Checked = node.SelectSingleNode("LegBilateral1") == null ? chkLegBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("LegBilateral1").InnerText);
            chkKneeBilateral1.Checked = node.SelectSingleNode("KneeBilateral1") == null ? chkKneeBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("KneeBilateral1").InnerText);
            chkAnkleBilateral1.Checked = node.SelectSingleNode("AnkleBilateral1") == null ? chkAnkleBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("AnkleBilateral1").InnerText);
            chkFeetBilateral1.Checked = node.SelectSingleNode("FeetBilateral1") == null ? chkFeetBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("FeetBilateral1").InnerText);
            chkToeBilateral1.Checked = node.SelectSingleNode("ToeBilateral1") == null ? chkToeBilateral1.Checked : Convert.ToBoolean(node.SelectSingleNode("ToeBilateral1").InnerText);
            chkSideLeft2.Checked = node.SelectSingleNode("SideLeft2") == null ? chkSideLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("SideLeft2").InnerText);
            chkButtockLeft2.Checked = node.SelectSingleNode("ButtockLeft2") == null ? chkButtockLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ButtockLeft2").InnerText);
            chkGroinLeft2.Checked = node.SelectSingleNode("GroinLeft2") == null ? chkGroinLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("GroinLeft2").InnerText);
            chkHipLeft2.Checked = node.SelectSingleNode("HipLeft2") == null ? chkHipLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("HipLeft2").InnerText);
            chkThighLeft2.Checked = node.SelectSingleNode("ThighLeft2") == null ? chkThighLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ThighLeft2").InnerText);
            chkLegLeft2.Checked = node.SelectSingleNode("LegLeft2") == null ? chkLegLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("LegLeft2").InnerText);
            chkKneeLeft2.Checked = node.SelectSingleNode("KneeLeft2") == null ? chkKneeLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("KneeLeft2").InnerText);
            chkAnkleLeft2.Checked = node.SelectSingleNode("AnkleLeft2") == null ? chkAnkleLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("AnkleLeft2").InnerText);
            chkFeetLeft2.Checked = node.SelectSingleNode("FeetLeft2") == null ? chkFeetLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("FeetLeft2").InnerText);
            chkToeLeft2.Checked = node.SelectSingleNode("ToeLeft2") == null ? chkToeLeft2.Checked : Convert.ToBoolean(node.SelectSingleNode("ToeLeft2").InnerText);
            chkSideRight2.Checked = node.SelectSingleNode("SideRight2") == null ? chkSideRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("SideRight2").InnerText);
            chkButtockRight2.Checked = node.SelectSingleNode("ButtockRight2") == null ? chkButtockRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("ButtockRight2").InnerText);
            chkGroinRight2.Checked = node.SelectSingleNode("GroinRight2") == null ? chkGroinRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("GroinRight2").InnerText);
            chkHipRight2.Checked = node.SelectSingleNode("HipRight2") == null ? chkHipRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("HipRight2").InnerText);
            chkThighRight2.Checked = node.SelectSingleNode("ThighRight2") == null ? chkThighRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("ThighRight2").InnerText);
            chkLegRight2.Checked = node.SelectSingleNode("LegRight2") == null ? chkLegRight2.Checked : Convert.ToBoolean(node.SelectSingleNode("LegRight2").InnerText);
            chkSideBilateral2.Checked = node.SelectSingleNode("SideBilateral2") == null ? chkSideBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("SideBilateral2").InnerText);
            chkButtockBilateral2.Checked = node.SelectSingleNode("ButtockBilateral2") == null ? chkButtockBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ButtockBilateral2").InnerText);
            chkGroinBilateral2.Checked = node.SelectSingleNode("GroinBilateral2") == null ? chkGroinBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("GroinBilateral2").InnerText);
            chkHipBilateral2.Checked = node.SelectSingleNode("HipBilateral2") == null ? chkHipBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("HipBilateral2").InnerText);
            chkThighBilateral2.Checked = node.SelectSingleNode("ThighBilateral2") == null ? chkThighBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ThighBilateral2").InnerText);
            chkLegBilateral2.Checked = node.SelectSingleNode("LegBilateral2") == null ? chkLegBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("LegBilateral2").InnerText);
            chkKneeBilateral2.Checked = node.SelectSingleNode("KneeBilateral2") == null ? chkKneeBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("KneeBilateral2").InnerText);
            chkAnkleBilateral2.Checked = node.SelectSingleNode("AnkleBilateral2") == null ? chkAnkleBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("AnkleBilateral2").InnerText);
            chkFeetBilateral2.Checked = node.SelectSingleNode("FeetBilateral2") == null ? chkFeetBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("FeetBilateral2").InnerText);
            chkToeBilateral2.Checked = node.SelectSingleNode("ToeBilateral2") == null ? chkToeBilateral2.Checked : Convert.ToBoolean(node.SelectSingleNode("ToeBilateral2").InnerText);
            txtWeeknessIn.Text = node.SelectSingleNode("WeeknessIn") == null ? txtWeeknessIn.Text.ToString().Trim() : node.SelectSingleNode("WeeknessIn").InnerText;
            chkWorseSitting.Checked = node.SelectSingleNode("WorseSitting") == null ? chkWorseSitting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSitting").InnerText);
            chkWorseStanding.Checked = node.SelectSingleNode("WorseStanding") == null ? chkWorseStanding.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseStanding").InnerText);
            chkWorseLyingDown.Checked = node.SelectSingleNode("WorseLyingDown") == null ? chkWorseLyingDown.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingDown").InnerText);
            chkWorseMovement.Checked = node.SelectSingleNode("WorseMovement") == null ? chkWorseMovement.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovement").InnerText);
            chkWorseBending.Checked = node.SelectSingleNode("WorseBending") == null ? chkWorseBending.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseBending").InnerText);
            chkWorseLifting.Checked = node.SelectSingleNode("WorseLifting") == null ? chkWorseLifting.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLifting").InnerText);
            chkWorseWalking.Checked = node.SelectSingleNode("WorseWalking") == null ? chkWorseWalking.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWalking").InnerText);
            chkWorseClimbingStairs.Checked = node.SelectSingleNode("WorseClimbingStairs") == null ? chkWorseClimbingStairs.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseClimbingStairs").InnerText);
            chkWorseDriving.Checked = node.SelectSingleNode("WorseDriving") == null ? chkWorseDriving.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseDriving").InnerText);
            chkWorseWorking.Checked = node.SelectSingleNode("WorseWorking") == null ? chkWorseWorking.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorking").InnerText);
            chkWorseOther.Checked = node.SelectSingleNode("WorseOther") == null ? chkWorseOther.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseOther").InnerText);
            txtWorseOtherText.Text = node.SelectSingleNode("WorseOtherText") == null ? txtWorseOtherText.Text.ToString().Trim() : node.SelectSingleNode("WorseOtherText").InnerText;
            chkImprovedResting.Checked = node.SelectSingleNode("ImprovedResting") == null ? chkImprovedResting.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedResting").InnerText);
            chkImprovedMedication.Checked = node.SelectSingleNode("ImprovedMedication") == null ? chkImprovedMedication.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedication").InnerText);
            chkImprovedTherapy.Checked = node.SelectSingleNode("ImprovedTherapy") == null ? chkImprovedTherapy.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapy").InnerText);
            chkImprovedSleeping.Checked = node.SelectSingleNode("ImprovedSleeping") == null ? chkImprovedSleeping.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleeping").InnerText);
            chkImprovedMovement.Checked = node.SelectSingleNode("ImprovedMovement") == null ? chkImprovedMovement.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMovement").InnerText);
            txtFwdFlexNormal.Text = node.SelectSingleNode("LowBackFwdFlexNormal") == null ? txtFwdFlexNormal.Text.ToString().Trim() : node.SelectSingleNode("LowBackFwdFlexNormal").InnerText;
            //  txtFwdFlexWas.Text = node.SelectSingleNode("FwdFlex") == null ? txtFwdFlexWas.Text.ToString().Trim() : node.SelectSingleNode("FwdFlex").InnerText;
            txtFwdFlex.Text = node.SelectSingleNode("FwdFlex") == null ? txtFwdFlex.Text.ToString().Trim() : node.SelectSingleNode("FwdFlex").InnerText;
            txtExtensionNormal.Text = node.SelectSingleNode("LowBackExt") == null ? txtExtensionNormal.Text.ToString().Trim() : node.SelectSingleNode("LowBackExt").InnerText;
            // txtExtensionWas.Text = node.SelectSingleNode("Extension") == null ? txtExtensionWas.Text.ToString().Trim() : node.SelectSingleNode("Extension").InnerText;
            txtExtension.Text = node.SelectSingleNode("Extension") == null ? txtExtension.Text.ToString().Trim() : node.SelectSingleNode("Extension").InnerText;
            //    txtRotationRightWas.Text = node.SelectSingleNode("RotationRight") == null ? txtRotationRightWas.Text.ToString().Trim() : node.SelectSingleNode("RotationRight").InnerText;
            txtRotationRight.Text = node.SelectSingleNode("RotationRight") == null ? txtRotationRight.Text.ToString().Trim() : node.SelectSingleNode("RotationRight").InnerText;

            txttxtRotationNormal.Text = node.SelectSingleNode("LowBackRotNormal") == null ? txttxtRotationNormal.Text.ToString().Trim() : node.SelectSingleNode("LowBackRotNormal").InnerText;

            //  txtRotationLeftWas.Text = node.SelectSingleNode("RotationLeft") == null ? txtRotationLeftWas.Text.ToString().Trim() : node.SelectSingleNode("RotationLeft").InnerText;
            txtRotationLeft.Text = node.SelectSingleNode("RotationLeft") == null ? txtRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("RotationLeft").InnerText;

            txtLateralFlexNormal.Text = node.SelectSingleNode("LowBackLatFlex") == null ? txtLateralFlexNormal.Text.ToString().Trim() : node.SelectSingleNode("LowBackLatFlex").InnerText;

            txtLateralFlexRight.Text = node.SelectSingleNode("LateralFlexRight") == null ? txtLateralFlexRight.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexRight").InnerText;
            //  txtLateralFlexRightWas.Text = node.SelectSingleNode("LateralFlexRight") == null ? txtLateralFlexRightWas.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexRight").InnerText;
            //   txtLateralFlexLeftWas.Text = node.SelectSingleNode("LateralFlexLeft") == null ? txtLateralFlexLeftWas.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexLeft").InnerText;
            txtLateralFlexLeft.Text = node.SelectSingleNode("LateralFlexLeft") == null ? txtLateralFlexLeft.Text.ToString().Trim() : node.SelectSingleNode("LateralFlexLeft").InnerText;
            cboLevels.Text = node.SelectSingleNode("Levels") == null ? cboLevels.Text.ToString().Trim() : node.SelectSingleNode("Levels").InnerText;
            chkLegRaisedExamLeft.Checked = node.SelectSingleNode("LegRaisedExamLeft") == null ? chkLegRaisedExamLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("LegRaisedExamLeft").InnerText);
            chkBraggardLeft.Checked = node.SelectSingleNode("BraggardLeft") == null ? chkBraggardLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("BraggardLeft").InnerText);
            chkKernigLeft.Checked = node.SelectSingleNode("KernigLeft") == null ? chkKernigLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("KernigLeft").InnerText);
            chkBrudzinskiLeft.Checked = node.SelectSingleNode("BrudzinskiLeft") == null ? chkBrudzinskiLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("BrudzinskiLeft").InnerText);
            chkSacroiliacLeft.Checked = node.SelectSingleNode("SacroiliacLeft") == null ? chkSacroiliacLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SacroiliacLeft").InnerText);
            chkSacralNotchLeft.Checked = node.SelectSingleNode("SacralNotchLeft") == null ? chkSacralNotchLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SacralNotchLeft").InnerText);
            chkOberLeft.Checked = node.SelectSingleNode("OberLeft") == null ? chkOberLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("OberLeft").InnerText);
            chkLegRaisedExamRight.Checked = node.SelectSingleNode("LegRaisedExamRight") == null ? chkLegRaisedExamRight.Checked : Convert.ToBoolean(node.SelectSingleNode("LegRaisedExamRight").InnerText);
            chkBraggardRight.Checked = node.SelectSingleNode("BraggardRight") == null ? chkBraggardRight.Checked : Convert.ToBoolean(node.SelectSingleNode("BraggardRight").InnerText);
            chkKernigRight.Checked = node.SelectSingleNode("KernigRight") == null ? chkKernigRight.Checked : Convert.ToBoolean(node.SelectSingleNode("KernigRight").InnerText);
            chkBrudzinskiRight.Checked = node.SelectSingleNode("BrudzinskiRight") == null ? chkBrudzinskiRight.Checked : Convert.ToBoolean(node.SelectSingleNode("BrudzinskiRight").InnerText);
            chkSacroiliacRight.Checked = node.SelectSingleNode("SacroiliacRight") == null ? chkSacroiliacRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SacroiliacRight").InnerText);
            chkSacralNotchRight.Checked = node.SelectSingleNode("SacralNotchRight") == null ? chkSacralNotchRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SacralNotchRight").InnerText);
            chkOberRight.Checked = node.SelectSingleNode("OberRight") == null ? chkOberRight.Checked : Convert.ToBoolean(node.SelectSingleNode("OberRight").InnerText);
            chkLegRaisedExamBilateral.Checked = node.SelectSingleNode("LegRaisedExamBilateral") == null ? chkLegRaisedExamBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("LegRaisedExamBilateral").InnerText);
            chkBraggardBilateral.Checked = node.SelectSingleNode("BraggardBilateral") == null ? chkBraggardBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("BraggardBilateral").InnerText);
            chkKernigBilateral.Checked = node.SelectSingleNode("KernigBilateral") == null ? chkKernigBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("KernigBilateral").InnerText);
            chkBrudzinskiBilateral.Checked = node.SelectSingleNode("BrudzinskiBilateral") == null ? chkBrudzinskiBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("BrudzinskiBilateral").InnerText);
            chkSacroiliacBilateral.Checked = node.SelectSingleNode("SacroiliacBilateral") == null ? chkSacroiliacBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("SacroiliacBilateral").InnerText);
            chkSacralNotchBilateral.Checked = node.SelectSingleNode("SacralNotchBilateral") == null ? chkSacralNotchBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("SacralNotchBilateral").InnerText);
            chkOberBilateral.Checked = node.SelectSingleNode("OberBilateral") == null ? chkOberBilateral.Checked : Convert.ToBoolean(node.SelectSingleNode("OberBilateral").InnerText);
            txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim().Replace("      ", string.Empty) : node.SelectSingleNode("FreeFormA").InnerText.Trim().Replace("      ", string.Empty);
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
            chkWorseSeatingtoStandingUp.Checked = node.SelectSingleNode("WorseSeatingtoStandingUp") == null ? chkWorseSeatingtoStandingUp.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseSeatingtoStandingUp").InnerText);
            chkWorseDescendingStairs.Checked = node.SelectSingleNode("WorseDescendingStairs") == null ? chkWorseDescendingStairs.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseDescendingStairs").InnerText);
            _fldPop = false;
        }
    }
    public void BindDataGrid()
    {
        if (_CurIEid == "" || _CurIEid == "0")
            return;
        string sProvider = System.Configuration.ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        try
        {
            SqlDataAdapter oSQLAdpr;
            DataTable Standards = new DataTable();
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            //SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' Order By BodyPart,Heading";
            SqlStr = @"Select 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN Convert(varchar,p.ProcedureDetail_ID) +'_R'
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN  Convert(varchar,p.ProcedureDetail_ID) +'_S'
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN Convert(varchar,p.ProcedureDetail_ID) +'_E'
                              END  END END as ID, 
                        CASE 
                              WHEN p.Requested is not null 
                               THEN p.Heading
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_Heading
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_Heading
                              END  END END as Heading, 
                        	  CASE 
                              WHEN p.Requested is not null 
                               THEN p.PDesc
                              ELSE 
                        		case when p.Scheduled is not null
                        			THEN p.S_PDesc
                        		ELSE
                        		   CASE
                        				WHEN p.Executed is not null
                        				THEN p.E_PDesc
                              END  END END as PDesc
                        	 -- ,p.Requested,p.Heading RequestedHeading,p.Scheduled,p.S_Heading ScheduledHeading,p.Executed,p.E_Heading ExecutedHeading
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' AND PatientFU_ID = '" + _FuId + "' and IsConsidered=0 Order By BodyPart,Heading";
            oSQLCmd.Connection = oSQLConn;
            oSQLCmd.CommandText = SqlStr;
            oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
            oSQLAdpr.Fill(Standards);
            dgvStandards.DataSource = "";
            dgvStandards.DataSource = Standards.DefaultView;
            dgvStandards.DataBind();
            oSQLAdpr.Dispose();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }
    public string SaveStandards(string ieID)
    {

        string ids = string.Empty;
        try
        {
            foreach (GridViewRow row in dgvStandards.Rows)
            {


                string Procedure_ID, MCODE, BodyPart, Heading, CCDesc, PEDesc, ADesc, PDesc;
                Procedure_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                Heading = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                PDesc = row.Cells[2].Controls.OfType<TextBox>().FirstOrDefault().Text;


                ids += Session["PatientIE_ID2"].ToString() + ",";
                SaveStdUI(ieID, Procedure_ID, Heading, PDesc);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Standard(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }
    public void SaveStdUI(string ieID, string iStdID, string heading, string pdesc)
    {
        string[] _Type = iStdID.Split('_');
        int _StdID = Convert.ToInt32(_Type[0]);
        string Part = Convert.ToString(_Type[1]);

        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        //long _StdID = Convert.ToInt64(iStdID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + ieID + " AND ProcedureDetail_ID = " + _StdID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        //if (sqlTbl.Rows.Count == 0 && StdChecked == true)
        //    _ieMode = "New";
        //else if (sqlTbl.Rows.Count == 0 && StdChecked == false)
        //    _ieMode = "None";
        //else if (sqlTbl.Rows.Count > 0 && StdChecked == false)
        //    _ieMode = "Delete";
        //else
        _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["ProcedureDetail_ID"] = _StdID;
            TblRow["PatientIE_ID"] = _ieID;

            if (Part.Equals("R"))
            {
                TblRow["Heading"] = heading.ToString().Trim();
                TblRow["PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("S"))
            {
                TblRow["S_Heading"] = heading.ToString().Trim();
                TblRow["S_PDesc"] = pdesc.ToString().Trim();
            }
            else if (Part.Equals("E"))
            {
                TblRow["E_Heading"] = heading.ToString().Trim();
                TblRow["E_PDesc"] = pdesc.ToString().Trim();
            }

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }


    private void Page_Loaded_1(object sender, EventArgs e) //RoutedEventArgs 
    {
        PopulateStrightFwd();
    }

    protected void btnReset_Click(object sender, EventArgs e)// RoutedEventArgs
    {
        chkSideLeft1.Checked = false;
        chkButtockLeft1.Checked = false;
        chkGroinLeft1.Checked = false;
        chkHipLeft1.Checked = false;
        chkThighLeft1.Checked = false;
        chkLegLeft1.Checked = false;
        chkKneeLeft1.Checked = false;
        chkAnkleLeft1.Checked = false;
        chkAnkleRight1.Checked = false;
        chkFeetLeft1.Checked = false;
        chkToeLeft1.Checked = false;
        chkSideRight1.Checked = false;
        chkButtockRight1.Checked = false;
        chkGroinRight1.Checked = false;
        chkHipRight1.Checked = false;
        chkThighRight1.Checked = false;
        chkLegRight1.Checked = false;
        chkSideBilateral1.Checked = false;
        chkButtockBilateral1.Checked = false;
        chkGroinBilateral1.Checked = false;
        chkHipBilateral1.Checked = false;
        chkThighBilateral1.Checked = false;
        chkLegBilateral1.Checked = false;
        chkKneeBilateral1.Checked = false;
        chkAnkleBilateral1.Checked = false;
        chkFeetBilateral1.Checked = false;
        chkToeBilateral1.Checked = false;
        chkSide1None.Checked = false;
        chkButtock1None.Checked = false;
        chkGroin1None.Checked = false;
        chkHip1None.Checked = false;
        chkThigh1None.Checked = false;
        chkLeg1None.Checked = false;
        chkKnee1None.Checked = false;
        chkAnkle1None.Checked = false;
        chkFeet1None.Checked = false;
        chkToe1None.Checked = false;
    }

    protected void btnReset1_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        chkSideLeft2.Checked = false;
        chkButtockLeft2.Checked = false;
        chkGroinLeft2.Checked = false;
        chkHipLeft2.Checked = false;
        chkThighLeft2.Checked = false;
        chkLegLeft2.Checked = false;
        chkKneeLeft2.Checked = false;
        chkAnkleLeft2.Checked = false;
        chkFeetLeft2.Checked = false;
        chkToeLeft2.Checked = false;
        chkSideRight2.Checked = false;
        chkButtockRight2.Checked = false;
        chkGroinRight2.Checked = false;
        chkHipRight2.Checked = false;
        chkThighRight2.Checked = false;
        chkLegRight2.Checked = false;
        chkSideBilateral2.Checked = false;
        chkButtockBilateral2.Checked = false;
        chkGroinBilateral2.Checked = false;
        chkHipBilateral2.Checked = false;
        chkThighBilateral2.Checked = false;
        chkLegBilateral2.Checked = false;
        chkKneeBilateral2.Checked = false;
        chkAnkleBilateral2.Checked = false;
        chkFeetBilateral2.Checked = false;
        chkToeBilateral2.Checked = false;
        chkSide2None.Checked = false;
        chkButtock2None.Checked = false;
        chkGroin2None.Checked = false;
        chkHip2None.Checked = false;
        chkThigh2None.Checked = false;
        chkLeg2None.Checked = false;
        chkKnee2None.Checked = false;
        chkAnkle2None.Checked = false;
        chkFeet2None.Checked = false;
        chkToe2None.Checked = false;
    }

    public string SaveDiagnosis(string ieID)
    {
        string ids = string.Empty;
        try
        {
            ieID = Session["PatientIE_ID"].ToString();
            RemoveDiagCodesDetail(Session["patientFUId"].ToString());
            foreach (GridViewRow row in dgvDiagCodes.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    string Description, DiagCode, DiagCode_ID;

                    DiagCode_ID = row.Cells[0].Controls.OfType<HiddenField>().FirstOrDefault().Value;
                    //        DiagCodeDetail_ID = row.Cells[2].Controls.OfType<HiddenField>().FirstOrDefault().Value;

                    Description = row.Cells[1].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    DiagCode = row.Cells[0].Controls.OfType<TextBox>().FirstOrDefault().Text;

                    bool isChecked = row.Cells[2].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (isChecked)
                    {
                        ids += DiagCode_ID + ",";
                        SaveDiagUI(ieID, DiagCode_ID, true, _CurBP, Description, DiagCode);
                    }

                }
            }

        }
        catch (Exception ex)
        {
            //MessageBox.Show(ex.Message);
        }
        if (ids != string.Empty)
            return "Diagnosis Code(s) " + ids.Trim(',') + " saved...";
        else
            return "";
    }
    public void SaveDiagUI(string ieID, string iDiagID, bool DiagChecked, string bp, string dcd, string dc)
    {
        string _ieMode = "";
        long _ieID = Convert.ToInt64(ieID);
        long _DiagID = Convert.ToInt64(iDiagID);
        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * FROM tblDiagCodesDetail WHERE PatientIE_ID = " + ieID + " AND Diag_Master_ID = " + _DiagID + " AND PatientFu_ID=" + Session["patientFUId"].ToString() + " and BodyPart like '%" + _CurBP + "%' ";
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count == 0 && DiagChecked == true)
            _ieMode = "New";
        else if (sqlTbl.Rows.Count == 0 && DiagChecked == false)
            _ieMode = "None";
        else if (sqlTbl.Rows.Count > 0 && DiagChecked == false)
            _ieMode = "Delete";
        else
            _ieMode = "Update";

        if (_ieMode == "New")
            TblRow = sqlTbl.NewRow();
        else if (_ieMode == "Update" || _ieMode == "Delete")
        {
            TblRow = sqlTbl.Rows[0];
            TblRow.AcceptChanges();
        }
        else
            TblRow = null;

        if (_ieMode == "Update" || _ieMode == "New")
        {
            TblRow["Diag_Master_ID"] = _DiagID;
            TblRow["PatientIE_ID"] = _ieID;
            TblRow["BodyPart"] = bp.ToString().Trim();
            TblRow["DiagCode"] = dc.ToString().Trim();
            TblRow["Description"] = dcd.ToString().Trim();
            TblRow["PatientFU_ID"] = Session["patientFUId"].ToString();

            if (_ieMode == "New")
            {
                TblRow["CreatedBy"] = "Admin";
                TblRow["CreatedDate"] = DateTime.Now;
                sqlTbl.Rows.Add(TblRow);
            }
            sqlAdapt.Update(sqlTbl);
        }
        else if (_ieMode == "Delete")
        {
            TblRow.Delete();
            sqlAdapt.Update(sqlTbl);
        }
        if (TblRow != null)
            TblRow.Table.Dispose();
        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();
    }
    public void BindDCDataGrid()
    {
        //_CurIEid = Session["PatientIE_ID"].ToString();
      
        try
        {
            if (!IsPostBack)
            {
                if (_FuId == "" || _FuId == "0")
                    return;
                string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
                string SqlStr = "";

                SqlDataAdapter oSQLAdpr;
                DataTable Diagnosis = new DataTable();
                oSQLConn.ConnectionString = sProvider;
                oSQLConn.Open();
                SqlStr = "Select * from tblDiagCodesDetail WHERE PatientFU_ID = " + _FuId + " AND BodyPart LIKE '%" + _CurBP + "%' Order By BodyPart, Description";
                oSQLCmd.Connection = oSQLConn;
                oSQLCmd.CommandText = SqlStr;
                oSQLAdpr = new SqlDataAdapter(SqlStr, oSQLConn);
                oSQLAdpr.Fill(Diagnosis);
                dgvDiagCodes.DataSource = "";
                dgvDiagCodes.DataSource = Diagnosis.DefaultView;
                dgvDiagCodes.DataBind();
                oSQLAdpr.Dispose();
                oSQLConn.Close();
            }
            else
            {
                if (ViewState["DiagnosisList"] != null)
                {
                    List<Adddiagnosis> objList = (List<Adddiagnosis>)ViewState["DiagnosisList"];

                    dgvDiagCodes.DataSource = objList;
                    dgvDiagCodes.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void LoadDV_Click(object sender, ImageClickEventArgs e)// RoutedEventArgs
    {
        PopulateUIDefaults();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string ieMode = "New";
        SaveDiagnosis(Session["PatientIE_ID"].ToString());
        SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        // SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["patientFUId"].ToString());
        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
        }
    }
    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        bindgridPoup();
        // SaveUI(Session["patientFUId"].ToString(), ieMode, true);
        //// SaveStandards(Session["PatientIE_ID"].ToString());
        // Response.Redirect("AddDiagnosis.aspx");
    }
    public void bindDropdown()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(Server.MapPath("~/xml/HSMData.xml"));

        foreach (XmlNode node in doc.SelectNodes("//HSM/Levels/Level"))
        {
            cboLevels.Items.Add(new ListItem(node.Attributes["name"].InnerText, node.Attributes["name"].InnerText));
        }

    }


    protected void btnDaigSave_Click(object sender, EventArgs e)
    {
        SaveStandardsPopup(Session["PatientIE_ID"].ToString());
        BindDCDataGrid();
        txDesc.Text = string.Empty;
        ScriptManager.RegisterStartupScript(Page, this.GetType(), "TestFU", "closeModelPopup()", true);
    }

    public string SaveStandardsPopup(string ieID)
    {
        List<Adddiagnosis> objList = new List<Adddiagnosis>();
        Adddiagnosis obj = new Adddiagnosis();
        string ids = string.Empty;
        try
        {

            foreach (GridViewRow row in dgvDiagCodesPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    obj = new Adddiagnosis();
                    obj.Diag_Master_ID = dgvDiagCodesPopup.DataKeys[row.RowIndex].Value.ToString();
                    obj.BodyPart = row.Cells[1].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.DiagCode = row.Cells[2].Controls.OfType<Label>().FirstOrDefault().Text;
                    obj.Description = row.Cells[3].Controls.OfType<TextBox>().FirstOrDefault().Text;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.PN = row.Cells[4].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    obj.isChecked = row.Cells[0].Controls.OfType<CheckBox>().FirstOrDefault().Checked;
                    if (obj.isChecked)
                    {
                        ids += obj.DiagCode_ID + ",";
                        //  SaveStdUI(ieID, obj.DiagCode_ID, true, obj.BodyPart, obj.Description, obj.DiagCode);
                        objList.Add(obj);
                    }
                    //else
                    //{ SaveStdUI(ieID, obj.DiagCode_ID, false, obj.BodyPart, obj.Description, obj.DiagCode); }

                }
            }
            ViewState["DiagnosisList"] = objList;
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }
        return "";
    }


    private void bindgridPoup()
    {
        try
        {
            _FuId = Session["patientFUId"].ToString();
            string _CurBodyPart = _CurBP;
            string _SKey = "WHERE tblDiagCodes.Description LIKE '%" + txDesc.Text.Trim() + "%' AND BodyPart LIKE '%" + _CurBodyPart + "%'";
            DataSet ds = new DataSet();
            DataTable Standards = new DataTable();
            string SqlStr = "";
            if (_FuId != "")
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTSFU(" + _FuId + ", DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            else
                SqlStr = "Select tblDiagCodes.*, dbo.DIAGEXISTSFU('0', DiagCode_ID, '%" + _CurBodyPart + "%') as IsChkd FROM tblDiagCodes " + _SKey + " Order By BodyPart, Description";
            ds = gDbhelperobj.selectData(SqlStr);

            dgvDiagCodesPopup.DataSource = ds;
            dgvDiagCodesPopup.DataBind();
        }
        catch (Exception ex)
        {
            log.Error(ex.Message);
        }

    }

    protected void RemoveDiagCodesDetail(string PatientFU_ID)
    {
        try
        {
            string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            string SqlStr = "";

            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "delete tblDiagCodesDetail WHERE PatientFU_ID=" + PatientFU_ID + " and BodyPart like '%" + _CurBP + "%'";
            SqlCommand sqlCM = new SqlCommand(SqlStr, oSQLConn);
            sqlCM.ExecuteNonQuery();
            oSQLConn.Close();
        }
        catch (Exception ex)
        {
        }
    }

    public void checkTP()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        cboTPSide1.DataBind();
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/LowBack");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            if (isTP == false)
                divTP.Attributes.Add("style", "display:none");
            else
                divTP.Attributes.Add("style", "display:block");

        }
    }

    //protected void BindROM()
    //{

    //    _FuId = Session["patientFUId"].ToString();
    //    string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
    //    string SqlStr = "";
    //    oSQLConn.ConnectionString = sProvider;

    //    if (oSQLConn.State == ConnectionState.Closed)
    //        oSQLConn.Open();
    //    SqlStr = "Select * from tblFUbpLowback WHERE PatientFU_ID = " + _FuId;
    //    SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
    //    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
    //    DataTable sqlTbl = new DataTable();
    //    sqlAdapt.Fill(sqlTbl);
    //    oSQLConn.Close();
    //    if (sqlTbl.Rows.Count > 0)
    //    {
    //        string[] strname, strrom, strnormal;
    //        if (string.IsNullOrEmpty(sqlTbl.Rows[0]["NameROM"].ToString()) == false)
    //        {
    //            strname = sqlTbl.Rows[0]["NameROM"].ToString().Split(',');
    //            strrom = sqlTbl.Rows[0]["ROM"].ToString().Split(',');
    //            strnormal = sqlTbl.Rows[0]["NormalROM"].ToString().Split(',');


    //            // Create the Table
    //            DataTable OrdersTable = new DataTable("ROM");
    //            // Build the Orders schema
    //            OrdersTable.Columns.Add("name", Type.GetType("System.String"));
    //            OrdersTable.Columns.Add("rom", Type.GetType("System.String"));
    //            OrdersTable.Columns.Add("normal", Type.GetType("System.String"));

    //            DataRow workRow;

    //            for (int i = 0; i < strname.Length; i++)
    //            {

    //                workRow = OrdersTable.NewRow();
    //                workRow[0] = strname[i];
    //                workRow[1] = strrom[i];
    //                workRow[2] = strnormal[i];
    //                OrdersTable.Rows.Add(workRow);
    //            }

    //            if (OrdersTable.Rows.Count != 0)
    //            {
    //                repROM.DataSource = OrdersTable;
    //                repROM.DataBind();
    //            }
    //        }
    //        else
    //            getXMLROMvalue();
    //    }
    //    else
    //    {
    //        getXMLROMvalue();
    //    }
    //}

    //private void getXMLROMvalue()
    //{
    //    //open the tender xml file  
    //    XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Lowback.xml"));
    //    //reading the xml data  
    //    DataSet ds = new DataSet();
    //    ds.ReadXml(xmlreader);
    //    xmlreader.Close();
    //    //if ds is not empty  
    //    if (ds.Tables.Count != 0)
    //    {
    //        repROM.DataSource = ds;
    //        repROM.DataBind();
    //    }
    //}

}