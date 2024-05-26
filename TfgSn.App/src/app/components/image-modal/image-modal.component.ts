import { HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, Output } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-image-modal',
  templateUrl: './image-modal.component.html',
  styleUrls: ['./image-modal.component.scss']
})
export class ImageModalComponent {
  @Output() closeModal = new EventEmitter<void>();
  @Output() imageUploaded = new EventEmitter<Uint8Array>();
  selectedFile: File | null = null;

  constructor(private userService: UserService) { }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    if (this.selectedFile) {
      this.userService.uploadUserImage(this.selectedFile).subscribe({
        next: (response: any) => {
          console.log('Imagen subida exitosamente', response);
          const byteArray = new Uint8Array(response.imageData);
          this.imageUploaded.emit(byteArray);
          this.closeModal.emit();
        },
        error: (error: HttpErrorResponse) => {
          console.error('Error subiendo la imagen', error);
        }
      });
    } else {
      console.error('No se ha seleccionado ninguna imagen.');
    }
  }
}