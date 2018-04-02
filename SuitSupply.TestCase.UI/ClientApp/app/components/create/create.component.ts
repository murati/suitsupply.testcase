import { Component, ElementRef, ViewChild, OnInit, Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, FormControl } from "@angular/forms";

import { Product } from '../../models/product';
import { Http, RequestOptions, Headers } from '@angular/http';
import 'rxjs/Rx';
@Component({
    selector: 'create',
    templateUrl: './create.component.html'
})

export class CreateComponent implements OnInit {
    ngOnInit() {
        this.productForm = this._fb.group({
            Name: ['', [<any>Validators.required, <any>Validators.minLength(5)]],
            Price: ['', [<any>Validators.required, <any>Validators.min(1)]],
            PhotoStream: [null, [<any>Validators.required]],
        });
    }

    public productForm: FormGroup; // our model driven form
    public submitted: boolean; // keep track on whether form is submitted
    public file: File;
    @ViewChild('fileInput') fileInput: ElementRef;

    constructor(private _fb: FormBuilder, private _httpService: Http) { } // form builder simplify form initialization

    onFileChange(event) {
        if (event.target.files.length > 0) {
            this.file = event.target.files[0];
            this.productForm.get('PhotoStream').setValue(this.file);
        }
    }

    save(model: Product, isValid: boolean) {
        if (!isValid) return;

        this.submitted = true; // set form submit to true
        let data: FormData = new FormData();
        data.append("Name", model.Name);
        data.append("Price", model.Price.toFixed(2).replace(".",","));
        data.append("PhotoStream", this.file, this.file.name);
        let headers = new Headers({ 'Accept': 'application/json', 'enctype': 'multipart/form-data' });
        let options = new RequestOptions({ headers: headers });
        this._httpService.post('http://localhost:55556/api/products/create',data, options)
            .subscribe(data => { console.log(data); });
        this.productForm.reset();
    }

}




