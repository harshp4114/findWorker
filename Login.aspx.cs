using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;


namespace findWorker2
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                if (Session["Username"] != null)
                {
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
                    txtUsername.Text = "";
                    txtPassword.Text = "";
                    ddlUserType.SelectedIndex = 0;
                    lblMessage.Text = "";
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string userType = ddlUserType.SelectedValue;

            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;

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
                        if (dbPassword.ToString() == password)
                        {
                            Session["Username"] = username;
                            Session["UserType"] = userType;
                            Session["Password"] = password;

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
                            //Response.Write(dbPassword);
                            lblMessage.Text = "Password for the given username doesn't match";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                        }
                    }
                    else
                    {
                        lblMessage.Text = "Username not found for the selected user type.";
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

        protected void btnSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("SignUp.aspx");
        }
    }
}
