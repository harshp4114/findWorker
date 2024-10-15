using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

namespace findWorker2
{
    public partial class WorkerRequests : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            if (!IsPostBack)
            {
                BindRequestsGrid();
            }
        }

        private void BindRequestsGrid()
        {
            string workerUsername = Session["Username"].ToString();
            string connectionString = ConfigurationManager.ConnectionStrings["connectUser"].ConnectionString;
            string query = "SELECT r.RequestID, w.Title, r.Status " +
                           "FROM Requests r INNER JOIN Works w ON r.WorkID = w.WorkID " +
                           "WHERE r.WorkerUsername = @WorkerUsername";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@WorkerUsername", workerUsername);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                gvRequests.DataSource = reader;
                gvRequests.DataBind();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Worker.aspx");
        }
    }
}
