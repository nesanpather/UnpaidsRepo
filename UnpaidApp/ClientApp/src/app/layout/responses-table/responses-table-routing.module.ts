import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { ResponsesTableComponent } from './responses-table.component';

const routes: Routes = [
  {
    path: '',
    component: ResponsesTableComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ResponsesTableRoutingModule { }
