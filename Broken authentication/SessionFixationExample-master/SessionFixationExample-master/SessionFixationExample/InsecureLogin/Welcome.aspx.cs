using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SessionFixationExample
{
    public partial class Welcome : System.Web.UI.Page
    {
        //This is page load event of Welcome Page, this event will be triggered when user redirected to this page
        protected void Page_Load(object sender, EventArgs e)
        {
            // If the session value is not null
            if (Session["userLoggedin"] != null)
            {
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Font.Size = FontUnit.Point(15);
                lblMessage.Text = "Welcome, session is Alive and the value of Session is: " + Session["userLoggedin"].ToString();

                //Fetching the value of Cookie 'ASP.NET_SessionI' to show what the users session value is
                string cookieValue = "";
                if (Request.Cookies.Count > 0)
                {
                    cookieValue = Request.Cookies["ASP.NET_SessionId"].Value;
                    lblMessage.Text = "You session value is " + cookieValue;
                }
                btnLogout.Visible = true;

            }
            else
            {

                string cookieValue = "";
                if (Request.Cookies.Count > 0)
                {
                    cookieValue = Request.Cookies["ASP.NET_SessionId"].Value;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Font.Size = FontUnit.Point(15);
                    lblMessage.Text = "You session value is " + cookieValue;
                }
            }



        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //Clear the session and redirect to login
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/InsecureLogin/Login.aspx");
        }
    }
}