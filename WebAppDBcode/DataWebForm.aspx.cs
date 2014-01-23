using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;


namespace WebAppDB
{
	/// <summary>
	/// Summary description for DataWebForm.
	/// </summary>
	public class DataWebForm : System.Web.UI.Page
	{
		private OleDbDataAdapter oleDbDataAdapter;
		private OleDbDataAdapter oleDbInsertDataAdapter;
		private string indexName;
		private DataSet dataSet;
		private DataSet insertDataSet;
		private string connect;
		private string dbName;
		private string item;
		private string[] procParms, procParmNames;
		private string SQLstatement;
		private int insertTableColumnOffset = 1;	// to account for insert button column
		private int dbTableColumnOffset = 2;		// to account for update & delete button columns

		/// <value>Reference for/to calling web page.</value>
		public LogonWebForm sourcepage;
		/// <value>Main Data Grid.</value>
		protected System.Web.UI.WebControls.DataGrid DataGridDB;
		/// <value>Label1.</value>
		protected System.Web.UI.WebControls.Label Label1;
		/// <value>New Table Link Button.</value>
		protected System.Web.UI.WebControls.LinkButton NewTableLinkButton;
		/// <value>Insert data Grid.</value>
		protected System.Web.UI.WebControls.DataGrid InsertDataGrid;
		/// <value>HIDDEN error msg box.</value>
		protected System.Web.UI.WebControls.TextBox MsgTextBox;
		/// <value>SQL query text box.</value>
		protected System.Web.UI.WebControls.TextBox SQLTextBox;
	
