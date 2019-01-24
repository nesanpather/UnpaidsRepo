import { Component, OnInit } from '@angular/core';
import { MatBottomSheetRef } from '@angular/material';
import { Papa } from 'ngx-papaparse';
import { FileSaverService } from 'ngx-filesaver';
import jsPDF from 'jspdf';
import 'jspdf-autotable';
import { UnpaidService } from '../../shared/services/unpaid.service';
import { IUnpaidNotificationsResponse } from '../../shared/models/unpaid-notifications-response';

@Component({
  selector: 'app-export-overview',
  templateUrl: './export-overview.component.html',
  styleUrls: ['./export-overview.component.scss']
})
export class ExportOverviewComponent implements OnInit {

  constructor(private bottomSheetRef: MatBottomSheetRef<ExportOverviewComponent>, private unpaidService: UnpaidService, private fileSaverService: FileSaverService) { }

  ngOnInit() {
  }

  public export(event: MouseEvent, extension: string): void {
    const fileName = this.generateFileName(extension);

    if (extension === "pdf") {
      this.captureScreen(fileName);

    } else {
      this.unpaidService.getUnpaidNotificationResponses().subscribe(
        (response: IUnpaidNotificationsResponse[]) => {
          console.log("unpaidService.getUnpaidNotificationResponses response", response);

          if (!response) {
            return;
          }

          let fileData = "";
          if (extension === "csv") {
            const papa = new Papa();

            const options = {
              quotes: false,
              delimiter: ",",
              header: true,
              newline: "\r\n"
            };

            const csv = papa.unparse(response, options);
            if (csv) {
              fileData = csv;
            }
          }

          this.createFileDownload(fileName, fileData);

          this.bottomSheetRef.dismiss();

        },
        (error) => {
          console.log("unpaidService.getUnpaidNotificationResponses error", error);
        }
      );

    }

    
    event.preventDefault();
  }

  private generateFileName(extension: string): string {
    const todaysDate = new Date(Date.now());
    const utc =
      `${todaysDate.getUTCDate()}${todaysDate.getUTCMonth()}${todaysDate.getUTCFullYear()}${todaysDate.getUTCHours()}${
        todaysDate.getUTCMinutes()}${todaysDate.getUTCSeconds()}`;

    return `Responses_${utc}.${extension}`;
  }

  private createFileDownload(fileName: string, fileData: any) {
    const fileType = this.fileSaverService.genType(fileName);
    const txtBlob = new Blob([fileData], { type: fileType });
    this.fileSaverService.save(txtBlob, fileName);
  }

  private captureScreen(fileName: string) {
    var data = document.getElementById('contentToConvert');

    const width = data.clientWidth;
    const height = data.clientHeight;
    let rotate = 'p';
    if (width > height) {
      rotate = 'l';
    }

    let doc = new jsPDF(rotate);
    doc.autoTable({ html: '#contentToConvert', theme: 'striped' });   
    doc.save(fileName);
    this.bottomSheetRef.dismiss();
  }
}
