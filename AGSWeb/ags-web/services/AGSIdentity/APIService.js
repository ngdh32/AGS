import {
    api_url, 
    resposne_success
}from '../../config/AGSIdentity.js'


class AGSIdentity{
    constructor(access_token){
        this.access_token = access_token;
        this.authentication_header_config = {
            header: {
                Authorization: `Bearer ${access_token}` 
            }
        }
    }
    
    async GetAllUsers(){
        const axios = require('axios')
        const response = await axios.default.get(api_url + "/v1/users", this.authentication_header_config)
        if (response == resposne_success){
            console.log(response.Data);
        }
    }

    GetUserById(){

    }

}

export default AGSIdentity;