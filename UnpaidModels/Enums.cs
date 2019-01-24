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

    public enum ContactOption
    {
        CallMe = 1,
        EmailMe
    }

    public enum DateType
    {
        DateAdded = 1,
        DateNotificationSent,
        DateNotificationResponseAdded,        
    }

    public enum LoggingEvents
    {
        EntryMethod = 10,
        LeaveMethod,
        GenerateItems = 1000,
        ListItems,
        GetItem,
        InsertItem,
        UpdateItem,
        DeleteItem,
        ValidationFailed = 3000,
        GetItemNotFound = 4000,
        UpdateItemNotFound,
        ExternalCall = 5000
    }
}
