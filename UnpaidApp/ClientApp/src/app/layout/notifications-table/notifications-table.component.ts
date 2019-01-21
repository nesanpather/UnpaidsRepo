import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

import { IUnpaidNotifications } from '../../shared/models/unpaid-notifications';
import { UnpaidService } from '../../shared/services/unpaid.service';
import { HelperService } from '../../shared/services/helper.service';

@Component({
  selector: 'app-notifications-table',
  templateUrl: './notifications-table.component.html',
  styleUrls: ['./notifications-table.component.scss']
})
export class NotificationsTableComponent implements OnInit {
  displayedColumns = ['unpaidId', 'policyNumber', 'idNumber', 'name', 'message', 'dateAdded', 'notificationRequestId', 'notificationType', 'notificationSentStatus', 'notificationErrorMessage', 'dateNotificationSent'];
  dataSource: MatTableDataSource<IUnpaidNotifications>;
  dateRangeType: string = "1";
  startDate: Date;
  endDate: Date;
  showDateFilter: boolean = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private unpaidService: UnpaidService, private helperService: HelperService) {

  }

  ngOnInit() {
    this.getAllUnpaidNotifications();
  }

  private getAllUnpaidNotifications() {
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

  private mapNotifications(response: IUnpaidNotifications[]): IUnpaidNotifications[] {
    const notifications: IUnpaidNotifications[] = [];

    if (!response) {
      return notifications;
    }

    return response;
  }

  private mapDataSource(notifications: IUnpaidNotifications[]) {
    this.dataSource = new MatTableDataSource(notifications);

    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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

  public addEvent(type: number, event: MatDatepickerInputEvent<Date>) {
    if (type === 1) {
      this.startDate = event.value;
    } else if (type === 2) {
      this.endDate = event.value;
    }

    this.getUnpaidNotificationsByDateRange(this.startDate, this.endDate, +this.dateRangeType);
  }

  public dateTypeChange(dateType: number) {
    this.getUnpaidNotificationsByDateRange(this.startDate, this.endDate, dateType);
  }

  public toggleDateFilter() {
    this.showDateFilter = !this.showDateFilter;

    if (!this.showDateFilter) {
      this.getAllUnpaidNotifications();
    }
  }

  private getUnpaidNotificationsByDateRange(startDate: Date, endDate: Date, dateType: number) {
    if (!startDate || !endDate || !dateType) {
      return;
    }

    if (dateType <= 0) {
      return;
    }


    this.unpaidService.getUnpaidNotificationsByDateRange(dateType, this.helperService.formatDate(startDate), this.helperService.formatDate(endDate)).subscribe(
      (response) => {

        this.mapDataSource(this.mapNotifications(response));
      },
      (error) => {
        this.mapDataSource(this.mapNotifications([]));
      }
    );
  }

}
