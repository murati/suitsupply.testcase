import { Component, OnInit } from '@angular/core';
import { Http, ResponseContentType, RequestOptions, Headers } from '@angular/http';
import { DatePipe } from '@angular/common';
@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})

export class HomeComponent implements OnInit {
    public filter: string = "";
    constructor(private _httpService: Http) { }
    products: any[];
    imageSources: string = "http://localhost:55556/productImages/";
    ngOnInit() {
        this._httpService.get('http://localhost:55556/api/products/get').subscribe(products => {
            this.products = products.json();
        });
    }

    deleteProduct(productId: number) {
        if (!confirm("Are you sure want to delete this record?"))
            return;
        let headers = new Headers({
            'Accept': 'application/json',
            'enctype': 'multipart/form-data'
        });
        let options = new RequestOptions({ headers: headers });

        this._httpService.delete('http://localhost:55556/api/products/delete/' + productId).subscribe(data => {
            console.log(data);
            for (var i = 0; i < this.products.length; i++) {
                if (this.products[i].id == productId) {
                    let productIndex = this.products.indexOf(this.products[i]);
                    this.products.splice(productIndex, 1);
                }
            }
        });
    }

    search(filter: string) {
        this._httpService.get('http://localhost:55556/api/products/get/' + this.filter).subscribe(products => {
            this.products = products.json();
        });
    }
    exportToExcel(filter: string) {
        alert("Excel will import what is on the table right now.");
        this._httpService.get('http://localhost:55556/api/products/export/' + this.filter, { responseType: ResponseContentType.Blob })
            .subscribe(res => {
                var url = window.URL.createObjectURL(res.blob());
                window.open(url, '_blank');
            });
    }

}
