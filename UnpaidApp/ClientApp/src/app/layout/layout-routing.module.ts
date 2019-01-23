import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { LayoutComponent } from './layout.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: '',
        redirectTo: 'notifications/sent'
      },
      //{
      //  path: 'dashboard',
      //  loadChildren: './dashboard/dashboard.module#DashboardModule'
      //},
      //{
      //  path: 'charts',
      //  loadChildren: './charts/charts.module#ChartsModule'
      //},
      //{
      //  path: 'components',
      //  loadChildren:
      //    './material-components/material-components.module#MaterialComponentsModule'
      //},
      //{
      //  path: 'forms',
      //  loadChildren: './forms/forms.module#FormsModule'
      //},
      //{
      //  path: 'grid',
      //  loadChildren: './grid/grid.module#GridModule'
      //},
      //{
      //  path: 'tables',
      //  loadChildren: './tables/tables.module#TablesModule'
      //},
      //{
      //  path: 'blank-page',
      //  loadChildren: './blank-page/blank-page.module#BlankPageModule'
      //},
      {
        path: 'add',
        loadChildren: './upload-csv/upload-csv.module#UploadCsvModule'
      },
      {
        path: 'notifications/sent',
        loadChildren: './notifications-table/notifications-table.module#NotificationsTableModule'
      },
      {
        path: 'notifications/responses',
        loadChildren: './responses-table/responses-table.module#ResponsesTableModule'
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
