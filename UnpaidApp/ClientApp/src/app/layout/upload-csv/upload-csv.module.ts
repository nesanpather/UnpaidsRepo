import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FlexLayoutModule } from '@angular/flex-layout';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
  MatBottomSheetModule, MatButtonModule, MatCardModule, MatDialogModule, MatFormFieldModule,
  MatIconModule, MatInputModule, MatListModule, MatOptionModule, MatPaginatorModule,
  MatProgressBarModule, MatRadioModule, MatSelectModule, MatSliderModule, MatSnackBarModule,
  MatTooltipModule
} from '@angular/material';
import { UploadCsvRoutingModule } from './upload-csv-routing.module';
import { UploadCsvComponent } from './upload-csv.component';
import { PapaParseModule } from 'ngx-papaparse';

@NgModule({
  imports: [CommonModule, UploadCsvRoutingModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatListModule,
    MatBottomSheetModule,
    MatSnackBarModule,
    MatCardModule,
    ReactiveFormsModule,
    MatOptionModule,
    MatSelectModule,
    MatTooltipModule,
    MatRadioModule,
    MatSliderModule,
    MatProgressBarModule,
    MatPaginatorModule,
    MatIconModule,
    PapaParseModule,
    FlexLayoutModule.withConfig({ addFlexToParent: false })    ],
  declarations: [UploadCsvComponent]
})
export class UploadCsvModule { }
