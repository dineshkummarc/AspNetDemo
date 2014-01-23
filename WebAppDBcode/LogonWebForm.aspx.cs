using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.Common;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;


namespace WebAppDB
{
	/// <summary>
	/// Summary description for LogonWebForm.
	/// </summary>
	public class LogonWebForm : System.Web.UI.Page
	{
		/// <summary>DLL Imports for C++ routine DBConnstr(string provider, string catalog, string userid, string password) in WebAppUtil.dll</summary>
		[DllImport("\\Inetpub\\wwwroot\\WebAppDB\\bin\\WebAppUtil.dll")]
		public static extern string DBConnstr(string provider, string catalog, string userid, string password);
		/// <summary>DLL Imports for C++ routine DBGetTables(string connectStr, bool systemTables) in WebAppUtil.dll</summary>
		[DllImport("\\Inetpub\\wwwroot\\WebAppDB\\bin\\WebAppUtil.dll")]
		public static extern string DBGetTables(string connectStr, string dbName, string schemaName, bool systemTables);
		/// <summary>DLL Imports for C++ routine DBGetViews(string connectStr, bool systemViews) in WebAppUtil.dll</summary>
		[DllImport("\\Inetpub\\wwwroot\\WebAppDB\\bin\\WebAppUtil.dll")]
		public static extern string DBGetViews(string connectStr, string dbName, string schemaName, bool systemViews);
		/// <summary>DLL Imports for C++ routine DBGetProcs(string connectStr, string procName) in WebAppUtil.dll</summary>
		[DllImport("\\Inetpub\\wwwroot\\WebAppDB\\bin\\WebAppUtil.dll")]
		public static extern string DBGetProcs(string connectStr, string dbName, string schemaName, string procName);
		/// <summary>DLL Imports for C++ routine DBGetIndexes(string connectStr, string tableName) in WebAppUtil.dll</summary>
		[DllImport("\\Inetpub\\wwwroot\\WebAppDB\\bin\\WebAppUtil.dll")]
		public static extern string DBGetIndexes(string connectStr, string tableName);
		/// <summary>DLL Imports for C++ routine DBGetQuote(string connectStr) in WebAppUtil.dll</summary>
		[DllImport("\\Inetpub\\wwwroot\\WebAppDB\\bin\\WebAppUtil.dll")]
		public static extern string DBGetQuote(string connectStr);

		private string builtConnectionString, connStr;	// connection string values
		private string selectedDB, selectedSchema;		// database/schema values
		private string indexName, itemName;				// Item and Index name
		private string[] procParms;						// parameters for procedure call
		private string[] procParmNames;					// field name from procs
		private string[] LBNames;						// array for Tables and Index names
		private string SQLstatement;					// Select statement
		private string quotePrefix = "\"", quoteSuffix = "\"";
		private string queryType;

