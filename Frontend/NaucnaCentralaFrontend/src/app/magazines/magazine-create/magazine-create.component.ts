import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-magazine-create',
  templateUrl: './magazine-create.component.html',
  styleUrls: ['./magazine-create.component.css']
})
export class MagazineCreateComponent implements OnInit {
  kreirajCasopisForma: FormGroup;
  listaNaucnihOblasti = ['Biofizika', 'Matematicka ekonomija', 'Nuklearna fizika', 'Kinematika', 'Internet marketing']
  
  constructor(private httpClient: HttpClient, private router: Router) { }

  ngOnInit() {
    this.inicijalizujFormu();
  }

  inicijalizujFormu() {
    let naucneOblasti = new FormArray([]);
    naucneOblasti.push(new FormGroup({
      'naziv': new FormControl(null, Validators.required)
    }));

    this.kreirajCasopisForma = new FormGroup({
      'naziv': new FormControl(null, Validators.required),
      'issnBroj': new FormControl(null, Validators.required),
      'openAccess': new FormControl(false),
      'naucneOblasti': naucneOblasti
    })
  }

  dodajNaucnuOblast() {
    (<FormArray>this.kreirajCasopisForma.get('naucneOblasti')).push(
      new FormGroup({
        'naziv': new FormControl(null, Validators.required)
      })
    );
  }

  obrisiNaucnuOblast(index: number) {
    (<FormArray>this.kreirajCasopisForma.get('naucneOblasti')).removeAt(index);
  }

  potvrdi()
  {
    this.httpClient.post('https://localhost:44372/api/casopis', this.kreirajCasopisForma.value).subscribe(
      (res) => {
        alert('Uspesno kreiran casopis.')
        this.router.navigate(['pocetna']);
      },
      (error) => {
        this.router.navigate(['pocetna']);
        alert('Neuspesno kreiran casopis.')
      }
    )
  }


}
