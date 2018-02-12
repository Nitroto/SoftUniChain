import { Component } from '@angular/core';
import {Router} from '@angular/router';

@Component({
  selector: 'navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent {
  constructor(private router: Router){

  }

  exit() {
    sessionStorage.clear();

    this.router.navigate(['home']);
  }
}
