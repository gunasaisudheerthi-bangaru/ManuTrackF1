export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  role: string;
  name: string;
  userId: number;   // C# LoginResponse record has 'UserId' → JSON 'userId'
  email: string;
}

// Matches RegisterRequest.cs in AuthService — phone is required, no department
export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  role: string;
  phone: string;
}

export interface AuthUserViewModel {
  userID: number;
  name: string;
  role: string;
  email: string;
  phone?: string;
  isActive: boolean;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}
