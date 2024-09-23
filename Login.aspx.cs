using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Drawing.Printing;
using System.Web;

namespace findWorker2
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["Username"] != null)
                {
                    // Redirect to appropriate page based on user type
                    string userType = Session["UserType"].ToString();
                    if (userType == "Provider")
                    {
                        Response.Redirect("Provider.aspx");
                    }
                    else if (userType == "Worker")
                    {
                        Response.Redirect("Worker.aspx");
                    }
                }
                else
                {
                    // Reset fields
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    ddlUserType.SelectedIndex = 0;
                    lblMessage.Text = "";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            // Get user inputs
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string userType = ddlUserType.SelectedValue;

            // Get connection string from Web.config
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;

            // SQL query to check for user existence and validate password
            string query = "SELECT password FROM [dbo].[Users] WHERE username = @Username AND userType = @UserType";
            SqlConnection con = new SqlConnection(connectionString);

            using (con)
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@UserType", userType);

                try
                {
                    con.Open();
                    object dbPassword = cmd.ExecuteScalar();

                    if (dbPassword != null)
                    {
                        // In btnLogin_Click method
                        if (dbPassword.ToString() == password)
                        {
                            // Password matches, set session variables
                            Session["Username"] = username;
                            Session["UserType"] = userType;
                            Session["Password"] = password; 

                            // Redirect based on user type
                            if (userType == "Provider")
                            {
                                Response.Redirect("Provider.aspx");
                            }
                            else if (userType == "Worker")
                            {
                                Response.Redirect("Worker.aspx");
                            }
                        }else
                        {
                            //Response.Write(dbPassword);
                            // Password does not match
                            lblMessage.Text = "Password for the given username doesn't match";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        // Username not found
                        lblMessage.Text = "Username not found for the selected user type.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors (e.g., log the error or display a message)
                    lblMessage.Text = "An error occurred: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("SignUp.aspx");
        }
    }
}
