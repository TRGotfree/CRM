// tslint:disable: max-line-length

import { Component, OnInit } from '@angular/core';
import { MenuItem } from '../../models/menuItem';

@Component({
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements OnInit {

  constructor() { }

  menuItems: MenuItem[];

  ngOnInit() {
    this.menuItems = [
      { title: 'Справочники', description: 'Настройка и редактирование справочников', icon: 'book', link: '/dictionary', imageLink: '' },
      { title: 'Пользователи', description: 'Настройка и редактирование пользователей', icon: 'account_box', link: '/user', imageLink: '' }
    ];
  }

}
