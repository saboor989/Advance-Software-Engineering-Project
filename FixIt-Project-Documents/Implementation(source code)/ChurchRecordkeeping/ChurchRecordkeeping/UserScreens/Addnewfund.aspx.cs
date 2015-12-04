using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ChurchRecordkeeping.Business;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace ChurchRecordkeeping.UserScreens
{
    public partial class Addnewfund : System.Web.UI.Page
    {
        #region Variable Declaration 
        Fund Objfund = new Fund();
        Validations val = new Validations();
        string FundName = string.Empty;
        int fundID = 0;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                getdetail();
            }

        }
        #endregion

        #region getdetail

        // getdetail method through which we can get the FundID (FID) form query string 
        //based on the FID if it is not empty then we can getEditDetails to update the fund details
        public void getdetail()
        {

            if (Request.QueryString["FID"] != null)
                fundID = Convert.ToInt32((Request.QueryString["FID"].ToString()));
            if (fundID != 0)
            {
                Save1.Text = "Update";
                createlabel.Text = "Update Fund";
                getEditDetails(fundID);
            }
        }
        #endregion

        #region EVENTS
        #region Save_onclick
        // Save_onclick in this event we are checking validatedonationcontrol for all fields 
        //from add new  fund page if validation are true then we insert new fund into database using 
        //InsertFundDetails if we have DID number which we get it from querystring if that is not empty so on 
        //the basis of that we update that fund details basis on FID value 
        protected void Save_onclick(object sender, EventArgs e)
        {
            lblErrorMsg.Text = string.Empty;

            if (validatefundcontrol() == false)
            {
                Validations.showMessage(lblErrorMsg, Validations.Msg_mandatory, "Error");
                return;
            }
            else
            {

                string lbl = fundnametxtbox.Text;
               
                Regex r = new Regex("^[a-zA-Z ]+$");

                if (r.IsMatch(fundnametxtbox.Text))
                {

                    lblErrorMsg.Text = "";

                    if (Request.QueryString["FID"] != null)
                        fundID = Convert.ToInt32((Request.QueryString["FID"].ToString()));

                    if (fundID == 0)
                    {
                        InsertFundDetails();
                    }
                    else
                    {
                        Save1.Text = "Update";
                        createlabel.Text = "Update Fund";
                        UpdateFundDetails(fundID);
                    }




                }
                else
                {

                    lblErrorMsg.Text = ("Enter the Fund for ex.General Fund");
                    fundnametxtbox.Text = "";
                    
                }
            }
            
           

           
        }
        #endregion

        #region Cancel_onclick

        //Cancel_onclick event can clear Textbox details 
        protected void Cancel_onclick(object sender, EventArgs e)
        {
            lblErrorMsg.Text = "";
            fundnametxtbox.Text = "";
        }
        #endregion

        #endregion


        #region InsertFundDetails
        //InsertFundDetails method we can insert new donation into database 
        //by paasing all the value through business class to dataclass if it is 
        //inserted then it gives message as successfully inserted
        protected void InsertFundDetails()
        {

            int noOfRowsaffected = 0;
            Objfund.Fundname = fundnametxtbox.Text.Trim();
            string IsAvailabel = string.Empty;
            IsAvailabel = Objfund.chkAvailableFundName();

            if (IsAvailabel == "0")
            {
                try
                {
                    noOfRowsaffected = Objfund.InsertintoFund();
                    Validations.showMessage(lblErrorMsg, Validations.Msg_AddFund, "Success");
                }
                catch (Exception ex)
                {

                }

            }
            else
            {
                Validations.showMessage(lblErrorMsg, "Fund Name" + Validations.Err_Duplicate, "Error");
            }
        }

        #endregion


        #region UpdateFundDetails
        //UpdateFundDetails method is to update the details of Fund based on fundID 
        //can be updated by UpdateFundDetails and store it to database 
        protected void UpdateFundDetails(int fundID)
        {
            int noOfRowsaffected = 0;
            Objfund.Fundnumber = fundID;
            Objfund.Fundname = fundnametxtbox.Text.Trim();
          
            try
            {
                noOfRowsaffected = Objfund.UpdateFundDetails();
                Validations.showMessage(lblErrorMsg, Validations.Msg_updatedFund, "Success");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region validation
        //validatefundcontrol method is used to validate the below controls and 
        // if those are empty then set it to ValidateFlag as false else make it true.
        public bool validatefundcontrol()
        {
            bool ValidateFlag = true;

            if (fundnametxtbox.Text == string.Empty)
                ValidateFlag = false;

           
            return ValidateFlag;
        }
        #endregion

        #region getEditDetails
        // getEDITFUNDDETAILS method is displays info of fund details to using fid number which comes from
        //Querystring it will displyed in respective fields using data from database based on fid
        protected void getEditDetails(int fundID)
        {
            Objfund.Fundnumber = fundID;
            using (SqlDataReader dr = Objfund.getEDITFUNDDETAILS())
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        fundnametxtbox.Text = dr["FundName"].ToString();
                       
                    }
                }
            }

        }

        #endregion
    }
}