import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { IUnpaidNotifications } from '../../shared/models/unpaid-notifications';
import { UnpaidService } from '../../shared/services/unpaid.service';

@Component({
  selector: 'app-notifications-table',
  templateUrl: './notifications-table.component.html',
  styleUrls: ['./notifications-table.component.scss']
})
export class NotificationsTableComponent implements OnInit {
  displayedColumns = ['unpaidId', 'policyNumber', 'idNumber', 'name', 'message', 'dateAdded', 'notificationRequestId', 'notificationType', 'notificationSentStatus', 'notificationErrorMessage', 'dateNotificationSent'];
  dataSource: MatTableDataSource<IUnpaidNotifications>;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private unpaidService: UnpaidService) {
    this.unpaidService.getUnpaidNotifications().subscribe(
      (response) => {
        console.log("unpaidService.getUnpaidNotifications response", response);

        // Assign the data to the data source for the table to render
        this.dataSource = new MatTableDataSource(this.mapNotifications(response));

        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      },
      (error) => {
        console.log("unpaidService.getUnpaidNotifications error", error);
      }
    );
  }

  ngOnInit() {
  }

  private mapNotifications(response: IUnpaidNotifications[]): IUnpaidNotifications[] {
    const notifications: IUnpaidNotifications[] = [];

    if (!response) {
      return notifications;
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

  public applyColor(status: string): string {
    if (!status) {
      return "black";
    }

    switch (status) {
      case "Pending":
        return "orange";
      case "Failed":
        return "red";
      case "Success":
        return "green";
      default:
        return "black";
    }
  }

}
