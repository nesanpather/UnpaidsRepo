export interface IUnpaidNotificationsResponse {
  policyNumber: string;
  idNumber: string;
  dateAdded: string;
  notificationType: string;
  correlationId: string;
  dateNotificationSent: string;
  contactOptionType: string;
  accepted: boolean;
  dateNotificationResponseAdded: string;  
}
