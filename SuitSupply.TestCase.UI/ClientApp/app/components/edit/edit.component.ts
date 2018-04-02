import { Component, ElementRef, ViewChild, OnInit, Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from "@angular/forms";

import { Product } from '../../models/product';
import { Http, RequestOptions, Headers } from '@angular/http';
import 'rxjs/Rx';
import { ActivatedRoute } from '@angular/router';
import { NgModel } from '@angular/forms';
@Component({
    selector: 'edit',
    templateUrl: './edit.component.html'
})

export class EditComponent implements OnInit {
    //public productForm: FormGroup; // our model driven form
    public submitted: boolean; // keep track on whether form is submitted
    public file: File;
    private productId: string;
    public product: any;
    private imageSource: string = "http://localhost:55556/productImages/";
    @ViewChild('fileInput') fileInput: ElementRef;

  

    ngOnInit() {
        this.productId = this.route.snapshot.paramMap.get('id');
        this._httpService.get('http://localhost:55556/api/products/get/' + this.productId).subscribe(product => {
            this.product = product.json();
        });
      
    }
     

    constructor(private _fb: FormBuilder, private _httpService: Http, private route: ActivatedRoute) { } // form builder simplify form initialization

    onFileChange(event) {
        if (event.target.files.length > 0) {
            this.file = event.target.files[0];
        }
    }

    save(model: any, isValid: boolean) {
        console.log(model);
        if (!isValid) return;

        this.submitted = true; // set form submit to true
        let data: FormData = new FormData();
        data.append("Id", this.productId);
        data.append("Name", model.name);
        data.append("Price", parseFloat(model.price).toFixed(2).replace(".", ","));
        data.append("PhotoStream", this.file, this.file.name);
        let headers = new Headers({ 'Accept': 'application/json', 'enctype': 'multipart/form-data' });
        let options = new RequestOptions({ headers: headers });
        this._httpService.put('http://localhost:55556/api/products/update', data, options)
            .subscribe(data => { console.log(data); });
        //this.productForm.reset();
    }

}




