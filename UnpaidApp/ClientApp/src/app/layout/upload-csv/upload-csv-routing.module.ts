import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UploadCsvComponent } from './upload-csv.component';

const routes: Routes = [
  {
    path: '',
    component: UploadCsvComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UploadCsvRoutingModule { }
