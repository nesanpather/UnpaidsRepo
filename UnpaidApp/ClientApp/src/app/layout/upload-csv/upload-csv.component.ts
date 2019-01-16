import { Component, OnInit } from '@angular/core';
import { Papa } from 'ngx-papaparse';
import { IUnpaidInput } from '../../shared/models/unpaid-input';
import { UnpaidService } from '../../shared/services/unpaid.service';

@Component({
  selector: 'app-upload-csv',
  templateUrl: './upload-csv.component.html',
  styleUrls: ['./upload-csv.component.scss']
})
export class UploadCsvComponent implements OnInit {
  selectedFile: File;
  addUnpaidStatus: boolean = false;

  constructor(private unpaidService: UnpaidService) { }  

  uploadFile(fileInput: any): void {
    const file: File = fileInput.files[0];
    const reader = new FileReader();
    const papa = new Papa();

    reader.addEventListener('load',
      (event: any) => {

        const csvDataText = event.target.result;
        console.log("File Event", csvDataText);
        console.log("File", file);

        const options = {
          complete: (results, file) => {
            console.log('Parsed: ', results);
            console.log('data: ', results.data);
            
            if (results && results.data) {
              const unpaidInputs: IUnpaidInput[] = [];

              unpaidInputs.push(results.data);

              this.unpaidService.addUnpaid(results.data).subscribe(
                (response) => {
                  console.log("unpaidService.addUnpaid response", response);                  
                  this.addUnpaidStatus = true;
                },
                (error) => {
                  console.log("unpaidService.addUnpaid error", error);
                  this.addUnpaidStatus = false;
                }
              );

            }
            
            
          },// Add your options here
          header: true                      
        };

        papa.parse(csvDataText, options);

      });

    reader.readAsText(file);
  }

  ngOnInit() {
  }

}
