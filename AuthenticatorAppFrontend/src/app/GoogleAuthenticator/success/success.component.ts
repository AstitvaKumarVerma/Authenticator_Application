import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-success',
  standalone: true,
  imports: [],
  templateUrl: './success.component.html',
  styleUrl: './success.component.css'
})
export class SuccessComponent {
  @ViewChild('countdown') countdownElement!: ElementRef;
  countdown = 5;
  intervalId: any;

  constructor(private router: Router) { }

  ngAfterViewInit() {
    if (this.countdownElement) {
      this.countdownElement.nativeElement.textContent = this.countdown.toString();
    }
    debugger;
    this.intervalId = setInterval(() => {
    this.countdown--;
    this.countdownElement.nativeElement.textContent = this.countdown.toString();

      if (this.countdown === 0) {
        clearInterval(this.intervalId);
        this.router.navigate(['/home']); // Redirect to home
      }
    }, 1000); // Update every second
  }

  ngOnDestroy() {
    clearInterval(this.intervalId); // Clear interval on component destruction
  }
}
