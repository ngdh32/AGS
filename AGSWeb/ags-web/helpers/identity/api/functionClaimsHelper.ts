import { api_url, function_claims_version } from '../../../config/identity'
import { GetBearerConfig } from '../../common/utilityHelper'
import { NextApiRequest, NextApiResponse } from 'next'
import AuthenticationHeader from '../../../models/common/authenticationHeader'
import AGSResponse from '../../../models/common/agsResponse'
import { FunctionClaimItemType } from '../../../models/identity/functionClaimItemType'

export default class FunctionClaimsHelper {
    config:any
    authenticaionHeader: AuthenticationHeader

    constructor(req: NextApiRequest, res: NextApiResponse) {
        const authenticaionHeader = GetBearerConfig(req, res);
        this.config = {
            headers: authenticaionHeader
        }
    }

    async GetFunctionClaims() : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        console.log("Function Claim start")
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims`;
            const response = await axios.get(url, this.config)
            console.log("Function Claim Response")
            console.log({response})
            result.SetSuccessfulResponse(response);
        } catch (err) {
            console.log({err})
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }


    async CreateFunctionClaim(functionclaim: FunctionClaimItemType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims`;
            console.log({config: this.config})
            const response = await axios.post(url, functionclaim, this.config);
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }

    async UpdateFunctionClaim(functionclaim: FunctionClaimItemType) : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims`;
            const response = await axios.put(url, functionclaim, this.config);
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }
    
    async DeleteFunctionClaim(functionClaimId: string)  : Promise<AGSResponse> {
        const result = new AGSResponse();
        const axios = require('axios');
        try {
            const url = `${api_url}/v${function_claims_version}/functionclaims/${functionClaimId}`;
            const response = await axios.delete(url, this.config)
            result.SetSuccessfulResponse(response);
        } catch (err) {
            result.SetUnsuccessfulResponseWithError(err);
        }
        return result;
    }
}