import { Component, OnInit } from '@angular/core';
import { MenuItem } from '../models/menuItem';

@Component({
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor() { }

  menuItems: MenuItem[];

  ngOnInit() {
    this.menuItems = [
      { title: 'Справочники', icon: 'book', description: 'Редактирование справочников', imageLink: '', link: '/dictionary' }
    ];
  }

}
