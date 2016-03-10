using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNet.Identity.MongoDB;
using System.Collections;
using DevExpress.Web;
using MongoDB.Bson;
using SelectPdf;
using System.IO;
using System.Net;

namespace MBTimeSheetWebApp
{
    public partial class History : System.Web.UI.Page
    {
        public static ProjectInfo Proj { get; set; }
        public static ObjectId ProjId { get; set; }
        public static bool isDraft;
        public DataSet Ds;
        public DataSet SvDS;
        public DataSet CrDS;
        public DataSet UserDS;
        public static string DataSetName;
        public static bool isFilteredView = false;
        public static bool isNewSelection;

        protected void Page_Load(object sender, EventArgs e)
        {
            string permissions = MongoDb.CheckUserPermissions();
            if (permissions == "Admin")
            {
                LoadUsersBtn.Visible = true;
                DeleteBtn.Visible = true;
                DeleteBtn.ClientVisible = true;
                LoadUsersBtn.ClientVisible = true;
            }
            if (!isFilteredView)
            {
                BindGrid();
            }

            isNewSelection = false;
            isDraft = false;
        }

        private void BindGrid()
        {
            DataTable dt = new DataTable();
            if (DataSetName != null)
            {
                SetVisibleGrid(DataSetName);

                if (DataSetName == "HistoryDS")
                {
                    dt = SetDataSource(MongoDb.GetProjectInfo(), "HistoryDS");
                    
                    HistoryGV.DataSource = dt;//GetDataSet(DataSetName); ;
                    HistoryGV.DataBind();
                }
                else if (DataSetName == "SvDS")
                {
                    GetDataSet(DataSetName);
                    dt = SetDataSource(MongoDb.GetWorksheetinfo(), "SvDS");
                    SvGridView.DataSource = dt;//GetDataSet(DataSetName); 
                    SvGridView.DataBind();
                }
                else if (DataSetName == "CrDS")
                {
                    dt = SetDataSource(MongoDb.GetCRinfo(), "CrDS");
                    //CrDS = GetDataSet(DataSetName);
                    CrGridView.DataSource = dt;// CrDS;
                    CrGridView.DataBind();
                }
                else if (DataSetName == "UserDS")
                {
                    ASPxGridView grid = GetCurrentGrid(false);
                    UserDS = GetDataSet(DataSetName);
                    grid.DataSource = UserDS.Tables[0];
                    grid.DataBind();
                }
            }
            else
            {
                ASPxGridView grid =  GetCurrentGrid(false);
                dt = SetDataSource(MongoDb.GetProjectInfo(), GetDataSourceName(grid));
                grid.DataSource = dt;
                grid.DataBind();
            }

        }

        private void SetVisibleGrid(string dataSourceName)
        {
            if (dataSourceName == "HistoryDS")
            {
                HistoryGV.Visible = true;
                SvGridView.Visible = false;
                CrGridView.Visible = false;
                UsersGridView.Visible = false;
            }
            else if (dataSourceName == "CrDS")
            {
                CrGridView.Visible = true;
                HistoryGV.Visible = false;
                SvGridView.Visible = false;
                UsersGridView.Visible = false;
            }
            else if (dataSourceName == "SvDS")
            {
                SvGridView.Visible = true;
                HistoryGV.Visible = false;
                CrGridView.Visible = false;
                UsersGridView.Visible = false;
            }
            else if (dataSourceName == "UsersDS")
            {
                UsersGridView.Visible = true;
                SvGridView.Visible = false;
                HistoryGV.Visible = false;
                CrGridView.Visible = false;
            }
            
        }

        protected void LoadBtn_Click(object sender, EventArgs e)
        {
            ASPxGridView grid = GetCurrentGrid(true);
            DataRowView x = grid.GetRow(grid.FocusedRowIndex) as DataRowView;
            DataRow row = x.Row;
            ProjId = ObjectId.Parse(row.ItemArray[1].ToString());
            isNewSelection = true;

            Response.Redirect("~/", false);

        }

        protected void LoadUsersBtn_Click(object sender, EventArgs e)
        {
            UsersGridView.DataSource = MongoDb.GetAllUsers();
            UsersGridView.DataBind();
            UsersGridView.Visible = true;
            LoadProjBtn.Visible = true;
        }

        protected void RefreshBtn_Click(object sender, EventArgs e)
        {
            ASPxGridView grid = GetCurrentGrid(true);
            SetUsersProjectGridValues(grid);
        }

