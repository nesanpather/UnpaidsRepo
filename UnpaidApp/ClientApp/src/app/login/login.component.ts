import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UnpaidService } from '../shared/services/unpaid.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private router: Router, private unpaidService: UnpaidService) { }

  ngOnInit() { }

  onLogin() {

    this.unpaidService.authenticateUser().subscribe(
      (response) => {
        console.log("unpaidService.authenticateUser response", response);

      },
      (error) => {
        console.log("unpaidService.authenticateUser error", error);
      }
    );

    localStorage.setItem('isLoggedin', 'true');
    this.router.navigate(['/dashboard']);
  }

}
