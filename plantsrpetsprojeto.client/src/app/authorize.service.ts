import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { UserInfo } from './authorize.dto';
import { UserProfile } from './models/user-profile';

@Injectable({
  providedIn: 'root'
})

/**
 * Serviço responsável pela gestão de autenticação, sessão e perfil do utilizador.
 * Inclui funcionalidades como login, registo, confirmação de e-mail, gestão de token JWT
 * e operações relacionadas com o perfil (obter/atualizar).
 */
export class AuthorizeService {
  private _authStateChanged: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(this.hasToken());
  private _loginRequested = new Subject<void>();
  public loginRequested$ = this._loginRequested.asObservable();

  constructor(private http: HttpClient) { }

  /**
   * Observable que notifica alterações no estado de autenticação (login/logout).
   */
  public onStateChanged(): Observable<boolean> {
    return this._authStateChanged.asObservable();
  }

  /**
   * Verifica se existe token de autenticação armazenado localmente.
   */
  private hasToken(): boolean {
    return !!localStorage.getItem('authToken');
  }

  /**
   * Armazena o token JWT no localStorage.
   */
  private saveToken(token: string): void {
    localStorage.setItem('authToken', token);
  }

  /**
   * Remove o token JWT do localStorage.
   */
  private clearToken(): void {
    localStorage.removeItem('authToken');
  }

  /**
   * Efetua o login do utilizador através da API.
   * @param email Email do utilizador
   * @param password Palavra-passe do utilizador
   * @returns Observable que devolve true se o login foi bem-sucedido, false caso contrário
   */
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

  /**
   * Regista um novo utilizador na aplicação.
   * @param nickname Apelido/nome do utilizador
   * @param email Email
   * @param password Palavra-passe
   * @returns Observable que indica se o registo foi bem-sucedido
   */
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

  /**
   * Confirmação de e-mail após registo, utilizando token enviado por email.
   * @param email Email a confirmar
   * @param token Token de confirmação
   */
  public confirmEmail(email: string, token: string): Observable<boolean> {
    return this.http.get<{ message: string }>(
      `/api/confirm-email?email=${encodeURIComponent(email)}&token=${encodeURIComponent(token)}`
    ).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }

  /**
   * Termina sessão do utilizador.
   * Limpa o token e emite o novo estado de autenticação.
   */
  public signOut(): void {
    this.clearToken();
    this._authStateChanged.next(false);
  }

  /**
   * Notifica que foi solicitado um popup/modal de login.
   * Útil para componentes que reagem a este evento.
   */
  public requestLoginPopup(): void {
    this._loginRequested.next();
  }

  /**
   * Indica se o utilizador se encontra autenticado.
   */
  public isSignedIn(): boolean {
    return this.hasToken();
  }

  /**
   * Obtém informação básica do utilizador autenticado (ex: nome, e-mail).
   * Utiliza cookies para autenticação.
   */
  public user(): Observable<UserInfo> {
    return this.http.get<UserInfo>('/manage/info', {
      withCredentials: true
    }).pipe(
      catchError((_: HttpErrorResponse, __: Observable<UserInfo>) => {
        return of({} as UserInfo);
      }));
  }

  /**
   * Obtém informação do utilizador autenticado através do token JWT.
   * Inclui dados gerais do utilizador.
   */
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

  /**
   * Obtém o perfil completo do utilizador, incluindo bio, imagem de perfil,
   * pets favoritos e plantações em destaque.
   */
  public getUserProfile(): Observable<UserProfile> {
    const token = localStorage.getItem('authToken');
    if (!token) {
      return of({
        nickname: '',
        profile: {
          bio: '',
          profilePicture: null,
          favoritePets: [],
          highlightedPlantations: [],
          profileId: 0,
          userId: ''
        }
      });
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.get<UserProfile>('/api/user-profile', { headers }).pipe(
      catchError(() => of({
        nickname: '',
        profile: {
          bio: '',
          profilePicture: null,
          favoritePets: [],
          highlightedPlantations: [],
          profileId: 0,
          userId: ''
        }
      }))
    );
  }

  /**
   * Atualiza o perfil do utilizador (incluindo imagem de perfil e listas de favoritos).
   * Os dados são enviados como FormData para permitir upload de ficheiros.
   */
  public updateProfile(profileData: UserProfile, file: File | null): Observable<UserProfile> {
    const token = localStorage.getItem('authToken');
    if (!token) {
      return of({
        nickname: '',
        profile: {
          bio: '',
          profilePicture: null,
          favoritePets: [],
          highlightedPlantations: [],
          profileId: 0,
          userId: ''
        }
      });
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    const formData = new FormData();
    formData.append('Nickname', profileData.nickname);
    formData.append('Bio', profileData.profile.bio);
    if (file) {
      formData.append('ProfilePicture', file);
    }
    formData.append('FavoritePets', JSON.stringify(profileData.profile.favoritePets));
    formData.append('HighlightedPlantations', JSON.stringify(profileData.profile.highlightedPlantations));

    console.log("Sending profile data:", formData);

    return this.http.put<UserProfile>('/api/update-profile', formData, { headers }).pipe(
      catchError(() => of({
        nickname: '',
        profile: {
          bio: '',
          profilePicture: null,
          favoritePets: [],
          highlightedPlantations: [],
          profileId: 0,
          userId: ''
        }
      }))
    );
  }
}
