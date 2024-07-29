import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';

@Component({
  selector: 'app-test-errors',
  standalone: true,
  imports: [],
  templateUrl: './test-errors.component.html',
  styleUrl: './test-errors.component.scss'
})
export class TestErrorsComponent {
  baseUrl = "http://localhost:5289/api/";
  private http = inject(HttpClient);
  validationErrors: string[] = [];

  get400() {
    this.http.get(this.baseUrl + "error/bad-request").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get401() {
    this.http.get(this.baseUrl + "error/auth").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get404() {
    this.http.get(this.baseUrl + "error/not-found").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get500() {
    this.http.get(this.baseUrl + "error/server-error").subscribe({
      next: response => console.log(response),
      error: error => console.log(error)
    })
  }

  get400Validation() {
    this.http.post(this.baseUrl + "accounts/register", {} ).subscribe({
      next: response => console.log(response),
      error: error => {
        console.log(error)
        this.validationErrors = error;
        }
    })
  }
}
