import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {
  constructor(private httpClient: HttpClient) { }

  logOut(): void {
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
  }

  korisnikAutentifikovan(): boolean {
    return localStorage.jwt !== undefined;
  }

  getToken(): string {
    return localStorage.jwt;
  }

  dobaviUlogu(): string {
    return localStorage.role;
  }
}
