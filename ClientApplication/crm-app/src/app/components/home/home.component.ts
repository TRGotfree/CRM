// tslint:disable: prefer-for-of
// tslint:disable: align
import {
  Component, OnInit, ViewChild, AfterViewInit
} from '@angular/core';
import { MenuItem } from '../../models/menuItem';
import { UserTaskService } from '../../services/userTask.service';
import { UserTaskMeta } from '../../models/userTaskMeta';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorIntl } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { TaskComponent } from '../task/task.component';
import { merge, Observable, of as observableOf } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { UserTask } from 'src/app/models/userTask';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, AfterViewInit {

  showTasksTable = false;
  countOfTasks = 10;
  menuItems: MenuItem[];
  dataSource = new MatTableDataSource([]);
  gridColumns: UserTaskMeta[];
  tasksCount = 0;
  visibleGridColumns: string[];

  @ViewChild(MatPaginator, { static: false }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild(MatPaginatorIntl, { static: false }) matPaginatorIntl: MatPaginatorIntl;

  constructor(private userTaskService: UserTaskService,
    private snackBar: MatSnackBar,
    public taskDialog: MatDialog) {
  }

  ngOnInit(): void {
    this.menuItems = [
      { title: 'Задачи', description: 'Ваши задачи требующие исполнения', icon: 'done_outline', link: '', imageLink: '' },
      { title: 'Клиенты', description: 'Список клиентов', icon: 'face', link: '', imageLink: '' },
      { title: 'Контакты', description: 'Список контактов', icon: 'contacts', link: '', imageLink: '' },
      { title: 'База знаний', description: 'Накопленные знания', icon: 'book', link: '', imageLink: '' },
    ];

  }

  ngAfterViewInit(): void {
    this.loadTasksGridColumns();
    this.loadTasks(this.countOfTasks);
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          //TO-DO: Продолжить здесь, необходимо получить отсортированный массив задач относительно индекса страницы
          this.showTasksTable = false;
          return this.userTaskService.getSortedOrFilteredTasks()
        }),
        map(res => {

          return res.data;
         }),
      catchError(() => {
        this.showTasksTable = true;
        return observableOf([]);
      }))
      .subscribe(res => {

      }, error => {

      });
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
        this.tasksCount = res.total ? res.total : 0;
        this.dataSource.sort = this.sort;

        this.dataSource.paginator = this.paginator;

      }, error => {
        this.snackBar.open('Произошла ошибка во время получения списка задач!', 'OK', { duration: 3000 });
      });
    } catch (error) {
      this.snackBar.open('Произошла ошибка во время получения метаданных списка задач!', 'OK', { duration: 3000 });
    }
  }

  newTask(): void {
    const newTaskDialog = this.taskDialog.open(TaskComponent, { width: '40%', height: '45%', data: {} });
    newTaskDialog.afterClosed().subscribe(newTaskData => {
      if (!newTaskData) {
        return;
      }

    }, error => {

    });
  }

}
