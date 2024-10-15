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
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindWorksGrid();
            }
        }

        protected void btnSignOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }

        private void BindWorksGrid()
        {
            string workerUsername = Session["Username"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = @"SELECT w.WorkID, w.ProviderUsername, w.Title, w.Description, w.Status, w.DateOfWork, w.WorkerUsername,
                             CASE WHEN r.RequestID IS NOT NULL THEN 1 ELSE 0 END AS HasApplied
                             FROM Works w
                             LEFT JOIN Requests r ON w.WorkID = r.WorkID AND r.WorkerUsername = @WorkerUsername";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvWorks.DataSource = reader;
                gvWorks.DataBind();
            }
        }

        protected void btnViewRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("WorkerRequests.aspx");
        }

        protected void gvWorks_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Apply")
            {
                int workID = Convert.ToInt32(e.CommandArgument);
                string workerUsername = Session["Username"].ToString();

                string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
                string checkQuery = "SELECT COUNT(*) FROM Requests WHERE WorkID = @WorkID AND WorkerUsername = @WorkerUsername";

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                    checkCmd.Parameters.AddWithValue("@WorkID", workID);
                    checkCmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                    con.Open();
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        lblMessage.Text = "You have already applied for this work.";
                        lblMessage.Visible = true;
                        return;
                    }
                }

                string insertQuery = "INSERT INTO Requests (WorkID, WorkerUsername) VALUES (@WorkID, @WorkerUsername)";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@WorkID", workID);
                    cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (result > 0)
                        {
                            lblMessage.Text = "Request sent successfully!";
                            lblMessage.Visible = true;
                            BindWorksGrid();
                        }
                        else
                        {
                            lblMessage.Text = "Failed to send request.";
                            lblMessage.Visible = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred: " + ex.Message;
                        lblMessage.Visible = true;
                    }
                }
            }
        }
    }
}
