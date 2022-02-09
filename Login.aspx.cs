using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {
                string userid = HttpUtility.HtmlEncode(tb_userid.Text.ToString().Trim());
                string pwd = HttpUtility.HtmlEncode(tb_pwd.Text.ToString().Trim());

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(userid);
                string dbSalt = getDBSalt(userid);
                string dbFailedAttempt = getFailedAttempt(userid);

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbFailedAttempt != null && dbHash.Length > 0)
                    {
                        int dbFailedAttemptInt = Convert.ToInt32(dbFailedAttempt);
                        if (dbFailedAttemptInt < 3)
                        {
                            string pwdWithSalt = pwd + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);

                            if (userHash.Equals(dbHash))
                            {
                                Session["UserID"] = userid;
                                string guid = Guid.NewGuid().ToString();
                                Session["AuthToken"] = guid;
                                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                                dbFailedAttemptInt = 0;
                                updateFailedAttempt(userid, dbFailedAttemptInt);
                                Response.Redirect("Success.aspx", false);
                            }
                            else
                            {
                                errorMsg = "Userid or password is not valid. Please try again.";
                                lb_error.Text = errorMsg;
                                lb_error.ForeColor = Color.Red;
                                dbFailedAttemptInt += 1;
                                updateFailedAttempt(userid, dbFailedAttemptInt);
                                return;
                            }
                        }
                        else
                        {
                            errorMsg = "Account Lockout. Please contact the admin!";
                            lb_error.Text = errorMsg;
                            lb_error.ForeColor = Color.Red;
                            return;
                        }
                    }
                    else
                    {
                        errorMsg = userid;
                        lb_error.Text = errorMsg;
                        lb_error.ForeColor = Color.Red;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { }
            }
        }


        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }

        protected string getFailedAttempt(string userid)
        {
            string fAttempt = "";

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select FailedAttempt FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["FailedAttempt"] != null)
                        {
                            if (reader["FailedAttempt"] != DBNull.Value)
                            {
                                fAttempt = reader["FailedAttempt"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return fAttempt;
        }

        protected void updateFailedAttempt(string userid, int dbFailedAttemptInt)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            try
            {
                string sql = "update Account Set FailedAttempt=@FailedAttempt WHERE Email=@USERID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", userid);
                command.Parameters.AddWithValue("@FailedAttempt", dbFailedAttemptInt);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6LdkzmkeAAAAAKWE4M9kvpPCq8zeBd-uxTZ3zFAP &response=" + captchaResponse);

            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        lbl_gScore.Text = jsonResponse.ToString();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }
    }
}