import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Params, Router } from '@angular/router';

@Component({
  selector: 'app-editor-task-list',
  templateUrl: './editor-task-list.component.html',
  styleUrls: ['./editor-task-list.component.css']
})
export class EditorTaskListComponent implements OnInit {
  dodajZadatakRecenzentu = null;
  korigujCasopis = null;

  constructor(private httpClient: HttpClient, private router: Router, private route: ActivatedRoute) { }

  ngOnInit() {
    this.refreshTasks();
  }

  refreshTasks() {
    this.httpClient.get('https://localhost:44372/api/casopis/dobavi-casopise-za-dodavanje-urednika-i-recenzenata').subscribe(
      (res) => {
        this.dodajZadatakRecenzentu = res;
      },
      (error) => {

      }
      
    )

    this.httpClient.get('https://localhost:44372/api/casopis/dobavi-casopis-za-korigovanje').subscribe(
      (res) => {
        this.korigujCasopis = res;
      },
      (error) => {

      }
      
    )
  }

  dodaj(id: Number) {
    this.router.navigate(['../', 'dodaj-recenzente', id], {relativeTo: this.route});
  }

  koriguj(id: Number) {
    this.router.navigate(['../', 'koriguj-casopis', id], {relativeTo: this.route});
  }

}

