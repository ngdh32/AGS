import { 
    client_id, 
    client_secret, 
    redirect_uri, 
    authentication_url, 
    scope,
    pkce_cookie_name, 
    auth_code_params_cookie_name
} from '../../config/auth.js'
import Cookies from 'cookies';

export async function GetClient(){
    const { Issuer } = require('openid-client');
    const agsIdentityIssuer = await Issuer.discover(authentication_url);
    const client = new agsIdentityIssuer.Client({
        client_id: client_id,
        client_secret: client_secret,
        redirect_uris: [redirect_uri],
        response_types: ['code']
    });

    return client;
}

export async function GetRedirectUri(code_verifier){
    const { generators } = require('openid-client');
    const client = await GetClient();
    const url = client.authorizationUrl({
        scope: scope,
        code_challenge: generators.codeChallenge(code_verifier),
        code_challenge_method: 'S256'
    });
    return url;
}

export function GenerateCodeVerifier(){
    const { generators } = require('openid-client');
    const code_verifier = generators.codeVerifier();
    return code_verifier;
}

export async function GetAuthCodeParams(req){
    const client = await GetClient();
    const params = client.callbackParams(req);
    return params;
}

export function SetCodeVerifierCookie(req, res, code_verifier){
    const cookies = new Cookies(req, res);
    cookies.set(pkce_cookie_name, code_verifier, {
        httpOnly: true,
        sameSite: "strict",
        secure: true
    })
}

export function GetPKCECookie(req, res){
    const cookies = new Cookies(req, res);
    return cookies.get(pkce_cookie_name);
}

export function RemovePKCECookie(req, res){
    const cookies = new Cookies(req, res);
    cookies.set(pkce_cookie_name)
}

async function GetTokenSet(code_verifier, auth_code_params){
    const client = await GetClient();
    const tokenSet = await client.callback(redirect_uri, auth_code_params, {code_verifier});
    return tokenSet;
}

export async function SetAccessToken(req, res, code_verifier, auth_code_params){
    const tokenSet = await GetTokenSet(code_verifier, auth_code_params);
    const access_token = tokenSet.access_token;
    const cookies = new Cookies(req, res);
    cookies.set(auth_code_params_cookie_name, access_token, {
        httpOnly: true,
        sameSite: "strict",
        secure: true
    })
}

export async function GetUserInfo(req, res){
    const cookies = new Cookies(req, res);
    const access_token = cookies.get(auth_code_params_cookie_name);

    if (access_token == null){
        return null;
    }

    const client = await GetClient();
    let claims = null;
    try {
        claims = await client.userinfo(access_token);
    }catch(err){
        console.log(err);
        return null;
    }

    return claims;
}
