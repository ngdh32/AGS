import { api_url, function_claims_version } from '../../../config/identity.js'
import { GetBearerConfig } from '../../common/utilityHelper.js'

export default class FunctionClaimsHelper {
    constructor(req, res) {
        this.config = GetBearerConfig(req, res);
    }

    async GetFunctionClaims() {
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims`;
            result = await axios.get(url, this.config)
        } catch (err) {
            return err.response.status;
        }
    
        return result.data;
    }
}