import { useEffect } from 'react';
import { GetRedirectUri
    , GenerateCodeVerifier
    , SetCodeVerifierCookie
} from '../../helpers/auth/authHelper.js'

export default function Login({redirectUrl}) {
    useEffect(() => {
        window.location = redirectUrl;
    }, [])

    return (
        <h1>Redirect to login page...</h1>
    )
}

export async function getServerSideProps(context){
    // generate code_verfier and store it into session cookie
    const code_verifier = GenerateCodeVerifier();
    const redirectUrl = await GetRedirectUri(code_verifier);

    SetCodeVerifierCookie(context.req, context.res, code_verifier);

    console.log(`Redirect Url: ${redirectUrl}`);

    return {
        props: {
            redirectUrl
        }
    }
}

