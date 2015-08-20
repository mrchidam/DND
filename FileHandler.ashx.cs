using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DragAndDrop_GenericHandler
{
    <summary>
    This is the Server Side Program which receives a file from the Client and Save it in the Specified filePath.
    </summary>
    public class FileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                int flag = 0; //To check whether File Uploaded Successfully
                string filePath = null;
                if (context.Request.Files.Count > 0)
                {
                    HttpFileCollection files = context.Request.Files;
                    foreach (string fileName in files)
                    {
                        HttpPostedFile file = files[fileName];
                        filePath = @"D:\DragAndDrop\" + fileName;
                        file.SaveAs(filePath);
                        flag = 1;
                    }
                }
                if (flag == 1)
                    context.Response.Write("File Uploaded Successfully to the Path: " + filePath);
                else
                    context.Response.Write("File was not Uploaded!");
            }
            catch(Exception ex)
            {
                context.Response.Write("Exception: " + ex.Message);
                context.Response.Write(" Contact Your System Administrator to resolve this issue!");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
