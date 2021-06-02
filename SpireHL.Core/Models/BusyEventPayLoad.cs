namespace SpireHL.Core.Models
{
    public class BusyEventPayLoad
    {
        public bool IsBusy { get; set; }
        public string Message { get; set; }

        public BusyEventPayLoad(bool isBusy, string message = "")
        {
            IsBusy = isBusy;
            Message = message;
        }
    }
}
