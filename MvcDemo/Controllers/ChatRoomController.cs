using MvcDemo.Hubs;
using MvcDemo.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace MvcDemo.Controllers
{
    public class ChatRoomController : Controller
    {

        public ActionResult ChatAppli()
        {
            return View();
        }

        // GET: /ChatRoom/
        public ActionResult SignalRChat()
        {
            return View();
        }
        [HttpPost]
        public ActionResult export()
        {
            ChatHistoryEntities1 context = new ChatHistoryEntities1();
            List<ChatHistory> FileData = context.ChatHistories.ToList();
            //var res2 = context.ChatHistories.ToArray();
            //var res3 = context.ds.Tables;
            try
            {
                //foreach (var item in FileData[0].) { }
                DataTable Dt = new DataTable();
                Dt.Columns.Add("From Adress", typeof(string));
                //Dt.Columns.Add("From Adress", typeof(string));
                Dt.Columns.Add("To Adress", typeof(string));
                Dt.Columns.Add("Messages", typeof(string));
                Dt.Columns.Add("Created Date", typeof(string));
                foreach (var data in FileData)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = data.FromAddress;
                    row[1] = data.ToAddress;
                    row[2] = data.Messages;
                    row[3] = data.CreatedDate;
                    Dt.Rows.Add(row);                    
                }

                var memoryStream = new MemoryStream();
                // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;
                    //using (ExcelRange Rng = worksheet.Cells["A1"])
                    //{
                    //    Rng.Merge = true;
                    //    Rng.Style.Font.Bold = true;
                    //    Rng.Style.Font.Color.SetColor(Color.Red);
                    //    Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    //    Rng.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    //}
                    worksheet.DefaultRowHeight = 18;
                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(2).AutoFit();
                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public ActionResult export2( string TableName )
        {
            try
            {
                ChatHub chat = new ChatHub();
                var result = chat.getDataTables(TableName);
                DataTable dt = result.Tables[0];             
                var memoryStream = new MemoryStream();
                    // ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var excelPackage = new ExcelPackage(memoryStream))
                    {
                        var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                        worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.None);
                        worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                        worksheet.DefaultRowHeight = 18;
                        worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.DefaultColWidth = 20;
                        worksheet.Column(2).AutoFit();
                        Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                        return Json("", JsonRequestBehavior.AllowGet);
                    }
            }
            catch (Exception ex)
            {
                return Json(1);
                //Console.WriteLine(ex.Message);
                //throw;               
            }
        }

        public ActionResult Download()
        {
            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", "ChatHistory.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult Download2()
        {
            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", "ChatHistory.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}