		/// <summary>
		/// Occurs when the page loads and when posts are received from the server. 
		/// </summary>
		/// <param name="sender">The raiser of this event is the system.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void Page_Load(object sender, System.EventArgs e)
		{
			// set up number of rows per web page
			DataGridDB.AllowPaging = true;
			DataGridDB.PagerStyle.Mode = PagerMode.NumericPages;
			DataGridDB.PagerStyle.PageButtonCount = 10;	// # of selections before "..."
			DataGridDB.PageSize = 25;
			InsertDataGrid.AllowPaging = false;
			InsertDataGrid.PagerStyle.Mode = PagerMode.NumericPages;
			InsertDataGrid.PagerStyle.PageButtonCount = 1;	// # of selections before "..."
			InsertDataGrid.PageSize = 1;

			if (!IsPostBack) 
			{
				MsgTextBox.Text = "";
				// get reference to calling page to get variables
				LogonWebForm sourcepage = (LogonWebForm) Context.Handler;
				// retrieve Session variables
				Session["myitem"] = item = sourcepage.GetItemName;
				Session["myconnect"] = connect = sourcepage.GetConnectString;
				Session["myindex"] = indexName = sourcepage.GetIndexName;
				Session["mydbName"] = dbName = sourcepage.GetDBName;
				Session["mySQL"] = SQLstatement = sourcepage.GetSQL;
				procParms = sourcepage.GetProcParms;
				procParmNames = sourcepage.GetProcParmNames;
				SQLTextBox.Text = SQLstatement;
				Session["mycbInsertindexes"] = null;
				Session["mycbIndexes"] = null;

				if (Session["mydataset"] != null)
				{
					dataSet = (DataSet)Session["mydataset"];
					dataSet.Dispose();
				}
				if (Session["mydataadapter"] != null)
				{
					oleDbDataAdapter = (OleDbDataAdapter)Session["mydataadapter"];
					oleDbDataAdapter.Dispose();
				}
				if (Session["myinsertdataset"] != null)
				{
					insertDataSet = (DataSet)Session["mydataset"];
					insertDataSet.Dispose();
				}
				if (Session["myinsertdataadapter"] != null)
				{
					oleDbInsertDataAdapter = (OleDbDataAdapter)Session["myinsertdataadapter"];
					oleDbInsertDataAdapter.Dispose();
				}

				// normal init

				try 
				{
					oleDbDataAdapter = new OleDbDataAdapter((SQLstatement), connect);
					OleDbCommandBuilder custCB = new OleDbCommandBuilder(oleDbDataAdapter);
					// set up for spaces in names
					custCB.QuotePrefix = sourcepage.GetQuotePrefix;
					custCB.QuoteSuffix = sourcepage.GetQuoteSuffix;
					// create and set dynamically Insert, Update & Delete commands !!
					if (indexName != null && indexName != "")
					{
						oleDbDataAdapter.InsertCommand = custCB.GetInsertCommand();
						oleDbDataAdapter.UpdateCommand = custCB.GetUpdateCommand();
						oleDbDataAdapter.DeleteCommand = custCB.GetDeleteCommand();
					}

					Session["mydataadapter"] = oleDbDataAdapter;
					dataSet = new DataSet();
					if (procParmNames != null && procParmNames.Length > 0)
					{
						oleDbDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
						for (int i = 0; i < procParms.Length; i++)
						{
							oleDbDataAdapter.SelectCommand.Parameters.Add(procParmNames[i].ToString(),procParms[i].ToString());
						}

					}
					Session["mydataset"] = dataSet;
					oleDbDataAdapter.Fill(dataSet);
//					dataSet.DefaultViewManager.DataViewSettings[0].Sort = indexName.ToString();
					// setup datagrid parms
					DataGridDB.DataSource = dataSet;
					DataGridDB.DataKeyField = indexName;
					DataGridDB.Columns[0].HeaderText = dbName;
					DataGridDB.Columns[1].HeaderText = item;
					// bind data to grid
					DataGridDB.DataBind();

					if (indexName != null && indexName != "")
					{
						// build convelueted query that works with most DB's
						// returns no data but gets column names !
						// 'Select * from Categories where 1=0
						// 'Select * from Categories where 1=0 and State = 'CA'
						string insertSQL = SQLstatement.ToUpper();
						int index = insertSQL.IndexOf(" WHERE ");
						if (index < 0) 
						{	// no where statement but need to place after table name
							index = SQLstatement.IndexOf(item);
							insertSQL = SQLstatement.Insert(index+item.Length, " where 1=0 ");
						}
						else
							insertSQL = SQLstatement.Insert(index+7, "1=0 and ");

						// get the database column/format for insert datagrid
						oleDbInsertDataAdapter = new OleDbDataAdapter(insertSQL, connect);
						// create and set dynamically insert command !!
						Session["myinsertdataadapter"] = oleDbInsertDataAdapter;
						insertDataSet = new DataSet();
						oleDbInsertDataAdapter.Fill(insertDataSet);

						// build blank entry for insert
						DataRow myRow = insertDataSet.Tables[0].NewRow();
						insertDataSet.Tables[0].Rows.Add(myRow);
						insertDataSet.AcceptChanges();
						Session["myinsertdataset"] = insertDataSet;
						// update insert grid
						InsertDataGrid.DataSource = insertDataSet;
						InsertDataGrid.DataKeyField = indexName;
						InsertDataGrid.Columns[0].HeaderText = item;
						InsertDataGrid.EditItemIndex = 0;
						// bind data to grid
						InsertDataGrid.DataBind();
					}
				}
				catch (Exception er) 
				{
					LogErrorMsg ("Exception on initialization");
					LogException(er);
				}
			}
			else	// it's a post back
			{
				LogTraceMsg("Post Back");
				oleDbDataAdapter = (OleDbDataAdapter)Session["mydataadapter"];
				dataSet = (DataSet)Session["mydataset"];
				DataGridDB.DataSource = dataSet;
			}
			
		}


