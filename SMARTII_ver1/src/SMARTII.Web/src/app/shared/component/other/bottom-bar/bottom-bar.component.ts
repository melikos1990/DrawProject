import { Component, OnInit, HostListener } from '@angular/core';

@Component({
  selector: 'app-bottom-bar',
  templateUrl: './bottom-bar.component.html',
  styleUrls: ['./bottom-bar.component.scss']
})
export class BottomBarComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  @HostListener("window:scroll", ["$event"])
  onWindowScroll() {


    var element = document.getElementById('bottom-bar');

    //element高度
    let pos = element.offsetTop + element.clientHeight;

    //畫面容器高度
    let max = document.documentElement.clientHeight;

    //Scroll 目前高度
    let scroll = document.documentElement.scrollTop;

    // pos/max will give you the distance between scroll bottom and and bottom of screen in percentage.
    if (pos >= Math.ceil(max + scroll)) {
      document.getElementById('bottom-bar').style.opacity = "0.3";
    }
    else {
      document.getElementById('bottom-bar').style.opacity = "1";
    }
  }


  mouseEnter(div: string) {
    document.getElementById('bottom-bar').style.opacity = "1";
  }
  mouseLeave(div: string) {
    var element = document.getElementById('bottom-bar')
    //目前畫面高度
    let pos = (document.documentElement.offsetHeight - (element.offsetTop)) - element.offsetTop;
    //容器高度
    let max = document.documentElement.clientHeight;

    if (pos >= max) {
      document.getElementById('bottom-bar').style.opacity = "0.3";
    }
  }
}