        private void SetUsersProjectGridValues(ASPxGridView grid)
        {
            var users = UsersGridView.GetSelectedFieldValues("UserName");
            //string dsName = GetDataSourceName(grid);
            string dsName = "UserDS";
            List<string> userList = users.OfType<string>().ToList();

            DataTable dt = GetUsersProjects(userList, grid);
            UserDS = new DataSet();

            UserDS.Tables.AddRange(new DataTable[] { dt });
            Session[dsName] = UserDS;
            DataSetName = dsName;  

            if (grid != HistoryGV)
            {
                grid.Visible = false;
                HistoryGV.Visible = true;
            }

            HistoryGV.DataSource = UserDS.Tables[0];
            HistoryGV.DataBind();
        }

        private DataTable GetUsersProjects(List<string> userList, ASPxGridView grid)
        {
            DataTable dt = new DataTable();
            DataTable existingDt = new DataTable();
            DataSet dset = (DataSet)Session["HistoryDS"];
            existingDt = dset.Tables[0];
            isFilteredView = true;

            dt = (DataTable)grid.DataSource;
            DataView dv = new DataView(existingDt);

            string query = "";
            int count = 0;

            foreach (string user in userList)
            {

                var docs = MongoDb.GetUserDocs(user);

                if (userList.Count > 1)
                {
                    if (count < userList.Count - 1)
                    {
                        query += "UserName = '" + user + "' OR ";
                        count++;
                    }
                    else
                    {
                        query += "UserName = '" + user + "'";
                    }

                }
                else
                {
                    query += "UserName = '" + user + "'";
                }

            }
            dv.RowFilter = query;
            
            //return dt;
            return dt = dv.ToTable();
        }

        protected void userCheck_Init(object sender, EventArgs e)
        {
            ASPxCheckBox cb = (ASPxCheckBox)sender;
            GridViewDataItemTemplateContainer container = (GridViewDataItemTemplateContainer)cb.NamingContainer;

            cb.ClientInstanceName = string.Format("cbCheck{0}", container.VisibleIndex);
            cb.Checked = UsersGridView.Selection.IsRowSelected(container.VisibleIndex);
            cb.ClientSideEvents.CheckedChanged =
                string.Format("function (s, e) {{ grid.SelectRowOnPage({0}, s.GetChecked()); }}", container.VisibleIndex);
        }

        private DataSet GetDataSet(string dsName)
        {
            return Ds = (DataSet)Session[dsName];
        }

        private string GetDataSourceName(ASPxGridView grid)
        {
            string name = String.Empty;
 
            if (grid.ID == "HistoryGV")
            {
                name = "HistoryDS";
            }
            else if (grid.ID == "CrGridView")
            {
                name = "CrDS";
            }
            else if (grid.ID == "SvGridView")
            {
                name = "SvDS";
            }
            else if( grid.ID == "UsersGridView")
            {
                name = "UsersDS";
            }


            return name;
        }

        private DataTable SetDataSource(List<ProjectInfo> proj, string dsName)
        {
            //if ((Session[dsName] == null) || isFilteredView)
            //{
                Ds = new DataSet();
                DataTable masterTable = new DataTable();
                masterTable.Columns.Add("ID", typeof(Int32));
                masterTable.Columns.Add("ObjectID", typeof(ObjectId));
                masterTable.Columns.Add("UserName", typeof(String));
                masterTable.Columns.Add("Customer", typeof(String));
                masterTable.Columns.Add("Project", typeof(String));
                masterTable.Columns.Add("ProjCode", typeof(String));
                masterTable.Columns.Add("PM", typeof(String));
                masterTable.Columns.Add("Date", typeof(DateTime));
                masterTable.Columns.Add("Cust Contact", typeof(String));
                masterTable.Columns.Add("Contact Email", typeof(String));
                masterTable.Columns.Add("Signature", typeof(String));
                masterTable.Columns.Add("Version", typeof(String));
                masterTable.Columns.Add("Type", typeof(String));
                masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };

                int count = 0;
                if (proj != null)
                {
                    for (int i = 0; i < proj.Count; i++)
                    {
                        masterTable.Rows.Add(new object[] { i, proj[i].ID, proj[i].UserName, proj[i].CustName, proj[i].ProjectName, proj[i].ProjectCode, proj[i].PmName, proj[i].Date.ToShortDateString(), proj[i].CustContact, proj[i].CustEmail, proj[i].CustSignature, proj[i].Version, proj[i].Type });
                        count++;
                    }
                }

                Ds.Tables.AddRange(new DataTable[] { masterTable });
                Session[dsName] = Ds;
                DataSetName = dsName;
            //}
            //else
            //    Ds = (DataSet)Session[dsName];

