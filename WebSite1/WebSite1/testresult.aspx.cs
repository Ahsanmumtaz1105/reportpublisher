using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HtmlAgilityPack;
using System.Configuration;

public partial class testresult : System.Web.UI.Page
{
    public string virtualDirectory =  ConfigurationManager.AppSettings["http://10.66.38.158:8081/testreports/"].ToString();
    public string basedirectory = ConfigurationManager.AppSettings["basedirectory"].ToString();
    public string resultFilePrefix = ConfigurationManager.AppSettings["ResultFilePrefix"].ToString();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            string event_called = Page.Request.Params["__EVENTTARGET"];
            if (!event_called.Contains('f'))
            {
                CreateControl(false);
                ViewState["folder"] = event_called;
                

            }
            else if (event_called.Contains('f'))
            {
                CreateControl(true);
                ViewState["file"] = event_called;
            }
        }
        else
        {
            CreateControl(false);
        }
    }

    private void CreateControl(bool file)
    {
        List<string> collectionString = new List<string>();
        collectionString = GetFolderName();
        foreach (string item in collectionString) 
        {
            //create instance of LinkButton
            LinkButton lb = new LinkButton();
            lb.Font.Name = "Calibri";
            lb.Text = item + "<br />"; //LinkButton Text
            lb.ID = item; // LinkButton ID’s
            lb.CommandArgument = item; //LinkButton CommandArgument
            lb.CommandName = item; //LinkButton CommanName
            PlaceHolder1.Controls.Add(lb); // Adding the LinkButton in PlaceHolder
            lb.Click += lb_Command;
        }
        if (file)
        {
            string item = ViewState["folder"].ToString();
            createReportLinks(item);
        }
    }

    void createReportLinks(string folder)
    {
        List<string> reportFiles = new List<string>();
        List<string[]> fileDetails = new List<string[]>();
        fileDetails = GetReportFiles(folder);
        Table tblResults = createTable(fileDetails);
        PlaceHolder2.Controls.Add(tblResults);
    }

    void lb_Command(object sender, EventArgs e)
    {
        List<string> reportFiles = new List<string>();
        LinkButton lnk = sender as LinkButton;
        string a = ViewState["folder"].ToString();
        if (!lnk.Font.Bold)
        {
            lnk.Font.Bold = true;
            lnk.ForeColor = System.Drawing.Color.Green;
        }
        
        //if (lnk.Font.Bold == true)
        //{
        //    lnk.Font.Bold = false;
        //    lnk.ForeColor = System.Drawing.Color.Blue;
        //}
        //else
        //{
        //    lnk.Font.Bold = true;
        //    lnk.ForeColor = System.Drawing.Color.Green;
        //}
        List<string[]>fileDetails = new List<string[]>();
        fileDetails =  GetReportFiles(lnk.ID);
        Table tblResults = createTable(fileDetails);
        PlaceHolder2.Controls.Add(tblResults);
    }

    private Table createTable(List<string[]> reportFiles)
    {
        Table tableResults = new Table();
        tableResults.Attributes.Add("Style", "border:1px solid #ccc");
        TableHeaderRow hdrRow = new TableHeaderRow();
        tableResults.Rows.Add(hdrRow);
        hdrRow.Style.Add("background", "Bisque");
        hdrRow.Style.Add("border-style", "solid");
        hdrRow.Style.Add("border-width", "1px");
        TableHeaderCell hdrCell = new TableHeaderCell();
        hdrCell.Text = "Result File Name";
        hdrCell.Font.Name = "Calibri";
        hdrRow.Cells.Add(hdrCell);
        TableHeaderCell hdrCell1 = new TableHeaderCell();
        hdrCell1.Text = "Passed Test Cases";
        hdrCell1.Font.Name = "Calibri";
        hdrCell1.Style.Add("padding-left", "25px");
        hdrCell1.Style.Add("padding-right", "25px");
        hdrRow.Cells.Add(hdrCell1);
        TableHeaderCell hdrCell2 = new TableHeaderCell();
        hdrCell2.Text = "Failed Test Cases";
        hdrCell2.Font.Name = "Calibri";
        hdrCell2.Style.Add("padding-left", "25px");
        hdrCell2.Style.Add("padding-right", "25px");
        hdrRow.Cells.Add(hdrCell2);
        TableHeaderCell hdrCell3 = new TableHeaderCell();
        hdrCell3.Text = "Total Test Cases";
        hdrCell3.Font.Name = "Calibri";
        hdrCell3.Style.Add("padding-left", "25px");
        hdrCell3.Style.Add("padding-right", "25px");
        hdrRow.Cells.Add(hdrCell3);

        for (int i = 0; i <=reportFiles.Count-1; i++)
        {
            TableRow tr = new TableRow();
            //tr.Style.Add("border-style", "solid");
            
            for (int j = 0; j <=reportFiles[i].Length-1; j++)
            {
                TableCell tblCell = new TableCell();
                tableResults.Rows.Add(tr);
                if (j == 0)
                {
                    LinkButton reportlink = new LinkButton();
                    //reportlink = new LinkButton();
                    reportlink.Text = reportFiles[i][j] + "<br />"; //LinkButton Text
                    reportlink.ID = reportFiles[i][j] + "f"; // LinkButton ID’s
                    reportlink.CommandArgument = reportFiles[i][j]; //LinkButton CommandArgument
                    reportlink.CommandName = reportFiles[i][j]; //LinkButton CommanName
                    tblCell.Controls.Add(reportlink);
                    reportlink.Click += reportlink_Command;
                    tblCell.Style.Add("padding-left", "25px");
                    tblCell.Style.Add("padding-right", "25px");
                    tblCell.Font.Name = "Calibri";
                    tblCell.BorderStyle = BorderStyle.Groove;
                    tblCell.Style.Add("border-width", "1px");
                    tr.Cells.Add(tblCell);
                }
                else
                {
                        tblCell.Text = reportFiles[i][j];
                        tr.Cells.Add(tblCell);
                        tblCell.Font.Name = "Calibri";
                        tblCell.Font.Bold = true;
                        tblCell.HorizontalAlign = HorizontalAlign.Center;
                        tblCell.VerticalAlign = VerticalAlign.Middle;
                        tblCell.BorderStyle = BorderStyle.Groove;
                        tblCell.Style.Add("border-width", "1px");
                        if(j==2 && Convert.ToInt16(reportFiles[i][2])>0)
                        {
                            tblCell.Style.Add("color", "red");
                        }
                }
            }
        }
        return tableResults; 
    }

    void reportlink_Command(object sender, EventArgs e)
    {
        LinkButton report = sender as LinkButton;
        if (report.Font.Bold == true)
        {
            report.Font.Bold = false;
            report.ForeColor = System.Drawing.Color.Blue;
        }
        else
        {
            report.ForeColor = System.Drawing.Color.DarkBlue;
        }
        string folder = ViewState["folder"].ToString();
        string filename = ViewState["file"].ToString();
        string url = virtualDirectory +    folder + @"/" + filename.Remove(filename.Length - 1) + ".html";
        //report.Attributes.Add("href", url);
        ScriptManager.RegisterStartupScript(Page, typeof(Page), "OpenWindow", "window.open('" + url + "');", true);
        //System.Diagnostics.Process.Start(url);
    }

    private List<string[]> GetReportFiles(string id)
    {

        // Get the report links
        string reportdirectory = basedirectory + id;
        List<string[]> filesName = new List<string[]>();
        DirectoryInfo info = new DirectoryInfo(reportdirectory);
        FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
        for(int i=files.Length-1; i>=0; i--)
        {
            string[] fileDetails = new string[4];
            string filename = files[i].Name;
            if (filename.StartsWith(resultFilePrefix))
            {
                fileDetails[0] = filename.Replace(".html", "");
                Tuple<string, string> resultSummary = getReportDetail(files[i]);
                fileDetails[1] = resultSummary.Item1;
                fileDetails[2] = resultSummary.Item2;
                int totalTC = Convert.ToInt16(resultSummary.Item1) + Convert.ToInt16(resultSummary.Item2);
                fileDetails[3] = Convert.ToString(totalTC);
                filesName.Add(fileDetails);
            }
        }
        return filesName;
    }

    private Tuple<string, string> getReportDetail(FileInfo fileInfo)
    {
        string startswith = "\r\nwindow.output[\"stats\"]";
        HtmlDocument htmlDoc = new HtmlDocument();
        string filepath = fileInfo.FullName;
        
        htmlDoc.Load(filepath);

        HtmlNodeCollection fileNodes = htmlDoc.DocumentNode.SelectNodes("//script[@type='text/javascript']");    
        foreach(HtmlNode node in fileNodes)
        {

            if (node.InnerText.StartsWith(startswith))
            {
                string pass = null;
                string fail = null;
                string[] rawDetails = node.InnerText.Split('{');
                string strToProcess = rawDetails[2];

                string[] result = strToProcess.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string row in result)
                {
                    if(row.ToLower().StartsWith("\"fail\""))
                    {
                        fail = row.Substring(7);
                    }

                    if (row.ToLower().StartsWith("\"pass\""))
                    {
                        pass = row.Substring(7, (row.Length - 2) - 7);

                    }
                }

                Tuple<string, string> resultSummary = new Tuple<string, string>(pass, fail);
                return resultSummary;
            }
        }
        return null;
    }

    private List<string> GetFolderName()
    {
        List<string> folderName = new List<string>();
        var di = new DirectoryInfo(basedirectory);
        var directories = di.EnumerateDirectories()
                    .OrderBy(d => d.CreationTime)
                    .Select(d => d.Name)
                    .ToList();
        List<string> dir = new List<string>();
        dir = directories;
 
        for(int i = (dir.Count-1); i>=0; i--) 
        {
            folderName.Add(dir[i]);
        }   
        return folderName;
    }
}