import { useEffect } from 'react';
import { GetLogoutRedirectUri
    , RemoveAccessToken
} from '../../helpers/auth/authHelper.js'

export default function Logout({redirectUrl}) {
    useEffect(() => {
        window.location = redirectUrl;
    }, [])

    return (
        <h1>Redirect to logout page...</h1>
    )
}

export async function getServerSideProps(context){
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

