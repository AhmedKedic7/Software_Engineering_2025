import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { UserService } from '../../services/user.service';

export interface Users{
  userId:string,
  firstName:string,
  lastName:string,
  email:string,
  isAdmin:boolean,
  isLogedin:boolean
}

@Component({
  selector: 'app-real-user-table',
  standalone: true,
  imports: [CommonModule,DataTablesModule],
  templateUrl: './real-user-table.component.html',
  styleUrl: './real-user-table.component.scss'
})
export class RealUserTableComponent implements OnInit {
  @Output() userNumberChange: EventEmitter<number> = new EventEmitter<number>();

  userNumber: number = 0;
  userlist:any[]=[];
  dtoptions:Config={};
  dttrigger:Subject<any>=new Subject<any>();
  
  constructor(private userService:UserService){}

  ngOnInit(): void {
    this.dtoptions = {
      pagingType: 'full_numbers',   
      pageLength: 10,  
      processing: true,  
      language: {
        search: "Filter records:",   
        lengthMenu: "Show _MENU_ records per page"
      }
    };

    this.loadusers(); 
    
   
  }

  ngAfterViewInit(): void {
     
    if (this.userlist.length) {
      this.dttrigger.next(null);  
    }
  }

  loadusers(){
    this.userService.getAllUsers().subscribe(user => {
      this.userlist = user; 
      console.log(this.userlist)
      this.userNumber=this.userlist.length;
      console.log(this.userNumber)
      this.userNumberChange.emit(this.userNumber);
      if (this.userlist.length) {
        this.dttrigger.next(null);  
      }
    });
  }
}