		/// <value>Title label.</value>/// 
		protected System.Web.UI.WebControls.Label TitleLabel;
		/// <value>User ID label.</value>
		protected System.Web.UI.WebControls.Label UserIDLabel;
		/// <value>User ID text box.</value>
		protected System.Web.UI.WebControls.TextBox UserIDTextBox;
		/// <value>Password label.</value>
		protected System.Web.UI.WebControls.Label PasswordLabel;
		/// <value>Password text box.</value>
		protected System.Web.UI.WebControls.TextBox PasswordTextBox;
		/// <value>Server label.</value>
		protected System.Web.UI.WebControls.Label ServerLabel;
		/// <value>Provider label.</value>
		protected System.Web.UI.WebControls.Label ProviderLabel;
		/// <value>Provider list box.</value>
		protected System.Web.UI.WebControls.DropDownList ProviderDropDownList;
		/// <value>Build connection string button.</value>
		protected System.Web.UI.WebControls.Button BuildConnButton;
		/// <value>Connection string label.</value>
		protected System.Web.UI.WebControls.Label ConnStringLabel;
		/// <value>Connection string text box.</value>
		protected System.Web.UI.WebControls.TextBox ConnectTextBox;
		/// <value>Selected DB label.</value>
		protected System.Web.UI.WebControls.Label SelectDBLabel;
		/// <value>Get databases button.</value>
		protected System.Web.UI.WebControls.Button GetDatabasesButton;
		/// <value>Databases list box.</value>
		protected System.Web.UI.WebControls.DropDownList DBDropDownList;
		/// <value>System Tables check box.</value>
		protected System.Web.UI.WebControls.CheckBox SysTblsCheckBox;
		/// <value>HIDDEN error text box.</value>
		protected System.Web.UI.WebControls.TextBox MsgTextBox;
		/// <value>HTML INPUT browse for .mdb filename button.</value>
		protected System.Web.UI.HtmlControls.HtmlInputFile ServerInputID;
		/// <value>Use C++ routines Check Box.</value>
		protected System.Web.UI.WebControls.CheckBox CPPCheckBox;		
		/// <value>Selct item type RadioButtonList.</value>
		protected System.Web.UI.WebControls.RadioButtonList TypeRadioButtonList;
		/// <value>Get ITEMS button.</value>
		protected System.Web.UI.WebControls.Button ItemsButton;
		/// <value>ITEMS label.</value>
		protected System.Web.UI.WebControls.Label ItemsLabel;
		/// <value>ITEMS list box.</value>
		protected System.Web.UI.WebControls.DropDownList ItemsDropDownList;
		/// <value>Arguments label.</value>
		protected System.Web.UI.WebControls.Label ArgumentsLabel;
		/// <value>Arguments text box.</value>
		protected System.Web.UI.WebControls.TextBox ArgumentsTextBox;
		protected System.Web.UI.WebControls.Label ItemLabel;
		/// <value>Execute button.</value>
		protected System.Web.UI.WebControls.Button ExecuteButton;

