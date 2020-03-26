import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { MenuItem } from '../../models/menuItem';
import { User } from '../../models/user';
import { Router } from '@angular/router';
import { HomeComponent } from '../home/home.component';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MenuComponent implements OnInit {

  constructor(private router: Router) {
  }

  @Input() menuItems: MenuItem[];
  sideBarOpened = false;
  isHomePage = true;
  user: User;

  ngOnInit() {
    const config = this.router.config;
    const homeRoute = config.find(route => route.component === HomeComponent);
    this.isHomePage = `/${homeRoute.path}` === this.router.url;
    const userJsonData = sessionStorage.getItem('user');
    this.user = JSON.parse(userJsonData);
  }

}
