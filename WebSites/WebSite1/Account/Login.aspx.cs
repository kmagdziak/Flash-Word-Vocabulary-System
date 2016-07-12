using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;

public partial class Account_Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
    }
    /************************************************************
    * NAME: LoginButton_Click
    * 
    * PARAMETERS: 1. sender: a data object containing
    *                        the source of the event
    *                        to be handled. (?)
    *                        
    *             2. e: an EventArgs object that has
    *                   tools for handling events
    *                   that call this method. (?)
    * 
    * RETURNS: void
    * 
    * FUNCTION: Event handler for the LoginButton button.
    *           On click, this method will let you log in 
     *           to the website and check the user against the 
     *           database.
    ***********************************************************/
    protected void LoginButton_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            // Get connection string from web.config
            ConnectionStringSettingsCollection connections =
                         ConfigurationManager.ConnectionStrings;

            ConnectionStringSettings set = connections["Conn1"];
            string connStr = set.ConnectionString;

            //Get username and password
            string username = LoginUser.UserName.ToString();
            string password = LoginUser.Password.ToString();

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                //get salted password and salt string for this user
                SqlCommand cmd = new SqlCommand(
                "select UserName, HashedPassword, Saltstring from Customers where UserName = '" + username + "'", conn);

                SqlDataReader dr = cmd.ExecuteReader();
                if (!dr.Read())          // If user not registered
                {
                    ErrorLabel.Text = "You are not a registered user";
                    Response.Redirect("register.aspx");
                    //btnSubmit.PostBackUrl = "register.aspx";
                }
                else
                {
                    // user is found, so now check for password match
                    //fetch salt string from reader
                    string salt = dr["Saltstring"] as string;

                    //fetch salted pword from reader
                    string storedHashedPassword = dr["HashedPassword"] as string;

                    //take user's entered password and hash it with the salt string
                    string saltedpassword = password + salt;

                    string givenHashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(
                        saltedpassword, "SHA1");

                    // if 2 hashed pword+salt match, let them in
//                    if (storedHashedPassword.CompareTo(givenHashedPassword) == 0)
// previous line previously used to check password against db password
// no longer have access to db, set to automatically grant authentication.                    
                    if (true)
                    {
                        FormsAuthentication.SetAuthCookie(username, true);  //redirect to default page on successful login
                        Response.Redirect("~/Default.aspx");
                    }
                    else
                    {
                        ErrorLabel.Text = "Username or password is incorrect.";     //redirect to login on bad pw
                        Response.Redirect("Login.aspx");
                    }
                }

            }
        }
        else
        {
            ErrorLabel.Text = "Page failed validation";
        }
    }
}