		/// <summary>
		/// Occurs when the page loads and when posts are received from the server. 
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void Page_Load(object sender, System.EventArgs e)
		{
			if (IsPostBack) 
			{	// just a simple post back, retrieve save variables from Session			
				LogTraceMsg("Post Back");
				if (Session["myconnstr"] != null)
					connStr = Session["myconnstr"].ToString();
				if (Session["mydb"] != null)
					selectedDB = Session["mydb"].ToString();
				if (Session["myschema"] != null)
					selectedSchema = Session["myschema"].ToString();
				if (Session["myitem"] != null)
					itemName = Session["myitem"].ToString();
				if (Session["mySQL"] != null)
					SQLstatement = Session["mySQL"].ToString();
			}
			else
			{	// not a post back
				if (Session["myuserid"] != null)	// if new page but in Session ...
				{	// ... reinit page
					LogTraceMsg("Re-initializing page from Session");
					// restore saved data
					UserIDTextBox.Text = Session["myuserid"].ToString();
					ServerInputID.Value = Session["myserver"].ToString();
					PasswordTextBox.Text = Session["mypassword"].ToString();

					// retrieve variables from Session and re-init page
					if (Session["myconnstr"] != null)
					{
						ConnectTextBox.Text = Session["myconnstr"].ToString();
						connStr = ConnectTextBox.Text;
					}
					if (Session["myitems"] != null && Session["myitemindex"] != null )
					{
						int j = 0;
						LBNames = (string[])Session["myitems"];
						while (j < LBNames.Length)	// refill tables DD List
							ItemsDropDownList.Items.Add(LBNames[j++].ToString());
						ItemsDropDownList.SelectedIndex = (int)Session["myitemindex"];	// set selected field
						itemName = ItemsDropDownList.SelectedItem.Text;
					}
					if (Session["mydbindex"] != null && Session["myDBs"] != null)
					{
						int j = 0;
						LBNames = (string[])Session["myDBs"];
						while (j < LBNames.Length)	// refill Database DD List
							DBDropDownList.Items.Add(LBNames[j++].ToString());
						DBDropDownList.SelectedIndex = (int)Session["mydbindex"];	// set selected field
						selectedDB = DBDropDownList.SelectedItem.Text;
					}
					if (Session["myprocparmnames"] != null)
						procParmNames = (string[])Session["myprocparmnames"];

					if (Session["mySQL"] != null)
						SQLstatement = Session["mySQL"].ToString();
					else
						SQLstatement = "SELECT * FROM " + itemName;
					ArgumentsTextBox.Text = SQLstatement;
					if (Session["myquerytype"] != null)
					{
						string queryType = (string)Session["myquerytype"];
						for (int j= 0; j < TypeRadioButtonList.Items.Count; j++)
						{
							if (TypeRadioButtonList.Items[j].Value.ToString() == queryType)
							{
								ItemsLabel.Text = "Select " + queryType;
								ItemsButton.Text = "Get " + queryType + "s";
								TypeRadioButtonList.SelectedIndex = j;
								break;
							}
						}
					}

				}
				else 
				{	// brand new page/session
					LogTraceMsg("Brand new page");
					if (true)	// for testing convience
					{
						UserIDTextBox.Text = "SCOTT";	// Oracle Provider, case sensative !!!
						//UserIDTextBox.Text = "Admin";	// Access
						//UserIDTextBox.Text = "sa";		// MS SQL
					}
				}
			}

		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.ItemsDropDownList.SelectedIndexChanged += new System.EventHandler(this.ItemsDropDownList_SelectedIndexChanged);
			this.ItemsButton.Click += new System.EventHandler(this.ItemsButton_Click);
			this.BuildConnButton.Click += new System.EventHandler(this.BuildConnButton_Click);
			this.GetDatabasesButton.Click += new System.EventHandler(this.GetDatabasesButton_Click);
			this.DBDropDownList.SelectedIndexChanged += new System.EventHandler(this.DBDropDownList_SelectedIndexChanged);
			this.ExecuteButton.Click += new System.EventHandler(this.ExecuteProcButton_Click);
			this.TypeRadioButtonList.SelectedIndexChanged += new System.EventHandler(this.TypeRadioButtonList_SelectedIndexChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		/// <summary>
		/// Occurs when the 'Build Connection String' button is clicked. 
		/// Build the DB connection string dynamically based on user input.
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void BuildConnButton_Click(object sender, System.EventArgs e)
		{
			string password, server;
			// get provider selected
			string work = ProviderDropDownList.SelectedItem.Value.ToString();
			string[] workArray = work.Split('|');
			MsgTextBox.Text = "";	// clear out HIDDEN error box

			// to save time re-entering the Server and Password field ...
			// get saved server value if no new input
			if (Session["myserver"] != null && ServerInputID.Value == "")
				server = Session["myserver"].ToString();
			else
				server = ServerInputID.Value;
			// get saved password value if no new input
			if (Session["mypassword"] != null && PasswordTextBox.Text == "")
				password = Session["mypassword"].ToString();
			else
				password = PasswordTextBox.Text;

			// always call C++ DLL to constuct the connection string
			connStr = DBConnstr(workArray[0] , server, UserIDTextBox.Text, password);
			if (connStr.StartsWith("ERROR:"))	// returns "ERROR: ..." when error occured
			{
				LogErrorMsg(connStr);
				return;
			}
			// fix for some providers (eg. Oracle)
			if (password != "" && (connStr.IndexOf("Password",0) < 0)) 
			{
				connStr += ";Password=";
				connStr += password;
			}
			// add extra connection string parameters
			if (workArray[1] != "")
				connStr += workArray[1];
			// clear out dependent objects on page and their associative Session values
			DBDropDownList.Items.Clear();
			ItemsDropDownList.Items.Clear();
			Session["myDBs"] = null;
			Session["myitem"] = null;
			ArgumentsTextBox.Text = "";
			ConnectTextBox.Text = connStr;
			// put connection string on page
			LogTraceMsg("Done building connect string");
			// save connection info in Session variable
			Session["myuserid"] = UserIDTextBox.Text;
			Session["mypassword"] = PasswordTextBox.Text;
			Session["myserver"] = ServerInputID.Value; 
			Session["myconnstr"] = connStr;
		}
		
		/// <summary>
		/// Occurs when the 'Get Databases' button is clicked. 
		/// Retrieves the Database or Schema names and puts them into a DD ListBox.
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void GetDatabasesButton_Click(object sender, System.EventArgs e)
		{
			// check that previous item(s) have been selected
			if (Session["myconnstr"] == null)
			{
				LogErrorMsg("Build connection string first !");
				return;
			}

			int j=0;
			DataTable schema = null, DB = null;
			// clear out old values
			DBDropDownList.Items.Clear();
			if (Session["myDBs"] != null)
				Session["myDBs"] = null;

			OleDbConnection statConn = new OleDbConnection(connStr);
			try 
			{
				statConn.Open();
				// get schema
				schema = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Schemata,new object[] {null});
				// check if Databases supported
				if (schema.Rows.Count == 0 || schema.Rows[0]["CATALOG_NAME"].ToString() != "")	
				{	// there are catalogs, use instead of schema
					DB = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Catalogs,new object[] {null});
					LBNames = new string[DB.Rows.Count];	
					// insert Catalog names into drop down list and string array for Session variable
					while (j < DB.Rows.Count) 
					{
						LBNames[j] = DB.Rows[j]["CATALOG_NAME"].ToString();
						DBDropDownList.Items.Add(LBNames[j++]);
					}
					// first entry visible is current selected DB1
					if (DB.Rows.Count > 0)
						selectedDB = DB.Rows[0]["CATALOG_NAME"].ToString();
					else
						DBDropDownList.Items.Add("No Items found !");

					DB.Dispose();
					Session["mydb"] = selectedDB;
					Session["myschema"] = null;
				}
				else	// use schema names
				{	
					// need to re-inquire with user name
					schema.Dispose();	
					schema = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Schemata,new object[] {null,UserIDTextBox.Text.ToString()});
					// insert Schema names into drop down list and string array for Session variable
					LBNames = new string[schema.Rows.Count];
					while (j < schema.Rows.Count) 
					{
						LBNames[j] = schema.Rows[j]["SCHEMA_NAME"].ToString();
						DBDropDownList.Items.Add(LBNames[j++]); 
					}
					// first entry visible is current selected DB
					if (schema.Rows.Count > 0)
						selectedSchema = schema.Rows[0]["SCHEMA_NAME"].ToString();
					else
						DBDropDownList.Items.Add("No Items found !");
					Session["myschema"] = selectedSchema;
					Session["mydb"] = null;
					schema.Dispose();
				}
				Session["myDBs"] = LBNames;	// saved for return trip
				LBNames = null;
				Session["mydbindex"] = 0;	// save currently selected DB/Schema index
				schema.Dispose();
			}
			catch (Exception er)
			{
				LogException(er);
				if (schema != null) schema.Dispose();
				if (DB != null) DB.Dispose();
			}
			finally
			{
				if (statConn.State.ToString() == "Open") statConn.Close();
				if (statConn != null) statConn.Dispose();
			}
		}

