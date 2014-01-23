<%@ Page Language="C#" AutoEventWireup="true" CodeFile="homePage.aspx.cs" Inherits="_homePage" %>
<%@ Register Src="~/FBInbox.ascx" TagName="FBInbox" TagPrefix="ctrl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="facebook.css" rel="stylesheet" type="text/css" />
    

    <script src="jquery.js" type="text/javascript"></script>
    <script src="jquery.cycle.all.min.js" type="text/javascript"></script>
    <script src="Facebook.js" type="text/javascript"></script>


    <script src="jquery.floatobject-1.0.js" type="text/javascript"></script>

    <title>Facebook Inbox</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
           <img src="Images/facebook.png" />
         <ctrl:FBInbox runat="server" id="FBInbox1" />
    </div>
    </form>
</body>
</html>
