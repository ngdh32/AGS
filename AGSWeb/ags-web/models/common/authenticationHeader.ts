export default class AuthenticationHeader
{
    authorization: string

    constructor(accessToken: string)
    {
        this.authorization = `Bearer ${accessToken}`;
    }
}