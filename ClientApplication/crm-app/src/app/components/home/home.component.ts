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
import { merge, Observable, of } from 'rxjs';
import { catchError, map, startWith, switchMap } from 'rxjs/operators';
import { UserTask } from 'src/app/models/userTask';
import { User } from 'src/app/models/user';
import { error } from 'protractor';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit, AfterViewInit {

  showTasksTable = false;
  countOfTasks = 10;
  pageSize = 30;
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
    this.sort.sortChange.subscribe(() => this.paginator.pageIndex = 0);
    this.loadTasksGridColumns().then(res => this.mergePaginationAndSorting()).catch(() => this.showTasksTable = true);
  }

  loadTasksGridColumns(): Promise<any> {
    return new Promise((resolve, reject) => this.userTaskService.getUserTasksMeta().subscribe((res) => {
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

        resolve();

      } catch (error) {
        this.snackBar.open('Произошла ошибка во время получения метаданных по задачам!', 'OK', { duration: 3000 });
        reject(error);
      }
    }, error => {
      this.snackBar.open('Произошла ошибка во время получения метаданных по задачам!', 'OK', { duration: 3000 });
      reject(error);
    })
    );
  }

  loadTasks(numberOfTasks: number): void {
    try {
      const user = JSON.parse(sessionStorage.getItem('user')) as User;
      if (!user || !user.login) {
        return;
      }

      this.userTaskService.getTasks(user.login, numberOfTasks).subscribe(res => {
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

  mergePaginationAndSorting() {
    merge(this.sort.sortChange, this.paginator.page)
      .pipe(
        startWith({}),
        switchMap(() => {
          this.showTasksTable = false;
          const user = JSON.parse(sessionStorage.getItem('user')) as User;
          if (!user || !user.login) {
            return;
          }
          const sortBy = !this.sort.active ? 'id' : this.sort.active;
          const orderBy = !this.sort.direction ? 'asc' : this.sort.direction;
          const pageIndex = this.paginator.pageIndex <= 0 ? 1 : this.paginator.pageIndex;
          const to = pageIndex * this.pageSize - 1;
          const from = (to - this.pageSize) < 0 ? 0 : (to - this.pageSize);

          return this.userTaskService.getSortedOrFilteredTasks(user.login, from, to, orderBy, sortBy, '', '');
        }),
        map(res => {
          const result = res as { data: [], total: number };
          this.tasksCount = result.total ? (result.total as number) : 0;
          return result.data;
        }),
        catchError(() => {
          this.showTasksTable = true;
          this.snackBar.open('Не удалось получить данные по задачам!', 'OK', { duration: 3000 });
          return of([]);
        }))
      .subscribe(res => {
        this.showTasksTable = true;
        if (!res) {
          return;
        }
        this.dataSource = new MatTableDataSource(res);
      }, error => {
        this.showTasksTable = true;
        this.snackBar.open('Не удалось получить данные по задачам!', 'OK', { duration: 3000 });
      });
  }

  newTask(): void {
    const newTaskDialog = this.taskDialog.open(TaskComponent, { width: '700px', height: '450px', data: {} });
    newTaskDialog.afterClosed().subscribe(newTaskData => {
      if (!newTaskData) {
        return;
      }

      //TO-DO


    }, error => {

    });
  }

}
