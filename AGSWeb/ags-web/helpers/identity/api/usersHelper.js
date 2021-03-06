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
            return err;
        }

        return result.data;
    }

    async CreateUser(user){
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${users_version}/users`;
            result = await axios.post(url, user, this.config);
        } catch (err) {
            console.log(err);
            return err;
        }

        console.log(result.data)
        return result.data;
    }

    async UpdateUser(user){
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${users_version}/users`;
            result = await axios.put(url, user, this.config);
        } catch (err) {
            console.log(err);
            return err;
        }

        console.log(result.data)
        return result.data;
    }
    
    async DeleteUser(userId) {
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${users_version}/users/${userId}`;
            result = await axios.delete(url, this.config)
            
        } catch (err) {
            console.log(err);
            return err;
        }

        return result.data;
    }
 
    async ChangePassword(changePWRequest){
        const axios = require('axios');
        let result = null
        try {
            const url = `${api_url}/v${users_version}/users/changepw`;
            result = await axios.post(url, changePWRequest, this.config);
        } catch (err) {
            console.log("ChangePassword Error")
            console.log(err);
            return err;
        }

        console.log(result.data)
        return result.data;
    }
}

