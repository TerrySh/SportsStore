import { Component } from '@angular/core';
import { Repository } from './models/repository';
import { Product } from './models/product.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  //title = 'Angular & ASP.NET Core MVC';
  //title = no_such_object;

  constructor(private repo: Repository){}

  get product(): Product {
    return this.repo.product;
  }

  get products(): Product[]{
    return this.repo.products;
  }
}
