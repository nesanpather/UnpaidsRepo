namespace UnpaidModels
{
    public enum Status
    {
        Pending = 1,
        Success,
        Failed
    }

    public enum Notification
    {
        Push = 1,
        Sms,
        Email,
        Call
    }

    public enum Response
    {
        CallMe = 1,
        EmailMe
    }
}
