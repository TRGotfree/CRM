import { Input, Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MenuItem } from '../../models/menuItem';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ToolbarComponent implements OnInit {

  constructor() { }

  @Input() menuItems: MenuItem[];

  ngOnInit() {
  }

}
