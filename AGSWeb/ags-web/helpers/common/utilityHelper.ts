import Cookies from  'cookies'
import {cookies_config} from "../../config/cookies"
import { auth_code_params_cookie_name } from '../../config/auth'
import AuthenticationHeader from '../../models/common/authenticationHeader'
import  { AxiosResponse } from 'axios';
import AGSResponse from '../../models/common/agsResponse';
import { IncomingMessage, ServerResponse } from 'http';

export function GetBearerConfig(req: IncomingMessage, res: ServerResponse): AuthenticationHeader{
    const cookies = new Cookies(req, res, cookies_config);
    const accessToken = cookies.get(auth_code_params_cookie_name);
    const authenticationHeader = new AuthenticationHeader(accessToken);
    return authenticationHeader;
}

export function CheckIfUnauthorizedResponse(result: AGSResponse){
    return result.responseCode == 403 ? true : false
}

export function GetRedirectToErrorPageObject(){
    return {
        redirect: {
            permanent: false,
            destination: '/error'
        }
    }
}