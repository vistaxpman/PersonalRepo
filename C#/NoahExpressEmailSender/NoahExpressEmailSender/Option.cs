namespace NoahExpressEmailSender
{
    public class Option
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string SenderAddress { get; set; }

        public string SenderPassword { get; set; }

        public string Subject { get; set; }

        public string BodyFile { get; set; }

        public string Attachments { get; set; }

        public string EmailAddressFile { get; set; }
    }
}