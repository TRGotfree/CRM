// tslint:disable: prefer-for-of
// tslint:disable: align
import {
  Component, OnInit, ChangeDetectionStrategy, Inject, AfterViewInit
} from '@angular/core';

import { UserTask } from '../../models/userTask';
import { Priority } from '../../models/priority';
import { ExecutorUser } from '../../models/executorUser';
import { UserTaskService } from '../../services/userTask.service';
import { UserTaskTypeService } from '../../services/userTaskType.service';
import { PriorityService } from '../../services/priority.service';
import { UserService } from '../../services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserTaskType } from '../../models/userTaskType';
import { UserTaskTypeComponent } from '../usertasktype/usertasktype.component';
import { User } from '../../models/user';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit, AfterViewInit {

  constructor(public dialogRef: MatDialogRef<TaskComponent>,
    @Inject(MAT_DIALOG_DATA) public userTask: UserTask,
    public userTaskTypeDialog: MatDialog,
    private userTaskService: UserTaskService,
    private userTaskTypeService: UserTaskTypeService,
    private priorityService: PriorityService,
    private userService: UserService,
    private snackBar: MatSnackBar) { }

  userTaskFormGroup: FormGroup;
  taskTypes: UserTaskType[] = [];
  priorities: Priority[] = [];
  executorUsers: ExecutorUser[] = [];
  caption = (this.userTask && this.userTask.id > 0) ? 'Задача №' + this.userTask.id : 'Новая задача';
  taskTypeControl = new FormControl(this.userTask.userTaskType, [Validators.required]);
  priorityControl = new FormControl(this.userTask.priority, [Validators.required]);
  executorUserControl = new FormControl(this.userTask.executorUser, [Validators.required]);
  executionDateControl = new FormControl(this.userTask.executeTaskUntilDate, [Validators.required]);
  descriptionControl = new FormControl(this.userTask.description);
  additinalFileControl = new FormControl();

  //validateMessage: string;

  ngOnInit(): void {

    this.userTaskFormGroup =
      new FormGroup(
        {
          userTaskType: this.taskTypeControl,
          priority: this.priorityControl,
          executorUser: this.executorUserControl,
          executionDate: this.executionDateControl,
          description: this.descriptionControl,
          additinalFile: this.additinalFileControl
        });

    this.userTaskTypeService.getTaskTypes().subscribe(res => {
      if (!res || !res.data) {
        this.snackBar.open('Произошла ошибка во время получения данных по типам задач!', 'OK', { duration: 3000 });
        return;
      }
      this.taskTypes = res.data;
    }, error => {
      this.snackBar.open('Произошла ошибка во время получения данных по типам задач!', 'OK', { duration: 3000 });
    });

    this.priorityService.getPriorities().subscribe(res => {
      if (!res || !res.data) {
        this.snackBar.open('Произошла ошибка во время получения данных по приоритетам задач!', 'OK', { duration: 3000 });
        return;
      }

      this.priorities = res.data;
    }, error => {
      this.snackBar.open('Произошла ошибка во время получения данных по приоритетам задач!', 'OK', { duration: 3000 });
    });

    this.userService.getExecutorUsers().subscribe(res => {
      if (!res || !res.data) {
        this.snackBar.open('Произошла ошибка во время получения данных по исполнителям задач!', 'OK', { duration: 3000 });
        return;
      }

      this.executorUsers = res.data;
    }, error => {
      this.snackBar.open('Произошла ошибка во время получения данных по исполнителям задач!', 'OK', { duration: 3000 });
    });
  }

  ngAfterViewInit(): void {

  }

  addTaskType(): void {
    const taskTypeDialog = this.userTaskTypeDialog.open(UserTaskTypeComponent, { width: '400px', height: '250px', data: {} });
    taskTypeDialog.afterClosed().subscribe(newTaskType => {
      if (!newTaskType) {
        return;
      }
      this.taskTypes.push(newTaskType);
      this.taskTypeControl.setValue(newTaskType.name);
    });
  }

  save(data: any): void {

    if (!this.userTask) { return; }

    if (this.taskTypeControl.invalid) {
      return;
    }

    if (this.priorityControl.invalid) {
      return;
    }

    if (this.executorUserControl.invalid) {
      return;
    }

    if (this.executionDateControl.invalid) {
      return;
    }

    this.userTask.executeTaskUntilDate = this.executionDateControl.value.format();

    const currentUser = JSON.parse(sessionStorage.getItem('user')) as User;
    if (!currentUser) {
      this.snackBar.open('Не удалось сохранить задачу! Не удается получить текущего пользователя!', 'OK', { duration: 3000 });
      return;
    }

    this.userTask.taskManagerUserLogin = currentUser.login;

    this.userTaskService.saveTask(this.userTask).subscribe(res => {
      if (!res || !res.data) {
        this.snackBar.open('Не удалось сохранить задачу!', 'OK', { duration: 3000 });
        return;
      }

      this.dialogRef.close(res.data);
    }, error => {
      this.snackBar.open('Не удалось сохранить задачу!', 'OK', { duration: 3000 });
    });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
