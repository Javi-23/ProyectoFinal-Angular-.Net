import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user/user.service';

@Component({
  selector: 'app-settings-modal',
  templateUrl: './settings-modal.component.html',
  styleUrls: ['./settings-modal.component.scss']
})
export class SettingsModalComponent  {

  constructor(private userService: UserService) {}

  selectedOption: string | null = null;
  newDescription: string = '';

  selectOption(option: string) {
    this.selectedOption = option;
  }

  resetSelection() {
    this.selectedOption = null;
  }

  updateDescription() {
    if (this.newDescription.trim() !== '') {
      this.userService.updateUserDescription(this.newDescription).subscribe({
        next: response => {
          console.log('Descripción actualizada con éxito', response);
          this.resetSelection();
        },
        error: error => {
          console.error('Error actualizando la descripción', error);
        },
        complete: () => {
          console.log('Proceso de actualización de descripción completado');
        }
      });
    }
  }
}