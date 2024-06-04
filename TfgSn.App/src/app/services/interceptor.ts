
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { SharedService } from "./shared/sharedservice.service.spec";
import { jwtDecode } from "jwt-decode";

@Injectable()
export class AuhtenticationInterceptor implements HttpInterceptor {
    constructor(private sharedService: SharedService) {}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        const token = localStorage.getItem('jwtToken')

        if (token) {
            try {
                const decodedToken: any = jwtDecode(token);
                const userId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];

                this.sharedService.setUserId(userId);

                req = req.clone({
                    setHeaders: { Authorization: `Bearer ${token}` }
                });
            } catch (error) {
                console.error('Error decoding token', error);
            }
        }

        return next.handle(req);
    }
}
