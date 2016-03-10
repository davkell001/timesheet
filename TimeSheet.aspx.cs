using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using SelectPdf;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using FileHelpers;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Excel;
using System.ComponentModel;
using DevExpress.Data;
using System.Drawing;
using DevExpress.Web;
using DevExpress.Web.Data;
using System.Collections;
using System.Net.Mime;
using System.Data.OleDb;

namespace MBTimeSheetWebApp
{
    public partial class TimeSheet : System.Web.UI.Page
    {

        [Browsable(true), Category("Action")]
        public event EventHandler SaveButtonClick;
        public event EventHandler ClearButtonClick;
        private DataSet Ds = null;
        private DataSet ChecklistDS = null;
        private DataSet AgendaDS = null;
        private DataSet ReasonDS = null;
        private DataSet OutcomeDS = null;
        private static bool isSaved;
        private static bool isWorkSheet = false;
        private static bool isCr = false;
        private static bool isNewProject = false;
        private static bool isChecklist = false;
        private static ObjectId objId { get; set; }

        const string WORKSHEETSTR = "Worksheet";
        const string CHECKLISTSTR = "Checklist";
        const string CRSTR = "CR";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
            {
                TabName.Value = Request.Form[TabName.UniqueID];
            }

            if (History.isNewSelection)
            {
                //if new project loaded clear all previous data and set new selection to false
                ClearData();
                History.isNewSelection = false;
                 if (!History.isDraft)
                 {
                     AddProjectInfo(GetMongoData(History.ProjId));
                     isNewProject = true;
                 }
                 else
                 {
                     var data = GetMongoData(History.ProjId);
                     AddProjectInfo(data);
                     GetDraftData(data);
                     isNewProject = false;
                 }
            }

            BindGrid();
           // DateTxt.Text = DateTime.Now.ToShortDateString();
        }


        private void BindGrid()
        {
            if ((DataSet)Session["AgendaDS"] != null)
            {
                AgendaDS = (DataSet)Session["AgendaDS"];
                AgendaGrid.DataSource = AgendaDS;
                AgendaGrid.DataBind();
            }
            else
            {
                AgendaGrid.DataSource = SetDataSource(null, "AgendaDS");
                AgendaGrid.DataBind(); 
            }

            if ((DataSet)Session["OutcomeDS"] != null)
            {
                OutcomeDS = (DataSet)Session["OutcomeDS"];
                outcomeGrid.DataSource = OutcomeDS;
                outcomeGrid.DataBind();
            }

            else
            {
                outcomeGrid.DataSource = SetDataSource(null, "OutcomeDS"); ;
                outcomeGrid.DataBind();
            }
            if ((DataSet)Session["ReasonDS"] != null)
            {
                ReasonDS = (DataSet)Session["ReasonDS"];
                reasonGrid.DataSource = ReasonDS;
                reasonGrid.DataBind();
            }
            else
            {
                reasonGrid.DataSource = SetDataSource(null, "ReasonDS"); ;
                reasonGrid.DataBind();
            }
            if ((DataSet)Session["ChecklistDS"] != null)
            {
                ChecklistDS = (DataSet)Session["ChecklistDS"];
                ChecklistGV.DataSource = ChecklistDS;
                ChecklistGV.DataBind();

            }
        }

