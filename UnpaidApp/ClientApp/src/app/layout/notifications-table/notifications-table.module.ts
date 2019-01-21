import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material';
import { MatFormFieldModule, MatPaginatorModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { MatRadioModule, MatDatepickerModule, MatNativeDateModule, MatSlideToggleModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

import { NotificationsTableRoutingModule } from './notifications-table-routing.module';
import { NotificationsTableComponent } from './notifications-table.component';

@NgModule({
  imports: [
    CommonModule,
    NotificationsTableRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    MatTableModule,
    MatFormFieldModule,
    MatPaginatorModule,
    MatInputModule,
    MatRadioModule,        
    MatSlideToggleModule,
    MatDatepickerModule,
    MatNativeDateModule,
    FlexLayoutModule
  ],
  declarations: [NotificationsTableComponent]
})
export class NotificationsTableModule { }