            return Ds.Tables[0];

        }

        private DataTable SetDataSource(List<CRObj> proj, string dsName)
        {
            //if ((Session[dsName] == null))
            //{
                CrDS = new DataSet();
                DataTable masterTable = new DataTable();
                masterTable.Columns.Add("ID", typeof(Int32));
                masterTable.Columns.Add("ObjectID", typeof(ObjectId));
                masterTable.Columns.Add("UserName", typeof(String));
                masterTable.Columns.Add("Customer", typeof(String));
                masterTable.Columns.Add("Project", typeof(String));
                masterTable.Columns.Add("ProjCode", typeof(String));
                masterTable.Columns.Add("PM", typeof(String));
                masterTable.Columns.Add("Date", typeof(DateTime));
                masterTable.Columns.Add("Cust Contact", typeof(String));
                masterTable.Columns.Add("Contact Email", typeof(String));
                masterTable.Columns.Add("Signature", typeof(String));
                masterTable.Columns.Add("Version", typeof(String));
                masterTable.Columns.Add("Type", typeof(String));

                masterTable.Columns.Add("Estimate", typeof(String));
                masterTable.Columns.Add("DeptMb", typeof(String));
                masterTable.Columns.Add("Time", typeof(String));
                masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };

                int count = 0;
                if (proj != null)
                {
                    for (int i = 0; i < proj.Count; i++)
                    {
                        masterTable.Rows.Add(new object[] { i, proj[i].ID, proj[i].UserName, proj[i].CustName, proj[i].ProjectName, proj[i].ProjectCode, proj[i].PmName, proj[i].Date.ToShortDateString(), proj[i].CustContact, proj[i].CustEmail, proj[i].CustSignature, proj[i].Version, proj[i].Type, proj[i].Estimates, proj[i].DeptMb, proj[i].Time });
                        count++;
                    }
                }

                CrDS.Tables.AddRange(new DataTable[] { masterTable });
                Session[dsName] = CrDS;
                DataSetName = dsName;
            //}
            //else
            //    CrDS = (DataSet)Session[dsName];

