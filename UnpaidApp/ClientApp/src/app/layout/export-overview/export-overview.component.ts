import { Component, OnInit } from '@angular/core';
import { MatBottomSheetRef } from '@angular/material';
import { Papa } from 'ngx-papaparse';
import { FileSaverService } from 'ngx-filesaver';
import { UnpaidService } from '../../shared/services/unpaid.service';

@Component({
  selector: 'app-export-overview',
  templateUrl: './export-overview.component.html',
  styleUrls: ['./export-overview.component.scss']
})
export class ExportOverviewComponent implements OnInit {

  constructor(private bottomSheetRef: MatBottomSheetRef<ExportOverviewComponent>, private unpaidService: UnpaidService, private fileSaverService: FileSaverService) { }

  ngOnInit() {
  }

  export(event: MouseEvent, extension: string): void {
    const todaysDate = new Date(Date.now());
    const utc =
      `${todaysDate.getUTCDate()}${todaysDate.getUTCMonth()}${todaysDate.getUTCFullYear()}${todaysDate.getUTCHours()}${
        todaysDate.getUTCMinutes()}${todaysDate.getUTCSeconds()}`;  

    const fileName = `Responses_${utc}.${extension}`;

    this.unpaidService.getUnpaidNotificationResponses().subscribe(
      (response) => {
        console.log("unpaidService.getUnpaidNotificationResponses response", response);

        if (response) {
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

          const fileType = this.fileSaverService.genType(fileName);          
          const txtBlob = new Blob([fileData], { type: fileType });
          this.fileSaverService.save(txtBlob, fileName);

          this.bottomSheetRef.dismiss();
        }
      },
      (error) => {
        console.log("unpaidService.getUnpaidNotificationResponses error", error);
      }
    );
    
    event.preventDefault();
  }
}
