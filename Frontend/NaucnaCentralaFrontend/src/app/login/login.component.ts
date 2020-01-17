import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  logovanjeForma: FormGroup;
  
  constructor(private httpClient: HttpClient, private router: Router) 
  { 
  }

  ngOnInit() {
    this.logovanjeForma = new FormGroup({
      'korisnickoIme': new FormControl(null, Validators.required),
      'lozinka': new FormControl(null, Validators.required)
    })
  }

  potvrdi(){
    this.httpClient.post('https://localhost:44372/api/korisnik/uloguj-se', this.logovanjeForma.value, { responseType: 'text' }).subscribe(
      (res) => {
        localStorage.setItem('jwt', res.toString());

        this.httpClient.get('https://localhost:44372/api/korisnik/uloga', { responseType: 'text' }).subscribe(
        (res1) => {
          localStorage.setItem('role', res1.toString())
          this.router.navigate(['pocetna']);
        },
        (err) => {
          alert("Neuspesno logovanje.")
        }
        )   
      },
      (error) => {
        alert("Neuspesno logovanje.")
      }
    )    
  }

}
