import { Component, Input, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-body',
  templateUrl: './body.component.html',
  styleUrls: ['./body.component.scss']
})
export class BodyComponent implements OnInit {
  @Input() collapsed = false;
  @Input() screenWidth = 0;
  rutaActual: string = '';

  constructor(private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.rutaActual = event.url;
      }
    });
  }

  esRutaHijaDeMain(): boolean {
    return this.rutaActual.startsWith('/main/');
  }

  getBodyClass(): string {
    let styleClass = 'body';
    if (this.esRutaHijaDeMain()) {
      if (this.collapsed && this.screenWidth > 768) {
        styleClass += ' body-trimmed';
      } else if (this.collapsed && this.screenWidth <= 768 && this.screenWidth > 0) {
        styleClass += ' body-md-screen';
      }
    } else {
      styleClass += ' no-main';
    }
    return styleClass;
  }
}