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
            console.log({url})
            result = await axios.get(url, this.config)
        } catch (err) {
            console.log(err);
            result = response;
        }
    
        return result.data;
    }


    async CreateFunctionClaim(functionclaim){
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims`;
            result = await axios.post(url, functionclaim, this.config);
        } catch (err) {
            console.log(err);
            result = response;
        }

        return result.data;
    }

    async UpdateFunctionClaim(functionclaim){
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims`;
            result = await axios.put(url, functionclaim, this.config);
        } catch (err) {
            console.log(err);
            result = response;
        }

        return result.data;
    }
    
    async DeleteFunctionClaim(functionClaimId) {
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims/${functionClaimId}`;
            result = await axios.delete(url, this.config)
            
        } catch (err) {
            console.log(err);
            result = response;
        }

        return result.data;
    }
}