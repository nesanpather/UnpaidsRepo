import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { UnpaidService } from '../../../shared/services/unpaid.service';
import { IUser } from '../../../shared/models/user';

@Component({
  selector: 'app-topnav',
  templateUrl: './topnav.component.html',
  styleUrls: ['./topnav.component.scss']
})
export class TopnavComponent implements OnInit {
  public pushRightClass: string;
  public userName: string;

  constructor(public router: Router, private unpaidService: UnpaidService) {
    this.router.events.subscribe(val => {
      if (val instanceof NavigationEnd && window.innerWidth <= 992 && this.isToggled()) {
        this.toggleSidebar();
      }
    });
  }

  ngOnInit() {
    this.pushRightClass = 'push-right';
    this.login();
  }

  isToggled(): boolean {
    const dom: Element = document.querySelector('body');
    return dom.classList.contains(this.pushRightClass);
  }

  toggleSidebar() {
    const dom: any = document.querySelector('body');
    dom.classList.toggle(this.pushRightClass);
  }

  public login() {

    this.unpaidService.authenticateUser().subscribe(
      (response) => {
        console.log("unpaidService.authenticateUser response", response);
        if (!response) {
          return;
        }

        if (response.isAuthenticated && response.userName) {
          this.userName = response.userName;
        } 
      },
      (error) => {
        console.log("unpaidService.authenticateUser error", error);
      }
    );
  }

  onLoggedout() {
    localStorage.removeItem('isLoggedin');
    this.router.navigate(['/login']);
  }
}
