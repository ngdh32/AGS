import { api_url, users_version } from '../../../config/identity'
import { GetBearerConfig } from '../../common/utilityHelper'
import { NextApiRequest, NextApiResponse } from 'next'
import AuthenticationHeader from '../../../models/common/authenticationHeader'
import AGSResponse from '../../../models/common/agsResponse'
import { UserItemType } from '../../../models/identity/userItemType'
import { ChangePasswordRequestType } from '../../../models/identity/changePasswordRequestType'


export default class UsersHelper {
    config:any
    authenticaionHeader: AuthenticationHeader

    constructor(req: NextApiRequest, res: NextApiResponse) {
        const authenticaionHeader = GetBearerConfig(req, res);
        this.config = {
            headers: authenticaionHeader
        }
    }

    async GetUsers() : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${users_version}/users`;
            const response = await axios.get(url, this.config)
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }

    async CreateUser(user: UserItemType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${users_version}/users`;
            const response = await axios.post(url, user, this.config);
            console.log({response})
            result.SetSuccessfulResponse(response);
        } catch (err) {
            console.log({err})
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }

    async UpdateUser(user: UserItemType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${users_version}/users`;
            const response = await axios.put(url, user, this.config);
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }
    
    async DeleteUser(userId: string) : Promise<AGSResponse>  {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${users_version}/users/${userId}`;
            const response = await axios.delete(url, this.config)
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }
 
    async ChangePassword(changePWRequest: ChangePasswordRequestType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${users_version}/users/changepw`;
            const response = await axios.post(url, changePWRequest, this.config);
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }
}

