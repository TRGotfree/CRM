
import { Component, OnInit, ChangeDetectionStrategy, ViewChild, AfterViewInit } from '@angular/core';
import { MenuItem } from '../models/menuItem';
import { UserTask } from '../models/userTask';
import { UserTaskService } from '../services/userTask.service';
import { UserTaskMeta } from '../models/userTaskMeta';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit, AfterViewInit {

  menuItems: MenuItem[];
  userTasks: UserTask[];
  gridColumns: UserTaskMeta[];

  visibleGridColumns: string[];

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(private userTaskService: UserTaskService, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.menuItems = [
      { title: 'Задачи', description: 'Ваши задачи требующие исполнения', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', description: 'Список клиентов', icon: 'face', link: '', imageLink: '' },
      { title: 'Контакты', description: 'Список контактов', icon: 'contacts', link: '', imageLink: '' },
      { title: 'База знаний', description: 'Накопленные знания', icon: 'book', link: '', imageLink: '' },
    ];

    this.userTaskService.getUserTasksMeta().subscribe(data => {
      try {

        if (!data) {
          throw new Error('Сервер вернул пустые данные!');
        }

        this.gridColumns = data;
        this.visibleGridColumns = this.gridColumns.map(p => p.prop);
      } catch (error) {
        this.snackBar.open('Произошла ошибка во время получения метаданных по задачам!', 'OK', { duration: 3000 });
      }
    }, error => {
      this.snackBar.open('Произошла ошибка во время получения метаданных по задачам!', 'OK', { duration: 3000 });
    });
  }

  ngAfterViewInit(): void {
    this.userTasks = [] as UserTask[];

  }

}
