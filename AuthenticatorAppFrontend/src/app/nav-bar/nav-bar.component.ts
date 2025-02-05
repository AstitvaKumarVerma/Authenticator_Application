import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ServicesService } from '../ApiServices/services.service';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  imports: [MatToolbarModule],
  templateUrl: './nav-bar.component.html',
  styleUrl: './nav-bar.component.css'
})
export class NavBarComponent implements OnInit  {
  userEmail:any;
  constructor(private router: Router, private service: ServicesService) { }

  ngOnInit(): void {
    this.userEmail = localStorage.getItem("UserEmail");
  }

  logout() {
    const data = { userEmail: this.userEmail }
    
    this.service.disableAuthentication(data).subscribe((data:any)=> {

    });

    // Clear localStorage or perform any logout logic here
    localStorage.clear();

    // Redirect to login page or any other appropriate action after logout
    this.router.navigate(['/']);
  }
}
