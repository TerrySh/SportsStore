import { Product } from "./product.model";
import { Injectable } from '@angular/core';
import { Http, RequestMethod, Request, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';

const productsUrl = "/api/products";

@Injectable()
export class Repository {

  private productData: Product;
  constructor(private http: Http) {
    this.getProduct(1);
  }

  getProduct(id: number) {
    this.sendRequest(RequestMethod.Get, productsUrl + '/' + id)
      .subscribe(response => {
        this.productData = response;
        console.log('Product Data Received')
      });
  }

  private sendRequest(verb: RequestMethod, url: string, data?: any){
    return this.http.request(new Request({
      method: verb, url: url, body: data
    })).map(response => response.json());
  }

  get product(): Product {
    console.log('Product Data Requested');
    return this.productData;
  }
}
