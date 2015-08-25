using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Threading;
using System.Configuration;
public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void upload_Click(object sender, EventArgs e)
    {
        //Waits for 1 Second till the Handler.ashx writes the File to the sourcePath
        Thread.Sleep(1000);
        string a = txtFileName.Text;
        string sourcePath = @"C:\Users\Chidambaram_M01.ITLINFOSYS\Downloads\DD_Folder1\";
        string targetPath = @"C:\Users\Chidambaram_M01.ITLINFOSYS\Downloads\DD_Folder2\";

        // Use Path class to manipulate file and directory paths. 
        string sourceFile = System.IO.Path.Combine(sourcePath, a);
        string destFile = System.IO.Path.Combine(targetPath, a);

        // To copy a file to another location and  
        // overwrite the destination file if it already exists.
        System.IO.File.Copy(sourceFile, destFile, true);
    }
    [WebMethod]
    public static string validempmail(string fileName, string company)
    {
        bool res = false;
        //Int32.TryParse(json.Company,out company);
        int system = 0;
        //Int32.TryParse(json.System,out system);


        String documentType = string.Empty;
        String additionalInformation = string.Empty;
        String ordNo = string.Empty;
        String distributor = string.Empty;
        String branch = string.Empty;
        String poNumber = string.Empty;


        if (!fileName.Equals(""))
        {
            try
            {

                ordNo = fileName.Substring(0, fileName.IndexOf("."));
                if (ordNo.Contains("-") || (ordNo.Contains("_")))
                {
                    ordNo = ordNo.Replace("_", "-");
                    String[] ordData;
                    ordData = ordNo.Split('-');
                    ordNo = ordData[0];
                    if (ordData.Length > 1 && !ordData[1].Trim().Equals(""))
                    {
                        documentType = Helper.getStringSql(string.Format("SELECT DOCTYPE FROM DOORGRP.W_DOCTYPES WHERE TRIM(DOCCODE) = '{0}'", ordData[1].ToUpper().Trim()), ConfigurationManager.ConnectionStrings["CurriesOleDB"].ConnectionString);
                    }
                    else
                    {
                        documentType = "Miscellaneous";
                    }
                    if (ordData.Length >= 3)
                    {
                        additionalInformation = ordData[2];
                    }
                }

                if (!ordNo.Equals(""))
                {
                    clsOrderData odm = new clsOrderData();
                    odm.orderNumber = ordNo.ToUpper();
                    if (company.Equals("0") && system == 0)
                    {
                        odm = Helper.getCurriesOrderData(odm, ConfigurationManager.ConnectionStrings["CurriesOleDB"].ConnectionString);
                    }
                    else if (company.Equals("1") && system == 0)
                    {
                        odm = Helper.getGrahamOrderData(odm, ConfigurationManager.ConnectionStrings["CurriesOleDB"].ConnectionString);
                    }
                    else if (company.Equals("2") && system == 0)
                    {
                        odm = Helper.getCecoOrderData(odm, ConfigurationManager.ConnectionStrings["CurriesOleDB"].ConnectionString);
                    }
                    else if (company.Equals("3") && system == 0)
                    {
                        odm = Helper.getFrameworksOrderData(odm, ConfigurationManager.ConnectionStrings["CurriesOleDB"].ConnectionString);
                    }
                    if (odm.distNumber != null && !odm.distNumber.Trim().Equals(""))
                    {
                        distributor = odm.distNumber;
                        branch = odm.branch;
                        poNumber = odm.poNumber;
                        String OrderStatus = odm.status;
                        res = true;
                        //btnUpload.Enabled = true;
                    }
                    else
                    {
                        //txtOrdNo.BackColor = Color.Red;
                        //btnUpload.Enabled = false;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        //Adding comma and Sending the Response to JavaScript (AJAX)
		string Output = ordNo;
        Output = Output + ",";
        Output = Output + distributor;
        Output = Output + ",";
        Output = Output + branch;
        Output = Output + ",";
        Output = Output + poNumber;
        Output = Output + ",";
        Output = Output + documentType;
        Output = Output + ",";
        Output = Output + additionalInformation;
        Output = Output + ",";
        Output = Output + res.ToString();

        return Output;


        //string a = "1,2,3,4";
        //return a;
    }
}
