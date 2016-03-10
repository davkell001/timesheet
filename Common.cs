using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace MBTimeSheetWebApp
{
    public class Common
    {

        public static System.Drawing.Image LoadImage(string signatureStr)
        {
            System.Drawing.Image image = null;
            System.Drawing.Image image2 = null;
            if (signatureStr != String.Empty)
            {
                string base64Str = signatureStr.Replace("data:image/png;base64,", "");
                byte[] bytes = Convert.FromBase64String(base64Str);

                using (MemoryStream ms = new MemoryStream(bytes))
                {
                    image = System.Drawing.Image.FromStream(ms);
                }
                image2 = new Bitmap(image.Width, image.Height);
                Color color = Color.White;
                Rectangle rect = new Rectangle(Point.Empty, image.Size);

                using (Graphics G = Graphics.FromImage(image2))
                {
                    G.Clear(color);
                    G.DrawImageUnscaledAndClipped(image, rect);
                }

            }

            return image2;
        }

        public static System.Drawing.Image LoadImageFromFile(string filename)
        {
            System.Drawing.Image image = null;
            System.Drawing.Image image2 = null;
            if (filename != String.Empty)
            {
                image = System.Drawing.Image.FromFile(filename);
                image2 = new Bitmap(image.Width, image.Height);

                Color color = Color.White;
                Rectangle rect = new Rectangle(Point.Empty, image.Size);

                using (Graphics G = Graphics.FromImage(image2))
                {
                    G.Clear(color);
                    G.DrawImageUnscaledAndClipped(image, rect);
                }

            }

            return image2;
        }

        public static string GetSiteVistHtmlString(ProjectInfo proj, TimeSheetObj tab1, bool includesCR)
        {
            List<DataColumn> colList = new List<DataColumn>();

            colList.Add(new DataColumn() { ColumnName = "Point" });
            colList.Add(new DataColumn() { ColumnName = "Description" });

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div class='container' style='margin-right:50px; margin-left:50px; font-family: verdana'>");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #00adef; color:#ffffff;' colspan = '4'><b>Site Visit</b></td></tr>");
                    sb.Append("<tr><td colspan = '4'></td></tr>");
                    //project info section
                    sb.Append("<tr><td width=20%><b>Customer Code: </b></td><td><input type='text' value='" + proj.CustCode + "' /></td>");
                    sb.Append("<td width=20%><b>Customer Name: </b></td><td><input type='text' value='" + proj.CustName + "' /></td></tr>");
                    sb.Append("<tr><td><b>Project Code: </b></td><td><input type='text' value='" + proj.ProjectCode + "' /></td>");
                    sb.Append("<td><b>Project Name: </b></td><td><input type='text' value='" + proj.ProjectName + "' /></td></tr>");
                    sb.Append("<tr><td><b>PM Name: </b></td><td><input type='text' value='" + proj.PmName + "' /></td>");
                    sb.Append("<td><b>PM Email: </b></td><td><input type='text' value='" + proj.PmEmail + "' /></td></tr>");
                    sb.Append("<tr><td><b>Date: </b></td><td> <input type='text' value='" + proj.Date.ToShortDateString() + "' /></td>");
                    sb.Append("<td><b>Consultant: </b></td><td> <input type='text' value='" + HttpContext.Current.User.Identity.Name + "' /></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");

                    //tab1 section
                    sb.Append("<table width='50%' cellspacing='0' cellpadding='2'>");
                    //sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td width=40%><b>Time Spend: </b></td><td> <input type='text' value='" + tab1.TimeSpent + "' /></td></tr>");
                    sb.Append("<tr><td><b>Deviation: </b></td><td> <input type='text' value='" + tab1.Deviation + "' /></td></tr>");
                    sb.Append("<tr><td><b>CR Required: </b></td><td> <input type='text' value='" + tab1.CrRequired + "' /></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    //agenda table
                    sb.Append("<h3>Agenda:</h3>");
                    sb.Append("<table width='100%'>");
                    sb.Append("<tr>");
                    sb.Append("<th style = 'background-color: #91268f;color:#ffffff'> Point </th>");
                    sb.Append("<th style = 'background-color: #91268f;color:#ffffff'> Description </th>");
                    sb.Append("</tr>");
                    if (tab1.Agenda != null)
                    {
                        for (int i = 1; i < tab1.Agenda.Count(); i += 2)
                        {
                            string cell1 = tab1.Agenda[i - 1];
                            string cell2 = tab1.Agenda[i];

                            sb.Append("<tr>");
                            if (cell1 != String.Empty || cell1 != null)
                            {
                                sb.Append("<td width = 20%>");
                                sb.Append(cell1);
                                sb.Append("</td>");
                            }

                            if (cell2 != String.Empty || cell2 != null)
                            {
                                sb.Append("<td>");
                                sb.Append(cell2);
                                sb.Append("</td>");
                            }
                            sb.Append("</tr>");
                        }
                    }
                    

                    sb.Append("</table>");
                    //Outcome table
                    sb.Append("<h3>Outcome:</h3>");
                    sb.Append("<table width='100%''>");
                    sb.Append("<tr>");
                    sb.Append("<th style = 'background-color: #91268f;color:#ffffff'> Point </th>");
                    sb.Append("<th style = 'background-color: #91268f;color:#ffffff'> Description </th>");
                    sb.Append("</tr>");
                    if (tab1.Outcome != null)
                    {
                        for (int i = 1; i < tab1.Outcome.Count(); i += 2)
                        {
                            string cell1 = tab1.Outcome[i - 1];
                            string cell2 = tab1.Outcome[i];

                            sb.Append("<tr>");
                            if (cell1 != String.Empty || cell1 != null)
                            {
                                sb.Append("<td width = 20%>");
                                sb.Append(cell1);
                                sb.Append("</td>");
                            }

                            if (cell2 != String.Empty || cell2 != null)
                            {
                                sb.Append("<td>");
                                sb.Append(cell2);
                                sb.Append("</td>");
                            }
                            sb.Append("</tr>");
                        }
                    }
                    
                    sb.Append("</table>");
                    sb.Append("<br />");
                    if (!includesCR)
                    {
                        sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                        sb.Append("<tr><td align='center' style='background-color: #00adef; color:#ffffff;' colspan = '2'><b>Sign Off</b></td></tr>");
                        sb.Append("<tr><td colspan = '2'></td></tr>");
                        sb.Append("<tr><td><b>Contact Name: </b><input type='text' value='" + proj.CustContact + "' /></td>");
                        sb.Append("<td><b>Customer Authorisation: </b><input type='text' value='" + proj.CustEmail + "' /></td></tr>");
                        
                    }
                    sb.Append("</div>");

                    StringReader sr = new StringReader(sb.ToString());

                    return sb.ToString();

                }
            }
        }

        public static string GetCRHtmlString(ProjectInfo proj, CRObj tab2)
        {
            List<DataColumn> colList = new List<DataColumn>();

            colList.Add(new DataColumn() { ColumnName = "Point" });
            colList.Add(new DataColumn() { ColumnName = "Description" });

            using (StringWriter sw = new StringWriter())
            {
                using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                {

                    StringBuilder sb = new StringBuilder();
                    //heading
                    sb.Append("<div class='container' style='margin-right:50px; margin-left:50px; font-family: verdana;'>");
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #00adef; color:#ffffff' colspan = '4'><b>Change Request</b></td></tr>");
                    sb.Append("<tr><td colspan = '4'></td></tr>");
                    //project info section
                    sb.Append("<tr><td width=20%><b>Customer Code: </b></td><td><input type='text' value='" + proj.CustCode + "' /></td>");
                    sb.Append("<td><b>Customer Name: </b></td><td><input type='text' value='" + proj.CustName + "' /></td></tr>");
                    sb.Append("<tr><td><b>Project Code: </b></td><td><input type='text' value='" + proj.ProjectCode + "' /></td>");
                    sb.Append("<td><b>Project Name: </b></td><td><input type='text' value='" + proj.ProjectName + "' /></td></tr>");
                    sb.Append("<tr><td><b>PM Name: </b></td><td><input type='text' value='" + proj.PmName + "' /></td>");
                    sb.Append("<td><b>PM Email: </b></td><td><input type='text' value='" + proj.PmEmail + "' /></td></tr>");
                    sb.Append("<tr><td><b>Date: </b></td><td> <input type='text' value='" + proj.Date.ToShortDateString() + "' /></td>");
                    sb.Append("<td><b>Consultant: </b></td><td> <input type='text' value='" + HttpContext.Current.User.Identity.Name + "' /></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");

                    //tab2
                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td width=20%><b>Please Note:</b></td> <td><b>Any CR will have a likely impact on project timeline and will need to be evaluated against project plan</b></td></tr>");
                    sb.Append("</table>");
                    sb.Append("</br>");
                    sb.Append("<table width='50%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td width=40%><b>Estimate Available: </b></td><td><input type='text' value='" + tab2.Estimates + "' /></td></tr>");
                    sb.Append("<tr><td><b>Department MB: </b></td><td><input type='text' value='" + tab2.DeptMb + "' /></td></tr>");
                    sb.Append("<tr><td><b>Time: </b></td><td><input type='text' value='" + tab2.Time + "' /></td></tr>");
                    sb.Append("<tr><td><b>Customer Authorisation: </b></td><td><input type='text' value='" + tab2.CustAuth + "' /></td></tr>");
                    sb.Append("</table>");
                    sb.Append("<br />");
                    //Reason table
                    sb.Append("<h3>Reasons:</h3>");
                    sb.Append("<table width='100%'>");
                    sb.Append("<tr>");
                    sb.Append("<th style = 'background-color: #91268f;color:#ffffff'> Point </th>");
                    sb.Append("<th style = 'background-color: #91268f;color:#ffffff'> Description </th>");
                    sb.Append("</tr>");


                    for (int i = 1; i < tab2.Reasons.Count(); i += 2)
                    {
                        string cell1 = tab2.Reasons[i - 1];
                        string cell2 = tab2.Reasons[i];

                        sb.Append("<tr>");

                        if (cell1 != String.Empty || cell1 != null)
                        {
                            sb.Append("<td width = 20%>");
                            sb.Append(cell1);
                            sb.Append("</td>");
                        }

                        if (cell2 != String.Empty || cell2 != null)
                        {
                            sb.Append("<td>");
                            sb.Append(cell2);
                            sb.Append("</td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");
                    sb.Append("<br />");

                    sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                    sb.Append("<tr><td align='center' style='background-color: #00adef; color:#ffffff;' colspan = '2'><b>Sign Off</b></td></tr>");
                    sb.Append("<tr><td colspan = '2'></td></tr>");
                    sb.Append("<tr><td><b>Contact Name: </b><input type='text' value='" + proj.CustContact + "' /></td>");
                    sb.Append("<td><b>Customer Authorisation: </b><input type='text' value='" + proj.CustEmail + "' /></td></tr>");
                    sb.Append("</div>");

                    StringReader sr = new StringReader(sb.ToString());

                    return sb.ToString();

                }
            }
        }
    }
}