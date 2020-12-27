import UsersHelper from '../../../helpers/Identity/api/usersHelper'
import { resposne_success } from  '../../../config/identity.js'

// req = HTTP incoming message, res = HTTP server response
export default async function UsersHandler(req, res) {
    const usersHelper = new UsersHelper(req,res);
    const { method } = req;
    if (method == "POST"){
        const result = await usersHelper.CreateUser(req.body.user);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}