import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../_services/members.service';
import { ActivatedRoute } from '@angular/router';
import { Member } from '../../_models/member';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { TimeagoModule } from 'ngx-timeago';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  imports: [NgbNavModule, GalleryModule, TimeagoModule, DatePipe],
  templateUrl: './member-detail.component.html',
  styleUrl: './member-detail.component.scss'
})
export class MemberDetailComponent implements OnInit {
  private memberService = inject(MembersService);
  private route = inject(ActivatedRoute);
  images: GalleryItem[] = [];
  member?: Member;
  //Nav tab 1 active by default
  active = 1;

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const username = this.route.snapshot.paramMap.get("username");
    if(!username) return;
    this.memberService.getMember(username).subscribe({
      next: member => {
          this.member = member
          member.photos.map(p => {
            this.images.push(new ImageItem({src: p.url, thumb: p.url}))
          })
        }
    })
  }

}
