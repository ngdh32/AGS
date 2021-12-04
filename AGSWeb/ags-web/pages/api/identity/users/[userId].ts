import UsersHelper from '../../../../helpers/identity/api/usersHelper'
import { NextApiRequest, NextApiResponse } from 'next';

// req = HTTP incoming message, res = HTTP server response
export default async function UsersHandler(req: NextApiRequest, res: NextApiResponse) {
    const usersHelper = new UsersHelper(req,res);
    const { method, query } = req;
    const { userId } = query

    if (method == "POST" && typeof userId === 'string' && userId.toLowerCase() == "changepw"){
        const result = await usersHelper.ChangePassword(req.body.changePWReuqest);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }
    
    if (method == "DELETE"  && typeof userId === 'string'){
        const result = await usersHelper.DeleteUser(userId);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}