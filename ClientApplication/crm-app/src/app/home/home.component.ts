import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MenuItem } from '../models/menuItem';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit {

  constructor() { }

  menuItems: MenuItem[];

  ngOnInit() {
    this.menuItems = [
      { title: 'Задачи', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', icon: 'face', link: '', imageLink: '' },
      { title: 'База знаний', icon: 'book', link: '', imageLink: '' },
      { title: 'Настройки', icon: 'settings', link: '', imageLink: '' }
    ];

  }

}
