import { api_url, groups_version } from '../../../config/identity.js'
import { GetBearerConfig } from '../../common/utilityHelper.js'

export default class GroupsHelper {
    constructor(req, res) {
        this.config = GetBearerConfig(req, res);
    }

    async GetGroups() {
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${groups_version}/groups`;
            result = await axios.get(url, this.config)
        } catch (err) {
            console.log(err);
        }

        console.log(result.data)
        return result.data.data;
    }
}