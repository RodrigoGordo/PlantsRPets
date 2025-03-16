import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserInfo } from './authorize.dto';
import { UserProfile } from './models/user-profile';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeService {
  private _authStateChanged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.hasToken());

  constructor(private http: HttpClient) { }

  public onStateChanged(): Observable<boolean> {
    return this._authStateChanged.asObservable();
  }

  private hasToken(): boolean {
    return !!localStorage.getItem('authToken');
  }

  private saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  private clearToken(): void {
    localStorage.removeItem('authToken');
  }

  public signIn(email: string, password: string): Observable<boolean> {
    return this.http.post<{ token: string }>('/api/signin', { email, password }).pipe(
      map((response) => {
        if (response && response.token) {
          this.saveToken(response.token);
          this._authStateChanged.next(true);
          return true;
        }
        return false;
      }),
      catchError(() => {
        return of(false);
      })
    );
  }

  public register(nickname: string, email: string, password: string): Observable<boolean> {
    return this.http.post('api/signup', {
      Nickname: nickname,
      Email: email,
      Password: password
    }, { observe: 'response' }).pipe(
      map((res) => res.ok),
      catchError(() => of(false))
    );
  }

  public signOut(): void {
    this.clearToken();
    this._authStateChanged.next(false);
  }

  public isSignedIn(): boolean {
    return this.hasToken();
  }

  public user(): Observable<UserInfo> {
    return this.http.get<UserInfo>('/manage/info', {
      withCredentials: true
    }).pipe(
      catchError((_: HttpErrorResponse, __: Observable<UserInfo>) => {
        return of({} as UserInfo);
      }));
  }

  public getUserInfo(): Observable<UserInfo> {
    const token = localStorage.getItem('authToken');
    if (!token) {
      return of({} as UserInfo);
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<UserInfo>('/api/userinfo', { headers }).pipe(
      catchError(() => of({} as UserInfo))
    );
  }

  public getUserProfile(): Observable<UserProfile> {
    const token = localStorage.getItem('authToken');
    if (!token) {
      return of({ nickname: null, profile: null });
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<UserProfile>('/api/user-profile', { headers }).pipe(
      catchError(() => of({ nickname: null, profile: null }))
    );
  }
}
