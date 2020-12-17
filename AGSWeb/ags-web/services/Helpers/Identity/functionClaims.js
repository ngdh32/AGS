import Cookies from 'cookies'
import { auth_code_params_cookie_name } from '../../../config/Auth.js'
import { api_url, function_claims_version, resposne_success } from '../../../config/AGSIdentity.js'

function GetBearerConfig(req, res) {
    const cookies = new Cookies(req, res);
    const access_token = cookies.get(auth_code_params_cookie_name);

    return {
        headers: { Authorization: `Bearer ${access_token}` }
    }
}

export async function GetFunctionClaims(req, res) {
    const config = GetBearerConfig(req, res);
    const axios = require('axios');
    let result = null
    try {
        const url = `${api_url}/v${function_claims_version}/functionclaims`;
        result = await axios.get(url, config)
    } catch (err) {
        console.log(err);
    }

    return result.data.data;
}