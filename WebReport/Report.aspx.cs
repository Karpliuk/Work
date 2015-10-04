using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebReport.ServiceReference;
using System.IO;
using System.Net;
using System.IO.Compression;
using System.Collections;
using NLog;

namespace WebReport
{
    public partial class Report : System.Web.UI.Page
    {
        Logger logger = LogManager.GetCurrentClassLogger();

        protected override void InitializeCulture()
        {
            try
            {
                string lang = Page.RouteData.Values["lang"] as string ?? "auto";
                // string lang = Request.QueryString["lang"] as string ?? "auto"; // Get 
                UICulture = lang;
                Culture = lang;
                base.InitializeCulture();
            }
            catch (Exception ex)
            {
               // logger.Warn(ex.Message);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WarningMessageCulture(); // проверка культуры на укр язык? ок, вывести предуприждение            
            if (!IsPostBack)
            {

            }
        }

        protected void WebAccessCodeTextBox_TextChanged(object sender, EventArgs e)
        {
            this.ErrorLabel.Text = null;
            this.StatusLabel.Text = null; 
            this.PanelResult.Visible = false;
            this.ErrorDownloadMainResultLabel.Text = null;
            this.ErrorDownloadMicroResultLabel.Text = null;
            this.ErrorDownloadExtraAttachmentResultLabel.Text = null;
            this.ResultPDFLabel.Text = GetLocalResourceObject("ResultPDFLabelText").ToString();
            this.ResultPDFMicroLabel.Text = GetLocalResourceObject("ResultPDFMicroLabelText").ToString();
            this.ResultPDFExtraAttachmentLabel.Text = GetLocalResourceObject("ResultPDFExtraAttachmentLabelText").ToString();
            this.tabPanelMainResult.Style["display"] = "none";
            this.tabPanelMicroResult.Style["display"] = "none";
            this.tabPanelExtraAttachmentResult.Style["display"] = "none";
        }

        protected void BtnGetResult_Click(object sender, EventArgs e)
        {
            ArrayList arrExtraAttachment = null; // для массива ExtraAttachment
            bool flag = false; // для проверки ExtraAttachment
            bool active = true; // для активной вкладки

            if (String.IsNullOrEmpty(WebAccessCodeTextBox.Text))
            {
                this.ErrorLabel.Text = GetLocalResourceObject("ErrorLabelText").ToString();
            }
            else
            {
                try
                {
                    ResultServiceClient client =  new ResultServiceClient();
                    var resultSummary = client.GetResultSummaryByWebCode(WebAccessCodeTextBox.Text);     
                    if (resultSummary == null)
                    {
                        this.StatusLabel.ForeColor = System.Drawing.Color.Red;
                        this.StatusLabel.Text = GetLocalResourceObject("StatusLabelOrderIsNotFound").ToString();
                    }
                    else
                    {
                        this.StatusLabel.Text = null;
                        //// to set up a local instance of the client
                        //var client2 = new HipChat.HipChatClient("4fc92f77bc0deaf0d203344e68c650", "1894434", "DotNetSender");
                        //// send a message
                        //client2.SendMessage("Hello World!");

                        //// for one-off use, there is a static method - NB this creates an instance internally, so 
                        //// only use when making one-off calls.
                        //HipChat.HipChatClient.SendMessage("4fc92f77bc0deaf0d203344e68c650", "1894434", "DotNetSender", "Hello World!");
                        // "http://192.168.1.251/SilabIntegration/ResultDownload.svc/Result?orderId=11019592&resultType=0"
                        //logger.Error("Test");
                        if ((resultSummary.Results.Count() > 0) && (resultSummary.IsFinal == true)) // ваш заказ готов результаты есть
                        {
                            this.StatusLabel.ForeColor = System.Drawing.Color.Green;
                            this.StatusLabel.Text = GetLocalResourceObject("StatusLabelReady").ToString();                            
                        }
                        if ((resultSummary.Results.Count() > 0) && (resultSummary.IsFinal == false)) // ваш заказ не готов показаны предварительные результаты
                        {
                            this.StatusLabel.ForeColor = System.Drawing.Color.Red;
                            this.StatusLabel.Text = GetLocalResourceObject("StatusLabelNotReady").ToString();                           
                        }
                        if ((resultSummary.Results.Count() == 0) && (resultSummary.IsFinal == true)) // по вашему заказу нет результатов, заказ закрыт
                        {
                            this.StatusLabel.ForeColor = System.Drawing.Color.Red;
                            this.StatusLabel.Text = GetLocalResourceObject("StatusLabelPerezabor").ToString();
                            return;
                        }
                        if ((resultSummary.Results.Count() == 0) && (resultSummary.IsFinal == false)) // по вашему заказу нет результатов
                        {
                            this.StatusLabel.ForeColor = System.Drawing.Color.Red;
                            this.StatusLabel.Text = GetLocalResourceObject("StatusLabelNotResult").ToString();
                            return;
                        }

                        this.PanelResult.Visible = true;                       
                        DeleteFileAndClearSession();  //удалим все файлы для даного сеанса и удалим из состояния сеанса все ключи и значения
                        Session["barcode"] = resultSummary.Barcode.ToString();
                        foreach (var result in resultSummary.Results)
                        {
                            if (String.Equals("SilabMainResult", result.Type.ToString()))
                            {
                                Session["SilabMainResultUri"] = result.ServiceUri;
                                GetFileSilabMainResult();
                                this.tabPanelMainResult.Style["display"] = "inline-block";
                                if (string.IsNullOrEmpty(this.tabPanelMainResult.Attributes["class"]))
                                {
                                   
                                }
                                else
                                {
                                    var newClassValue = this.panelMainResult.Attributes["class"].Replace("tab-pane fade in active", "tab-pane fade");
                                    this.panelMainResult.Attributes["class"] = newClassValue;
                                    this.tabPanelMainResult.Attributes.Remove("class");
                                }                              
                                if (active)
                                {
                                    var newClassValue = this.panelMainResult.Attributes["class"].Replace("tab-pane fade", "tab-pane fade in active");
                                    this.panelMainResult.Attributes["class"] = newClassValue;
                                    this.tabPanelMainResult.Attributes.Add("class", "active");
                                    active = false;
                                }
                               
                            }
                            else
                            if (String.Equals("SilabMicroResult", result.Type.ToString()))
                            {
                                Session["SilabMicroResultUri"] = result.ServiceUri;
                                GetFileSilabMicroResult();
                                this.tabPanelMicroResult.Style["display"] = "inline-block";
                                if (string.IsNullOrEmpty(this.tabPanelMicroResult.Attributes["class"]))
                                {
                                    
                                }
                                else
                                {
                                    var newClassValue = this.panelMicroResult.Attributes["class"].Replace("tab-pane fade in active", "tab-pane fade");
                                    this.panelMicroResult.Attributes["class"] = newClassValue;
                                    this.tabPanelMicroResult.Attributes.Remove("class");
                                }
                                if (active)
                                {
                                    var newClassValue = this.panelMicroResult.Attributes["class"].Replace("tab-pane fade", "tab-pane fade in active");
                                    this.panelMicroResult.Attributes["class"] = newClassValue;
                                    this.tabPanelMicroResult.Attributes.Add("class", "active");
                                    active = false;
                                }                               
                            }
                            else
                            if (String.Equals("ExtraAttachment", result.Type.ToString()))
                            {
                                if (flag == false)
                                {
                                    arrExtraAttachment = new ArrayList();
                                }
                                flag = true;
                                arrExtraAttachment.Add(result.ServiceUri);
                                this.tabPanelExtraAttachmentResult.Style["display"] = "inline-block";
                                if (string.IsNullOrEmpty(this.tabPanelExtraAttachmentResult.Attributes["class"]))
                                {
                                    
                                }   
                                else
                                {
                                    var newClassValue = this.panelExtraAttachmentResult.Attributes["class"].Replace("tab-pane fade in active", "tab-pane fade");
                                    this.panelExtraAttachmentResult.Attributes["class"] = newClassValue;
                                    this.tabPanelExtraAttachmentResult.Attributes.Remove("class");
                                }                            
                                if (active)
                                {
                                    var newClassValue = this.panelExtraAttachmentResult.Attributes["class"].Replace("tab-pane fade", "tab-pane fade in active");
                                    this.panelExtraAttachmentResult.Attributes["class"] = newClassValue;                                   
                                    this.tabPanelExtraAttachmentResult.Attributes.Add("class", "active");
                                    active = false;
                                }
                                //this.rowResultPDFExtraAttachmentLabel.Style["display"] = "block";
                            }
                        }
                        if (flag)
                        {
                            Session["ExtraAttachmentUri"] = arrExtraAttachment;                           
                        }
                        if (resultSummary.Results.Length >= 2)
                        {
                            this.ResultAll.Text = GetLocalResourceObject("ResultAllText").ToString();
                            this.rowResultAllLabel.Style["display"] = "block";
                        }
                    }
                    client.Close();
                }
                catch (Exception ex)
                {
                    this.StatusLabel.ForeColor = System.Drawing.Color.Red;
                    this.StatusLabel.Text = GetLocalResourceObject("StatusLabelExeption").ToString(); 
                    logger.Error(ex.Message +" | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
                }
            }
        }

        private void WarningMessageCulture()
        {
            if (!String.Equals(UICulture, "Ukrainian"))
                this.WarningMessage.Visible = true;
        }

        protected void MainExportButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] arr = webClient.DownloadData(Session["SilabMainResultUri"].ToString());

                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoResults" + Session["barcode"] + ".pdf");
                Response.ContentType = "application/pdf";
                Response.OutputStream.Write(arr, 0, arr.Length);
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            #region alternative
            /*
            string filename = @"C:\SynevoResults" + Session["barcode"] + ".pdf";
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            webClient.DownloadFile(Session["SilabMainResultUri"].ToString(), filename);
            FileInfo fileInfo = new FileInfo(filename);
            if (fileInfo.Exists)
            {
                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoResults" + Session["barcode"] + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Flush();
                Response.WriteFile(fileInfo.FullName);
                Response.End();
            }
            fileInfo.Delete();
            */
            #endregion
        }

