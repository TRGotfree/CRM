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
      { title: 'Задачи', description: '', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', description: '', icon: 'face', link: '', imageLink: '' },
      { title: 'Контакты', description: '', icon: 'contacts', link: '', imageLink: '' },
      { title: 'База знаний', description: '', icon: 'book', link: '', imageLink: '' },
      { title: 'Настройки', description: '', icon: 'settings', link: '/settings', imageLink: 'assets/img/settings.png' }
    ];

  }

}
