import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';


@Injectable({
    providedIn: 'root'
})
export class GeneralService {
    url_api = 'http://localhost:49966/api/';


    headers = new HttpHeaders();

    constructor(private http: HttpClient) { }

    /* POST Request */
    service_general_post(url, parametros): Observable<any> {
        this.setHeaders();
        return this.http.post(this.url_api + url, parametros, { headers: this.headers });
    }
    // --------------------------

    /* PUT Request */
    service_general_put(url, parametros): Observable<any> {
        this.setHeaders();
        return this.http.put(this.url_api + url, parametros, { headers: this.headers });
    }
    // --------------------------

    /* DELETE Request */
    service_general_delete(url): Observable<any> {
        this.setHeaders();
        return this.http.delete(this.url_api + url, { headers: this.headers });
    }
    // --------------------------

    /* GET Request */
    service_general_get(url): Observable<any> {
        this.setHeaders();
        return this.http.get(this.url_api + url, { headers: this.headers });
    }
    // --------------------------

    setHeaders() {
        this.headers = new HttpHeaders(
            {
                'content-type': 'application/json; charset=utf-8',
                'Accept': 'application/json'
            }
        );
    }
}
