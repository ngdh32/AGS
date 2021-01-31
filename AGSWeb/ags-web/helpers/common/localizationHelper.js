import Cookies from 'cookies';
import {cookies_config} from "../../config/cookies.js"
import UniversalCookies from "universal-cookie"
import { locale_cookie_name, default_locale, locale_strings } from '../../config/localization.js'

export function GetLocalizedString(label){
    const locale = GetLocaleCookieInClient();
    const locale_string_label = locale_strings[label];
    if (locale_string_label == null){
        return label;
    }

    const locale_string = locale_string_label[locale];
    if (locale_string == null){
        return label;
    }

    return locale_string;
}


export function GetLocaleCookieInClient(){
    const cookies = new UniversalCookies();
    let locale = cookies.get(locale_cookie_name);
    return locale == null? default_locale.value : locale ;
}

export function SetLocaleCookieInClient(locale){
    const cookies = new UniversalCookies();
    cookies.set(locale_cookie_name, locale, {
        httpOnly: false
    })
}

export function GetLocaleCookieInServer(req, res){
    const cookies = new Cookies(req, res, cookies_config);
    let locale = cookies.get(locale_cookie_name);
    return locale == null? default_locale.value : locale ;
}

export function SetLocaleCookieInServer(req, res, locale){
    const cookies = new Cookies(req, res, cookies_config);
    cookies.set(locale_cookie_name, locale, {
        httpOnly: false
    })
}