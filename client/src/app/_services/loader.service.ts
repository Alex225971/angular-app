import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';

@Injectable({
  providedIn: 'root'
})
export class LoaderService {
  loadRequestCount = 0;
  private spinnerService = inject(NgxSpinnerService)

  loading() { 
    this.loadRequestCount++;
    this.spinnerService.show(undefined, {
      type: "ball-spin-clockwise",
      bdColor: "rgba(255,255,255,0)",
      color: "#32fbe2"
    })
  }

  idle() {
    this.loadRequestCount --;
    if(this.loadRequestCount <= 0) {
      this.loadRequestCount = 0;
      this.spinnerService.hide();
    }
  }
}
