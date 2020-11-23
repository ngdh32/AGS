async function GetAGSIdentityClient() {
    const { Issuer } = require('openid-client');
    const agsIdentityIssuer = await Issuer.discover('https://demo.identityserver.io');
    const client = new agsIdentityIssuer.Client({
        client_id: 'interactive.confidential',
        client_secret: 'secret',
        redirect_uris: ['http://localhost:3000/auth/callback'],
        response_types: ['code']
    })
    return client;
}

export default GetAGSIdentityClient;