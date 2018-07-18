import {IUploadResult} from '../interfaces/IUploadResult';
import {FILE_TYPE} from '../enums/FileTypes';
import { FILE_LOCATION } from '../enums/FileLocation';

export namespace FileUpload {
  'use strict';

  function generateFileUploadXhr(files: File[], success: (result: IUploadResult[]) => void, error: (result: string) => void, updateProgress?: (progress: number) => void) {
    let xhr = new XMLHttpRequest();
    xhr.onprogress = (ev: ProgressEvent) => {
      if (ev.lengthComputable && xhr.status < 400 && updateProgress !== undefined) {
        let percentComplete = ev.loaded / ev.total;
        // do something with upload progress
        updateProgress(percentComplete * 100);
      }
    };
    xhr.onload = (ev: Event) => {
      if (xhr.status >= 200 && xhr.status < 400) {
        if (xhr.responseText && xhr.responseText.length) {
          try {
            success(JSON.parse(xhr.responseText));
          } catch (error) { console.log('generateFileUploadXhr', error); }
        } else {
          success(JSON.parse(xhr.response));
        }
      } else {
        if (401 === xhr.status) {
          // unauthorized. Assume session has expired.
          // handleExpiredSession();
        } else {
          if (xhr.responseText && xhr.responseText.length) {
            try {
              error(JSON.parse(xhr.responseText).message);
            } catch (error) { error(xhr.responseText); }
          } else {
            error(xhr.response);
          }
        }
      }
    };
    xhr.onerror = (ev: Event) => { // network error
      if (xhr.responseText && xhr.responseText.length) {
        try {
          error(JSON.parse(xhr.responseText).message);
        } catch (error) { error(xhr.responseText); }
      } else {
        error(xhr.response);
      }
    };

    return xhr;
  }

  export function apiFileUploadReplace(files: File[], uploadApiPath: string, originalKey: string, success: (result: IUploadResult[]) => void, error: (result: string) => void, updateProgress?: (progress: number) => void) {
    let xhr = generateFileUploadXhr(files, success, error, updateProgress);

    if (originalKey) {
      uploadApiPath = uploadApiPath + '\?original_id=' + originalKey;
    }

    let data = new FormData();
    files.map((file) => { data.append('file', file, file.name); });
    xhr.open('put', uploadApiPath, true);
    xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
    xhr.send(data);
  }

  export function apiFileUpload(files: File[], uploadApiPath: string, fileLocation: FILE_LOCATION, success: (result: IUploadResult[]) => void, error: (result: string) => void, updateProgress?: (progress: number) => void) {
    let xhr = generateFileUploadXhr(files, success, error, updateProgress);

    if (fileLocation) {
      uploadApiPath = uploadApiPath + '\?fileLocation=' + fileLocation;
    }

    let data = new FormData();
    files.map((file, index) => { data.append(`file${index}`, file, file.name); });
    xhr.open('post', uploadApiPath, true);
    xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
    xhr.send(data);
  }

  export function convertFileListToArray(filelist: FileList): File[] {
    let data: File[] = [];
    for (let file in filelist) {
      if (!filelist.hasOwnProperty(file)) {
        continue;
      }
      data.push(filelist[file]);
    }

    return data;
  }

  export function convertToFile(canvas: HTMLCanvasElement, filename: string): File {
    let imageBlob: Blob = dataURItoBlob(canvas.toDataURL('image/jpeg', 0.75));
    let fileExtension = {name: filename, lastModifiedDate: new Date()};
    return Object.assign(imageBlob, fileExtension) as File;
  }

  export function dataURItoBlob(dataURI: string): Blob {
    // convert base64/URLEncoded data component to raw binary data held in a string
    let byteString: string;
    if (dataURI.split(',')[0].indexOf('base64') >= 0) {
        byteString = atob(dataURI.split(',')[1]);
    } else {
        byteString = decodeURI(dataURI.split(',')[1]);
    }
    // separate out the mime component
    let mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
    // write the bytes of the string to a typed array
    let ia = new Uint8Array(byteString.length);
    for (let i = 0; i < byteString.length; i++) {
        ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ia], {type: mimeString});
  }



  // http://stackoverflow.com/questions/7584794/accessing-jpeg-exif-rotation-data-in-javascript-on-the-client-side
  export function getEXIFOrientation(file: File, callback: (file: File, rotation: number) => void) {
    let reader = new FileReader();
    reader.onload = function(e: any) {
      let view = new DataView(e.target.result);
      if (view.getUint16(0, false) !== 0xFFD8) {
        return callback(file, -2);
      }

      let length = view.byteLength, offset = 2;
      while (offset < length) {
        let marker = view.getUint16(offset, false);
        offset += 2;
        if (marker === 0xFFE1) {
          if (view.getUint32(offset += 2, false) !== 0x45786966) {
            return callback(file, -1);
          }
          let little = view.getUint16(offset += 6, false) === 0x4949;
          offset += view.getUint32(offset + 4, little);
          let tags = view.getUint16(offset, little);
          offset += 2;
          for (let i = 0; i < tags; i++) {
            if (view.getUint16(offset + (i * 12), little) === 0x0112) {
              return callback(file, view.getUint16(offset + (i * 12) + 8, little));
            }
          }
        // tslint:disable-next-line:no-bitwise
        } else if ((marker & 0xFF00) !== 0xFF00) {
          break;
        } else {
          offset += view.getUint16(offset, false);
        }
      }

      return callback(file, -1);
    }; // .bind(this); // compile error this implicit any

    reader.onprogress = function(data) {
      if (data.lengthComputable) {
        // let progress = parseInt(((data.loaded / data.total) * 100).toString(), 10);
      }
    };

    reader.readAsArrayBuffer(file.slice(0, 64 * 1024));
  }

  export function isFileImageCropable(file: File): boolean {
    let validTypesRegEx = /^.*\.(jpg|gif|png|bmp)$/;
    return validTypesRegEx.test(file.name);
  }

  export function getFileExtension(filetype: FILE_TYPE): string {
    if (FILE_TYPE[filetype] === undefined) {
      return '';
    }

    return FILE_TYPE[filetype].toLowerCase();
  }

  export function getMimeType(filetype: FILE_TYPE): string {
      switch (filetype) {
          case FILE_TYPE.BMP: return 'image/bmp';
          case FILE_TYPE.GIF: return 'image/gif';
          case FILE_TYPE.JPG: return 'image/jpeg';
          case FILE_TYPE.PNG: return 'image/png';
          case FILE_TYPE.TIF: return 'image/tiff';
          default: return 'text/plain';
      }
  }

  export function getMimeTypes(filetypes: FILE_TYPE[]): string[] {
    let mimetypes = filetypes.map(function(filetype: FILE_TYPE) { return getMimeType(filetype); });
    return [...new Set(mimetypes)];
  }

  export function isFileTypeClientEditable(filetype: FILE_TYPE) {
    let editableFilesTypes = [FILE_TYPE.BMP,
                              FILE_TYPE.GIF,
                              FILE_TYPE.JPG,
                              FILE_TYPE.PNG];

    return editableFilesTypes.includes(filetype);
  }

  export function getImageFileTypes(): FILE_TYPE[] {
    return [FILE_TYPE.BMP,
            FILE_TYPE.GIF,
            FILE_TYPE.JPG,
            FILE_TYPE.PNG,
            FILE_TYPE.TIF];
  }

  export function getUniqueFileTypeExtensions(filetypes: FILE_TYPE[]): string[] {
    return [...new Set(filetypes.map(function(filetype: FILE_TYPE) { return getFileExtension(filetype); }))];
  }
}

export default FileUpload;
