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

public partial class EditFuShoulder : System.Web.UI.Page
{
    SqlConnection oSQLConn = new SqlConnection();
    SqlCommand oSQLCmd = new SqlCommand();
    private bool _fldPop = false;
    public string _CurIEid = "";
    public string _FuId = "";
    public string _CurBP = "Shoulder";
    string Position = "";
    string pos = "";
    DBHelperClass gDbhelperobj = new DBHelperClass();

    ILog log = log4net.LogManager.GetLogger(typeof(EditFuShoulder));

    protected void Page_Load(object sender, EventArgs e)
    {
        Position = Request.QueryString["P"];
        switch (Position)
        {
            case "L":
                pos = "left";
                break;
            case "R":
                pos = "right";
                break;
            case "B":
                pos = "bilateral";
                break;

        }
        Session["PageName"] = "Shoulder";
        if (Session["uname"] == null)
            Response.Redirect("Login.aspx");
        if (!IsPostBack)
        {
            //BindROM();
            checkTP();
            if (Session["PatientIE_ID"] != null && Session["patientFUId"] != null)
            {
                _CurIEid = Session["PatientIE_ID"].ToString();
                _FuId = Session["patientFUId"].ToString();
                SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString);
                DBHelperClass db = new DBHelperClass();
                string query = ("select count(*) as FuCount FROM tblFUbpShoulder WHERE PatientFU_ID = " + _FuId + "");
                SqlCommand cm = new SqlCommand(query, cn);
                SqlDataAdapter Fuda = new SqlDataAdapter(cm);
                cn.Open();
                DataSet FUds = new DataSet();
                Fuda.Fill(FUds);
                cn.Close();
                string query1 = ("select count(*) as IECount FROM tblbpShoulder WHERE PatientIE_ID= " + _CurIEid + "");
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
                if (Position != "")
                {
                    switch (Position)
                    {
                        case "L":

                            //first div
                            WrapLeft.Visible = true;
                            wrpRight.Visible = false;
                            //second div
                            wrpPELeft.Visible = true;
                            wrpPERight.Visible = false;
                            //Right
                            txtAbductionLeft.ReadOnly = true;
                            txtFlexionLeft.ReadOnly = true;
                            txtExtRotationLeft.ReadOnly = true;
                            txtIntRotationLeft.ReadOnly = true;
                            //Left
                            txtAbductionLeftWas.ReadOnly = false;
                            txtFlexionLeftWas.ReadOnly = false;
                            txtExtRotationLeftWas.ReadOnly = false;
                            txtIntRotationLeftWas.ReadOnly = false;
                            //Left checkbox
                            chkNeerLeft.Enabled = true;
                            chkHawkinLeft.Enabled = true;
                            chkYergasonsLeft.Enabled = true;
                            chkDropArmLeft.Enabled = true;
                            chkReverseBeerLeft.Enabled = true;
                            //Right checkbox
                            chkNeerRight.Enabled = false;
                            chkHawkinRight.Enabled = false;
                            chkYergasonsRight.Enabled = false;
                            chkDropArmRight.Enabled = false;
                            chkReverseBeerRight.Enabled = false;
                            //Left 
                            //cboTPSide1.DataBind();
                            //cboTPSide1.SelectedValue = "left";

                            //cboTPSide2.DataBind();
                            //cboTPSide2.SelectedValue = "left";

                            //cboTPSide3.DataBind();
                            //cboTPSide3.SelectedValue = "left";

                            //cboTPSide4.DataBind();
                            //cboTPSide4.SelectedValue = "left";

                            //cboTPSide5.DataBind();
                            //cboTPSide5.SelectedValue = "left";

                            //cboTPSide6.DataBind();
                            //cboTPSide6.SelectedValue = "left";

                            //cboTPSide7.DataBind();
                            //cboTPSide7.SelectedValue = "left";

                            //cboTPSide8.DataBind();
                            //cboTPSide8.SelectedValue = "left";

                            break;
                        case "R":
                            pos = "right";
                            //first div
                            wrpRight.Visible = true;
                            WrapLeft.Visible = false;
                            //second div
                            wrpPELeft.Visible = false;
                            wrpPERight.Visible = true;
                            //Left
                            txtAbductionLeftWas.ReadOnly = true;
                            txtFlexionLeftWas.ReadOnly = true;
                            txtExtRotationLeftWas.ReadOnly = true;
                            txtIntRotationLeftWas.ReadOnly = true;
                            //right
                            txtAbductionLeft.ReadOnly = false;
                            txtFlexionLeft.ReadOnly = false;
                            txtExtRotationLeft.ReadOnly = false;
                            txtIntRotationLeft.ReadOnly = false;
                            //Left checkbox
                            chkNeerLeft.Enabled = false;
                            chkHawkinLeft.Enabled = false;
                            chkYergasonsLeft.Enabled = false;
                            chkDropArmLeft.Enabled = false;
                            chkReverseBeerLeft.Enabled = false;
                            //Right checkbox
                            chkNeerRight.Enabled = true;
                            chkHawkinRight.Enabled = true;
                            chkYergasonsRight.Enabled = true;
                            chkDropArmRight.Enabled = true;
                            chkReverseBeerRight.Enabled = true;
                            //Dropdown
                            //cboTPSide1.DataBind();
                            //cboTPSide1.SelectedValue = "right";

                            //cboTPSide2.DataBind();
                            //cboTPSide2.SelectedValue = "right";

                            //cboTPSide3.DataBind();
                            //cboTPSide3.SelectedValue = "right";

                            //cboTPSide4.DataBind();
                            //cboTPSide4.SelectedValue = "right";

                            //cboTPSide5.DataBind();
                            //cboTPSide5.SelectedValue = "right";

                            //cboTPSide6.DataBind();
                            //cboTPSide6.SelectedValue = "right";

                            //cboTPSide7.DataBind();
                            //cboTPSide7.SelectedValue = "right";

                            //cboTPSide8.DataBind();
                            //cboTPSide8.SelectedValue = "right";

                            break;
                        case "B":
                            pos = "bilateral";
                            //first div
                            wrpRight.Visible = true;
                            WrapLeft.Visible = true;
                            //second div
                            wrpPELeft.Visible = true;
                            wrpPERight.Visible = true;
                            //Left
                            txtAbductionLeftWas.ReadOnly = false;
                            txtFlexionLeftWas.ReadOnly = false;
                            txtExtRotationLeftWas.ReadOnly = false;
                            txtIntRotationLeftWas.ReadOnly = false;
                            //right
                            txtAbductionLeft.ReadOnly = false;
                            txtFlexionLeft.ReadOnly = false;
                            txtExtRotationLeft.ReadOnly = false;
                            txtIntRotationLeft.ReadOnly = false;
                            //Left checkbox
                            chkNeerLeft.Enabled = true;
                            chkHawkinLeft.Enabled = true;
                            chkYergasonsLeft.Enabled = true;
                            chkDropArmLeft.Enabled = true;
                            chkReverseBeerLeft.Enabled = true;
                            //Right checkbox
                            chkNeerRight.Enabled = true;
                            chkHawkinRight.Enabled = true;
                            chkYergasonsRight.Enabled = true;
                            chkDropArmRight.Enabled = true;
                            chkReverseBeerRight.Enabled = true;
                            //Dropdown
                            //cboTPSide1.DataBind();
                            //cboTPSide1.SelectedValue = "bilateral";

                            //cboTPSide2.DataBind();
                            //cboTPSide2.SelectedValue = "bilateral";

                            //cboTPSide3.DataBind();
                            //cboTPSide3.SelectedValue = "bilateral";

                            //cboTPSide4.DataBind();
                            //cboTPSide4.SelectedValue = "bilateral";

                            //cboTPSide5.DataBind();
                            //cboTPSide5.SelectedValue = "bilateral";

                            //cboTPSide6.DataBind();
                            //cboTPSide6.SelectedValue = "bilateral";

                            //cboTPSide7.DataBind();
                            //cboTPSide7.SelectedValue = "bilateral";

                            //cboTPSide8.DataBind();
                            //cboTPSide8.SelectedValue = "bilateral";

                            break;
                    }
                }
            }
            else
            {
                Response.Redirect("EditFU.aspx");
            }
        }
        BindDCDataGrid();
        Logger.Info(Session["uname"].ToString() + "- Visited in  EditFuShoulder for -" + Convert.ToString(Session["LastNameFUEdit"]) + Convert.ToString(Session["FirstNameFUEdit"]) + "-" + DateTime.Now);
    }


    public string SaveUI(string ieID, string fuID, string ieMode, bool bpChecked)
    {
        try
        {
            _CurIEid = Session["PatientIE_ID"].ToString();
            _FuId = Session["patientFUId"].ToString();
            long _fuID = Convert.ToInt64(_FuId);
            string _ieMode = "";

            string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
            string SqlStr = "";
            oSQLConn.ConnectionString = sProvider;
            oSQLConn.Open();
            SqlStr = "Select * from tblFUbpShoulder WHERE PatientFU_ID = " + _FuId;
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
                TblRow["PatientFU_ID"] = _FuId;
                TblRow["ConstantLeft"] = chkContentLeft.Checked;
                TblRow["ConstantRight"] = chkContentRight.Checked;
                TblRow["IntermittentLeft"] = chkIntermittentLeft.Checked;
                TblRow["IntermittentRight"] = chkIntermittentRight.Checked;
                TblRow["PainScaleLeft"] = txtPainScaleLeft.Text.ToString();
                TblRow["SharpLeft"] = chkSharpLeft.Checked;
                TblRow["ElectricLeft"] = chkSharpLeft.Checked;
                TblRow["ShootingLeft"] = chkShootingLeft.Checked;
                TblRow["ThrobblingLeft"] = chkThrobblingLeft.Checked;
                TblRow["PulsatingLeft"] = chkPulsatingLeft.Checked;
                TblRow["DullLeft"] = chkDullLeft.Checked;
                TblRow["AchyLeft"] = chkAchyLeft.Checked;
                TblRow["WorseLyingLeft"] = chkWorseLyingLeft.Checked;
                TblRow["WorseMovementLeft"] = chkWorseMovementLeft.Checked;
                TblRow["WorseRaisingLeft"] = false;
                TblRow["WorseLiftingLeft"] = chkWorseLiftingLeft.Checked;
                TblRow["WorseRotationLeft"] = chkWorseRotationLeft.Checked;
                TblRow["WorseWorkingLeft"] = chkWorseWorkingLeft.Checked;
                TblRow["WorseActivitiesLeft"] = chkWorseActivitiesLeft.Checked;
                TblRow["ImprovedRestingLeft"] = chkImprovedRestingLeft.Checked;
                TblRow["ImprovedMedicationLeft"] = chkImprovedMedicationLeft.Checked;
                TblRow["ImprovedTherapyLeft"] = chkImprovedTherapyLeft.Checked;
                TblRow["ImprovedSleepingLeft"] = chkImprovedSleepingLeft.Checked;
                TblRow["PainScaleRight"] = txtPainScaleRight.Text.ToString();
                TblRow["SharpRight"] = chkSharpRight.Checked;
                TblRow["ElectricRight"] = chkElectricRight.Checked;
                TblRow["ShootingRight"] = chkShootingRight.Checked;
                TblRow["ThrobblingRight"] = chkThrobblingRight.Checked;
                TblRow["PulsatingRight"] = chkPulsatingRight.Checked;
                TblRow["DullRight"] = chkDullRight.Checked;
                TblRow["AchyRight"] = chkAchyRight.Checked;
                TblRow["WorseLyingRight"] = chkWorseLyingRight.Checked;
                TblRow["WorseMovementRight"] = chkWorseMovementRight.Checked;
                TblRow["WorseRaisingRight"] = false;// chkWorseRaisingRight.Checked;
                TblRow["WorseLiftingRight"] = chkWorseLiftingRight.Checked;
                TblRow["WorseRotationRight"] = chkWorseRotationRight.Checked;
                TblRow["WorseWorkingRight"] = chkWorseWorkingRight.Checked;
                TblRow["WorseActivitiesRight"] = chkWorseActivitiesRight.Checked;
                TblRow["ImprovedRestingRight"] = chkImprovedRestingRight.Checked;
                TblRow["ImprovedMedicationRight"] = chkImprovedMedicationRight.Checked;
                TblRow["ImprovedTherapyRight"] = chkImprovedTherapyRight.Checked;
                TblRow["ImprovedSleepingRight"] = chkImprovedSleepingRight.Checked;

                //Left values
                TblRow["AbductionLeft"] = txtAbductionLeftWas.Text.ToString();
                TblRow["FlexionLeft"] = txtFlexionLeftWas.Text.ToString();
                TblRow["ExtRotationLeft"] = txtExtRotationLeftWas.Text.ToString();
                TblRow["IntRotationLeft"] = txtIntRotationLeftWas.Text.ToString();
                // Right values.
                TblRow["AbductionRight"] = txtAbductionLeft.Text.ToString();
                TblRow["FlexionRight"] = txtFlexionLeft.Text.ToString();
                TblRow["ExtRotationRight"] = txtExtRotationLeft.Text.ToString();
                TblRow["IntRotationRight"] = txtIntRotationLeft.Text.ToString();
                // Normal Value.
                TblRow["AbductionNormal"] = txtAbductionRightWas.Text.ToString();
                TblRow["FlexionNormal"] = txtFlexionRightWas.Text.ToString();
                TblRow["ExtRotationNormal"] = txtExtRotationRightWas.Text.ToString();
                TblRow["IntRotationNormal"] = txtIntRotationRightWas.Text.ToString();

                TblRow["PalpationText1Left"] = txtPalpationText1Left.Text.ToString();
                // TblRow["PalpationText2Left"] = txtPalpationText2Left.Text.ToString();
                TblRow["ACJointLeft"] = chkACJointLeft.Checked;
                TblRow["GlenohumeralLeft"] = chkGlenohumeralLeft.Checked;
                TblRow["CorticoidLeft"] = chkCorticoidLeft.Checked;
                TblRow["SupraspinatusLeft"] = chkSupraspinatusLeft.Checked;
                TblRow["ScapularLeft"] = chkScapularLeft.Checked;
                TblRow["DeepLabralLeft"] = chkDeepLabralLeft.Checked;
                TblRow["DeltoidLeft"] = chkDeltoidLeft.Checked;
                TblRow["TrapeziusLeft"] = chkTrapeziusLeft.Checked;
                TblRow["EccymosisLeft"] = chkEccymosisLeft.Checked;
                TblRow["EdemaLeft"] = chkEdemaLeft.Checked;
                TblRow["RangeOfMotionLeft"] = chkRangeOfMotionLeft.Checked;
                TblRow["PalpationText1Right"] = txtPalpationText1Right.Text.ToString();
                // TblRow["PalpationText2Right"] = txtPalpationText2Right.Text.ToString();
                TblRow["ACJointRight"] = chkACJointRight.Checked;
                TblRow["GlenohumeralRight"] = chkGlenohumeralRight.Checked;
                TblRow["CorticoidRight"] = chkCorticoidRight.Checked;
                TblRow["SupraspinatusRight"] = chkSupraspinatusRight.Checked;
                TblRow["ScapularRight"] = chkScapularRight.Checked;
                TblRow["DeepLabralRight"] = chkDeepLabralRight.Checked;
                TblRow["DeltoidRight"] = chkDeltoidRight.Checked;
                TblRow["TrapeziusRight"] = chkTrapeziusRight.Checked;
                TblRow["EccymosisRight"] = chkEccymosisRight.Checked;
                TblRow["EdemaRight"] = chkEdemaRight.Checked;
                TblRow["RangeOfMotionRight"] = chkRangeOfMotionRight.Checked;
                TblRow["NeerLeft"] = chkNeerLeft.Checked;
                TblRow["HawkinLeft"] = chkHawkinLeft.Checked;
                TblRow["YergasonsLeft"] = chkYergasonsLeft.Checked;
                TblRow["DropArmLeft"] = chkDropArmLeft.Checked;
                TblRow["ReverseBeerLeft"] = chkReverseBeerLeft.Checked;
                TblRow["NeerRight"] = chkNeerRight.Checked;
                TblRow["HawkinRight"] = chkHawkinRight.Checked;
                TblRow["YergasonsRight"] = chkYergasonsRight.Checked;
                TblRow["DropArmRight"] = chkDropArmRight.Checked;
                TblRow["ReverseBeerRight"] = chkReverseBeerRight.Checked;
                TblRow["TPSide1"] = cboTPSide1.Text.ToString();
                TblRow["TPText1"] = txtTPText1.Text.ToString();
                TblRow["TPSide2"] = cboTPSide2.Text.ToString();
                TblRow["TPText2"] = txtTPText2.Text.ToString();
                TblRow["TPSide3"] = cboTPSide3.Text.ToString();
                TblRow["TPText3"] = txtTPText3.Text.ToString();
                TblRow["TPSide4"] = cboTPSide4.Text.ToString();
                TblRow["TPText4"] = txtTPText4.Text.ToString();
                TblRow["TPSide5"] = cboTPSide5.Text.ToString();
                TblRow["TPText5"] = txtTPText5.Text.ToString();
                TblRow["TPSide6"] = cboTPSide6.Text.ToString();
                TblRow["TPText6"] = txtTPText6.Text.ToString();
                TblRow["TPSide7"] = cboTPSide7.Text.ToString();
                TblRow["TPText7"] = txtTPText7.Text.ToString();
                TblRow["TPSide8"] = cboTPSide8.Text.ToString();
                TblRow["TPText8"] = txtTPText8.Text.ToString();
                TblRow["FreeForm"] = txtFreeForm.Text.ToString();
                TblRow["FreeFormCC"] = txtFreeFormCC.Text.ToString();
                TblRow["FreeFormA"] = txtFreeFormA.Text.ToString();
                TblRow["FreeFormP"] = txtFreeFormP.Text.ToString();
                //TblRow["SprainStrainSide"] = cboSprainStrainSide.Text.ToString();
                //TblRow["SprainStrain"] = chkSprainStrain.Checked;
                //TblRow["DerangmentSide"] = cboDerangmentSide.Text.ToString();
                //TblRow["Derangment"] = chkDerangment.Checked;
                //TblRow["SyndromeSide"] = cboSyndromeSide.Text.ToString();
                //TblRow["Syndrome"] = chkSyndrome.Checked;
                //TblRow["Plan"] = chkPlan.Checked;
                //TblRow["ScanType"] = cboScanType.Text.ToString();
                //TblRow["ScanSide"] = cboScanSide.Text.ToString();
                TblRow["ElectricLeft"] = chkElectricLeft.Checked;

                //string strname = "", strleft = "", strright = "", strnormal = "";

                //for (int i = 0; i < repROM.Items.Count; i++)
                //{
                //    Label lblname = repROM.Items[i].FindControl("lblname") as Label;
                //    TextBox txtleft = repROM.Items[i].FindControl("txtleft") as TextBox;
                //    TextBox txtright = repROM.Items[i].FindControl("txtright") as TextBox;
                //    TextBox txtnormal = repROM.Items[i].FindControl("txtnormal") as TextBox;

                //    strname = strname + "," + lblname.Text;
                //    strleft = strleft + "," + txtleft.Text;
                //    strright = strright + "," + txtright.Text;
                //    strnormal = strnormal + "," + txtnormal.Text;
                //}

                //TblRow["LeftROM"] = strleft.Substring(1);
                //TblRow["RightROM"] = strright.Substring(1);
                //TblRow["NormalROM"] = strnormal.Substring(1);
                //TblRow["NameROM"] = strname.Substring(1);


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
                return "Shoulder has been added...";
            else if (_ieMode == "Update")
                return "Shoulder has been updated...";
            else if (_ieMode == "Delete")
                return "Shoulder has been deleted...";
            else
                return "";
        }
        catch (Exception ex)
        {
            return "";
        }
    }
    public void PopulateStrightFwd()
    {
        //bool bLeft = bL;
        //bool bRight = bR;
        //tbRomLIs.Text = "Left";
        //tbRomLWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbRomRIs.Text = "Right";
        //tbRomRWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtAbductionRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtAbductionLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFlexionRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtFlexionLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtExtRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtIntRotationRightWas.Visibility = System.Windows.Visibility.Collapsed;
        //txtIntRotationLeftWas.Visibility = System.Windows.Visibility.Collapsed;
        //tbNormal.Visibility = System.Windows.Visibility.Visible;
        //txtAbductionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtExtRotationNormal.Visibility = System.Windows.Visibility.Visible;
        //txtFlexionNormal.Visibility = System.Windows.Visibility.Visible;
        //txtIntRotationNormal.Visibility = System.Windows.Visibility.Visible;

        //wrpPELeft.IsEnabled = bLeft;
        //grdROMLeft.IsEnabled = bLeft;
        //wrpLeft1.IsEnabled = bLeft;
        //wrpLeft2.IsEnabled = bLeft;
        //wrpLeft3.IsEnabled = bLeft;
        //grdTestLeft.IsEnabled = bLeft;

        //txtAbductionLeft.IsEnabled = bLeft;
        //txtFlexionLeft.IsEnabled = bLeft;
        //txtExtRotationLeft.IsEnabled = bLeft;
        //txtIntRotationLeft.IsEnabled = bLeft;
        //txtAbductionLeftWas.IsEnabled = bLeft;
        //txtFlexionLeftWas.IsEnabled = bLeft;
        //txtExtRotationLeftWas.IsEnabled = bLeft;
        //txtIntRotationLeftWas.IsEnabled = bLeft;

        //wrpPERight.IsEnabled = bRight;
        //grdROMRight.IsEnabled = bRight;
        //wrpRight1.IsEnabled = bRight;
        //wrpRight2.IsEnabled = bRight;
        //grdTestRight.IsEnabled = bRight;


        //txtAbductionRight.IsEnabled = bRight;
        //txtFlexionRight.IsEnabled = bRight;
        //txtExtRotationRight.IsEnabled = bRight;
        //txtIntRotationRight.IsEnabled = bRight;
        //txtAbductionRightWas.IsEnabled = bRight;
        //txtFlexionRightWas.IsEnabled = bRight;
        //txtExtRotationRightWas.IsEnabled = bRight;
        //txtIntRotationRightWas.IsEnabled = bRight;

        //chkNeerLeft.IsEnabled = bLeft;
        //chkNeerLeft.IsChecked = bLeft;
        //chkHawkinLeft.IsEnabled = bLeft;
        //chkHawkinLeft.IsChecked = bLeft;
        //chkYergasonsLeft.IsEnabled = bLeft;
        //chkDropArmLeft.IsEnabled = bLeft;
        //chkReverseBeerLeft.IsEnabled = bLeft;

        //chkNeerRight.IsEnabled = bRight;
        //chkNeerRight.IsChecked = bRight;
        //chkHawkinRight.IsEnabled = bRight;
        //chkHawkinRight.IsChecked = bRight;
        //chkYergasonsRight.IsEnabled = bRight;
        //chkDropArmRight.IsEnabled = bRight;
        //chkReverseBeerRight.IsEnabled = bRight;

        //if (bLeft && bRight)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = cboTPSide1.SelectedIndex =
        //    cboTPSide2.SelectedIndex = cboTPSide7.SelectedIndex = 3;
        //else if (bLeft)
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = cboTPSide1.SelectedIndex =
        //    cboTPSide2.SelectedIndex = cboTPSide7.SelectedIndex = 1;
        //else
        //    cboScanSide.SelectedIndex = cboSprainStrainSide.SelectedIndex =
        //    cboDerangmentSide.SelectedIndex = cboTPSide1.SelectedIndex =
        //    cboTPSide2.SelectedIndex = cboTPSide7.SelectedIndex = 2;
    }

    public void PopulateUI(string fuID)
    {


        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblFUbpShoulder WHERE PatientFU_ID = " + fuID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString().Trim();
            chkContentLeft.Checked = CommonConvert.ToBoolean(TblRow["ConstantLeft"].ToString());
            chkContentRight.Checked = CommonConvert.ToBoolean(TblRow["ConstantRight"].ToString());

            chkIntermittentLeft.Checked = CommonConvert.ToBoolean(TblRow["IntermittentLeft"].ToString());
            chkIntermittentRight.Checked = CommonConvert.ToBoolean(TblRow["IntermittentRight"].ToString());

            chkSharpLeft.Checked = CommonConvert.ToBoolean(TblRow["SharpLeft"].ToString());
            chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            chkShootingLeft.Checked = CommonConvert.ToBoolean(TblRow["ShootingLeft"].ToString());
            chkThrobblingLeft.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingLeft"].ToString());
            chkPulsatingLeft.Checked = CommonConvert.ToBoolean(TblRow["PulsatingLeft"].ToString());
            chkDullLeft.Checked = CommonConvert.ToBoolean(TblRow["DullLeft"].ToString());
            chkAchyLeft.Checked = CommonConvert.ToBoolean(TblRow["AchyLeft"].ToString());
            chkWorseLyingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingLeft"].ToString());
            chkWorseMovementLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementLeft"].ToString());
            //  chkWorseRaisingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseRaisingLeft"].ToString());
            chkWorseLiftingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseLiftingLeft"].ToString());
            chkWorseRotationLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseRotationLeft"].ToString());
            chkWorseWorkingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseWorkingLeft"].ToString());
            chkWorseActivitiesLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesLeft"].ToString());
            chkImprovedRestingLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedRestingLeft"].ToString());
            chkImprovedMedicationLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedicationLeft"].ToString());
            chkImprovedTherapyLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapyLeft"].ToString());
            chkImprovedSleepingLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleepingLeft"].ToString());
            txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString().Trim();
            chkSharpRight.Checked = CommonConvert.ToBoolean(TblRow["SharpRight"].ToString());
            chkElectricRight.Checked = CommonConvert.ToBoolean(TblRow["ElectricRight"].ToString());
            chkShootingRight.Checked = CommonConvert.ToBoolean(TblRow["ShootingRight"].ToString());
            chkThrobblingRight.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingRight"].ToString());
            chkPulsatingRight.Checked = CommonConvert.ToBoolean(TblRow["PulsatingRight"].ToString());
            chkDullRight.Checked = CommonConvert.ToBoolean(TblRow["DullRight"].ToString());
            chkAchyRight.Checked = CommonConvert.ToBoolean(TblRow["AchyRight"].ToString());
            chkWorseLyingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingRight"].ToString());
            chkWorseMovementRight.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementRight"].ToString());
            //chkWorseRaisingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseRaisingRight"].ToString());
            chkWorseLiftingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseLiftingRight"].ToString());
            chkWorseRotationRight.Checked = CommonConvert.ToBoolean(TblRow["WorseRotationRight"].ToString());
            chkWorseWorkingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseWorkingRight"].ToString());
            chkWorseActivitiesRight.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesRight"].ToString());
            chkImprovedRestingRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedRestingRight"].ToString());
            chkImprovedMedicationRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedicationRight"].ToString());
            chkImprovedTherapyRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapyRight"].ToString());
            chkImprovedSleepingRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleepingRight"].ToString());

            txtAbductionLeftWas.Text = TblRow["AbductionLeft"].ToString().Trim();
            txtFlexionLeftWas.Text = TblRow["FlexionLeft"].ToString().Trim();
            txtExtRotationLeftWas.Text = TblRow["ExtRotationLeft"].ToString().Trim();
            txtIntRotationLeftWas.Text = TblRow["IntRotationLeft"].ToString().Trim();

            txtAbductionLeft.Text = TblRow["AbductionRight"].ToString().Trim();
            txtFlexionLeft.Text = TblRow["FlexionRight"].ToString().Trim();
            txtExtRotationLeft.Text = TblRow["ExtRotationRight"].ToString().Trim();
            txtIntRotationLeft.Text = TblRow["IntRotationRight"].ToString().Trim();

            txtAbductionRightWas.Text = TblRow["AbductionNormal"].ToString().Trim();
            txtFlexionRightWas.Text = TblRow["FlexionNormal"].ToString().Trim();
            txtExtRotationRightWas.Text = TblRow["ExtRotationNormal"].ToString().Trim();
            txtIntRotationRightWas.Text = TblRow["IntRotationNormal"].ToString().Trim();

            txtPalpationText1Left.Text = TblRow["PalpationText1Left"].ToString().Trim();
            // txtPalpationText2Left.Text = TblRow["PalpationText2Left"].ToString().Trim();
            chkACJointLeft.Checked = CommonConvert.ToBoolean(TblRow["ACJointLeft"].ToString());
            chkGlenohumeralLeft.Checked = CommonConvert.ToBoolean(TblRow["GlenohumeralLeft"].ToString());
            chkCorticoidLeft.Checked = CommonConvert.ToBoolean(TblRow["CorticoidLeft"].ToString());
            chkSupraspinatusLeft.Checked = CommonConvert.ToBoolean(TblRow["SupraspinatusLeft"].ToString());
            chkScapularLeft.Checked = CommonConvert.ToBoolean(TblRow["ScapularLeft"].ToString());
            chkDeepLabralLeft.Checked = CommonConvert.ToBoolean(TblRow["DeepLabralLeft"].ToString());
            chkDeltoidLeft.Checked = CommonConvert.ToBoolean(TblRow["DeltoidLeft"].ToString());
            chkTrapeziusLeft.Checked = CommonConvert.ToBoolean(TblRow["TrapeziusLeft"].ToString());
            chkEccymosisLeft.Checked = CommonConvert.ToBoolean(TblRow["EccymosisLeft"].ToString());
            chkEdemaLeft.Checked = CommonConvert.ToBoolean(TblRow["EdemaLeft"].ToString());
            chkRangeOfMotionLeft.Checked = CommonConvert.ToBoolean(TblRow["RangeOfMotionLeft"].ToString());
            txtPalpationText1Right.Text = TblRow["PalpationText1Right"].ToString().Trim();
            //txtPalpationText2Right.Text = TblRow["PalpationText2Right"].ToString().Trim();
            chkACJointRight.Checked = CommonConvert.ToBoolean(TblRow["ACJointRight"].ToString());
            chkGlenohumeralRight.Checked = CommonConvert.ToBoolean(TblRow["GlenohumeralRight"].ToString());
            chkCorticoidRight.Checked = CommonConvert.ToBoolean(TblRow["CorticoidRight"].ToString());
            chkSupraspinatusRight.Checked = CommonConvert.ToBoolean(TblRow["SupraspinatusRight"].ToString());
            chkScapularRight.Checked = CommonConvert.ToBoolean(TblRow["ScapularRight"].ToString());
            chkDeepLabralRight.Checked = CommonConvert.ToBoolean(TblRow["DeepLabralRight"].ToString());
            chkDeltoidRight.Checked = CommonConvert.ToBoolean(TblRow["DeltoidRight"].ToString());
            chkTrapeziusRight.Checked = CommonConvert.ToBoolean(TblRow["TrapeziusRight"].ToString());
            chkEccymosisRight.Checked = CommonConvert.ToBoolean(TblRow["EccymosisRight"].ToString());
            chkEdemaRight.Checked = CommonConvert.ToBoolean(TblRow["EdemaRight"].ToString());
            chkRangeOfMotionRight.Checked = CommonConvert.ToBoolean(TblRow["RangeOfMotionRight"].ToString());
            chkNeerLeft.Checked = CommonConvert.ToBoolean(TblRow["NeerLeft"].ToString());
            chkHawkinLeft.Checked = CommonConvert.ToBoolean(TblRow["HawkinLeft"].ToString());
            chkYergasonsLeft.Checked = CommonConvert.ToBoolean(TblRow["YergasonsLeft"].ToString());
            chkDropArmLeft.Checked = CommonConvert.ToBoolean(TblRow["DropArmLeft"].ToString());
            chkReverseBeerLeft.Checked = CommonConvert.ToBoolean(TblRow["ReverseBeerLeft"].ToString());
            chkNeerRight.Checked = CommonConvert.ToBoolean(TblRow["NeerRight"].ToString());
            chkHawkinRight.Checked = CommonConvert.ToBoolean(TblRow["HawkinRight"].ToString());
            chkYergasonsRight.Checked = CommonConvert.ToBoolean(TblRow["YergasonsRight"].ToString());
            chkDropArmRight.Checked = CommonConvert.ToBoolean(TblRow["DropArmRight"].ToString());
            chkReverseBeerRight.Checked = CommonConvert.ToBoolean(TblRow["ReverseBeerRight"].ToString());
            cboTPSide1.Text = TblRow["TPSide1"].ToString().Trim();
            txtTPText1.Text = TblRow["TPText1"].ToString().Trim();
            cboTPSide2.Text = TblRow["TPSide2"].ToString().Trim();
            txtTPText2.Text = TblRow["TPText2"].ToString().Trim();
            cboTPSide3.Text = TblRow["TPSide3"].ToString().Trim();
            txtTPText3.Text = TblRow["TPText3"].ToString().Trim();
            cboTPSide4.Text = TblRow["TPSide4"].ToString().Trim();
            txtTPText4.Text = TblRow["TPText4"].ToString().Trim();
            cboTPSide5.Text = TblRow["TPSide5"].ToString().Trim();
            txtTPText5.Text = TblRow["TPText5"].ToString().Trim();
            cboTPSide6.Text = TblRow["TPSide6"].ToString().Trim();
            txtTPText6.Text = TblRow["TPText6"].ToString().Trim();
            cboTPSide7.Text = TblRow["TPSide7"].ToString().Trim();
            txtTPText7.Text = TblRow["TPText7"].ToString().Trim();
            cboTPSide8.Text = TblRow["TPSide8"].ToString().Trim();
            txtTPText8.Text = TblRow["TPText8"].ToString().Trim();
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            //cboSprainStrainSide.Text = TblRow["SprainStrainSide"].ToString().Trim();
            //chkSprainStrain.Checked = Convert.ToBoolean(TblRow["SprainStrain"]);
            //cboDerangmentSide.Text = TblRow["DerangmentSide"].ToString().Trim();
            //chkDerangment.Checked = Convert.ToBoolean(TblRow["Derangment"]);
            //cboSyndromeSide.Text = TblRow["SyndromeSide"].ToString().Trim();
            //chkSyndrome.Checked = Convert.ToBoolean(TblRow["Syndrome"]);
            //chkPlan.Checked = Convert.ToBoolean(TblRow["Plan"]);
            //cboScanType.Text = TblRow["ScanType"].ToString().Trim();
            //cboScanSide.Text = TblRow["ScanSide"].ToString().Trim();
            chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            _fldPop = false;
        }

        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }

    public void PopulateIEUI(string ieID)
    {


        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
        string SqlStr = "";
        oSQLConn.ConnectionString = sProvider;
        oSQLConn.Open();
        SqlStr = "Select * from tblbpShoulder WHERE PatientIE_ID = " + ieID;
        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
        DataTable sqlTbl = new DataTable();
        sqlAdapt.Fill(sqlTbl);
        DataRow TblRow;

        if (sqlTbl.Rows.Count > 0)
        {
            _fldPop = true;
            TblRow = sqlTbl.Rows[0];
            txtPainScaleLeft.Text = TblRow["PainScaleLeft"].ToString().Trim();
            chkContentLeft.Checked = CommonConvert.ToBoolean(TblRow["ConstantLeft"].ToString());
            chkContentRight.Checked = CommonConvert.ToBoolean(TblRow["ConstantRight"].ToString());

            chkIntermittentLeft.Checked = CommonConvert.ToBoolean(TblRow["IntermittentLeft"].ToString());
            chkIntermittentRight.Checked = CommonConvert.ToBoolean(TblRow["IntermittentRight"].ToString());

            chkSharpLeft.Checked = CommonConvert.ToBoolean(TblRow["SharpLeft"].ToString());
            chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            chkShootingLeft.Checked = CommonConvert.ToBoolean(TblRow["ShootingLeft"].ToString());
            chkThrobblingLeft.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingLeft"].ToString());
            chkPulsatingLeft.Checked = CommonConvert.ToBoolean(TblRow["PulsatingLeft"].ToString());
            chkDullLeft.Checked = CommonConvert.ToBoolean(TblRow["DullLeft"].ToString());
            chkAchyLeft.Checked = CommonConvert.ToBoolean(TblRow["AchyLeft"].ToString());
            chkWorseLyingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingLeft"].ToString());
            chkWorseMovementLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementLeft"].ToString());
            // chkWorseRaisingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseRaisingLeft"].ToString());
            chkWorseLiftingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseLiftingLeft"].ToString());
            chkWorseRotationLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseRotationLeft"].ToString());
            chkWorseWorkingLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseWorkingLeft"].ToString());
            chkWorseActivitiesLeft.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesLeft"].ToString());
            chkImprovedRestingLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedRestingLeft"].ToString());
            chkImprovedMedicationLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedicationLeft"].ToString());
            chkImprovedTherapyLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapyLeft"].ToString());
            chkImprovedSleepingLeft.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleepingLeft"].ToString());
            txtPainScaleRight.Text = TblRow["PainScaleRight"].ToString().Trim();
            chkSharpRight.Checked = CommonConvert.ToBoolean(TblRow["SharpRight"].ToString());
            chkElectricRight.Checked = CommonConvert.ToBoolean(TblRow["ElectricRight"].ToString());
            chkShootingRight.Checked = CommonConvert.ToBoolean(TblRow["ShootingRight"].ToString());
            chkThrobblingRight.Checked = CommonConvert.ToBoolean(TblRow["ThrobblingRight"].ToString());
            chkPulsatingRight.Checked = CommonConvert.ToBoolean(TblRow["PulsatingRight"].ToString());
            chkDullRight.Checked = CommonConvert.ToBoolean(TblRow["DullRight"].ToString());
            chkAchyRight.Checked = CommonConvert.ToBoolean(TblRow["AchyRight"].ToString());
            chkWorseLyingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseLyingRight"].ToString());
            chkWorseMovementRight.Checked = CommonConvert.ToBoolean(TblRow["WorseMovementRight"].ToString());
            // chkWorseRaisingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseRaisingRight"].ToString());
            chkWorseLiftingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseLiftingRight"].ToString());
            chkWorseRotationRight.Checked = CommonConvert.ToBoolean(TblRow["WorseRotationRight"].ToString());
            chkWorseWorkingRight.Checked = CommonConvert.ToBoolean(TblRow["WorseWorkingRight"].ToString());
            chkWorseActivitiesRight.Checked = CommonConvert.ToBoolean(TblRow["WorseActivitiesRight"].ToString());
            chkImprovedRestingRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedRestingRight"].ToString());
            chkImprovedMedicationRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedMedicationRight"].ToString());
            chkImprovedTherapyRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedTherapyRight"].ToString());
            chkImprovedSleepingRight.Checked = CommonConvert.ToBoolean(TblRow["ImprovedSleepingRight"].ToString());

            txtAbductionLeftWas.Text = TblRow["AbductionLeft"].ToString().Trim();
            txtFlexionLeftWas.Text = TblRow["FlexionLeft"].ToString().Trim();
            txtExtRotationLeftWas.Text = TblRow["ExtRotationLeft"].ToString().Trim();
            txtIntRotationLeftWas.Text = TblRow["IntRotationLeft"].ToString().Trim();

            txtAbductionLeft.Text = TblRow["AbductionRight"].ToString().Trim();
            txtFlexionLeft.Text = TblRow["FlexionRight"].ToString().Trim();
            txtExtRotationLeft.Text = TblRow["ExtRotationRight"].ToString().Trim();
            txtIntRotationLeft.Text = TblRow["IntRotationRight"].ToString().Trim();

            txtAbductionRightWas.Text = TblRow["AbductionNormal"].ToString().Trim();
            txtFlexionRightWas.Text = TblRow["FlexionNormal"].ToString().Trim();
            txtExtRotationRightWas.Text = TblRow["ExtRotationNormal"].ToString().Trim();
            txtIntRotationRightWas.Text = TblRow["IntRotationNormal"].ToString().Trim();

            txtPalpationText1Left.Text = TblRow["PalpationText1Left"].ToString().Trim();
            // txtPalpationText2Left.Text = TblRow["PalpationText2Left"].ToString().Trim();
            chkACJointLeft.Checked = CommonConvert.ToBoolean(TblRow["ACJointLeft"].ToString());
            chkGlenohumeralLeft.Checked = CommonConvert.ToBoolean(TblRow["GlenohumeralLeft"].ToString());
            chkCorticoidLeft.Checked = CommonConvert.ToBoolean(TblRow["CorticoidLeft"].ToString());
            chkSupraspinatusLeft.Checked = CommonConvert.ToBoolean(TblRow["SupraspinatusLeft"].ToString());
            chkScapularLeft.Checked = CommonConvert.ToBoolean(TblRow["ScapularLeft"].ToString());
            chkDeepLabralLeft.Checked = CommonConvert.ToBoolean(TblRow["DeepLabralLeft"].ToString());
            chkDeltoidLeft.Checked = CommonConvert.ToBoolean(TblRow["DeltoidLeft"].ToString());
            chkTrapeziusLeft.Checked = CommonConvert.ToBoolean(TblRow["TrapeziusLeft"].ToString());
            chkEccymosisLeft.Checked = CommonConvert.ToBoolean(TblRow["EccymosisLeft"].ToString());
            chkEdemaLeft.Checked = CommonConvert.ToBoolean(TblRow["EdemaLeft"].ToString());
            chkRangeOfMotionLeft.Checked = CommonConvert.ToBoolean(TblRow["RangeOfMotionLeft"].ToString());
            txtPalpationText1Right.Text = TblRow["PalpationText1Right"].ToString().Trim();
            //txtPalpationText2Right.Text = TblRow["PalpationText2Right"].ToString().Trim();
            chkACJointRight.Checked = CommonConvert.ToBoolean(TblRow["ACJointRight"].ToString());
            chkGlenohumeralRight.Checked = CommonConvert.ToBoolean(TblRow["GlenohumeralRight"].ToString());
            chkCorticoidRight.Checked = CommonConvert.ToBoolean(TblRow["CorticoidRight"].ToString());
            chkSupraspinatusRight.Checked = CommonConvert.ToBoolean(TblRow["SupraspinatusRight"].ToString());
            chkScapularRight.Checked = CommonConvert.ToBoolean(TblRow["ScapularRight"].ToString());
            chkDeepLabralRight.Checked = CommonConvert.ToBoolean(TblRow["DeepLabralRight"].ToString());
            chkDeltoidRight.Checked = CommonConvert.ToBoolean(TblRow["DeltoidRight"].ToString());
            chkTrapeziusRight.Checked = CommonConvert.ToBoolean(TblRow["TrapeziusRight"].ToString());
            chkEccymosisRight.Checked = CommonConvert.ToBoolean(TblRow["EccymosisRight"].ToString());
            chkEdemaRight.Checked = CommonConvert.ToBoolean(TblRow["EdemaRight"].ToString());
            chkRangeOfMotionRight.Checked = CommonConvert.ToBoolean(TblRow["RangeOfMotionRight"].ToString());
            chkNeerLeft.Checked = CommonConvert.ToBoolean(TblRow["NeerLeft"].ToString());
            chkHawkinLeft.Checked = CommonConvert.ToBoolean(TblRow["HawkinLeft"].ToString());
            chkYergasonsLeft.Checked = CommonConvert.ToBoolean(TblRow["YergasonsLeft"].ToString());
            chkDropArmLeft.Checked = CommonConvert.ToBoolean(TblRow["DropArmLeft"].ToString());
            chkReverseBeerLeft.Checked = CommonConvert.ToBoolean(TblRow["ReverseBeerLeft"].ToString());
            chkNeerRight.Checked = CommonConvert.ToBoolean(TblRow["NeerRight"].ToString());
            chkHawkinRight.Checked = CommonConvert.ToBoolean(TblRow["HawkinRight"].ToString());
            chkYergasonsRight.Checked = CommonConvert.ToBoolean(TblRow["YergasonsRight"].ToString());
            chkDropArmRight.Checked = CommonConvert.ToBoolean(TblRow["DropArmRight"].ToString());
            chkReverseBeerRight.Checked = CommonConvert.ToBoolean(TblRow["ReverseBeerRight"].ToString());
            cboTPSide1.Text = TblRow["TPSide1"].ToString().Trim();
            txtTPText1.Text = TblRow["TPText1"].ToString().Trim();
            cboTPSide2.Text = TblRow["TPSide2"].ToString().Trim();
            txtTPText2.Text = TblRow["TPText2"].ToString().Trim();
            cboTPSide3.Text = TblRow["TPSide3"].ToString().Trim();
            txtTPText3.Text = TblRow["TPText3"].ToString().Trim();
            cboTPSide4.Text = TblRow["TPSide4"].ToString().Trim();
            txtTPText4.Text = TblRow["TPText4"].ToString().Trim();
            cboTPSide5.Text = TblRow["TPSide5"].ToString().Trim();
            txtTPText5.Text = TblRow["TPText5"].ToString().Trim();
            cboTPSide6.Text = TblRow["TPSide6"].ToString().Trim();
            txtTPText6.Text = TblRow["TPText6"].ToString().Trim();
            cboTPSide7.Text = TblRow["TPSide7"].ToString().Trim();
            txtTPText7.Text = TblRow["TPText7"].ToString().Trim();
            cboTPSide8.Text = TblRow["TPSide8"].ToString().Trim();
            txtTPText8.Text = TblRow["TPText8"].ToString().Trim();
            txtFreeForm.Text = TblRow["FreeForm"].ToString().Trim();
            txtFreeFormCC.Text = TblRow["FreeFormCC"].ToString().Trim();
            txtFreeFormA.Text = TblRow["FreeFormA"].ToString().Trim();
            txtFreeFormP.Text = TblRow["FreeFormP"].ToString().Trim();
            //cboSprainStrainSide.Text = TblRow["SprainStrainSide"].ToString().Trim();
            //chkSprainStrain.Checked = Convert.ToBoolean(TblRow["SprainStrain"]);
            //cboDerangmentSide.Text = TblRow["DerangmentSide"].ToString().Trim();
            //chkDerangment.Checked = Convert.ToBoolean(TblRow["Derangment"]);
            //cboSyndromeSide.Text = TblRow["SyndromeSide"].ToString().Trim();
            //chkSyndrome.Checked = Convert.ToBoolean(TblRow["Syndrome"]);
            //chkPlan.Checked = Convert.ToBoolean(TblRow["Plan"]);
            //cboScanType.Text = TblRow["ScanType"].ToString().Trim();
            //cboScanSide.Text = TblRow["ScanSide"].ToString().Trim();
            chkElectricLeft.Checked = CommonConvert.ToBoolean(TblRow["ElectricLeft"].ToString());
            _fldPop = false;
        }

        sqlTbl.Dispose();
        sqlCmdBuilder.Dispose();
        sqlAdapt.Dispose();
        oSQLConn.Close();

    }
    public void PopulateUIDefaults()
    {
        XmlDocument xmlDoc = new XmlDocument();
        string filename;
        filename = "~/Template/Default_" + Session["uname"].ToString() + ".xml";
        cboTPSide1.DataBind();
        if (File.Exists(Server.MapPath(filename)))
        { xmlDoc.Load(Server.MapPath(filename)); }
        else { xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml")); }
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Shoulder");
        foreach (XmlNode node in nodeList)
        {
            _fldPop = true;


            bool isTP = node.SelectSingleNode("IsTP") != null ? Convert.ToBoolean(node.SelectSingleNode("IsTP").InnerText) : true;

            if (isTP == false)
                divTP.Attributes.Add("style", "display:none");
            else
                divTP.Attributes.Add("style", "display:block");


            txtPainScaleLeft.Text = node.SelectSingleNode("PainScaleLeft") == null ? txtPainScaleLeft.Text.ToString().Trim() : node.SelectSingleNode("PainScaleLeft").InnerText;
            chkSharpLeft.Checked = node.SelectSingleNode("SharpLeft") == null ? chkSharpLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SharpLeft").InnerText);
            chkSharpLeft.Checked = node.SelectSingleNode("ElectricLeft") == null ? chkSharpLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricLeft").InnerText);
            chkShootingLeft.Checked = node.SelectSingleNode("ShootingLeft") == null ? chkShootingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ShootingLeft").InnerText);
            chkThrobblingLeft.Checked = node.SelectSingleNode("ThrobblingLeft") == null ? chkThrobblingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ThrobblingLeft").InnerText);
            chkPulsatingLeft.Checked = node.SelectSingleNode("PulsatingLeft") == null ? chkPulsatingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("PulsatingLeft").InnerText);
            chkDullLeft.Checked = node.SelectSingleNode("DullLeft") == null ? chkDullLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DullLeft").InnerText);
            chkAchyLeft.Checked = node.SelectSingleNode("AchyLeft") == null ? chkAchyLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("AchyLeft").InnerText);
            chkWorseLyingLeft.Checked = node.SelectSingleNode("WorseLyingLeft") == null ? chkWorseLyingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingLeft").InnerText);
            chkWorseMovementLeft.Checked = node.SelectSingleNode("WorseMovementLeft") == null ? chkWorseMovementLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovementLeft").InnerText);
            //chkWorseRaisingLeft.Checked = node.SelectSingleNode("WorseRaisingLeft") == null ? chkWorseRaisingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRaisingLeft").InnerText);
            chkWorseLiftingLeft.Checked = node.SelectSingleNode("WorseLiftingLeft") == null ? chkWorseLiftingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLiftingLeft").InnerText);
            chkWorseRotationLeft.Checked = node.SelectSingleNode("WorseRotationLeft") == null ? chkWorseRotationLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRotationLeft").InnerText);
            chkWorseWorkingLeft.Checked = node.SelectSingleNode("WorseWorkingLeft") == null ? chkWorseWorkingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorkingLeft").InnerText);
            chkWorseActivitiesLeft.Checked = node.SelectSingleNode("WorseActivitiesLeft") == null ? chkWorseActivitiesLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseActivitiesLeft").InnerText);
            chkImprovedRestingLeft.Checked = node.SelectSingleNode("ImprovedRestingLeft") == null ? chkImprovedRestingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedRestingLeft").InnerText);
            chkImprovedMedicationLeft.Checked = node.SelectSingleNode("ImprovedMedicationLeft") == null ? chkImprovedMedicationLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedicationLeft").InnerText);
            chkImprovedTherapyLeft.Checked = node.SelectSingleNode("ImprovedTherapyLeft") == null ? chkImprovedTherapyLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapyLeft").InnerText);
            chkImprovedSleepingLeft.Checked = node.SelectSingleNode("ImprovedSleepingLeft") == null ? chkImprovedSleepingLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleepingLeft").InnerText);
            txtPainScaleRight.Text = node.SelectSingleNode("PainScaleRight") == null ? txtPainScaleRight.Text.ToString().Trim() : node.SelectSingleNode("PainScaleRight").InnerText;
            chkSharpRight.Checked = node.SelectSingleNode("SharpRight") == null ? chkSharpRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SharpRight").InnerText);
            chkElectricRight.Checked = node.SelectSingleNode("ElectricRight") == null ? chkElectricRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricRight").InnerText);
            chkShootingRight.Checked = node.SelectSingleNode("ShootingRight") == null ? chkShootingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ShootingRight").InnerText);
            chkThrobblingRight.Checked = node.SelectSingleNode("ThrobblingRight") == null ? chkThrobblingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ThrobblingRight").InnerText);
            chkPulsatingRight.Checked = node.SelectSingleNode("PulsatingRight") == null ? chkPulsatingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("PulsatingRight").InnerText);
            chkDullRight.Checked = node.SelectSingleNode("DullRight") == null ? chkDullRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DullRight").InnerText);
            chkAchyRight.Checked = node.SelectSingleNode("AchyRight") == null ? chkAchyRight.Checked : Convert.ToBoolean(node.SelectSingleNode("AchyRight").InnerText);
            chkWorseLyingRight.Checked = node.SelectSingleNode("WorseLyingRight") == null ? chkWorseLyingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLyingRight").InnerText);
            chkWorseMovementRight.Checked = node.SelectSingleNode("WorseMovementRight") == null ? chkWorseMovementRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseMovementRight").InnerText);
            //chkWorseRaisingRight.Checked = node.SelectSingleNode("WorseRaisingRight") == null ? chkWorseRaisingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRaisingRight").InnerText);
            chkWorseLiftingRight.Checked = node.SelectSingleNode("WorseLiftingRight") == null ? chkWorseLiftingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseLiftingRight").InnerText);
            chkWorseRotationRight.Checked = node.SelectSingleNode("WorseRotationRight") == null ? chkWorseRotationRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseRotationRight").InnerText);
            chkWorseWorkingRight.Checked = node.SelectSingleNode("WorseWorkingRight") == null ? chkWorseWorkingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseWorkingRight").InnerText);
            chkWorseActivitiesRight.Checked = node.SelectSingleNode("WorseActivitiesRight") == null ? chkWorseActivitiesRight.Checked : Convert.ToBoolean(node.SelectSingleNode("WorseActivitiesRight").InnerText);
            chkImprovedRestingRight.Checked = node.SelectSingleNode("ImprovedRestingRight") == null ? chkImprovedRestingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedRestingRight").InnerText);
            chkImprovedMedicationRight.Checked = node.SelectSingleNode("ImprovedMedicationRight") == null ? chkImprovedMedicationRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedMedicationRight").InnerText);
            chkImprovedTherapyRight.Checked = node.SelectSingleNode("ImprovedTherapyRight") == null ? chkImprovedTherapyRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedTherapyRight").InnerText);
            chkImprovedSleepingRight.Checked = node.SelectSingleNode("ImprovedSleepingRight") == null ? chkImprovedSleepingRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ImprovedSleepingRight").InnerText);

            txtAbductionRightWas.Text = node.SelectSingleNode("ShoulderAbdNormal") == null ? txtAbductionRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderAbdNormal").InnerText;
            txtFlexionRightWas.Text = node.SelectSingleNode("ShoulderFlexNormal") == null ? txtFlexionRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderFlexNormal").InnerText;
            txtExtRotationRightWas.Text = node.SelectSingleNode("ShoulderExtRotNormal") == null ? txtExtRotationRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderExtRotNormal").InnerText;
            txtIntRotationRightWas.Text = node.SelectSingleNode("ShoulderIntRot") == null ? txtIntRotationRightWas.Text.ToString().Trim() : node.SelectSingleNode("ShoulderIntRot").InnerText;


            txtAbductionLeftWas.Text = node.SelectSingleNode("AbductionRight") == null ? txtAbductionLeftWas.Text.ToString().Trim() : node.SelectSingleNode("AbductionRight").InnerText;
            txtFlexionLeftWas.Text = node.SelectSingleNode("FlexionRight") == null ? txtFlexionLeftWas.Text.ToString().Trim() : node.SelectSingleNode("FlexionRight").InnerText;
            txtExtRotationLeftWas.Text = node.SelectSingleNode("ExtRotationRight") == null ? txtExtRotationLeftWas.Text.ToString().Trim() : node.SelectSingleNode("ExtRotationRight").InnerText;
            txtIntRotationLeftWas.Text = node.SelectSingleNode("IntRotationRight") == null ? txtIntRotationLeftWas.Text.ToString().Trim() : node.SelectSingleNode("IntRotationRight").InnerText;

            txtAbductionLeft.Text = node.SelectSingleNode("AbductionLeft") == null ? txtAbductionLeft.Text.ToString().Trim() : node.SelectSingleNode("AbductionLeft").InnerText;
            txtFlexionLeft.Text = node.SelectSingleNode("FlexionLeft") == null ? txtFlexionLeft.Text.ToString().Trim() : node.SelectSingleNode("FlexionLeft").InnerText;
            txtExtRotationLeft.Text = node.SelectSingleNode("ExtRotationLeft") == null ? txtExtRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("ExtRotationLeft").InnerText;
            txtIntRotationLeft.Text = node.SelectSingleNode("IntRotationLeft") == null ? txtIntRotationLeft.Text.ToString().Trim() : node.SelectSingleNode("IntRotationLeft").InnerText;

            txtPalpationText1Left.Text = node.SelectSingleNode("PalpationText1Left") == null ? txtPalpationText1Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Left").InnerText;
            // txtPalpationText2Left.Text = node.SelectSingleNode("PalpationText2Left") == null ? txtPalpationText2Left.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Left").InnerText;

            chkACJointLeft.Checked = node.SelectSingleNode("ACJointLeft") == null ? chkACJointLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ACJointLeft").InnerText);
            chkGlenohumeralLeft.Checked = node.SelectSingleNode("GlenohumeralLeft") == null ? chkGlenohumeralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("GlenohumeralLeft").InnerText);
            chkCorticoidLeft.Checked = node.SelectSingleNode("CorticoidLeft") == null ? chkCorticoidLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("CorticoidLeft").InnerText);
            chkSupraspinatusLeft.Checked = node.SelectSingleNode("SupraspinatusLeft") == null ? chkSupraspinatusLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("SupraspinatusLeft").InnerText);
            chkScapularLeft.Checked = node.SelectSingleNode("ScapularLeft") == null ? chkScapularLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapularLeft").InnerText);
            chkDeepLabralLeft.Checked = node.SelectSingleNode("DeepLabralLeft") == null ? chkDeepLabralLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DeepLabralLeft").InnerText);
            chkDeltoidLeft.Checked = node.SelectSingleNode("DeltoidLeft") == null ? chkDeltoidLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DeltoidLeft").InnerText);
            chkTrapeziusLeft.Checked = node.SelectSingleNode("TrapeziusLeft") == null ? chkTrapeziusLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("TrapeziusLeft").InnerText);
            chkEccymosisLeft.Checked = node.SelectSingleNode("EccymosisLeft") == null ? chkEccymosisLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("EccymosisLeft").InnerText);
            chkEdemaLeft.Checked = node.SelectSingleNode("EdemaLeft") == null ? chkEdemaLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("EdemaLeft").InnerText);
            chkRangeOfMotionLeft.Checked = node.SelectSingleNode("RangeOfMotionLeft") == null ? chkRangeOfMotionLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("RangeOfMotionLeft").InnerText);
            txtPalpationText1Right.Text = node.SelectSingleNode("PalpationText1Right") == null ? txtPalpationText1Right.Text.ToString().Trim() : node.SelectSingleNode("PalpationText1Right").InnerText;
            // txtPalpationText2Right.Text = node.SelectSingleNode("PalpationText2Right") == null ? txtPalpationText2Right.Text.ToString().Trim() : node.SelectSingleNode("PalpationText2Right").InnerText;
            chkACJointRight.Checked = node.SelectSingleNode("ACJointRight") == null ? chkACJointRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ACJointRight").InnerText);
            chkGlenohumeralRight.Checked = node.SelectSingleNode("GlenohumeralRight") == null ? chkGlenohumeralRight.Checked : Convert.ToBoolean(node.SelectSingleNode("GlenohumeralRight").InnerText);
            chkCorticoidRight.Checked = node.SelectSingleNode("CorticoidRight") == null ? chkCorticoidRight.Checked : Convert.ToBoolean(node.SelectSingleNode("CorticoidRight").InnerText);
            chkSupraspinatusRight.Checked = node.SelectSingleNode("SupraspinatusRight") == null ? chkSupraspinatusRight.Checked : Convert.ToBoolean(node.SelectSingleNode("SupraspinatusRight").InnerText);
            chkScapularRight.Checked = node.SelectSingleNode("ScapularRight") == null ? chkScapularRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ScapularRight").InnerText);
            chkDeepLabralRight.Checked = node.SelectSingleNode("DeepLabralRight") == null ? chkDeepLabralRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DeepLabralRight").InnerText);
            chkDeltoidRight.Checked = node.SelectSingleNode("DeltoidRight") == null ? chkDeltoidRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DeltoidRight").InnerText);
            chkTrapeziusRight.Checked = node.SelectSingleNode("TrapeziusRight") == null ? chkTrapeziusRight.Checked : Convert.ToBoolean(node.SelectSingleNode("TrapeziusRight").InnerText);
            chkEccymosisRight.Checked = node.SelectSingleNode("EccymosisRight") == null ? chkEccymosisRight.Checked : Convert.ToBoolean(node.SelectSingleNode("EccymosisRight").InnerText);
            chkEdemaRight.Checked = node.SelectSingleNode("EdemaRight") == null ? chkEdemaRight.Checked : Convert.ToBoolean(node.SelectSingleNode("EdemaRight").InnerText);
            chkRangeOfMotionRight.Checked = node.SelectSingleNode("RangeOfMotionRight") == null ? chkRangeOfMotionRight.Checked : Convert.ToBoolean(node.SelectSingleNode("RangeOfMotionRight").InnerText);
            chkNeerLeft.Checked = node.SelectSingleNode("NeerLeft") == null ? chkNeerLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("NeerLeft").InnerText);
            chkHawkinLeft.Checked = node.SelectSingleNode("HawkinLeft") == null ? chkHawkinLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("HawkinLeft").InnerText);
            chkYergasonsLeft.Checked = node.SelectSingleNode("YergasonsLeft") == null ? chkYergasonsLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("YergasonsLeft").InnerText);
            chkDropArmLeft.Checked = node.SelectSingleNode("DropArmLeft") == null ? chkDropArmLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("DropArmLeft").InnerText);
            chkReverseBeerLeft.Checked = node.SelectSingleNode("ReverseBeerLeft") == null ? chkReverseBeerLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ReverseBeerLeft").InnerText);
            chkNeerRight.Checked = node.SelectSingleNode("NeerRight") == null ? chkNeerRight.Checked : Convert.ToBoolean(node.SelectSingleNode("NeerRight").InnerText);
            chkHawkinRight.Checked = node.SelectSingleNode("HawkinRight") == null ? chkHawkinRight.Checked : Convert.ToBoolean(node.SelectSingleNode("HawkinRight").InnerText);
            chkYergasonsRight.Checked = node.SelectSingleNode("YergasonsRight") == null ? chkYergasonsRight.Checked : Convert.ToBoolean(node.SelectSingleNode("YergasonsRight").InnerText);
            chkDropArmRight.Checked = node.SelectSingleNode("DropArmRight") == null ? chkDropArmRight.Checked : Convert.ToBoolean(node.SelectSingleNode("DropArmRight").InnerText);
            chkReverseBeerRight.Checked = node.SelectSingleNode("ReverseBeerRight") == null ? chkReverseBeerRight.Checked : Convert.ToBoolean(node.SelectSingleNode("ReverseBeerRight").InnerText);

            cboTPSide1.Text = node.SelectSingleNode(pos + "/TPSide1") == null ? cboTPSide1.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide1").InnerText;
            txtTPText1.Text = node.SelectSingleNode(pos + "/TPText1") == null ? txtTPText1.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText1").InnerText;
            cboTPSide2.Text = node.SelectSingleNode(pos + "/TPSide2") == null ? cboTPSide2.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide2").InnerText;
            txtTPText2.Text = node.SelectSingleNode(pos + "/TPText2") == null ? txtTPText2.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText2").InnerText;
            cboTPSide3.Text = node.SelectSingleNode(pos + "/TPSide3") == null ? cboTPSide3.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide3").InnerText;
            txtTPText3.Text = node.SelectSingleNode(pos + "/TPText3") == null ? txtTPText3.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText3").InnerText;
            cboTPSide4.Text = node.SelectSingleNode(pos + "/TPSide4") == null ? cboTPSide4.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide4").InnerText;
            txtTPText4.Text = node.SelectSingleNode(pos + "/TPText4") == null ? txtTPText4.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText4").InnerText;
            cboTPSide5.Text = node.SelectSingleNode(pos + "/TPSide5") == null ? cboTPSide5.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide5").InnerText;
            txtTPText5.Text = node.SelectSingleNode(pos + "/TPText5") == null ? txtTPText5.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText5").InnerText;
            cboTPSide6.Text = node.SelectSingleNode(pos + "/TPSide6") == null ? cboTPSide6.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide6").InnerText;
            txtTPText6.Text = node.SelectSingleNode(pos + "/TPText6") == null ? txtTPText6.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText6").InnerText;
            cboTPSide7.Text = node.SelectSingleNode(pos + "/TPSide7") == null ? cboTPSide7.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide7").InnerText;
            txtTPText7.Text = node.SelectSingleNode(pos + "/TPText7") == null ? txtTPText7.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPText7").InnerText;
            cboTPSide8.Text = node.SelectSingleNode(pos + "/TPSide8") == null ? cboTPSide8.Text.ToString().Trim() : node.SelectSingleNode(pos + "/TPSide8").InnerText;

            txtTPText8.Text = node.SelectSingleNode("TPText8") == null ? txtTPText8.Text.ToString().Trim() : node.SelectSingleNode("TPText8").InnerText;
            txtFreeForm.Text = node.SelectSingleNode("FreeForm") == null ? txtFreeForm.Text.ToString().Trim() : node.SelectSingleNode("FreeForm").InnerText;
            txtFreeFormCC.Text = node.SelectSingleNode("FreeFormCC") == null ? txtFreeFormCC.Text.ToString().Trim() : node.SelectSingleNode("FreeFormCC").InnerText;
            txtFreeFormA.Text = node.SelectSingleNode("FreeFormA") == null ? txtFreeFormA.Text.ToString().Trim() : node.SelectSingleNode("FreeFormA").InnerText;
            txtFreeFormP.Text = node.SelectSingleNode("FreeFormP") == null ? txtFreeFormP.Text.ToString().Trim() : node.SelectSingleNode("FreeFormP").InnerText;
            //cboSprainStrainSide.Text = node.SelectSingleNode("SprainStrainSide") == null ? cboSprainStrainSide.Text.ToString().Trim() : node.SelectSingleNode("SprainStrainSide").InnerText;
            //chkSprainStrain.Checked = node.SelectSingleNode("SprainStrain") == null ? chkSprainStrain.Checked : Convert.ToBoolean(node.SelectSingleNode("SprainStrain").InnerText);
            //cboDerangmentSide.Text = node.SelectSingleNode("DerangmentSide") == null ? cboDerangmentSide.Text.ToString().Trim() : node.SelectSingleNode("DerangmentSide").InnerText;
            //chkDerangment.Checked = node.SelectSingleNode("Derangment") == null ? chkDerangment.Checked : Convert.ToBoolean(node.SelectSingleNode("Derangment").InnerText);
            //cboSyndromeSide.Text = node.SelectSingleNode("SyndromeSide") == null ? cboSyndromeSide.Text.ToString().Trim() : node.SelectSingleNode("SyndromeSide").InnerText;
            //chkSyndrome.Checked = node.SelectSingleNode("Syndrome") == null ? chkSyndrome.Checked : Convert.ToBoolean(node.SelectSingleNode("Syndrome").InnerText);
            //chkPlan.Checked = node.SelectSingleNode("Plan") == null ? chkPlan.Checked : Convert.ToBoolean(node.SelectSingleNode("Plan").InnerText);
            chkElectricLeft.Checked = node.SelectSingleNode("ElectricLeft") == null ? chkElectricLeft.Checked : Convert.ToBoolean(node.SelectSingleNode("ElectricLeft").InnerText);
            //cboScanType.Text = node.SelectSingleNode("ScanType") == null ? cboScanType.Text.ToString().Trim() : node.SelectSingleNode("ScanType").InnerText;
            //cboScanSide.Text = node.SelectSingleNode("ScanSide") == null ? cboScanSide.Text.ToString().Trim() : node.SelectSingleNode("ScanSide").InnerText;
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
            //SqlStr = "Select * from tblProceduresDetail WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' Order By BodyPart,Heading";
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
                         from tblProceduresDetail p WHERE PatientIE_ID = " + _CurIEid + " AND BodyPart = '" + _CurBP + "' and IsConsidered=0 Order By BodyPart,Heading";
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

                ids += Session["PatientIE_ID"].ToString() + ",";
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
        // long _StdID = Convert.ToInt64(iStdID);
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

    protected void AddDiag_Click(object sender, EventArgs e)//RoutedEventArgs 
    {
        string ieMode = "New";
        bindgridPoup();
        //SaveUI(Convert.ToString(Session["PatientIE_ID"]), Convert.ToString(Session["patientFUId"]), ieMode, true);
        //SaveStandards(Session["PatientIE_ID"].ToString());
        // Response.Redirect("AddDiagnosis.aspx");
    }

    private void AddStd_Click(object sender, EventArgs e) //RoutedEventArgs e
    {

        BindDataGrid();

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
            TblRow["PatientFU_ID"] = Session["patientFUId"].ToString();
            TblRow["BodyPart"] = bp.ToString().Trim();
            TblRow["DiagCode"] = dc.ToString().Trim();
            TblRow["Description"] = dcd.ToString().Trim();

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
        string ieMode = "Update";

        _CurIEid = Session["PatientIE_ID"].ToString();
        _FuId = Session["patientFUId"].ToString();
        SaveDiagnosis(_CurIEid);
        SaveUI(_CurIEid, _FuId, ieMode, true);
        SaveStandards(Session["PatientIE_ID"].ToString());
        PopulateUI(Session["patientFUId"].ToString());

        if (pageHDN.Value != null && pageHDN.Value != "")
        {
            Response.Redirect(pageHDN.Value.ToString());
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
        XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Shoulder");
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

    //    try
    //    {
    //        _FuId = Session["patientFUId"].ToString();
    //        string sProvider = ConfigurationManager.ConnectionStrings["connString_WFP"].ConnectionString;
    //        string SqlStr = "";
    //        oSQLConn.ConnectionString = sProvider;

    //        if (oSQLConn.State == ConnectionState.Closed)
    //            oSQLConn.Open();
    //        SqlStr = "Select * from tblFUbpShoulder WHERE PatientFU_ID = " + _FuId;
    //        SqlDataAdapter sqlAdapt = new SqlDataAdapter(SqlStr, oSQLConn);
    //        SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(sqlAdapt);
    //        DataTable sqlTbl = new DataTable();
    //        sqlAdapt.Fill(sqlTbl);
    //        oSQLConn.Close();
    //        if (sqlTbl.Rows.Count > 0)
    //        {
    //            string[] strname, strleft, strright, strnormal;
    //            if (string.IsNullOrEmpty(sqlTbl.Rows[0]["NameROM"].ToString()) == false)
    //            {
    //                strname = sqlTbl.Rows[0]["NameROM"].ToString().Split(',');
    //                strleft = sqlTbl.Rows[0]["LeftROM"].ToString().Split(',');
    //                strright = sqlTbl.Rows[0]["RightROM"].ToString().Split(',');
    //                strnormal = sqlTbl.Rows[0]["NormalROM"].ToString().Split(',');


    //                // Create the Table
    //                DataTable OrdersTable = new DataTable("ROM");
    //                // Build the Orders schema
    //                OrdersTable.Columns.Add("name", Type.GetType("System.String"));
    //                OrdersTable.Columns.Add("left", Type.GetType("System.String"));
    //                OrdersTable.Columns.Add("right", Type.GetType("System.String"));
    //                OrdersTable.Columns.Add("normal", Type.GetType("System.String"));

    //                DataRow workRow;

    //                for (int i = 0; i < strname.Length; i++)
    //                {

    //                    workRow = OrdersTable.NewRow();
    //                    workRow[0] = strname[i];
    //                    workRow[1] = strleft[i];
    //                    workRow[2] = strright[i];
    //                    workRow[3] = strnormal[i];
    //                    OrdersTable.Rows.Add(workRow);
    //                }

    //                if (OrdersTable.Rows.Count != 0)
    //                {
    //                    repROM.DataSource = OrdersTable;
    //                    repROM.DataBind();
    //                }
    //            }
    //            else
    //                getXMLROMvalue();
    //        }
    //        else
    //        {
    //            getXMLROMvalue();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //private void getXMLROMvalue()
    //{
    //    //open the tender xml file  
    //    XmlTextReader xmlreader = new XmlTextReader(Server.MapPath("~/XML/Shoulder.xml"));
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

    //protected void repROM_ItemDataBound(object sender, RepeaterItemEventArgs e)
    //{
    //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
    //    {
    //        if (Request["P"] != null)
    //        {
    //            if (Request["P"] == "R")
    //            {
    //                TextBox txtleft = e.Item.FindControl("txtleft") as TextBox;
    //                txtleft.ReadOnly = true;
    //            }
    //            else if (Request["P"] == "L")
    //            {
    //                TextBox txtright = e.Item.FindControl("txtright") as TextBox;
    //                txtright.ReadOnly = true;
    //            }
    //        }
    //    }
    //}
}