		#region Web Form Designer generated code
		/// <summary>
		/// Summary description for OnInit.
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
			this.DataGridDB.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGridDB_ItemCreated);
			this.DataGridDB.PageIndexChanged += new System.Web.UI.WebControls.DataGridPageChangedEventHandler(this.DataGridDB_PageIndexChanged);
			this.DataGridDB.CancelCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridDB_CancelCommand);
			this.DataGridDB.EditCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridDB_EditCommand);
			this.DataGridDB.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.DataGridDB_SortCommand);
			this.DataGridDB.UpdateCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridDB_UpdateCommand);
			this.DataGridDB.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.DataGridDB_DeleteCommand);
			this.NewTableLinkButton.Click += new System.EventHandler(this.NewTableLinkButton_Click);
			this.InsertDataGrid.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.InsertDataGrid_ItemCreated);
			this.InsertDataGrid.DeleteCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.InsertDataGrid_InsertCommand);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
		/// <summary>
		/// Occurs when the 'New Table' link is clicked. 
		/// </summary>
		/// <param name="sender">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void NewTableLinkButton_Click(object sender, System.EventArgs e)
		{	// go back to first page
			Server.Transfer ("LogonWebForm.aspx");
		
		}


		/// <summary>
		/// Occurs when a 'Update' button is clicked on the DataGrid. 
		/// </summary>
		/// <remarks>Taken and modified from sample code.
		/// </remarks>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_UpdateCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string categoryField, fieldType;
			// find the offset into the dataset based on what page were on and the row index
			int dsItemIndexOffset = (DataGridDB.CurrentPageIndex * DataGridDB.PageSize) + e.Item.ItemIndex;

			// Gets the value of the key field of the row being updated
			string key = DataGridDB.DataKeys[e.Item.ItemIndex].ToString();

			// Gets the value of the controls (textboxes) that the user
			// updated. The DataGrid columns are exposed as the Cells collection.
			// Each cell has a collection of controls. In this case, there is only one 
			// control in each cell -- a TextBox control. To get its value,
			// you copy the TextBox to a local instance.
			//
			// The first column -- Cells(0) -- contains the Update and Cancel buttons.
			// The second column -- Cells(1) -- contains the Delete button.
			TextBox tb;

			// get original row from the view for update
			DataRow dr = dataSet.DefaultViewManager.DataSet.Tables[0].DefaultView[dsItemIndexOffset].Row;
			// check type, looking for checkbox
			for (int i = dbTableColumnOffset; i < e.Item.Cells.Count; i++)
			{
				string c = e.Item.Controls[i].Controls[0].GetType().ToString();
				if (c == "System.Web.UI.WebControls.TextBox") 
				{
					tb = (TextBox)(e.Item.Cells[i].Controls[0]);
					categoryField = tb.Text;
				}
				else if (c == "System.Web.UI.WebControls.CheckBox")
				{
					bool b = ((CheckBox)(e.Item.Cells[i].Controls[0])).Checked;
					if (b)
						categoryField = "True";
					else
						categoryField = "False";
				}
				else
				{
					LogErrorMsg("Unkown field type in Grid");	// shouldn't happen
					return;
				}
					
				// check if there is a non supported field (better allow NULLS), 
				// if so, skip over it in the DataRow
				do 
				{	// loop until a supported row is found
					fieldType = dataSet.Tables[0].Columns[i-dbTableColumnOffset].DataType.ToString();
					if (fieldType != "System.Byte[]" && fieldType != "System.Object" && fieldType != "System.Guid")
						break;
				} while(++i < dr.Table.Columns.Count+dbTableColumnOffset);

				// update only if field has changed
				if (!dataSet.Tables[0].Rows[dsItemIndexOffset].ItemArray[i-dbTableColumnOffset].Equals(categoryField))
						dr[i-dbTableColumnOffset] = categoryField;
			}

			if (dataSet.HasChanges())	// then do update to DataSet & DB
			{
				try 
				{
					oleDbDataAdapter.Update(dataSet);
					dataSet.AcceptChanges();
				}
				catch (Exception er) 
				{
					LogException(er);
					dataSet.RejectChanges();
				}
				Session["mydataset"] = dataSet;
				Session["mydataadapter"] = oleDbDataAdapter;
			}

			// Takes the DataGrid row out of editing mode
			DataGridDB.EditItemIndex = -1;

			// Refreshes the grid
			DataGridDB.DataBind();
		}
		
		/// <summary>
		/// Occurs when the 'Insert' button is clicked on the insertDataGrid. 
		/// </summary>
		/// <remarks>Taken and modified from sample code.
		/// </remarks>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void InsertDataGrid_InsertCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			string categoryField, fieldType;
			TextBox tb;

			insertDataSet = (DataSet)Session["myinsertdataset"];
			dataSet = (DataSet)Session["mydataset"];
			oleDbInsertDataAdapter = (OleDbDataAdapter)Session["myinsertdataadapter"];

			try 
			{	// make a new data row from the main dataset, trickey !
				DataRow dr = dataSet.Tables[0].NewRow();
				// check type, looking for checkbox
				for (int i = insertTableColumnOffset; i < e.Item.Cells.Count; i++)
				{
					string c = e.Item.Controls[i].Controls[0].GetType().ToString();
					if (c == "System.Web.UI.WebControls.TextBox") 
					{
						tb = (TextBox)(e.Item.Cells[i].Controls[0]);
						categoryField = tb.Text;
					}
					else if (c == "System.Web.UI.WebControls.CheckBox")
					{
						bool b = ((CheckBox)(e.Item.Cells[i].Controls[0])).Checked;
						if (b)
							categoryField = "True";
						else
							categoryField = "False";
					}
					else
					{
						LogErrorMsg("Unkown field type in InsertGrid");	// shouldn't happen
						return;
					}

					// check if there is a non supported field (better allow NULLS), if so skip over it in the DataRow
					do 
					{	// loop until a supported row is found
						fieldType = insertDataSet.Tables[0].Columns[i-insertTableColumnOffset].DataType.ToString();
						if (fieldType != "System.Byte[]" && fieldType != "System.Object" && fieldType != "System.Guid")
							break;
					} while(++i < dr.Table.Columns.Count+insertTableColumnOffset);

					// isert data into row
					dr[i-insertTableColumnOffset] = categoryField;
				}
				if (e.Item.Cells.Count > 1)
				{	// then do update to DataSet & DB
					dataSet.Tables[0].Rows.Add(dr);
					oleDbDataAdapter.Update(dataSet);
					dataSet.AcceptChanges();
					dataSet.Clear();
					oleDbDataAdapter.Fill(dataSet);
					DataGridDB.DataBind();

					// clear out inserted data from grid
					insertDataSet.Clear();
					oleDbInsertDataAdapter.Fill(insertDataSet);
					// build blank entry for insert
					DataRow myRow = insertDataSet.Tables[0].NewRow();
					insertDataSet.Tables[0].Rows.Add(myRow);
					insertDataSet.AcceptChanges();
					InsertDataGrid.DataSource = insertDataSet;
					InsertDataGrid.DataBind();
				}
			}
			catch (Exception er) 
			{
				LogException(er);
				insertDataSet.RejectChanges();
			}
			Session["myinsertdataset"] = insertDataSet;
			Session["myinsertdataadapter"] = oleDbInsertDataAdapter;
			Session["mydataadapter"] = oleDbDataAdapter;
		}

		/// <summary>
		/// Occurs when a 'Edit' button is clicked on the DataGrid. 
		/// </summary>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_EditCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			indexName = (string)Session["myindex"];
			if (indexName == null || indexName == "")
				return;	// no index so edits allowed
			DataGridDB.EditItemIndex = e.Item.ItemIndex;
			DataGridDB.DataBind();	
		}

		/// <summary>
		/// Occurs when a 'Update' button is clicked on the DataGrid. 
		/// </summary>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_CancelCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			DataGridDB.EditItemIndex = -1;
			DataGridDB.DataBind();
		
		}

		/// <summary>
		/// Occurs when the page is changed. 
		/// </summary>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_PageIndexChanged(object source, System.Web.UI.WebControls.DataGridPageChangedEventArgs e)
		{	// moving to a new page of data
			DataGridDB.CurrentPageIndex = e.NewPageIndex;
			DataGridDB.DataBind();	
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
#if (_DEBUG) // in VS7 it's now 'DEBUG'
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
		/// Occurs when a 'Delete' button is clicked on the DataGrid. 
		/// </summary>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_DeleteCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			indexName = (string)Session["myindex"];
			if (indexName == null || indexName == "")
				return;	// no index so deletes allowed
			// find the offset into the dataset based on what page were on and the row index
			int dsItemIndexOffset = (DataGridDB.CurrentPageIndex * DataGridDB.PageSize) + e.Item.ItemIndex;
			DataRow dr = dataSet.DefaultViewManager.DataSet.Tables[0].DefaultView[dsItemIndexOffset].Row;
			dr.Delete();	// it's gone
			try 
			{
				oleDbDataAdapter.Update(dataSet);
				dataSet.AcceptChanges();
			}
			catch (Exception er) 
			{
				LogException(er);
				dataSet.RejectChanges();
			}
			Session["mydataset"] = dataSet;
			Session["mydataadapter"] = oleDbDataAdapter;
	
			// Refreshes the grid
			DataGridDB.DataBind();		
		}

		/// <summary>
		/// Occurs when an item is created on the insertDataGrid. 
		/// </summary>
		/// <param name="sender">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void InsertDataGrid_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.EditItem ) // checkbox only on edit
			{
				// after an edit is completed or canceled, get called with no DataItem's
				// found that a dummy CheckBox inserted at the same offset keeps
				// everyone happy ???
				if (e.Item.DataItem == null)	
				{				
					DataSet ds = (DataSet)Session["myinsertdataset"];
					int i = 0;
					do 
					{	// if there is a boolean field in dataset, add to Controls
						if (ds.Tables[0].Columns[i].DataType.ToString() == "System.Boolean")
						{
							CheckBox cb = new CheckBox();
							e.Item.Controls[i+insertTableColumnOffset].Controls.Add(cb);
							e.Item.Controls[i+insertTableColumnOffset].Controls.RemoveAt(0);
						}
					} while(++i < ds.Tables[0].Columns.Count);
				}
				else	// normal
				{
					for (int i = 0; i < e.Item.Controls.Count-insertTableColumnOffset; i++)
					{
						try
						{
							string itemType = dataSet.DefaultViewManager.DataViewSettings[0].Table.Columns[i].DataType.ToString();
							if (itemType == "System.Boolean")
							{
								CheckBox cb = new CheckBox();
								e.Item.Controls[i+insertTableColumnOffset].Controls.Add(cb);
								e.Item.Controls[i+insertTableColumnOffset].Controls.RemoveAt(0);
							}
						}
						catch (Exception er) 
						{
							LogException(er);
						}
					}
				}
			}
		}

		/// <summary>
		/// Occurs when an item is created on the DataGrid. 
		/// </summary>
		/// <param name="sender">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.EditItem)	// checkbox only on edit
			{ 
				// after an edit is completed or canceled, get called with no DataItem's
				// found that a dummy CheckBox inserted at the same offset keeps
				// everyone happy ???
				if (e.Item.DataItem == null)	
				{			
					DataSet ds = (DataSet)Session["mydataset"];
					int i = 0;
					do 
					{	// if there is a boolean field in dataset, add to Controls
						if (ds.Tables[0].Columns[i].DataType.ToString() == "System.Boolean")
						{
							CheckBox cb = new CheckBox();
							// set Column name with data field name for bind event
							cb.ID  = ds.Tables[0].Columns[i].ColumnName;
							e.Item.Controls[i+dbTableColumnOffset].Controls.Add(cb);
							e.Item.Controls[i+dbTableColumnOffset].Controls.RemoveAt(0);
						}
					} while(++i < ds.Tables[0].Columns.Count);
				}
				else	// normal
				{
					for (int i = 0; i < e.Item.Controls.Count-dbTableColumnOffset; i++)
					{
						try
						{
							string itemType = dataSet.DefaultViewManager.DataViewSettings[0].Table.Columns[i].DataType.ToString();
							if (itemType == "System.Boolean")
							{
								CheckBox cb = new CheckBox();
								// put data field name in ID field for identification during binddata
								cb.ID = ((DataTable)((DataView)((DataRowView)e.Item.DataItem).DataView).Table).Columns[i].ColumnName.ToString();
								cb.DataBinding += new EventHandler(this.BindCheckBoxData);
								e.Item.Controls[i+dbTableColumnOffset].Controls.Add(cb);
								e.Item.Controls[i+dbTableColumnOffset].Controls.RemoveAt(0);
							}
						}
						catch (Exception er) 
						{
							LogException(er);
						}
					}
				}
			}
		}
		/// <summary>
		/// Handler for the DataBinding event where we bind the data for a specific row 
		/// to the CheckBox.
		/// </summary>
		/// <remarks>
		/// Taken from Shaun Wildes 'Adding a CheckBox column to your DataGrid' see:
		/// http://www.codeproject.com/aspnet/DataGridCheckBox.asp.
		/// </remarks>
		/// <param name="sender">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void BindCheckBoxData(object sender, EventArgs e)
		{
			CheckBox box = (CheckBox) sender;
			DataGridItem container = (DataGridItem) box.NamingContainer;
			box.Checked = false;
			string data = ((DataRowView) container.DataItem)[box.ID].ToString();
			Type t = ((DataRowView)container.DataItem).DataView.Table.Columns[box.ID].DataType;
			if (data.Length>0)
			{
				switch (t.ToString())
				{
					case "System.Boolean":
						if (( data == "True") || (data == "true"))
						{
							box.Checked = true;
						}
						break;
					default:
						break;
				}
			}
		}

		/// <summary>
		/// Occurs when a column name is clicked on the DataGrid for sorting. 
		/// </summary>
		/// <param name="source">The raiser of the event.</param>
		/// <param name="e">A System.EventArgs that contains the event data.</param>
		private void DataGridDB_SortCommand(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
		{
			// dispose of dataset when changing sort 
			// seems to have problems otherwise, 
			// remembers old sort or else I do
			dataSet.Dispose();
			dataSet = new DataSet();
			oleDbDataAdapter.Fill(dataSet);
			dataSet.DefaultViewManager.DataViewSettings[0].Sort = e.SortExpression.ToString();
			DataGridDB.DataSource = dataSet;
			DataGridDB.DataBind();
			Session["mydataset"] = dataSet;
			Session["mydataadapter"] = oleDbDataAdapter;
		}

	
	}
}