            return CrDS.Tables[0];

        }

        private DataTable SetDataSource(List<TimeSheetObj> proj, string dsName)
        {
            //if ((Session[dsName] == null))
            //{
                SvDS = new DataSet();
                DataTable masterTable = new DataTable();
                masterTable.Columns.Add("ID", typeof(Int32));
                masterTable.Columns.Add("ObjectID", typeof(ObjectId));
                masterTable.Columns.Add("UserName", typeof(String));
                masterTable.Columns.Add("Customer", typeof(String));
                masterTable.Columns.Add("Project", typeof(String));
                masterTable.Columns.Add("ProjCode", typeof(String));
                masterTable.Columns.Add("PM", typeof(String));
                masterTable.Columns.Add("Date", typeof(DateTime));
                masterTable.Columns.Add("Cust Contact", typeof(String));
                masterTable.Columns.Add("Contact Email", typeof(String));
                masterTable.Columns.Add("Signature", typeof(String));
                masterTable.Columns.Add("Version", typeof(String));
                masterTable.Columns.Add("Type", typeof(String));

                masterTable.Columns.Add("Deviation", typeof(String));
                masterTable.Columns.Add("CrRequired", typeof(String));
                masterTable.Columns.Add("TimeSpend", typeof(String));
                masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };
                int count = 0;
                if (proj != null)
                {
                    for (int i = 0; i < proj.Count; i++)
                    {
                        masterTable.Rows.Add(new object[] { i, proj[i].ID, proj[i].UserName, proj[i].CustName, proj[i].ProjectName, proj[i].ProjectCode, proj[i].PmName, proj[i].Date.ToShortDateString(), proj[i].CustContact, proj[i].CustEmail, proj[i].CustSignature, proj[i].Version, proj[i].Type, proj[i].Deviation, proj[i].CrRequired, proj[i].TimeSpent });
                        count++;
                    }
                }

                SvDS.Tables.AddRange(new DataTable[] { masterTable });
                Session[dsName] = SvDS;
                DataSetName = dsName;
            //}
            //else
            //    SvDS = (DataSet)Session[dsName];

            return SvDS.Tables[0];

        }

        protected void LoadDraftBtn_Click(object sender, EventArgs e)
        {
            ASPxGridView grid = GetCurrentGrid(true);
            isDraft = true;
            DataRowView x = grid.GetRow(grid.FocusedRowIndex) as DataRowView;
            DataRow row = x.Row;
            ProjId = ObjectId.Parse(row.ItemArray[1].ToString());
            isNewSelection = true;

            Response.Redirect("~/", false);
        }

        protected void LoadCr_Click(object sender, EventArgs e)
        {
            CrGridView.Visible = true;
            HistoryGV.Visible = false;
            SvGridView.Visible = false;
            isFilteredView = false;
            DataTable dt = SetDataSource(MongoDb.GetCRinfo(), "CrDS");
            DataSetName = "CrDS";
            CrGridView.DataSource = dt;
            CrGridView.DataBind();

        }

        protected void LoadWs_Click(object sender, EventArgs e)
        {

            SvGridView.Visible = true;
            HistoryGV.Visible = false;
            CrGridView.Visible = false;
            isFilteredView = false;
            DataTable dt = SetDataSource(MongoDb.GetWorksheetinfo(), "SvDS");
            DataSetName = "SvDS";
            SvGridView.DataSource = dt;
            SvGridView.DataBind();

        }

        protected void LoadAll_Click(object sender, EventArgs e)
        {
            HistoryGV.Visible = true;
            CrGridView.Visible = false;
            SvGridView.Visible = false;
            isFilteredView = false;
            DataTable dt = SetDataSource(MongoDb.GetProjectInfo(), "HistoryDS");
            DataSetName = "HistoryDS";
            HistoryGV.DataSource = dt;
            HistoryGV.DataBind();
        }

        private void ExportSiteVisitToPDF(ASPxGridView grid)
        {
            DataRowView x = grid.GetRow(grid.FocusedRowIndex) as DataRowView;
            DataRow row = x.Row;

            var id = ObjectId.Parse(row.ItemArray[1].ToString());

            BsonDocument doc = MongoDb.GetDoc(id).Result[0];
            var proj = doc["ProjectInfo"];
            var timesheet = doc["TimeSheet"];
            ProjectInfo projInfo = new ProjectInfo();
            TimeSheetObj tm = new TimeSheetObj();

            projInfo.CustCode = proj["CustCode"].ToString();
            projInfo.CustContact = proj["CustContact"].ToString();
            projInfo.CustEmail = proj["CustEmail"].ToString();
            projInfo.CustName = proj["CustName"].ToString();
            projInfo.CustSignature = proj["Signature"].ToString();
            projInfo.Date = DateTime.Parse(proj["Date"].ToString());
            projInfo.PmEmail = proj["PmEmail"].ToString();
            projInfo.PmName = proj["PmName"].ToString();
            projInfo.ProjectCode = proj["ProjCode"].ToString();
            projInfo.ProjectName = proj["ProjName"].ToString();
            // TimeSheet/Sitvisit information
            tm.Deviation = timesheet["Deviation"].ToString();
            tm.Outcome = MongoDb.ConvertBsonArrayToStringArray(timesheet["Outcome"].AsBsonArray);
            tm.Agenda = MongoDb.ConvertBsonArrayToStringArray(timesheet["Agenda"].AsBsonArray);
            tm.CrRequired = timesheet["CrRequired"].ToString();
            tm.TimeSpent = timesheet["TimeSpend"].ToString();


            string html = Common.GetSiteVistHtmlString(projInfo, tm, false);
            ConvertToPDF(html, projInfo.CustSignature, false, true);
        }

        private void ExportCrToPDF(ASPxGridView grid)
        {

            DataRowView x = grid.GetRow(grid.FocusedRowIndex) as DataRowView;
            DataRow row = x.Row;

            var id = ObjectId.Parse(row.ItemArray[1].ToString());

            BsonDocument doc = MongoDb.GetDoc(id).Result[0];
            var proj = doc["ProjectInfo"];
            var cr = doc["ChangeRequest"];
            ProjectInfo projInfo = new ProjectInfo();
            CRObj newCr = new CRObj();

            projInfo.CustCode = proj["CustCode"].ToString();
            projInfo.CustContact = proj["CustContact"].ToString();
            projInfo.CustEmail = proj["CustEmail"].ToString();
            projInfo.CustName = proj["CustName"].ToString();
            projInfo.CustSignature = proj["Signature"].ToString();
            projInfo.Date = DateTime.Parse(proj["Date"].ToString());
            projInfo.PmEmail = proj["PmEmail"].ToString();
            projInfo.PmName = proj["PmName"].ToString();
            projInfo.ProjectCode = proj["ProjCode"].ToString();
            projInfo.ProjectName = proj["ProjName"].ToString();
            // CR information
            newCr.DeptMb = cr["DeptMb"].ToString();
            newCr.Time = cr["Time"].ToString();
            newCr.Estimates = cr["Estimate"].ToString();
            newCr.Reasons = MongoDb.ConvertBsonArrayToStringArray(cr["Reason"].AsBsonArray);
            newCr.CustAuth = cr["Authorisation"].ToString();


            string html = Common.GetCRHtmlString(projInfo, newCr);
            ConvertToPDF(html, projInfo.CustSignature, true, false);
        }

        public void ConvertToPDF(string url, string imgStr, bool isCR, bool isSiteVisit)
        {
            HtmlToPdf converter = new HtmlToPdf();
            var img = Common.LoadImage(imgStr);
            string subfoldername = "Content\\Images";
            string filename = "MB_Logo_Full_Colour_Black_PDF_Header.jpg";

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfoldername, filename);
            PdfImageSection headImg = new PdfImageSection(20, 0, 200, path);
            if (img != null)
            {
                PdfImageSection footImg = new PdfImageSection(0, 0, 0, img);

                // footer settings
                converter.Options.DisplayFooter = true;
                converter.Footer.Height = 150;
                converter.Footer.Add(footImg);
                //converter.Footer.
                //header settings
                converter.Options.DisplayHeader = true;
                converter.Header.Height = 100;
                converter.Header.Add(headImg);

            }
            PdfDocument pdf = converter.ConvertHtmlString(url);
            // MemStream = new MemoryStream();
            //PdfDocument pdf = converter.ConvertUrl(url);

            if (isSiteVisit & isCR)
            {
                pdf.Save(Server.MapPath(@"~/Files/Site_Visit_&_Change_Request.pdf"));
                path = "/Files/Site_Visit_&_Change_Request.pdf";
                //filename = "Site_Visit_&_Change_Request.pdf";
                filename = Server.MapPath(@"~/Files/Site_Visit_&_Change_Request.pdf");
            }
            else if (isSiteVisit)
            {
                pdf.Save(Server.MapPath(@"~/Files/Site_Visit.pdf"));
                path = "/Files/Site_Visit.pdf";
                //filename = "Site_Visit.pdf";
                filename = Server.MapPath(@"~/Files/Site_Visit.pdf");
            }
            else if (isCR)
            {
                pdf.Save(Server.MapPath(@"~/Files/ChangeRequest.pdf"));
                path = "/Files/ChangeRequest.pdf";
                //filename = "ChangeRequest.pdf";
                filename = Server.MapPath(@"~/Files/ChangeRequest.pdf");
            }

            pdf.Close();
            pdf.DetachStream();

        }

        protected void ExportToPdf_Click(object sender, EventArgs e)
        {
            ASPxGridView grid = GetCurrentGrid(true);
            DataTable dt = new DataTable();
            if (grid.DataSource.GetType() == typeof(DataTable))
            {
                dt = (DataTable)grid.DataSource;
            }
            else
            {
                DataSet ds = (DataSet)grid.DataSource;
                dt = ds.Tables[0];
            }
            

            int rowIndex = grid.FocusedRowIndex;

            DataRowView x = grid.GetRow(rowIndex) as DataRowView;
            DataRow row = x.Row;
            GridViewColumnCollection cols = grid.Columns as GridViewColumnCollection;
            GridViewColumn col = cols["Type"];

            int index = cols.IndexOf(col);
            var type = x.Row.ItemArray[index];
            if (type.ToString() == "CR")
            {
                ExportCrToPDF(grid);
                Response.Redirect("~/Files/ChangeRequest.pdf");
            }
            else
            {
                ExportSiteVisitToPDF(grid);
                Response.Redirect("~/Files/Site_Visit.pdf");
            }
        }

        private ASPxGridView GetCurrentGrid(bool Bind)
        {
            ASPxGridView grid = new ASPxGridView();

            if (CrGridView.Visible == true)
            {
                grid = CrGridView;
            }
            else if (SvGridView.Visible == true)
            {
                grid = SvGridView;
            }
            else
            {
                grid = HistoryGV;
            }

            if (Bind)
            {
                BindGrid();
            }

            return grid;
        }

        protected void DeleteBtn_Click(object sender, EventArgs e)
        {
            ASPxGridView grid = GetCurrentGrid(true);
           
            DataRowView x = grid.GetRow(grid.FocusedRowIndex) as DataRowView;
            DataRow row = x.Row;

            var id = ObjectId.Parse(row.ItemArray[1].ToString());
            if (row.ItemArray[11].ToString() != "Complete")
            {
                MongoDb.DeleteEntry(id);
                DataSetName = null;
            }
            else
            {
                string script = "<script>alert('Cannot Delete Completed Version');</script>";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            }

        }

    }
}