        protected void SaveDraft_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(SaveDraft)); 
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(Save));
        }

        private async Task Save()
        {
            if (contactEmail.Text != String.Empty || crContactEmail.Text != String.Empty)
            {
                List<object> list = GetInputValues();

                var proj = list.OfType<ProjectInfo>();
                var tab1 = list.OfType<TimeSheetObj>();
                var tab2 = list.OfType<CRObj>();
                var tab3 = list.OfType<ChecklistObj>();

                await MongoDb.InsertDoc(proj.ElementAt(0), tab1.ElementAt(0), tab2.ElementAt(0), tab3.ToList(), isWorkSheet, isCr, isChecklist, isNewProject);
                //await MongoDb.InsertDoc(proj.ElementAt(0), tab1.ElementAt(0), tab2.ElementAt(0), isWorkSheet, isCr, isChecklist, tab3.ToList());

                isSaved = true;
                string toAddrs = String.Empty;

                if (crContactEmail.Text == String.Empty)
                {
                    toAddrs = contactEmail.Text;
                }
                else
                {
                    toAddrs = crContactEmail.Text;
                }

                if (isWorkSheet && isCr)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Common.GetSiteVistHtmlString(proj.ElementAt(0), tab1.ElementAt(0), true));
                    sb.Append(Common.GetCRHtmlString(proj.ElementAt(0), tab2.ElementAt(0)));
                    ConvertToPDF(sb.ToString(), proj.ElementAt(0).CustSignature, true, true);
                    SendEmail(toAddrs, PmEmail.Text, Server.MapPath(@"~/Files/Site_Visit_&_Change_Request.pdf"));
                }
                else if (isCr)
                {
                    ConvertToPDF(Common.GetCRHtmlString(proj.ElementAt(0), tab2.ElementAt(0)), proj.ElementAt(0).CustSignature, true, false);

                    SendEmail(toAddrs, PmEmail.Text, Server.MapPath(@"~/Files/ChangeRequest.pdf"));
                }
                else
                {
                    ConvertToPDF(Common.GetSiteVistHtmlString(proj.ElementAt(0), tab1.ElementAt(0), false), proj.ElementAt(0).CustSignature, false, true);
                    SendEmail(toAddrs, PmEmail.Text, Server.MapPath(@"~/Files/Site_Visit.pdf"));
                }
                //  ExportToXls();

                string script = "alert('Data Saved and Email Sent');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                ClearData();
                History.ProjId = ObjectId.Empty;
            }
            else
            {
                string script = "alert('Please enter a contact email address before emailing to customer');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            }
            
        }

        private async Task SaveDraft()
        {
            List<object> list = GetInputValues();

            var proj = list.OfType<ProjectInfo>();
            var tab1 = list.OfType<TimeSheetObj>();
            var tab2 = list.OfType<CRObj>();
            var tab3 = (List<ChecklistObj>)list[3];

            await MongoDb.InsertDoc(proj.ElementAt(0), tab1.ElementAt(0), tab2.ElementAt(0), tab3.ToList(), isWorkSheet, isCr, isChecklist, isNewProject);

            isSaved = true;

            string script = "alert('Data Saved');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            ClearData();
            History.ProjId = ObjectId.Empty;
        }

        protected void ClearButton_Click(object sender, EventArgs e)
        {
            ClearData();
            History.ProjId = ObjectId.Empty;
        }

        protected void SaveSigBtn_Click(object sender, EventArgs e)
        {
            Save(e);
        }

        protected void Save(EventArgs e)
        {
            EventHandler handler = this.SaveButtonClick;
            if (handler != null) { handler(this, e); }
        }

        private void SendEmail(string toAddress, string pmAddress, string filename)
        {

            System.Net.NetworkCredential cred = new System.Net.NetworkCredential();
            MailMessage mail = new MailMessage();
            ContentType ct = new ContentType(MediaTypeNames.Application.Pdf);
  
            //create mail
            Attachment attach = new Attachment(filename);

            string[] names = filename.Split('\\');
            string fileName = names[names.Length - 1].Replace(".pdf", " ");
            DateTime date = new DateTime();
            String dateStr = String.Empty;

            if (DateTime.TryParse(DateTxt.Text, out date))
            {
                dateStr = (date.Day + "-" + date.Month + "-" + date.Year);
            }
            else
            {
                date = DateTime.Now;
                dateStr = (date.Day + "-" + date.Month + "-" + date.Year);
            }
             
            attach.Name = fileName + CustName.Text + " " + dateStr + ".pdf";
            mail.Attachments.Add(attach);

            MailAddress pm = new MailAddress(pmAddress);
            MailAddress consultant = new MailAddress(HttpContext.Current.User.Identity.Name);
            MailAddress to = new MailAddress(toAddress);
            MailAddress from = new MailAddress("accounts@milnerbrowne.com");
            mail.CC.Add(pm);
            mail.CC.Add(consultant);
            mail.To.Add(to); 
            string cntactName = contactName.Text != null ? contactName.Text : crContactName.Text;
            mail.Subject = "Milner Browne: Site Visit - " + CustName.Text + " " + dateStr;
            mail.Body = "Hi " + cntactName + "," + System.Environment.NewLine + System.Environment.NewLine + "Please see file attached." + System.Environment.NewLine + System.Environment.NewLine + "For any queries regarding this project please contact your project manager." + System.Environment.NewLine + System.Environment.NewLine + "Kind Regards," + System.Environment.NewLine + "Milner Browne";
            mail.From = from;

            //create credentials
            cred.UserName = "c0ee54e35cf689c875fce64400d3b539";
            cred.Password = "28a8e2facfda8dd4f09f555feaba30bc";


            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    //create the client
                    client.Host = "in.mailjet.com";
                    //client.Host = "in-v3.mailjet.com";
                    client.Port = 25;
                    client.EnableSsl = true;
                    client.Credentials = cred;
                    client.Send(mail);
                }

            }
            catch (SmtpException ex)
            {
                Console.WriteLine("Exception caught in SendEmail(): {0}",
                  ex.Message);
            }
            finally
            {
                mail.Attachments.Dispose();
                mail.Dispose();
            }
        }

        private string GetVersion(string type)
        {
            string version = String.Empty;

            if (type == "Worksheet")
            {
                if (hdfSignatureDataBitmap1.Value == String.Empty || PmEmail.Text == String.Empty || contactEmail.Text == String.Empty)
                {
                    version = "Draft";
                }
                else
                {
                    version = "Complete";
                }
            }
            else if (type == "CR")
            {
                if (estimateDropDown.SelectedItem.Text == "No" || custAuthSelect.SelectedValue == "No" || contactEmail.Text == String.Empty || crSigDataBitmap.Value == String.Empty)
                {
                    version = "Draft";
                }
                else
                {
                    version = "Complete";
                }
            }
            else if (type == "Both")
            {
                if (hdfSignatureDataBitmap1.Value == String.Empty || crSigDataBitmap.Value == String.Empty)
                {
                    version = "Draft";
                }
                else
                {
                    version = "Complete";
                }
            }

            return version;
        }

        private List<object> GetInputValues()
        {
            List<object> list = new List<object>();
            ContentPlaceHolder placeHolder = (ContentPlaceHolder)this.Master.FindControl("MainContent");
            string sign = String.Empty;
            bool crSign = false;
            bool wsSign = false;
            TimeSheetObj tab1 = new TimeSheetObj();
            CRObj tab2 = new CRObj();
            DataTable agTable;
            DataTable oTable;
            DataTable rtable;
            string version = "";

            //if documents signed, get the signatures
            if (hdfSignatureDataBitmap1.Value != String.Empty && crSigDataBitmap.Value != String.Empty)
            {
                sign = hdfSignatureDataBitmap1.Value;
                wsSign = true;
                crSign = true;
            }
            else if (hdfSignatureDataBitmap1.Value != String.Empty)
            {
                sign = hdfSignatureDataBitmap1.Value;
                wsSign = true;
            }
            else if (crSigDataBitmap.Value != String.Empty)
            {
                sign = crSigDataBitmap.Value;
                crSign = true;
            }
            //check if document is a worksheet
            if (timeSpendTxt.Text != String.Empty || AgendaGrid.VisibleRowCount > 0)
            {
                isWorkSheet = true;
                if (AgendaGrid.DataSource.GetType() == typeof(DataTable))
                {
                    agTable = (DataTable)AgendaGrid.DataSource;
                    oTable = (DataTable)outcomeGrid.DataSource;
                }
                else
                {
                    DataSet agDs = (DataSet)AgendaGrid.DataSource;
                    agTable = agDs.Tables[0];
                    DataSet outDs = (DataSet)outcomeGrid.DataSource;
                    oTable = outDs.Tables[0];
                }

                tab1.Agenda = GetRowItems(agTable);
                tab1.Outcome = GetRowItems(oTable);
                tab1.TimeSpent = timeSpendTxt.Text;
                tab1.CrRequired = crSelect.SelectedItem.Text;
                tab1.Deviation = deviationList.SelectedItem.Text;
                tab1.IsSignedOff = wsSign;
                tab1.Version = GetVersion(WORKSHEETSTR);
            }

            //check if document is a change request
            if (timeTxt.Text != String.Empty || reasonGrid.VisibleRowCount > 0)
            {
                isCr = true;
                //tab2 - Change Request
                if (reasonGrid.DataSource.GetType() == typeof(DataTable))
                {
                    rtable = (DataTable)reasonGrid.DataSource;
                }
                else
                {
                    DataSet reasDs = (DataSet)reasonGrid.DataSource;
                    rtable = reasDs.Tables[0];
                }

                tab2.Reasons = GetRowItems(rtable);
                tab2.Time = timeTxt.Text;
                tab2.CustAuth = custAuthSelect.SelectedItem.Text;
                tab2.DeptMb = DeptDropDown.SelectedItem.ToString();
                tab2.Estimates = estimateDropDown.SelectedItem.Text;
                tab2.IsSignedOff = crSign;
                tab2.Version = GetVersion(CRSTR);

            }
            version = tab1.Version != null ? tab1.Version : tab2.Version;
            //tab 3 Checklist
            List<ChecklistObj> tab3List = new List<ChecklistObj>();
            if (ChecklistDS != null)
            {
                //Checklist and Worksheet have same criteria for version
                version = GetVersion(WORKSHEETSTR);
                foreach (DataRow row in ChecklistDS.Tables[0].Rows)
                {

                    tab3List.Add(new ChecklistObj()
                    {
                        ID = int.Parse(row.ItemArray[0].ToString()),
                        Area = row.ItemArray[1].ToString(),
                        Paragraph = row.ItemArray[2].ToString(),
                        Action = row.ItemArray[3].ToString(),
                        Priority = row.ItemArray[4].ToString(),
                        ClientAssign = row.ItemArray[5].ToString(),
                        MBAssign = row.ItemArray[6].ToString(),
                        Complete = row.ItemArray[7].ToString(),
                        DateFinished = row.ItemArray[8].ToString(),
                        Comments = row.ItemArray[9].ToString()

                    });

                }
            }

            //General Project information values
            ProjectInfo proj = new ProjectInfo()
            {
                ID = objId,
                ProjectName = ProjName.Text,
                ProjectCode = ProjCode.Text,
                CustName = CustName.Text,
                CustCode = CustCode.Text,
                PmName = PmName.Text,
                PmEmail = PmEmail.Text,
                Date = DateTime.Parse(DateTxt.Text),
                CustContact = contactName.Text != String.Empty ? contactName.Text : crContactName.Text,
                CustEmail = contactEmail.Text != String.Empty ? contactEmail.Text : crContactEmail.Text,
                CustSignature = sign,
                Version = version
            };


            list.Add(proj);
            list.Add(tab1);
            list.Add(tab2);
            list.Add(tab3List);

            return list;
        }

        private string[] GetRowItems(DataTable dt)
        {
            var rows = dt.Rows;
            string[] array = new string[rows.Count * 2];
            int count = 0;
            foreach (DataRow row in rows)
            {
                if (row.ItemArray[1].ToString() != String.Empty && row.ItemArray[2].ToString() != String.Empty)
                {
                    array[count] = row.ItemArray[1].ToString();
                    count++;
                    array[count] = row.ItemArray[2].ToString();
                    count++;
                }
            }

            return array;
        }

        private void AddProjectInfo(BsonDocument doc)
        {
            var proj = doc["ProjectInfo"];
            objId = doc["_id"].AsObjectId;
            ProjName.Text = proj["ProjName"].ToString();
            ProjCode.Text = proj["ProjCode"].ToString();
            PmName.Text = proj["PmName"].ToString();
            PmEmail.Text = proj["PmEmail"].ToString();
            DateTxt.Date = DateTime.Parse(proj["Date"].ToString());
            CustCode.Text = proj["CustCode"].ToString();
            CustName.Text = proj["CustName"].ToString();

        }

        private void GetDraftData(BsonDocument doc)
        {
            var proj = doc["ProjectInfo"];
            
            if (proj["Type"].ToString() == WORKSHEETSTR)
            {
                var tm = doc["TimeSheet"];
                AddTimeSheetFields(tm);
                if (tm["IsSigned"].AsBoolean == true)
                {
                    hdfSignatureDataBitmap1.Value = proj["Signature"].ToString();
                }
                contactEmail.Text = proj["CustEmail"].ToString();
                contactName.Text = proj["CustContact"].ToString();
            }
            else if (proj["Type"].ToString() == CRSTR)
            {
                var cr = doc["ChangeRequest"];
                AddCRFields(cr);
                if (cr["IsSigned"].AsBoolean == true)
                {
                    crSigDataBitmap.Value = proj["Signature"].ToString();
                }
                crContactEmail.Text = proj["CustEmail"].ToString();
                crContactName.Text = proj["CustContact"].ToString();
            }
            else
            {
                var checklist = doc["Checklist"];
                AddChecklistFields(checklist);
            }
        }

        private void AddTimeSheetFields(BsonValue tm)
        {

            timeSpendTxt.Text = tm["TimeSpend"].ToString();

            string[] agenda = MongoDb.ConvertBsonArrayToStringArray(tm["Agenda"].AsBsonArray);

            List<TableObj> agTables = new List<TableObj>();
            int count = 0;

            for (int i = 0; i < (agenda.Length / 2); i++)
            {

                TableObj agtable = new TableObj();
                agtable.Point = agenda[count];
                count++;
                agtable.Description = agenda[count];
                count++;
                agTables.Add(agtable);

            }

            AgendaGrid.DataSource = SetDataSource(agTables, "AgendaDS");
            AgendaGrid.DataBind();

            deviationList.SelectedIndex = tm["Deviation"].ToString() == "Yes" ? 0 : 1;
            crSelect.SelectedIndex = tm["CrRequired"].ToString() == "Yes" ? 0 : 1;

            string[] outcomes = MongoDb.ConvertBsonArrayToStringArray(tm["Outcome"].AsBsonArray);
            List<TableObj> outcomeTables = new List<TableObj>();
            int cellCount = 0;

            for (int i = 0; i < (outcomes.Length / 2); i++)
            {

                TableObj outcomtable = new TableObj();
                outcomtable.Point = outcomes[cellCount];
                cellCount++;
                outcomtable.Description = outcomes[cellCount];
                cellCount++;
                outcomeTables.Add(outcomtable);

            }
            outcomeGrid.DataSource = SetDataSource(outcomeTables, "OutcomeDS");
            outcomeGrid.DataBind();

        }

        private void AddCRFields(BsonValue cr)
        {
            estimateDropDown.SelectedIndex = cr["Estimate"].ToString() == "Yes" ? 0 : 1;
            DeptDropDown.SelectedItem.Text = cr["DeptMb"].ToString();
            timeTxt.Text = cr["Time"].ToString();
            custAuthSelect.SelectedIndex = cr["Authorisation"].ToString() == "Yes" ? 0 : 1;

            string[] reasons = MongoDb.ConvertBsonArrayToStringArray(cr["Reason"].AsBsonArray);
            List<TableObj> reasonTables = new List<TableObj>();
            int index = 0;

            for (int i = 0; i < (reasons.Length / 2); i++)
            {

                TableObj reasontable = new TableObj();
                reasontable.Point = reasons[index];
                index++;
                reasontable.Description = reasons[index];
                index++;
                reasonTables.Add(reasontable);

            }

            reasonGrid.DataSource = SetDataSource(reasonTables, "ReasonDS");
            reasonGrid.DataBind();

        }

        private void AddChecklistFields(BsonValue checklist)
        {
            var vals = checklist["Values"].AsBsonArray;
            List<ChecklistObj> ListChecklist = new List<ChecklistObj>();

            for (int i = 0; i < vals.Count; i++)
            {

                ListChecklist.Add(new ChecklistObj()
                {
                    ID = vals[i]["ID"].ToInt32(),
                    Area = vals[i]["Area"].ToString(),
                    Paragraph = vals[i]["Paragraph"].ToString(),
                    Action = vals[i]["Action"].ToString(),
                    Priority = vals[i]["Priority"].ToString(),
                    ClientAssign = vals[i]["ClientAssign"].ToString(),
                    MBAssign = vals[i]["MBAssign"].ToString(),
                    Complete = vals[i]["Complete"].ToString(),
                    DateFinished = vals[i]["DateFinished"].ToString(),
                    Comments = vals[i]["Comments"].ToString()

                });

            }
            DataTable dt = ConvertListToDataTable(ListChecklist);
            ChecklistDS = new DataSet();
            ChecklistDS.Tables.AddRange(new DataTable[] { dt });
            Session["ChecklistDS"] = ChecklistDS;
            ChecklistGV.DataSource = ChecklistDS;
            ChecklistGV.DataBind();
                    
        }

        private void ExportToXls()
        {
            try
            {
                var engine = new FileHelperEngine<ChecklistObj>();
                //var rows = GetChecklistVals();
                List<ChecklistObj> list = new List<ChecklistObj>();
                foreach (DataRow row in ChecklistDS.Tables[0].Rows)
                {
                    ChecklistObj vals = new ChecklistObj();
                    list.Add(new ChecklistObj()
                    {
                        ID = int.Parse(row.ItemArray[0].ToString()),
                        Area = row.ItemArray[1].ToString(),
                        Paragraph = row.ItemArray[2].ToString(),
                        Action = row.ItemArray[3].ToString(),
                        Priority = row.ItemArray[4].ToString(),
                        ClientAssign = row.ItemArray[5].ToString(),
                        MBAssign = row.ItemArray[6].ToString(),
                        Complete = row.ItemArray[7].ToString(),
                        DateFinished = row.ItemArray[8].ToString(),
                        Comments = row.ItemArray[9].ToString()

                    });

                }


                engine.WriteFile(Server.MapPath(@"~/Files/checklist.txt"), list);

                ConvertToXls();

            }
            catch (FileHelpersException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private List<ChecklistObj> ImportFromXls(string filename)
        {
            FileStream stream = File.Open(Server.MapPath(@filename), FileMode.Open, FileAccess.Read);
            List<ChecklistObj> list = new List<ChecklistObj>();
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                string[] cells;
                int rowCount = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    cells = line.Split(',');
                    //skip column header/first line
                    if (rowCount != 0)
                    {
                        list.Add(new ChecklistObj()
                        {
                            ID = rowCount,
                            Area = cells[0],
                            Paragraph = cells[1],
                            Action = cells[2],
                            Priority = cells[3],
                            ClientAssign = cells[4],
                            MBAssign = cells[5],
                            Complete = cells[6].ToString(),
                            DateFinished = cells[7],
                            Comments = cells[8]
                        });
                    }

                    rowCount++;
                }
                isChecklist = true;
                return list;
            }

        }

        private BsonDocument GetMongoData(ObjectId id)
        {
            BsonDocument doc = MongoDb.GetDoc(id).Result[0];
            return doc;

        }

        public void ConvertToXls()
        {
            string line;
            string[] rows = System.IO.File.ReadAllLines(Server.MapPath(@"~/Files/checklist.txt"));
            int rowCount = rows.Count();
            string[] lines = new string[rowCount + 1];
            int count = 1;

            //assign heading for excel file  
            string head = "id,Area,Paragraph,Action,Priority,Client Assign,MB Assign,Complete,Finish date,Comments";
            lines[0] = head;
            using (StreamReader read = new StreamReader(Server.MapPath(@"~/Files/checklist.txt")))
            {
                while ((line = read.ReadLine()) != null)
                {
                    lines[count] = line;
                    count++;
                }
                //using (StreamWriter writer = new StreamWriter(Server.MapPath(@"~/Files/checklist.xlsx")))
                //{
                //    foreach (string l in lines)
                //    {
                //        string row = l;

                //        //if (l != lines[0])
                //        //{
                //        //    row = l.Replace(" ", ",");
                //        //}

                //        writer.WriteLine(row + ",");
                //    }
                //}
                using (StreamWriter writer = new StreamWriter(Server.MapPath(@"~/Files/checklist.csv")))
                {
                    foreach (string l in lines)
                    {
                        string row = l;

                        //if (l != lines[0])
                        //{
                        //    row = l.Replace(" ", ",");
                        //}

                        writer.WriteLine(row + ",");
                    }
                }
            }
        }

        public void ClearData()
        {
            ProjName.Text = "";
            ProjCode.Text = "";
            PmName.Text = "";
            PmEmail.Text = "";
            CustCode.Text = "";
            CustName.Text = "";
            contactName.Text = "";
            contactEmail.Text = "";
            timeSpendTxt.Text = "";
            DeptDropDown.SelectedIndex = 0;
            timeTxt.Text = "";
            custAuthSelect.SelectedIndex = 1;
            deviationList.SelectedIndex = 1;
            crSelect.SelectedIndex = 1;
            crContactEmail.Text = "";
            crContactName.Text = "";


            AgendaGrid.DataSource = SetEmptyDS("AgendaDS");
            //Session["AgendaDS"] = null;
            AgendaGrid.DataBind();
            outcomeGrid.DataSource = SetEmptyDS("OutcomeDS"); ;
            //Session["OutcomeDS"] = null;
            outcomeGrid.DataBind();
            reasonGrid.DataSource = SetEmptyDS("ReasonDS"); ;
            //Session["ReasonDS"] = null;
            reasonGrid.DataBind();
            ChecklistGV.DataSource = SetChecklistDS();
            ChecklistGV.DataBind();
            hdfSignatureDataBitmap1.Value = "";
            crSigDataBitmap.Value = "";
            isWorkSheet = false;
            isCr = false;
            isChecklist = false;

        }


        public void ConvertToPDF(string url, string imgStr, bool isCR, bool isSiteVisit)
        {
            HtmlToPdf converter = new HtmlToPdf();
            if (imgStr == String.Empty)
            {
                imgStr = "data:image/png;base64,R0lGODlhAQABAAAAACH5BAEKAAEALAAAAAABAAEAAAICTAEAOw==";
            }
            var img = Common.LoadImage(imgStr);
            string subfoldername = "Content\\Images";
            //string filename = "MB_Logo_Full_Colour_Black_PDF_Header.jpg";
            string filename = "High_Quality_Logo.png";
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfoldername, filename);
            System.Drawing.Image headImage = Common.LoadImageFromFile(path);
            //PdfImageSection headImg = new PdfImageSection(20, 0, 200, path);
            PdfImageSection headImg = new PdfImageSection(20, 0, 200, headImage);
            if (img != null)
            {
                PdfImageSection footImg = new PdfImageSection(0, 0, 0, img);
                converter.Footer.Add(footImg);
            }
            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.Height = 150;
            //header settings
            converter.Options.DisplayHeader = true;
            converter.Header.Height = 100;
            converter.Options.MarginTop = 70;
            if (headImg != null)
            {
                converter.Header.Add(headImg);
            }


            PdfDocument pdf = converter.ConvertHtmlString(url);
            int count = pdf.Pages.Count;

            if (count % 2 == 0)
            {
                pdf.Footer.DisplayOnEvenPages = true;
                pdf.Footer.DisplayOnFirstPage = false;
            }
            else
            {
                pdf.Footer.DisplayOnFirstPage = true;
                if (count > 2)
                {
                    pdf.Footer.DisplayOnFirstPage = false;
                    pdf.Footer.DisplayOnOddPages = true;
                }

            }
            // MemStream = new MemoryStream();
            //PdfDocument pdf = converter.ConvertUrl(url);

            if (isSiteVisit & isCR)
            {
                pdf.Save(Server.MapPath(@"~/Files/Site_Visit_&_Change_Request.pdf"));
            }
            else if (isSiteVisit)
            {
                pdf.Save(Server.MapPath(@"~/Files/Site_Visit.pdf"));
            }
            else if (isCR)
            {
                pdf.Save(Server.MapPath(@"~/Files/ChangeRequest.pdf"));
            }

            pdf.Close();
            pdf.DetachStream();

        }

        protected void RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            DataSet ds = (DataSet)Session["AgendaDS"];
            Update(ds, "AgendaDS", sender, e);
        }

        protected void AgendaGrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            AgendaDS = (DataSet)Session["AgendaDS"];
            Insert(AgendaDS, "AgendaDS", sender, e);
            Insert(OutcomeDS, "OutcomeDS", outcomeGrid, e);
            //ASPxCallbackPanel1_Callback(sender, new CallbackEventArgs(""));
        }

        private void CopyDataTable(string DataSetName, string copyToDataSetName, ASPxGridView grid)
        {
            Ds = (DataSet)Session[DataSetName];
            DataSet dsCopy = (DataSet)Session[copyToDataSetName];
          

            //DataSet dsCopy = Ds.Copy();
            //DataTable dt = dsCopy.Tables[0];
            //Session[copyToDataSetName] = dsCopy;
            //grid.DataSource = null;
            //grid.DataSource = dsCopy;
            //grid.DataBind();
        }

        private int GetNewId(string DataSetName)
        {
            Ds = (DataSet)Session[DataSetName];
            DataTable table = Ds.Tables[0];
            if (table.Rows.Count == 0) return 0;
            int max = Convert.ToInt32(table.Rows[0]["ID"]);
            for (int i = 1; i < table.Rows.Count; i++)
            {
                if (Convert.ToInt32(table.Rows[i]["ID"]) > max)
                    max = Convert.ToInt32(table.Rows[i]["ID"]);
            }
            return max + 1;
        }

        private DataTable SetEmptyDS(string DataSetName)
        {
            Ds = new DataSet();
            DataTable masterTable = new DataTable();
            masterTable.Columns.Add("ID", typeof(Int32));
            masterTable.Columns.Add("Point", typeof(string));
            masterTable.Columns.Add("Description", typeof(string));
            masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };
            Ds.Tables.AddRange(new DataTable[] { masterTable });
            Session[DataSetName] = Ds;

            return Ds.Tables[0];
        }
        private DataTable SetDataSource(List<TableObj> table, string DataSetName)
        {
            if (!IsPostBack || (Session[DataSetName] == null))
            {
                Ds = new DataSet();
                DataTable masterTable = new DataTable();
                masterTable.Columns.Add("ID", typeof(Int32));
                masterTable.Columns.Add("Point", typeof(string));
                masterTable.Columns.Add("Description", typeof(string));
                masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };
                int count = 0;
                if (table != null)
                {
                    for (int i = 0; i < table.Count; i++)
                    {
                        if (table[i].Point != String.Empty && table[i].Description != String.Empty)
                        {
                            masterTable.Rows.Add(new object[] { i, table[i].Point, table[i].Description });
                            count++;
                        }

                    }
                }

                //masterTable.Rows.Add(new object[] { count, "", "" });
                Ds.Tables.AddRange(new DataTable[] { masterTable });
                Session[DataSetName] = Ds;
            }
            else
                Ds = (DataSet)Session[DataSetName];

            return Ds.Tables[0];

        }

        private DataTable SetChecklistDS()
        {
            string DataSetName = "ChecklistDS";
            if (!IsPostBack || (Session[DataSetName] == null))
            { 
                Ds = new DataSet();
                DataTable masterTable = new DataTable();
                masterTable.Columns.Add("ID", typeof(Int32));
                masterTable.Columns.Add("Area", typeof(string));
                masterTable.Columns.Add("Paragraph", typeof(string));
                masterTable.Columns.Add("Action", typeof(string));
                masterTable.Columns.Add("Priority", typeof(string));
                masterTable.Columns.Add("ClientAssign", typeof(string));
                masterTable.Columns.Add("MBAssign", typeof(string));
                masterTable.Columns.Add("Complete", typeof(string));
                masterTable.Columns.Add("DateFinished", typeof(string));
                masterTable.Columns.Add("Comments", typeof(string));
                masterTable.PrimaryKey = new DataColumn[] { masterTable.Columns["ID"] };
       
                Ds.Tables.AddRange(new DataTable[] { masterTable });
                Session[DataSetName] = Ds;
            }
            else
                Ds = (DataSet)Session[DataSetName];

            return Ds.Tables[0];

        }

        protected void outcomeGrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            Ds = (DataSet)Session["OutcomeDS"];
            Update(Ds, "OutcomeDS", sender, e);
        }

        protected void outcomeGrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            Ds = (DataSet)Session["OutcomeDS"];
            Insert(Ds, "OutcomeDS", sender, e);
        }

        private void Update(DataSet ds, string dsName, object sender, ASPxDataUpdatingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable table = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];

            DataRow row = table.Rows.Find(e.Keys[0]);
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                row[enumerator.Key.ToString()] = enumerator.Value;
            }
            gridView.CancelEdit();
            e.Cancel = true;
            gridView.DataSource = table;
            gridView.DataBind();
        }

        private void Insert(DataSet ds, string dsname, object sender, ASPxDataInsertingEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            e.NewValues["ID"] = GetNewId(dsname);
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            dataTable.Rows.Add(row);
            gridView.DataSource = dataTable;
            gridView.DataBind();
        }

        private void Insert(DataSet ds, string dsname, ASPxGridView gridView, ASPxDataInsertingEventArgs e)
        {
            //ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            e.NewValues["ID"] = GetNewId(dsname);
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            e.Cancel = true;
            dataTable.Rows.Add(row);
            gridView.DataSource = dataTable;
            gridView.DataBind();
        }

        protected void reasonGrid_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            Ds = (DataSet)Session["ReasonDS"];
            Insert(Ds, "ReasonDS", sender, e);
        }

        protected void reasonGrid_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            Ds = (DataSet)Session["ReasonDS"];
            Update(Ds, "ReasonDS", sender, e);
        }

        protected void reasonGrid_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            e.ExceptionHandled = true;
            Ds = (DataSet)Session["ReasonDS"];
            Ds.Tables[0].Rows.Remove(Ds.Tables[0].Rows.Find(e.Keys[reasonGrid.KeyFieldName]));
            gridView.DataSource = Ds;
            gridView.DataBind();
        }

        protected void outcomeGrid_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            e.ExceptionHandled = true;
            Ds = (DataSet)Session["OutcomeDS"];
            Ds.Tables[0].Rows.Remove(Ds.Tables[0].Rows.Find(e.Keys[outcomeGrid.KeyFieldName]));
            gridView.DataSource = Ds;
            gridView.DataBind();
        }

        protected void AgendaGrid_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            e.ExceptionHandled = true;
            Ds = (DataSet)Session["AgendaDS"];
            Ds.Tables[0].Rows.Remove(Ds.Tables[0].Rows.Find(e.Keys[AgendaGrid.KeyFieldName]));
            gridView.DataSource = Ds;
            gridView.DataBind();
        }

        protected void AgendaGrid_RowInserted(object sender, ASPxDataInsertedEventArgs e)
        {
            AgendaDS = (DataSet)Session["AgendaDS"];
            Insert(AgendaDS, "AgendaDS", sender, e);

            ReasonDS = (DataSet)Session["ReasonDS"];
            Insert(ReasonDS, "ReasonDS", sender, e);
            reasonGrid.DataSource = ReasonDS;
            reasonGrid.DataBind();
        }

        protected void ASPxCallbackPanel1_Callback(object sender, CallbackEventArgsBase e)
        {
            //CopyDataTable("AgendaDS", "OutcomeDS", outcomeGrid);
            
        }

        protected void UploadBtn_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                try
                {
                    ChecklistDS = new DataSet();
                    string filename = Path.GetFileName(FileUpload.FileName);
                    string extension = Path.GetExtension(FileUpload.PostedFile.FileName);
                    FileUpload.SaveAs(Server.MapPath("~/Files/") + filename);
                    StatusLbl.Visible = true;
                    StatusLbl.Text = "Upload status: File uploaded!";

                    DataTable dt = ConvertListToDataTable(ImportFromXls("~/Files/" + filename));
                    ChecklistDS.Tables.AddRange(new DataTable[] { dt });
                    Session["ChecklistDS"] = ChecklistDS;
                    ChecklistGV.DataSource = ChecklistDS.Tables[0];
                    ChecklistGV.DataBind();

                }
                catch (Exception ex)
                {
                    StatusLbl.Visible = true;
                    StatusLbl.ForeColor = Color.Red;
                    StatusLbl.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }

        protected void ChecklistGV_RowInserting(object sender, ASPxDataInsertingEventArgs e)
        {
            ChecklistDS = (DataSet)Session["ChecklistDS"];
            Insert(ChecklistDS, "ChecklistDS", sender, e);
            isChecklist = true;
            ChecklistGV.DataSource = ChecklistDS;
            ChecklistGV.DataBind();
        }

        protected void ChecklistGV_RowUpdating(object sender, ASPxDataUpdatingEventArgs e)
        {
            ChecklistDS = (DataSet)Session["ChecklistDS"];
            Update(ChecklistDS, "ChecklistDS", sender, e);
            isChecklist = true;
            ChecklistGV.DataSource = ChecklistDS;
            ChecklistGV.DataBind();
        }

        static DataTable ConvertListToDataTable(List<ChecklistObj> list)
        {
            // New table.
            DataTable table = new DataTable();

            // Get max columns.
            int rows = list.Count;

            // Add columns.
            foreach (var prop in typeof(ChecklistObj).GetProperties())
            {
                table.Columns.Add(new DataColumn() { ColumnName = prop.Name });
            }

            // Add rows.
            foreach (var item in list)
            {
                table.Rows.Add(new object[] { item.ID, item.Area, item.Paragraph, item.Action, item.Priority, item.ClientAssign, item.MBAssign, item.Complete, item.DateFinished, item.Comments });
                table.PrimaryKey = new DataColumn[] { table.Columns["ID"] };
            }

            return table;
        }

        protected void ChecklistGV_RowDeleted(object sender, ASPxDataDeletedEventArgs e)
        {
            e.ExceptionHandled = true;
            ChecklistDS = (DataSet)Session["ChecklistDS"];          
            ChecklistDS.Tables[0].Rows.Remove(ChecklistDS.Tables[0].Rows.Find(e.Keys[ChecklistGV.KeyFieldName]));
            isChecklist = true;
           
            ChecklistGV.DataSource = ChecklistDS;
            ChecklistGV.DataBind();
            
        }



        private void Insert(DataSet ds, string dsname, object sender, ASPxDataInsertedEventArgs e)
        {
            ASPxGridView gridView = (ASPxGridView)sender;
            DataTable dataTable = gridView.GetMasterRowKeyValue() != null ? ds.Tables[1] : ds.Tables[0];
            DataRow row = dataTable.NewRow();
            e.NewValues["ID"] = GetNewId(dsname);
            IDictionaryEnumerator enumerator = e.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
                if (enumerator.Key.ToString() != "Count")
                    row[enumerator.Key.ToString()] = enumerator.Value;
            gridView.CancelEdit();
            //e.Cancel = true;
            dataTable.Rows.Add(row);
            gridView.DataSource = dataTable;
            gridView.DataBind();
        }

        protected void ExportBtn_Click(object sender, EventArgs e)
        {
            ExportToXls();
            Response.Redirect(@"~/Files/checklist.csv");
        }

        protected void cb_Init(object sender, EventArgs e)
        {
            //if (IsPostBack)
            //{
            //    GridViewDataItemTemplateContainer c = ((ASPxCheckBox)sender).NamingContainer as GridViewDataItemTemplateContainer;
            //    int index = c.VisibleIndex;
            //    int test = c.ItemIndex;
            //    string key = c.KeyValue.ToString();
            //    string val = 
            //    string ownerGridClientInstanceName = c.Grid.ClientInstanceName;

            //    ChecklistDS = (DataSet)Session["ChecklistDS"];

            //    DataTable table = ChecklistGV.GetMasterRowKeyValue() != null ? ChecklistDS.Tables[1] : ChecklistDS.Tables[0];

            //    DataRow row = table.Rows[index];
            //    row[index] = key;

            //    ChecklistGV.CancelEdit();

            //}

        }

        public bool GetChecked(string isComplete)
        {
            switch (isComplete)
            {
                case "Complete":
                    return true;
                case "Yes":
                    return true;
                case "True":
                    return true;
                case "No":
                    return false;
                case "False":
                    return false;
                case " ":
                    return false;
                default:
                    return false;
            }
        }

        protected void cb_CheckedChanged(object sender, EventArgs e)
        {
            ASPxDataUpdatingEventArgs args = (ASPxDataUpdatingEventArgs)e;
            ASPxCheckBox cb = (ASPxCheckBox)sender;
            ChecklistDS = (DataSet)Session["ChecklistDS"];
            isChecklist = true;

            DataTable table = ChecklistGV.GetMasterRowFieldValues() != null ? ChecklistDS.Tables[1] : ChecklistDS.Tables[0];

            DataRow row = table.Rows.Find(args.Keys[0]);
            IDictionaryEnumerator enumerator = args.NewValues.GetEnumerator();
            enumerator.Reset();
            while (enumerator.MoveNext())
            {
                row[enumerator.Key.ToString()] = enumerator.Value;
            }
            ChecklistGV.CancelEdit();

        }






    }
}