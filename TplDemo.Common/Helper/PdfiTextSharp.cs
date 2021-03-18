using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TplDemo.Common.Helper
{
    /// <summary>生成pdf</summary>
    public class PdfiTextSharp
    {
        /// <summary>多个图片合并成Pdf</summary>
        /// <param name="filesimg">图片流集合</param>
        /// <returns></returns>
        public byte[] imgPdfcreate(List<Byte[]> filesimg)
        {
            MemoryStream outputStream = new MemoryStream();
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(document, outputStream);
                document.Open();
                iTextSharp.text.Image image;
                for (int i = 0; i < filesimg.Count; i++)
                {
                    image = iTextSharp.text.Image.GetInstance(filesimg[i]);
                    image.SetAbsolutePosition(0, 0);//'设置图片的位置在0.0
                                                    // image.ScaleAbsolute(PageSize.A4);
                                                    // image.ScaleAbsolute(PageSize.A4); image.ScaleAbsolute(PageSize.A4);//'设置图片大小为A4纸大小
                    document.NewPage();
                    document.Add(image);
                }
                document.Close();
                outputStream.Close();
                return outputStream.ToArray();
            }
            catch (Exception ex)
            {
                document.Close();
                outputStream.Close();
                return null;
            }
        }
    }
}