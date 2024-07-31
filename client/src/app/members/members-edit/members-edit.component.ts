import { Component, ElementRef, HostListener, inject, OnInit, ViewChild } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { Member } from '../../_models/member';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-members-edit',
  standalone: true,
  imports: [NgbNavModule, FormsModule],
  templateUrl: './members-edit.component.html',
  styleUrl: './members-edit.component.scss'
})
export class MembersEditComponent implements OnInit {
  @ViewChild("editForm") editForm?: NgForm;
  @HostListener("window:beforeunload", ["$event"]) notify($event:any) {
    if(this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  member?: Member;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService);

  //Nav tab 1 active by default
  active = 1;
  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.accountService.currentUser();
    if(!user) return;
    this.memberService.getMember(user.username).subscribe({
      next: member => this.member = member
    })
  }

  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.toastr.success("Profile updated successfully");
        this.editForm?.reset(this.member);
      }
    })
  }
}
