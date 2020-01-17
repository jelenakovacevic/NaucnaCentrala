import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, FormArray, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-add-reviewers',
  templateUrl: './add-reviewers.component.html',
  styleUrls: ['./add-reviewers.component.css']
})
export class AddReviewersComponent implements OnInit {
  dodajRecenzentaForma: FormGroup;
  listaRecenzenata = null;
  listaUrednika = null;
  id: number;

  constructor(private httpClient: HttpClient, private route: ActivatedRoute, private router: Router) { }

  ngOnInit() {
    this.route.params.subscribe(
      (params: Params) => {
        this.id = +params['id'];
        this.inicijalizujFormu();
      }
    )
  }

  inicijalizujFormu() {
    let listaRecenzenata = new FormArray([]);
    listaRecenzenata.push(new FormGroup({
      'imeRecenzenta': new FormControl(null, Validators.required)
    }));

    listaRecenzenata.push(new FormGroup({
      'imeRecenzenta': new FormControl(null, Validators.required)
    }));

    let listaUrednika = new FormArray([]);

    this.dodajRecenzentaForma = new FormGroup({
      'recenzenti': listaRecenzenata,
      'urednici' : listaUrednika
    })

    this.httpClient.get(`https://localhost:44372/api/korisnik/urednici/${this.id}`).subscribe(
        (res) => 
        {
            this.listaUrednika = res;
        },
        (err) => 
        {
        }
    )

    this.httpClient.get(`https://localhost:44372/api/korisnik/recenzenti/${this.id}`).subscribe(
      (res) => 
      {
          this.listaRecenzenata = res;
      },
      (err) => 
      {
      }
    )
  }

  dodajRecenzenta() {
    (<FormArray>this.dodajRecenzentaForma.get('recenzenti')).push(
      new FormGroup({
        'imeRecenzenta': new FormControl(null, Validators.required)
      })
    );
  }

  obrisiRecenzenta(index: number) {
    (<FormArray>this.dodajRecenzentaForma.get('recenzenti')).removeAt(index);
  }

  dodajUrednika() {
    (<FormArray>this.dodajRecenzentaForma.get('urednici')).push(
      new FormGroup({
        'imeUrednika': new FormControl(null, Validators.required)
      })
    );
  }

  obrisiUrednika(index: number) {
    (<FormArray>this.dodajRecenzentaForma.get('urednici')).removeAt(index);
  }

  
  potvrdi() {
    this.httpClient.post(`https://localhost:44372/api/casopis/dodavanje-recenzenata-i-urednika/${this.id}`, this.dodajRecenzentaForma.value).subscribe(
      (res) => 
      {
          alert('Uspesno dodati urednici i recenzenti.');
          this.router.navigate(['pocetna']);
      },
      (err) => 
      {
        alert('Neuspesno dodati urednici i recenzenti.');
      }
  );
  }
}
