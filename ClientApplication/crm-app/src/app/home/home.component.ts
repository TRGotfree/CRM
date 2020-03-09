
import { Component, OnInit, ChangeDetectionStrategy, ViewChild, AfterViewInit } from '@angular/core';
import { MenuItem } from '../models/menuItem';
import { UserTask } from '../models/userTask';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit, AfterViewInit {

  menuItems: MenuItem[];
  userTasks: UserTask[];
  gridColumns = [
    { prop: 'id', header: 'Id задачи' },
    { prop: 'userTaskType', header: 'Тип задачи' },
    { prop: 'description', header: 'Описание' },
    { prop: 'userTaskState', header: 'Состояние' },
    { prop: 'priority', header: 'Приоритет' },
    { prop: 'executorUser', header: 'Исполнитель' },
    { prop: 'taskManagerUser', header: 'Постановщик' },
    { prop: 'executeTaskUntilDate', header: 'Дата завершения' },
    { prop: 'isPayloadExists', header: 'Файл' }
  ];

  visibleGridColumns = this.gridColumns.map(p => p.prop);

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(private httpClient: HttpClient) { }

  ngOnInit(): void {
    this.menuItems = [
      { title: 'Задачи', description: 'Ваши задачи требующие исполнения', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', description: 'Список клиентов', icon: 'face', link: '', imageLink: '' },
      { title: 'Контакты', description: 'Список контактов', icon: 'contacts', link: '', imageLink: '' },
      { title: 'База знаний', description: 'Накопленные знания', icon: 'book', link: '', imageLink: '' },
    ];
  }

  ngAfterViewInit(): void {
      this.userTasks = [] as UserTask[];
  }

}
