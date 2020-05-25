import { extend } from 'webdriver-js-extender';

export class Login {
    constructor() { }
    public Username: string;
    public Password: string;
}

export class Register {
    constructor() { }
    public Id: number;
    public Username: string;
    public LastName: string;
    public MotherLastName: string;
    public Password: string;
    public Name: string;
    public Email: string;
    public Estado: boolean;
}

export class Recover extends Login {
}
