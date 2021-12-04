import { 
    GetAuthCodeParams
    , GetPKCECookie
    , RemovePKCECookie
    , SetAccessToken
 } from '../../helpers/auth/authHelper'
import { useEffect } from 'react';
import { GetServerSidePropsResult } from 'next'
import { IncomingMessage, ServerResponse } from 'http';
import { Redirect } from 'next/dist/lib/load-custom-routes';
import { SignInOidcProps } from '../../models/pages/auth/signInOidcProps';

export default function callback(){
    useEffect(() => {

    }, []) 

    return (
        <h1>Signed In successfully</h1>
    ) 
}

export async function getServerSideProps(context: { req: IncomingMessage; res: ServerResponse; }) : Promise<GetServerSidePropsResult<SignInOidcProps>>{
    console.log("Sign-in callback called")

    // get and remove pkce cookie
    const code_verifier = GetPKCECookie(context.req, context.res);
    // console.log({code_verifier})
    RemovePKCECookie(context.req, context.res);
    const auth_code_params = await GetAuthCodeParams(context.req);
    console.log({code_verifier, auth_code_params})
    await SetAccessToken(context.req, context.res, code_verifier, auth_code_params);

    return {
        redirect: {
          permanent: false,
          destination: '/'
        }
    }
}