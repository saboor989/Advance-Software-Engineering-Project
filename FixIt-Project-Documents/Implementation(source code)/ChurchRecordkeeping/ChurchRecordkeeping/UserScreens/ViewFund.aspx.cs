using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Telerik.Web.UI;
using ChurchRecordkeeping.Business;

namespace ChurchRecordkeeping.UserScreens
{
    public partial class ViewFund : System.Web.UI.Page
    {
        Fund objfund = new Fund();
        int no_of_rows_affected = 0;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindGrid();
            }

        }

        #region SearchButton_Click
        //SearchButton_Click event is used to check each every column of grid including char column and number column
        protected void SearchButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblErrorMsg.Text = "";

                GridFilteringItem item = gvfund.MasterTableView.GetItems(GridItemType.FilteringItem)[0] as GridFilteringItem;

                string Fundnumber = (item["FundNumber"].Controls[0] as TextBox).Text;
                string FundName = (item["FundName"].Controls[0] as TextBox).Text;
                if (Fundnumber == "" && FundName == "")
                {
                    Validations.showMessage(lblErrorMsg, Validations.Msg_EnterSearchText, "Error");
                    return;
                }


                #region Get Expression

                string expression = "";

                if (FundName.Trim() != "")
                {
                    if (expression != "")
                        expression += " OR ";
                    expression += "([FundName]  LIKE \'%" + FundName + "%\')";
                }
                if (Fundnumber.Trim() != "")
                {
                    if (expression != "")
                        expression += " OR ";
                    expression += "([Fundnumber]  = \'" + Fundnumber + "\')";
                }
              
                #endregion

                BindGrid();

                #region Assign CurrentFilterValue

                gvfund.MasterTableView.GetColumnSafe("Fundnumber").CurrentFilterValue = Fundnumber;
                gvfund.MasterTableView.GetColumnSafe("FundName").CurrentFilterValue = FundName;

                #endregion

                #region Assign CurrentFilterFunction

                gvfund.MasterTableView.GetColumnSafe("Fundnumber").CurrentFilterFunction = GridKnownFunction.Contains;
                gvfund.MasterTableView.GetColumnSafe("FundName").CurrentFilterFunction = GridKnownFunction.Contains;
      
                #endregion

                gvfund.EnableLinqExpressions = false;
                gvfund.MasterTableView.FilterExpression = expression;
                gvfund.MasterTableView.Rebind();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region ResetButton_Click
        //ResetButton_Click event makes CurrentFilterFunction for each column as null and then binds grid 
        protected void ResetButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                lblErrorMsg.Text = "";
                foreach (GridColumn column in gvfund.MasterTableView.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                gvfund.MasterTableView.FilterExpression = string.Empty;
                BindGrid();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region GridFund_ItemCommand
        //GridFund_ItemCommand event is use for two command name edit and delete
        protected void GridFund_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            string FundNumber;
            //if  command name is EditFund of fund it will get the id of particuar
            //row to edit the fund and redirecte it to newfund screen to update that fund
            if (e.CommandName == "EditFund")
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    FundNumber = item["FundNumber"].Text.ToString().Trim();

                    Response.Redirect("~/UserScreens/Addnewfund.aspx?FID=" + FundNumber);
                }

            }
            //If command name is delete it will get the id that fund which needs to be deleted by calling DELETEFund 
            //it will successfully get deleted 
            else if (e.CommandName == "Delete")
            {
                GridDataItem item = (GridDataItem)e.Item;
                objfund.Fundnumber =Convert.ToInt32( item["FundNumber"].Text.ToString());

                no_of_rows_affected = objfund.DELETEFund();
                if (no_of_rows_affected == 1)
                {
                    Validations.showMessage(lblErrorMsg, "Fund " + Validations.RecordDeleted, "");
                    BindGrid();
                  
                }
                else
                {
                    Validations.showMessage(lblErrorMsg, "Fund " + Validations.Err_RefDelete, "Error");
                }
            }

        }
        #endregion

        #region CouponTitle_NeedDataSource
        //CouponTitle_NeedDataSource event is use to bind getfunddetails to gvfund grid  from data of database
        protected void CouponTitle_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = objfund.getfunddetails();

            gvfund.DataSource = dt;

        }
        #endregion

        #region BindGrid()
        //BindGrid this method is used to bind data to gvfund using getfunddetails 
        private void BindGrid()
        {
            DataTable dt = new DataTable();
            {
                dt = objfund.getfunddetails();

                gvfund.DataSource = dt;
                gvfund.DataBind();
            }
        }
        #endregion
    }
}