import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap, map, throwError, catchError } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiRoutes } from '../../shared/api-routes';
import { ApiResponse } from '../../shared/api-response';
import { AuthTokens } from '../../models/user/authToken';


@Injectable({ providedIn: 'root' })
export class AuthService {
  private baseUrl = environment.apiBaseUrl;
  private tokenKey = '';
  private refreshKey = '';
  private tokenExpiration = '';
  private _isLoggedIn = new BehaviorSubject<boolean>(this.hasToken());

  isLoggedIn$ = this._isLoggedIn.asObservable();

  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<ApiResponse<AuthTokens>> {
    debugger
    return this.http.post<ApiResponse<AuthTokens>>(
      `${this.baseUrl}/${ApiRoutes.login}`,
      { email, password }
    ).pipe(
      map(response => {
        if (!response.sucesso) {
          throw new Error(response.mensagem || 'Falha no login');
        }
        if (response.dados?.accessToken) {
          localStorage.setItem(this.tokenKey, response.dados.accessToken);
          localStorage.setItem(this.refreshKey, response.dados.refreshToken);
          localStorage.setItem(this.tokenExpiration, response.dados.accessTokenExpiration);
          this._isLoggedIn.next(true);
        }
        return response;
      }),
      catchError(err => {
        const errorMsg = err.message || 'Erro inesperado ao comunicar com o servidor.';
        return throwError(() => new Error(errorMsg));
      })
    );
  }

  register(nome: string, email: string, password: string, confirmPassword: string): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(
      `${this.baseUrl}/${ApiRoutes.register}`,
      { email, password, confirmPassword }
    ).pipe(
      map(response => {
        if (!response.sucesso) {
          throw new Error(response.mensagem || '');
        }
        return response;
      }),
      catchError(err => {
        const errorMsg = err.message || 'Erro inesperado ao comunicar com o servidor.';
        return throwError(() => new Error(errorMsg));
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.refreshKey);
    this._isLoggedIn.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshKey);
  }

  isAuthenticated(): boolean {
    return this.hasToken();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }
}
