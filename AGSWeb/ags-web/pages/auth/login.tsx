import { useEffect } from 'react';
import { GetRedirectUri
    , GenerateCodeVerifier
    , SetCodeVerifierCookie
} from '../../helpers/auth/authHelper'
import { LoginProps } from '../../models/pages/auth/loginProps';
import { GetServerSidePropsResult } from 'next'
import { IncomingMessage, ServerResponse } from 'http';

export default function Login({redirectUrl}: LoginProps) {
    useEffect(() => {
        window.location.href = redirectUrl;
    }, [])

    return (
        <h1>Redirect to login page...</h1>
    )
}

export async function getServerSideProps(context: { req: IncomingMessage; res: ServerResponse; }) : Promise<GetServerSidePropsResult<LoginProps>>{
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

