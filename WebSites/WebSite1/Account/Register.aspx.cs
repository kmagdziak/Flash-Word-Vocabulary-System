using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security; 
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;     
using System.Data.SqlClient;
using System.Text;
using wordLib;

public partial class Account_Register : System.Web.UI.Page
{
    Database db = new Database();
    //load page
    protected void Page_Load(object sender, EventArgs e)
    {
       //RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
    }
    //register to our webpage
    public void RegisterUser_CreatedUser(object sender, EventArgs e)
    {

    }
    /************************************************************
    * NAME: CreateUserButton_Click
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
    * FUNCTION: Event handler for the CreateUserButton button.
    *           On click, this method will add a user and their
     *          credentials to the database.
    ***********************************************************/
    public void CreateUserButton_Click(object sender, EventArgs e)
    {
        //Check IsValid Page property to make sure
        //all controls passed validation
        if (Page.IsValid)
        {
            string uname = UserName.Text;
            string pword = Password.Text;
            string fname = FirstName.Text;
            string lname = LastName.Text;

            // We are going to salt this user's password so it is
            // saved in the database in an encrypted fashion

            // Generate random salt string
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[16];
            csp.GetNonZeroBytes(saltBytes);
            string saltString = Convert.ToBase64String(saltBytes);

            // Append the salt string to the password
            string saltedPassword = pword + saltString; // append to user entered password

            // Hash the salted password and save in string "hash"
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile
                (saltedPassword, "SHA1");

            //Get connection string
            ConnectionStringSettingsCollection connections = ConfigurationManager.ConnectionStrings;
            ConnectionStringSettings settings = connections["Conn1"];
            string connStr = settings.ConnectionString;

            //Get connected and enter user into database
            SqlConnection conn = new SqlConnection(connStr);
            conn.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = conn;
            string insertstr = "Insert into Customers values (@fname, @lname, @uname, @hpassword, @salt)";      //insert into the database values for register
            command.CommandText = insertstr;
            command.Parameters.AddWithValue("@fname", fname);
            command.Parameters.AddWithValue("@lname", lname);
            command.Parameters.AddWithValue("@uname", uname);
            command.Parameters.AddWithValue("@hpassword", hash);
            command.Parameters.AddWithValue("@salt", saltString);
            try
            {
                // Do insert. Then redirect back to login page.
                int x = command.ExecuteNonQuery();
                Response.Redirect("Login.aspx");

            }
            catch (SqlException sqle) {}
            finally
            {
                conn.Close();
            }
        }
    }
}
