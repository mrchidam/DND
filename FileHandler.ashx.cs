using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DragAndDrop_GenericHandler
{
    /// <summary>
    /// Summary description for FileHandler
    /// </summary>
    public class FileHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int flag = 0; //To check whether File Uploaded Successfully
            string filePath = null;
            if (context.Request.Files.Count > 0)
            {
                HttpFileCollection files = context.Request.Files;
                foreach (string fileName in files)
                {
                    HttpPostedFile file = files[fileName];
                    filePath = "D:\\DragAndDrop\\" + fileName;
                    file.SaveAs(filePath);
                    flag = 1;
                }
            }
            context.Response.ContentType = "text/plain";
            if(flag == 1)
                context.Response.Write("File Uploaded Successfully to the Path: " + filePath);
            else
                context.Response.Write("File was not Uploaded!");
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
