using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MBTimeSheetWebApp
{
    public class ConnectDI
    {
        public static SAPbobsCOM.Company _company;
        private static SAPbobsCOM.Recordset oRs;

        private static List<string> colNames = new List<string>();
        private static List<object> vals = new List<object>();

        //fill in the mandatory parameters for connection. Mandatory Parameters are:
        //Database server,
        //Database server type,
        //Database name, 
        //License server, string consisting of company server and port number
        //Company user name, 
        //Company user password.
        public static int OpenDIConnection(List<string> ConnectionVariables)
        {
            //string dbServer = ConnectionVariables.ElementAt(0);
            //string serverType = ConnectionVariables.ElementAt(1);
            //string companyDbName = ConnectionVariables.ElementAt(2);
            //string licenseServer = ConnectionVariables.ElementAt(3);
            //string companyUserName = ConnectionVariables.ElementAt(4);
            //string companyPassword = ConnectionVariables.ElementAt(5);
            //string dbUser = ConnectionVariables.ElementAt(6);
            //string dbPass = ConnectionVariables.ElementAt(7);

            //sessionId = _login.Login("10.0.128.160", "MILNERBROWNEIRELAND",
            //   LoginDatabaseType.dst_SYBASE, "manager", "B1Admin",
            //   LoginLanguage.ln_English, "30015");

            string dbServer = "10.0.128.160";
            string serverType = "HANADB";
            string companyDbName = "MILNERBROWNEIRELAND";
            string licenseServer = "30015";
            string companyUserName = "manager";
            string companyPassword = "B1Admin";
            string dbUser = "";
            string dbPass = "";

            _company = new SAPbobsCOM.Company();

            try
            {
                if (_company.Connected)
                {
                    return 0;
                }
                else
                {

                    int returnValue = -1;

                    _company.Server = dbServer;

                    if (serverType == "2005")

                        _company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;

                    else if (serverType == "2008")

                        _company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;

                    else if (serverType == "2012")

                        _company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;

                    else if (serverType == "HANADB")

                        _company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;

                    else
                        _company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;

                    _company.CompanyDB = companyDbName;
                    _company.LicenseServer = licenseServer;
                    _company.UserName = companyUserName;
                    _company.Password = companyPassword;
                    _company.DbUserName = dbUser;
                    _company.DbPassword = dbPass;
                    _company.language = SAPbobsCOM.BoSuppLangs.ln_English;

                    try
                    {
                        returnValue = _company.Connect();

                        if (returnValue != 0)
                        {
                           // MessageBox.Show(_company.GetLastErrorDescription());
                        }

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception occured: " + ex.Message);
                    }
                    //MessageBox.Show("connected to SAP");
                    return returnValue;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occured: " + ex.Message);
            }
            return -1;
        }

        public static SAPbobsCOM.Recordset GetRecordSet()
        {
            if (_company.Connected)
            {
                oRs = _company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            }
            else
            {
                Connect();
                GetRecordSet();
            }

            return oRs;

        }

        public static SAPbobsCOM.Company GetCompany(SAPbobsCOM.Recordset recSet)
        {
            if (_company.Connected)
            {
                return _company;
            }
            else
            {
                Connect();
                GetCompany(recSet);
            }
            return _company;
        }

        //disconnect from DI
        public static void DisconnectDI()
        {
            _company.Disconnect();

        }

        public static int Connect()
        {
            if (_company != null)
            {
                if (_company.Connected)
                {
                    return 0;
                }
                else
                {
                    ConnectDI.OpenDIConnection(Global.ConnectionVariables);
                    if (!_company.Connected)
                    {
                        return -1;
                    }

                    else
                        return 0;
                }
            }
            else
            {
                //MessageBox.Show("Error connecting to SAP");
                return -1;
            }
        }
    }
}