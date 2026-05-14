import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse, RegisterRequest, AuthUserViewModel, ChangePasswordRequest } from '../models/auth.model';
import { ApiResponse } from '../models/notification.model';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private url = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  login(request: LoginRequest): Observable<ApiResponse<LoginResponse>> {
    return this.http.post<ApiResponse<LoginResponse>>(`${this.url}/login`, request).pipe(
      tap(res => {
        if (res.success && res.data) {
          localStorage.setItem('token', res.data.token);
          localStorage.setItem('role', res.data.role);
          localStorage.setItem('name', res.data.name);
          localStorage.setItem('userId', String(res.data.userId));
          localStorage.setItem('email', res.data.email);
        }
      })
    );
  }

  register(request: RegisterRequest): Observable<ApiResponse<AuthUserViewModel>> {
    return this.http.post<ApiResponse<AuthUserViewModel>>(`${this.url}/register`, request);
  }

  getAllUsers(): Observable<ApiResponse<AuthUserViewModel[]>> {
    return this.http.get<ApiResponse<AuthUserViewModel[]>>(`${this.url}/users`);
  }

  activateUser(id: number): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.url}/users/${id}/activate`, {});
  }

  deactivateUser(id: number): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.url}/users/${id}/deactivate`, {});
  }

  changePassword(request: ChangePasswordRequest): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.url}/change-password`, request);
  }

  logout(): void { localStorage.clear(); }

  getToken(): string | null { return localStorage.getItem('token'); }
  getRole(): string | null { return localStorage.getItem('role'); }
  getName(): string | null { return localStorage.getItem('name'); }
  getUserId(): string | null { return localStorage.getItem('userId'); }
  isLoggedIn(): boolean { return !!this.getToken(); }
}
