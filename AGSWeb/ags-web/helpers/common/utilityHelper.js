import Cookies from  'cookies'
import {cookies_config} from "../../config/Cookies"
import { auth_code_params_cookie_name } from '../../config/auth.js'

export function GetBearerConfig(req, res) {
    const cookies = new Cookies(req, res, cookies_config);
    const access_token = cookies.get(auth_code_params_cookie_name);

    return {
        headers: { Authorization: `Bearer ${access_token}` }
    }
}

export function CheckIfUnauthorizedResponse(result){
    return result == 403 ? true : false
}

export function GetRedirectToErrorPageObject(){
    return {
        redirect: {
            permanent: false,
            destination: '/error'
        }
    }
}