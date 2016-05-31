namespace EnergyHelpline.QuotesTool.Common.Models
{
    public class Notification
    {
        public string Subject { get; set; }
        public string Payload { get; set; }
        public string AttachmentFilePath { get; set; }
        public User Recipient { get; set; }
        public string HtmlPayload { get; set; }
    }
}
