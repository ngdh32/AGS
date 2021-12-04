import Cookies from 'cookies';
import {cookies_config} from "../../config/cookies"
import UniversalCookies from "universal-cookie"
import { locale_cookie_name, default_locale, locale_strings } from '../../config/localization'
import { IncomingMessage, ServerResponse } from 'http';

export function GetLocalizedString(label: string){
    const locale = GetLocaleCookieInClient();
    const locale_string_label = locale_strings.find(x => x.key === label);
    if (locale_string_label == null){
        return label;
    }

    const locale_string = locale_string_label.pairs.find(x => x.language == locale);
    if (locale_string == null){
        return label;
    }

    return locale_string.value;
}


export function GetLocaleCookieInClient(){
    const cookies = new UniversalCookies();
    let locale = cookies.get(locale_cookie_name);
    return locale == null? default_locale.value : locale ;
}

export function SetLocaleCookieInClient(locale: string){
    const cookies = new UniversalCookies();
    cookies.set(locale_cookie_name, locale, {
        httpOnly: false
    })
}

export function GetLocaleCookieInServer(req: IncomingMessage, res: ServerResponse){
    const cookies = new Cookies(req, res, cookies_config);
    let locale = cookies.get(locale_cookie_name);
    return locale == null? default_locale.value : locale ;
}

export function SetLocaleCookieInServer(req: IncomingMessage, res: ServerResponse, locale: string){
    const cookies = new Cookies(req, res, cookies_config);
    cookies.set(locale_cookie_name, locale, {
        httpOnly: false
    })
}