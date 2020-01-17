import { Component, OnInit } from '@angular/core';
import { AccountsService } from '../services/account.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent implements OnInit {

  constructor(private accountsService: AccountsService, private router: Router) { }

  ngOnInit() {
  }

  izlogujSe() {
    this.accountsService.logOut();
    this.router.navigate(['pocetna']);
  }
}
