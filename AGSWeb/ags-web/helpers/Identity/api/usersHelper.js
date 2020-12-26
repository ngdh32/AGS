import { api_url, users_version } from '../../../config/identity.js'
import { GetBearerConfig } from '../../common/utilityHelper.js'

export default class UsersHelper {
    constructor(req, res) {
        this.config = GetBearerConfig(req, res);
    }

    async GetUsers() {
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${users_version}/users`;
            result = await axios.get(url, this.config)
        } catch (err) {
            console.log(err);
        }

        console.log(result.data)
        return result.data.data;
    }
}

