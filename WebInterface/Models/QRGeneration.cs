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
        public string Url { get; set; }
        public string SaveAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        public QRGeneration (string url, string saveaddress)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save("C:\\Users\\Paulina\\Pictures\\mu.jpeg");

        }



    }
}
