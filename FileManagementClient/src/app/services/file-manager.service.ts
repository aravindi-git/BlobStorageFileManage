import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable} from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FileManagerService {

  private baseApiUrl: string;
  private apiDownloadUrl: string;

  constructor(private httpClient: HttpClient) {
    this.baseApiUrl = environment.baseURL; 
  }

  uploadFiles(fileToUpload: File) : Observable<any>
  {
    const formData = new FormData(); 
    formData.append('file', fileToUpload, fileToUpload.name);
    return this.httpClient.post(`${this.baseApiUrl}files/upload`, formData, {reportProgress: true, observe: 'events'}); 
  }

  getFiles() : Observable<any>
  {
    return  this.httpClient.get(`${this.baseApiUrl}files/getFiles`);
  }

  deleteFile(fileName: string) : Observable<any>
  {
    return  this.httpClient.get(`${this.baseApiUrl}files/delete?fileName=${fileName}`);
  }

  getBaseUrl()
  {
    return this.baseApiUrl; 
  }

 
}
