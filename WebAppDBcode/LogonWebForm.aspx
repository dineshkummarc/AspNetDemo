<%@ Page language="c#" Codebehind="LogonWebForm.aspx.cs" AutoEventWireup="false" Inherits="WebAppDB.LogonWebForm" smartNavigation="True" Description="Edit DataBase Logon"%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>LogonWebForm</title>
		<LINK href="../../../samples.css" type="text/css" rel="stylesheet">
		<meta content="Microsoft Visual Studio 7.0" name="GENERATOR">
		<meta content="C#" name="CODE_LANGUAGE">
		<meta content="JavaScript" name="vs_defaultClientScript">
		<meta content="http://schemas.microsoft.com/intellisense/ie5" name="vs_targetSchema">
		<SCRIPT language="JavaScript"> 
	       function errPopup() {
				var errmsg = document.forms[0].MsgTextBox.value;
				if (errmsg != "") {
					var oPopup = window.createPopup();
					var dlgBox;
					dlgBox = "<div id=\"myid\" style=\"position:absolute; top:0; left:0; width:100%; height:100%; background:#cccccc; border:1px solid black; border-top:1px solid white; border-left:1px solid white; padding:10px;  font:normal 10pt tahoma; padding-left:18px \"> <b>Error Message Box</b><hr size=\"1\" style=\"border:1px solid black;\"><div style=\"width:220px; font-family:tahoma; font-size:80%; line-height:1.5em\"><br><br></div><br><br></DIV><div style=\"position: absolute; top:50; left:18px; width:570px; height:225px; border:1px solid black; border-bottom:1px solid white; border-right:1px solid white; font:normal 10pt tahoma;  filter:progid:DXImageTransform.Microsoft.Gradient(GradientType=0, StartColorStr=gold, EndColorStr=#FFFFFF); padding:10px\" ><b>" ;
					dlgBox += errmsg; 
					dlgBox += "</div></div>"; 
					oPopup.document.body.innerHTML = dlgBox;
					oPopup.show(100, 50, 600, 300, document.body);
					}
				}
		</SCRIPT>
	</HEAD>
	<body onload="errPopup()" MS_POSITIONING="GridLayout">
		<form id="LogonWebForm" method="post" runat="server">
			<asp:textbox id="UserIDTextBox" style="Z-INDEX: 101; LEFT: 216px; POSITION: absolute; TOP: 64px" tabIndex="1" runat="server" Height="26" Width="154"></asp:textbox>
			<asp:label id="ItemsLabel" style="Z-INDEX: 126; LEFT: 483px; POSITION: absolute; TOP: 391px" runat="server" Width="187px" Height="16px">Select Tables</asp:label>
			<asp:dropdownlist id="ItemsDropDownList" style="Z-INDEX: 125; LEFT: 378px; POSITION: absolute; TOP: 423px" tabIndex="13" runat="server" Width="361px" Height="40px" AutoPostBack="True"></asp:dropdownlist>
			<asp:button id="BuildConnButton" style="Z-INDEX: 108; LEFT: 334px; POSITION: absolute; TOP: 160px" tabIndex="5" runat="server" Height="32px" Width="152px" Text="Build Connection String"></asp:button><asp:textbox id="ConnectTextBox" style="Z-INDEX: 109; LEFT: 177px; POSITION: absolute; TOP: 212px" tabIndex="6" runat="server" Height="24px" Width="562px"></asp:textbox><asp:label id="ConnStringLabel" style="Z-INDEX: 110; LEFT: 35px; POSITION: absolute; TOP: 212px" runat="server" Height="16px" Width="120px">Connection String</asp:label><asp:checkbox id="SysTblsCheckBox" style="Z-INDEX: 111; LEFT: 480px; POSITION: absolute; TOP: 474px" tabIndex="10" runat="server" Height="16px" Width="112px" Text="System Items"></asp:checkbox><asp:button id="GetDatabasesButton" style="Z-INDEX: 112; LEFT: 169px; POSITION: absolute; TOP: 290px" tabIndex="7" runat="server" Height="33px" Width="159px" Text="Get Databases/Schemas"></asp:button><asp:dropdownlist id="DBDropDownList" style="Z-INDEX: 113; LEFT: 377px; POSITION: absolute; TOP: 299px" tabIndex="8" runat="server" Height="24px" Width="362px" AutoPostBack="True"></asp:dropdownlist>&nbsp;
			<asp:button id="ItemsButton" style="Z-INDEX: 124; LEFT: 169px; POSITION: absolute; TOP: 417px" tabIndex="12" runat="server" Width="159px" Height="32px" Text="Get Tables"></asp:button><asp:label id="UserIDLabel" style="Z-INDEX: 102; LEFT: 104px; POSITION: absolute; TOP: 64px" runat="server" Height="19px" Width="81px">UserID</asp:label><asp:label id="PasswordLabel" style="Z-INDEX: 103; LEFT: 103px; POSITION: absolute; TOP: 111px" runat="server" Height="19px" Width="81px">Password</asp:label><asp:label id="TitleLabel" style="Z-INDEX: 104; LEFT: 296px; POSITION: absolute; TOP: 24px" runat="server" Height="28px" Width="248px" Font-Bold="True">OLE DB Connection To Database</asp:label><asp:label id="ServerLabel" style="Z-INDEX: 105; LEFT: 401px; POSITION: absolute; TOP: 62px" runat="server" Height="19px" Width="92px">Server/Filename</asp:label><asp:label id="SelectDBLabel" style="Z-INDEX: 106; LEFT: 439px; POSITION: absolute; TOP: 271px" runat="server" Height="24px" Width="157px">Select Database/Schema</asp:label><asp:textbox id="PasswordTextBox" style="Z-INDEX: 107; LEFT: 215px; POSITION: absolute; TOP: 111px" tabIndex="2" runat="server" Height="26px" Width="154px" TextMode="Password"></asp:textbox>
			<asp:textbox id="MsgTextBox" style="Z-INDEX: 114; LEFT: 44px; POSITION: absolute; TOP: 43px" runat="server" Height="1px" Width="1px"></asp:textbox><asp:label id="ProviderLabel" style="Z-INDEX: 115; LEFT: 400px; POSITION: absolute; TOP: 112px" runat="server" Height="16px" Width="72px">Provider</asp:label><asp:dropdownlist id="ProviderDropDownList" style="Z-INDEX: 116; LEFT: 512px; POSITION: absolute; TOP: 112px" tabIndex="4" runat="server" Height="32px" Width="216px">
				<asp:ListItem Value="SQLOLEDB||">Microsoft Provider for SQL Server</asp:ListItem>
				<asp:ListItem Value="MSDAORA||">Microsoft Provider for Oracle</asp:ListItem>
				<asp:ListItem Value="OraOLEDB.Oracle|;OLEDB.NET=TRUE|">Oracle Provider</asp:ListItem>
				<asp:ListItem Value="Microsoft.Jet.OLEDB.4.0||">Microsoft Provider for Jet 4.0</asp:ListItem>
			</asp:dropdownlist><asp:checkbox id="CPPCheckBox" style="Z-INDEX: 117; LEFT: 221px; POSITION: absolute; TOP: 474px" tabIndex="9" runat="server" Height="24px" Width="214px" Text="use C++ code to get Item  info"></asp:checkbox><asp:label id="ArgumentsLabel" style="Z-INDEX: 118; LEFT: 26px; POSITION: absolute; TOP: 524px" runat="server" Height="29px" Width="112px">SQL Statement or Parameters</asp:label><INPUT id="ServerInputID" title="Enter the server or the .mdb filename" style="Z-INDEX: 119; LEFT: 513px; WIDTH: 214px; POSITION: absolute; TOP: 62px; HEIGHT: 27px" accessKey="ServerInputKey" tabIndex="3" type="file" size="16" name="ServerInput" runat="server">
			<HR style="Z-INDEX: 120; LEFT: 115px; WIDTH: 64.64%; POSITION: absolute; TOP: 353px; HEIGHT: 6px" width="64.64%" color="#cccccc" SIZE="6">
			<asp:Button id="ExecuteButton" style="Z-INDEX: 121; LEFT: 365px; POSITION: absolute; TOP: 594px" runat="server" Width="87px" Height="31px" Text="Execute" tabIndex="15"></asp:Button>
			<asp:TextBox id="ArgumentsTextBox" style="Z-INDEX: 122; LEFT: 161px; POSITION: absolute; TOP: 519px" runat="server" Width="577px" Height="47px" TextMode="MultiLine" Rows="14"></asp:TextBox>
			<asp:RadioButtonList id="TypeRadioButtonList" style="Z-INDEX: 123; LEFT: 43px; POSITION: absolute; TOP: 402px" runat="server" Width="89px" Height="68px" AutoPostBack="True" BorderColor="Silver" BorderStyle="Solid" tabIndex="11">
				<asp:ListItem Value="Table" Selected="True">Table</asp:ListItem>
				<asp:ListItem Value="View">View</asp:ListItem>
				<asp:ListItem Value="Procedure">Procedure</asp:ListItem>
			</asp:RadioButtonList>
			<asp:Label id="ItemLabel" style="Z-INDEX: 127; LEFT: 58px; POSITION: absolute; TOP: 375px" runat="server" Width="71px" Height="10px">Select Item</asp:Label></form>
	</body>
</HTML>
