import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material';
import { MatFormFieldModule, MatPaginatorModule } from '@angular/material';
import { MatInputModule } from '@angular/material';

import { NotificationsTableRoutingModule } from './notifications-table-routing.module';
import { NotificationsTableComponent } from './notifications-table.component';

@NgModule({
  imports: [
    CommonModule,
    NotificationsTableRoutingModule,
    MatTableModule,
    MatFormFieldModule,
    MatPaginatorModule,
    MatInputModule
  ],
  declarations: [NotificationsTableComponent]
})
export class NotificationsTableModule { }
