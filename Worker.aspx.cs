using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace findWorker2
{
    public partial class Worker : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindWorksGrid(); // Load all works on first page load
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        private void BindWorksGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            // Select all works regardless of status
            string query = "SELECT WorkID, ProviderUsername, Title, Description, Status, DateOfWork, WorkerUsername FROM Works";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvWorks.DataSource = reader;
                gvWorks.DataBind();
            }
        }

        protected void gvWorks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Apply")
            {
                int workID = Convert.ToInt32(e.CommandArgument); // Get WorkID from CommandArgument
                string workerUsername = Session["Username"].ToString(); // Get current worker's username

                // Update the work status and worker username
                string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
                string query = "UPDATE Works SET Status = 'Applied', WorkerUsername = @WorkerUsername WHERE WorkID = @WorkID";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                    cmd.Parameters.AddWithValue("@WorkID", workID);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text = "You have successfully applied for the work!";
                            lblMessage.ForeColor = System.Drawing.Color.Green;
                            BindWorksGrid(); // Refresh the grid after applying
                        }
                        else
                        {
                            lblMessage.Text = "Failed to apply for the work.";
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
    }
}
