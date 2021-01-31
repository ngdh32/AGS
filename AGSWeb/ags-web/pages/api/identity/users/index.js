import UsersHelper from '../../../../helpers/identity/api/usersHelper.js'

// req = HTTP incoming message, res = HTTP server response
export default async function UsersHandler(req, res) {
    const usersHelper = new UsersHelper(req,res);
    const { method } = req;
    if (method == "POST"){
        console.log(req.body.user)
        const result = await usersHelper.CreateUser(req.body.user);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

    if (method == "PUT"){
        const result = await usersHelper.UpdateUser(req.body.user);
        res.setHeader('Content-Type', 'application/json');
        res.end(JSON.stringify(result));
        return ;
    }

}