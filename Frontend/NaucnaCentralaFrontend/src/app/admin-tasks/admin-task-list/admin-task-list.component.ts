import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-task-list',
  templateUrl: './admin-task-list.component.html',
  styleUrls: ['./admin-task-list.component.css']
})
export class AdminTaskListComponent implements OnInit {
  korisnikCekaPotvrdu = null;
  casopisCekaPregled = null;

  constructor(private httpClient: HttpClient, private router: Router) { }

  ngOnInit() {
    this.refreshTasks();
  }

  refreshTasks() {
    this.httpClient.get('https://localhost:44372/api/korisnik/dobavi-korisnike-za-potvrdu').subscribe(
      (res) => {
        this.korisnikCekaPotvrdu = res;
      },
      (error) => {
        console.log('Neuspesno dobavljeni zadaci za admina.');
      }
    )

    this.httpClient.get('https://localhost:44372/api/casopis/dobavi-casopise-za-potvrdu').subscribe(
      (res) => {
        this.casopisCekaPregled = res;
      },
      (error) => {
        console.log('Neuspesno dobavljeni zadaci za admina.');
      }
    )
  }

  potvrda(id: Number) {
    this.httpClient.post(`https://localhost:44372/api/korisnik/odobri/${id}`, null).subscribe(
      (res) => {
        alert('Uspesno potvrdjen korisnik kao recenzent.');
        this.router.navigate(['pocetna']);
      },
      (err) => {
        alert('Neuspesna potvrda korisnika kao recenzenta.');
        this.router.navigate(['pocetna']);
      }
    )
  }

  potvrdaCasopisa(id: Number) {
    this.httpClient.post(`https://localhost:44372/api/casopis/odobri/${id}`, null).subscribe(
      (res) => {
        alert('Uspesno potvrdjeni casopis podaci.');
        this.router.navigate(['pocetna']);
      },
      (err) => {
        alert('Neuspesna potvrda casopis podataka.');
        this.router.navigate(['pocetna']);
      }
    )
  }

  odbijanjeCasopisa(id: Number) {
    this.httpClient.post(`https://localhost:44372/api/casopis/odbij/${id}`, null).subscribe(
      (res) => {
        alert('Uspesno odbijeni casopis podaci.');
        this.router.navigate(['pocetna']);
      },
      (err) => {
        alert('Neuspesno odbijeni casopis podaci.');
        this.router.navigate(['pocetna']);
      }
    )
  }

}
