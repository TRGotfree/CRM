import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { MenuItem } from '../models/menuItem';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class MenuComponent implements OnInit {

  constructor() { }

  @Input() menuItems: MenuItem[];
  sideBarOpened = false;

  ngOnInit() {
  }

}
