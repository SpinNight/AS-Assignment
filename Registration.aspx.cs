using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AS_Assignment
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private bool emailExist(string email)
        {
            using (SqlConnection con = new SqlConnection(MYDBConnectionString))
            {
                con.Open();
                SqlCommand check_User_Name = new SqlCommand("SELECT * FROM Account WHERE Email = @Email", con);
                check_User_Name.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(email));
                SqlDataReader reader = check_User_Name.ExecuteReader();
                if (reader.HasRows)
                {
                    lbl_emailchecker.Text = "Email already exist!";
                    lbl_emailchecker.ForeColor = Color.Red;
                    return true;
                }
                con.Close();

            }
            return false;
        }


        private bool emailIsValid(string email)
        {
            string mailAccount = System.Configuration.ConfigurationManager.ConnectionStrings["mailAccount"].ConnectionString;
            string mailPassword = System.Configuration.ConfigurationManager.ConnectionStrings["mailPassword"].ConnectionString;
            try
            {
                Random random = new Random();
                int code = random.Next(000000, 1000000);

                Session["Emailcode"] = code;

                MailMessage mm = new MailMessage();
                mm.To.Add(new MailAddress(email, "Request for Verification"));
                mm.From = new MailAddress(mailAccount);
                mm.Body = "Your verification code is: " + code;
                mm.IsBodyHtml = true;
                mm.Subject = "Verification";
                SmtpClient smcl = new SmtpClient();
                smcl.Host = "smtp.gmail.com";
                smcl.Port = 587;
                smcl.Credentials = new NetworkCredential(mailAccount, mailPassword);
                smcl.EnableSsl = true;
                smcl.Send(mm);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
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
            lbl_pwdchecker.Text = status + "password";
            return score;
        }


        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string email = HttpUtility.HtmlEncode(tb_userid.Text.Trim());
            string pwd = HttpUtility.HtmlEncode(tb_pwd.Text.Trim());
            string cfmpwd = HttpUtility.HtmlEncode(tb_cfpwd.Text.Trim());
            string first = HttpUtility.HtmlEncode(tb_firstname.Text.Trim());
            string last = HttpUtility.HtmlEncode(tb_lastname.Text.Trim());
            string ccard = HttpUtility.HtmlEncode(tb_creditcard.Text.Trim());


            if (string.IsNullOrEmpty(email))
            {
                lbl_emailchecker.Text = "Input field is empty!";
                lbl_emailchecker.ForeColor = Color.Red;
                return;
            }
            else
            {
                if(!emailExist(email))
                {
                    if (emailIsValid(email))
                    {
                        lbl_emailchecker.Text = "Valid Email";
                        lbl_emailchecker.ForeColor = Color.Green;
                    }
                    else
                    {
                        lbl_emailchecker.Text = "Invalid Email";
                        lbl_emailchecker.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    return;
                }
            }

            if (string.IsNullOrEmpty(first))
            {
                lbl_first.Text = "Input field is empty!";
                lbl_first.ForeColor = Color.Red;
                return;
            }
            else
            {
                lbl_first.Text = "";
            }

            if (string.IsNullOrEmpty(last))
            {
                lbl_last.Text = "Input field is empty!";
                lbl_last.ForeColor = Color.Red;
                return;
            }
            else
            {
                lbl_last.Text = "";
            }

            if (string.IsNullOrEmpty(pwd))
            {
                lbl_pwdchecker.Text = "Input field is empty!";
                return;
            }
            else
            {
                int score = checkPassword(pwd);
                if (score < 4)
                {
                    lbl_pwdchecker.ForeColor = Color.Red;
                    return;
                }

                lbl_pwdchecker.ForeColor = Color.Green;

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = pwd + salt;
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
            }

            if (string.IsNullOrEmpty(cfmpwd))
            {
                lbl_cfpwd.Text = "Input field is empty";
                return;
            }
            else
            {
                if (cfmpwd != pwd)
                {
                    lbl_cfpwd.Text = "Password does not match";
                    lbl_cfpwd.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    lbl_cfpwd.Text = "";
                }
            }

            if (string.IsNullOrEmpty(ccard))
            {
                lbl_creditcardchecker.Text = "Input field is empty";
                return;
            }
            else
            {
                if (ccard.Length !=16 && Regex.IsMatch(ccard, "^[0-9]*$"))
                {
                    lbl_creditcardchecker.Text = "Minimum 16 digit";
                    lbl_creditcardchecker.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    lbl_creditcardchecker.Text = "";
                }
            }

            createAccount();

            Response.Redirect("Verification.aspx", false);
        }
        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@FirstName,@LastName,@CreditCardNumber,@Email,@DateOfBirth,@Photo,@PasswordHash,@PasswordSalt,@IV,@Key,@FailedAttempt,@IsVerified,@LastLogin,@PasswordHistory,@LastUpdatedPassword)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@FirstName", HttpUtility.HtmlEncode(tb_firstname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@LastName", HttpUtility.HtmlEncode(tb_lastname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@Email", HttpUtility.HtmlEncode(tb_userid.Text.Trim()));
                            Session["Email"] = HttpUtility.HtmlEncode(tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@CreditCardNumber", HttpUtility.HtmlEncode(Convert.ToBase64String(encryptData(tb_creditcard.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@DateOfBirth", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Photo", DBNull.Value);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@FailedAttempt", 0);
                            cmd.Parameters.AddWithValue("@IsVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@LastLogin", DBNull.Value);
                            List<string> passwordlist = new List<string>();
                            passwordlist.Add(finalHash);
                            cmd.Parameters.AddWithValue("@PasswordHistory", DBNull.Value);
                            cmd.Parameters.AddWithValue("@LastUpdatedPassword", DateTime.Now);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }
    }
}