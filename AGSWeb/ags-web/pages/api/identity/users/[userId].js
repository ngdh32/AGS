import UsersHelper from '../../../../helpers/Identity/api/usersHelper.js'

// req = HTTP incoming message, res = HTTP server response
export default async function UsersHandler(req, res) {
    const usersHelper = new UsersHelper(req,res);
    const { method, query } = req;
    const { userId } = query

    if (method == "POST" && userId.toLowerCase() == "changepw"){
        const result = await usersHelper.ChangePassword(req.body.changePWReuqest);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }
    
    if (method == "DELETE"){
        const result = await usersHelper.DeleteUser(userId);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}