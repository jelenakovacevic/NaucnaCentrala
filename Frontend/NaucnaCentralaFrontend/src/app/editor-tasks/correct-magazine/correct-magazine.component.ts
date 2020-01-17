import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-correct-magazine',
  templateUrl: './correct-magazine.component.html',
  styleUrls: ['./correct-magazine.component.css']
})
export class CorrectMagazineComponent implements OnInit {
  korigujCasopisForma: FormGroup;
  id: number;
  casopis = null;

  constructor(private httpClient: HttpClient, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.inicijalizujFormu();
      }
    )
  }

  inicijalizujFormu() {
    this.httpClient.get(`https://localhost:44372/api/casopis/dobavi-casopis/${this.id}`).subscribe(
      (res) => {
        this.casopis = res;
        this.korigujCasopisForma = new FormGroup({
          'issnBroj': new FormControl(this.casopis.issNbroj, Validators.required),
          'openAccess': new FormControl(this.casopis.openAccess, null)
        })
      },
      (error) => {        
      }
    )
  }

  potvrdi() {
    this.httpClient.post(`https://localhost:44372/api/casopis/koriguj/${this.id}`, this.korigujCasopisForma.value).subscribe(
      (res) => {
        alert('Uspesno korigovani casopis podaci.');
        this.router.navigate(['pocetna']);
      },
      (err) => {
        alert('Neuspesno korigovani casopis podaci.');
      }
    )
  }

}
