using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace findWorker2
{
    public partial class Provider : System.Web.UI.Page
    {
        // Declare the welcomeUser variable at the class level
        public string welcomeUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                LoadUserName(); // Load the user's full name on first page load
                BindWorksGrid(); // Load works when page loads
            }
        }

        protected void ValidateFutureDate(object source, ServerValidateEventArgs args)
        {
            DateTime selectedDate;
            if (DateTime.TryParse(txtDateOfWork.Text, out selectedDate))
            {
                // Check if the selected date is today or later
                if (selectedDate >= DateTime.Today)
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                }
            }
            else
            {
                args.IsValid = false; // If date parsing fails, mark as invalid
            }
        }

        private void LoadUserName()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT fullName FROM Users WHERE username=@Username AND password=@password AND userType=@type";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Username", Session["Username"]);
                cmd.Parameters.AddWithValue("@password", Session["Password"]);
                cmd.Parameters.AddWithValue("@type", Session["UserType"]);

                try
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        welcomeUser = result.ToString(); // Assign full name to welcomeUser
                    }
                    else
                    {
                        welcomeUser = "User"; // Fallback if not found
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "An error occurred: " + ex.Message;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        protected void btnAddWork_Click(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                string title = txtTitle.Text;
                string description = txtDescription.Text;
                string providerUsername = Session["Username"].ToString();
                string workerUsername = "Not applied by anyone yet"; // Default value
                string status = "Open"; // Default status when adding new work
                DateTime dateOfWork;

                // Validate and parse the date
                if (!DateTime.TryParse(txtDateOfWork.Text, out dateOfWork))
                {
                    lblMessage.Text = "Please enter a valid date.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    return;
                }

                // Get the connection string from Web.config
                string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;

                // SQL query to insert new work
                string query = "INSERT INTO Works (ProviderUsername, WorkerUsername, Title, Description, Status, DateOfWork) " +
                               "VALUES (@ProviderUsername, @WorkerUsername, @Title, @Description, @Status, @DateOfWork)";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@ProviderUsername", providerUsername);
                    cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@DateOfWork", dateOfWork);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            lblMessage.Text = "Work added successfully!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            ClearFields();
                            BindWorksGrid(); // Refresh the grid
                        }
                        else
                        {
                            lblMessage.Text = "Failed to add work.";
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
        }

        protected void gvWorks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblWorkerUsername = (Label)e.Row.FindControl("lblWorkerUsername");
                if (lblWorkerUsername != null)
                {
                    if (lblWorkerUsername.Text == "Not applied by anyone yet")
                    {
                        lblWorkerUsername.ForeColor = System.Drawing.Color.Red; // Set to red if not applied
                    }
                    else
                    {
                        lblWorkerUsername.ForeColor = System.Drawing.Color.Black; // Set to black if applied by a worker
                    }
                }
            }
        }


        private void BindWorksGrid()
        {
            string providerUsername = Session["Username"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT WorkID, Title, Description, Status, DateOfWork, WorkerUsername FROM Works WHERE ProviderUsername = @ProviderUsername";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@ProviderUsername", providerUsername);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvWorks.DataSource = reader;
                gvWorks.DataBind();
            }
        }

        private void ClearFields()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtDateOfWork.Text = ""; // Clear the date field
        }
    }
}
