using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public class GridItem
{
    public string imgSrc = "";
    public string Title = "";
    public string LinkSrc = "";
    public string Summary = "";


    public GridItem(string _imgSrc, string _Title, string _LinkSrc, string _Summary)
    {
        this.imgSrc = _imgSrc;
        this.Title = _Title;
        this.LinkSrc = _LinkSrc;
        this.Summary = _Summary;

    }
}


public partial class FBInbox : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Request.Params["Callback"]))
        {
            return;
        }
        else
        {
            // *** Route to the Page level callback 'handler'
            this.HandleCallbacks();

            //reading request JSON parameters
            string requestJsonParam = "";
            byte[] b = new byte[Request.ContentLength];
            Request.InputStream.Read(b, 0, Request.ContentLength);
            requestJsonParam = System.Text.UTF8Encoding.UTF8.GetString(b);

            // do something with input request parameter, but nothing to do for now
        }
    }

    // Callback routing
    public void HandleCallbacks()
    {
        // *** We have an action try and match it to a handler
        switch (Request.Params["Callback"])
        {
            case "fillGrid":
                this.FillGrid();
                break;
        }

        Response.StatusCode = 500;
        Response.Write("Invalid Callback Method");
        Response.End();
    }

    public void FillGrid()
    {
        List<GridItem> lsttst = new List<GridItem>();
        lsttst.Add(new GridItem(this.ResolveUrl("~/images/img1.jpg"), "Arlen Navasartian", "http://sites.google.com/site/arlen4mysite/", "My name is Arlen Navasartian ,I put some useful codes in this site . I believe to share my knowledge with others because I learn from others. I want to thank CodeProject for this opportunity that everyone can share the knowledge with others."));
        lsttst.Add(new GridItem(this.ResolveUrl("~/images/img2.jpg"), "Name Family2", "http://www.msn.com", "This is a description about image 2 you can read more details about image 2 and read more news about image 2 by clicking the link"));
        lsttst.Add(new GridItem(this.ResolveUrl("~/images/img3.jpg"), "Name Family3", "http://www.msn.com", "This is a description about image 3 you can read more details about image 3 and read more news about image 3 by clicking the link"));
        lsttst.Add(new GridItem(this.ResolveUrl("~/images/img4.jpg"), "Name Family4", "http://www.microsoft.com", "This is a description about image 4 you can read more details about image 4 and read more news about image 4 by clicking the link"));
        lsttst.Add(new GridItem(this.ResolveUrl("~/images/img5.jpg"), "Name Family5", "http://www.test.com", "This is a description about image 5 you can read more details about image 5 and read more news about image 5 by clicking the link"));

        Response.ContentType = "application/json; charset=utf-8";
        Response.Write(Newtonsoft.Json.JavaScriptConvert.SerializeObject(lsttst));
        Response.End();
    }

}
