<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="WebReport.Report"%> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Результаты исследований</title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="css/Style.css"  type = "text/css" rel="stylesheet" />   
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
       
      <div class="row">  
          <div class="col-xs-12">
            <asp:Image ID="Image1" runat="server" ImageUrl="img/Synevo_logo_.png" Width="130" Height="140" /><br />
          </div>     
      </div>                 
      <div class="row">
          <div class="col-xs-12" style="padding-left:40px; margin-top:-30px;"> 
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
                <asp:Button ID="BtnGetResult" runat="server" Text="<%$ Resources:BtnGetResultText %>" ToolTip="<%$ Resources:BtnGetResultToolTip %>" BackColor="#FCB829" OnClick="BtnGetResult_Click" />          
      </div> 

      <div class="row" style="padding-top:10px;">
           <div class="col-xs-" style="padding-top:0px;
                 text-align:justify;
                 max-width:270px; 
                 min-width:270px;
                 margin:auto;">
               <asp:Label ID="StatusLabel" runat="server"></asp:Label>   
           </div>
      </div>
          

     <asp:Panel ID="PanelResult" runat="server" BackColor="White" BorderStyle="None" 
                HorizontalAlign="Left" Width="100%" Visible="False">
     
      <div class="row" style="height:60px; padding-top:10px;"> 
          <div class="col-xs-12"
               style="min-width:305px; text-align:center;">
               <asp:Label ID="ResultPDFLabel" runat="server"></asp:Label>
               <asp:ImageButton ID="MainExportButton" runat="server" 
                 ImageUrl="img/exportbutton.jpeg" onclick="MainExportButton_Click" 
                 Visible="False" BorderColor="Black" BorderStyle="Solid" 
                 BorderWidth="1px" ToolTip="<%$ Resources:ButtonToolTip %>" />&nbsp
          </div>  
      </div> 
          <hr />
      <div class="row" style="height:60px; padding-top:10px;">  
          <div class="col-xs-12"
               style="min-width:305px; text-align:center;">
               <asp:Label ID="ResultPDFMicroLabel" runat="server"></asp:Label>
               <asp:ImageButton ID="MicroExportButton" runat="server" 
                 ImageUrl="img/exportbutton.jpeg" onclick="MicroExportButton_Click" 
                 Visible="False" BorderColor="Black" BorderStyle="Solid" 
                 BorderWidth="1px" ToolTip="<%$ Resources:ButtonToolTip %>" />&nbsp
          </div>  
      </div> 
          <hr />
        <div class="row" style="height:60px; padding-top:10px;"> 
          <div class="col-xs-12"
               style="min-width:305px; text-align:center;">
               <asp:Label ID="ResultPDFExtraAttachmentLabel" runat="server"></asp:Label>
               <asp:ImageButton ID="ExtraAttachmentExportButton" runat="server" 
                 ImageUrl="img/exportbutton.jpeg" onclick="ExtraAttachmentExportButton_Click" 
                 Visible="False" BorderColor="Black" BorderStyle="Solid" 
                 BorderWidth="1px" ToolTip="<%$ Resources:ButtonToolTip %>" />&nbsp
          </div>  
      </div> 
          <hr />
      <div class="row" style="height:60px; padding-top:10px;">
          <div class="col-xs-12"
               style="min-width:305px; text-align:center;">
               <asp:Label ID="ResultAll" runat="server"></asp:Label>
               <asp:ImageButton ID="ResultAllExportButton" runat="server" 
                 ImageUrl="img/download.png" onclick="ResultAllExportButton_Click" 
                 Visible="False" BorderColor="Black" BorderStyle="Solid" 
                 BorderWidth="1px" ToolTip="<%$ Resources:ButtonDownloadToolTip %>" />&nbsp
          </div>        
      </div> 
     </asp:Panel><br />
    </div>
   </form>
</body>
</html>