        protected void MicroExportButton_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                byte[] arr = webClient.DownloadData(Session["SilabMicroResultUri"].ToString());

                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoResults" + Session["barcode"] + "_Micro.pdf");
                Response.ContentType = "application/pdf";
                Response.OutputStream.Write(arr, 0, arr.Length);
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            catch (Exception ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
        }

        protected void ExtraAttachmentExportButton_Click(object sender, ImageClickEventArgs e)
        {
            ArrayList arrExtraAttachment = (ArrayList)Session["ExtraAttachmentUri"];
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            if (arrExtraAttachment.Count == 1) // если один файл
            {
                try
                {
                    string tmp = arrExtraAttachment[0].ToString();
                    byte[] arr = webClient.DownloadData(arrExtraAttachment[0].ToString());

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=SynevoAddResults" + Session["barcode"] + "_Add.pdf");
                    Response.ContentType = "application/pdf";
                    Response.OutputStream.Write(arr, 0, arr.Length);
                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
                }
                catch (Exception ex)
                {
                    logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
                }
            }
            else
            {
                string zipFileName = null;
                try
                {
                    DirectoryInfo directoryinfo = Directory.CreateDirectory(@"C:\" + DateTime.Now.ToString("dd MMMM yyyy") + " SynevoAddResults" + Session["barcode"]);

                    string fileNameAdd = "SynevoAddResults" + Session["barcode"] + "_";
                    zipFileName = directoryinfo.FullName + ".zip";

                    for (int i=0; i< arrExtraAttachment.Count; i++)
                    {
                        webClient.DownloadFile(arrExtraAttachment[i].ToString(), Path.Combine(directoryinfo.FullName, fileNameAdd + i + ".pdf"));
                    }
                   

                    ZipFile.CreateFromDirectory(directoryinfo.FullName, zipFileName);
                    directoryinfo.Delete(true);

                    Response.Clear();
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + directoryinfo.Name + ".zip");
                    Response.ContentType = "application/zip";
                    Response.WriteFile(zipFileName);
                    Response.Flush();
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                catch (System.Threading.ThreadAbortException ex)
                {
                    logger.Error(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
                }
                finally
                {
                    if (File.Exists(zipFileName))
                    {
                        File.Delete(zipFileName);
                    }
                }
            }
        }

        protected void ResultAllExportButton_Click(object sender, ImageClickEventArgs e)
        {
            string zipFileName = null;
            try
            {
                DirectoryInfo directoryinfo = Directory.CreateDirectory(@"C:\" + DateTime.Now.ToString("dd MMMM yyyy") + " SynevoResults" + Session["barcode"]);

                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;

                string fileNameMain = "SynevoResults" + Session["barcode"] + ".pdf";
                string fileNameMicro = "SynevoResults" + Session["barcode"] + "_Micro.pdf";
                string fileNameAdd = "SynevoAddResults" + Session["barcode"] + "_";
                zipFileName = directoryinfo.FullName + ".zip";

                if (Session["SilabMainResultUri"] != null)
                {
                    webClient.DownloadFile(Session["SilabMainResultUri"].ToString(), Path.Combine(directoryinfo.FullName, fileNameMain));
                }
                if (Session["SilabMicroResultUri"] != null)
                {
                    webClient.DownloadFile(Session["SilabMicroResultUri"].ToString(), Path.Combine(directoryinfo.FullName, fileNameMicro));
                }
                if (Session["ExtraAttachmentUri"] != null)
                {
                    ArrayList arrExtraAttachment = (ArrayList)Session["ExtraAttachmentUri"];
                    for (int i = 0; i < arrExtraAttachment.Count; i++)
                    {
                        webClient.DownloadFile(arrExtraAttachment[i].ToString(), Path.Combine(directoryinfo.FullName, fileNameAdd + i + ".pdf"));
                    }
                }


                ZipFile.CreateFromDirectory(directoryinfo.FullName, zipFileName);
                directoryinfo.Delete(true);

                Response.Clear();
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + directoryinfo.Name+".zip");
                Response.ContentType = "application/zip";
                Response.WriteFile(zipFileName);
                Response.Flush();
                Response.SuppressContent = true;
                HttpContext.Current.ApplicationInstance.CompleteRequest();
            }
            catch (System.Threading.ThreadAbortException ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            catch (Exception ex)
            {
                 logger.Warn(ex.Message +" | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            finally
            {
                if (File.Exists(zipFileName))
                {
                    File.Delete(zipFileName);
                }
            }                
        }

        public void GetFileSilabMainResult()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                string fileNameMain = "SynevoResults" + Session["barcode"] + ".pdf";
                webClient.DownloadFile(Session["SilabMainResultUri"].ToString(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileNameMain));
                Session["SilabMainResultFile"] = fileNameMain;
            }            
             catch (System.Threading.ThreadAbortException ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            catch (Exception ex)
            {                
                this.ErrorDownloadMainResultLabel.Text= GetLocalResourceObject("ErrorDownloadResultLabel").ToString();
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
        }

        public void GetFileSilabMicroResult()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.Credentials = CredentialCache.DefaultCredentials;
                string fileNameMicro = "SynevoResults" + Session["barcode"] + "_Micro.pdf";
                webClient.DownloadFile(Session["SilabMicroResultUri"].ToString(), Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileNameMicro));
                Session["SilabMіcroResultFile"] = fileNameMicro;                
            }           
             catch (System.Threading.ThreadAbortException ex)
            {
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
            catch (Exception ex)
            { 
                this.ErrorDownloadMicroResultLabel.Text = GetLocalResourceObject("ErrorDownloadResultLabel").ToString();
                logger.Warn(ex.Message + " | Row: " + ex.StackTrace.Substring(ex.StackTrace.LastIndexOf(' ')) + " | CodeForWebFFSOrder: " + WebAccessCodeTextBox.Text);
            }
        }

        public void DeleteFileAndClearSession()
        {
            if (Session["SilabMainResultFile"] != null)
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMainResultFile"].ToString())))
                {
                    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMainResultFile"].ToString()));
                }
            }
            if (Session["SilabMicroResultFile"] != null)
            {
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMicroResultFile"].ToString())))
                {
                    File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Session["SilabMicroResultFile"].ToString()));
                }
            }
            Session.RemoveAll();
        }

        [System.Web.Services.WebMethod]
        public static string GetSilabMainResult()
        {
            try
            {
                return HttpContext.Current.Session["SilabMainResultFile"].ToString();
            }
            catch (Exception ex)
            {
                //результаты невозможно выкачать с сервиса
            }
            return null;  
        }

        [System.Web.Services.WebMethod]
        public static string GetSilabMicroResult()
        {
            try
            {
                return HttpContext.Current.Session["SilabMіcroResultFile"].ToString();
            }
            catch (Exception ex)
            {
                //результаты невозможно выкачать с сервиса
            }
            return null;

        }
    }
}