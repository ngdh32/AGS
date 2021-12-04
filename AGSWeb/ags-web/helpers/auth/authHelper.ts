import { 
    client_id, 
    client_secret, 
    redirect_uri, 
    authentication_url, 
    scope,
    pkce_cookie_name, 
    auth_code_params_cookie_name,
    auth_code_id_token_cookie_name,
    redirect_post_logout_url,
    request_timeout
} from '../../config/auth'
import Cookies from 'cookies';
import { cookies_config } from "../../config/cookies";
import { IncomingMessage, ServerResponse } from 'http';
import { Issuer, custom, Client, generators, CallbackParamsType } from 'openid-client'

export async function GetClient() : Promise<Client>{
    custom.setHttpOptionsDefaults({
        timeout: request_timeout
    });
    const agsIdentityIssuer = await Issuer.discover(authentication_url);
    const client = new agsIdentityIssuer.Client({
        client_id: client_id,
        client_secret: client_secret,
        redirect_uris: [redirect_uri],
        post_logout_redirect_uris: [redirect_post_logout_url],
        response_types: ['code']
    });

    return client;
}

export async function GetRedirectUri(code_verifier: string){
    custom.setHttpOptionsDefaults({
        timeout: request_timeout
    });
    const client = await GetClient();
    const url = client.authorizationUrl({
        scope: scope,
        code_challenge: generators.codeChallenge(code_verifier),
        code_challenge_method: 'S256'
    });
    return url;
}

export async function GetLogoutRedirectUri(req: IncomingMessage, res: ServerResponse){
    const cookies = new Cookies(req, res, cookies_config);
    const id_token = cookies.get(auth_code_id_token_cookie_name);

    const client = await GetClient();
    const url = client.endSessionUrl({ id_token_hint: id_token, redirect_post_logout_url } );
    return url;
}

export function GenerateCodeVerifier(){
    custom.setHttpOptionsDefaults({
        timeout: request_timeout
    });
    const code_verifier = generators.codeVerifier();
    return code_verifier;
}

export async function GetAuthCodeParams(req: IncomingMessage){
    const client = await GetClient();
    const params = client.callbackParams(req);
    return params;
}

export function SetCodeVerifierCookie(req: IncomingMessage, res: ServerResponse, code_verifier: string){
    const cookies = new Cookies(req, res, cookies_config);
    cookies.set(pkce_cookie_name, code_verifier, {
        httpOnly: true,
        sameSite: "lax",
        secure: true
    })
}

export function GetPKCECookie(req: IncomingMessage, res: ServerResponse){
    const cookies = new Cookies(req, res, cookies_config);
    return cookies.get(pkce_cookie_name);
}

export function RemovePKCECookie(req: IncomingMessage, res: ServerResponse){
    const cookies = new Cookies(req, res, cookies_config);
    cookies.set(pkce_cookie_name)
}

async function GetTokenSet(code_verifier: string, auth_code_params: CallbackParamsType){
    const client = await GetClient();
    const tokenSet = await client.callback(redirect_uri, auth_code_params, {code_verifier});
    return tokenSet;
}

export async function SetAccessToken(req: IncomingMessage, res: ServerResponse, code_verifier: string, auth_code_params: CallbackParamsType){
    const tokenSet = await GetTokenSet(code_verifier, auth_code_params);
    const access_token = tokenSet.access_token;
    const id_token = tokenSet.id_token;
    const cookies = new Cookies(req, res, cookies_config);
    cookies.set(auth_code_params_cookie_name, access_token, {
        httpOnly: true,
        sameSite: "lax",
        secure: true
    })
    cookies.set(auth_code_id_token_cookie_name, id_token, {
        httpOnly: true,
        sameSite: "lax",
        secure: true
    })
}

export async function RemoveAccessToken(req: IncomingMessage, res: ServerResponse){
    const cookies = new Cookies(req, res, cookies_config);
    cookies.set(auth_code_params_cookie_name);
    cookies.set(auth_code_id_token_cookie_name);
}

export async function GetUserInfo(req: IncomingMessage, res: ServerResponse){
    const cookies = new Cookies(req, res, cookies_config);
    const access_token = cookies.get(auth_code_params_cookie_name);

    if (access_token == null){
        return null;
    }

    const client = await GetClient();
    let claims = null;
    try {
        claims = await client.userinfo(access_token);
        console.log({claims})
    }catch(err){
        console.log(err);
        return null;
    }

    return claims;
}
