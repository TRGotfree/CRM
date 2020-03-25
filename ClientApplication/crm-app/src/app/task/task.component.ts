// tslint:disable: prefer-for-of
// tslint:disable: align
import {
    Component, OnInit, ChangeDetectionStrategy, Inject, AfterViewInit
} from '@angular/core';
import { UserTask } from '../models/userTask';
import { Priority } from '../models/priority';
import { ExecutorUser } from '../models/executorUser';
import { UserTaskService } from '../services/userTask.service';
import { UserTaskTypeService } from '../services/userTaskType.service';
import { PriorityService } from '../services/priority.service';
import { UserService } from '../services/user.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormControl } from '@angular/forms';
import { UserTaskType } from '../models/userTaskType';
import { UserTaskTypeComponent } from '../usertasktype/usertasktype.component';

@Component({
    selector: 'app-task',
    templateUrl: './task.component.html',
    styleUrls: ['./task.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
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

    taskTypes: UserTaskType[] = [];
    priorities: Priority[] = [];
    executorUsers: ExecutorUser[] = [];
    caption = 'Новая задача';

    ngOnInit(): void {
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
        const taskTypeDialog = this.userTaskTypeDialog.open(UserTaskTypeComponent, { width: '30%', height: '25%', data: {} });
        taskTypeDialog.afterClosed().subscribe(newTaskType => {
            if (!newTaskType) {
                return;
            }

        });
    }

    save(): void {
        this.userTaskService.saveTask(this.userTask);
    }

    cancel(): void {
        this.dialogRef.close();
    }
}
