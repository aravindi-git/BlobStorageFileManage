import { Component, OnInit } from '@angular/core';
import { FileManagerService } from '../services/file-manager.service'; 

@Component({
  selector: 'app-file-list',
  templateUrl: './file-list.component.html',
  styleUrls: ['./file-list.component.css']
})
export class FileListComponent implements OnInit {

  public response: { fileURL: ''};
  public filesList : string[] = []; 
  baseUrl: string; 

  constructor(private fileService : FileManagerService) { }

  ngOnInit(): void {
    this.getAttachments(); 
    this.baseUrl = this.fileService.getBaseUrl(); 
  }

  public uploadFinished = (event) => {
    this.response = event;
    this.getAttachments(); 
  }

  getAttachments()
  {
    this.fileService.getFiles() .subscribe( (response)=> {
      this.filesList = response as string[]; 
    });
  }

  downloadFile(fileName: string): void
  {
    var downloadWindow = window.open( `${this.baseUrl}files/download?fileName=${fileName}` ,"_blank");
    downloadWindow.focus();
  }

  viewFile(fileName: string): void
  {
    window.open( `${this.baseUrl}files/view?fileName=${fileName}` , "_blank" );
  }

  deleteFile(fileName: string): void
  {
    this.fileService.deleteFile(fileName) .subscribe( (response)=> {
        this.getAttachments(); 
      });
  }


}
