import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatTableModule } from '@angular/material';
import { MatFormFieldModule, MatPaginatorModule, MatButtonModule, MatListModule } from '@angular/material';
import { MatInputModule, MatBottomSheetModule } from '@angular/material';
import { MatRadioModule, MatDatepickerModule, MatNativeDateModule, MatSlideToggleModule, MatSortModule } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';

import { ResponsesTableRoutingModule } from './responses-table-routing.module';
import { ResponsesTableComponent } from './responses-table.component';
import { ExportOverviewComponent } from '../export-overview/export-overview.component';

@NgModule({
  imports: [
    CommonModule,
    ResponsesTableRoutingModule,
    MatTableModule,
    MatFormFieldModule,
    MatPaginatorModule,
    MatButtonModule,
    MatListModule,
    MatInputModule,
    MatBottomSheetModule,
    MatRadioModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatSlideToggleModule,
    FormsModule,
    ReactiveFormsModule,
    FlexLayoutModule,
    MatSortModule
    ],
  declarations: [ResponsesTableComponent, ExportOverviewComponent],
  exports: [MatSortModule],
  entryComponents: [ExportOverviewComponent]
})
export class ResponsesTableModule { }
