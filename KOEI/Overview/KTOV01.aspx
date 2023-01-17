<%@ Page Language="VB" AutoEventWireup="false" CodeFile="KTOV01.aspx.vb" Inherits="Overview_KTOV01" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>KOEI TOOL Management System</title>
    <link href="../App_Themes/common.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/design.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/font.css" type="text/css" rel="Stylesheet" />
    <link href="../App_Themes/fix_table.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        
        <div class="login_bg">
	        <div class="login_logo"></div><!--KOEI TOOL Logo-->
            <div class="login_center"><!--INPUT User Name-->
    	        <div class="font font_color_normal font_size_14 float_l">USER NAME&nbsp;&nbsp;</div>
                <div><asp:TextBox ID="txtUsername" runat="server" MaxLength="20" Width="200px"></asp:TextBox></div>
            </div>
            <br />            
            <div class="login_center"><!--INPUT Password-->
    	        <div class="font font_color_normal font_size_14 float_l">PASSWORD&nbsp;&nbsp;</div>
                <div><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="40" Width="200px" ></asp:TextBox></div>
            </div><br />
             <asp:Button ID="btnLogin" runat="server" Text="" CssClass="login_center text_center login_mouse" /><!--BUTTOM Login-->
             <br />
             <div class="text_right"><asp:Label ID="lblVersion" CssClass="font_version" runat="server" Text="Version:1.0-20141003-001"></asp:Label></div>
        </div>
                
    </form>
</body>
</html>




