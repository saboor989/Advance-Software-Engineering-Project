using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;



namespace ChurchRecordkeeping
{
    public partial class LoginPage : System.Web.UI.Page
    {
        //PageLoad method get executed whenever the page is loaded . 
        protected void Page_Load(object sender, EventArgs e)
        {
            //Postback is check if the page is referred for first time or loading
            if (!IsPostBack)
            {
                //Checking the Cookie used "yourapp" if its empty or no if empty do nothing.
                if (Request.Cookies["yourapp"] == null)
                {
                }
                else
                {
                    //The username present in the cookie is read to populate the username field onload
                    string username = Request.Cookies["yourapp"].Values["UserName1"];

                    UserName.Text = username;
                }
            }
        }

        //Function get executed when the login button is clicked. An event function.
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            //Reemberme checkbox is read for true or false.
            if (RememberMe.Checked == true)
            {
                //The password and username field values and read and their values are compared with the correct value for authentication
                if (Password.Text == "secretary" && UserName.Text == "wutrich")
                {
                    string s = UserName.Text;
                    //The cookie is created to store the username.
                    HttpCookie cookie = new HttpCookie("yourapp");
                    cookie.Values.Add("UserName1", s);
                    //The cookie is set expiration days
                    cookie.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(cookie);
                    //Redirecting the page to Member page 
                    Response.Redirect("~/UserScreens/memb.aspx");
                }
                else
                {
                    //Setting the error message if wrong username and password is entered.
                    lblErrorMsg.Text = "Please enter correct username and password";
                }
            }
            else
            {
                //The password and username field values and read and their values are compared with the correct value for authentication
                if (Password.Text == "secretary" && UserName.Text == "wutrich")
                {
                    //The cookie is created to store the username.
                    HttpCookie cookie = new HttpCookie("yourapp");
                    cookie.Values.Add("UserName1", "");
                    //The cookie is set expiration days
                    cookie.Expires = DateTime.Now.AddDays(15);
                    Response.Cookies.Add(cookie);
                    //Redirecting the page to Member page 
                    Response.Redirect("~/UserScreens/memb.aspx");
                }
                else
                {
                    //Setting the error message if wrong username and password is entered.
                    lblErrorMsg.Text ="Please enter correct username and password";
                }

            }




        }
       
    }
}


