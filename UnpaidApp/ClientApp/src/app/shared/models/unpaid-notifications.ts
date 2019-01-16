export interface IUnpaidNotifications {
  unpaidId: number;
  policyNumber: string;
  idNumber: string;
  name: string;
  message: string;
  dateAdded: string;
  notificationRequestId: number;
  notificationType: string;
  notificationSentStatus: string;
  notificationErrorMessage: string;
  dateNotificationSent: string;
}
