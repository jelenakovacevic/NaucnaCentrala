import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registracijaForma: FormGroup;
   listaNaucnihOblasti = ['Biofizika', 'Matematicka ekonomija', 'Nuklearna fizika', 'Kinematika', 'Internet marketing']

  constructor(private httpClient: HttpClient, private router: Router) { }

  ngOnInit() {
    this.inicijalizujFormu();
  }

  inicijalizujFormu() {
    let registerScientificAreas = new FormArray([]);
    registerScientificAreas.push(new FormGroup({
      'naziv': new FormControl(null, Validators.required)
    }));

    this.registracijaForma = new FormGroup({
      'korisnickoIme': new FormControl(null, Validators.required),
      'lozinka': new FormControl(null, Validators.required),
      'ime': new FormControl(null, Validators.required),
      'prezime': new FormControl(null, Validators.required),
      'grad': new FormControl(null, Validators.required),
      'drzava': new FormControl(null, Validators.required),
      'titula': new FormControl(),
      'recenzent': new FormControl(false),
      'naucneOblasti': registerScientificAreas
    })
  }

  dodajNaucnuOblast() {
    (<FormArray>this.registracijaForma.get('naucneOblasti')).push(
      new FormGroup({
        'naziv': new FormControl(null, Validators.required)
      })
    );
  }

  obrisiNaucnuOblast(index: number) {
    (<FormArray>this.registracijaForma.get('naucneOblasti')).removeAt(index);
  }

  potvrdi()
  {
    this.httpClient.post('https://localhost:44372/api/korisnik/registruj-se', this.registracijaForma.value).subscribe(
      (res) => {
        alert('Uspesno ste uneli podatke. Potvrdite email.')
        this.router.navigate(['pocetna']);
      },
      (error) => {
        alert('Neuspesna registracija.');
      }
    )
  }

}
