using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SessionFixationExample
{
    public partial class Login : System.Web.UI.Page
    {
        //This is the page load event. 
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMessage.ForeColor = System.Drawing.Color.DarkKhaki;
            lblMessage.Text = " username and password: [admin/admin], [test/test] ";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        { 
            //When user enter valid credentials in username and password field
            if ((txtUsername.Text.Trim().Equals("admin") && txtPassword.Text.Trim().Equals("admin")) ||
                (txtUsername.Text.Trim().Equals("test") && txtPassword.Text.Trim().Equals("test")))
            {
                //Creating a Session for that user and stores it on the server side
                Session["userLoggedin"] = txtUsername.Text.Trim();
                Response.Cookies["ASP.NET_SessionId"].HttpOnly = false;

                //Once the session is created the user redirects to the Welcome page
                Response.Redirect("~/InsecureLogin/Welcome.aspx");

            }
            else
            {
                //If user enter invalid credentials throw an error
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Invalid username or password";
            }
        }


    }
}