// tslint:disable: prefer-for-of
import {
    Component, OnInit, ChangeDetectionStrategy, ViewChild, AfterViewInit, Input,
    ChangeDetectorRef
} from '@angular/core';
import { UserTask } from '../models/userTask';
import { UserTaskService } from '../services/userTask.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
    selector: 'app-task',
    templateUrl: './task.component.html',
    styleUrls: ['./task.component.css'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class TaskComponent implements OnInit {

    constructor(private userTaskService: UserTaskService, private snackBar: MatSnackBar) {}

    @Input() userTask: UserTask;

    ngOnInit(): void {

    }
}
