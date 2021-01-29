using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ClosedXML.Excel;
using Ionic.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Office.Interop.Excel;
using MultipartDataMediaFormatter.Infrastructure;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using SMARTII.Domain.IO;
using FreeSpire = Spire.Xls;


namespace SMARTII.Domain.Report
{
    public static class ReportUtility
    {
        public static byte[] HtmlToPDF(this string html)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                var doc = new Document(PageSize.A4);
                doc.SetMargins(0, 0, 0, 10);
                PdfDestination pdfDest = new PdfDestination(PdfDestination.XYZ, 0, doc.PageSize.Height, 1f);
                PdfWriter writer = PdfWriter.GetInstance(doc, outputStream);
                doc.Open();

                byte[] data = Encoding.UTF8.GetBytes(html); //字串轉成byte[]
                using (MemoryStream msInput = new MemoryStream(data))
                {
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msInput, null, Encoding.UTF8, new UnicodeFontHelper());
                    PdfAction action = PdfAction.GotoLocalPage(1, pdfDest, writer);
                    writer.SetOpenAction(action);
                }

                doc.Close();
                return outputStream.ToArray();
            }
        }


        public static byte[] ConvertExcelBytesToPDF(byte[] bytes)
        {
            string targetpdf = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.pdf");

            try
            {

                #region FreeSpire.xls


                using (var ms = new MemoryStream(bytes))
                using (var xlWorkbook = new XLWorkbook(ms))
                using (var workbook = new FreeSpire.Workbook())
                {

                    var ws = xlWorkbook.Worksheet(1);

                    var view = ws.SheetView;
                    ws.PageSetup.PagesWide = 1;
                    ws.PageSetup.PagesTall = 1;
                    view.ZoomScale = 80;


                    Stream stream = new MemoryStream(ConvertBookToByte(xlWorkbook));

                    workbook.LoadFromStream(stream);

                    workbook.SaveToFile(targetpdf, FreeSpire.FileFormat.PDF);
                }


                #endregion


                return File.ReadAllBytes(targetpdf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FileUtility.DeleteFile(targetpdf);
            }
        }


        public static byte[] ExcelToPDF(string excelPath)
        {
            // Excel 檔案位置
            string sourcexlsx = excelPath;
            // PDF 儲存位置
            string targetpdf = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.pdf");

            try
            {

                #region microsoft interop

                //建立 Excel application instance
                Microsoft.Office.Interop.Excel.Application appExcel = new Microsoft.Office.Interop.Excel.Application();
                //開啟 Excel 檔案
                var xlsxDocument = appExcel.Workbooks.Open(sourcexlsx);

                ((Microsoft.Office.Interop.Excel._Worksheet)
                    xlsxDocument.ActiveSheet).PageSetup.FitToPagesWide = 1;
                ((Microsoft.Office.Interop.Excel._Worksheet)
                    xlsxDocument.ActiveSheet).PageSetup.FitToPagesTall = 1;
                ((Microsoft.Office.Interop.Excel._Worksheet)
                    xlsxDocument.ActiveSheet).PageSetup.Zoom = 80;

                //匯出為 pdf
                xlsxDocument.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, targetpdf, IncludeDocProperties: true, IgnorePrintAreas: true);
                //關閉 Excel 檔
                xlsxDocument.Close();
                //結束 Excel
                appExcel.Quit();

                #endregion microsoft interop

                #region sautinsoft

                //ExcelToPdf converter = new ExcelToPdf();

                //var nomaring = new CPageMargin();
                //nomaring.mm(0);

                //var a4 = new CPageSize();
                //a4.A4();

                //converter.PageStyle = new CPageStyle()
                //{
                //    PageMarginTop = nomaring,
                //    PageMarginBottom = nomaring,
                //    PageSize = a4

                //};

                //converter.OutputFormat = SautinSoft.ExcelToPdf.eOutputFormat.Pdf;
                //converter.UnicodeOptions = new CUnicodeOptions()
                //{
                //    DetectFontsDirectory = CUnicodeOptions.eUnicodeDetectFontsDirectory.Auto
                //};
                //var file = File.ReadAllBytes(sourcexlsx);
                //try
                //{
                //    converter.ConvertByteToFile(file, targetpdf);
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //    Console.ReadLine();
                //}

                #endregion sautinsoft

                return File.ReadAllBytes(targetpdf);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                FileUtility.DeleteFile(targetpdf);
            }
        }

        public static byte[] RazorToPDF<T>(T model, string razorPath)
        {
            var template = FileUtility.GetFileText(razorPath);

            var config = new TemplateServiceConfiguration()
            {
                DisableTempFileLocking = true,
                ReferenceResolver = new ReportReferenceResolver(),
            };

            using (var service = new TemplateService(config))
            {
                var html = service.Parse(template, model, null, "presist");

                var pdf = ReportUtility.HtmlToPDF(html);

                return pdf;
            }
        }

        public static byte[] ConvertBookToByte(this XLWorkbook Workbook, string password = "")
        {
            if (string.IsNullOrEmpty(password) == false)
            {
                Workbook.Protect(password);
                //SaveAsProtectionExcel(tempFullPath, password);
            }


            Byte[] result = null;

            using (MemoryStream stream = new MemoryStream())
            {
                Workbook.SaveAs(stream);
                result = stream.ToArray();
            }


            return result;
        }

        public static void SaveAsProtectionExcel(string fileName, string password = "")
        {
            // create new excel application
            var oexcel = new Microsoft.Office.Interop.Excel.Application();
            oexcel.Application.DisplayAlerts = false;

            Workbook obook = oexcel.Application.Workbooks.Open(fileName);
            oexcel.Visible = false;

            obook.WritePassword = password;
            obook.SaveAs(fileName);

            obook.Close();
            obook = null;
            oexcel.Quit();
            oexcel = null;
        }


        public static byte[] ConvertZipByte(this byte[] @byte, string fileName, string password = null)
        {
            MemoryStream output = new MemoryStream();
            using (var zip = new ZipFile())
            {

                if (!string.IsNullOrEmpty(password))
                    zip.Password = password;

                Stream stream = new MemoryStream(@byte);
                zip.ProvisionalAlternateEncoding = Encoding.UTF8;
                zip.AddEntry(fileName, stream);
                
                zip.Save(output);
                stream.Close();
            }

            byte[] bytes = output.ToArray();
            output.Close();

            return bytes;
        }

        public static HttpFile GenerateZip(this List<HttpFile> files, string zipName, string password = null)
        {
            MemoryStream output = new MemoryStream();
            using (var zip = new ZipFile())
            {
                zip.ProvisionalAlternateEncoding = Encoding.UTF8;

                if (!string.IsNullOrEmpty(password))
                    zip.Password = password;


                files.ForEach(file =>
                {
                    zip.AddEntry(file.FileName, file.Buffer);
                });
                

                zip.Save(output);
            }

            return new HttpFile(zipName, "application/zip", output.ToArray());
        }
        
    }
}
