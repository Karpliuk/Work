using System;
using System.Web;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Collections;
using NLog;
using System.Text;
using System.Configuration;

namespace WebReport
{
    public class ResultHandler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            Logger logger = LogManager.GetCurrentClassLogger();
            try
            {
                WebClient webClient = new WebClient();
                string login = "", pass = "";
                login = ConfigurationManager.AppSettings["loginForService"];
                pass = ConfigurationManager.AppSettings["passwordForService"];
                webClient.Credentials = new NetworkCredential(login, pass);

                byte[] arrMain;
                byte[] arrMicro;
                byte[] arrExtraAttachment;

                if ((context.Session["SilabMainResultUri"] != null) && (context.Request.Form["Main"] != null || context.Request.QueryString["Main"] != null))
                {
                    arrMain = webClient.DownloadData(context.Session["SilabMainResultUri"].ToString());
                    context.Response.Clear();
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoResults" + context.Session["barcode"] + ".pdf");
                    context.Response.ContentType = "application/pdf";
                    context.Response.BinaryWrite(arrMain);
                    context.Response.Flush();
                }
                if ((context.Session["SilabMicroResultUri"] != null) && (context.Request.Form["Micro"] != null || context.Request.QueryString["Micro"] !=null))
                {
                    arrMicro = webClient.DownloadData(context.Session["SilabMicroResultUri"].ToString());
                    context.Response.Clear();
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoResults" + context.Session["barcode"] + "_Micro.pdf");
                    context.Response.ContentType = "application/pdf";
                    context.Response.BinaryWrite(arrMicro);
                    context.Response.Flush();
                }
                if ((context.Session["ExtraAttachmentUri"] != null) && (context.Request.Form["ExtraAttachment"] != null || context.Request.QueryString["ExtraAttachment"] != null))
                {
                    ArrayList arrExtraAttachment_ = (ArrayList)context.Session["ExtraAttachmentUri"];
                    arrExtraAttachment = webClient.DownloadData(arrExtraAttachment_[0].ToString());
                    context.Response.Clear();
                    context.Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoAddResults" + context.Session["barcode"] + "_Add.pdf");
                    context.Response.ContentType = "application/pdf";
                    context.Response.BinaryWrite(arrExtraAttachment);
                    context.Response.Flush();
                }
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')));
            }          
        }   
        #endregion
    }
}
