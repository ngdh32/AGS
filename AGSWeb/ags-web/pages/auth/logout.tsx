import { useEffect } from 'react';
import { GetLogoutRedirectUri
    , RemoveAccessToken
} from '../../helpers/auth/authHelper'
import { LogoutProps } from '../../models/pages/auth/logoutProps';
import { GetServerSidePropsResult } from 'next'
import { IncomingMessage, ServerResponse } from 'http';

export default function Logout({redirectUrl}: LogoutProps) {
    useEffect(() => {
        window.location.href = redirectUrl;
    }, [])

    return (
        <h1>Redirect to logout page...</h1>
    )
}

export async function getServerSideProps(context: { req: IncomingMessage; res: ServerResponse; }) : Promise<GetServerSidePropsResult<LogoutProps>>{
    // generate code_verfier and store it into session cookie
    const redirectUrl = await GetLogoutRedirectUri(context.req, context.res);

    RemoveAccessToken(context.req, context.res);

    console.log(`Redirect Url: ${redirectUrl}`);

    return {
        props: {
            redirectUrl
        }
    }
}

