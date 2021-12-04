import { api_url, groups_version } from '../../../config/identity'
import { GetBearerConfig } from '../../common/utilityHelper'
import { NextApiRequest, NextApiResponse } from 'next'
import AuthenticationHeader from '../../../models/common/authenticationHeader'
import AGSResponse from '../../../models/common/agsResponse'
import { GroupItemType } from '../../../models/identity/groupItemType'

export default class GroupsHelper {
    config:any
    authenticaionHeader: AuthenticationHeader

    constructor(req: NextApiRequest, res: NextApiResponse) {
        const authenticaionHeader = GetBearerConfig(req, res);
        this.config = {
            headers: authenticaionHeader
        }
    }

    async GetGroups() : Promise<AGSResponse>  {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${groups_version}/groups`;
            const response = await axios.get(url, this.config)
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }

    async CreateGroup(group: GroupItemType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${groups_version}/groups`;
            const response = await axios.post(url, group, this.config);
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }

    async UpdateGroup(group: GroupItemType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${groups_version}/groups`;
            const response = await axios.put(url, group, this.config);
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }
    
    async DeleteGroup(groupId: string) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${groups_version}/groups/${groupId}`;
            const response = await axios.delete(url, this.config)
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        console.log(result)
        return result;
    }
}