import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { MatBottomSheet } from '@angular/material';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

import { ExportOverviewComponent } from '../export-overview/export-overview.component';
import { IUnpaidNotificationsResponse } from '../../shared/models/unpaid-notifications-response';
import { UnpaidService } from '../../shared/services/unpaid.service';
import { HelperService } from '../../shared/services/helper.service';

@Component({
  selector: 'app-responses-table',
  templateUrl: './responses-table.component.html',
  styleUrls: ['./responses-table.component.scss']
})
export class ResponsesTableComponent implements OnInit {
  displayedColumns = ['policyNumber', 'idNumber', 'dateAdded', 'notificationType', 'correlationId', 'dateNotificationSent', 'contactOptionType', 'accepted', 'dateNotificationResponseAdded'];
  dataSource: MatTableDataSource<IUnpaidNotificationsResponse>;
  dateRangeType: string = "1";
  startDate: Date;
  endDate: Date;
  showDateFilter: boolean = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private unpaidService: UnpaidService, private bottomSheet: MatBottomSheet, private helperService: HelperService) {
  }

  ngOnInit() {
    this.getAllUnpaidNotificationResponses();
  }

  private getAllUnpaidNotificationResponses() {
    this.unpaidService.getUnpaidNotificationResponses().subscribe(
      (response) => {
        console.log("unpaidService.getUnpaidNotificationResponses response", response);

        this.mapDataSource(this.mapNotificationResponses(response));
      },
      (error) => {
        console.log("unpaidService.getUnpaidNotificationResponses error", error);
      }
    );
  }

  private mapNotificationResponses(response: IUnpaidNotificationsResponse[]): IUnpaidNotificationsResponse[] {
    const notificationResponses: IUnpaidNotificationsResponse[] = [];

    if (!response) {
      return notificationResponses;
    }

    return response;
  }

  private mapDataSource(notificationResponses: IUnpaidNotificationsResponse[]) {
    this.dataSource = new MatTableDataSource(notificationResponses);

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

  public mapContactType(contactType: boolean): string {
    if (contactType) {
      return "Yes";
    }

    return "No";
  }

  public openExportOptions(): void {
    this.bottomSheet.open(ExportOverviewComponent);
  }

  public addEvent(type: number, event: MatDatepickerInputEvent<Date>) {

    if (type === 1) {
      this.startDate = event.value;
    } else if (type === 2) {
      this.endDate = event.value;
    }

    this.getUnpaidResponsesByDateRange(this.startDate, this.endDate, +this.dateRangeType);
  }

  public dateTypeChange(dateType: number) {
    this.getUnpaidResponsesByDateRange(this.startDate, this.endDate, dateType);
  }

  public toggleDateFilter() {
    this.showDateFilter = !this.showDateFilter;

    if (!this.showDateFilter) {
      this.getAllUnpaidNotificationResponses();
    }
  }

  private getUnpaidResponsesByDateRange(startDate: Date, endDate: Date, dateType: number) {
    if (!startDate || !endDate || !dateType) {
      return;
    }

    if (dateType <= 0) {
      return;
    }

    this.unpaidService.getUnpaidNotificationResponsesByDateRange(dateType, this.helperService.formatDate(startDate), this.helperService.formatDate(endDate)).subscribe(
      (response) => {
        this.mapDataSource(this.mapNotificationResponses(response));
      },
      (error) => {
        this.mapDataSource(this.mapNotificationResponses([]));
      }
    );
  }

}
