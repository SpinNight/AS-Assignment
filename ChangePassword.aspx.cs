using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        bool restriction = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                lbl_policy.ForeColor = Color.Red;
                if (checkAge(Session["UserID"].ToString(), "Min"))
                {
                    lbl_policy.Text = "Password cannot be changed after 1 min of update";
                    restriction = true;
                }
                else if(checkAge(Session["UserID"].ToString(), "Max"))
                {
                    lbl_policy.Text = "Password must be changed after 3 min of update";
                }
                return;
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        
        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string email = (string)Session["UserID"];
            string pwd = HttpUtility.HtmlEncode(tb_pwd.Text.Trim());
            string cfmpwd = HttpUtility.HtmlEncode(tb_cfmpwd.Text.Trim());
            int score = checkPassword(pwd);
            if (score < 4)
            {
                lbl_pwd.ForeColor = Color.Red;
                return;
            }

            lbl_pwd.ForeColor = Color.Green;
            string dbSalt = getDBSalt(email);
            string pwdWithSalt = pwd + dbSalt;
            SHA512Managed hashing = new SHA512Managed();
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            string pwdhistory = viewHistory(email);
            string dbhash = viewPwd(email);
            if (finalHash.Equals(dbhash) || finalHash.Equals(pwdhistory))
            {
                lbl_pwd.Text = "Password must be different from the last two password history";
                lbl_pwd.ForeColor = Color.Red;
                return;
            }
            else
            {
                if (cfmpwd == pwd)
                {
                    if (restriction)
                    {
                        lbl_policy.Text = "Password cannot be changed after 1 min of update";
                        return;
                    }
                    else
                    {
                        updatePassword(email, finalHash);
                        Response.Redirect("Success.aspx", false);
                    }
                }
                else
                {
                    lbl_cfmpwd.Text = "Password is different";
                    lbl_cfmpwd.ForeColor = Color.Red;
                    return;
                }
            }
            

        }

        protected void updatePassword(string email, string pwd)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            try
            {
                updateHistory(email, viewPwd(email));
                updatePwdDate(email);
                string sql = "update Account Set PasswordHash=@PasswordHash WHERE Email=@USERID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", email);
                command.Parameters.AddWithValue("@PasswordHash", pwd);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void updateHistory(string email, string history)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            try
            {
                string sql = "update Account Set PasswordHistory=@PasswordHistory WHERE Email=@USERID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", email);
                command.Parameters.AddWithValue("@PasswordHistory", history);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void updatePwdDate(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            try
            {
                string sql = "update Account Set LastUpdatedPassword=@Updated WHERE Email=@USERID";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@USERID", email);
                command.Parameters.AddWithValue("@Updated", DateTime.Now);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }

            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected string viewHistory(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHistory FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", email);
            string pwd_history = "";
            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHistory"] != null)
                        {
                            if (reader["PasswordHistory"] != DBNull.Value)
                            {
                                pwd_history = (string)reader["PasswordHistory"];
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
            return pwd_history;
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
        protected string viewPwd(string email)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", email);
            string pwdhash = "";
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
                                pwdhash = (string)reader["PasswordHash"];
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
            return pwdhash;
        }
        private int checkPassword(string password)
        {
            int score = 0;
            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }

            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }

            if (Regex.IsMatch(password, "[!@#$%^&*()]"))
            {
                score++;
            }

            string status = "";
            switch (score)
            {
                case 1:
                    status = "Very Weak";
                    break;
                case 2:
                    status = "Weak";
                    break;
                case 3:
                    status = "Medium";
                    break;
                case 4:
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            lbl_pwd.Text = status + "password";
            return score;
        }
        protected DateTime getDate(string email)
        {
            DateTime s = new DateTime();
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select LastUpdatedPassword FROM Account WHERE Email=@Email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["LastUpdatedPassword"] != null)
                        {
                            if (reader["LastUpdatedPassword"] != DBNull.Value)
                            {
                                s = (DateTime) reader["LastUpdatedPassword"];
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

        protected bool checkAge(string email, string parameter)
        {
            DateTime lastupdated = getDate(email);
            if (parameter == "Min")
            {
                if(DateTime.Now.Subtract(lastupdated).Minutes < 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (parameter == "Max")
            {
                if (DateTime.Now.Subtract(lastupdated).Minutes > 3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}