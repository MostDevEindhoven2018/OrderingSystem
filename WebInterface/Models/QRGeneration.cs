using System;
using System.Collections.Generic;
using System.DrawingCore;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;

namespace WebInterface.Models
{
    public class QRGeneration
    {
        public string Table_Number { get; set; }

        public List<int> TableNum { get; set; } 
        //public string Directory { get; set; }

        /// <summary>
        /// Generate QR JPEG from provided URL
        /// Saves it to the path provided by the USER
        /// </summary>
        /// <param name="table"></param>
        public QRGeneration(string table)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("http://localhost:52892/menucard/addguest?tableno=" + table, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            qrCodeImage.Save("C:\\Users\\Paulina\\Pictures\\"+"QRTable_"+ table +".jpeg");
        }



    }
}
