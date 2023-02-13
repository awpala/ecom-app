import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { PagingHeaderComponent } from './paging-header/paging-header.component';

@NgModule({
  declarations: [
    PagingHeaderComponent,
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(), // N.B. method `forRoot()` loads `PaginationModule` as a singleton, to be used throughout the app (i.e., only one instance is required)
  ],
  exports: [
    PaginationModule,
    PagingHeaderComponent,
  ],
})
export class SharedModule {}
