import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatBottomSheet } from '@angular/material';

import { ExportOverviewComponent } from '../export-overview/export-overview.component';
import { IUnpaidNotificationsResponse } from '../../shared/models/unpaid-notifications-response';
import { UnpaidService } from '../../shared/services/unpaid.service';

@Component({
  selector: 'app-responses-table',
  templateUrl: './responses-table.component.html',
  styleUrls: ['./responses-table.component.scss']
})
export class ResponsesTableComponent implements OnInit {
  displayedColumns = ['unpaidId', 'policyNumber', 'idNumber', 'dateAdded', 'notificationRequestId', 'notificationType', 'dateNotificationSent', 'contactOptionType', 'accepted', 'dateNotificationResponseAdded'];
  dataSource: MatTableDataSource<IUnpaidNotificationsResponse>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private unpaidService: UnpaidService, private bottomSheet: MatBottomSheet) {
    this.unpaidService.getUnpaidNotificationResponses().subscribe(
      (response) => {
        console.log("unpaidService.getUnpaidNotificationResponses response", response);

        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.mapNotificationResponses(response));

        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      (error) => {
        console.log("unpaidService.getUnpaidNotificationResponses error", error);
      }
    );
  }

  ngOnInit() {
  }

  private mapNotificationResponses(response: IUnpaidNotificationsResponse[]): IUnpaidNotificationsResponse[] {
    const notificationResponses: IUnpaidNotificationsResponse[] = [];

    if (!response) {
      return notificationResponses;
    }

    return response;
  }

  public applyFilter(filterValue: string) {
    filterValue = filterValue.trim(); // Remove whitespace
    filterValue = filterValue.toLowerCase(); // Datasource defaults to lowercase matches
    this.dataSource.filter = filterValue;

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  public mapContactType(contactType: boolean): string {
    if (contactType) {
      return "Yes";
    }

    return "No";
  }

  public openExportOptions(): void {
    this.bottomSheet.open(ExportOverviewComponent);
  }

}
