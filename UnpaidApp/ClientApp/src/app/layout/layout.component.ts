import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {

    //var loginStatus = localStorage.getItem("isLoggedin");
    //if (!loginStatus || loginStatus === "false") {
    //  this.router.navigate(['/login']);
    //}

  }

}
