using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MBTimeSheetWebApp.LoginService;
using MBTimeSheetWebApp.CompanyService;
using MBTimeSheetWebApp.ProjectsService;
using Sap.Data.Hana;

namespace MBTimeSheetWebApp
{
    public class SAPConnect
    {
        private static LoginService.LoginServiceSoapClient _login;
        private static CompanyService.CompanyServiceSoapClient _company = new CompanyService.CompanyServiceSoapClient();
        private static ProjectsService.ProjectsServiceSoapClient _project = new ProjectsServiceSoapClient();

        public static string GetSessionId()
        {
            string sessionId = "";
            try
            {
                _login = new LoginServiceSoapClient();

                //sessionId = _login.Login("10.0.128.160", "MILNERBROWNEIRELAND",
                //   LoginDatabaseType.dst_SYBASE, "manager", "B1Admin",
                //   LoginLanguage.ln_English, "30015");

                sessionId = _login.Login("IIS-SPARE-HP\\SQLEXPRESS", "SBOAndrewUK",
                    LoginDatabaseType.dst_MSSQL2014, "manager",
                    "password", LoginLanguage.ln_English, "localhost");

                Console.WriteLine(sessionId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return sessionId;
        }

        public static CompanyService.MsgHeader GetCompany()
        {
            //          ' Create Service 
            //myCompanyService = New CompanyService

            //' Create Header
            //Dim msgHeader As MsgHeader = New MsgHeader()
            //msgHeader.SessionID = GlbData.sessionID
            //msgHeader.ServiceName = MsgHeaderServiceName.CompanyService
            //msgHeader.ServiceNameSpecified = True
            //myCompanyService.MsgHeaderValue = msgHeader
            CompanyService.MsgHeader msgHead = new CompanyService.MsgHeader();
            msgHead.SessionID = GetSessionId();
            msgHead.ServiceName = MBTimeSheetWebApp.CompanyService.MsgHeaderServiceName.CompanyService;
            msgHead.ServiceNameSpecified = true;

            //GetCompanyInfoRequest request = new GetCompanyInfoRequest();    
            //request.MsgHeader.SessionID = GetSessionId();
            //MessageBox.Show(request.ToString());
            CompanyService.Recordset recSet = new CompanyService.Recordset();
            //GetCompanyInfo companyInfo1 = new GetCompanyInfo();
            //_company.GetCompanyInfo(msgHead, companyInfo1);
            //MessageBox.Show(companyInfo1.ToString());

            return msgHead;

        }

        public static HanaConnection ConnectHana()
        {
            HanaConnection con = new HanaConnection();
            char[] x = new char[6];
            System.Security.SecureString pass = new System.Security.SecureString();
            //HanaCredential password = new HanaCredential("SYSTEM",);
            try
            {
                con = new HanaConnection("Server=20.0.201.20:30015;Database=MILNERBROWNEIRELAND;" +
                "UserID=SYSTEM;Password=SAPB1Admin");

               // con = new HanaConnection("Server=10.0.128.160:30015;Database=MILNERBROWNEIRELAND;" +
               //"UserID=SYSTEM; Password=Passw0rd");

                Console.WriteLine(con.ConnectionString);

                con.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
            
            return con;
        }

        public static ProjectInfo GetProjInfo(CompanyService.MsgHeader header)
        {
            Recordset recset = new Recordset();
            RecordsetParams rsParams = new RecordsetParams();
            int row = 0;
            ProjectInfo proj = new ProjectInfo();
            string queryStr = "SELECT "+"\"PrjCode\", "+"\"PrjName\", " +"\"U_IIS_prjManager\", "+ "\"U_IIS_prjStatus\" FROM OPRJ ";

            rsParams.Query = queryStr;
            Query qry = new Query();
            qry.RecordsetParams = rsParams;
            recset = _company.Query(header, qry);

            //multidimensional array
            // row[] = row number, property[].name = column name, property[].value = column value
            for (int i = 0; i < recset.Row.Length - 1; i++)
            {
                row = i;
                try
                {
                    if (recset.Row[row].Property.IsValidIndex(0))
                    {
                        proj.ProjectCode = recset.Row[row].Property[0].Value.ToString();
                    }
                    else
                    {
                        proj.ProjectCode = String.Empty;
                    }

                    if (recset.Row[row].Property.IsValidIndex(1))
                    {
                        proj.ProjectName = recset.Row[row].Property[1].Value.ToString();
                    }
                    else
                    {
                        proj.ProjectName = String.Empty;
                    }

                    if (recset.Row[row].Property.IsValidIndex(2))
                    {
                        proj.PmName = recset.Row[row].Property[2].Value.ToString();
                    }
                    else
                    {
                        proj.PmName = "";
                    }

                }
                catch (IndexOutOfRangeException ex)
                {
                    throw new ArgumentOutOfRangeException("exception occurred in row: " + row, ex);
                }

                
            }
            return proj;
        }// SQL

        public static ProjectInfo GetProjInfo()
        {
            ProjectInfo proj = new ProjectInfo();
            try
            {
                ConnectHana();
                using (HanaConnection connect = ConnectHana())
                {
                    string queryStr = "SELECT " + "\"PrjCode\", " + "\"PrjName\", " + "\"U_IIS_prjManager\", " + "\"U_IIS_prjStatus\" FROM OPRJ ";

                    HanaCommand cmd = new HanaCommand(queryStr, connect);

                    HanaDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        proj.ProjectCode = reader.GetValue(0).ToString();
                        proj.ProjectName = reader.GetValue(1).ToString();
                        proj.PmName = reader.GetValue(2).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }

            return proj;
        } //HANA

        public static void GetProjectInfo(CompanyService.MsgHeader header)
        {
            Recordset recset = new Recordset();
            RecordsetParams rsParams = new RecordsetParams();
            int row = 0;
            ProjectInfo proj = new ProjectInfo();
            string queryStr = "SELECT " + "\"PrjCode\", " + "\"PrjName\", " + "\"U_IIS_prjManager\", " + "\"U_IIS_prjStatus\" FROM OPRJ ";

            rsParams.Query = queryStr;
            Query qry = new Query();
            qry.RecordsetParams = rsParams;
            recset = _company.Query(header, qry);

        }


     
    }
}