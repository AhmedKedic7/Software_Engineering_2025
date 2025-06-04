import { Component, EventEmitter ,OnInit,Output} from '@angular/core';
import { Config } from 'datatables.net';
import { Subject } from 'rxjs';
import { OrderService } from '../../services/order.service';
import { CommonModule } from '@angular/common';
import { DataTablesModule } from 'angular-datatables';


export interface Orders{
  orderId:string;
  userId:string,
  createdAt: Date,
  totalPrice:number,
  shippingAddress:string
}

@Component({
  selector: 'app-order-table',
  standalone: true,
  imports: [CommonModule,DataTablesModule],
  templateUrl: './order-table.component.html',
  styleUrl: './order-table.component.scss'
})
export class OrderTableComponent implements OnInit{
  @Output() orderNumberChange: EventEmitter<number> = new EventEmitter<number>();

  orderNumber: number = 0;
  orderlist:any[]=[];
  dtoptions:Config={};
  dttrigger:Subject<any>=new Subject<any>();
  
  constructor(private orderService:OrderService){}

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

    this.loadorders(); 
    
   
  }

  ngAfterViewInit(): void {
     
    if (this.orderlist.length) {
      this.dttrigger.next(null);  
    }
  }

  loadorders(){
    this.orderService.getAllOrders().subscribe(order => {
      this.orderlist = order; 
       
      this.orderNumber=this.orderlist.length;
      console.log(this.orderNumber)
      this.orderNumberChange.emit(this.orderNumber);
      if (this.orderlist.length) {
        this.dttrigger.next(null);  
      }
    });
  }
}
