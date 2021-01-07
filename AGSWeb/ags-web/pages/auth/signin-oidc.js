import { 
    GetAuthCodeParams
    , GetPKCECookie
    , RemovePKCECookie
    , SetAccessToken
 } from '../../helpers/auth/authHelper.js'
import { useEffect } from 'react';

export default function callback({tokens}){
    useEffect(() => {

    }, []) 

    return (
        <h1>Signed In successfully</h1>
    ) 
}

export async function getServerSideProps(context){
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