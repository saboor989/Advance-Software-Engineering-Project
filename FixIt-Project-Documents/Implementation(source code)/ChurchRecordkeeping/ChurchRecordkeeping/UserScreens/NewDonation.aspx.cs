    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using ChurchRecordkeeping.Business;
    using System.Data;
    using Telerik.Web.UI;
    using System.Data.SqlClient;

    namespace ChurchRecordkeeping.UserScreens
    {
    public partial class NewDonation : System.Web.UI.Page
    {
        #region variable declaration
        Donation objd = new Donation();
        Validations val = new Validations();
        string envelopID = string.Empty;
        string fundname = string.Empty;
        string DID = string.Empty;
        #endregion

        #region PageLoad
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                getfundnamedetails();
                GetEnvelopenumberdetails();
                getdetail();
            }

          

        }
        #endregion

        #region getdetail
        //getdetail this method is used to get the DID value from querystring and if tha tDID is not an empty then it will change
        //text of save to update  calling geteditDetails method by passing DID to get edit the details  
        public void getdetail()
        {

            if (Request.QueryString["DID"] != null)
            {
                DID = (Request.QueryString["DID"].ToString());
            }


            if (DID != string.Empty)
            {
                Savebutton.Text = "Update";
                createdonation.Text = "Update Donation";
                getEditDetails(DID);
            }
        }
        #endregion

        #region getEditDetails
        // getEditDetails method is displays info of donation details to using DID number which comes from
        //Querystring it will displyed in respective fields using data from database based on DID
        protected void getEditDetails(string DID)
        {
            objd.DonationID = DID;
          
            using (SqlDataReader dr = objd.EDITDonationDETAILS())
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                       
                            GetEnvelopenumberdetails();
                        EnveRadComboBox.SelectedItem.Text = dr["Envelopenumber"].ToString().Trim();
                        getfundnamedetails();
                        Fundnameradcombo.SelectedItem.Text = dr["FundName"].ToString().Trim();
                        moneytypecombo.SelectedIndex = 0;
                        moneytypecombo.SelectedItem.Text = dr["Moneytype"].ToString().Trim();
                        Amounttxtbox.Text = dr["Amount"].ToString();
                        NoteRadTextBox.Text = dr["Note"].ToString();
                        RadDatePicker.SelectedDate =Convert.ToDateTime( dr["Date"].ToString());
                       
                    }
                }
            }

        }

        #endregion

        #region ValidateNumber
        //ValidateNumber method is used to validate amount number by converting the data type into Double
        public static bool ValidateNumber(string number)
        {
            bool validatenum = true;
            try
            {
                double _num = Convert.ToDouble(number.Trim());
                if (_num > 0)
                {
                    return true;
                }
               
               
            }
            catch
            {
                return false;
            }
            return false;
        }
        #endregion

        #region validation
        //validatedonationcontrol method is used to validate the below controls and 
        // if those are empty then set it to ValidateFlag as false else make it true.
        public bool validatedonationcontrol()
        {
            bool ValidateFlag = true;

            if (Amounttxtbox.Text == string.Empty)
                ValidateFlag = false;

            if (RadDatePicker.SelectedDate == null)
                ValidateFlag = false;

            if (EnveRadComboBox.SelectedItem.Text == "--- Select ---")
                ValidateFlag = false;

            if (Fundnameradcombo.SelectedItem.Text == "--- Select ---")
                ValidateFlag = false;

            if (moneytypecombo.SelectedItem.Text == "---Select Money Type---")
                ValidateFlag = false;

            return ValidateFlag;
        }
        #endregion

        #region EditDonationDetails
        //EditDonationDetails method is to update the details of donation based on DID 
        //can be updated by UpdateDonationDetails and store it to database 
        protected void EditDonationDetails(string Did)
        {
            int noOfRowsaffected = 0;
            objd.DonationID = Did;
            objd.Amount = Amounttxtbox.Text.Trim();
            objd.Note = NoteRadTextBox.Text.Trim();
            objd.Moneytype = moneytypecombo.SelectedItem.Text.Trim();
            objd.Date = RadDatePicker.SelectedDate.Value;
            objd.FundName = Fundnameradcombo.SelectedItem.Text.Trim();
            objd.Envelopenumber = EnveRadComboBox.SelectedItem.Text.Trim();
           
            try
            {
                noOfRowsaffected = objd.UpdateDonationDetails();
                Validations.showMessage(lblErrorMsg, Validations.Msg_updatedonation, "Success");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region GetEnvelopenumberdetails
        //This GetEnvelopenumberdetails method is used to bind details using GetEnvelopenumberdetails of Envelopenumber and Firstname
        //to EnveRadComboBox  in order to select the Envelopenumber by user.
        public void GetEnvelopenumberdetails()
        {

            using (DataTable dt = objd.GetEnvelopenumberdetails())
            {
                EnveRadComboBox.DataSource = dt;
                EnveRadComboBox.DataTextField = "Envelopenumber";
                EnveRadComboBox.DataValueField = "Firstname";
                EnveRadComboBox.DataBind();
                EnveRadComboBox.Items.Insert(0, new RadComboBoxItem("--- Select ---", "0"));

            }
        }
        #endregion

        #region getfundnamedetails
        //This getfundnamedetails method is used to bind details of FundName and Fundnumber
        //to Fundnameradcombo  in order to select the fundname by user.
        public void getfundnamedetails()
        {

            using (DataTable dt = objd.getfundnamedetails())
            {
                Fundnameradcombo.DataSource = dt;
                Fundnameradcombo.DataTextField = "FundName";
                Fundnameradcombo.DataValueField = "Fundnumber";
                Fundnameradcombo.DataBind();
                Fundnameradcombo.Items.Insert(0, new RadComboBoxItem("--- Select ---", "0"));

            }
        }
        #endregion

        #region InsertMemberDetails
        //using InsertMemberDetails method we can insert new donation into database 
        //by paasing all the value through business class to dataclass if it is 
        //inserted then it gives message as successfully inserted
        protected void InsertMemberDetails()
        {
            
                int noOfRowsaffected = 0;
                objd.Amount = Amounttxtbox.Text.Trim();
                if (RadDatePicker.SelectedDate == null)
                {
                    RadDatePicker.SelectedDate= DateTime.Today;
                    objd.Date = RadDatePicker.SelectedDate.Value;
                }
                else
                {
                    objd.Date = RadDatePicker.SelectedDate.Value;
                }
                objd.Envelopenumber = EnveRadComboBox.SelectedItem.Text.Trim();
                objd.FundName = Fundnameradcombo.SelectedItem.Text.Trim();
                objd.Note = NoteRadTextBox.Text.Trim();
                objd.Moneytype = moneytypecombo.SelectedItem.Text.Trim();
                string IsAvailabel = string.Empty;
                    try
                    {
                        noOfRowsaffected = objd.InsertintoDonation();
                        Validations.showMessage(lblErrorMsg, Validations.Msg_Adddonation, "Success");
                    }
                    catch (Exception ex)
                    {
                        Validations.showMessage(lblErrorMsg, "Is not inserted", "Error");
                    }
                   
            }
        #endregion

        #region Events 
        #region Save_onclick
        // Save_onclick in this event we are checking validatedonationcontrol for all fields 
        //from add new  donation page if validation are true then we insert new donation into database using 
        //InsertMemberDetails if we have DID number which we get it from querystring if that is not empty so on 
        //the basis of that we update that donation details basis on DID value
        protected void Save_onclick(object sender, EventArgs e)
        {
            lblErrorMsg.Text = string.Empty;
            //validatedonationcontrol is false then it gives error 
            if (validatedonationcontrol() == false)
            {
                Validations.showMessage(lblErrorMsg, Validations.Msg_mandatory, "Error");
                return;
            }
            else if (validatedonationcontrol() == true)
            {
                bool isValidNumeric = ValidateNumber(Amounttxtbox.Text);
                if (isValidNumeric == false)
                {
                    // lblErrorMsg.Text = "Please enter valid numbers.";
                    Validations.showMessage(lblErrorMsg, Validations.Msg_entervalidamount, "Error");

                    return;
                }

                else if (Request.QueryString["DID"] != null)
                {
                    DID = (Request.QueryString["DID"].ToString());
                    EditDonationDetails(DID);
                }

                else
                {
                    InsertMemberDetails();
                }



            }


        }
        #endregion

        #region clearbutton_click
        //clearbutton_click this event clears all fields of donation page null or clear including texboxes and comboboxes
        protected void clearbutton_click(object sender, EventArgs e)
        {
            Amounttxtbox.Text = string.Empty;
            RadDatePicker.SelectedDate = null;
            GetEnvelopenumberdetails();
            getfundnamedetails();
            moneytypecombo.Items.Clear();
          
            moneytypecombo.Items.Insert(0,
        new Telerik.Web.UI.RadComboBoxItem { Text = "Cash", Value = "1" });
            moneytypecombo.Items.Insert(0,
        new Telerik.Web.UI.RadComboBoxItem { Text = "Check", Value = "2" });


            lblErrorMsg.Text = "";

        }
        #endregion
        #endregion

    }   
    }
