import { Component,OnInit, EventEmitter, Output} from '@angular/core';
import {  HttpEventType } from '@angular/common/http';
import { FileManagerService } from '../services/file-manager.service'; 

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent implements OnInit {
  public progress: number;
  public isUploadInprogress: boolean;
  
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private fileService : FileManagerService) { }

  ngOnInit(): void {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    
    let fileToUpload = <File>files[0];
    this.fileService.uploadFiles(fileToUpload).subscribe((response) => {
      if (response.type === HttpEventType.UploadProgress)
      {
        this.isUploadInprogress = true; 
        this.progress = Math.round(100 * response.loaded / response.total);
      }
      else if (response.type === HttpEventType.Response) 
      {
        this.isUploadInprogress = false;  
        this.onUploadFinished.emit(response.body);
      }
    }); 
 
  } 


}
