using FileHelpers;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MBTimeSheetWebApp
{
    public class ProjectInfo
    {
        public ObjectId ID { get; set; }
        public string UserName { get; set; }
        public string CustCode { get; set; }
        public string CustName { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string PmName { get; set; }
        public string PmEmail { get; set; }
        public DateTime Date { get; set; }
        public string CustContact { get; set; }
        public string CustEmail { get; set; }
        public string CustSignature { get; set; }
        public string Version { get; set; }
        public int VersionId { get; set; }
        public string Type { get; set; }

        public ProjectInfo(ObjectId id, string cCode, string cName, string pjCode, string pjName, string pmName, string pmEmail, DateTime date, string custCntact, string custEmail, string signature, string version, int versionId, string edit)
        {
            this.ID = id;
            this.CustCode = cCode;
            this.CustName = cName;
            this.ProjectCode = pjCode;
            this.ProjectName = pjName;
            this.PmName = pmName;
            this.PmEmail = pmEmail;
            this.Date = date;
            this.CustContact = custCntact;
            this.CustEmail = custEmail;
            this.CustSignature = signature;
            this.Version = version;
            this.VersionId = versionId;
            this.Type = edit;
        }
        public ProjectInfo()
        {
           
        }
    }

    public class TimeSheetObj: ProjectInfo
    {
        public string TimeSpent { get; set; }
        public string[] Agenda { get; set; }
        public string Deviation { get; set; }
        public string CrRequired { get; set; }
        public string[] Outcome { get; set; }
        public bool IsSignedOff { get; set; }

        public TimeSheetObj(string projCode, string time, string[] agenda, string dev, string cr, string[] outcome, bool signed)
        {
            this.ProjectCode = projCode;
            this.TimeSpent = time;
            this.Agenda = agenda;
            this.Deviation = dev;
            this.CrRequired = cr;
            this.Outcome = outcome;
            this.IsSignedOff = signed;
        }

        public TimeSheetObj()
        {

        }
    }

    public class CRObj: ProjectInfo
    {
        public string Estimates { get; set; }
        public string DeptMb { get; set; }
        public string Time { get; set; }
        public string[] Reasons { get; set; }
        public string CustAuth { get; set; }
        public bool IsSignedOff { get; set; }

        public CRObj(string projCode, string est, string dept, string time, string[] reason, string auth, bool signed)
        {
            this.ProjectCode = projCode;
            this.Estimates = est;
            this.DeptMb = dept;
            this.Time = time;
            this.Reasons = reason;
            this.CustAuth = auth;
            this.IsSignedOff = signed;
        }
        public CRObj()
        {

        }
    }

    [DelimitedRecord(",")]
    public class ChecklistObj
    {
        public int ID { get; set; }
        public string Area { get; set; }
        public string Paragraph { get; set; }
        public string Action { get; set; }
        public string Priority { get; set; }
        public string ClientAssign { get; set; }
        public string MBAssign { get; set; }
        public string Complete { get; set; }
        public string DateFinished { get; set; }
        public string Comments { get; set; }

        public ChecklistObj()
        {

        }
    }

    [DataObject(true)]
    public class TableObj 
    {
        public string Point { get; set; }
        public string Description { get; set; }

        [DataObjectMethod(DataObjectMethodType.Update,true)]
        public static void Update(string point, string description)
        {

        }
    }
                                            
}