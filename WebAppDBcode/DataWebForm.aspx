<%@ Page language="c#" Codebehind="DataWebForm.aspx.cs" AutoEventWireup="false" Inherits="WebAppDB.DataWebForm" %>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<HTML>
	<HEAD>
		<title>DataWebForm</title>
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
	<body MS_POSITIONING="GridLayout" onload="errPopup()">
		<form id="Form1" method="post" runat="server">
			<asp:datagrid id="DataGridDB" style="Z-INDEX: 101; LEFT: 32px; POSITION: absolute; TOP: 216px" runat="server" Height="464px" Width="712px" BorderColor="White" BorderStyle="Ridge" CellSpacing="1" BorderWidth="2px" BackColor="White" CellPadding="3" GridLines="None" AllowSorting="True">
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
				<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
				<HeaderStyle Font-Bold="True" ForeColor="#E7E7FF" BackColor="#4A3C8C"></HeaderStyle>
				<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
				<Columns>
					<asp:EditCommandColumn ButtonType="PushButton" UpdateText="Update" CancelText="Cancel" EditText="Edit"></asp:EditCommandColumn>
					<asp:ButtonColumn Text="Delete" ButtonType="PushButton" CommandName="Delete"></asp:ButtonColumn>
				</Columns>
				<PagerStyle HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
			</asp:datagrid><asp:label id="Label1" style="Z-INDEX: 102; LEFT: 304px; POSITION: absolute; TOP: 16px" runat="server" Height="24px" Width="104px">Data</asp:label><asp:linkbutton id="NewTableLinkButton" style="Z-INDEX: 103; LEFT: 640px; POSITION: absolute; TOP: 16px" runat="server" Height="32px" Width="72px">New Item</asp:linkbutton>
			<asp:DataGrid id="InsertDataGrid" style="Z-INDEX: 104; LEFT: 34px; POSITION: absolute; TOP: 50px" runat="server" GridLines="None" CellPadding="3" BackColor="White" BorderWidth="2px" CellSpacing="1" BorderStyle="Ridge" BorderColor="White" Width="712px" Height="96px" PageSize="1" AllowPaging="True">
				<SelectedItemStyle Font-Bold="True" ForeColor="White" BackColor="#9471DE"></SelectedItemStyle>
				<ItemStyle ForeColor="Black" BackColor="#DEDFDE"></ItemStyle>
				<HeaderStyle Font-Bold="True" ForeColor="#E7E7FF" BackColor="#4A3C8C"></HeaderStyle>
				<FooterStyle ForeColor="Black" BackColor="#C6C3C6"></FooterStyle>
				<Columns>
					<asp:ButtonColumn Text="Insert" ButtonType="PushButton" CommandName="Delete"></asp:ButtonColumn>
				</Columns>
				<PagerStyle Visible="False" HorizontalAlign="Right" ForeColor="Black" BackColor="#C6C3C6"></PagerStyle>
			</asp:DataGrid>
			<asp:TextBox id="MsgTextBox" style="Z-INDEX: 105; LEFT: 520px; POSITION: absolute; TOP: 32px" runat="server" Width="1px" Height="1px"></asp:TextBox>
			<asp:TextBox id="SQLTextBox" style="Z-INDEX: 106; LEFT: 32px; POSITION: absolute; TOP: 176px" runat="server" Width="713px" ReadOnly="True"></asp:TextBox></form>
		</SCRIPT>
	</body>
</HTML>
