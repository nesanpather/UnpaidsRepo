export interface IUnpaidNotificationsResponse {
  unpaidId: number;
  policyNumber: string;
  idNumber: string;
  dateAdded: string;
  notificationRequestId: number;
  notificationType: string;
  dateNotificationSent: string;
  contactOptionType: string;
  accepted: boolean;
  dateNotificationResponseAdded: string;
}
