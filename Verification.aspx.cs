using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Verification : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Emailcode"] == null && Session["LoggingIn"] == null)
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string code = HttpUtility.HtmlEncode(tb_code.Text);
            if (Session["Emailcode"] != null)
            {
                if (code == (string)Session["Emailcode"].ToString())
                {
                    SqlConnection connection = new SqlConnection(MYDBConnectionString);
                    string email = (string)Session["Email"];
                    try
                    {
                        string sql = "update Account Set IsVerified=@IsVerified WHERE Email=@USERID";
                        SqlCommand command = new SqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@USERID", email);
                        command.Parameters.AddWithValue("@IsVerified", true);
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        Session["Email"] = null;
                        Session["Emailcode"] = null;
                    }

                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }

                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lbl_code.Text = "Invalid code";
                    lbl_code.ForeColor = Color.Red;
                    return;
                }
            }
            else if (Session["LoggingIn"] != null)
            {
                if (code == Session["LoggingIn"].ToString())
                {
                    Session["LoggingIn"] = null;
                    Response.Redirect("Success.aspx", false);
                }
                else
                {
                    lbl_code.Text = "Invalid code";
                    lbl_code.ForeColor = Color.Red;
                    return;
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
    }
}