// Matches NotificationViewModel.cs in NotificationService
export interface NotificationViewModel {
  notificationID: number;
  userID: string;
  title: string;
  message: string;
  category: string;
  status: string;       // Unread | Read
  createdDate: string;
  readDate?: string;
}

export interface SendNotificationRequest {
  userID: string;
  title: string;
  message: string;
  category: string;
}

export interface BroadcastNotificationRequest {
  userIDs: string[];
  title: string;
  message: string;
  category: string;
}

export interface ApiResponse<T> {
  success: boolean;
  message: string;
  data: T;
  errors?: string[];
}
