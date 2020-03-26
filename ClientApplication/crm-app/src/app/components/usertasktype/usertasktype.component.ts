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
import { FormControl } from '@angular/forms';
import { UserTaskType } from '../../models/userTaskType';

@Component({
    selector: 'app-task-type',
    templateUrl: './usertasktype.component.html',
    styleUrls: ['./usertasktype.component.css']
})
export class UserTaskTypeComponent implements OnInit {

    constructor(public dialogRef: MatDialogRef<UserTaskTypeComponent>,
        @Inject(MAT_DIALOG_DATA) public userTaskType: UserTaskType,
        private userTaskTypeService: UserTaskTypeService,
        private snackBar: MatSnackBar) { }

    caption = 'Новый тип задачи';

    ngOnInit(): void {
    }

    save(): void {
        if (!this.userTaskType.name) {
            this.snackBar.open('Укажите название типа задачи!', 'OK', { duration: 3000 });
            return;
        }

        if (!this.userTaskType.id) {
            this.userTaskType.id = -1;
            this.userTaskType.isDeleted = false;
        }

        this.userTaskTypeService.saveTaskTypeService(this.userTaskType).subscribe(res => {
            this.dialogRef.close(res.data);
        }, error => {
            this.snackBar.open('Не удалось сохранить тип задачи!', 'OK', { duration: 3000 });
        });
    }

    cancel(): void {
        this.dialogRef.close();
    }
}
