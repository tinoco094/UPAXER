import { Component, OnInit } from '@angular/core';
import español from 'src/assets/Language/ES/show-data.json';
import english from 'src/assets/Language/EN/show-data.json';
import { Recover, Register } from '../../Model/User';
import { GeneralService } from '../../Services/general.service';
import { Router } from '@angular/router';
import { Urls } from '../../Services/Urls';
import { CommentStmt } from '@angular/compiler';

@Component({
    selector: 'app-show-data',
    templateUrl: './show-data.component.html',
    styleUrls: ['./show-data.component.css']
})
export class ShowDataComponent implements OnInit {

    //#region "language"
    public Español: { id: number, data: string }[] = español;
    public English: { id: number, data: string }[] = english;

    public Actual = this.Español;
    public opcion: number = 1;
    //#endregion "language"

    public _Recover: Register = new Register();

    public error: boolean = false;
    public errorTesxt: string = "";

    public _urls: Urls = new Urls();

    constructor(
        private _service: GeneralService,
        private router: Router
    ) { }


    ngOnInit() {
        this._Recover = JSON.parse(localStorage.getItem("Usuario"));
        if (!this._Recover) {
            this.router.navigate(['/']);
        }

        //#region "load language"
        this.opcion = Number(localStorage.getItem("language"));
        if (this.opcion == 1)
            this.Actual = this.Español;
        else
            this.Actual = this.English;

        //#endregion "load language"
    }

    //#region "change language"
    changeLanguage(evento) {
        localStorage.setItem("language", evento.toString());
        if (evento == 1)
            this.Actual = this.Español;
        else
            this.Actual = this.English;
    }
    //#endregion "change language"

    //#region "Exit"
    Exit() {
        localStorage.removeItem("Usuario");
        this.router.navigate(['/']);
    }
    //#endregion "Exit"
    //#region "Update"
    Update() {
        this._service.service_general_post(this._urls.Update, this._Recover).subscribe(response => {
            if (response) {
                console.log("actualizado", response);
            }
        });
    }
    //#endregion "Update"
    //#region "Delete"
    Delete() {
        this._service.service_general_post(this._urls.Delete, this._Recover).subscribe(response => {
            if (response) {
                localStorage.removeItem("Usuario");
                this.router.navigate(['/']);
            }
        });
    }
    //#endregion "Delete"
}
