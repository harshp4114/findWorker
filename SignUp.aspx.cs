using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace findWorker2
{
    public partial class SignUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string email = txtEmail.Text;
            string userType = ddlUserType.SelectedValue;
            string fullName = txtFullName.Text;
            string phoneNum = txtPhoneNum.Text; 

            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;

            var validationMessage = ValidateUser(username, email, phoneNum, userType, connectionString);
            if (!string.IsNullOrEmpty(validationMessage))
            {
                lblMessage.Text = validationMessage;
                lblMessage.ForeColor = System.Drawing.Color.Red;
                return; 
            }

            string query = "INSERT INTO [dbo].[Users] (username, password, email, userType, fullName, phoneNum) " +
                           "VALUES (@Username, @Password, @Email, @UserType, @FullName, @PhoneNum)";
            SqlConnection con = new SqlConnection(connectionString);
            using (con)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@UserType", userType);
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@PhoneNum", phoneNum);

                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (result > 0)
                    {
                        lblMessage.Text = "Registration successful!";
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        ClearFields();
                        Response.Redirect("Login.aspx");
                    }
                    else
                    {
                        lblMessage.Text = "Registration failed!";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        private string ValidateUser(string username, string email, string phoneNum, string userType, string connectionString)
        {
            string message = "";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string userQuery = "SELECT COUNT(*) FROM [dbo].[Users] WHERE username = @Username AND userType = @UserType";
                SqlCommand userCmd = new SqlCommand(userQuery, con);
                userCmd.Parameters.AddWithValue("@Username", username);
                userCmd.Parameters.AddWithValue("@UserType", userType);

                con.Open();
                int userExists = (int)userCmd.ExecuteScalar();
                if (userExists > 0)
                {
                    message += "Username is already registered.<br />";
                }

                string emailQuery = "SELECT COUNT(*) FROM [dbo].[Users] WHERE email = @Email";
                SqlCommand emailCmd = new SqlCommand(emailQuery, con);
                emailCmd.Parameters.AddWithValue("@Email", email);
                int emailExists = (int)emailCmd.ExecuteScalar();
                if (emailExists > 0)
                {
                    message += "Email is already registered.<br />";
                }

                string phoneQuery = "SELECT COUNT(*) FROM [dbo].[Users] WHERE phoneNum = @PhoneNum";
                SqlCommand phoneCmd = new SqlCommand(phoneQuery, con);
                phoneCmd.Parameters.AddWithValue("@PhoneNum", phoneNum);
                int phoneExists = (int)phoneCmd.ExecuteScalar();
                if (phoneExists > 0)
                {
                    message += "Phone number is already registered.<br />";
                }
            }

            return message; 
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx"); 
        }

        private void ClearFields()
        {
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtEmail.Text = "";
            ddlUserType.SelectedIndex = 0; 
            txtFullName.Text = "";
            txtPhoneNum.Text = "";
        }
    }
}
