﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="WebReport.Report"%> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>
        <asp:Literal ID="myLiteral" runat="server" Text="<%$ Resources:Title %>"></asp:Literal>
    </title>
    <%-- include Bootstrap --%>        
    <script src="../Scripts/jquery-1.9.1.js"></script>
    <script src="../Scripts/jquery-ui.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.min.css" rel="stylesheet" /> 
    <link href="css/Style.css"  type = "text/css" rel="stylesheet" />

    <script src="../PDFjs/src/shared/util.js"></script>
    <script src="../PDFjs/src/display/api.js"></script>
    <script src="../PDFjs/src/display/canvas.js"></script>
    <script src="../PDFjs/src/display/webgl.js"></script>
    <script src="../PDFjs/src/display/font_loader.js"></script>
    <script>
        PDFJS.workerSrc = '../PDFjs/src/worker_loader.js';
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
       
      <div class="row">  
          <div class="col-xs-12">
            <asp:Image ID="Image1" runat="server" ImageUrl="img/Synevo_logo_.png" Width="130" Height="140" /><br />
               <asp:ImageButton ID="ImageButtonClearSession" runat="server" 
                  ImageUrl="img/edit-clear.png"  
                  ToolTip="<%$ Resources:ImageButtonClearSession %>"
                  OnClick="BtnClearSession_Click" />
          </div>  
      </div>                 
      <div class="row">
          <div class="col-xs-10" style="padding-left:40px; margin-top:-60px;"> 
             <asp:Label ID="Label1" runat="server" Text="<%$ Resources:LabelHeader %>"></asp:Label><br /><br /> 
          </div>          
      </div> 
        <div class="col-xs-12" style="text-align:center;">  
               <asp:Label ID="WarningMessage" runat="server" Text="<%$ Resources:WarningMessage %>" Visible="false"></asp:Label> 
        </div>
       <div class="row">
        <div class="col-xs-12" style="padding-left:40px">
            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:LabelCode %>"></asp:Label>
         </div>  
      </div> 
       <div class="row">
        <div class="col-xs-4" style="padding-left:40px">
             <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="https://www.synevo.ua/uk/kod-dostupa" Target="_blank"><i id="ex-on-img"><span>
                 <asp:Label ID="Label3" runat="server" Text ="<%$ Resources:LabelExample %>"></asp:Label></span></i></asp:HyperLink>             
         </div> 
      </div> 

      <div class="row">
        <div class="col-xs-12" style="padding-top:0px; padding-left:40px">
             <asp:TextBox ID="WebAccessCodeTextBox" runat="server" MaxLength="20"  OnTextChanged="WebAccessCodeTextBox_TextChanged"></asp:TextBox>
         </div>     
      </div> 


        <div class="row">
        <div class="col-xs-12" style="padding-top:0px; padding-left:40px">
              <asp:Label ID="ErrorLabel" runat="server" ForeColor="Red"></asp:Label>
         </div>     
      </div> 

      <div class="row" 
            style="padding: 5px 0px 2px 7px; 
            text-align:center; 
            max-width:120px;
            min-width:120px;
            margin:auto;
            border-radius: 8px;
            text-transform: uppercase;
            font-weight: bold;
            font-family: Helvetica, Arial, sans-serif;
            background-color: #ffb824;
            background: linear-gradient(to top, #faba01, #ffe696);
            border: 1px solid #ccc;
            box-shadow: 0 0 5px rgba(0,0,0,0.3); 
            min-width: 305px;">  
                <asp:Button ID="BtnGetResult" runat="server" Text="<%$ Resources:BtnGetResultText %>" ToolTip="<%$ Resources:BtnGetResultToolTip %>" BackColor="#FCB829" OnClick="BtnGetResult_Click"/>          
      </div> 

      <div class="row" style="padding-top:10px;">
           <div class="col-xs-" style="padding-top:0px;
                 text-align:left;
                 max-width:270px; 
                 min-width:270px;
                 margin:auto;">
               <asp:Label ID="StatusLabel" runat="server"></asp:Label>   
           </div>
      </div>

<%--     <iframe src="SynevoResults15348104_Micro.pdf" width="800px" height="600px" />  --%>

     <asp:Panel ID="PanelResult" runat="server" BackColor="White" BorderStyle="None" 
                HorizontalAlign="Left" Width="100%" Visible="False">   

         <asp:Image ID="ImageGif" runat="server" ImageUrl="img/712.gif"/>

          <ul class="nav nav-tabs" style="margin-top:30px">
              <li id="tabPanelMainResult" runat="server"> 
                  <a data-toggle="tab" href="#panelMainResult"> <asp:Label ID="ResultPDFLabel" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label> </a>
              </li>
              <li id="tabPanelMicroResult" runat="server">
                   <a data-toggle="tab" href="#panelMicroResult"> <asp:Label ID="ResultPDFMicroLabel" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label> </a>
              </li>
              <li id="tabPanelExtraAttachmentResult" runat="server">
                 <a data-toggle="tab" href="#panelExtraAttachmentResult"><asp:Label ID="ResultPDFExtraAttachmentLabel" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label></a>
              </li>
           </ul>     
         <br />

         <div class="row" style="text-align:center;">    
          <div class="tab-content">
            <div id="panelMainResult" class="tab-pane fade" runat="server">
             <%-- рендеринг MainResult --%>    
                  <asp:ImageButton ID="ImageButtonPrev" runat="server" 
                  ImageUrl="img/findbarButton-prev@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonPrev %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonNext" runat="server" 
                  ImageUrl="img/findbarButton-next@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonNext %>" />&nbsp &nbsp &nbsp

                  <span id="page_num" style="font-size:24px; vertical-align: 7px;"></span><span id="page_count" style="font-size:24px; vertical-align: 7px;"></span>&nbsp &nbsp &nbsp                  
                 
                  <asp:ImageButton ID="ImageButtonFirstPage" runat="server" 
                  ImageUrl="img/secondaryToolbarButton-firstPage@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonFirstPage %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonLastPage" runat="server" 
                  ImageUrl="img/secondaryToolbarButton-lastPage@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonLastPage %>" />&nbsp &nbsp &nbsp 

                  <asp:ImageButton ID="ImageButtonZoomIn" runat="server" 
                  ImageUrl="img/toolbarButton-zoomIn@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonZoomInToolTip %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonZoomOut" runat="server" 
                  ImageUrl="img/toolbarButton-zoomOut@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonZoomOutToolTip %>" />&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp

                  <asp:ImageButton ID="MainExportButton" runat="server" 
                  ImageUrl="img/toolbarButton-download@2x.png"
                  onclick="MainExportButton_Click"
                  ToolTip="<%$ Resources:ButtonToolTip %>" />
                 <br /> 
                 <asp:Label ID="ErrorDownloadMainResultLabel" runat="server"></asp:Label>                
                 <canvas id="canvasMain" width="0" height="0"/>                           
                 <script src="../PDFjs/src/rendering_contentMain.js"></script>                
              </div>

            <div id="panelMicroResult" class="tab-pane fade" runat="server">
                  <%-- рендеринг MicroResult --%>
                  <asp:ImageButton ID="ImageButtonPrevMicro" runat="server" 
                  ImageUrl="img/findbarButton-prev@2x.png" OnClientClick="return false"
                  ToolTip="<%$ Resources:ImageButtonPrev %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonNextMicro" runat="server" 
                  ImageUrl="img/findbarButton-next@2x.png" OnClientClick="return false"
                  ToolTip="<%$ Resources:ImageButtonNext %>" />&nbsp &nbsp &nbsp

                  <span id="page_numMicro" style="font-size:24px; vertical-align: 7px;"></span><span id="page_countMicro" style="font-size:24px; vertical-align: 7px;"></span>&nbsp &nbsp &nbsp                  

                  <asp:ImageButton ID="ImageButtonFirstPageMicro" runat="server" 
                  ImageUrl="img/secondaryToolbarButton-firstPage@2x.png"  OnClientClick="return false"
                  ToolTip="<%$ Resources:ImageButtonFirstPage %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonLastPageMicro" runat="server" 
                  ImageUrl="img/secondaryToolbarButton-lastPage@2x.png" OnClientClick="return false"
                  ToolTip="<%$ Resources:ImageButtonLastPage %>" />&nbsp &nbsp &nbsp
                
                
                  <asp:ImageButton ID="ImageButtonZoomInMicro" runat="server" 
                  ImageUrl="img/toolbarButton-zoomIn@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonZoomInToolTip %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonZoomOutMicro" runat="server" 
                  ImageUrl="img/toolbarButton-zoomOut@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonZoomOutToolTip %>" />&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp

                  <asp:ImageButton ID="MicroExportButton" runat="server" 
                  ImageUrl="img/toolbarButton-download@2x.png" 
                  onclick="MicroExportButton_Click"
                  ToolTip="<%$ Resources:ButtonToolTip %>" />
               
                  <br /> 
                 <asp:Label ID="ErrorDownloadMicroResultLabel" runat="server"></asp:Label>
                 <canvas id="canvasMicro" width="0" height="0"/>          
                 <script type="text/javascript" src="../PDFjs/src/rendering_contentMicro.js"></script>                           
              </div>

                <div id="panelExtraAttachmentResult" class="tab-pane fade" runat="server">
                  <%-- рендеринг ExtraAttachment --%>
                  <asp:ImageButton ID="ImageButtonPrevExtraAttachment" runat="server" 
                  ImageUrl="img/findbarButton-prev@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonPrev %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonNextExtraAttachment" runat="server" 
                  ImageUrl="img/findbarButton-next@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonNext %>" />&nbsp &nbsp &nbsp

                  <span id="page_numExtraAttachment" style="font-size:24px; vertical-align: 7px;"></span><span id="page_countExtraAttachment" style="font-size:24px; vertical-align: 7px;"></span>&nbsp &nbsp &nbsp                  
                 
                  <asp:ImageButton ID="ImageButtonFirstPageExtraAttachment" runat="server" 
                  ImageUrl="img/secondaryToolbarButton-firstPage@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonFirstPage %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonLastPageExtraAttachment" runat="server" 
                  ImageUrl="img/secondaryToolbarButton-lastPage@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonLastPage %>" />&nbsp &nbsp &nbsp 

                  <asp:ImageButton ID="ImageButtonZoomInExtraAttachment" runat="server" 
                  ImageUrl="img/toolbarButton-zoomIn@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonZoomInToolTip %>" />&nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ImageButtonZoomOutExtraAttachment" runat="server" 
                  ImageUrl="img/toolbarButton-zoomOut@2x.png" OnClientClick="return false" 
                  ToolTip="<%$ Resources:ImageButtonZoomOutToolTip %>" />&nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp &nbsp

                  <asp:ImageButton ID="ExtraAttachmentExportButton" runat="server" 
                  ImageUrl="img/toolbarButton-download@2x.png"
                  onclick="ExtraAttachmentExportButton_Click"
                  ToolTip="<%$ Resources:ExtraAttachmentExportButton %>" />
                 <br /> 
                 <asp:Label ID="ErrorDownloadExtraAttachmentResultLabel" runat="server"></asp:Label>               
                 <canvas id="canvasExtraAttachment" width="0" height="0"/>                                                
                 <script src="../PDFjs/src/rendering_contentExtraAttachment.js"></script> 
               </div>
          </div>
       </div>

      <div class="row" id="rowResultAllLabel" style="margin-top:10px; display:none;" runat="server">
          <div class="col-xs-12"
               style="min-width:305px; text-align:center;">
               <asp:Label ID="ResultAll" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
               <asp:ImageButton ID="ResultAllExportButton" runat="server" 
                 ImageUrl="img/toolbarButton-download-all.png" onclick="ResultAllExportButton_Click" 
                 ToolTip="<%$ Resources:ButtonDownloadToolTip %>" />&nbsp
          </div>        
      </div>  
        
        </asp:Panel><br />
         <script>
             setTimeout('document.getElementById("ImageGif").style.display = "none"',2000);
         </script> 
    </div>
   </form>
</body>
</html>
