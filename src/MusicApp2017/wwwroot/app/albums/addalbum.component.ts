import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Http, Headers } from '@angular/http';

@Component({
    selector: 'addalbum',
    templateUrl: './addalbum.component.html'

})

export class AddAlbumComponent {

    model: Album = new Album();
    postResponse: Object;
    showForm = false;

    constructor(private http: Http) {

    }

    onSubmit(form: NgForm) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        this.http.post('/api/albums', JSON.stringify(this.model), { headers: headers }).subscribe(res => this.postResponse = res.json());
        form.reset();
        this.showForm = !this.showForm;
    }

    toggleForm() {
        this.showForm = !this.showForm;
    }

}

class Album {
    constructor(private albumID: number = 0,
        public title: string = null,
        public artistID: number = 0,
        public genreID: number = 0,
        public averageRating: number = 0,
    ) { }
}