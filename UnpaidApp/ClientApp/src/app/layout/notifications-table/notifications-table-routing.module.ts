import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NotificationsTableComponent } from './notifications-table.component';

const routes: Routes = [
  {
    path: '',
    component: NotificationsTableComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationsTableRoutingModule { }
