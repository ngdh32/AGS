import { useEffect } from 'react';
import GetAGSIdentityClient from '../../services/AGSIdentity/AGSIdentityClient'
import Cookies from 'cookies';

export default function Login({redirectUrl}) {
    useEffect(() => {
        window.location = redirectUrl;
    }, [])

    return (
        <h1>Redirect to login page...</h1>
    )
}

export async function getServerSideProps(context){
    const { generators } = require('openid-client');
    // generate code_verfier and store it into session cookie
    const code_verifier = generators.codeVerifier();
    const cookies = new Cookies(context.req, context.res);
    cookies.set("pkce", code_verifier, {
        httpOnly: true
    })

    const code_challenge = generators.codeChallenge(code_verifier);
    const agsIdentityClient = await GetAGSIdentityClient();

    const redirectUrl = agsIdentityClient.authorizationUrl({
        scope: 'openid profile email api offline_access',
        code_challenge,
        code_challenge_method: 'S256'
    })

    console.log(`Redirect Url: ${redirectUrl}`);

    return {
        props: {
            redirectUrl
        }
    }
}