import GetAGSIdentityClient from '../../services/AGSIdentity/AGSIdentityClient'
import Cookies from 'cookies';

export default function callback({claims}){
    return (
        <h1>{`${JSON.stringify(claims)}`}</h1>
    )
}

export async function getServerSideProps(context){
    // get back the code_verifier from session cookie
    const cookies = new Cookies(context.req, context.res);
    const code_verifier = cookies.get("pkce");
    console.log("callback code_verifier:" + code_verifier);
    // remove pkce 
    cookies.set("pkce");

    const client = await GetAGSIdentityClient();
    const params = client.callbackParams(context.req);
    const tokenSet = await client.callback('http://localhost:3000/auth/callback', params, {code_verifier});

    // save the authorization code to cookie
    const claims = tokenSet.claims();

    return {
        props: {
            claims
        }
    }

}