﻿namespace DoAnChuyenNganh.Services.EmailSettings
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public bool UseSSL { get; set; }
    }
}
