
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
      { title: 'Задачи', description: 'Ваши задачи требующие исполнения', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', description: 'Список клиентов', icon: 'face', link: '', imageLink: '' },
      { title: 'Контакты', description: 'Список контактов', icon: 'contacts', link: '', imageLink: '' },
      { title: 'База знаний', description: 'Накопленные знания', icon: 'book', link: '', imageLink: '' },
      { title: 'Настройки', description: 'Настройки CRM', icon: 'settings', link: '/settings', imageLink: '' }
    ];

  }

}