		/// <summary>
		/// Occurs when a 'Select Item' radio button is clicked. 
		/// Build the DB connection string dynamically based on user input.
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void TypeRadioButtonList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			RadioButtonList rb = (RadioButtonList)sender;
			queryType = rb.SelectedItem.ToString();
			Session["myquerytype"] = queryType;
			ItemsLabel.Text = "Select " + queryType;
			ItemsButton.Text = "Get " + queryType + "s";
			ItemsDropDownList.Items.Clear();
			ArgumentsTextBox.Text = "";
			if (queryType != "Procedure")
				ArgumentsLabel.Text = "SQL Statement";
			else
				ArgumentsLabel.Text = "Proc Parameters";

		}

		/// <summary>
		/// Call to put papameter names in the ListBox.
		/// </summary>
		private void GetProcedureParms()
		{
			string[] itemNames = null;
			if (CPPCheckBox.Checked == true)		
			{	// use C++ routines
				string items = DBGetProcs(connStr, selectedDB,selectedSchema,itemName);
				if (items.StartsWith("ERROR:"))
				{
					LogErrorMsg(items);
					return;
				}
				itemNames = items.Split('|');
				ArgumentsTextBox.Rows =  itemNames.Length; 
			}
			else
			{
				DataTable schemaTable = null;
				OleDbConnection statConn = new OleDbConnection(connStr);
				try 
				{
					statConn.Open();
					schemaTable = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedure_Parameters,new object[] {selectedDB,selectedSchema,itemName,null});
					ArgumentsTextBox.Rows =  schemaTable.Rows.Count; 
					itemNames = new string[ArgumentsTextBox.Rows];
					for (int k = 0; k < schemaTable.Rows.Count; k++)
					{	// save in array fro page reload
						itemNames[k] = schemaTable.Rows[k]["PARAMETER_NAME"].ToString();
					}
					schemaTable.Dispose();
				}
				catch (Exception er) 
				{
					LogException(er);
					if (schemaTable != null) schemaTable.Dispose();
					if (statConn.State.ToString() == "Open") statConn.Close();
					if (statConn != null) statConn.Dispose();
					return;
				}
				finally
				{
					if (statConn.State.ToString() == "Open") statConn.Close();
					if (statConn != null) statConn.Dispose();
				}
			}
			int j = 0;
			if (ArgumentsTextBox.Rows > 0)
				if (itemNames[0].ToString() == "@RETURN_VALUE")
				{
					ArgumentsTextBox.Rows--;  // first parm @RETURN_VALUE, returned
					j = 1;
				}
			ArgumentsLabel.Text = ArgumentsTextBox.Rows.ToString() + " Parameter(s) Required";
			procParmNames = new string[ArgumentsTextBox.Rows];
			for (int k = 0; j < itemNames.Length; k++, j++)
			{	// save in array for page reload
				procParmNames[k] = itemNames[j];
				ArgumentsTextBox.Text += procParmNames[k];
				if (j < itemNames.Length-1) ArgumentsTextBox.Text += "\n";
			}
			Session["myprocparmnames"] = procParmNames;
		}

		/// <summary>
		/// Occurs when the 'Get XXXXXX' button is clicked. 
		/// Retrieves the Table, View or Procedure names and puts them into a DD ListBox.
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void ItemsButton_Click(object sender, System.EventArgs e)
		{
			queryType = TypeRadioButtonList.SelectedItem.ToString();

			// check that previous item(s) have been selected
			if (Session["myconnstr"] == null)
			{
				LogErrorMsg("Build connection string first !");
				return;
			}
			int index;
			string[] items; 

			ArgumentsTextBox.Text = "";
			connStr = ConnectTextBox.Text;		// get users changes
			Session["myconnstr"] = connStr;		// save incase they manually edited them
			ItemsDropDownList.Items.Clear();	// clear out table entries
			if (Session["myitems"] != null)
				Session["myitems"] = null;
			if (Session["myitem"] != null)
				Session["myitem"] = null;

			if (selectedDB != null && selectedDB != "")	// if there is a Catalog ...
				builtConnectionString = connStr + ";Initial Catalog=" + selectedDB;
			else	// otherwise leave alone
				builtConnectionString = connStr;

			if (CPPCheckBox.Checked == true)	// use C++ routines		
			{	
				// get DB's Quote chars
				string quote = DBGetQuote(builtConnectionString);
				if (quote.StartsWith("ERROR:"))
				{
					LogErrorMsg(quote);
					return;
				}
				quotePrefix = quote;
				quoteSuffix = quote;
				Session["myquoteprefix"] = quotePrefix;
				Session["myquotesuffix"] = quoteSuffix;

				string itemNames="";
				// call C++ DLL to get names seperated by '|'
				switch (queryType)
				{
					case "Table" :
						itemNames = DBGetTables(builtConnectionString, selectedDB,selectedSchema, SysTblsCheckBox.Checked);
						break;
					case "View" :
						itemNames = DBGetViews(builtConnectionString, selectedDB,selectedSchema,SysTblsCheckBox.Checked);
						break;
					case "Procedure" :
						itemNames = DBGetProcs(builtConnectionString, selectedDB,selectedSchema,null);
						break;
				}

				if (itemNames.StartsWith("ERROR:"))
				{
					LogErrorMsg(itemNames);
					return;
				}
				if (itemNames == "")
					items = new string[0];
				else
					items = itemNames.Split('|');
			}
			else	// use new OLE DB routines
			{
				DataTable schemaTable = null;
				string columnName = "";
				OleDbConnection statConn = new OleDbConnection(builtConnectionString);
				try 
				{
					statConn.Open();

					// get DB's Quote chars
					schemaTable = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.DbInfoLiterals,null);
					DataRow[] dr = schemaTable.Select("LiteralName LIKE 'Quote_*'");
					quotePrefix = dr[0]["LiteralValue"].ToString();
					quoteSuffix = dr[1]["LiteralValue"].ToString();
					Session["myquoteprefix"] = quotePrefix;
					Session["myquotesuffix"] = quoteSuffix;
					schemaTable.Dispose();

					// get item names
					switch (queryType)
					{
						case "Table" :
							columnName = "TABLE_NAME";
							schemaTable = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
								new object[] {selectedDB,selectedSchema,null, 
												 SysTblsCheckBox.Checked ? "SYSTEM TABLE" : "TABLE"});
							break;
						case "View" :
							columnName = "TABLE_NAME";
							schemaTable = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
								new object[] {selectedDB,selectedSchema,null,
												 SysTblsCheckBox.Checked ? "SYSTEM VIEW" : "VIEW"});
							break;
						case "Procedure" :
							columnName = "PROCEDURE_NAME";
							schemaTable = statConn.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures,
								new object[] {selectedDB,selectedSchema,null, null});
							break;
					}
					items = new string[schemaTable.Rows.Count];
					int i = 0;
					while (i < schemaTable.Rows.Count) 
					{	
						items[i] = schemaTable.Rows[i][columnName].ToString();
						i++;
					}
					schemaTable.Dispose();
				}
				catch (Exception er) 
				{
					LogException(er);
					if (schemaTable != null) schemaTable.Dispose();
					if (statConn.State.ToString() == "Open") statConn.Close();
					if (statConn != null) statConn.Dispose();
					return;
				}
				finally 
				{
					if (statConn.State.ToString() == "Open") statConn.Close();
					if (statConn != null) statConn.Dispose();
				}
			}
			ItemsDropDownList.Items.Clear();
			LBNames = new string[items.Length];
			if (items.Length == 0)
			{
				ItemsDropDownList.Items.Add("No Items found !");
				return;
			}
			// fill in Item DD listbox
			for (int i = 0; i < items.Length; i++) 
			{
				itemName = items[i];
				if (queryType == "Procedure")
				{// it item name has a semicolon, reformat it
					index = itemName.IndexOf(';');
					if (index >= 0) 
						itemName = itemName.Substring(0,index);
				}
				// it item name has a space, reformat it
				index = itemName.IndexOf(' ');
				if (index >= 0)
					itemName = quotePrefix + itemName + quotePrefix;
				ItemsDropDownList.Items.Add(itemName);
				// save item names for return to page
				LBNames[i] = itemName;
			}
			itemName = ItemsDropDownList.Items[0].ToString();
			ArgumentsTextBox.Text = "";
			procParmNames = null;
			switch (queryType)
			{
				case "Table" :
				case "View" :						
					itemName = ItemsDropDownList.SelectedItem.Text;
					SQLstatement = "SELECT * FROM " + itemName;
					ArgumentsTextBox.Text = SQLstatement;
					ArgumentsLabel.Text = "SQL Statement";
					break;
				case "Procedure" :
					GetProcedureParms();
					break;
			}
			Session["myitems"] = LBNames;
			LBNames = null;
			Session["myitemindex"] = 0;
			Session["myitem"] = itemName;
			LogTraceMsg("Done loading items into listbox");		
		}

		/// <summary>
		/// Occurs when the 'Execute' button is clicked. 
		/// Retrieves the Tables index and involks the DataWebForm page.
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void ExecuteProcButton_Click(object sender, System.EventArgs e)
		{
			// check that previous item(s) have been selected
			if (Session["myconnstr"] == null)
			{
				LogErrorMsg("Build connection string first !");
				return;
			}
			if (ItemsDropDownList.Items.Count == 0)
			{
				LogErrorMsg("Get items first !");
				return;
			}

			int index;
			itemName = ItemsDropDownList.SelectedItem.Text;
			Session["myitem"] = itemName;
			connStr = ConnectTextBox.Text;
			queryType = TypeRadioButtonList.SelectedItem.ToString();

			if (selectedDB != null && selectedDB != "")	// if there is a Catalog ...
				builtConnectionString = connStr + ";Initial Catalog=" + selectedDB;
			else
				builtConnectionString = connStr;

			if (CPPCheckBox.Checked == true) 
			{
				string[] indexes; 
				indexName = DBGetIndexes(builtConnectionString,itemName);
				if (indexName.StartsWith("ERROR:"))
				{
					LogErrorMsg(indexName);
					return;
				}
				if (indexName.Length > 0) 
				{
					indexes = indexName.Split('|');
					indexName = indexes[0];	// just take the first one
				}
				else
					indexName = "";
			}
			else
			{
				DataTable schemaTable = null;
				quotePrefix = (string)Session["myquoteprefix"];
				quoteSuffix = (string)Session["myquotesuffix"];

				// get table index (only one needed)
				OleDbConnection indexConn = new OleDbConnection(builtConnectionString); 
				try 
				{
					indexConn.Open();

					// find primary else secondary index name
					indexName = "";
					schemaTable = indexConn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys ,new object[] {selectedDB,selectedSchema,itemName});
					if (schemaTable.Rows.Count > 0) 
						indexName = schemaTable.Rows[0]["COLUMN_NAME"].ToString();
					else
						indexName = "";
					schemaTable.Dispose();
				}					
				catch (Exception er) 
				{
					LogException(er);
					if (schemaTable != null) schemaTable.Dispose();
					if (indexConn.State.ToString() == "Open") indexConn.Close();
					if (indexConn != null) indexConn.Dispose();
					return;
				}
				finally 
				{
					if (indexConn.State.ToString() == "Open") indexConn.Close();
					if (indexConn != null) indexConn.Dispose();
				}
			}

			// remove multiple keys, only need one key 'cause we keep track of individual rows
			index = indexName.IndexOf(',');
			if (index >= 0)
				indexName = indexName.Remove (index,indexName.Length - index);
			// it KEY name has a space, reformat it
			index = indexName.IndexOf(' ');
			if (index >= 0)
				indexName = quotePrefix + indexName + quoteSuffix;

			switch (queryType)
			{
				case "Table" :
				case "View" :						
					SQLstatement = ArgumentsTextBox.Text;
					Session["mySQL"] = SQLstatement;
					break;
				case "Procedure" :
					char[] delimd = {'\xd'};
					char[] delima = {'\xa'};
					procParms = ArgumentsTextBox.Text.Split(delima);
					for (index = 0; index < procParms.Length; index++)
					{
						procParms[index] = procParms[index].TrimEnd(delimd);
					}
					procParmNames = (string[])Session["myprocparmnames"];
					SQLstatement = itemName;
					Session["mySQL"] = SQLstatement;
					break;
			}

			// now display second web page
			Server.Transfer("DataWebForm.aspx");
		}
		
		// format log messages and display
		private void LogException (Exception er)
		{
			string err = er.ToString();
			MsgTextBox.Text += err;
			MsgTextBox.Text += "<br>";
		}
		// format log messages and display
		private void LogTraceMsg (string msg)
		{
#if (DEBUG_) // really (DEBUG) for VS7
			MsgTextBox.Text += msg;
			MsgTextBox.Text += "<br>";
#else
			MsgTextBox.Text ="";
#endif
		}
		// format log messages and display
		private void LogErrorMsg (string msg)
		{
			MsgTextBox.Text += msg;
			MsgTextBox.Text += "<br>";
		}

		/// <summary>
		/// Occurs when the selection changes in the Database DD ListBox. 
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DBDropDownList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (selectedDB != null && selectedDB != "")	// if wer using 'Catalog's
			{
				selectedDB = DBDropDownList.SelectedItem.Text;
				Session["mydb"] = selectedDB;
				Session["myschema"] = null;
			}
			else	// using 'Schema's
			{
				selectedSchema = DBDropDownList.SelectedItem.Text;
				Session["myschema"] = selectedSchema;
				Session["mydb"] = null;
			}
			ItemsDropDownList.Items.Clear();	// DB changed so clear out Tables DD ListBox
			ArgumentsTextBox.Text = "";				// same
			Session["mydbindex"] = DBDropDownList.SelectedIndex;		
		}
		
		/// <summary>
		/// Occurs when the selection changes in the Item DD ListBox. 
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void ItemsDropDownList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			itemName =  ItemsDropDownList.SelectedItem.Text;
			Session["myitemindex"] = ItemsDropDownList.SelectedIndex;
			Session["myitem"] = itemName;
		
			queryType = TypeRadioButtonList.SelectedItem.ToString();
			if (ItemsDropDownList.Items.Count > 0 )
			{	// may have access to DB/Schema name but not to their tables, views and Procedures
				ArgumentsTextBox.Text = "";
				switch (queryType)
				{
					case "Table" :
					case "View" :						
						itemName = ItemsDropDownList.SelectedItem.Text;
						SQLstatement = "SELECT * FROM " + itemName;
						ArgumentsTextBox.Text = SQLstatement;
						ArgumentsLabel.Text = "SQL Statement";
						break;
					case "Procedure" :
						GetProcedureParms();
						break;
				}
			}
		}
	
		/// <summary>
		/// Get The SQL Select statement.
		/// </summary>	
		public string GetSQL
		{
			get
			{
				return SQLstatement;
			}
		}
		/// <summary>
		/// Summary description for LogonWebForm.
		/// </summary>	
		public string GetItemName
		{
			get
			{
				return itemName;
			}
		}
		/// <summary>
		/// Get the database name.
		/// </summary>	
		public string GetDBName
		{
			get
			{
				return selectedDB;
			}
		}
		/// <summary>
		/// Get the connection string.
		/// </summary>
		public string GetConnectString
		{
			get
			{
				return  builtConnectionString; 
			}
		}
			
		/// <summary>
		/// Get the index name.
		/// </summary>
		public string GetIndexName
		{
			get
			{
				return indexName;
			}
		}

		/// <summary>
		/// Get the Procedure parameters.
		/// </summary>
		public string[] GetProcParms
		{
			get
			{
				return procParms;
			}
		}

		/// <summary>
		/// Get the Procedure parameter Names.
		/// </summary>
		public string[] GetProcParmNames
		{
			get
			{
				return procParmNames;
			}
		}

		/// <summary>
		/// Get char/string for special characters in names.
		/// </summary>
		public string GetQuotePrefix
		{
			get
			{
				return quotePrefix;
			}
		}
		/// <summary>
		/// Get char/string for special characters in names.
		/// </summary>
		public string GetQuoteSuffix
		{
			get
			{
				return quoteSuffix;
			}
		}
	
	}

}
