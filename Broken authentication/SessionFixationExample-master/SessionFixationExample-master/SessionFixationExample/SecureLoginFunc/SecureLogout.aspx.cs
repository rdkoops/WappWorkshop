using System;

using System.Web.UI.WebControls;

namespace SessionFixationExample
{
    public partial class SecureLogout : System.Web.UI.Page
    {
        //This is page load event of Logout Page, this event will be triggered when user redirected to this page (When he clicked the logout button)
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check all three variables Session1, Session2, Cookie. If all the three are not null then proceed
            if (Session["userLoggedin"] != null && Session["AuthToken"] != null
                          && Request.Cookies["AuthToken"] != null)
            {
                //Second Check, if the Cookie we created has the same value as Second Session we've created
                if ((Session["AuthToken"].ToString().Equals(
                           Request.Cookies["AuthToken"].Value)))
                {
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Font.Size = FontUnit.Point(15);
                    lblMessage.Text = "Welcome " + Session["userLoggedin"].ToString();
                    btnLogout.Visible = true;
                    string cookieValue = "";
                    cookieValue = Request.Cookies["AuthToken"].Value;
                    lblAuthCookie.ForeColor = System.Drawing.Color.Green;
                    lblAuthCookie.Font.Size = FontUnit.Point(15);
                    lblAuthCookie.Text = "You AuthToken is " + cookieValue;
                }
                else
                {
                    Response.Redirect("~/SecureLoginFunc/SecureLogin.aspx");

                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            //Clear Session
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                //Empty the Cookie ASP.NET_SessionId
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }
            if (Request.Cookies["AuthToken"] != null)
            {
                //Empty the Cookie AuthToken
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }
            Response.Redirect("~/SecureLoginFunc/SecureLogin.aspx");
        }
    }
}