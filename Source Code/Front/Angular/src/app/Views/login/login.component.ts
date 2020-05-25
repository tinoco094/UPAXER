import { Component, OnInit } from '@angular/core';
import español from 'src/assets/Language/ES/login.json';
import english from 'src/assets/Language/EN/login.json';
import { Login, Register, Recover } from '../../Model/User';
import { GeneralService } from '../../Services/general.service';
import { Urls } from '../../Services/Urls';
import { Router } from '@angular/router';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    //#region "language"
    public Español: { id: number, data: string }[] = español;
    public English: { id: number, data: string }[] = english;

    public Actual = this.Español;
    public opcion: number = 1;
    //#endregion "language"

    //#region "Error"
    public errorSignInText: string = "";
    public errorSignIn: boolean = false;
    public errorSignUpText: string = "";
    public errorSignUp: boolean = false;
    public errorRecoverText: string = "";
    public errorRecover: boolean = false;
    //#endregion "Error"

    //#region "SignIn"
    public _signIn: Login = new Login();
    //#endregion "SignIn"
    //#region "SignUp"
    public _signUp: Register = new Register();
    public _passwordRepeat: string = "";
    //#endregion "SignUp"
    //#region "Recover"
    public _Recover: Recover = new Recover();
    //#endregion "Recover"


    public _urls: Urls = new Urls();

    public Actividad: number = 1;

    constructor(
        private _service: GeneralService,
        private router: Router
    ) { }

    ngOnInit() {

        //#region "load language"
        localStorage.setItem("language", this.opcion.toString());
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


    //#region "SignUp"
    SignUp() {
        this.Actividad = 2;
    }
    SignUpSend() {
        if (this._signUp.Password == this._passwordRepeat)
            this._service.service_general_post(this._urls.Register, this._signUp).subscribe(response => {
                if (response) {
                    this.Actividad = 1;
                    this._signIn.Username = this._signUp.Username;
                }
                else {
                    this.errorSignUp = true;
                    this.errorSignUpText = this.Actual[17].data;
                }
            });
        else {
            this.errorSignUp = true;
            this.errorSignUpText = this.Actual[19].data;
        }
    }
    //#endregion "SignUp"

    //#region "Forgot Password"
    Forgot() {
        this.Actividad = 3;
    }
    ForgotSend() {
        this._service.service_general_post(this._urls.Recover, this._Recover).subscribe(response => {
            if (!response) {
                this.errorRecover = true;
                this.errorRecoverText = this.Actual[16].data;
            }

        });
    }
    //#endregion "Forgot Password"

    //#region "SignIn"
    SignIn() {
        this.Actividad = 1;
    }
    SignInSend() {
        this._service.service_general_post(this._urls.Login, this._signIn).subscribe(response => {
            if (!response.Estado) {
                this.errorSignIn = true;
                this.errorSignInText = this.Actual[18].data;
            } else {
                localStorage.setItem("Usuario", JSON.stringify( response));
                this.router.navigate(['/show']);
            }
        });
    }
    //#endregion "SignIn"
}
