// tslint:disable: prefer-for-of
import {
  Component, OnInit, ChangeDetectionStrategy, ViewChild, AfterViewInit,
  OnChanges,
  ChangeDetectorRef
} from '@angular/core';
import { MenuItem } from '../models/menuItem';
import { UserTask } from '../models/userTask';
import { UserTaskService } from '../services/userTask.service';
import { UserTaskMeta } from '../models/userTaskMeta';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent implements OnInit, OnChanges, AfterViewInit {

  showTasksTable = false;
  countOfTasks = 10;
  menuItems: MenuItem[];
  dataSource: MatTableDataSource<UserTask[]>;
  gridColumns: UserTaskMeta[];

  visibleGridColumns: string[];

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: false }) sort: MatSort;

  constructor(private userTaskService: UserTaskService, private snackBar: MatSnackBar, private changesDetector: ChangeDetectorRef) {
  }

  ngOnInit(): void {
    this.menuItems = [
      { title: 'Задачи', description: 'Ваши задачи требующие исполнения', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', description: 'Список клиентов', icon: 'face', link: '', imageLink: '' },
      { title: 'Контакты', description: 'Список контактов', icon: 'contacts', link: '', imageLink: '' },
      { title: 'База знаний', description: 'Накопленные знания', icon: 'book', link: '', imageLink: '' },
    ];
    this.loadTasksGridColumns();
    this.dataSource = new MatTableDataSource();
  }

  ngOnChanges(): void {
  }

  ngAfterViewInit(): void {
  }

  loadTasksGridColumns(): void {
    this.userTaskService.getUserTasksMeta().subscribe((res) => {
      try {

        if (!res || !res.data) {
          throw new Error('Сервер вернул пустые данные!');
        }

        const data = res.data;
        this.gridColumns = [];
        this.visibleGridColumns = [];
        for (let index = 0; index < data.length; index++) {
          const column = data[index];
          this.gridColumns.push({ prop: column.Key, header: column.Value });
          this.visibleGridColumns.push(column.Key);
        }

        this.showTasksTable = true;
        this.changesDetector.markForCheck();

      } catch (error) {
        this.snackBar.open('Произошла ошибка во время получения метаданных по задачам!', 'OK', { duration: 3000 });
      }
    }, error => {
      this.snackBar.open('Произошла ошибка во время получения метаданных по задачам!', 'OK', { duration: 3000 });
    });
  }

  loadTasks(numberOfTasks: number): void {
    try {
      this.userTaskService.getTasks(numberOfTasks).subscribe(res => {

        //TO-DO: Протестировать получение задач.
        //Добавить сортировку, возможно фильтрацию
        //Добавить кнопку с возможностью указания кол-ва задач для загрузки

        this.dataSource = new MatTableDataSource(res.data);
      }, error => {
        this.snackBar.open('Произошла ошибка во время получения списка задач!', 'OK', { duration: 3000 });
      });


    } catch (error) {
      this.snackBar.open('Произошла ошибка во время получения метаданных списка задач!', 'OK', { duration: 3000 });
    }
  }
}
