import { Component, OnInit } from '@angular/core';
import { AccountsService } from '../services/account.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  constructor(private accountsService: AccountsService) { }

  ngOnInit() {
  }

}
