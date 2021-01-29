import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-totop-button',
  templateUrl: './totop-button.component.html',
  styleUrls: ['./totop-button.component.scss']
})
export class TotopButtonComponent implements OnInit {


  constructor() { }

  ngOnInit() {

  }

  scrollToTop() {
    (function smoothscroll() {
      var currentScroll = document.documentElement.scrollTop || document.body.scrollTop;
      if (currentScroll > 0) {
        window.requestAnimationFrame(smoothscroll);
        window.scrollTo(0, currentScroll - (currentScroll / 8));
      }
    })();
  }

}
